using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public static string query;

        public static float downloadedCount;

        public static WebClient client = new WebClient();
        public static string path = File.ReadLines(string.Concat(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "\\config.txt")).First<string>();

        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                query = string.Concat("limit=", args[0], "&tags=", args[1]);
            }

            if (query == null)
            {
                Console.Title = "E621 Bulk Downloader";
                Console.WriteLine("Please input your search query! See https://e621.net/help/show/api#posts_list for help!");
                Console.WriteLine("Example: limit=10&tags=cute+dragon");
                Console.Write("Query: ");
                query = Console.ReadLine();
            }

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

                    List<Post> originalTweets = new List<Post>();

                    StreamReader reader = new StreamReader(stream, true);
                    String responseString = reader.ReadToEnd();
                    Root objects = JsonConvert.DeserializeObject<Root>(responseString);

                    int count = objects.posts.Count;

                    foreach (var post in objects.posts)
                    {
                        downloadedCount++;
                        Console.Title = count + " / " + (count - downloadedCount).ToString();
                        string filename = string.Concat(path, post.file.md5, ".", post.file.ext);
                        client.DownloadFile(post.file.url, filename);
                    }
                }
            }

            Console.Title = "E621 Bulk Downloader";
            Console.WriteLine(string.Concat("You downloaded ", downloadedCount, " images!"));
            File.AppendAllText(string.Concat(path, "\\queries.txt"), string.Concat(new object[] { DateTime.Now, " Query: ", query, " / Downloaded: ", downloadedCount, "\n" }));
            downloadedCount = 0;
            Console.WriteLine("");
            Console.WriteLine("Do you want to download more yiff?");
            Console.WriteLine("(Y/N)");
            if (Console.ReadLine() != "Y")
            {
                Environment.Exit(0);
                return;
            }
            Console.WriteLine("");
            Main(null);
        }
    }
}