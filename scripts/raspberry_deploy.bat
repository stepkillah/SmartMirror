SETLOCAL
SET SSHKeyFilePath=%1
SET SourceFilePath=%2
SET DestinationFilePath=%3

scp -r -P 22 -i %SSHKeyFilePath% %SourceFilePath% %DestinationFilePath%
pause

