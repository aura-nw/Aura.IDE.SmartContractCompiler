using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Aura.IDE.RustCompiler.Utils
{
    public static class TerminalUtil
    {
        public static List<string> RunScripts(List<string> scripts, string workingDir)
        {
            Process cmd = new Process();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cmd.StartInfo.FileName = "cmd.exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                cmd.StartInfo.FileName = "/usr/bin/bash";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                cmd.StartInfo.FileName = "sh";
            }

            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WorkingDirectory = workingDir;
            cmd.Start();

            foreach(var script in scripts)
            {
                cmd.StandardInput.WriteLine(script);
            }
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            //cmd.WaitForExit();
            return cmd.StandardError.ReadToEnd().Split("\n").ToList();
        }
    }
}
