/*
  Deep Sky Dad AF1 firmware
  
  EEPROM IMPLEMENTATION
    Autofocuser state is stored in EEPROM long[] array, using EEPROMEx library.
    Each index contains different property, with last one containing checksum (sum of all previous values, so we can validate its contents).
    Additionally, values are saved to a different address every time. Writing to same address every time would wear EEPROM out faster.
    Autofocuseer state:
    {<position>, <maxPosition>, <maxMovement>, <stepMode>, <speedMode>, <coilsMode>, <settleBufferMs>, <idleCoilsTimeoutMs>, <idleEepromWriteMs>, <reverseDirection>, <currentMove>, <currentHold>, <checksum>}

  COMMAND SET
    Commands are executed via serial COM port communication. 
    Each command starts with [ and ends with ]
    Sample of parameterless command: [MOVE]
    Sample of command with parameters: [STRG5000]
    Each response starts with ( and ends with )
    If command results in error, response starts with ! and ends with ), containing error code. List of error codes:
      100 - command not found
      101 - relative movement bigger from max. movement
    The actual set of required commands is based on ASCOM IFocuserV3 interface, for more check:
    https://ascom-standards.org/Help/Platform/html/T_ASCOM_DeviceInterface_IFocuserV3.htm

    version 1.0.0 - 25.11.2018

    version 1.1.0 - 10.2.2019
    -code optimized to reduce EEPROM wear
    -added reverse direction option
    -added maximum position setting
    -added maximum movement setting
    -added current control (for movement and holding)
    -revised coils control (always on, idle of or idle after a period of inactivity)
    -reduced motor vibration
    -optimised speed

    version 1.2.0 - 14.3.2019 
    -added speed selection option

    version 1.2.1 - 3.4.2019
    -motor noise fix (the PWM frequency of pin 6, which controls the current was changed from audible 1kHz to inaudible 64kHz
    Details: the upgrade required moving the timing functions from Timer 0 to Timer 2, as the pin 6 is bound to the Timer 0. 
    Without this, changing the frequency would throw off timing functions. 
*/

#include <Arduino.h>
#include <EEPROMEx.h>
#include "wiring.h"

#define EEPROM_AF_STATE_POSITION 0
#define EEPROM_AF_STATE_MAX_POSITION 1
#define EEPROM_AF_STATE_MAX_MOVEMENT 2
#define EEPROM_AF_STATE_STEP_MODE 3
#define EEPROM_AF_STATE_SPEED_MODE 4
#define EEPROM_AF_STATE_COILS_MODE 5
#define EEPROM_AF_STATE_SETTLE_BUFFER_MS 6
#define EEPROM_AF_STATE_IDLE_COILS_TIMEOUT_MS 7
#define EEPROM_AF_STATE_IDLE_EEPROM_WRITE_MS 8
#define EEPROM_AF_STATE_REVERSE_DIRECTION 9
#define EEPROM_AF_STATE_CURRENT_MOVE 10
#define EEPROM_AF_STATE_CURRENT_HOLD 11
#define EEPROM_AF_STATE_CHECKSUM 12

//{<position>, <maxPosition>, <maxMovement>, <stepMode>, <speedMode>, <coilsMode>, <settleBufferMs>, <idleCoilsTimeoutMs>, <idleEepromWriteMs>, <reverseDirection>, <currentMove>, <currentHold>, <checksum>}
long _eepromAfState[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9999};
long _eepromAfPrevState[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9999};
long _eepromAfStateDefault[] = {50000, 100000, 5000, 2, 3, 1, 0, 60000, 180000, 0, 140, 180, 0};
int _eepromAfStatePropertyCount = sizeof(_eepromAfState) / sizeof(long);
int _eepromAfStateAddressSize = sizeof(_eepromAfState);
int _eepromAfStateAdressesCount = EEPROMSizeATmega328 / _eepromAfStateAddressSize;
int _eepromAfStateCurrentAddress;
bool _eepromSaveAfState;

/* MP6500 pins */
#define MP6500_PIN_ENABLE 9
#define MP6500_PIN_DIR 2
#define MP6500_PIN_STEP 3
//motorDir limiting pin. To set specific motorDir, change PWM to specific voltage.
//equation: CURRENT=2.2−0.63*MP6500_PIN_I1
#define MP6500_PIN_I1 6
#define MP6500_PIN_I1_MOVE_MIN 100
#define MP6500_PIN_I1_MOVE_MAX 180
long MP6500_PIN_I1_MOVE_RANGE = MP6500_PIN_I1_MOVE_MAX - MP6500_PIN_I1_MOVE_MIN;
#define MP6500_PIN_I1_HOLD_MIN 160
#define MP6500_PIN_I1_HOLD_MAX 200
long MP6500_PIN_I1_HOLD_RANGE = MP6500_PIN_I1_HOLD_MAX - MP6500_PIN_I1_HOLD_MIN;
// MS1/MS2 sets stepping mode 00 = F, 10 = 1/2, 01 = 1/4, 11 = 1/8

#define MP6500_PIN_MS2 7
#define MP6500_PIN_MS1 8
#define MP6500_PIN_SLP 4

/* MOTOR */
bool _motorIsMoving;
unsigned long _motorTargetPosition;
unsigned long _motorSettleBufferPrevMs;
unsigned long _motorIsMovingLastRunMs;
unsigned long _motorLastMoveCoilsMs;
unsigned long _motorLastMoveEepromMs;
long _motorSpeedFactor;

/* Serial communication */
char _serialCommandRaw[70];
int _serialCommandRawIdx;
int _serialCommandRawLength;
char _command[5];
char _commandParam[65];
int _commandParamLength;

const char firmwareName[] = "DeepSkyDad.AF1";
const char firmwareVersion[] = "1.2.1";

int main( void )
{
  init();

  //https://forum.arduino.cc/index.php?topic=430814.0
  //initVariant();

  #if defined(USBCON)
    USBDevice.attach();
  #endif
	
	setup();
    
	for (;;) {
		loop();
    if (serialEventRun) serialEventRun();
	}
        
	return 0;
}

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

void eepromWrite(bool forceWrite)
{
  _eepromSaveAfState = false;
  _motorLastMoveEepromMs = 0L;

  //prevent unneccessary saves
  if (!forceWrite)
  {
    bool stateChanged = false;
    for (int i = 0; i < _eepromAfStatePropertyCount - 1; i++)
    {
      if (_eepromAfState[i] != _eepromAfPrevState[i])
      {
        stateChanged = true;
        break;
      }
    }

    if (!stateChanged)
    {
      return;
    }
  }

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
    _eepromAfPrevState[i] = _eepromAfState[i];
    checksum += _eepromAfState[i];
  }
  _eepromAfState[EEPROM_AF_STATE_CHECKSUM] = checksum;
  _eepromAfPrevState[EEPROM_AF_STATE_CHECKSUM] = checksum;

  //write to new address
  eepromGetAddress();

  EEPROM.writeBlock<long>(_eepromAfStateCurrentAddress, _eepromAfState, _eepromAfStatePropertyCount);

  //Serial.print("EEPROM WRITE");
}

void eepromResetState()
{
  for (int i = 0; i < _eepromAfStatePropertyCount; i++)
  {
    _eepromAfState[i] = _eepromAfStateDefault[i];
    _eepromAfPrevState[i] = _eepromAfStateDefault[i];
  }
  eepromWrite(true);
}

bool eepromRead()
{
  eepromGetAddress();
  EEPROM.readBlock<long>(_eepromAfStateCurrentAddress, _eepromAfState, _eepromAfStatePropertyCount);
  EEPROM.readBlock<long>(_eepromAfStateCurrentAddress, _eepromAfPrevState, _eepromAfStatePropertyCount);
  return eepromValidateChecksum();
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

void printResponse(unsigned long response)
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

void writeCoilsMode()
{
  if (_motorIsMoving)
    return;

  //Idle - off
  if (_eepromAfState[EEPROM_AF_STATE_COILS_MODE] == 0)
  {
    digitalWrite(MP6500_PIN_SLP, LOW);
  }
  //Always on
  else if (_eepromAfState[EEPROM_AF_STATE_COILS_MODE] == 1)
  {
    digitalWrite(MP6500_PIN_SLP, HIGH);
    analogWrite(MP6500_PIN_I1, _eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD]);
  }
  //Idle - coils timeout (ms)
  else if (_eepromAfState[EEPROM_AF_STATE_COILS_MODE] == 2)
  {
    if (_motorLastMoveCoilsMs != 0L)
    {
      if ((millis() - _motorLastMoveCoilsMs) > (unsigned long)_eepromAfState[EEPROM_AF_STATE_IDLE_COILS_TIMEOUT_MS])
      {
        digitalWrite(MP6500_PIN_SLP, LOW);
        _motorLastMoveCoilsMs = 0L;
      }
      else
      {
        digitalWrite(MP6500_PIN_SLP, HIGH);
        analogWrite(MP6500_PIN_I1, _eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD]);
      }
    }
    else
    {
      digitalWrite(MP6500_PIN_SLP, LOW);
    }
  }
}

void setCoilsMode(char param[])
{
  long coilsMode = strtol(param, NULL, 10);
  if (coilsMode != 0 && coilsMode != 1 && coilsMode != 2)
    return;

  _eepromAfState[EEPROM_AF_STATE_COILS_MODE] = coilsMode;
  writeCoilsMode();
}

void stopMotor()
{
  if (_motorIsMoving && _eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS] > 0)
    _motorSettleBufferPrevMs = millis();
  _motorLastMoveCoilsMs = millis();
  _motorLastMoveEepromMs = millis();

  _motorIsMoving = false;
  _motorTargetPosition = _eepromAfState[EEPROM_AF_STATE_POSITION];
  analogWrite(MP6500_PIN_I1, _eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD]);
}

void writeStepMode(int stepMode)
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
}

void setStepMode(char param[])
{
  long sm = strtol(param, NULL, 10);
  if (sm != 1 && sm != 2 && sm != 4 && sm != 8 && sm != 16)
  {
    return;
  }
  else
  {
    writeStepMode(sm);
  }
}

void setSpeedMode(char param[])
{
  long speedMode = strtol(param, NULL, 10);
  if (speedMode != 1 && speedMode != 2 && speedMode != 3)
  {
    return;
  }
  else
  {
     _eepromAfState[EEPROM_AF_STATE_SPEED_MODE] = speedMode;
  }
}

void setReverseDir(char param[])
{
  long motorDir = strtol(param, NULL, 10);
  if (motorDir != 0 && motorDir != 1)
    return;

  _eepromAfState[EEPROM_AF_STATE_REVERSE_DIRECTION] = motorDir;
}

void setMaxPos(char param[])
{
  long maxPos = strtol(param, NULL, 10);
  if (maxPos < 10000)
    maxPos = 10000;

  if (_motorTargetPosition > (unsigned long)maxPos)
    _motorTargetPosition = maxPos;

  if (_eepromAfState[EEPROM_AF_STATE_POSITION] > maxPos)
    _eepromAfState[EEPROM_AF_STATE_POSITION] = maxPos;

  _eepromAfState[EEPROM_AF_STATE_MAX_POSITION] = maxPos;
}

void setMaxMovement(char param[])
{
  long maxMov = strtol(param, NULL, 10);
  if (maxMov < 1000)
    maxMov = 1000;

  _eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT] = maxMov;
}

void setSettleBuffer(char param[])
{
  long settleBufferMs = strtol(param, NULL, 10);
  if (settleBufferMs < 0)
  {
    settleBufferMs = 0;
  }

  _eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS] = settleBufferMs;
}

void setIdleCoilsTimeoutMs(char param[])
{
  long ms = strtol(param, NULL, 10);
  if (ms < 0)
  {
    ms = 0;
  }

  _eepromAfState[EEPROM_AF_STATE_IDLE_COILS_TIMEOUT_MS] = ms;
}

void setIdleEepromWriteMs(char param[])
{
  long ms = strtol(param, NULL, 10);
  if (ms < 0)
  {
    ms = 0;
  }

  _eepromAfState[EEPROM_AF_STATE_IDLE_EEPROM_WRITE_MS] = ms;
}

void setCurrentMove(char param[])
{
  long current = strtol(param, NULL, 10);
  if (current > MP6500_PIN_I1_MOVE_MAX)
    current = MP6500_PIN_I1_MOVE_MAX;
  if (current < MP6500_PIN_I1_MOVE_MIN)
    current = MP6500_PIN_I1_MOVE_MIN;

  _eepromAfState[EEPROM_AF_STATE_CURRENT_MOVE] = current;
}

void setCurrentMovePercent(char param[])
{
  int length = strlen(param) - 1;
  char _percentValue[length];
  strncpy(_percentValue, param, length);
  float percent = atof(_percentValue) / 100.0;

  long current = MP6500_PIN_I1_MOVE_MAX - MP6500_PIN_I1_MOVE_RANGE * percent;
  _eepromAfState[EEPROM_AF_STATE_CURRENT_MOVE] = current;
}

void setCurrentHoldPercent(char param[])
{
  int length = strlen(param) - 1;
  char _percentValue[length];
  strncpy(_percentValue, param, length);
  float percent = atof(_percentValue) / 100.0;

  long current = MP6500_PIN_I1_HOLD_MAX - MP6500_PIN_I1_HOLD_RANGE * percent;
  _eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD] = current;
  writeCoilsMode();
}

void setCurrentHold(char param[])
{
  long current = strtol(param, NULL, 10);
  if (current > MP6500_PIN_I1_HOLD_MAX)
    current = MP6500_PIN_I1_HOLD_MAX;
  if (current < MP6500_PIN_I1_HOLD_MIN)
    current = MP6500_PIN_I1_HOLD_MIN;

  _eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD] = current;
  writeCoilsMode();
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
    if (abs(_eepromAfState[EEPROM_AF_STATE_POSITION] - pos) > _eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT])
    {
      printResponseErrorCode(101);
    }
    else if (pos < 0)
    {
      pos = 0;
    }
    else if (pos > _eepromAfState[EEPROM_AF_STATE_MAX_POSITION])
    {
      pos = _eepromAfState[EEPROM_AF_STATE_MAX_POSITION];
    }
    else
    {
      if (_motorTargetPosition == (unsigned long)pos)
      {
        printSuccess();
        return;
      }

      _motorTargetPosition = pos;
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
        if your focuser has any play, this can affect the autofocuser performance. SGP for example goes aways from motorDir position and
        than starts traversing back. When it changes focus direction (traverse back), focuser play can cause FOV to wiggle just a bit,
        which causes enlongated stars on the next exposure. Settle buffer option returns IsMoving as TRUE after focuser reaches target position,
        letting it to settle a bit. Advices by Jared Wellman of SGP.
			*/
      if (_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS] > 0L && _motorSettleBufferPrevMs != 0L)
      {
        long settleBufferCurrenMs = millis();
        if ((settleBufferCurrenMs - _motorSettleBufferPrevMs) < (unsigned long)_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS])
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
    digitalWrite(MP6500_PIN_SLP, HIGH);
    analogWrite(MP6500_PIN_I1, _eepromAfState[EEPROM_AF_STATE_CURRENT_MOVE]);
    /*
      When waking up from sleep mode,
      approximately 1ms of time must pass before a
      STEP command can be issued to allow the
      internal circuitry to stabilize.
    */
    delayMicroseconds(1000);
    _motorIsMovingLastRunMs = millis();

    switch (_eepromAfState[EEPROM_AF_STATE_SPEED_MODE])
    {
    case 1:
        _motorSpeedFactor = 30;
        break;
    case 2:
        _motorSpeedFactor = 12;
        break;
    case 3:
        _motorSpeedFactor = 1;
        break;
    default:
        _motorSpeedFactor = 1;
        break;
    }
  }
  else if (strcmp("STOP", _command) == 0)
  {
    stopMotor();
    writeCoilsMode();
  }
  else if (strcmp("GMXP", _command) == 0)
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_MAX_POSITION]);
  }
  else if (strcmp("SMXP", _command) == 0)
  {
    setMaxPos(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GMXM", _command) == 0)
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT]);
  }
  else if (strcmp("SMXM", _command) == 0)
  {
    setMaxMovement(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GSTP", _command) == 0)
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_STEP_MODE]);
  }
  else if (strcmp("SSTP", _command) == 0)
  {
    setStepMode(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GSPD", _command) == 0)
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_SPEED_MODE]);
  }
  else if (strcmp("SSPD", _command) == 0)
  {
    setSpeedMode(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("RSET", _command) == 0)
  {
    eepromResetState();
    printSuccess();
  }
  else if (strcmp("RBOT", _command) == 0)
  {
    asm volatile("  jmp 0");
  }
  else if (strcmp("GCLM", _command) == 0)
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_COILS_MODE]);
  }
  else if (strcmp("SCLM", _command) == 0)
  {
    setCoilsMode(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GBUF", _command) == 0)
  {
    printResponse((int)_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS]);
  }
  else if (strcmp("SBUF", _command) == 0)
  {
    setSettleBuffer(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("WEPR", _command) == 0)
  {
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("SREV", _command) == 0)
  {
    setReverseDir(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("SPOS", _command) == 0)
  {
    long newPosition = strtol(_commandParam, NULL, 10);
    if (newPosition < 0)
      newPosition = 0;
    else if (newPosition > _eepromAfState[EEPROM_AF_STATE_MAX_POSITION])
    {
      newPosition = _eepromAfState[EEPROM_AF_STATE_MAX_POSITION];
    }

    if (_eepromAfState[EEPROM_AF_STATE_POSITION] == newPosition)
    {
      printSuccess();
      return;
    }

    _eepromAfState[EEPROM_AF_STATE_POSITION] = newPosition;
    _motorTargetPosition = newPosition;
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GIDC", _command) == 0)
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_IDLE_COILS_TIMEOUT_MS]);
  }
  else if (strcmp("SIDC", _command) == 0)
  {
    setIdleCoilsTimeoutMs(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GIDE", _command) == 0)
  {
    printResponse((long)_eepromAfState[EEPROM_AF_STATE_IDLE_EEPROM_WRITE_MS]);
  }
  else if (strcmp("SIDE", _command) == 0)
  {
    setIdleEepromWriteMs(_commandParam);
    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GCMV", _command) == 0)
  {
    if (_commandParam[0] == '%')
    {
      int percent = ((float)MP6500_PIN_I1_MOVE_MAX - (float)_eepromAfState[EEPROM_AF_STATE_CURRENT_MOVE]) / (float)MP6500_PIN_I1_MOVE_RANGE * 100;
      char buffer[4];
      sprintf(buffer, "%d%%", percent);
      printResponse(buffer);
    }
    else
    {
      printResponse((int)_eepromAfState[EEPROM_AF_STATE_CURRENT_MOVE]);
    }
  }
  else if (strcmp("SCMV", _command) == 0)
  {
    if (_commandParam[_commandParamLength - 1] == '%')
    {
      setCurrentMovePercent(_commandParam);
    }
    else
    {
      setCurrentMove(_commandParam);
    }

    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("GCHD", _command) == 0)
  {
    if (_commandParam[0] == '%')
    {
      int percent = ((float)MP6500_PIN_I1_HOLD_MAX - (float)_eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD]) / (float)MP6500_PIN_I1_HOLD_RANGE * 100;
      char buffer[4];
      sprintf(buffer, "%d%%", percent);
      printResponse(buffer);
    }
    else
    {
      printResponse((int)_eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD]);
    }
  }
  else if (strcmp("SCHD", _command) == 0)
  {
    if (_commandParam[_commandParamLength - 1] == '%')
    {
      setCurrentHoldPercent(_commandParam);
    }
    else
    {
      setCurrentHold(_commandParam);
    }

    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("SPWF", _command) == 0) {
   
    long divisor = strtol(_commandParam, NULL, 10);
    setPwmFrequency(6, divisor);
    printSuccess();
  }
  else if (strcmp("CONF", _command) == 0)
  {
    //COMMAND FORMAT: "CONF{stepMode}|{coilsMode}|{reverseDirection}|{maxPosition}|{maxMovement}|{settleBuffer}|{idleCoilsTimeoutMs}|{idleEepromWriteMs}|{currentMove}|{currentHold}";
    char *param = strtok(_commandParam, "|");
    int idx = 0;
    while (param != 0)
    {
      switch (idx)
      {
      case 0:
        setStepMode(param);
        break;
      case 1:
        setCoilsMode(param);
        break;
      case 2:
        setReverseDir(param);
        break;
      case 3:
        setMaxPos(param);
        break;
      case 4:
        setMaxMovement(param);
        break;
      case 5:
        setSettleBuffer(param);
        break;
      case 6:
        setIdleCoilsTimeoutMs(param);
        break;
      case 7:
        setIdleEepromWriteMs(param);
        break;
      case 8:
        setCurrentMovePercent(param);
        break;
      case 9:
        setCurrentHoldPercent(param);
        break;
      default:
        break;
      }

      idx++;
      param = strtok(0, "|");
    }

    _eepromSaveAfState = true;
    printSuccess();
  }
  else if (strcmp("DEBG", _command) == 0)
  {
    Serial.print("Memory address: ");
    Serial.println(_eepromAfStateCurrentAddress);
    Serial.print("Position: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_POSITION]);
    Serial.print("Coils mode: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_COILS_MODE]);
    Serial.print("Max movement: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_MAX_MOVEMENT]);
    Serial.print("Max position: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_MAX_POSITION]);
    Serial.print("Settle buffer ms: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_SETTLE_BUFFER_MS]);
    Serial.print("Step mode: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_STEP_MODE]);
    Serial.print("Reverse direction: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_REVERSE_DIRECTION]);
    Serial.print("Idle coils timeout (ms): ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_IDLE_COILS_TIMEOUT_MS]);
    Serial.print("Idle eeprom write (ms): ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_IDLE_EEPROM_WRITE_MS]);
    Serial.print("Move current: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_CURRENT_MOVE]);
    Serial.print("Hold current: ");
    Serial.println(_eepromAfState[EEPROM_AF_STATE_CURRENT_HOLD]);
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
  writeStepMode(_eepromAfState[EEPROM_AF_STATE_STEP_MODE]);
  writeCoilsMode();

  _motorTargetPosition = _eepromAfState[EEPROM_AF_STATE_POSITION];
  _motorSettleBufferPrevMs = 0L;
  _motorLastMoveCoilsMs = 0L;
  _motorLastMoveEepromMs = 0L;
}
unsigned long test = millis();
void loop()
{ 
  if (_motorIsMoving)
  {
    //give priority to motor with dedicated 300ms loops (effectivly pausing main loop, including serial event processing)
    while (millis() - _motorIsMovingLastRunMs < 300)
    {
      if (_motorTargetPosition < (unsigned long)_eepromAfState[EEPROM_AF_STATE_POSITION])
      {
        digitalWrite(MP6500_PIN_DIR, _eepromAfState[EEPROM_AF_STATE_REVERSE_DIRECTION] == 0 ? LOW : HIGH);
        digitalWrite(MP6500_PIN_STEP, 1);
        delayMicroseconds(1);
        digitalWrite(MP6500_PIN_STEP, 0);
        _eepromAfState[EEPROM_AF_STATE_POSITION]--;
        delayMicroseconds(1600 / _eepromAfState[EEPROM_AF_STATE_STEP_MODE] * _motorSpeedFactor);
      }
      else if (_motorTargetPosition > (unsigned long)_eepromAfState[EEPROM_AF_STATE_POSITION])
      {
        digitalWrite(MP6500_PIN_DIR, _eepromAfState[EEPROM_AF_STATE_REVERSE_DIRECTION] == 0 ? HIGH : LOW);
        digitalWrite(MP6500_PIN_STEP, 1);
        delayMicroseconds(1);
        digitalWrite(MP6500_PIN_STEP, 0);
        _eepromAfState[EEPROM_AF_STATE_POSITION]++;
        delayMicroseconds(1600 / _eepromAfState[EEPROM_AF_STATE_STEP_MODE] * _motorSpeedFactor);
      }
      else
      {
        stopMotor();
        writeCoilsMode();
      }
    }

    _motorIsMovingLastRunMs = millis();
  }
  else
  {
    if (_motorLastMoveCoilsMs != 0L && _eepromAfState[EEPROM_AF_STATE_COILS_MODE] == 2 && (millis() - _motorLastMoveCoilsMs) > (unsigned long)_eepromAfState[EEPROM_AF_STATE_IDLE_COILS_TIMEOUT_MS])
    {
      writeCoilsMode();
    }

    //save eeprom only after period of time from last movement (prevent EEPROM wear with write after each move)
    if (_motorLastMoveEepromMs != 0L && (millis() - _motorLastMoveEepromMs) > (unsigned long)_eepromAfState[EEPROM_AF_STATE_IDLE_EEPROM_WRITE_MS])
    {
      _eepromSaveAfState = true;
    }

    if (_eepromSaveAfState)
    {
      eepromWrite(false);
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
      memset(_serialCommandRaw, 0, 70);
    }
    else if (c == ']')
    {

      _serialCommandRawLength = strlen(_serialCommandRaw);
      _commandParamLength = 0;
      memset(_command, 0, 5);
      memset(_commandParam, 0, 65);

      if (_serialCommandRawLength >= 4)
      {
        strncpy(_command, _serialCommandRaw, 4);
      }
      if (_serialCommandRawLength > 4)
      {
        _commandParamLength = _serialCommandRawLength - 4;
        strncpy(_commandParam, _serialCommandRaw + 4, _commandParamLength);
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
