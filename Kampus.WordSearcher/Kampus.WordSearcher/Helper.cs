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
        public HttpClient Client { get; set; }
     
        public ClientHttp ClientHttp { get; set; }
        public List<String> ListWord { get; set; }
    }
}
