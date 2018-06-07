using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SRCDSLauncher
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void AppStart(object sender, StartupEventArgs e)
        {
            foreach(var path in SteamInfo.GetSteamLibraryDirectories())
            {
                var cmm = Path.Combine(path, "common");
                if(Directory.Exists(cmm))
                {
                    Regrresion(cmm);
                }
            }
        }

        private void Regrresion(string dir)
        {
            var entries = Directory.GetFiles(dir);
            string find = null;
            if((find = Array.Find(entries, x => x.EndsWith("steam.inf") && File.Exists(x))) != null)
            {
                SourceGame sg = SourceGame.Initialize(dir);
                if(sg != null)
                {
                    Console.WriteLine(sg.Game);
                    Console.WriteLine(sg.ClientAppID);
                    Console.WriteLine(sg.ServerAppID);
                    Console.WriteLine(sg.ServerVersion);
                    Console.WriteLine(sg.NeedUpdate);
                }
            }
            else
            {
                foreach(var d in Directory.GetDirectories(dir))
                {
                    if(!d.Equals(".") && !d.Equals("..") && !d.Equals("..."))
                        Regrresion(d);
                }
            }
        }
    }
}
