/*
  Deep Sky Dad AF1 firmware, 25.11.2018
  
  EEPROM IMPLEMENTATION
    Autofocuser state is stored in EEPROM long[] array, using EEPROMEx library.
    Each index contains different property, with last one containing checksum (sum of all previous values, so we can validate its contents).
    Additionally, values are saved to a different address every time. Writing to same address every time would wear EEPROM out faster.
    Autofocuseer state:
    {<position>, <maxPosition>, <maxMovement>, <stepMode>, <isAlwaysOn>, <settleBufferMs>, <checksum>}

  COMMAND SET
    Commands are executed via serial COM port communication. 
    Each command starts with [ and ends with ]
    Sample of parameterless command: [MOVE]
    Sample of command with parameters: [STRG5000]
    Each response starts with ( and ends with )
    If command results in error, response starts with ! and ends with ), containing error code. List of error codes:
      100 - command not found
      101 - invalid target position
      102 - invalid step mode
    The actual set of required commands is based on ASCOM IFocuserV3 interface, for more check:
    https://ascom-standards.org/Help/Platform/html/T_ASCOM_DeviceInterface_IFocuserV3.htm
    
  
*/

#include <Arduino.h>
#include <EEPROMEx.h>

#define EEPROM_AF_STATE_POSITION 0
#define EEPROM_AF_STATE_MAX_POSITION 1
#define EEPROM_AF_STATE_MAX_MOVEMENT 2
#define EEPROM_AF_STATE_STEP_MODE 3
#define EEPROM_AF_STATE_IS_ALWAYS_ON 4
#define EEPROM_AF_STATE_SETTLE_BUFFER_MS 5
#define EEPROM_AF_STATE_CHECKSUM 6

//{<position>, <maxPosition>, <maxMovement>, <stepMode>, <isAlwaysOn>, <settleBufferMs>, <checksum>}
long _eepromAfState[] = {0, 0, 0, 0, 0, 0, 9999};
long _eepromAfStateDefault[] = {50000, 100000, 5000, 2, 0, 0, 0};
int _eepromAfStatePropertyCount = sizeof(_eepromAfState) / sizeof(long);
int _eepromAfStateAddressSize = sizeof(_eepromAfState);
int _eepromAfStateAdressesCount = EEPROMSizeATmega328 / _eepromAfStateAddressSize;
int _eepromAfStateCurrentAddress;
bool _eepromSaveAfState;

/* MP6500 pins */
#define MP6500_PIN_ENABLE 9
#define MP6500_PIN_DIR 2
#define MP6500_PIN_STEP 3
//current limiting pin. To set specific current, change PWM to specific voltage.
//equation: CURRENT=2.2−0.63*MP6500_PIN_I1
#define MP6500_PIN_I1 6
#define MP6500_PIN_I1_ALWAYS_ON 170
#define MP6500_PIN_I1_MOVING 155
// MS1/MS2 sets stepping mode 00 = F, 10 = 1/2, 01 = 1/4, 11 = 1/8
// steps per revolution = 200, 400, 800, 1600

#define MP6500_PIN_MS2 7
#define MP6500_PIN_MS1 8
#define MP6500_PIN_SLP 4

/* MOTOR */
bool _motorIsMoving;
long _motorTargetPosition;
long _motorSettleBufferPrevMs;

/* Serial communication */
char _serialCommandRaw[20];
int _serialCommandRawIdx;
char _command[20];
char _commandParam[20];

const char firmwareName[] = "DeepSkyDad.AF1";
const char firmwareVersion[] = "1.0.0";
const char firmwareSubversion[] = "1.0.1";

/* EEPROM functions */
bool eepromValidateChecksum()
{
  long checksum = 0;
  for (int i = 0; i < _eepromAfStatePropertyCount - 1; i++)
  {
    checksum += _eepromAfState[i];
  }

  return checksum == _eepromAfState[_eepromAfStatePropertyCount - 1];
}

void eepromGetAddress()
{
  _eepromAfStateCurrentAddress = EEPROM.getAddress(sizeof(_eepromAfState));
  if (_eepromAfStateCurrentAddress + sizeof(_eepromAfState) > EEPROMSizeATmega328)
  {
    EEPROM.setMemPool(0, EEPROMSizeATmega328);
    _eepromAfStateCurrentAddress = EEPROM.getAddress(sizeof(_eepromAfState));
  }
}

void eepromWrite()
{
  //invalidate previous state
  if (eepromValidateChecksum())
  {
    _eepromAfState[EEPROM_AF_STATE_CHECKSUM] = _eepromAfState[EEPROM_AF_STATE_CHECKSUM] - 1;
  }
  EEPROM.writeBlock<long>(_eepromAfStateCurrentAddress, _eepromAfState, _eepromAfStatePropertyCount);

  //write new state
  long checksum = 0;
  for (int i = 0; i < _eepromAfStatePropertyCount - 1; i++)
  {
    checksum += _eepromAfState[i];
  }
  _eepromAfState[EEPROM_AF_STATE_CHECKSUM] = checksum;

  //write to new address
  eepromGetAddress();

  /*Serial.print("Memory address: ");
  Serial.println(_eepromAfStateCurrentAddress);
  Serial.print("Position: ");
  Serial.println(_eepromAfState[EEPROM_AF_STATE_POSITION]);
  Serial.print("Always on: ");
  Serial.println(_eepromAfState[EEPROM_AF_STATE_IS_ALWAYS_ON]);
  Serial.print("Max movement: ");
  Serial.println(_eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT]);
  Serial.print("Max position: ");
  Serial.println(_eepromAfState[EEPROM_AF_STATE_MAX_POSITION]);
  Serial.print("Settle buffer ms: ");
  Serial.println(_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS]);
  Serial.print("Step mode: ");
  Serial.println(_eepromAfState[EEPROM_AF_STATE_STEP_MODE]);*/

  EEPROM.writeBlock<long>(_eepromAfStateCurrentAddress, _eepromAfState, _eepromAfStatePropertyCount);
}

void eepromResetState()
{
  for (int i = 0; i < _eepromAfStatePropertyCount; i++)
  {
    _eepromAfState[i] = _eepromAfStateDefault[i];
  }
  eepromWrite();
}

bool eepromRead()
{
  eepromGetAddress();
  EEPROM.readBlock<long>(_eepromAfStateCurrentAddress, _eepromAfState, _eepromAfStatePropertyCount);
  return eepromValidateChecksum();
}

void setAlwaysOn()
{
  if (_eepromAfState[EEPROM_AF_STATE_IS_ALWAYS_ON] == 1)
  {
    digitalWrite(MP6500_PIN_SLP, HIGH);
    analogWrite(MP6500_PIN_I1, MP6500_PIN_I1_ALWAYS_ON);
  }
  else
  {
    digitalWrite(MP6500_PIN_SLP, LOW);
  }
}

void stopMotor()
{
  setAlwaysOn();

  _motorTargetPosition = _eepromAfState[EEPROM_AF_STATE_POSITION];
  _eepromSaveAfState = true;

  if (_motorIsMoving && _eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS] > 0)
    _motorSettleBufferPrevMs = millis();

  _motorIsMoving = false;
}

void changeStepMode(int stepMode)
{
  if (stepMode == 1)
  {
    digitalWrite(MP6500_PIN_MS1, 0);
    digitalWrite(MP6500_PIN_MS2, 0);
  }
  else if (stepMode == 2)
  {
    digitalWrite(MP6500_PIN_MS1, 1);
    digitalWrite(MP6500_PIN_MS2, 0);
  }
  else if (stepMode == 4)
  {
    digitalWrite(MP6500_PIN_MS1, 0);
    digitalWrite(MP6500_PIN_MS2, 1);
  }
  else if (stepMode == 8)
  {
    digitalWrite(MP6500_PIN_MS1, 1);
    digitalWrite(MP6500_PIN_MS2, 1);
  }
  else
  {
    digitalWrite(MP6500_PIN_MS1, 1);
    digitalWrite(MP6500_PIN_MS2, 0);
    stepMode = 2;
  }

  _eepromAfState[EEPROM_AF_STATE_STEP_MODE] = stepMode;
  _eepromSaveAfState = true;
}

void printResponse(int response)
{
  Serial.print("(");
  Serial.print(response);
  Serial.print(")");
}

void printResponse(long response)
{
  Serial.print("(");
  Serial.print(response);
  Serial.print(")");
}

void printResponse(char response[])
{
  Serial.print("(");
  Serial.print(response);
  Serial.print(")");
}

void printSuccess()
{
  Serial.print("(OK)");
}

void printResponseErrorCode(int code)
{
  Serial.print("!");
  Serial.print(code);
  Serial.print(")");
}

void executeCommand()
{
  if (strcmp("GFRM", _command) == 0)
  {
    Serial.print("(");
    Serial.print("Board=");
    Serial.print(firmwareName);
    Serial.print(", Version=");
    Serial.print(firmwareVersion);
    Serial.print(")");
  }
  else if (strcmp("GSFR", _command) == 0)
  {
    Serial.print("(");
    Serial.print(firmwareSubversion);
    Serial.print(")");
  }
  else if (strcmp("GPOS", _command) == 0)
  {
    printResponse(_eepromAfState[EEPROM_AF_STATE_POSITION]);
  }
  else if (strcmp("GTRG", _command) == 0)
  {
    printResponse(_motorTargetPosition);
  }
  else if (strcmp("STRG", _command) == 0)
  {
    long pos = strtol(_commandParam, NULL, 10);
    if (abs(_eepromAfState[EEPROM_AF_STATE_POSITION] - pos) > _eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT] || pos < 0)
    {
      printResponseErrorCode(101);
    }
    else
    {
      _motorTargetPosition = pos;
      _eepromSaveAfState = true;
      printSuccess();
    }
  }
  else if (strcmp("GMOV", _command) == 0)
  {
    if (_motorIsMoving)
    {
      printResponse(1);
    }
    else
    {
      /*
        if your focuser has any play, this can affect the autofocuser performance. SGP for example goes aways from current position and
        than starts traversing back. When it changes focus direction (traverse back), focuser play can cause FOV to wiggle just a bit,
        which causes enlongated stars on the next exposure. Settle buffer option returns IsMoving as TRUE after focuser reaches target position,
        letting it to settle a bit. Advices by Jared Wellman of SGP.
			*/
      if (_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS] > 0L && _motorSettleBufferPrevMs != 0L)
      {
        long settleBufferCurrenMs = millis();
        if ((settleBufferCurrenMs - _motorSettleBufferPrevMs) < _eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS])
        {
          printResponse(1);
        }
        else
        {
          printResponse(0);
          _motorSettleBufferPrevMs = 0L;
        }
      }
      else
      {
        printResponse(0);
      }
    }
  }
  else if (strcmp("SMOV", _command) == 0)
  {
    _motorIsMoving = true;
  }
  else if (strcmp("STOP", _command) == 0)
  {
    stopMotor();
  }
  else if (strcmp("GMXP", _command) == 0)
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_MAX_POSITION]);
  }
  else if (strcmp("GMXS", _command) == 0)
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT]);
  }
  else if (strcmp("GSTP", _command) == 0)
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_STEP_MODE]);
  }
  else if (strcmp("SSTP", _command) == 0)
  {
    long sm = strtol(_commandParam, NULL, 10);
    if (sm != 1 && sm != 2 && sm != 4 && sm != 8 && sm != 16)
    {
      printResponseErrorCode(102);
    }
    else
    {
      changeStepMode(sm);
      _eepromSaveAfState = true;
      printSuccess();
    }
  }
  else if (strcmp("RSET", _command) == 0)
  {
    eepromResetState();
    printSuccess();
  }
  else if (strcmp("GAON", _command) == 0)
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_IS_ALWAYS_ON]);
  }
  else if (strcmp("SAON", _command) == 0)
  {
    _eepromAfState[EEPROM_AF_STATE_IS_ALWAYS_ON] = strtol(_commandParam, NULL, 10);
    _eepromSaveAfState = true;
    setAlwaysOn();
    printSuccess();
  }
  else if (strcmp("GBUF", _command) == 0)
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS]);
  }
  else if (strcmp("SBUF", _command) == 0)
  {
    _eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS] = strtol(_commandParam, NULL, 10);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("WEPR", _command) == 0)
  {
    _eepromSaveAfState = true;
    printSuccess();
  }
  else
  {
    printResponseErrorCode(100);
  }
}

void setup()
{
  Serial.begin(9600);
  while (!Serial)
  {
    ; // wait for serial port to connect. Needed for native USB port only
  }

  EEPROM.setMemPool(0, EEPROMSizeATmega328);

  bool afStateFound = false;
  for (int i = 0; i < _eepromAfStateAdressesCount; i++)
  {
    if (eepromRead())
    {
      afStateFound = true;
      break;
    }
  }

  if (!afStateFound)
  {
    eepromResetState();
  }

  pinMode(MP6500_PIN_DIR, OUTPUT);
  pinMode(MP6500_PIN_STEP, OUTPUT);
  pinMode(MP6500_PIN_MS1, OUTPUT);
  pinMode(MP6500_PIN_MS2, OUTPUT);
  pinMode(MP6500_PIN_I1, OUTPUT);
  pinMode(MP6500_PIN_SLP, OUTPUT);
  
  digitalWrite(MP6500_PIN_DIR, 0);
  digitalWrite(MP6500_PIN_STEP, 0);
  digitalWrite(MP6500_PIN_ENABLE, LOW);
  analogWrite(MP6500_PIN_I1, 255);
  changeStepMode(_eepromAfState[EEPROM_AF_STATE_STEP_MODE]);

  setAlwaysOn();

  _motorTargetPosition = _eepromAfState[EEPROM_AF_STATE_POSITION];
  _motorSettleBufferPrevMs = 0L;

  Serial.print("(READY)");
}

void loop()
{
  if (_motorIsMoving)
  {
    digitalWrite(MP6500_PIN_SLP, HIGH);
    analogWrite(MP6500_PIN_I1, MP6500_PIN_I1_MOVING);

    if (_motorTargetPosition < _eepromAfState[EEPROM_AF_STATE_POSITION])
    {
      digitalWrite(MP6500_PIN_DIR, LOW);
      digitalWrite(MP6500_PIN_STEP, 1);
      delayMicroseconds(1);
      digitalWrite(MP6500_PIN_STEP, 0);
      _eepromAfState[EEPROM_AF_STATE_POSITION]--;
      delay(2);
    }
    else if (_motorTargetPosition > _eepromAfState[EEPROM_AF_STATE_POSITION])
    {
      digitalWrite(MP6500_PIN_DIR, HIGH);
      digitalWrite(MP6500_PIN_STEP, 1);
      delayMicroseconds(1);
      digitalWrite(MP6500_PIN_STEP, 0);
      _eepromAfState[EEPROM_AF_STATE_POSITION]++;
      delay(2);
    }
    else
    {
      stopMotor();
    }
  }
  else
  {
    if (_eepromSaveAfState)
    {
      eepromWrite();
      _eepromSaveAfState = false;
    }
  }
}

void serialEvent()
{
  while (Serial.available())
  {
    char c = Serial.read();

    if (c == '[')
    {
      _serialCommandRawIdx = 0;
      memset(_serialCommandRaw, 0, 20);
    }
    else if (c == ']')
    {
     
      int len = strlen(_serialCommandRaw);
      memset(_command, 0, 20);
      memset(_commandParam, 0, 20);
      
      if (len >= 4)
      {
        strncpy(_command, _serialCommandRaw, 4);
      }
      if (len > 4)
      {
        strncpy(_commandParam, _serialCommandRaw + 4, len - 4);
      }

      executeCommand();
      break;
    }
    else
    {
      _serialCommandRaw[_serialCommandRawIdx] = c;
      _serialCommandRawIdx++;
    }
  }
}