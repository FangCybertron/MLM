using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facet.Combinatorics;
using System.Net;
using System.Threading;

namespace MLM
{
    class Program
    {
        static int success = 0;
        static void SendToGame(string id)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://mapi.mobilelegends.com/api/sendmail");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string template = "{\"gameid\":\"" + id + "\",\"captcha\":\"\",\"language\":\"en\"}";
                    streamWriter.Write(template);
                }

                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var s = streamReader.ReadToEnd();
                    if (s.Contains("Success"))
                        success++;
                }
            } catch(Exception e)
            {
               
            }
        }
        static void UpdateUI()
        {
            while(true)
            {
                Console.Write("\rTotal Sent: {0}", success);
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Game ID: ");
            string game_id = Console.ReadLine();

            Console.Write("Enter mail count to sent: ");
            ulong m = ulong.Parse(Console.ReadLine());

            new Thread(UpdateUI).Start();

            for(ulong i = 0; i < m; i++)
            {
                SendToGame(game_id);
                game_id += " ";
            }
            Console.Read();
        }
    }
}
