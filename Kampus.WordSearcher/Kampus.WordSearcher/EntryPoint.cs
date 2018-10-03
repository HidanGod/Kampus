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
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{



    class EntryPoint
    {

        private static void Main(string[] args)
        {

            string url = "http://Intern-test.skbkontur.ru:80";
            // string page1 = "/task/game/stats";
            // string page2 = "/task/game/start";
            // string page3 = "/task/move/down";
            //  string token = "b804a143-fa76-43d2-8888-bc5a7f094b57";

            ClientHttp clientHttp = new ClientHttp();
            clientHttp.Start(url);


            MapServise search = new MapServise();
            search.RunAsync();
            
            //List<bool[,]> bb = new List<bool[,]>();

            //string currDir = Environment.CurrentDirectory.ToString()+ "/abc.txt";

            //LetterCreater search1 = new LetterCreater();
            //bb = search1.Сreate(currDir,7,7);
            //WordSearch search2 = new WordSearch();  
            //List<string> Search = new List<string>();
            //int[,] map = new int[100,100];
            //Search =search2.Search(map,bb);
            Console.Read();
        }
      
    }
}