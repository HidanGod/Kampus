using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{
    class MatrService
    {
        Helper helpClass = new Helper();
        private static bool[,] MakeArray(bool[,] matr, int matrI, int matrJ,int matrIStart, int matrJStart)
        {

            bool[,] masResult = new bool[matrI, matrJ];
            if (matr.GetLength(0) < matrIStart + matrI && matr.GetLength(1) < matrJStart + matrJ)
                for (int i = 0; i < matrI; i++)
                {
                    for (int j = 0; j < matrJ; j++)
                    {
                        masResult[i, j] = matr[matrIStart + i, matrJStart + j];

                    }
                }

            return masResult;
        }

        //получает матрицу из строки
        public List<List<bool>> TakeMatr(string text, int i, int j)
        {
           // Matr matr = new Matr();
            List<List<bool>> matr = new List<List<bool>>();
           
            int num = 0;
            for (int i1 = 0; i1 < i; i1++)
            {
                matr.Add(new List<bool>());
                for (int j1 = 0; j1 <j; j1++)
                {
                   
                    if (text[num].ToString() == "1") matr[i1].Add(true);
                    else matr[i1].Add(false);
                    num++;
                }
            }
           // matr.matr = matrStart;
           // PrintMatrix(matr);
           // Console.WriteLine("mmm");
            return matr;
        }
        //добавляет строку к матрице вниз 
        public List<List<bool>> AddMatrDown(List<List<bool>> matrFirst, List<List<bool>> matrSecond,int k)
        {

            for (int i = matrSecond.Count - k; i < matrSecond.Count; i++)
            {
                matrFirst.Add(matrSecond[i]);

            }

            return matrFirst;
        }
        //добавляет строку к матрице вправо  
        public List<List<bool>> AddMatrLeft(List<List<bool>> matrFirst, List<List<bool>> matrSecond, int k,int startJ)
        {
            int j = startJ;
            for (int i = matrSecond.Count - k; i < matrSecond.Count; i++)
            {
                List<bool> c1 = matrFirst[j];
                foreach (bool b in c1)
                {
                    matrSecond[i].Add(b);
                }
                matrFirst[j] = matrSecond[i];
                j++;



            }
            

            return matrFirst;
        }
        //добавляет строку к матрице вправо  
        public List<List<bool>> AddMatrRight(List<List<bool>> matrFirst, List<List<bool>> matrSecond, int k, int startJ)
        {
            int j = startJ;
           
                foreach (List<bool> c in matrSecond)
                {
                    foreach (bool b in c)
                    {
                        matrFirst[j].Add(b);
                    }
                      j++;
            }
               // matrFirst[j] = matrSecond[i];
              



            return matrFirst;
        }


        //проверяет если в матрице минимальное количество единиц для формирования буквы
        public bool emptiness(List<List<bool>> matr)
        {          
            int sum = emptinessSum(matr);
            if (sum < 15) return false;
            else return true;
        }
        public int emptinessSum(List<List<bool>> matr)
        {
            int sum = 0;
            foreach (List<bool> c in matr)
            {
                foreach (bool b in c)
                {
                    if (b == true) sum++;

                }
            }
            // Console.Write(sum);

            return sum;
        }
        //проверяет есть ли в матрице буквы
        public bool occurrence(List<List<bool>> matr)
        {
            return true;
        }
            //выводит на экран матрицу из листов
        public void PrintMatrix(List<List<bool>> matr)
        {
            Console.WriteLine("");
            foreach (List<bool> c in matr)
            {
                foreach (bool b in c)
                {
                    if(b==true)Console.Write("1");
                    else Console.Write("0");

                }
                Console.WriteLine("");
            }

        }
        //выводит на экран матрицу из массива
        public async Task PrintMatrixArray(bool[,] matr)
        {
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                Console.WriteLine("");
                for (int j = 0; j < matr.GetLength(1); j++)
                {

                    if (matr[i, j].ToString() == "False")
                    {

                        Console.Write("0");
                    }
                    else
                    {

                        Console.Write("x");
                    }
                }

            }

        }
        //получает матрицу из строки массив
        public bool[,] TakeMatrArray(string text, bool[,] matr)
        {        
            int num = 0;
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    if (text[num] == 49) matr[i, j] = true;
                    else matr[i, j] = false;
                    num++;
                }
            }
            return matr;
        }
        //заполняет матрицу
        public bool[,] FillMatrix(bool[,] matr)
        {
            for (int i = 0; i < matr.GetLength(0); i++)
            {

                for (int j = 0; j < matr.GetLength(0); j++)
                {
                    matr[i, j] = true;
                }

            }
            return matr;

        }
        //копирует видимую область в шаблон буквы
        public bool[,] CopyArrayFull(bool[,] letter, bool[,] matr,int k)
        {
            int x = 0;
            int y = 0;
            if (letter.GetLength(0) < matr.GetLength(0)) x = letter.GetLength(0);
               else x = matr.GetLength(0);
            if (letter.GetLength(1) < matr.GetLength(1)) y = letter.GetLength(1);
            else y = matr.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    letter[i+k, j] = matr[i, j];
                }
            }
            return letter;
        }

        //копирует видимую область в шаблон буквы вниз 
        public bool[,] AddArrayDown(bool[,] letter, bool[,] matr, int x)
        {

           
                for (int j = 0; j < letter.GetLength(1); j++)
                {
                    letter[x, j] = matr[matr.GetLength(0)-1, j];
                }
           
            return letter;
        }
        //копирует видимую область в шаблон буквы вверх
        public bool[,] AddArrayUp(bool[,] letter, bool[,] matr, int x)
        {


            for (int j = 0; j < letter.GetLength(1); j++)
            {
                letter[x, j] = matr[0, j];
            }

            return letter;
        }
        public void СreateMapFile(string path, List<List<bool>> matr)
        {
            string str = "";
          
            foreach (List<bool> x in matr)
            {
                foreach (bool c in x)
                {
                    if(c==true)
                        str = str + "x";
                    else
                        str = str + "0";
                }
               File.AppendAllText(path, str + Environment.NewLine);
               str = "";
            }


        }
    }
}
