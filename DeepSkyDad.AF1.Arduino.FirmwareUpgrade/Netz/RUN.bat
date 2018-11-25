netz -s -z -so ../bin/Release/DeepSkyDad.AF1.Arduino.FirmwareUpgrade.exe ../bin/Release/CliWrap.dll ../bin/Release/Syroot.Windows.IO.KnownFolders.dll
copy "../bin/Release/AvrDude" "../bin/Release/DeepSkyDad.AF1.Arduino.FirmwareUpgrade.exe.netz/AvrDude"
cd "../bin/Release/DeepSkyDad.AF1.Arduino.FirmwareUpgrade.exe.netz"
del "FirmwareUpgrade.exe"
rename "DeepSkyDad.AF1.Arduino.FirmwareUpgrade.exe" "FirmwareUpgrade.exe"
