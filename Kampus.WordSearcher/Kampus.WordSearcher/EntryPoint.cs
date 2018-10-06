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

            // ClientHttp clientHttp = new ClientHttp();
            // clientHttp.Start(url);

            //MapElemant tom = map[0,0];
            // Console.WriteLine(tom?.mapElemant);
            ClientHttp clientHttp = new ClientHttp();
            Helper helper = new Helper();
            helper= clientHttp.Start(url).Result;
            helper.ClientHttp = clientHttp;
            StartPosition startPosition = new StartPosition();
            startPosition.Run(helper);
            startPosition.SeatchStartPosition();

            //Matrix startMap = new Matrix();
            //startPosition.GetFirstPartLeter(startMap, BasePatch.start);
            //for (int i = 0; i < 13; i++)
            //{
            //    clientHttp.SendRequest(BasePatch.left);
            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    clientHttp.SendRequest(BasePatch.down);
            //}
            //startPosition.GetFirstPartLeter(startMap, BasePatch.down);

            MapService2 search = new MapService2();
            //search.Run(helper);
            
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