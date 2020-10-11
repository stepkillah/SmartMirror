***REMOVED***
using System.Diagnostics;
using System.Linq;
***REMOVED***

namespace SmartMirror.Core
***REMOVED***
    public static class ShellHelper
    ***REMOVED***
        public static string Bash(this string cmd)
        ***REMOVED***
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            ***REMOVED***
                StartInfo = new ProcessStartInfo
                ***REMOVED***
                    FileName = "/bin/bash",
                    Arguments = $"-c \"***REMOVED***escapedArgs***REMOVED***\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
              ***REMOVED***
          ***REMOVED***;
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
      ***REMOVED***

        public static async Task<string> BashAsync(this string cmd)
        ***REMOVED***
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            ***REMOVED***
                StartInfo = new ProcessStartInfo
                ***REMOVED***
                    FileName = "/bin/bash",
                    Arguments = $"-c \"***REMOVED***escapedArgs***REMOVED***\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
              ***REMOVED***
          ***REMOVED***;
            process.Start();
            string result = await process.StandardOutput.ReadToEndAsync();
            process.WaitForExit();
            return result;
      ***REMOVED***

        public static Process StartBash(this string cmd)
        ***REMOVED***
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            ***REMOVED***
                StartInfo = new ProcessStartInfo
                ***REMOVED***
                    FileName = "/bin/bash",
                    Arguments = $"-c \"***REMOVED***escapedArgs***REMOVED***\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
              ***REMOVED***
          ***REMOVED***;
            process.Start();
            return process;
      ***REMOVED***


        public static async Task<string> BashAsync(TimeSpan commandDelay, params string[] cmd)
        ***REMOVED***
            for (int i = 0; i < cmd.Length; i++)
            ***REMOVED***
                cmd[i] = cmd[i].Replace("\"", "\\\"");
          ***REMOVED***

            var firstCmd = cmd.First();
            var otherCommands = cmd.Where(d => !d.Equals(firstCmd)).ToArray();
            var process = new Process()
            ***REMOVED***
                StartInfo = new ProcessStartInfo
                ***REMOVED***
                    FileName = "/bin/bash",
                    Arguments = $"-c \"***REMOVED***cmd.First()***REMOVED***\"",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
              ***REMOVED***
          ***REMOVED***;
            process.Start();
            string result = null;
            foreach (var command in otherCommands)
            ***REMOVED***
                await Task.Delay(commandDelay);
                await process.StandardInput.WriteLineAsync(command);
                result = await process.StandardOutput.ReadToEndAsync();
          ***REMOVED***
            process.WaitForExit();
            return result;
      ***REMOVED***
  ***REMOVED***

***REMOVED***
