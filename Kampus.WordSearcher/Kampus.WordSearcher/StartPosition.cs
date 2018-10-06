using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{
    class StartPosition
    {
        Helper helpClass = new Helper();
        ClientHttp clientHttp = new ClientHttp();
        WordService wordService = new WordService();
        MatrService matrService = new MatrService();

        public void Run(Helper helperes)
        {
            helpClass = helperes;
        }
            public void SeatchStartPosition()
        {
            Matrix startMap = new Matrix();
            SeatchFirstLeter();
            SeatchFirstLeterWord();
            SeatchFirstWord();

        }
        public void SeatchFirstWord()
        {
            Console.WriteLine("");
            Matrix firstLine = new Matrix();
            helpClass.ClientHttp.SendRequest(BasePatch.right);
            firstLine = GetFirstPartLeter(firstLine, BasePatch.left);

            firstLine = ReadMap(firstLine, 5, 0, BasePatch.down, "down");

            for (int i=0;i<10;i++)
            { 

            firstLine = ReadMap(firstLine, 11, 5, BasePatch.right, "right");
            firstLine = ReadMap(firstLine, 5, 0, BasePatch.up, "right");

            firstLine = ReadMap(firstLine, 11, 0, BasePatch.right, "right");
            firstLine = ReadMap(firstLine, 5, 5, BasePatch.down, "right");
            }
            matrService.СreateMapFile(BasePatch.mapFile, firstLine.matr);
            matrService.PrintMatrix(firstLine.matr);
           // helpClass.ClientHttp.GetAsync(BasePatch.stats);
            //}

        }
        public void SeatchLeterFirstWord(Matrix firstLine)
        {





        }



            public void SeatchFirstLeterWord()
        {
            string direction = BasePatch.down;
            bool[,] startWord = GetFirstPartWord(8, 2, direction);
            Console.WriteLine("");
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

                Console.WriteLine("");
            }
            if (direction == BasePatch.down) Navigation(-8, 2);
            else Navigation(-8, 0);
            //Matrix startMap = new Matrix();
            //helpClass.ClientHttp.SendRequest(BasePatch.right);
            //GetFirstPartLeter(startMap, BasePatch.left);
            //return startMap;
        }
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
                startMap = ReadMap(startMap, 5, x, BasePatch.down, "down");
                    x = x + 5;
                    //влево
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 11, x, BasePatch.left, "left");
                    }
                    //вверх
                    y = x - 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 5, y, BasePatch.up, "left");
                        y = y - 5;

                    }
                if (LeterExist(startMap.matr) == true) break;
                //влево
                startMap = ReadMap(startMap, 11, 0, BasePatch.left, "left");
                    //вниз
                    y = 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadMap(startMap, 5, y, BasePatch.down, "left");
                        y = y + 5;
                    }
                //вниз
                if (LeterExist(startMap.matr) == true) break;
                startMap = ReadMap(startMap, 5, x, BasePatch.down, "down");
                //вправо
                x = x + 5;
                    for (int i = 0; i < ofsetI + 1; i++)
                    {
                        startMap = ReadMap(startMap, 11, x, BasePatch.right,"right");
                    }
                    ofsetI = ofsetI + 2;
                if (LeterExist(startMap.matr) == true) break;
            }
            matrService.PrintMatrix(startMap.matr);
            int[] ofsetxy = wordService.SeatchWord(startMap.matr);
            x = startMap.matr[0].Count - 11- startMap.iMatr- ofsetxy[1];
            y = startMap.jMatr - ofsetxy[0];
            Navigation(x, y);
            startMap.matr.Clear();
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
            AlphabetService alphabetService = new AlphabetService();

            List<bool[,]> alphabet = alphabetService.Сreate(BasePatch.patchAlphabet, BaseIJ.templateI, BaseIJ.templateJ);
            
            string leter = "";
            leter = wordService.FindComparisonLetter(startMap, alphabet);
            if (leter == "") return false;
            return true;
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
            startMap.matr = matrService.TakeMatr(str, 5, 11);          
            matrService.PrintMatrix(startMap.matr);
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
           
            matrService.PrintMatrixArray(matrMapPart);
            return matrMapPart;
        }
      public Matrix ReadMap(Matrix startMap, int iMatrResult, int StartJ, string patch, string direction)
        {
            List<List<bool>> matrPart = new List<List<bool>>();
            string str = "";
            for (int i = 0; i < iMatrResult; i++)
            {
                str = helpClass.ClientHttp.SendRequest(patch).Result;
               // Console.WriteLine(str);
            }
            matrPart = matrService.TakeMatr(str, 5, 11);
            switch (direction)
            {
                case "down":
                    startMap.matr = matrService.AddMatrDown(startMap.matr, matrPart, iMatrResult);
                    CalcOfset(startMap, iMatrResult, patch);
                    break;
                case "left":
                    startMap.matr = matrService.AddMatrLeft(startMap.matr, matrPart, 5, StartJ);
                    CalcOfset(startMap, iMatrResult, patch);
                    break;
                case "right":
                    startMap.matr = matrService.AddMatrRight(startMap.matr, matrPart, 5, StartJ);
                    CalcOfset(startMap, iMatrResult, patch);
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
                    startMap.jMatr = startMap.jMatr - ofset;
                    break;
                case BasePatch.down:
                    startMap.jMatr = startMap.jMatr + ofset;
                    break;
                case BasePatch.left:
                    startMap.iMatr = startMap.iMatr + ofset;
                    break;
                case BasePatch.right:
                    startMap.iMatr = startMap.iMatr - ofset;
                    break;

            }
        }



        }
}
