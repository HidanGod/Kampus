using System;
using System.Collections.Generic;

namespace Kampus.WordSearcher
{
    class MapService
    {
        Helper helpClass = new Helper();
        ClientHttp clientHttp = new ClientHttp();
        WordService wordService = new WordService();
        MatrixService matrService = new MatrixService();

        public void Run(Helper helperes)
        {
            helpClass = helperes;
        }
            public void SeatchStartPosition()
        {
            

            wordService.start();
            Matrix startMap = new Matrix();
            Console.WriteLine("Начат поиск первой буквы на карте");
            SeatchFirstLeter();
            Console.WriteLine("Первая буква на карте найдена");
            Console.WriteLine("Начат поиск начала первого слова");
            SeatchFirstLeterWord();          
            Console.WriteLine("Начало первого слова найдено");
            Console.WriteLine("Начат поиск повторения первого слова по горизонтали");
            startMap =SeatchFirstWord();
            Console.WriteLine("Повторение первого слова по горизонтали найдено");
            Console.WriteLine("Начато считывание всей карты");
            startMap=GetMap(startMap);
            Console.WriteLine("Вся карта считана");
            helpClass.ClientHttp.GetAsync(BasePatch.stats);
            bool[,] array = wordService.ConvertListInArray(startMap.matrix);
            //List<string> listWord = new List<string>();
            Console.WriteLine("Начат поиск слов");
            helpClass.ListWord = wordService.Search(array, helpClass);
            Console.WriteLine("Все слова найдены");
            helpClass.ClientHttp.SendRequest(BasePatch.words);
            helpClass.ClientHttp.GetAsync(BasePatch.stats);
            Console.WriteLine("Набрано очков");
            Console.WriteLine(helpClass.ClientHttp.SendRequest(BasePatch.finish).Result.ToString());
        
        }
        public Matrix GetMap(Matrix map)
        {
            int indexX = 1;
            int ofsetY = 10;
            int letterSizeX = 7;
            int letterSizeY = 7;
            bool[,] baseLeter = new bool[letterSizeX, letterSizeY];
            string word = "";
            string firstWord = helpClass.ListWord[0];
            int k = (int)(map.matrix[0].Count/ 11);
           // Console.WriteLine(k);
            k = k * 11;
           // Console.WriteLine(k);
            k = map.matrix[0].Count - k;
           // Console.WriteLine(k);

        //   for (int dd=0;dd<100;dd++)
         while (true)
            {
          

          
                if (indexX + letterSizeX < map.matrix.Count)
                {
                    baseLeter = GiveLeterBase(map,0, indexX, letterSizeX, letterSizeY);
                    if (WordExist(baseLeter))
                    {
                        Console.WriteLine(WordTake(baseLeter));
                       // matrService.СreateMapFile(Environment.CurrentDirectory.ToString() + "/Resources/map.txt", map.matrix);
                        if (WordTake(baseLeter) == firstWord[0].ToString())
                        {
                            if (FirstWordDown(map, indexX, (firstWord.Length + 1) * 8) == firstWord) break;


                            // word = word + WordTake(baseLeter);
                            //indexX = indexX + letterSizeX;

                        }
                        else indexX = indexX + 5;

                    }
                    else indexX = indexX + 1;
                 
                }
                else
                {
                    map = ReadMap(map, 5, 5, ofsetY, BasePatch.down, "down");

                    //влево
                    for (int i = 0; i < (int)(((map.matrix[0].Count) - 11) / 11); i++)
                    {
                        map = ReadMap(map, 11, 5, ofsetY, BasePatch.left, "left");

                    }

                    map = ReadMap(map, k, 5, ofsetY, BasePatch.left, "left");
                    ofsetY = ofsetY + 5;
                    map = ReadMap(map, 5, 5, ofsetY, BasePatch.down, "down");
                    for (int i = 0; i < (int)(((map.matrix[0].Count) - 11) / 11); i++)
                    {
                        map = ReadMap(map, 11, 5, ofsetY, BasePatch.right, "right");

                    }
                    map = ReadMap(map, k, 5, ofsetY, BasePatch.right, "right");
                    ofsetY = ofsetY + 5;
                  

                }

            }
            matrService.СreateMapFile(Environment.CurrentDirectory.ToString() + "/Resources/map.txt", map.matrix);
             matrService.PrintMatrix(map.matrix);
            return map;
        }
        public string FirstWordDown(Matrix map,int indexX, int Length)
        {
            List<List<bool>> FistWordList = new List<List<bool>>();
            for (int i = 0; i < 7; i++)
            {
                FistWordList.Add(map.matrix[indexX + i]);
                FistWordList[i].RemoveRange(FistWordList[i].Count - Length, Length);
            }
            bool[,] FistWordArray = wordService.ConvertListInArray(FistWordList);
            List<string> wordList = wordService.Search(FistWordArray, helpClass);
           // matrService.PrintMatrixArray(FistWordArray);
            return wordList[0];
        }
            //ищет 1 слово и его повторение
        public Matrix SeatchFirstWord()
        {
         

            Matrix firstLine = new Matrix();

            helpClass.ClientHttp.SendRequest(BasePatch.right);
            firstLine = GetFirstPartLeter(firstLine, BasePatch.left);

            int indexY = 0;
            int letterSizeX = 7;
            int letterSizeY = 7;
            bool[,] baseLeter = new bool[letterSizeX, letterSizeY];
            string word = "";
            firstLine = ReadMap(firstLine, 5, 5, 0, BasePatch.down, "down");
            while (true)
            {
                if (indexY + letterSizeY + 1 < firstLine.matrix[0].Count)
                {
                    baseLeter = GiveLeterBase(firstLine, indexY,0, letterSizeX, letterSizeY);

                    if (WordExist(baseLeter))
                    {
                        word = word + WordTake(baseLeter);
                        indexY = indexY + letterSizeY+1;
                    }
                    else
                    {
                       
                        if (word!="")
                            if (helpClass.ListWord.Count > 0)
                                if (word == helpClass.ListWord[0])
                                {
                                    int x = 10;
                                    int y = word.Length * 8 + (firstLine.matrix[0].Count - indexY);
                                    Navigation(y, 0);
                                   // matrService.PrintMatrix(firstLine.matr);
                                    //Console.WriteLine("");
                                    DeletWord(firstLine.matrix,x, firstLine.matrix[0].Count-y, y);
                                    break;
                                }
                                else
                                {
                                    helpClass.ListWord.Add(word);
                                    word = "";
                                }
                            else
                            {
                                helpClass.ListWord.Add(word);
                                word = "";
                            }
                        indexY = indexY + 1;
                    }
                   
                }
                else
                {
                    firstLine = ReadMap(firstLine, 11, 5, 5, BasePatch.right, "right");
                    firstLine = ReadMap(firstLine, 5, 5, 0, BasePatch.up, "right");
                   firstLine = ReadMap(firstLine, 11, 5, 0, BasePatch.right, "right");
                   firstLine = ReadMap(firstLine, 5, 5, 5, BasePatch.down, "right");


                   // helpClass.ClientHttp.GetAsync(BasePatch.stats);
                    //matrService.PrintMatrix(firstLine.matr);
                    //Console.Read();
                }
            }
            // matrService.СreateMapFile(Environment.CurrentDirectory.ToString() + "/Resources/map.txt", firstLine.matrix);
            // matrService.PrintMatrix(firstLine.matr);


            //firstLine.matr.Clear();
            //helpClass.ClientHttp.GetAsync(BasePatch.stats);
            //helpClass.ClientHttp.SendRequest(BasePatch.right);
            //firstLine = GetFirstPartLeter(firstLine, BasePatch.right);
         
            return firstLine;
        }
        //удаляет слово из карты
        public List<List<bool>> DeletWord(List<List<bool>> firstLine, int x, int y,int count)
        {
            for (int i = 0; i < x; i++)
            {
                List<bool> lineList = firstLine[i];
                                
                    lineList.RemoveRange(y, count);
                
                firstLine[i] = lineList;
            }
            return firstLine;
        }
        //получает шаблон буквы из карты
        public bool[,] GiveLeterBase(Matrix firstLine, int indexY, int indexX, int letterSizeX,int letterSizeY)
        {
            bool[,] baseLeter = new bool[letterSizeX, letterSizeY];
            for (int i = 0; i < letterSizeX; i++)
            {
                List<bool> lineList = firstLine.matrix[i+ indexX];
                for (int j = 0; j < letterSizeY; j++)
                {
                    baseLeter[i, j] = lineList[j + indexY];
                }
            }
          ///  Console.WriteLine("");
           // matrService.PrintMatrixArray(baseLeter);
            return baseLeter;
        }

        //ищет 1 букву в слове
      public void SeatchFirstLeterWord()
        {
            string direction = BasePatch.down;
            bool[,] startWord = GetFirstPartWord(8, 2, direction);
            //Console.WriteLine("");
            while (WordExist(startWord))
           //for (int i =0;i<3;i++)
            {
                if (direction == BasePatch.down)
                {
                    direction = BasePatch.up;
                    startWord = GetFirstPartWord(8, 2, direction);
                }
                else
                {
                    direction = BasePatch.down;
                    startWord = GetFirstPartWord(8, 2, direction);
                }

                //Console.WriteLine("");
            }
            if (direction == BasePatch.down) Navigation(-8, 2);
            else Navigation(-8, 0);
            //Matrix startMap = new Matrix();
            //helpClass.ClientHttp.SendRequest(BasePatch.right);
            //GetFirstPartLeter(startMap, BasePatch.left);
            //return startMap;
        }
        //ищет 1 букву на карте
        public void SeatchFirstLeter()
        {
            Matrix startMap = new Matrix();
            startMap = GetFirstPartLeter(startMap,BasePatch.start);
            int sum = 0;
            int ofsetI = 1;
            int x = 0;
            int y = 0;

            while ( true)
            {              
                //вниз
                startMap = ReadMap(startMap, 5,5, x, BasePatch.down, "down");
                    x = x + 5;
                    //влево
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 11, 5, x, BasePatch.left, "left");
                    }
                    //вверх
                    y = x - 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 5, 5, y, BasePatch.up, "left");
                        y = y - 5;

                    }
                if (LeterExist(startMap.matrix) == true) break;
                //влево
                startMap = ReadMap(startMap, 11, 5, 0, BasePatch.left, "left");
                    //вниз
                    y = 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 5,5, y, BasePatch.down, "left");
                        y = y + 5;
                    }
                //вниз
                if (LeterExist(startMap.matrix) == true) break;
                startMap = ReadMap(startMap, 5, 5, x, BasePatch.down, "down");
                //вправо
                x = x + 5;
                    for (int i = 0; i < ofsetI + 1; i++)
                    {
                        startMap = ReadMap(startMap, 11, 5, x, BasePatch.right,"right");
                    }
                    ofsetI = ofsetI + 2;
                if (LeterExist(startMap.matrix) == true) break;
            }
           // matrService.PrintMatrix(startMap.matrix);
            int[] ofsetxy = wordService.SeatchWord(startMap.matrix);
            x = startMap.matrix[0].Count - 11- startMap.iMatrix- ofsetxy[1];
            y = startMap.jMatrix - ofsetxy[0];
            Navigation(x, y);
            startMap.matrix.Clear();
            helpClass.ClientHttp.SendRequest(BasePatch.right);
            GetFirstPartLeter(startMap, BasePatch.left);
        }

        //проверяет наличие букв на карте
        public bool LeterExist(List<List<bool>> startMap)
        {
            int[] ofsetxy = wordService.SeatchWord(startMap);
            if(ofsetxy[0]==-1) return false;
            return true;
        }
        //проверяет наличие слов на карте
        public bool WordExist(bool[,] startMap)
        {
            string leter = WordTake(startMap);
            if (leter == "") return false;
            return true;
        }
        //получает букву
        public string WordTake(bool[,] startMap)
        {
            AlphabetService alphabetService = new AlphabetService();

            List<bool[,]> alphabet = alphabetService.Сreate(BaseIJ.templateI, BaseIJ.templateJ);

            string leter = "";
            leter = wordService.FindComparisonLetter(startMap);
  
            return leter;
        }
        //перемещает указатель на указаные координаты
        public void Navigation(int x, int y)
        {
            for (int i = 0; i < Math.Abs(x); i++)
            {
                if (x>0) helpClass.ClientHttp.SendRequest(BasePatch.left);
                else helpClass.ClientHttp.SendRequest(BasePatch.right);
            }
            for (int i = 0; i < Math.Abs(y); i++)
            {
                if (y > 0) helpClass.ClientHttp.SendRequest(BasePatch.up);
                else helpClass.ClientHttp.SendRequest(BasePatch.down);
            }
        }
            //получает получает 1 матрицу 5х11
      public Matrix GetFirstPartLeter(Matrix startMap,string patch)
      {          
            string str = helpClass.ClientHttp.SendRequest(patch).Result;
            startMap.matrix = matrService.TakeMatr(str, 5, 11);          
            //matrService.PrintMatrix(startMap.matrix);
            return startMap;
      }
        //ищет 1 букву 1 слова
      public bool[,] GetFirstPartWord(int x, int y, string direction)
        {
            bool[,] matrPart = new bool[5, 11];
            bool[,] matrMapPart = new bool[7,7];
            matrService.FillMatrix(matrMapPart);
            string str = "";
            for (int i = 0; i < (x-1); i++)
            {
               helpClass.ClientHttp.SendRequest(BasePatch.left);
            }
            
            str = helpClass.ClientHttp.SendRequest(BasePatch.left).Result;

            matrPart = matrService.TakeMatrArray(str, matrPart);
            if (direction == BasePatch.down)
                matrMapPart = matrService.CopyArrayFull(matrMapPart, matrPart,0);
            else
                matrMapPart = matrService.CopyArrayFull(matrMapPart, matrPart, 2);
            for (int i = 0; i < y; i++)
            {
                str=helpClass.ClientHttp.SendRequest(direction).Result;
                matrPart = matrService.TakeMatrArray(str, matrPart);
                if(direction==BasePatch.down)
                    matrMapPart = matrService.AddArrayDown(matrMapPart, matrPart, 5+i);
                else
                    matrMapPart = matrService.AddArrayUp(matrMapPart, matrPart, y-1 - i);
            }
           
            //matrService.PrintMatrixArray(matrMapPart);
            return matrMapPart;
        }
        //читает карту в любом направлении
        public Matrix ReadMap(Matrix startMap, int iRow, int jColumn, int StartJ, string patch, string direction)
        {
            List<List<bool>> matrPart = new List<List<bool>>();
            string str = "";
            for (int i = 0; i < iRow; i++)
            {
                str = helpClass.ClientHttp.SendRequest(patch).Result;
               // Console.WriteLine(str);
            }
            
            matrPart = matrService.TakeMatr(str, 5, 11);
            switch (direction)
            {
                case "down":
                    startMap.matrix = matrService.AddMatrDown(startMap.matrix, matrPart, iRow);
                    CalcOfset(startMap, iRow, patch);
                    break;
                case "left":
                    startMap.matrix = matrService.AddMatrLeft(startMap.matrix, matrPart, jColumn, StartJ, 11-iRow, patch);
                    CalcOfset(startMap, iRow, patch);
                    break;
                case "right":  
                    startMap.matrix = matrService.AddMatrRight(startMap.matrix, matrPart, jColumn, StartJ, 11-iRow, patch);
                    CalcOfset(startMap, iRow, patch);
                    break;
            }
            return startMap;
        }

      
            //считает сдвиг координат 
            public void CalcOfset(Matrix startMap, int ofset, string patch)
        {
            switch (patch)
            {
                case BasePatch.up:
                    startMap.jMatrix = startMap.jMatrix - ofset;
                    break;
                case BasePatch.down:
                    startMap.jMatrix = startMap.jMatrix + ofset;
                    break;
                case BasePatch.left:
                    startMap.iMatrix = startMap.iMatrix + ofset;
                    break;
                case BasePatch.right:
                    startMap.iMatrix = startMap.iMatrix - ofset;
                    break;

            }
        }



        }
}
