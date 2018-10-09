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
        public async Task SeatchStartPosition(CancellationToken token)
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
            startMap=GetMap(startMap, token);
            Console.WriteLine("Вся карта считана");
            helpClass.ClientHttp.GetAsync(BasePatch.Stats);
            bool[,] array = wordService.ConvertListInArray(startMap.matrix);
            //List<string> listWord = new List<string>();
            Console.WriteLine("Начат поиск слов");
            helpClass.ListWord = wordService.Search(array, helpClass);
            Console.WriteLine("Все слова найдены");
            helpClass.ClientHttp.SendRequest(BasePatch.Words);
            helpClass.ClientHttp.GetAsync(BasePatch.Stats);
            Console.WriteLine("Набрано очков");
            Console.WriteLine(helpClass.ClientHttp.SendRequest(BasePatch.Finish).Result.ToString());
        
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
            int k = (int)(map.matrix[0].Count/ 11);
            k = k * 11;
            k = map.matrix[0].Count - k;
         while (true)
            {
                if (token.IsCancellationRequested)break;

                if (indexX + letterSizeX < map.matrix.Count)
                {
                    baseLeter = GiveLeterBase(map,0, indexX, letterSizeX, letterSizeY);
                    if (WordExist(baseLeter))
                    {
                        if (WordTake(baseLeter) == firstWord[0].ToString())
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
                    for (int i = 0; i < (int)(((map.matrix[0].Count) - 11) / 11); i++)
                    {
                        map = ReadMap(map, 11, 5, ofsetY, BasePatch.Left, "left");
                    }
                    map = ReadMap(map, k, 5, ofsetY, BasePatch.Left, "left");
                    ofsetY = ofsetY + 5;
                    if (token.IsCancellationRequested) break;
                    //вправо
                    map = ReadMap(map, 5, 5, ofsetY, BasePatch.Down, "down");
                    for (int i = 0; i < (int)(((map.matrix[0].Count) - 11) / 11); i++)
                    {
                        map = ReadMap(map, 11, 5, ofsetY, BasePatch.Right, "right");
                    }
                    map = ReadMap(map, k, 5, ofsetY, BasePatch.Right, "right");
                    ofsetY = ofsetY + 5;
                }
            }
            matrService.СreateMapFile(Environment.CurrentDirectory.ToString() + "/Resources/map.txt", map.matrix);
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
            firstLine = ReadMap(firstLine, 5, 5, 0, BasePatch.Down, "down");
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
                   firstLine = ReadMap(firstLine, 11, 5, 5, BasePatch.Right, "right");
                   firstLine = ReadMap(firstLine, 5, 5, 0, BasePatch.Up, "right");
                   firstLine = ReadMap(firstLine, 11, 5, 0, BasePatch.Right, "right");
                   firstLine = ReadMap(firstLine, 5, 5, 5, BasePatch.Down, "right");
                }
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
                List<bool> lineList = firstLine.matrix[i+ indexX];
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
                if (LeterExist(startMap.matrix) == true) break;
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
                if (LeterExist(startMap.matrix) == true) break;
                startMap = ReadMap(startMap, 5, 5, x, BasePatch.Down, "down");
                //вправо
                x = x + 5;
                    for (int i = 0; i < ofsetI + 1; i++)
                    {
                        startMap = ReadMap(startMap, 11, 5, x, BasePatch.Right,"right");
                    }
                    ofsetI = ofsetI + 2;
                if (LeterExist(startMap.matrix) == true) break;
            }
            int[] ofsetxy = wordService.SeatchWord(startMap.matrix);
            x = startMap.matrix[0].Count - 11- startMap.iMatrix- ofsetxy[1];
            y = startMap.jMatrix - ofsetxy[0];
            Navigation(x, y);
            startMap.matrix.Clear();
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
            string leter = WordTake(startMap);
            if (leter == "") return false;
            return true;
        }
        //получает букву
        public string WordTake(bool[,] startMap)
        {
            AlphabetService alphabetService = new AlphabetService();
            List<bool[,]> alphabet = alphabetService.Сreate(BaseIJ.TemplateI, BaseIJ.TemplateJ);
            string leter = "";
            leter = wordService.FindComparisonLetter(startMap);
            return leter;
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
            startMap.matrix = matrService.TakeMatr(str, 5, 11);          
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
                case BasePatch.Up:
                    startMap.jMatrix = startMap.jMatrix - ofset;
                    break;
                case BasePatch.Down:
                    startMap.jMatrix = startMap.jMatrix + ofset;
                    break;
                case BasePatch.Left:
                    startMap.iMatrix = startMap.iMatrix + ofset;
                    break;
                case BasePatch.Right:
                    startMap.iMatrix = startMap.iMatrix - ofset;
                    break;

            }
        }

    }
}
