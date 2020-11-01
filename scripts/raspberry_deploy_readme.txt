Usefull links:
https://github.com/microsoft/MIEngine/wiki/Offroad-Debugging-of-.NET-Core-on-Linux---OSX-from-Visual-Studio
https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html
https://github.com/OmniSharp/omnisharp-vscode/wiki/Attaching-to-remote-processes
https://docs.microsoft.com/en-us/visualstudio/ide/customize-build-and-debug-tasks-in-visual-studio?view=vs-2019
https://www.hanselman.com/blog/RemoteDebuggingWithVSCodeOnWindowsToARaspberryPiUsingNETCoreOnARM.aspx

Required software:

Linux:
1. dotnet
2. vsdbg
3. ssh server


Windows:
1. Visual Studio
2. plink
3. pcsp

Setup:

Linux:

1. Setup ssh
2. Install VSDBG by running the following command. Replace '~/vsdbg' with wherever you want vsdbg installed to.
# Using cURL
curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg
# Alternatively, wget
wget https://aka.ms/getvsdbgsh -O - 2>/dev/null | /bin/sh /dev/stdin -v latest -l ~/vsdbg
3. Configure ssh auto-login
sudo nano /etc/ssh/sshd_config
#PermitRootLogin yes
4. Grant permission
install -d -m 700 ~/.ssh
5. Set Public key from puttygen.exe
nano ~/.ssh/authorized_keys
6. Grant another permissions
sudo chmod 644 ~/.ssh/authorized_keys
sudo chown ***REMOVED***:***REMOVED*** ~/.ssh/authorized_keys

Windows:

1. Generate public key for passwordless file transfer (See SSH section)
2. Create new configuration 
Example: RespberryDebug
3. Create bat file with post build script
e:\ssh\pscp.exe -r -P 22 -i e:\ssh\***REMOVED***_key.ppk "%~1\*" ***REMOVED***@***REMOVED***08:/home/***REMOVED***/Projects/SmartMirror.LedStripe
exit 0
4. Generate launch.vs.json
https://docs.microsoft.com/en-us/visualstudio/ide/customize-build-and-debug-tasks-in-visual-studio?view=vs-2019
5. Set plink addapter in launch.vs.json
***REMOVED***
    "version": "0.2.1",
    "adapter": "e:\\ssh\\plink.exe",
    "adapterArgs": "-i e:\\ssh\\***REMOVED***_key.ppk ***REMOVED***@***REMOVED***08 -batch -T ~/vsdbg/vsdbg --interpreter=vscode",
  
    "configurations": [
        ***REMOVED***
            "name": ".NET Core Raspberry Launch",
            "type": "coreclr",
            "cwd": "~/Projects/SmartMirror.LedStripe",
            "program": "/home/***REMOVED***/Projects/SmartMirror.LedStripe/SmartMirror.LedStripe.dll",
            "request": "launch",
            "logging": ***REMOVED***
                "engineLogging": true
          ***REMOVED***
      ***REMOVED***
    ]
***REMOVED***
6. Set portable debug type in csproj
    <PropertyGroup Condition="'$(Configuration)' == 'RaspberryDebug'">
        <DebugType>portable</DebugType>
    </PropertyGroup>
7. Add Post build scipt to execute previously created bat
    <Target Name="PostBuild" AfterTargets="PostBuildEvent" Condition="'$(Configuration)' == 'RaspberryDebug'">
        <Exec Command="call &quot;$(SolutionDir)scripts\raspberry_deploy.bat&quot; &quot;$(TargetDir)&quot;" />
    </Target> 
6. Start debug using following command in VisualStudio command line
DebugAdapterHost.Launch /LaunchJson:"E:\Job\SmartMirror\SmartMirror\.vs\launch.vs.json" /EngineGuid:541B8A8A-6081-4506-9F0A-1CE771DEBC04




SSH
For SSH support, you will first want to enable SSH in your Linux server. For example, on Ubuntu you can do that by running:

sudo apt-get install openssh-server

Then, on Windows you need an SSH client designed to be used programmatically. Plink.exe from PuTTY fits the bill, though you can certainly use other tools too. If you are running on a recent version of Windows 10, you can use the built-in ssh.exe (C:\Windows\sysnative\OpenSSH\ssh.exe). Note: Using C:\Windows\System32\ will not work as expected.

Next, you need a scriptable way to authenticate. One option is to provide the password on the command line, but obviously there are some security concerns there. A more secure option is to use SSH keys --

Download puttygen.exe from PuTTY.
Run the tool and click 'Generate' and follow the instructions. Note: leave 'Key passphrase' empty, otherwise plink.exe will fail to open the key.
Save the generated private key to a file.
Copy the public key's text from the top part of the PuTTY Key Generator's Window.
Add this to the ~/.ssh/authorized_keys file on your server.
Test your connection from the command line. This will also allow you to accept the server's key on the client, which must be done the first time.
Example:

c:\mytools\plink.exe -i c:\users\greggm\ssh-key.ppk greggm@mylinuxbox -batch -T echo "hello world"
# 'hello world' should print

