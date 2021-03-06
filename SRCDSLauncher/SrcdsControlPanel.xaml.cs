﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SRCDSLauncher
{
    public partial class SrcdsControlPanel : Window
    {
        public string SrcdsFile { get; set; }
        private Process SrcdsProcess { get; set; }

        public string Command { get; set; }

        public SrcdsControlPanel()
        {
            InitializeComponent();

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

        private void DataReceived(object sender, DataReceivedEventArgs arg)
        {
            if (!String.IsNullOrEmpty(arg.Data))
            {
                Console.WriteLine(arg.Data);
            }
        }
    }
}
