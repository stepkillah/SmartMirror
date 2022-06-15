# SmartMirror

Smart mirror helper for [speech recognition](#speech-recognition), led control and [MagicMirror](#magicmirrorrunner)

## Configuration

To configure different aspect of application you can use `appsettings.json`.  
Here is an example of settings file
```
***REMOVED***
  "MagicMirrorRunner": ***REMOVED***
    "DefaultUserName": "<Default UserName for sudo command>",
    "WorkingDirectory": "<MagicMirror root folder path on target device>"
***REMOVED***
  "LED": ***REMOVED***
    "Count": <Total count of LED lights>,
    "Missing": [<Index of lights that is missing/not working in LED strip>],
    "BusId": <Physical SPI bus id (0)>,
    "ChipSelectLine": <SPI line (0)>
***REMOVED***
  "SpeechRecognition": ***REMOVED***
    "ActivationRecognitionTablePath": "<path to .table file for keyword recognition>",
    "SubscriptionKey": "<A***REMOVED***Key from Azure Portal>",
    "Region": "<Region from Azure Portal>"
***REMOVED***
***REMOVED***
```

## Debbug and deployment

For debugging and deployment you can check deployment readme located [here](scripts/raspberry_deploy_readme.md). I'm using raspberry as target.

## Commands

To control what is going on with your SmartMirror you can use commands - either using your [voice](#speech-recognition) or keyboard input. For now, application supports these commands:

- `light on` enable led lights
- `light off` disable led lights
- `color ***REMOVED***colorName or code***REMOVED***` set led color to specified in command
- `sound test` play Success sound (used for test purposes)

## LED Manager

SmartMirror app designed to work with Ws2812b LED strip

In order to control LED strip, app uses `ws2812b` binding from [this NuGet](https://www.nuget.org/packages/Iot.Device.Bindings/), through SPI interface.  
Each ***REMOVED***xel is set separately, so in order to change color of the whole LED strip - each ***REMOVED***xel should be set to that color.

## 3rd party services

App uses some 3rd party external processes to perform some of the actions, for example, MagicMirror is run from the inside of the application.

### Speech recognition

Current active speech recognition technology is Microsoft one, but there also possible to use DeepSpeech (not tested)

#### Microsoft

For speech recognition app uses [Microsoft Cognitive Services](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/) ([GitHub](https://github.com/Azure-Samples/cognitive-services-speech-sdk))  
Speech recognition is split into two parts - keyword recognition and actual command recognition.  
Keyword recognition works offline and uses `Assets/mirror_activation.table` table created in Azure Portal, trained to use `Hey mirror` as an activation word.  
After the application recognizes the keyword - it will start listening for an actual command that is described above and if the command is recognized successfully by the Microsoft Azure Cognitive Services - it will be executed.  
All actions will be confirmed with appropriate sound

**Be aware, Microsoft Cognitive Services [nuget](https://www.nuget.org/packages/Microsoft.CognitiveServices.Speech) works only with ARM64. Tested on [Ubuntu server](https://***REMOVED***.com/download/raspberry-***REMOVED***) 18.04 and 20.04, probably will work the same with on latest 22.04**

#### DeepSpeech

Another option for speech recognition is to use [DeepSpeech](https://github.com/mozilla/DeepSpeech), it works offline and does not requires active internet connection.  
In order to use DeepSpeech - custom C# wrapper should be created that supports .NET6 ([PR](https://github.com/mozilla/DeepSpeech/pull/3373) for .NET Core support)  
This approach is not tested in real life.  
Application source code contains commented DeepSpeech service as an direction how to start

### MagicMirrorRunner

Service that starts and manages [MagicMirror](https://github.com/MichMich/MagicMirror) electron application.  
It uses C# `Process` to execute `sudo -H -u ***REMOVED***DefaultUserName***REMOVED*** DISPLAY=:0.0 npm start` command to start MagicMirror on first display (`DISPLAY=:0.0`).   
The current hardcoded working directory is `/home/***REMOVED***/Projects/MagicMirror`  
MagicMirror itself should be configured separately by following [guides](https://docs.magicmirror.builders/)

### Audio Player

App uses two types of audio players - one for [linux](#aplayrunner) and one for [windows](#naudioplayerservice)

Simple playback for a specific file. SmartMirror app has currently two sounds to play:
- Recognitions success (`Assets/success.wav`)
- Recognition failed (`Assets/error.wav`)

SmartMirror app provides a file path to platform-specific player which does all the magic for the playback.

#### APlayRunner

On Linux app uses [aplay](http://manpages.***REMOVED***.com/manpages/focal/man1/aplay.1.html).

#### NAudioPlayerService

On Windows app uses [NAudio](https://github.com/naudio/NAudio)