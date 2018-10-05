using System;
using System.Collections.Generic;
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
           /// PrintMatrix(matr);
            //Console.WriteLine("mmm");
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
            //выводит на экран матрицу
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
    }
}
