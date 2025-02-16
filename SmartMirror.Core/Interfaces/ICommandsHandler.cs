﻿using System.Threading;
using System.Threading.Tasks;
using SmartMirror.Core.Enums;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Interfaces
{
    public interface ICommandsHandler
    {
        ValueTask HandleCommand(SmartMirrorCommand command, object data);
        ValueTask HandleCommand(SmartMirrorCommand command, object data, CancellationToken cancellationToken);
        ValueTask<CommandRecognitionResult> RecognizeCommand(string rawText);
    }
}
