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
    class HelpClass
    {

        private System.Net.Http.HttpClient client;

        public HttpClient Client
        {
            get
            {
                return client;
            }

            set
            {
                client = value;
            }
        }
        private bool[,] map;

        public bool[,] Map
        {
            get
            {
                return map;
            }

            set
            {
                map = value;
            }
        }
        private ClientHttp clientHttp;
        public ClientHttp ClientHttp
        {
            get
            {
                return clientHttp;
            }

            set
            {
                clientHttp = value;
            }
        }
    }
}
