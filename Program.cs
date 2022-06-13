using System;
using System.Net;
using System.Net.Http;
using System.IO;

namespace Imgur_Downloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Keywords");
            Console.WriteLine();
            string read = Console.ReadLine();
            Search(read);
        }

        public static void Search(string input)
        {
            int pics = 0;
            int videos = 0;
            int total = 0;
            int gifs = 0;
            Directory.CreateDirectory(input);
            string keywords = "";
            foreach (string word in input.Split(' '))
            {
                keywords += word + "+";
            }
            using (WebClient client = new WebClient())
            {
                string result = client.DownloadString("https://imgur.com/search?q=" + keywords);
                foreach (string line in result.Split('\n'))
                {
                    if (line.Contains("i.imgur.com/"))
                    {
                        total += 1;
                        string code = line.Replace("<img alt=\"\" src=\"//", "").Replace("\" />", "").Replace("        ", "");
                        string url = "https://" + code.Remove(code.LastIndexOf('b'), 1);
                        string file = code.Replace("i.imgur.com/", "");
                        string filename = file.Remove(file.LastIndexOf('.'));
                        string dir = input + "\\" + filename;
                        using (HttpClient http = new HttpClient())
                        {
                            string req = http.GetAsync(url, HttpCompletionOption.ResponseContentRead).Result.ToString();
                            if (req.Contains("Content-Type: image/jpeg"))
                            {
                                client.DownloadFile(url, dir + ".png");
                                pics += 1;
                            }
                            else if (req.Contains("Content-Type: image/gif"))
                            {
                                client.DownloadFile(url, dir + ".gif");
                                gifs += 1;
                            }
                            else if (req.Contains("Content-Type: video/"))
                            {
                                client.DownloadFile(url, dir + ".mp4");
                                videos += 1;
                            }
                        }
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("=================================");
                Console.WriteLine("");
                Console.WriteLine("Total Found : " + total);
                Console.WriteLine("");
                Console.WriteLine("Total Pics : " + pics);
                Console.WriteLine("");
                Console.WriteLine("Total Gifs : " + gifs);
                Console.WriteLine("");
                Console.WriteLine("Total Vids : " + videos);
                Console.WriteLine("");
                Console.WriteLine("=================================");
                Console.WriteLine("");
                Console.ReadLine();
            }
        }
    }
}
