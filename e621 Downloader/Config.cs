using System;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace e621Downloader
{
    public class Config
    {
        public string path { get; set; }
        public string login { get; set; }
        public string api { get; set; }
        public string[] blacklist { get; set; }

        public Config getConfiguration(string confPath)
        {
            Config config = new Config();
            if (File.Exists(confPath))
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(confPath));
            }
            else
            {
                bool validPath = false;
                do
                {
                    Console.WriteLine("Please enter your desired download path!\nExample: C:\\Users\\Vendell\\Downloads\\Yiff\\\n");
                    string pathInput = Console.ReadLine();
                    if (!pathInput.EndsWith("\\"))
                    {
                        pathInput = pathInput + "\\";
                    }
                    if (!Directory.Exists(pathInput) || pathInput == "\\")
                    {
                        Console.WriteLine("Please enter a valid Path!");
                    }
                    else
                    {
                        validPath = true;
                        config.path = pathInput;
                    }
                } while (!validPath);

                Console.WriteLine("If you wish to be logged into the e621 API, please enter your credentials. If not, leave empty.");
                Console.WriteLine("Please enter your e621 Username: ");
                config.login = Console.ReadLine();
                if (config.login != "")
                {
                    Console.WriteLine("Please enter your e621 API Key: ");
                    config.api = Console.ReadLine();
                }
                config.blacklist = new string[] {""};
                File.WriteAllText(confPath, JsonConvert.SerializeObject(config, Formatting.Indented));
            }

            return config;
        }
    }
}
