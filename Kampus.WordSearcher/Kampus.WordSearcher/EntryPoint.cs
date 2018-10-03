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

    class EntryPoint
    {
        public HttpClient client { get; set; }

       
        public EntryPoint()
        {

            client=new HttpClient();
        }
       


        public async Task Start(string url)
        {
            GetMap c = new GetMap();
            //HttpClient client = new HttpClient();
          
            string token = "token b804a143-fa76-43d2-8888-bc5a7f094b57";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", token);
            //client.DefaultRequestHeaders.Add("test", "true");
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("UTF-8"));
           // c.Client = client;
        }
            
     
        public async Task<string> SendRequest(string Patch, string content)
        {
            //GetMap c = new GetMap();
            //HttpClient client1 = new HttpClient();
            //client1 = c.Client;
            var content1 = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("test=","true")
            });
           
            HttpContent cont = new StringContent(content);
           
            HttpResponseMessage response = await client.PostAsJsonAsync(Patch, content1);
           // Console.Write("я живой3");
            string data = "";

        
            //Console.WriteLine(response.StatusCode.ToString());
            // Console.WriteLine(Patch);
            if (response.IsSuccessStatusCode) data =  StringIsString(response.Content.ReadAsStringAsync().Result.ToString());
            else Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            if ((int)response.StatusCode == 504)
            {
                Console.WriteLine("ошибка 504, по пробуем еще раз");
                data = await SendRequest(Patch, content);
            }
            return data;
        }


    //посылает гет запрос
        public async Task GetAsync(string Patch)
        {
            
            HttpResponseMessage response = await client.GetAsync(Patch);
            Console.WriteLine(response.StatusCode.ToString());
            if (response.IsSuccessStatusCode)
            {
                var products = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(products);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
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
