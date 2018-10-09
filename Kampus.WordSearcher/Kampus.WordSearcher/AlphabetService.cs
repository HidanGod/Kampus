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
    class AlphabetService
    {
        //нахождение шаблонов букв из документа
        // принимает путь, количестко букв, размеры матрицы в которую они вписаны

        public List<bool[,]> Сreate(int imart, int jmatr)
        {
            List<bool[,]> bb = new List<bool[,]>();


            //path = @"C:\Users\HidiMeedi\source\repos\Kampus.WordSearcher\Kampus.WordSearcher\bin\Debug\abc.txt";
            using (StreamReader read = new StreamReader(Environment.CurrentDirectory.ToString() + "/Resources/abc.txt", Encoding.Default))//string path - путь к файлу
                while (!read.EndOfStream)
                {

                    int int_i;
                    bool[,] strmas = new bool[imart, jmatr];
                    string text = read.ReadLine().ToString();
                    if (int.TryParse(text, out int_i))
                    {
                       // Console.WriteLine(text);

                        for (int i = 0; i < imart; i++)
                        {
                            for (int j = 0; j < jmatr; j++)
                            {
                                if (Convert.ToInt32(text[j]) == 49) strmas[i, j] = true;
                                else strmas[i, j] = false;

                            }
                           
                            text = read.ReadLine().ToString();
                        }
                        bb.Add(strmas);

                    }
                    else
                    {
                       // Console.WriteLine("");
                    }
                   
                }

            /*
            foreach (bool[,] x in bb)
            {
                for (int i = 0; i < imart; i++)
                {
                    for (int j = 0; j < jmatr; j++)
                    {
                       if(x[i, j].ToString()=="False")
                        Console.Write(" ");
                        else Console.Write("x");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
            }*/
           // string currDir = Environment.CurrentDirectory.ToString() + "/map.txt";
          //  СreateMapFile(currDir, bb[0],7,7);
            //  Console.WriteLine($"{FirstName} {LastName}");
            return bb;
        }
        public void СreateMapFile(string path, int[,] map)
        {

            List<string> MasText = MatrInText(map, map.GetLength(0), map.GetLength(1));
            foreach (string x in MasText)
                {
                    File.AppendAllText(path, x + Environment.NewLine);
                }

           
        }

        public List<string> MatrInText(int[,] matr, int i, int j)
        {

            List<string> MasText = new List<string>();
            string text = "";
            for (int i1 = 0; i1 < i; i1++)
            {
                //Console.WriteLine("");
                for (int j1 = 0; j1 < j; j1++)
                {
                    if(matr[i1, j1].ToString()=="0")
                    text = text + "0";
                    else text = text + "1";

                }
                MasText.Add(text);
                text = "";
            }
            return MasText;
        }


    }
}