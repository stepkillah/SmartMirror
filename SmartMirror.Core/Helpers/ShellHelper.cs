using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMirror.Core.Helpers
{
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static async Task<string> BashAsync(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string result = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return result;
        }

        public static Process StartBash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            return process;
        }


        public static async Task<string> BashAsync(TimeSpan commandDelay, params string[] cmd)
        {
            for (int i = 0; i < cmd.Length; i++)
            {
                cmd[i] = cmd[i].Replace("\"", "\\\"");
            }

            var firstCmd = cmd.First();
            var otherCommands = cmd.Skip(1).ToArray();
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{firstCmd}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string result = null;
            foreach (var command in otherCommands)
            {
                await Task.Delay(commandDelay);
                await process.StandardInput.WriteLineAsync(command);
                result = await process.StandardOutput.ReadToEndAsync();
            }
            await process.WaitForExitAsync();
            return result;
        }
    }

}
