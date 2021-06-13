using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toka.WebApp.Models
{
    public class TokenRequest
    {
        [JsonProperty("Data")]
        public string Token { get; set; }
    }
}
