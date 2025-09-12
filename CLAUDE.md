# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

SmartMirror is a C# .NET 9.0 application that provides smart mirror functionality with speech recognition, LED control, and MagicMirror integration. The application is designed to run on Raspberry Pi with remote debugging capabilities from Windows.

## Architecture

The project follows a modular architecture with dependency injection:

- **SmartMirror.Core**: Single executable project containing all services and logic
- **Services**: Background services for speech recognition, LED control, keyboard input, and external process management
- **Interfaces**: Abstraction layer for cross-platform compatibility
- **Models**: Configuration and data models with JSON serialization
- **Extensions**: Utility extensions for configuration, logging, and service registration

### Key Services

- `SpeechRecognitionHostedService`: Manages keyword activation and command recognition using Microsoft Cognitive Services
- `LedManager`: Controls WS2812B LED strips via SPI interface
- `KeyboardListener`: Background service for keyboard command input
- `MagicMirrorRunner`: Manages external MagicMirror electron application
- `CommandsHandler`: Central command processing and execution
- `DisplayManager`: GPIO-based display control
- `GpioButtonListener`: Hardware button input handling

### Platform-Specific Audio

- Windows: `NAudioPlayerService` using NAudio library
- Linux: `APlayRunner` using system aplay command

## Build & Development Commands

### Building
```bash
# Standard build
dotnet build

# Raspberry Pi debug build with auto-deploy
dotnet build -c RaspberryDebug
dotnet build -c RaspberryDebugX64

# Using rsync deployment
dotnet build -c RaspberryDebugRsync
dotnet build -c RaspberryDebugRsyncX64
```

### Running
```bash
# Local development
dotnet run --project SmartMirror.Core

# On Raspberry Pi (after deployment)
dotnet SmartMirror.Core.dll
```

### Remote Debugging

The project includes automated deployment scripts for Raspberry Pi development:
- `scripts/raspberry_deploy.bat`: SCP-based deployment
- `scripts/raspberry_deploy_rsync.bat`: rsync-based deployment

Configuration requires SSH key setup and appropriate build configurations as documented in `scripts/raspberry_deploy_readme.md`.

## Configuration

The application uses `appsettings.json` for configuration with these main sections:

- **MagicMirrorRunner**: MagicMirror working directory path
- **LED**: LED strip configuration (count, missing LEDs, SPI settings)
- **SpeechRecognition**: Azure Cognitive Services credentials and activation table path
- **ButtonControls**: GPIO pin assignments for LED and display controls

## Key Dependencies

- **Iot.Device.Bindings**: WS2812B LED control via SPI
- **Microsoft.CognitiveServices.Speech**: Speech recognition (ARM64 only)
- **System.Device.Gpio**: GPIO hardware control
- **Microsoft.Extensions.Hosting**: Background service hosting
- **NAudio**: Windows audio playback
- **Polly**: Resilience and retry policies

## Supported Commands

The application recognizes these voice/keyboard commands:
- `light on/off`: LED strip control
- `color {name/code}`: Set LED color
- `sound test`: Play test audio
- `display toggle`: Toggle display on/off via GPIO control

## Development Notes

- Target framework: .NET 9.0
- Primary deployment target: Raspberry Pi (ARM64)
- Speech recognition requires active internet connection for Azure services
- LED control requires hardware SPI interface
- MagicMirror integration requires separate MagicMirror installation
- Cross-platform audio support with platform-specific implementations