using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{
    class go
    {
        Helper helpClass = new Helper();
        ClientHttp clientHttp = new ClientHttp();
        Matrix matr = new Matrix();
        Matrix Map = new Matrix();
        Matrix ABC = new Matrix();
        Map map = new Map();
        WordService wordService = new WordService();
        MatrService matrService = new MatrService();
        int x = 0;
        int y = 0;

        public void Go(Helper helperes)
        {
            helpClass = helperes;
         
            GetMap();


        }
        public async Task GetMap()
        {
            Matrix startMap = new Matrix();
            startMap = GetFirstPart(startMap);
            int sum = 0;
            int ofsetI = 1;
            int x = 0;
            int y = 0;

            while ( true)
            {              
                //вниз
                startMap = ReadDown(startMap, 5, BasePatch.down);
                    x = x + 5;
                    //влево
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadLeft(startMap, 11, x, BasePatch.left);
                    }
                    //вверх
                    y = x - 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadLeft(startMap, 5, y, BasePatch.up);
                        y = y - 5;

                    }
                if (wordService.SeatchWord(startMap.matr) == true) break;
                //влево
                startMap = ReadLeft(startMap, 11, 0, BasePatch.left);
                    //вниз
                    y = 5;
                    for (int i = 0; i < ofsetI; i++)
                    {
                        startMap = ReadLeft(startMap, 5, y, BasePatch.down);
                        y = y + 5;
                    }
                //вниз
                if (wordService.SeatchWord(startMap.matr) == true) break;
                startMap = ReadDown(startMap, 5, BasePatch.down);
                    //вправо
                    x = x + 5;
                    for (int i = 0; i < ofsetI + 1; i++)
                    {
                        startMap = ReadRight(startMap, 11, x, BasePatch.right);
                    }
                    ofsetI = ofsetI + 2;
                if (wordService.SeatchWord(startMap.matr) == true) break;
            }

            matrService.PrintMatrix(startMap.matr);
            // matrService.PrintMatrix(startMap.matr);
            Console.WriteLine(wordService.SeatchWord(startMap.matr));
        

        }
       
        //получает минимальный кусочек карты (7x11) для нахождения в нем букв размером 7х7
        public Matrix GetFirstPart(Matrix startMap)
        {
           
            string str = helpClass.ClientHttp.SendRequest(BasePatch.start).Result;
           // str = helpClass.ClientHttp.SendRequest(BasePatch.left).Result;
           // str = helpClass.ClientHttp.SendRequest(BasePatch.left).Result;
            startMap.matr = matrService.TakeMatr(str, 5, 11);          
            //for (int i = 0; i < 2; i++)
            //{
            //    startMap= GoDown(startMap);
            //}
            matrService.PrintMatrix(startMap.matr);
            return startMap;
        }
        public Matrix ReadDown(Matrix startMap, int iMatrResult, string direction)
        {
            List<List<bool>> matrPart = new List<List<bool>>();
            //   bool[,] matrPart = new bool[5, 11];
            string str="";
            for (int i = 0; i < iMatrResult;i++)
            {
                str = helpClass.ClientHttp.SendRequest(direction).Result;
            }

            matrPart = matrService.TakeMatr(str, 5, 11);
           // matrService.PrintMatrix(matrPart);
            startMap.matr = matrService.AddMatrDown(startMap.matr, matrPart, iMatrResult);

            return startMap;
        }
        public Matrix ReadLeft(Matrix startMap, int jMatrResult, int StartJ, string direction)
        {
            List<List<bool>> matrPart = new List<List<bool>>();
            //   bool[,] matrPart = new bool[5, 11];
            string str = "";
            for (int i = 0; i < jMatrResult; i++)
            {
                str = helpClass.ClientHttp.SendRequest(direction).Result;
            }

            matrPart = matrService.TakeMatr(str, 5, 11);
            // matrService.PrintMatrix(matrPart);
            startMap.matr = matrService.AddMatrLeft(startMap.matr, matrPart, 5, StartJ);

            return startMap;
        }
        public Matrix ReadRight(Matrix startMap, int jMatrResult, int StartJ, string direction)
        {
            List<List<bool>> matrPart = new List<List<bool>>();
            //   bool[,] matrPart = new bool[5, 11];
            string str = "";
            for (int i = 0; i < jMatrResult; i++)
            {
                str = helpClass.ClientHttp.SendRequest(direction).Result;
            }

            matrPart = matrService.TakeMatr(str, 5, 11);
            // matrService.PrintMatrix(matrPart);
            startMap.matr = matrService.AddMatrRight(startMap.matr, matrPart, 5, StartJ);

            return startMap;
        }


        public Matrix GoDown(Matrix startMap)
        {
            List<List<bool>> matrPart = new List<List<bool>>();
            string str = helpClass.ClientHttp.SendRequest(BasePatch.down).Result;
            matrPart = matrService.TakeMatr(str, 5, 11);
            startMap.matr = matrService.AddMatrDown(startMap.matr, matrPart,1);

            return startMap;

        }
         public Matrix GoRight(Matrix startMap)
        {
            List<List<bool>> matrPart = new List<List<bool>>();
            string str = helpClass.ClientHttp.SendRequest(BasePatch.down).Result;
            matrPart = matrService.TakeMatr(str, 5, 11);
            startMap.matr = matrService.AddMatrDown(startMap.matr, matrPart,1);
            matrService.PrintMatrix(startMap.matr);
            return startMap;

        }
        public async Task<bool> WordInMatr(bool[,] matr)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 11; j++)
                {

                }
            }
          //  matrService.PrintMatrix(matr, 7, 7);

            return true;
        }
    }
}
