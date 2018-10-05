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
    class Helper
    {

        private HttpClient client;
        public HttpClient Client { get; set; }

        private bool[,] map;
        public bool[,] Map { get; set; }

        private ClientHttp clientHttp;
        public ClientHttp ClientHttp { get; set; }

        private List<String> listWord;
        public List<String> ListWord { get; set; }
    }
}
