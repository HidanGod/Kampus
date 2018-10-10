using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{



    class EntryPoint
    {

        private static void Main(string[] args)
        {
            // string url = "http://Intern-test.skbkontur.ru:80";
            // string tokenGame = "b804a143-fa76-43d2-8888-bc5a7f094b57";

            if (args.Length != 2) Console.WriteLine("запустите приложения с аргументами Url и token");
            else
            {
                Console.WriteLine("Start");
                string url = args[0];
                string tokenGame = args[1];
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken token = cancelTokenSource.Token;
                new Task(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(280));
                    Console.WriteLine("Запущен поиск слов");
                    cancelTokenSource.Cancel();
                }).Start();
                ClientHttp clientHttp = new ClientHttp();
                clientHttp.client = new HttpClient();
                Helper helper = new Helper();
                helper = clientHttp.Start(url, tokenGame).Result;
                helper.ClientHttp = clientHttp;
                helper.ListWord = new List<string>();
                MapService mapService = new MapService();
                mapService.Run(helper);
                mapService.SeatchStartPosition(token);
            }
            Console.Read();
        }
      
    }
}