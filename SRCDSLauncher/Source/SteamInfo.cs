using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.KeyValue;

namespace SRCDSLauncher
{
    public class SteamInfo
    {
        public static string GetSteamDirectory()
        {
            var dir = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null);
            if(dir != null && Directory.Exists(dir))
            {
                return dir;
            }
            return null;
        }

        public static string[] GetSteamLibraryDirectories()
        {
            var dir = GetSteamDirectory();
            if (dir != null)
            {
                var libraryfolders = Path.Combine(dir, "steamapps", "libraryfolders.vdf");
                if (File.Exists(libraryfolders))
                {
                    KeyValues kv = KVFile.ImportKeyValue(File.ReadAllText(libraryfolders), false);

                    int i = 1;
                    object obj;
                    var list = new List<string>();

                    list.Add(Path.GetFullPath(Path.Combine(dir, "steamapps")));
                    while((obj = kv.Root.GetValue(i++.ToString())) != null)
                    {
                        list.Add(Path.GetFullPath(Path.Combine((string)obj, "steamapps")));
                    }

                    if (list.Count > 0)
                        return list.ToArray();
                }
            }
            return null;
        }

        public static string GetSteamStoreHeaderImage(SourceGame sg)
        {
            return String.Format("https://steamcdn-a.akamaihd.net/steam/apps/{0}/header.jpg", sg.ClientAppID);
        }
    }
}
