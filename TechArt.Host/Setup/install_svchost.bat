@ECHO OFF
TITLE Install Service Host

@Set service_port=37000
@Set service_name=TechArt Service Host(37000)
@Set service_desc=TechArt Service Host

rem ===== Certificate Thumbprint =======
@SET certhash=1e16c9f4c5758bbd27d9fc8fc5a0da70836585d2
rem ===== Certificate password ======
@SET certPassword=910204
rem ===== Service exe name ======
@SET service_exe=TechArt.Host.exe
@SET path=%windir%\system32\inetsrv\;%windir%\;%windir%\system32\;%windir%\Microsoft.NET\Framework\v4.0.30319\;
	

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


@ECHO  -----  Add Root CA : TechArt-------
certutil.exe -addstore root "%~dp0TechArt.cer"

rem pfx file
@SET Loc=%~dp0
@SET certDir=%Loc%TechArt.pfx
@SET ServiceFile="%Loc%\..\%service_exe%"

rem key Container-localhost certificate
@SET appid={d58f7917-f637-4609-99c3-69005d8c4c0b}
rem TFS position

@ECHO.
@ECHO.
@ECHO  -----  Install SSLCert : TechArt-------

rem import key
certutil  -p "%certPassword%" -importpfx "%certDir%"

netsh http add sslcert ipport=0.0.0.0:%service_port% certstorename=MY certhash=%certhash% appid=%appid%

IF not "%1" == "exe" (
@ECHO  ----- Install Service -------
installutil /SN="%service_name%" /SD="%service_desc%" %ServiceFile%
)

sc start "%service_name%"
@ECHO .
@ECHO If you don't see any error message .. that means Install Certificate Success.

:end
del %TempFile_Name% 1>nul 2>nul
IF not "%1" == "exe" (
pause
)