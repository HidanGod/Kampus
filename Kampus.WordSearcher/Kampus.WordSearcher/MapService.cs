
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{
    class MapService
    {
        Helper helpClass = new Helper();
        ClientHttp clientHttp = new ClientHttp();
        Matrix matr = new Matrix();
        Matrix Map = new Matrix();
        Matrix ABC = new Matrix();
        Map mapContent = new Map();

        public async Task RunAsync()
        {
            string url = "http://Intern-test.skbkontur.ru:80";

            helpClass = clientHttp.Start(url).Result;
            helpClass.ClientHttp = clientHttp;

            //  data = new Map[0,0];

            Map.iMatr = 100;
            Map.jMatr = 100;
            ABC.iMatr = 7;
            ABC.jMatr = 7;
            matr.iMatr = 5;
            matr.jMatr = 11;

            




        bool[,] map = new bool[Map.iMatr, Map.jMatr];
            //HttpClient client = await entryPoint.Start(url);
            //Console.Write("я живой1");
            //client = await entryPoint.Start(url);
            map =  GetMap(map).Result;
            //  Console.Write("я живой2");
            PrintMatrixArray(map);
                List<bool[,]> abc = new List<bool[,]>();
                string currDir = Environment.CurrentDirectory.ToString() + "/abc.txt";
                AlphabetService search1 = new AlphabetService();
                abc = search1.Сreate(currDir, ABC.iMatr, ABC.jMatr);
                WordService wordSearch = new WordService();
                List<string> strlist = wordSearch.Search(map, abc, Map.iMatr, Map.jMatr, ABC.iMatr, ABC.jMatr);
            //strlist.Clear();
            //strlist.Add("дорога");
             // strlist.Add("УЧЁБА");
             // strlist.Add("УЧЁБА");

             helpClass.ListWord = strlist;
             helpClass.ClientHttp.SendRequest(BasePatch.words);

            //foreach (string x in strlist)
            //  {
            //     await helpClass.ClientHttp.SendRequest(words, x);
            //  }
             helpClass.ClientHttp.GetAsync(BasePatch.stats);
             helpClass.ClientHttp.SendRequest(BasePatch.finish);
            

        }

        public async Task GetMap1()
        {
            Map frirstmap = new Map();

            //string str = "";
            string str = helpClass.ClientHttp.SendRequest(BasePatch.start).Result;
            bool[,] matrContent = TakeMatr(str, matr.iMatr, matr.jMatr).Result;
            WordInMatr(matrContent);

        }

        public async Task<bool> WordInMatr(bool[,] matr)
        {
            for (int i=0;i<5;i++)
            {
                for (int j = 0; j < 11; j++)
                {

                }
            }
            PrintMatrixArray(matr);

            return true;
        }




        //создает матрицу карты
        public async Task<bool[,]> GetMap(bool[,] map)
        {
            HttpResponseMessage response = new HttpResponseMessage();
          
            string str = "";
            bool[,] matrContent = new bool[matr.iMatr, matr.jMatr];
            bool[] StrMatrContent = new bool[matr.jMatr];
           
            // FillMatrix(map, 200, 200);
            // PrintMatrix(map, 100, 100);
            str =  helpClass.ClientHttp.SendRequest(BasePatch.start).Result;



            matrContent =  TakeMatr(str, matr.iMatr, matr.jMatr).Result;
            map =  FirstMatrInMap(map, matrContent);

            bool ind = true;
            int offsetY = 0;
            int offsetX = matr.jMatr;
           
            for (int ll=0; ll<0; ll++)
            { 
             helpClass.ClientHttp.SendRequest(BasePatch.up);
             helpClass.ClientHttp.SendRequest(BasePatch.left);
            }

           //for (int h = 0; h < 0; h++)
            for (int h = 0; h < Map.iMatr / matr.iMatr * 2-1; h++)
            {
                Console.WriteLine("идет считывание карты:" + h);
                if (h % 2 == 0)
                {
               
                    if (ind == true)
                    {
                        offsetX = matr.jMatr;
                        map =  LineMatrForMap(map, BasePatch.right, offsetY, offsetX, 1).Result;
                        ind = false;
                        offsetY = offsetY + matr.iMatr;
                    }
                    else
                    {
                        offsetX = Map.jMatr - matr.jMatr - 1;
                        map =  LineMatrForMap(map, BasePatch.left, offsetY, offsetX, 0).Result;
                        ind = true;
                        offsetY = offsetY + matr.iMatr;
                    }
                }
                else
                {
                  
                    if (ind == true)
                    {
                        map =  RowMatrForMap(map, BasePatch.down, offsetY, 0).Result;
                    }
                    else
                    {
                        map =  RowMatrForMap(map, BasePatch.down, offsetY, Map.jMatr - matr.jMatr).Result;
                    }
                }   
            }
            return map;
        }
        //посылает пост запрос
     


        //заполняет матрицу
        public async Task FillMatrix(bool[,] matr, int i, int j)
        {
            for (int i1 = 0; i1 < i; i1++)
            {
                
                for (int j1 = 0; j1 < j; j1++)
                {
                    matr[i1, j1]=true;
                }

            }

        }
        //выводит на экран матрицу
        public async Task PrintMatrixArray(bool[,] matr)
        {
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                Console.WriteLine("");
                for (int j = 0; j < matr.GetLength(1); j++)
                {

                    if (matr[i, j].ToString() == "False")
                    {

                        Console.Write("0");
                    }
                    else
                    {

                        Console.Write("x");
                    }
                }

            }

        }
      


        //получает матрицу из строки
        public async Task<bool[,]> TakeMatr(string text, int i, int j)
        {
            bool[,] matr=new bool[i,j];
            int num = 0;
            for (int i1 = 0; i1 < i; i1++)
            {
                for (int j1 = 0; j1 < j; j1++)
                {
                    if (text[num] == 49) matr[i1, j1] = true;
                    else matr[i1, j1] = false;
                    num++;
                }
            }
            return matr;
        }
        //добавляет к карте масив строк от текущего места положения по горизонтали
        public async Task<bool[,]> LineMatrForMap(bool[,] map,string Patch, int offsetY, int offsetX, int RorL)
        {
           
            bool[] StrMatrContent = new bool[matr.jMatr];
            string str = "";
            for (int i1 = 0; i1 < Map.jMatr - matr.jMatr; i1++)
            {
                
                str =  helpClass.ClientHttp.SendRequest(Patch).Result;
                StrMatrContent = TakeMatrStroffsetX(str, matr.jMatr, matr.iMatr, RorL);
                map = MapAddoffsetX(map, offsetY, StrMatrContent, matr.iMatr, offsetX);
                if (RorL==1) offsetX++;
                else offsetX--;
            }

            return map;
        }
        //добавляет к карте масив столбцов от текущего места положения вниз
        public async Task<bool[,]> RowMatrForMap(bool[,] map, string Patch, int offsetY, int offsetX)
        {
           

            bool[] StrMatrContent = new bool[matr.jMatr];
            string str = "";
            for (int i1 = 0; i1 < matr.iMatr; i1++)
            {
               
                str =  helpClass.ClientHttp.SendRequest(Patch).Result;
                StrMatrContent = TakeMatrStrDown(str, matr.iMatr, matr.jMatr);
                map = MapAddDown(map, offsetY, StrMatrContent, matr.jMatr, offsetX);
                offsetY++;
            }
            return map;

        }
        //получает строку матрицы которую нужно добавить для продвижения по вертикали
        public bool[] TakeMatrStrDown(string StrContent, int i, int j)
        {

            bool[] matr = new bool[j];
            if (StrContent.Length > i * j - 1)
            {
       
                for (int i1 = 0; i1 < j; i1++)
                {

                    if (StrContent[(i1 + i * j - j)] == 49)
                    {
                       // Console.Write(1);
                        matr[i1] = true;
                    }
                    else
                    {
                       // Console.Write(0);
                        matr[i1] = false;
                    }

                }
               // Console.WriteLine("");
            }
            return matr;
        }
        //получает столбец матрицы который нужно добавить для продвижения по горизонтали
        public bool[] TakeMatrStroffsetX(string StrContent, int i, int j,int k)
        {
            bool[] matr = new bool[j];
            if (StrContent.Length > i * j-1)
            {

                for (int i1 = 0; i1 < j; i1++)
            {

                if (StrContent[((i1+k) *i)-k] == 49)
                {
                 // Console.Write(1);
                    matr[i1] = true;
                }
                else
                {
                  //  Console.Write(0);
                    matr[i1] = false;
                }

            }
           // Console.WriteLine("");
            }
            return matr;
        }

        //добавляет к карте 1 строку от текущего местаположения вниз
        public bool[,] MapAddDown(bool[,] map,int offsetY,bool[] StrMatrContent,int j,int offsetX)
        {
            for (int i = 0; i < j; i++)
            {
                map[offsetY, offsetX + i] = StrMatrContent[i];

            }

            return map;
        }
        //добавляет к карте 1 строку от текущего местаположения влево или право
        public bool[,] MapAddoffsetX(bool[,] map, int offsetY, bool[] StrMatrContent, int j, int offsetX)
        {
          
            for (int i = 0; i < j; i++)
            {

                map[offsetY + i, offsetX] = StrMatrContent[i];

            }

            return map;
        }
        

        //заполняет 1 матрицу при старте 
        public bool[,] FirstMatrInMap(bool[,] map, bool[,] matr1)
        {
         

            for (int i = 0; i < matr.iMatr; i++)
            {
                for (int j = 0; j < matr.jMatr; j++)
                {
                    map[i, j] = matr1[i, j];
                }
            }
            return map;
        }

   



    }
}