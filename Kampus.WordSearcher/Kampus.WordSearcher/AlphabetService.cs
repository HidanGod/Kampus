using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Kampus.WordSearcher
{
    class AlphabetService
    {
        //нахождение шаблонов букв из документа
        // принимает размеры матрицы в которую они вписаны
        public List<bool[,]> Сreate(int iMatrix, int jMatrix)
        {
            List<bool[,]> alfabet = new List<bool[,]>();
            using (StreamReader read = new StreamReader(Environment.CurrentDirectory.ToString() + "/Resources/abc.txt", Encoding.Default))//string path - путь к файлу
                while (!read.EndOfStream)
                {
                    int int_i;
                    bool[,] leter = new bool[iMatrix, jMatrix];
                    string leterArray = read.ReadLine().ToString();
                    if (int.TryParse(leterArray, out int_i))
                    {
                        for (int i = 0; i < iMatrix; i++)
                        {
                            for (int j = 0; j < jMatrix; j++)
                            {
                                if (leterArray[j] == Convert.ToChar("1")) leter[i, j] = true;
                                else leter[i, j] = false;
                            }
                           
                            leterArray = read.ReadLine().ToString();
                        }
                        alfabet.Add(leter);
                    } 
                }
            return alfabet;
        }
       // создает карту в файле
        public void СreateMapFile(string path, int[,] map)
        {
            List<string> MasText = MatrInText(map);
            foreach (string x in MasText)
                {
                    File.AppendAllText(path, x + Environment.NewLine);
                }
        }
        //создает строку для записи в файл
        public List<string> MatrInText(int[,] matrix)
        {
            List<string> MasText = new List<string>();
            string text = "";
            for (int i1 = 0; i1 < matrix.GetLength(0); i1++)
            {
                for (int j1 = 0; j1 < matrix.GetLength(1); j1++)
                {
                    if(matrix[i1, j1].ToString()=="0")
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