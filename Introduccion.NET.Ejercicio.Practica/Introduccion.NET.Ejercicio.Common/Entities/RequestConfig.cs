using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Entities
{
    public class RequestConfig
    {
        public RequestConfig()
        {
            Body = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
            QueryParams = new Dictionary<string, string>();
        }

        [JsonProperty("url")]
        public string URL { get; set; }
        [JsonProperty("queryParams")]
        public Dictionary<string, string> QueryParams { get; set; }
        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }
        [JsonProperty("body")]
        public Dictionary<string, string> Body { get; set; }

        [JsonProperty("BaseAddress")]
        public string BaseAddress { get; set; }
    }
}
