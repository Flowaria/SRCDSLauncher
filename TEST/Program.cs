using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "srcds.exe",
                    Arguments = "-console -game tf -port 27099 +map ctf_2fort +maxplayers 32",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };

            proc.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                // Prepend line numbers to each line of the output.
                if (!String.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
            });

            proc.Start();
            proc.BeginOutputReadLine();
            proc.WaitForExit();
        }
    }
}
