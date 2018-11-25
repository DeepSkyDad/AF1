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
long _eepromAfStateDefault[] = {5000, 100000, 5000, 2, 0, 0, 0};
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
#define MP6500_PIN_I1_ALWAYS_ON 160
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
String _commandRaw;
String _command;
String _commandParam;

const String programName = "DeepSkyDad.AF1";
const String programVersion = "1.0.0";

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

void printResponse(String response)
{
  Serial.print("(");
  Serial.print(response);
  Serial.print(")");
}

void printSuccess()
{
  Serial.print("(OK)");
}

void printResponseErrorCode(String code)
{
  Serial.print("!");
  Serial.print(code);
  Serial.print(")");
}

void executeCommand()
{
  if (_command == "GFRM")
  {
    printResponse("Board=" + programName + ", Version=" + programVersion);
  }
  else if (_command == "GPOS")
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_POSITION]);
  }
  else if (_command == "GTRG")
  {
    printResponse((int)_motorTargetPosition);
  }
  else if (_command == "STRG")
  {
    long pos = (long)_commandParam.toInt();
    if (abs(_eepromAfState[EEPROM_AF_STATE_POSITION] - pos) > _eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT] || pos < 0)
    {
      printResponseErrorCode("101");
    }
    else
    {
      _motorTargetPosition = pos;
      _eepromSaveAfState = true;
      printSuccess();
    }
  }
  else if (_command == "GMOV")
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
  else if (_command == "SMOV")
  {
    _motorIsMoving = true;
  }
  else if (_command == "STOP")
  {
    stopMotor();
  }
  else if (_command == "GMXP")
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_MAX_POSITION]);
  }
  else if (_command == "GMXS")
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT]);
  }
  else if (_command == "GSTP")
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_STEP_MODE]);
  }
  else if (_command == "SSTP")
  {
    long sm = _commandParam.toInt();
    if (sm != 1 && sm != 2 && sm != 4 && sm != 8 && sm != 16)
    {
      printResponseErrorCode("102");
    }
    else
    {
      changeStepMode(sm);
      _eepromSaveAfState = true;
      printSuccess();
    }
  }
  else if (_command == "RSET")
  {
    eepromResetState();
    printSuccess();
  }
  else if (_command == "GAON")
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_IS_ALWAYS_ON]);
  }
  else if (_command == "SAON")
  {
    _eepromAfState[EEPROM_AF_STATE_IS_ALWAYS_ON] = _commandParam.toInt();
    _eepromSaveAfState = true;
    setAlwaysOn();
    printSuccess();
  }
  else if (_command == "GBUF")
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS]);
  }
  else if (_command == "SBUF")
  {
    _eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS] = _commandParam.toInt();
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (_command == "WEPR")
  {
    _eepromSaveAfState = true;
    printSuccess();
  }
  else
  {
    printResponseErrorCode("100");
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
      _commandRaw = "";
    }
    else if (c == ']')
    {
      int len = _commandRaw.length();
      if (len >= 4)
      {
        _command = _commandRaw.substring(0, 4);
      }
      if (len > 4)
      {
        _commandParam = _commandRaw.substring(4, len);
      }

      _commandRaw = "";

      executeCommand();
      break;
    }
    else
    {
      _commandRaw += c;
    }
  }
}