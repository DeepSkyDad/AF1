;
; Script generated by the ASCOM Driver Installer Script Generator 6.2.0.0
; Generated by Pavle Gartner on 12/11/2017 (UTC)
;
[Setup]
;1. add signtool.exe to path (C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64)
;2. Tools -> Configure Sign Tools
;3. Add, enter "MsSign" for name
;4. Enter command "signtool.exe sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a $p" 
SignTool=MsSign $f
AppID={{10cc53ba-ec1f-4cca-8e95-62aa97be4f96}
AppName=ASCOM DSD AF1 v1.2.2
AppVerName=ASCOM DSD AF1 v1.2.2
AppVersion=1.2.2
AppPublisher=Pavle Gartner <pavle.gartner@gmail.com>
AppPublisherURL=mailto:pavle.gartner@gmail.com
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
VersionInfoVersion=1.2.2
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Focuser"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="ASCOM DSD AF1 Setup v1.2.2"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Installer Generator\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Installer Generator\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Focuser folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Focuser\DSD AF1"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Focuser\DSD AF1"
Name: "{app}\ASCOM.DeepSkyDad.AF1"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[Files]
Source: "D:\Projects\DeepSkyDad.AF1\DeepSkyDad.AF1.ASCOM\bin\Release\ASCOM.DeepSkyDad.AF1.Focuser.dll"; DestDir: "{app}\ASCOM.DeepSkyDad.AF1"
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "D:\Projects\DeepSkyDad.AF1\DeepSkyDad.AF1.ASCOM\ReadMe.htm"; DestDir: "{app}\ASCOM.DeepSkyDad.AF1"; Flags: isreadme
; TODO: Add other files needed by your driver here (add subfolders above)
Source: "D:\Projects\DeepSkyDad.AF1\DeepSkyDad.AF1.ASCOM\readme.jpg"; DestDir: "{app}\ASCOM.DeepSkyDad.AF1";

; Only if driver is .NET
[Run]
; Only for .NET assembly/in-proc drivers
Filename: "{dotnet4032}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.DeepSkyDad.AF1\ASCOM.DeepSkyDad.AF1.Focuser.dll"""; Flags: runhidden 32bit
Filename: "{dotnet4064}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.DeepSkyDad.AF1\ASCOM.DeepSkyDad.AF1.Focuser.dll"""; Flags: runhidden 64bit; Check: IsWin64




; Only if driver is .NET
[UninstallRun]
; Only for .NET assembly/in-proc drivers
Filename: "{dotnet4032}\regasm.exe"; Parameters: "-u ""{app}\ASCOM.DeepSkyDad.AF1\ASCOM.DeepSkyDad.AF1.Focuser.dll"""; Flags: runhidden 32bit
; This helps to give a clean uninstall
Filename: "{dotnet4064}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.DeepSkyDad.AF1\ASCOM.DeepSkyDad.AF1.Focuser.dll"""; Flags: runhidden 64bit; Check: IsWin64
Filename: "{dotnet4064}\regasm.exe"; Parameters: "-u ""{app}\ASCOM.DeepSkyDad.AF1\ASCOM.DeepSkyDad.AF1.Focuser.dll"""; Flags: runhidden 64bit; Check: IsWin64




[CODE]
//
// Before the installer UI appears, verify that the (prerequisite)
// ASCOM Platform 6.2 or greater is installed, including both Helper
// components. Utility is required for all types (COM and .NET)!
//
function InitializeSetup(): Boolean;
var
   U : Variant;
   H : Variant;
begin
   Result := FALSE;  // Assume failure
   // check that the DriverHelper and Utilities objects exist, report errors if they don't
   try
      H := CreateOLEObject('DriverHelper.Util');
   except
      MsgBox('The ASCOM DriverHelper object has failed to load, this indicates a serious problem with the ASCOM installation', mbInformation, MB_OK);
   end;
   try
      U := CreateOLEObject('ASCOM.Utilities.Util');
   except
      MsgBox('The ASCOM Utilities object has failed to load, this indicates that the ASCOM Platform has not been installed correctly', mbInformation, MB_OK);
   end;
   try
      if (U.IsMinimumRequiredVersion(6,2)) then	// this will work in all locales
         Result := TRUE;
   except
   end;
   if(not Result) then
      MsgBox('The ASCOM Platform 6.2 or greater is required for this driver.', mbInformation, MB_OK);
end;

// Code to enable the installer to uninstall previous versions of itself when a new version is installed
procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  UninstallExe: String;
  UninstallRegistry: String;
begin
  if (CurStep = ssInstall) then // Install step has started
	begin
      // Create the correct registry location name, which is based on the AppId
      UninstallRegistry := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#SetupSetting("AppId")}' + '_is1');
      // Check whether an extry exists
      if RegQueryStringValue(HKLM, UninstallRegistry, 'UninstallString', UninstallExe) then
        begin // Entry exists and previous version is installed so run its uninstaller quietly after informing the user
          MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
          Exec(RemoveQuotes(UninstallExe), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
          sleep(1000);    //Give enough time for the install screen to be repainted before continuing
        end
  end;
end;

