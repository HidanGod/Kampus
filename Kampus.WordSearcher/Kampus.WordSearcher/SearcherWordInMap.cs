
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
    class SearcherWordInMap
    {
        EntryPoint entryPoint = new EntryPoint();
        public async Task RunAsync(EntryPoint entryPoint1)
        {
            entryPoint = entryPoint1;
            int mapi = 300;
            int mapj = 300;
            int matriABC= 7;
            int matrjABC = 7;
            int matri = 5;
            int matrj = 11;
            string url = "http://Intern-test.skbkontur.ru:80";
            string start = "/task/game/start";
            string stats = "/task/game/stats";
            string right = "/task/move/right";
            string left = "/task/move/left";
            string up = "/task/move/up";
            string down = "/task/move/down";
            string words = "/task/words";
            string finish = "/task/game/finish";
          
            bool[,] map = new bool[mapi, mapj];
            //HttpClient client = await entryPoint.Start(url);
            Console.Write("я живой1");
            //client = await entryPoint.Start(url);
            map = await GetMap(map, mapi, mapj, matri, matrj);
            Console.Write("я живой2");
            SeeMatr(map, mapi, mapj);
                List<bool[,]> abc = new List<bool[,]>();
                string currDir = Environment.CurrentDirectory.ToString() + "/abc.txt";
                LetterCreater search1 = new LetterCreater();
                abc = search1.Сreate(currDir, matriABC, matrjABC);
                WordSearcher wordSearch = new WordSearcher();
                List<string> strlist = wordSearch.Search(map, abc, mapi, mapj, matriABC, matrjABC);
            //strlist.Clear();
            //strlist.Add("дорога");

                
                foreach (string x in strlist)
                {
                    await entryPoint.SendRequest(words, x);
                }
                await entryPoint.GetAsync(stats);
                await entryPoint.SendRequest(finish, "");
            

        }
        //создает матрицу карты
        public async Task<bool[,]> GetMap(bool[,] map, int iMap, int jMap, int iMatr, int jMatr)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string start = "/task/game/start";
            string right = "/task/move/right";
            string left = "/task/move/left";
            string up = "/task/move/up";
            string down = "/task/move/down";
            string str = "";
            bool[,] matrContent = new bool[iMatr, jMatr];
            bool[] StrMatrContent = new bool[jMatr];
           
            // MakeNewMatr(map, 200, 200);
            // seematr(map, 100, 100);
            str = await entryPoint.SendRequest(start,"");



            matrContent = await TakeMatr(str, iMatr, jMatr);
            map = await FirstMatrInMap(map, matrContent, iMatr, jMatr);

            bool ind = true;
            int offsetY = 0;
            int offsetX = jMatr;
           
            for (int ll=0; ll<0; ll++)
            { 
            await entryPoint.SendRequest(up, "");
            await entryPoint.SendRequest(left, "");
            }

           // for (int h = 0; h < 5; h++)
            for (int h = 0; h < iMap/iMatr*2-1; h++)
            {
                Console.WriteLine("идет считывание карты:" + h);
                if (h % 2 == 0)
                {
                    if (ind == true)
                    {
                        offsetX = jMatr;
                        map = await LineMatrForMap(map,  right, iMatr, jMatr, jMap, offsetY, offsetX, 1);
                        ind = false;
                        offsetY = offsetY + iMatr;
                    }
                    else
                    {
                        offsetX = jMap - jMatr - 1;
                        map = await LineMatrForMap(map,  left, iMatr, jMatr, jMap, offsetY, offsetX, 0);
                        ind = true;
                        offsetY = offsetY + iMatr;
                    }
                }
                else
                {
                    if (ind == true)
                    {
                        map = await RowMatrForMap(map,  down, iMatr, jMatr, offsetY, 0);
                    }
                    else
                    {
                        map = await RowMatrForMap(map, down, iMatr, jMatr, offsetY, jMap - jMatr);
                    }
                }   
            }
            return map;
        }
        //посылает пост запрос
     


        //заполняет матрицу
        public async Task MakeNewMatr(bool[,] matr, int i, int j)
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
        public async Task SeeMatr(bool[,] matr, int i, int j)
        {
            for (int i1 = 0; i1 < i; i1++)
            {
                Console.WriteLine("");
                for (int j1 = 0; j1 < j; j1++)
                {

                    if (matr[i1, j1].ToString() == "False")
                    { 

                    Console.Write(" ");
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
        public async Task<bool[,]> LineMatrForMap(bool[,] map,string Patch, int iMatr, int jMatr, int jMap, int offsetY, int offsetX, int RorL)
        {
            bool[] StrMatrContent = new bool[jMatr];
            string str = "";
            for (int i1 = 0; i1 < jMap - jMatr; i1++)
            {
                
                str = await entryPoint.SendRequest(Patch, "");
                StrMatrContent = TakeMatrStrGorizont(str, jMatr, iMatr, RorL);
                map = MapAddoffsetX(map, offsetY, StrMatrContent, iMatr, offsetX);
                if (RorL==1) offsetX++;
                else offsetX--;
            }

            return map;
        }
        //добавляет к карте масив столбцов от текущего места положения вниз
        public async Task<bool[,]> RowMatrForMap(bool[,] map,string Patch, int iMatr, int jMatr, int offsetY, int offsetX)
        {
            bool[] StrMatrContent = new bool[jMatr];
            string str = "";
            for (int i1 = 0; i1 < iMatr; i1++)
            {
               
                str = await entryPoint.SendRequest(Patch, "");
                StrMatrContent = TakeMatrStrDown(str, iMatr, jMatr);
                map = MapAddDown(map, offsetY, StrMatrContent, jMatr, offsetX);
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
        public bool[] TakeMatrStrGorizont(string StrContent, int i, int j,int k)
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
        public async Task<bool[,]> FirstMatrInMap(bool[,] map, bool[,] matr, int iMatr, int jMatr)
        {

            for (int i = 0; i < iMatr; i++)
            {
                for (int j = 0; j < jMatr; j++)
                {
                    map[i, j] = matr[i, j];
                }
            }
            return map;
        }






    }
}