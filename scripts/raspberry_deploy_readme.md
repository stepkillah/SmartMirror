# Overview

This is small guide on how to set all things up.

## Required software:

### Linux:

  1. dotnet
  2. vsdbg
  3. SSH Server

### Windows:

  1. Visual Studio/VS Code
  2. plink/OpenSSH
  3. PCSP/RSync/SCP

## Setup:

### Linux:

  1. Setup [ssh](#ssh)
  2. Install VSDBG by running the following command. Replace `~/vsdbg` with wherever you want vsdbg installed to.

    #### Using cURL

    `curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg`
    
    #### Alternatively, wget

    `wget https://aka.ms/getvsdbgsh -O - 2>/dev/null | /bin/sh /dev/stdin -v latest -l ~/vsdbg`

  3. Configure ssh auto-login  
execute `sudo nano /etc/ssh/sshd_config` and set `#PermitRootLogin yes`
  4. Grant permission  
`install -d -m 700 ~/.ssh`
  5. Set Public key from puttygen.exe  
`nano ~/.ssh/authorized_keys`
  6. Grant another permissions  
`sudo chmod 644 ~/.ssh/authorized_keys`  
`sudo chown pi:pi ~/.ssh/authorized_keys`

### Windows:

  1. Generate public key for passwordless file transfer (See [SSH](#ssh) section)
  2. Create new configuration in `.csproj` file  
Example:  
`<Configurations>Debug;Release;RaspberryDebug;</Configurations>`
  3. Create bat file with post build script  
Example using SCP/PSCP or using [script](raspberry_deploy.bat):  
```
scp -r -P 22 -i e:\ssh\pi_key.ppk "path\to\source\*" pi@192.168.0.108:/home/pi/Projects/SmartMirror.LedStripe
pause
```
Example using RSync located [here](raspberry_deploy_rsync.bat)

  4. Generate launch.vs.json  
https://docs.microsoft.com/en-us/visualstudio/ide/customize-build-and-debug-tasks-in-visual-studio
  5. Set ssh or plink addapter in launch.vs.json
```
{
    "version": "0.2.1",
    "adapter": "e:\\ssh\\ssh.exe",
    "adapterArgs": "-i e:\\ssh\\pi_key.ppk pi@192.168.0.108 -batch -T ~/vsdbg/vsdbg --interpreter=vscode",  
    "configurations": [
        {
            "name": ".NET Core Raspberry Launch",
            "type": "coreclr",
            "cwd": "~/Projects/SmartMirror.LedStripe",
            "program": "/home/pi/Projects/SmartMirror.LedStripe/SmartMirror.LedStripe.dll",
            "request": "launch",
            "logging": {
                "engineLogging": true
            }
        }
    ]
}
```
  6. Set portable debug type in csproj  
```
    <PropertyGroup Condition="'$(Configuration)' == 'RaspberryDebug'">
        <DebugType>portable</DebugType>
    </PropertyGroup>
```
  7. Add Post build scipt to execute previously created bat. This is used to automatically deploy build files to target machine, in order to simplify remote debugging  
```
    <Target Name="PostBuild" AfterTargets="PostBuildEvent" Condition="'$(Configuration)' == 'RaspberryDebug'">
        <Exec Command="call &quot;$(SolutionDir)scripts\raspberry_deploy.bat&quot; &quot;$(TargetDir)&quot;" />
    </Target> 
```
  8. Start debug using following command in VisualStudio command line or using [RemoteDebugLauncher](https://marketplace.visualstudio.com/items?itemName=xpasza.RemoteDebugLauncher22) extension which automates that part.
`DebugAdapterHost.Launch /LaunchJson:"E:\Job\SmartMirror\SmartMirror\.vs\launch.vs.json"`

## SSH

For SSH support, you will first want to enable SSH in your Linux server. For example, on Ubuntu you can do that by running:

`sudo apt-get install openssh-server`

Then, on Windows you need an SSH client designed to be used programmatically. Plink.exe from PuTTY fits the bill, though you can certainly use other tools too. If you are running on a recent version of Windows 10, you can use the built-in ssh.exe (C:\Windows\sysnative\OpenSSH\ssh.exe). Note: Using C:\Windows\System32\ will not work as expected.

Next, you need a scriptable way to authenticate. One option is to provide the password on the command line, but obviously there are some security concerns there. A more secure option is to use SSH keys

Download puttygen.exe from PuTTY.  
Run the tool and click 'Generate' and follow the instructions. Note: leave 'Key passphrase' empty, otherwise plink.exe will fail to open the key.  
Save the generated private key to a file.  
Copy the public key's text from the top part of the PuTTY Key Generator's Window.  
Add this to the `~/.ssh/authorized_keys` file on your server.  
Test your connection from the command line. This will also allow you to accept the server's key on the client, which must be done the first time.  
Example:  

`c:\mytools\plink.exe -i c:\users\greggm\ssh-key.ppk greggm@mylinuxbox -batch -T echo "hello world"`

>hello world

should print

## Usefull links:

https://github.com/microsoft/MIEngine/wiki/Offroad-Debugging-of-.NET-Core-on-Linux---OSX-from-Visual-Studio  
https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html  
https://github.com/OmniSharp/omnisharp-vscode/wiki/Attaching-to-remote-processes  
https://docs.microsoft.com/en-us/visualstudio/ide/customize-build-and-debug-tasks-in-visual-studio?view=vs-2019  
https://www.hanselman.com/blog/RemoteDebuggingWithVSCodeOnWindowsToARaspberryPiUsingNETCoreOnARM.aspx
