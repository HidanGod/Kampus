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
    class WordService
    {


        public bool SeatchWord(List<List<bool>> matr)
        {
            bool[,] array = ConvertListInArray(matr);
            bool[,] arraySearch = new bool[BaseIJ.templateI, BaseIJ.templateJ];
            string Letter = "";
            AlphabetService alphabetService = new AlphabetService();
            List<bool[,]> alphabet = alphabetService.Сreate(BasePatch.patchAlphabet,BaseIJ.templateI,BaseIJ.templateJ);

            for (int i = 0; i <= array.GetLength(0) - BaseIJ.templateI; i++)
            {
                for (int j = 0; j <= array.GetLength(1) - BaseIJ.templateJ; j++)
                {
                    arraySearch = MakeArray(array, i, j, arraySearch.GetLength(0), arraySearch.GetLength(1));
                    Letter = FindComparisonLetter(arraySearch, alphabet);
                    if (Letter != "")
                    {  Console.WriteLine(i+" j="+j); return true;}

                        //MapService mapService = new MapService();
                        //Console.WriteLine("");
                        ///Console.WriteLine("буква " + Letter);
                        //Console.WriteLine("");
                        //mapService.PrintMatrix(arraySearch, arraySearch.GetLength(0), arraySearch.GetLength(1));


                    }
            }

           return false;
      
        }
        public bool[,] ConvertListInArray(List<List<bool>> matr)
        {
          
            bool[,] array = new bool[matr.Count, matr[0].Count];
            int i = 0;
            int j = 0;
            foreach (List<bool> c in matr)
            {               
                foreach (bool b in c)
                {
                    array[i, j] = b;
                   
                    j++;
                }
                j=0;
                i++;
            }
            //MapService mapService = new MapService();
           // Console.WriteLine("конверт");
           // mapService.PrintMatrix(array, matr.Count, matr[0].Count);
            return array;
        }




        public List<string> Search(bool[,] map, List<bool[,]> abc,int mapi, int mapj,int matri,int matrj)
        {
            MapService search2 = new MapService();
           // string currDir = Environment.CurrentDirectory.ToString() + "/map.txt";
          //  map=ReadMap(currDir,100,100);
           //  search2.seematr(map, 100, 100);

            List<string> masWorld = new List<string>();
            string World = "";
            string Letter = "";
            bool[,] masSearch = new bool[matri, matrj];
            for (int iMap = 0; iMap <= mapi- matri; iMap++)
            {
                for (int jMap = 0; jMap <= mapj - matrj; jMap++)
                {
                    masSearch=MakeArray(map, iMap, jMap, matri, matrj);
                    Letter=FindComparisonLetter(masSearch,abc);
                   // Console.Write(FindComparisonLetter(masSearch, abc));
                    if (Letter != "") {
                        if (FindComparisonWord(FindComparisonLetter(MakeArray(map, iMap, jMap+ matrj+1, matri, matrj), abc)))
                        {
                           World = World + Letter;
                        }
                    else if (World.Length > 0)
                    {
                            World = World + Letter;
                            masWorld.Add(World);
                            World = "";
                    }
                    }
                }


            }
            Console.WriteLine("");
            Console.WriteLine("слова");
            foreach (string x in masWorld) Console.WriteLine(x);

            Console.WriteLine("поиск завершен");



            return masWorld;

        }
        //берет матрицу 7х7 из карты для дальнейшей проверки
        private static bool[,] MakeArray(bool[,] matr, int iMatrStart, int jMatrStart, int i, int j)
        {

            bool[,] masForSearch = new bool[i, j];
            if(iMatrStart< matr.GetLength(0) - i+1 && jMatrStart< matr.GetLength(1) - j+1)
            for (int iSearch = 0; iSearch < i; iSearch++)
            {
                for (int jSearch = 0; jSearch < j; jSearch++)
                {
                    masForSearch[iSearch, jSearch] = matr[iMatrStart + iSearch, jMatrStart + jSearch];

                }
            }

            return masForSearch;
        }
        //проверяет если еще буква в этом слове
        private static bool FindComparisonWord(string letter)
        {
            bool letterbool = false;
            if(letter!="") letterbool = true;
            return letterbool;
        }
        //находит букву
        private static string FindComparisonLetter(bool[,] masSearch, List<bool[,]> abc)
        {
            string letter = "";
            int numLiter = 0;
            foreach (bool[,] mas in abc)
            {

                if (ArrayEquality(mas, masSearch))
                    letter=LiterNum(numLiter);

                numLiter++;
            }
            return letter;
        }
            //сравнивает матрицы
            private static bool ArrayEquality(bool[,] arrA, bool[,] arrB)
        {
            if (arrA.GetLength(0) != arrB.GetLength(0)) return false;
            if (arrA.GetLength(1) != arrB.GetLength(1)) return false;

            for (int i = 0; i < arrA.GetLength(0); i++)
            {
                for (int j = 0; j < arrA.GetLength(1); j++)
                {
                    if (arrA[i, j] != arrB[i, j]) return false;
                }
            }

            return true;
        }

        //определяет букву по коду
        private static string LiterNum(int num)
        {
            string leter = "";
            if(num<6) leter = ((char)(1040 + num)).ToString();
            else
                if(num==6)leter ="Ё";
            else leter = ((char)(1039 + num)).ToString();


            return leter;
        }
        //считывает матрицу из файла
        private static bool[,] ReadMap(string currDir,int iSize, int jSize)
        {
            bool[,] map = new bool[iSize, jSize];
            int k = 0;
            string text = "";
            foreach (var line in File.ReadLines(currDir))
            {

                text = line.ToString();

                for (int j = 0; j < jSize; j++)
                {
                    if (text[j] == 0) map[k, j] = false;
                    else map[k, j] = true;
                }
                k++;
            }


            return map;
        }

    }
}