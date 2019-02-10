//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for DeepSkyDad
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Focuser

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Threading;

namespace ASCOM.DeepSkyDad.AF1
{
    //
    // Your driver's DeviceID is ASCOM.DeepSkyDad.AF1.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.DeepSkyDad.AF1.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _DeepSkyDad from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// DSD AF1
    /// </summary>
    [Guid("10cc53ba-ec1f-4cca-8e95-62aa97be4f96")]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class Focuser : IFocuserV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.DeepSkyDad.AF1.Focuser";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Deep Sky Dad AF1";

        private static string firmwareVersion = "Board=DeepSkyDad.AF1, Version=1.1.0";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM10";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        internal static string stepSizeProfileName = "Step size";
        internal static string stepSizeDefault = "1/2";
        internal static string maxPositionProfileName = "Maximum positions";
        internal static string maxPositionDefault = "100000";
        internal static string maxMovementProfileName = "Maximum movement";
        internal static string maxMovementDefault = "5000";
        internal static string resetOnConnectProfileName = "Reset on connect";
        internal static string resetOnConnectDefault = "false";
        internal static string setPositonOnConnectProfileName = "Set position on connect";
        internal static string setPositonOnConnectDefault = "false";
        internal static string setPositonOnConnectValueProfileName = "Set position on connect value";
        internal static string setPositonOnConnectValueDefault = "50000";
        internal static string coilsModeProfileName = "Coils mode";
        internal static string coilsModeDefault = "Always on";
        internal static string idleCoilsTimeoutProfileName = "Idle - coils timeout (ms)";
        internal static string idleCoilsTimeoutDefault = "60000";
        internal static string settleBufferProfileName = "Settle buffer";
        internal static string settleBufferDefault = "0";
        internal static string reverseDirectionProfileName = "Reverse direction";
        internal static string reverseDirectionDefault = "false";
        internal static string currentMoveProfileName = "Current - move";
        internal static string currentMoveDefault = "75%";
        internal static string currentHoldProfileName = "Current - hold";
        internal static string currentHoldDefault = "50%";

        internal static string comPort; // Variables to hold the currrent device configuration
        internal static int maxPosition;
        internal static int maxMovement;
        internal static string stepSize;
        internal static bool traceState;
        internal static bool resetOnConnect;
        internal static bool setPositonOnConnect;
        internal static int setPositionOnConnectValue;
        internal static string coilsMode;
        internal static int idleCoilsTimeout;
        internal static int settleBuffer;
        internal static bool reverseDirection;
        internal static string currentMove;
        internal static string currentHold;
        internal static int commandTimeout = 2000;

        internal static int? maxIncrement = null;
        internal static int? maxStep = null;

        private Serial serial;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal static TraceLogger tl;
        internal static TraceLogger tls;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeepSkyDad"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser()
        {
            tl = new TraceLogger("", "DeepSkyDad");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl = new TraceLogger("", "DeepSkyDad.AF1");
            tl.Enabled = traceState;

            tl.LogMessage("Telescope", "Starting initialisation");

            tls = new TraceLogger("", "DeepSkyDad.AF1.Serial");
            tls.Enabled = traceState;

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            //TODO: Implement your additional construction here

            tl.LogMessage("Focuser", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE IFocuserV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(this))
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                F.Text = $"{driverDescription}, v{fvi.FileVersion.Substring(0, fvi.FileVersion.LastIndexOf('.'))}"; 
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
            // DO NOT have both these sections!  One or the other
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw, false);
            return ret == "1";
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public void Disconnect()
        {
            connectedState = false;
            LogMessage("Connected Set", "Disconnecting from port {0}", comPort);
            serial.Connected = false;
            serial.Dispose();
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    LogMessage("Connected Set", "Connecting to port {0}", comPort);
                    serial = new Serial();
                    serial.Speed = SerialSpeed.ps9600;
                    serial.PortName = comPort;
                    serial.DTREnable = false;
                    //serial.Handshake = SerialHandshake.None;
                    serial.Connected = true;
                    serial.ReceiveTimeoutMs = commandTimeout;
                    //serial.ReceiveTerminated("(READY)"); //wait for ready signal - this is needed because arduino auto-resets when serial connection is established to it (it can be prevented by putting capacitor between 5v and reset but not neccessary for autofocuser)

                    string actualFirmwareVersion = string.Empty;
                    try
                    {
                        actualFirmwareVersion = CommandString("GFRM");
                    }
                    catch(Exception)
                    {
                        actualFirmwareVersion = CommandString("GFRM");
                    }

                    if (actualFirmwareVersion != firmwareVersion)
                        throw new ASCOM.DriverException($"Invalid firmware version - required: {firmwareVersion}, installed {actualFirmwareVersion}");

                    if (resetOnConnect)
                    {
                        //deselect the flag
                        using (Profile driverProfile = new Profile())
                        {
                            resetOnConnect = false;
                            driverProfile.DeviceType = "Focuser";
                            driverProfile.WriteValue(driverID, resetOnConnectProfileName, resetOnConnect.ToString());
                        }
                            
                        CommandString("RSET");
                    }

                    if(setPositonOnConnect)
                    {
                        //deselect the flag
                        using (Profile driverProfile = new Profile())
                        {
                            setPositonOnConnect = false;
                            driverProfile.DeviceType = "Focuser";
                            driverProfile.WriteValue(driverID, setPositonOnConnectProfileName, setPositonOnConnect.ToString());
                        }

                        CommandString($"SPOS{setPositionOnConnectValue}");
                        setPositionOnConnectValue = maxPosition / 2;
                    }
                    
                    focuserPosition = (int)CommandLong("GPOS");

                    var ss = 2;
                    if (stepSize == "1")
                    {
                        ss = 1;
                    }
                    else if (stepSize == "1/2")
                    {
                        ss = 2;
                    }
                    else if (stepSize == "1/4")
                    {
                        ss = 4;
                    }
                    else if (stepSize == "1/8")
                    {
                        ss = 8;
                    }

                    var coilsModeInt = 1;
                    switch(coilsMode)
                    {
                        case "Idle - off":
                            coilsModeInt = 0;
                            break;
                        case "Always on":
                            coilsModeInt = 1;
                            break;
                        case "Idle - coils timeout (ms)":
                            coilsModeInt = 2;
                            break;
                    }

                    CommandString($"CONF{ss}|{coilsModeInt}|{(reverseDirection ? 1 : 0)}|{maxPosition}|{maxMovement}|{settleBuffer}|{idleCoilsTimeout}|180000|{currentMove}|{currentHold}");
                    //CommandString($"SREV{(reverseDirection ? 1 : 0)}");
                    //CommandString($"SMXP{maxPosition}");
                    //CommandString($"SMXM{maxMovement}");
                    //CommandString($"SBUF{settleBuffer}");
                    //CommandString($"SSTP{ss}");
                }
                else
                {
                    CommandString($"SCLM0");
                    Thread.Sleep(100);

                    Disconnect();
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "ASCOM DSD AF1 driver. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "1");
                return Convert.ToInt16("1");
            }
        }

        public string Name
        {
            get
            {
                string name = "ASCOM.DSD.AF";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation

        private int focuserPosition = -1; // Class level variable to hold the current focuser position

        public bool Absolute
        {
            get
            {
                tl.LogMessage("Absolute Get", true.ToString());
                return true;
            }
        }

        public void Halt()
        {
            tl.LogMessage("Halt", string.Empty);
            CommandBlind("STOP");
        }

        public bool IsMoving
        {
            get
            {
                var isMoving = CommandBool("GMOV");
                tl.LogMessage("IsMoving Get", isMoving.ToString());
                return isMoving;
            }
        }

        public bool Link
        {
            get
            {
                tl.LogMessage("Link Get", this.Connected.ToString());
                return this.Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
            set
            {
                tl.LogMessage("Link Set", value.ToString());
                this.Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
        }

        public int MaxIncrement
        {
            get
            {
                if(maxIncrement == null)
                {
                    maxIncrement = (int)CommandLong("GMXM");
                }
               
                tl.LogMessage("MaxIncrement Get", maxIncrement.ToString());
                return (int)maxIncrement; // Maximum change in one move
            }
        }

        public int MaxStep
        {
            get
            {
                if(maxStep == null)
                {
                    maxStep = (int?)CommandLong("GMXP");
                }
                
                tl.LogMessage("MaxStep Get", maxStep.ToString());
                return (int)maxStep; // Maximum extent of the focuser, so position range is 0 to 10,000
            }
        }

        public void Move(int position)
        {
            CommandString($"STRG{position.ToString()}");
            CommandBlind("SMOV");
            tl.LogMessage("Move", position.ToString());
            focuserPosition = position; // Set the focuser position
        }

        public int Position
        {
            get
            {
                if (focuserPosition == -1)
                    focuserPosition = (int)CommandLong("GPOS");
                return focuserPosition; // Return the focuser position
            }
        }

        public double StepSize
        {
            get
            {
                tl.LogMessage("StepSize Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("StepSize", false);
            }
        }

        public bool TempComp
        {
            get
            {
                tl.LogMessage("TempComp Get", false.ToString());
                return false;
            }
            set
            {
                tl.LogMessage("TempComp Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("TempComp", false);
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                tl.LogMessage("TempCompAvailable Get", false.ToString());
                return false; // Temperature compensation is not available in this driver
            }
        }

        public double Temperature
        {
            get
            {
                tl.LogMessage("Temperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Temperature", false);
            }
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Focuser";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
                maxPosition = Convert.ToInt32(driverProfile.GetValue(driverID, maxPositionProfileName, string.Empty, maxPositionDefault));
                maxMovement = Convert.ToInt32(driverProfile.GetValue(driverID, maxMovementProfileName, string.Empty, maxMovementDefault));
                stepSize = driverProfile.GetValue(driverID, stepSizeProfileName, string.Empty, stepSizeDefault);
                resetOnConnect = Convert.ToBoolean(driverProfile.GetValue(driverID, resetOnConnectProfileName, string.Empty, resetOnConnectDefault));
                setPositonOnConnect = Convert.ToBoolean(driverProfile.GetValue(driverID, setPositonOnConnectProfileName, string.Empty, setPositonOnConnectDefault));
                setPositionOnConnectValue = Convert.ToInt32(driverProfile.GetValue(driverID, setPositonOnConnectValueProfileName, string.Empty, setPositonOnConnectValueDefault)); ;
                coilsMode = driverProfile.GetValue(driverID, coilsModeProfileName, string.Empty, coilsModeDefault);
                idleCoilsTimeout = Convert.ToInt32(driverProfile.GetValue(driverID, idleCoilsTimeoutProfileName, string.Empty, idleCoilsTimeoutDefault));
                reverseDirection = Convert.ToBoolean(driverProfile.GetValue(driverID, reverseDirectionProfileName, string.Empty, reverseDirectionDefault));
                settleBuffer = Convert.ToInt32(driverProfile.GetValue(driverID, settleBufferProfileName, string.Empty, settleBufferDefault));
                currentMove = driverProfile.GetValue(driverID, currentMoveProfileName, string.Empty, currentMoveDefault);
                currentHold = driverProfile.GetValue(driverID, currentHoldProfileName, string.Empty, currentHoldDefault);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
                driverProfile.WriteValue(driverID, stepSizeProfileName, stepSize);
                driverProfile.WriteValue(driverID, maxPositionProfileName, maxPosition.ToString());
                driverProfile.WriteValue(driverID, maxMovementProfileName, maxMovement.ToString());
                driverProfile.WriteValue(driverID, resetOnConnectProfileName, resetOnConnect.ToString());
                driverProfile.WriteValue(driverID, setPositonOnConnectProfileName, setPositonOnConnect.ToString());
                driverProfile.WriteValue(driverID, setPositonOnConnectValueProfileName, setPositionOnConnectValue.ToString());
                driverProfile.WriteValue(driverID, coilsModeProfileName, coilsMode.ToString());
                driverProfile.WriteValue(driverID, idleCoilsTimeoutProfileName, idleCoilsTimeout.ToString());
                driverProfile.WriteValue(driverID, settleBufferProfileName, settleBuffer.ToString());
                driverProfile.WriteValue(driverID, reverseDirectionProfileName, reverseDirection.ToString());
                driverProfile.WriteValue(driverID, currentMoveProfileName, currentMove);
                driverProfile.WriteValue(driverID, currentHoldProfileName, currentHold);
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
