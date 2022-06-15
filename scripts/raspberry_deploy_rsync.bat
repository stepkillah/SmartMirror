
REM *****************************************************************
REM
REM CWRSYNC.CMD - Batch file template to start your rsync command (s).
REM
REM *****************************************************************

REM Make environment variable changes local to this batch file
SETLOCAL

REM Specify where to find rsync and related files
REM Default value is the directory of this batch file
SET CWRSYNCHOME=%1

SET SSHKeyFilePath=%2
SET SSHKeyDriveLetter=%SSHKeyFilePath:~0,1%
SET SSHKeyFilePath=%SSHKeyFilePath:~3%
SET SSHKeyFilePath=%SSHKeyFilePath:\=/%
SET SSHKeyFilePath=/cygdrive/%SSHKeyDriveLetter%/%SSHKeyFilePath%

SET SourceFilePath=%3
SET SourceFileDriveLetter=%SourceFilePath:~0,1%
SET SourceFilePath=%SourceFilePath:~3%
SET SourceFilePath=%SourceFilePath:\=/%
SET SourceFilePath=/cygdrive/%SourceFileDriveLetter%/%SourceFilePath%

SET DestinationFilePath=%4

REM Create a home directory for .ssh 
IF NOT EXIST %CWRSYNCHOME%\home\%USERNAME%\.ssh MKDIR %CWRSYNCHOME%\home\%USERNAME%\.ssh

REM Make cwRsync home as a part of system PATH to find required DLLs
SET CWOLDPATH=%PATH%
SET PATH=%CWRSYNCHOME%\bin;%PATH%

rsync -rvuzp -e 'ssh -i %SSHKeyFilePath%' -P --stats %SourceFilePath% %DestinationFilePath%
pause