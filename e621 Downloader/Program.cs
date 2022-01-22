using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace e621Downloader
{
    internal class Program
    {

        public static List<string> imagesJPG;
        public static List<string> imagesJPGNew;
        public static List<string> imagesPNG;
        public static List<string> imagesPNGNew;
        public static List<string> imagesGIF;
        public static List<string> imagesGIFNew;
        public static List<string> imagesWEBM;
        public static List<string> imagesWEBMNew;

        public static float fileIndex;

        public static string query;

        public static string newline;

        public static float downloadedCount;

        public static WebClient client;

        public static string path;

        static Program()
        {
            imagesJPG = new List<string>();
            imagesJPGNew = new List<string>();
            imagesPNG = new List<string>();
            imagesPNGNew = new List<string>();
            imagesGIF = new List<string>();
            imagesGIFNew = new List<string>();
            imagesWEBM = new List<string>();
            imagesWEBMNew = new List<string>();
            newline = Environment.NewLine;
            client = new WebClient();
            path = File.ReadLines(string.Concat(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "\\config.txt")).First<string>();
        }

        public static void Main()
        {
            Console.Title = "E621 Bulk Downloader";
            Console.WriteLine("Please input your search query! See https://e621.net/help/show/api#posts_list for help!");
            Console.WriteLine("Example: limit=10&tags=cute+dragon");
            Console.Write("Query: ");
            query = Console.ReadLine();
            if (query.Contains("cub"))
            {
                Console.WriteLine("");
                Console.WriteLine("FBI OPEN UP");
            }
            Console.WriteLine("");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Concat("https://e621.net/posts.json?", query, "&login=Vendell&api_key=AadndrowG4dy4SoJFjpPGxdc"));
            httpWebRequest.UserAgent = "Vendell / e621 Bulk Downloader";
            httpWebRequest.Method = "GET";
            using (WebResponse response = httpWebRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonTextReader JsonReader = new JsonTextReader(new StringReader((new StreamReader(stream)).ReadToEnd()));
                    while (JsonReader.Read())
                    {
                        if (JsonReader.Value == null)
                        {
                            continue;
                        }
                        string value = JsonReader.Value.ToString();
                        if (value.Contains("https://static1.e621.net/data/") && value.Contains(".jpg") && !value.Contains("preview") && !value.Contains("sample"))
                        {
                            imagesJPG.Add(value);
                        }
                        if (value.Contains("https://static1.e621.net/data/") && value.Contains(".png") && !value.Contains("preview") && !value.Contains("sample"))
                        {
                            imagesPNG.Add(value);
                        }
                        if (value.Contains("https://static1.e621.net/data/") && value.Contains(".gif") && !value.Contains("preview") && !value.Contains("sample"))
                        {
                            imagesGIF.Add(value);
                        }
                        if (value.Contains("https://static1.e621.net/data/") && value.Contains(".webm") && !value.Contains("preview") && !value.Contains("sample"))
                        {
                            imagesWEBM.Add(value);
                        }
                    }
                }
            }
            imagesJPGNew = imagesJPG.Distinct<string>().ToList<string>();
            foreach (string a in Program.imagesJPGNew)
            {
                Console.Title = string.Concat("E621 Bulk Downloader ", a);
                fileIndex += 1f;
                Console.WriteLine(a);
                client.DownloadFile(a, string.Concat(path, a.Remove(0, 36)));
            }
            imagesJPG.Clear();
            imagesPNGNew = imagesPNG.Distinct<string>().ToList<string>();
            foreach (string a in imagesPNGNew)
            {
                Console.Title = string.Concat("E621 Bulk Downloader ", a);
                fileIndex += 1f;
                Console.WriteLine(a);
                client.DownloadFile(a, string.Concat(path, a.Remove(0, 36)));
            }
            imagesPNG.Clear();
            imagesGIFNew = imagesGIF.Distinct<string>().ToList<string>();
            foreach (string a in imagesGIFNew)
            {
                Console.Title = string.Concat("E621 Bulk Downloader ", a);
                fileIndex += 1f;
                Console.WriteLine(a);
                client.DownloadFile(a, string.Concat(path, a.Remove(0, 36)));
            }
            imagesWEBMNew = imagesWEBM.Distinct<string>().ToList<string>();
            foreach (string a in imagesWEBMNew)
            {
                Console.Title = string.Concat("E621 Bulk Downloader ", a);
                fileIndex += 1f;
                Console.WriteLine(a);
                client.DownloadFile(a, string.Concat(path, a.Remove(0, 36)));
            }
            Console.Title = "E621 Bulk Downloader";
            Program.downloadedCount = Program.imagesGIFNew.Count + Program.imagesJPGNew.Count + Program.imagesPNGNew.Count + Program.imagesWEBMNew.Count;
            Console.WriteLine(string.Concat("You downloaded ", Program.downloadedCount, " images!"));
            File.AppendAllText(string.Concat(Program.path, "\\queries.txt"), string.Concat(new object[] { DateTime.Now, " Query: ", Program.query, " / Downloaded: ", Program.downloadedCount, Program.newline }));
            Program.imagesGIF.Clear();
            Program.imagesGIFNew.Clear();
            Program.imagesJPG.Clear();
            Program.imagesJPGNew.Clear();
            Program.imagesPNG.Clear();
            Program.imagesPNGNew.Clear();
            Program.imagesWEBM.Clear();
            Program.imagesWEBMNew.Clear();
            Program.downloadedCount = 0f;
            Console.WriteLine("");
            Console.WriteLine("Do you want to download more yiff?");
            Console.WriteLine("(Y/N)");
            if (Console.ReadLine() != "Y")
            {
                Environment.Exit(0);
                return;
            }
            Console.WriteLine("");
            Program.Main();
        }
    }
}