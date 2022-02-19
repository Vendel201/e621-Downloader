using System;
using Newtonsoft.Json;
using System.IO;

namespace e621Downloader
{
    public class Config
    {
        public string path { get; set; }
        public string login { get; set; }
        public string api { get; set; }

        public Config getConfiguration(string confPath)
        {
            Config config = new Config();
            if (File.Exists(confPath))
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(confPath));
            }
            else
            {
                Console.WriteLine("Please enter your desired download path!\nExample: C:\\Users\\Vendell\\Downloads\\Yiff\\\n");
                config.path = Console.ReadLine();
                Console.WriteLine("If you wish to be logged into the e621 API, please enter your credentials. If not, ignore the next two entries.");
                Console.WriteLine("Please enter your e621 Username: ");
                config.login = Console.ReadLine();
                Console.WriteLine("Please enter your e621 API Key: ");
                config.api = Console.ReadLine();
                File.WriteAllText(confPath, JsonConvert.SerializeObject(config));
            }

            return config;
        }
    }
}
