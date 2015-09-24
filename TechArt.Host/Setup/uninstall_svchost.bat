@ECHO OFF
TITLE Uninstall TServiceHost 

@Set service_port=37000
@Set service_name=TechArt Service Host(37000)

rem ===== Service exe name ======
@SET service_exe=TechArt.Host.exe
@SET path=%windir%\system32\inetsrv\;%windir%\;%windir%\system32\;%windir%\Microsoft.NET\Framework\v4.0.30319\;
@SET Loc=%~dp0
@SET ServiceFile="%Loc%\..\%service_exe%"

rem create file path in temp folder , if runas administrator
set TempFile_Name=%SystemRoot%\System32\BatTestUACin_SysRt%Random%.batemp
rem write in temp folder
( echo "BAT Test UAC in Temp" >%TempFile_Name% ) 1>nul 2>nul
if exist %TempFile_Name% (
  goto RunAsAdmin
) else (
  echo Please right click to run as Administrator.
  goto end
)

:RunAsAdmin
@ECHO.
@ECHO.
if not "%1" == "exe" (
@echo -----  Uninstall Service -------
installutil /SN="%service_name%" /u %ServiceFile%
) 

@ECHO .
@ECHO If you don't see any error message .. that means Install Certificate Success.
:end
del %TempFile_Name% 1>nul 2>nul
IF not "%1" == "exe" (
pause
)