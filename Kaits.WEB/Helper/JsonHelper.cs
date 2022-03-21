using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Helper
{
    public class JsonHelper
    {
        public JsonHelper(IConfiguration _configuration)
        {

            this.JWT_AUTH_KEY = _configuration["JWT_AUTH_KEY"];
            this.JWT_AUTH_TOKEN = _configuration["JWT_AUTH_TOKEN"];
            this.URL_BASE_API = _configuration["URL_BASE_API"];
        }
        public string JWT_AUTH_KEY { get; private set; }
        public string JWT_AUTH_TOKEN { get; private set; }
        public string URL_BASE_API { get; private set; }
    }
}
