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
    class WordSearch
    {
        public List<string> Search(bool[,] map, List<bool[,]> abc,int mapi, int mapj,int matri,int matrj)
        {
            WordSearcher search2 = new WordSearcher();
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
                    masSearch=MakeArray(map, iMap, jMap, matri, matrj, mapi, mapj);
                    Letter=FindComparisonLetter(masSearch,abc);
                   // Console.Write(FindComparisonLetter(masSearch, abc));
                    if (Letter != "") {
                        if (FindComparisonWord(FindComparisonLetter(MakeArray(map, iMap, jMap+ matrj+1, matri, matrj, mapi, mapj), abc)))
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
        private static bool[,] MakeArray(bool[,] map, int iMap, int jMap, int i, int j, int mapi, int mapj)
        {

            bool[,] masSearch = new bool[i, j];
            if(iMap< mapi-i+1 && jMap< mapj-j+1)
            for (int iSearch = 0; iSearch < i; iSearch++)
            {
                for (int jSearch = 0; jSearch < j; jSearch++)
                {
                    masSearch[iSearch, jSearch] = map[iMap + iSearch, jMap + jSearch];

                }
            }

            return masSearch;
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
                    if (Convert.ToInt32(text[j]) == 48) map[k, j] = false;
                    else map[k, j] = true;
                }
                k++;
            }


            return map;
        }

    }
}