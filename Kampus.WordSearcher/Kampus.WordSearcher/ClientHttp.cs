using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{

    class ClientHttp
    {
        public HttpClient client { get; set; }
        Helper helpClass = new Helper();

        public ClientHttp()
        {

            client=new HttpClient();
        }
       


        public async Task<Helper> Start(string url)
        {

            //HttpClient client = new HttpClient();
           
            string token = "b804a143-fa76-43d2-8888-bc5a7f094b57";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "token "+token);

            //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("UTF-8"));
            // c.Client = client;
            helpClass.Client = client;
            return helpClass;
        }
            
     
        public async Task<string> SendRequest(string Patch)
        {

            string json = "";
            if (Patch == BasePatch.words)
            {
                var ListWord = helpClass.ListWord.Distinct();
                var orderedwords = from i in ListWord
                                   orderby i.Length
                                   select i;
                json = JsonConvert.SerializeObject(orderedwords);
                Console.WriteLine("");
                Console.WriteLine("Найденые слова");
                foreach (string x in orderedwords) Console.WriteLine(x);


            }
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(Patch, content).Result;
            string data = "";
            //Console.WriteLine(response.StatusCode.ToString());
            // Console.WriteLine(Patch);
            if (response.IsSuccessStatusCode) data =  StringIsString(response.Content.ReadAsStringAsync().Result.ToString());
            else Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            if ((int)response.StatusCode == 504)
            {
                Console.WriteLine("ошибка 504, по пробуем еще раз");
                data = await SendRequest(Patch);
            }
            response.Dispose();
            return data;
        }


    //посылает гет запрос
        public async Task GetAsync(string Patch)
        {
            
            HttpResponseMessage response = await client.GetAsync(Patch);
            Console.WriteLine(response.StatusCode.ToString());
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            response.Dispose();
        }

        // оставляет в строке только цифры
        public string StringIsString(string text)
        {
            int[] intMatch = text.Where(Char.IsDigit).Select(x => int.Parse(x.ToString())).ToArray();
            text = string.Join("", intMatch);
            return text;
        }
    }
}
