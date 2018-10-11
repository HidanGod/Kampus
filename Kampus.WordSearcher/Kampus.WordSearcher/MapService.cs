using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task Seatch(CancellationToken token)
        {
          
            wordService.start();
            Matrix startMap = new Matrix();
            SeatchFirstLeter();
            SeatchFirstLeterWord();          
            startMap =SeatchFirstWord();
            startMap=GetMap(startMap, token);
            bool[,] array = wordService.ConvertListInArray(startMap.MatrixMase);
            helpClass.ListWord = wordService.Search(array, helpClass);
            helpClass.ClientHttp.SendRequest(BasePatch.Words);
            Console.WriteLine("Игровая статистика:");
            helpClass.ClientHttp.GetAsync(BasePatch.Stats);
            helpClass.ClientHttp.SendRequest(BasePatch.Finish).Result.ToString();
            Console.WriteLine("Finish");
        }
        public Matrix GetMap(Matrix map, CancellationToken token)
        {
            int indexX = 1;
            int ofsetY = 10;
            int letterSizeX = 7;
            int letterSizeY = 7;
            bool[,] baseLeter = new bool[letterSizeX, letterSizeY];
            string word = "";
            string firstWord = helpClass.ListWord[0];
            int k = (int)(map.MatrixMase[0].Count/ 11);
            k = k * 11;
            k = map.MatrixMase[0].Count - k;
            try
            {

                while (true)
                {
                  
                    if (indexX + letterSizeX < map.MatrixMase.Count)
                    {
                        if (token.IsCancellationRequested) break;
                        baseLeter = GiveLeterBase(map, 0, indexX, letterSizeX, letterSizeY);
                        if (WordExist(baseLeter))
                        {
                            if (wordService.FindComparisonLetter(baseLeter) == firstWord[0].ToString())
                            {
                                if (FirstWordDown(map, indexX, (firstWord.Length + 1) * 8) == firstWord) break;
                            }
                            else indexX = indexX + 5;
                        }
                        else indexX = indexX + 1;
                    }
                    else
                    {
                        if (token.IsCancellationRequested) break;
                        map = ReadMap(map, 5, 5, ofsetY, BasePatch.Down, "down");
                        //влево
                        for (int i = 0; i < (int)(((map.MatrixMase[0].Count) - 11) / 11); i++)
                        {
                            map = ReadMap(map, 11, 5, ofsetY, BasePatch.Left, "left");
                        }
                        map = ReadMap(map, k, 5, ofsetY, BasePatch.Left, "left");
                        ofsetY = ofsetY + 5;
                        if (token.IsCancellationRequested) break;
                        //вправо
                        map = ReadMap(map, 5, 5, ofsetY, BasePatch.Down, "down");
                        for (int i = 0; i < (int)(((map.MatrixMase[0].Count) - 11) / 11); i++)
                        {
                            map = ReadMap(map, 11, 5, ofsetY, BasePatch.Right, "right");
                        }
                        map = ReadMap(map, k, 5, ofsetY, BasePatch.Right, "right");
                        ofsetY = ofsetY + 5;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

            }
            //matrService.СreateMapFile(Environment.CurrentDirectory.ToString() + "/Resources/map.txt", map.MatrixMase);
            return map;
        }
        public string FirstWordDown(Matrix map,int indexX, int Length)
        {
            List<List<bool>> FistWordList = new List<List<bool>>();
            for (int i = 0; i < 7; i++)
            {
                FistWordList.Add(map.MatrixMase[indexX + i]);
                FistWordList[i].RemoveRange(FistWordList[i].Count - Length, Length);
            }
            bool[,] FistWordArray = wordService.ConvertListInArray(FistWordList);
            List<string> wordList = wordService.Search(FistWordArray, helpClass);
            return wordList[0];
        }
        //ищет 1 слово и его повторение
        public Matrix SeatchFirstWord()
        {       
            Matrix firstLine = new Matrix();
            helpClass.ClientHttp.SendRequest(BasePatch.Right);
            firstLine = GetFirstPartLeter(firstLine, BasePatch.Left);
            int indexY = 0;
            int letterSizeX = 7;
            int letterSizeY = 7;
            bool[,] baseLeter = new bool[letterSizeX, letterSizeY];
            string word = "";
            try
            {
                firstLine = ReadMap(firstLine, 5, 5, 0, BasePatch.Down, "down");

                while (true)
                {
                    if (indexY + letterSizeY + 1 < firstLine.MatrixMase[0].Count)
                    {
                        baseLeter = GiveLeterBase(firstLine, indexY, 0, letterSizeX, letterSizeY);
                        if (WordExist(baseLeter))
                        {
                            word = word + wordService.FindComparisonLetter(baseLeter);
                            indexY = indexY + letterSizeY + 1;
                        }
                        else
                        {
                            if (word != "")
                                if (helpClass.ListWord.Count > 0)
                                    if (word == helpClass.ListWord[0])
                                    {
                                        int x = 10;
                                        int y = word.Length * 8 + (firstLine.MatrixMase[0].Count - indexY);
                                        Navigation(y, 0);
                                        DeletWord(firstLine.MatrixMase, x, firstLine.MatrixMase[0].Count - y, y);
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
                        firstLine = ReadMap(firstLine, 11, 5, 5, BasePatch.Right, "right");
                        firstLine = ReadMap(firstLine, 5, 5, 0, BasePatch.Up, "right");
                        firstLine = ReadMap(firstLine, 11, 5, 0, BasePatch.Right, "right");
                        firstLine = ReadMap(firstLine, 5, 5, 5, BasePatch.Down, "right");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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
                List<bool> lineList = firstLine.MatrixMase[i+ indexX];
                for (int j = 0; j < letterSizeY; j++)
                {
                    baseLeter[i, j] = lineList[j + indexY];
                }
            }
            return baseLeter;
        }
        //ищет 1 букву в слове
      public void SeatchFirstLeterWord()
        {
            string direction = BasePatch.Down;
            bool[,] startWord = GetFirstPartWord(8, 2, direction);
            while (WordExist(startWord))
            {
                if (direction == BasePatch.Down)
                {
                    direction = BasePatch.Up;
                    startWord = GetFirstPartWord(8, 2, direction);
                }
                else
                {
                    direction = BasePatch.Down;
                    startWord = GetFirstPartWord(8, 2, direction);
                }
            }
            if (direction == BasePatch.Down) Navigation(-8, 2);
            else Navigation(-8, 0);
        }
        //ищет 1 букву на карте
        public void SeatchFirstLeter()
        {
            Matrix startMap = new Matrix();
            startMap = GetFirstPartLeter(startMap,BasePatch.Start);
            int sum = 0;
            int ofsetI = 1;
            int x = 0;
            int y = 0;
            while ( true)
            {              
                //вниз
                startMap = ReadMap(startMap, 5,5, x, BasePatch.Down, "down");
                    x = x + 5;
                    //влево
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 11, 5, x, BasePatch.Left, "left");
                    }
                    //вверх
                    y = x - 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 5, 5, y, BasePatch.Up, "left");
                        y = y - 5;

                    }
                if (LeterExist(startMap.MatrixMase) == true) break;
                //влево
                startMap = ReadMap(startMap, 11, 5, 0, BasePatch.Left, "left");
                    //вниз
                    y = 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 5,5, y, BasePatch.Down, "left");
                        y = y + 5;
                    }
                //вниз
                if (LeterExist(startMap.MatrixMase) == true) break;
                startMap = ReadMap(startMap, 5, 5, x, BasePatch.Down, "down");
                //вправо
                x = x + 5;
                    for (int i = 0; i < ofsetI + 1; i++)
                    {
                        startMap = ReadMap(startMap, 11, 5, x, BasePatch.Right,"right");
                    }
                    ofsetI = ofsetI + 2;
                if (LeterExist(startMap.MatrixMase) == true) break;
            }
            int[] ofsetxy = wordService.SeatchWord(startMap.MatrixMase);
            x = startMap.MatrixMase[0].Count - 11- startMap.IMatrix- ofsetxy[1];
            y = startMap.JMatrix - ofsetxy[0];
            Navigation(x, y);
            startMap.MatrixMase.Clear();
            helpClass.ClientHttp.SendRequest(BasePatch.Right);
            GetFirstPartLeter(startMap, BasePatch.Left);
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
            string leter = wordService.FindComparisonLetter(startMap);
            if (leter == "") return false;
            return true;
        }
     
        //перемещает указатель на указаные координаты
        public void Navigation(int x, int y)
        {
            for (int i = 0; i < Math.Abs(x); i++)
            {
                if (x>0) helpClass.ClientHttp.SendRequest(BasePatch.Left);
                else helpClass.ClientHttp.SendRequest(BasePatch.Right);
            }
            for (int i = 0; i < Math.Abs(y); i++)
            {
                if (y > 0) helpClass.ClientHttp.SendRequest(BasePatch.Up);
                else helpClass.ClientHttp.SendRequest(BasePatch.Down);
            }
        }
      //получает получает 1 матрицу 5х11
      public Matrix GetFirstPartLeter(Matrix startMap,string patch)
      {          
            string str = helpClass.ClientHttp.SendRequest(patch).Result;
            startMap.MatrixMase = matrService.TakeMatr(str, 5, 11);          
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
               helpClass.ClientHttp.SendRequest(BasePatch.Left);
            }
            str = helpClass.ClientHttp.SendRequest(BasePatch.Left).Result;
            matrPart = matrService.TakeMatrArray(str, matrPart);
            if (direction == BasePatch.Down)
                matrMapPart = matrService.CopyArrayFull(matrMapPart, matrPart,0);
            else
                matrMapPart = matrService.CopyArrayFull(matrMapPart, matrPart, 2);
            for (int i = 0; i < y; i++)
            {
                str=helpClass.ClientHttp.SendRequest(direction).Result;
                matrPart = matrService.TakeMatrArray(str, matrPart);
                if(direction==BasePatch.Down)
                    matrMapPart = matrService.AddArrayDown(matrMapPart, matrPart, 5+i);
                else
                    matrMapPart = matrService.AddArrayUp(matrMapPart, matrPart, y-1 - i);
            }
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
            }
            matrPart = matrService.TakeMatr(str, 5, 11);
            switch (direction)
            {
                case "down":
                    startMap.MatrixMase = matrService.AddMatrDown(startMap.MatrixMase, matrPart, iRow);
                    CalcOfset(startMap, iRow, patch);
                    break;
                case "left":
                    startMap.MatrixMase = matrService.AddMatrLeft(startMap.MatrixMase, matrPart, jColumn, StartJ, 11-iRow, patch);
                    CalcOfset(startMap, iRow, patch);
                    break;
                case "right":  
                    startMap.MatrixMase = matrService.AddMatrRight(startMap.MatrixMase, matrPart, jColumn, StartJ, 11-iRow, patch);
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
                case BasePatch.Up:
                    startMap.JMatrix = startMap.JMatrix - ofset;
                    break;
                case BasePatch.Down:
                    startMap.JMatrix = startMap.JMatrix + ofset;
                    break;
                case BasePatch.Left:
                    startMap.IMatrix = startMap.IMatrix + ofset;
                    break;
                case BasePatch.Right:
                    startMap.IMatrix = startMap.IMatrix - ofset;
                    break;

            }
        }

    }
}
