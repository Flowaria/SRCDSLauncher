using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SRCDSLauncher
{
    public class SourceGame
    {
        private const string VERSION_CHECK_URL = "https://api.steampowered.com/IGCVersion_{0}/GetServerVersion/v1?format=xml";

        public string Game { get; private set; }
        public string BaseDir { get; private set; }

        public int ClientAppID { get; private set; }
        public int ServerAppID { get; private set; }
        public int ServerVersion { get; private set; }
        public bool NeedUpdate { get; private set; }

        private SourceGame()
        {

        }

        public string GetSteamInfoFile()
        {
            return Path.Combine(BaseDir, "steam.inf");
        }

        public static SourceGame Initialize(string directory)
        {
            var infofile = Path.Combine(directory, "steam.inf");
            if (Directory.Exists(directory) && File.Exists(infofile))
            {
                int v_sv = 0;
                int appid_c = 0;
                int appid_s = 0;
                string name = null;

                var line = File.ReadAllLines(infofile);
                if (line.Length > 0)
                {
                    foreach(var t in line)
                    {
                        var split = t.Split('=');
                        if(split.Length == 2)
                        {
                            switch(split[0].ToLower())
                            {
                                case "appid":
                                    appid_c = int.Parse(split[1]);
                                break;

                                case "serverappid":
                                    appid_s = int.Parse(split[1]);
                                break;

                                case "serverversion":
                                    v_sv = int.Parse(split[1]);
                                break;

                                case "productname":
                                    name = split[1];
                                break;
                            }
                        }
                    }
                }
                if(v_sv != 0 && appid_c != 0 && appid_s != 0 && name != null)
                {
                    using (var wc = new WebClient())
                    {
                        try
                        {
                            var content = wc.DownloadString(String.Format(VERSION_CHECK_URL, appid_c));

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(content);

                            if (doc.DocumentElement["deploy_version"] != null)
                            {
                                var game = new SourceGame();

                                game.NeedUpdate = int.Parse(doc.DocumentElement["deploy_version"].InnerText) != v_sv;
                                game.ServerVersion = v_sv;
                                game.ClientAppID = appid_c;
                                game.ServerAppID = appid_s;
                                game.Game = name;
                                game.BaseDir = Path.GetFullPath(directory);

                                return game;
                            }
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        
    }
}
