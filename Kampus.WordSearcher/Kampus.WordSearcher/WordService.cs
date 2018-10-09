using System.Collections.Generic;
using System.IO;


namespace Kampus.WordSearcher
{
    class WordService
    {
        AlphabetService alphabetService = new AlphabetService();
        Helper helpClass { get; set; }
        bool[,] map { get; set; }
        List<bool[,]> abc { get; set; }

        public void start()
        {
          abc = alphabetService.Сreate(BaseIJ.TemplateI, BaseIJ.TemplateJ);
        }

        public int[] SeatchWord(List<List<bool>> matr)
        {
            int[] ofset = new int[2];
            ofset[0] = -1;
            bool[,] array = ConvertListInArray(matr);
            bool[,] arraySearch = new bool[BaseIJ.TemplateI, BaseIJ.TemplateJ];
            string Letter = "";
            for (int i = 0; i <= array.GetLength(0) - BaseIJ.TemplateI; i++)
            {
                for (int j = 0; j <= array.GetLength(1) - BaseIJ.TemplateJ; j++)
                {
                    arraySearch = MakeArray(array, i, j, arraySearch.GetLength(0), arraySearch.GetLength(1));
                    Letter = FindComparisonLetter(arraySearch);
                    if (Letter != "")
                    {
                        ofset[0] = i; ofset[1] = j;
                        return ofset;
                    }
                }
            }

           return ofset;
      
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
            return array;
        }

        public List<string> Search(bool[,] newMap, Helper helperes)
        {
            helpClass = helperes;
            map = newMap;
            MapService search2 = new MapService();
            List<bool[,]> abc = alphabetService.Сreate(BaseIJ.TemplateI, BaseIJ.TemplateJ);
            List<string> masWorld = new List<string>();
            string World = "";
            string Letter = "";
            bool[,] masSearch = new bool[BaseIJ.TemplateI, BaseIJ.TemplateJ];
            for (int iMap = 0; iMap <= map.GetLength(0) - BaseIJ.TemplateI; iMap++)
            {
                for (int jMap = 0; jMap <= map.GetLength(1) - BaseIJ.TemplateJ; jMap++)
                {
                    masSearch=MakeArray(map, iMap, jMap, BaseIJ.TemplateI, BaseIJ.TemplateJ);
                    Letter=FindComparisonLetter(masSearch);
                   // Console.Write(FindComparisonLetter(masSearch, abc));
                    if (Letter != "")
                    {
                        if (jMap < 8)
                        {
                            int k = map.GetLength(1)-8;


                        }

                        if (FindComparisonWord(iMap, jMap))
                        {
                           World = World + Letter;
                        }
                        else if (World.Length > 0)
                        {
                            World = World + Letter;
                            //helpClass.ListWord.Add(World);
                            masWorld.Add(World);
                            World = "";
                        }



                    }
                }


            }
            //Console.WriteLine("");
            //Console.WriteLine("слова");
            //foreach (string x in helpClass.ListWord) Console.WriteLine(x);

            //Console.WriteLine("поиск завершен");



            return masWorld;
        }
        //берет матрицу 7х7 из карты для дальнейшей проверки
        private static bool[,] MakeArray(bool[,] matr, int iMatrixStart, int jMatrStart, int i, int j)
        {

            bool[,] masForSearch = new bool[i, j];
            if(iMatrixStart< matr.GetLength(0) - i+1 && jMatrStart< matr.GetLength(1) - j+1)
            for (int iSearch = 0; iSearch < i; iSearch++)
            {
                for (int jSearch = 0; jSearch < j; jSearch++)
                {
                    masForSearch[iSearch, jSearch] = matr[iMatrixStart + iSearch, jMatrStart + jSearch];

                }
            }

            return masForSearch;
        }
        //проверяет если еще буква в этом слове
        private  bool FindComparisonWord(int iMap, int jMap)
        {
            bool[,] masSearch = MakeArray(map, iMap, jMap + BaseIJ.TemplateJ + 1, BaseIJ.TemplateI, BaseIJ.TemplateJ);
            string letter=FindComparisonLetter(masSearch);
          
            if(letter!="") return true;
            return false;
        }
        //находит букву
        public string FindComparisonLetter(bool[,] masSearch)
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