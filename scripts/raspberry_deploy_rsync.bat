
REM *****************************************************************
REM
REM CWRSYNC.CMD - Batch file template to start your rsync command (s).
REM
REM *****************************************************************

REM Make environment variable changes local to this batch file
SETLOCAL

REM Specify where to find rsync and related files
REM Default value is the directory of this batch file
REM E:\raspbery\cwrsync
SET CWRSYNCHOME=%1

REM E:/ssh/***REMOVED***_key_ssh
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

REM ***REMOVED***@***REMOVED***08:/home/***REMOVED***/Projects/SmartMirror

rsync -rvuzp -e 'ssh -i %SSHKeyFilePath%' -P --stats %SourceFilePath% %DestinationFilePath%
pause 
REM Windows paths may contain a colon (:) as a part of drive designation and 
REM backslashes (example c:\, g:\). However, in rsync syntax, a colon in a 
REM path means searching for a remote host. Solution: use absolute path 'a la unix', 
REM replace backslashes (\) with slashes (/) and put -/cygdrive/- in front of the 
REM drive letter:
REM 
REM Example : C:\WORK\* --> /cygdrive/c/work/*
REM 
REM Example 1 - rsync recursively to a unix server with an openssh server :
REM
REM       rsync -r /cygdrive/c/work/ remotehost:/home/user/work/
REM
REM Example 2 - Local rsync recursively 
REM
REM       rsync -r /cygdrive/c/work/ /cygdrive/d/work/doc/
REM
REM Example 3 - rsync to an rsync server recursively :
REM    (Double colons?? YES!!)
REM
REM       rsync -r /cygdrive/c/doc/ remotehost::module/doc
REM
REM Rsync is a very powerful tool. Please look at documentation for other options. 
REM

REM ** CUSTOMIZE ** Enter your rsync command(s) here
