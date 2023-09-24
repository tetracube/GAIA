
using System.Diagnostics;
using System.Threading;

namespace GAIA
{
    public class CommandExecuter
    {
        public static void Execute(string Command)
        {
            Process Process = StartProcess(Command);
            Process.WaitForExit();
            //output += "\nExitCode: " + Process.ExitCode;
            Process.Close();
        }

        public static void ExecuteNewWindowSleep(string Command, int WaitMilliseconds)
        {
            Process Process = StartProcess("START " + Command);
            Thread.Sleep(WaitMilliseconds);
            Process.Close();
        }

        private static Process StartProcess(string Command)
        {
            var ProcessInfo = new ProcessStartInfo("cmd.exe", "/c" + Command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            var Process = System.Diagnostics.Process.Start(ProcessInfo);

            Process.OutputDataReceived += (object Sender, DataReceivedEventArgs e) =>
            {
                if (e.Data.Length != 0)
                    UnityEngine.Debug.Log("[" + Command + "] Output >> " + e.Data + "\n");
            };
            Process.BeginOutputReadLine();

            Process.ErrorDataReceived += (object Sender, DataReceivedEventArgs e) =>
            {
                if (e.Data.Length != 0)
                    UnityEngine.Debug.Log("[" + Command + "] Error >> " + e.Data + "\n");
            };
            Process.BeginErrorReadLine();

            return Process;
        }
    }
}