@echo off
@setlocal

set start=%time%

title UniVR Packages Manager
echo Welcome to the UniVR Packages Manager!
echo This script will download and install the packages needed for UniVR.
echo /!\ You are about to download and import packages /!\
echo Please make sure you have an internet connection.
echo Press any key to continue...
pause >nul

title Downloading packages - UniVR Packages Manager
echo Downloading packages...
curl http://esieacraft.fr/packages/imported_packages.zip -o imported_packages.zip
echo Downloaded packages successfully.

title Extracting packages - UniVR Packages Manager
echo Extracting packages...
tar -xvf imported_packages.zip
echo Extracted packages successfully.

title Importing - UniVR Packages Manager
echo Importing packages...
xcopy /s /y .\Packages\* .\
echo Imported packages successfully.

title Deleting zip file - UniVR Packages Manager
echo Deleting zip file...
del imported_packages.zip
echo Deleted zip file successfully.

title Deleting Cache - UniVR Packages Manager
echo Deleting Cache...
rmdir /s /q .\Packages
echo Deleted Cache successfully.

set end=%time%
set options="tokens=1-4 delims=:.,"
for /f %options% %%a in ("%start%") do set start_h=%%a&set /a start_m=100%%b %% 100&set /a start_s=100%%c %% 100&set /a start_ms=100%%d %% 100
for /f %options% %%a in ("%end%") do set end_h=%%a&set /a end_m=100%%b %% 100&set /a end_s=100%%c %% 100&set /a end_ms=100%%d %% 100

set /a hours=%end_h%-%start_h%
set /a mins=%end_m%-%start_m%
set /a secs=%end_s%-%start_s%
set /a ms=%end_ms%-%start_ms%
if %ms% lss 0 set /a secs = %secs% - 1 & set /a ms = 100%ms%
if %secs% lss 0 set /a mins = %mins% - 1 & set /a secs = 60%secs%
if %mins% lss 0 set /a hours = %hours% - 1 & set /a mins = 60%mins%
if %hours% lss 0 set /a hours = 24%hours%
if 1%ms% lss 100 set ms=0%ms%

:: Mission accomplished
set /a totalsecs = %hours%*3600 + %mins%*60 + %secs%

title Done - UniVR Packages Manager
echo Packages installed successfully in %hours%:%mins%:%secs%.%ms% (%totalsecs%.%ms%s total)
echo Press any key to exit...
pause >nul

exit


