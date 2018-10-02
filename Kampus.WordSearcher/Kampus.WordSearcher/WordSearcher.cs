
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
    class WordSearcher
    {
       
        public async Task RunAsync()
        {
            int mapi = 100;
            int mapj = 100;
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
            string token = "token b804a143-fa76-43d2-8888-bc5a7f094b57";
            bool[,] map = new bool[mapi, mapj];
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", token);
                //client.DefaultRequestHeaders.Add("test", "true");
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("UTF-8"));

                //client.DefaultRequestHeaders.Add;
               // await SendRequest(client, start, "test=true");
                map= await GetMap(client, map, mapi, mapj, matri, matrj);
                SeeMatr(map, mapi, mapj);
                List<bool[,]> abc = new List<bool[,]>();
                string currDir = Environment.CurrentDirectory.ToString() + "/abc.txt";
                LetterCreater search1 = new LetterCreater();
                abc = search1.Сreate(currDir, matriABC, matrjABC);
                WordSearch wordSearch = new WordSearch();
                List<string> strlist = wordSearch.Search(map, abc, mapi, mapj, matriABC, matrjABC);
                //strlist.Clear();
                //strlist.Add("дорога");
                foreach (string x in strlist)
                {
                    await SendRequest(client, words, x);
                }
                await GetAsync(client, stats);
                await SendRequest(client, finish, "");
            }

        }
        //создает матрицу карты
        public async Task<bool[,]> GetMap(HttpClient client,bool[,] map, int iMap, int jMap, int iMatr, int jMatr)
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
            str = await SendRequest(client, start,"");
            matrContent = await TakeMatr(str, iMatr, jMatr);
            map = await FirstMatrInMap(map, matrContent, iMatr, jMatr);

            bool ind = true;
            int sdvigVert = 0;
            int sdvigGor = jMatr;
            for(int ll=0; ll<0; ll++)
            { 
            await SendRequest(client, up, "");
            await SendRequest(client, left, "");
            }

           // for (int h = 0; h < 5; h++)
            for (int h = 0; h < iMap/iMatr*2-1; h++)
            {
                Console.WriteLine("идет считывание карты:" + h);
                if (h % 2 == 0)
                {
                    if (ind == true)
                    {
                        sdvigGor = jMatr;
                        map = await GetLine(map, client, right, iMatr, jMatr, jMap, sdvigVert, sdvigGor, 1);
                        ind = false;
                        sdvigVert = sdvigVert + iMatr;
                    }
                    else
                    {
                        sdvigGor = jMap - jMatr - 1;
                        map = await GetLine(map, client, left, iMatr, jMatr, jMap, sdvigVert, sdvigGor, 0);
                        ind = true;
                        sdvigVert = sdvigVert + iMatr;
                    }
                }
                else
                {
                    if (ind == true)
                    {
                        map = await GetRow(map, client, down, iMatr, jMatr, sdvigVert, 0);
                    }
                    else
                    {
                        map = await GetRow(map, client, down, iMatr, jMatr, sdvigVert, jMap - jMatr);
                    }
                }   
            }
            return map;
        }
        //посылает пост запрос
        public async Task<string> SendRequest(HttpClient client, string Patch, string content)
        {
            var content1 = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("test=","true")
            });
            HttpContent cont = new StringContent(content);
            HttpResponseMessage response = await client.PostAsJsonAsync(Patch, content1);
            string data = "";
            //Console.WriteLine(response.StatusCode.ToString());
            // Console.WriteLine(Patch);
            if (response.IsSuccessStatusCode) data = GetString(response.Content.ReadAsStringAsync().Result.ToString());
            else Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            if ((int)response.StatusCode==504)
            {
                Console.WriteLine("ошибка 504, по пробуем еще раз");
                data=await SendRequest(client, Patch, content);
            }
            return data;
        }
        //посылает гет запрос
        public async Task GetAsync(HttpClient client, string Patch)
        {
            HttpResponseMessage response = await client.GetAsync(Patch);
            Console.WriteLine(response.StatusCode.ToString());
            if (response.IsSuccessStatusCode)
            {
                var products = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(products);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }


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

      // оставляет в строке только цифры
        public string GetString(string text)
        {
            int[] intMatch = text.Where(Char.IsDigit).Select(x => int.Parse(x.ToString())).ToArray();
            text = string.Join("", intMatch);
            return text;
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
        public async Task<bool[,]> GetLine(bool[,] map, HttpClient client,string Patch, int iMatr, int jMatr, int jMap, int sdvigVert, int sdvigGor, int RorL)
        {
            bool[] StrMatrContent = new bool[jMatr];
            string str = "";
            for (int i1 = 0; i1 < jMap - jMatr; i1++)
            {
                str = await SendRequest(client, Patch, "");
                StrMatrContent = TakeMatrStrGorizont(str, jMatr, iMatr, RorL);
                map = MapAddGorizont(map, sdvigVert, StrMatrContent, iMatr, sdvigGor);
                if (RorL==1) sdvigGor++;
                else sdvigGor--;
            }

            return map;
        }
        //добавляет к карте масив столбцов от текущего места положения вниз
        public async Task<bool[,]> GetRow(bool[,] map, HttpClient client, string Patch, int iMatr, int jMatr, int sdvigVert, int sdvigGor)
        {
            bool[] StrMatrContent = new bool[jMatr];
            string str = "";
            for (int i1 = 0; i1 < iMatr; i1++)
            {
                str = await SendRequest(client, Patch, "");
                StrMatrContent = TakeMatrStrDown(str, iMatr, jMatr);
                map = MapAddDown(map, sdvigVert, StrMatrContent, jMatr, sdvigGor);
                sdvigVert++;
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
        public bool[,] MapAddDown(bool[,] map,int sdvigVert,bool[] StrMatrContent,int j,int sdvigGor)
        {
            for (int i = 0; i < j; i++)
            {
                map[sdvigVert, sdvigGor + i] = StrMatrContent[i];

            }

            return map;
        }
        //добавляет к карте 1 строку от текущего местаположения влево или право
        public bool[,] MapAddGorizont(bool[,] map, int sdvigVert, bool[] StrMatrContent, int j, int sdvigGor)
        {
          
            for (int i = 0; i < j; i++)
            {

                map[sdvigVert + i, sdvigGor] = StrMatrContent[i];

            }

            return map;
        }
        //добавляет к карте строку от текущего места положения влево
        public bool[,] MapAddLeft(bool[,] map, int sdvigVert, bool[] StrMatrContent, int j, int sdvigGor)
        {


            for (int i = 0; i < j; i++)
            {

                map[sdvigVert + i, sdvigGor] = StrMatrContent[j];

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