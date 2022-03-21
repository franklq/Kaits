using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Helper
{
    public class JsonHelper
    {
        public JsonHelper()
        {
            this.JWT_SECRET_KEY = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            this.JWT_AUTH_KEY = Environment.GetEnvironmentVariable("JWT_AUTH_KEY");
            this.JWT_TIME_OUT_MIN = Environment.GetEnvironmentVariable("JWT_TIME_OUT_MIN");

            this.BD_DATA_SOURCE = Environment.GetEnvironmentVariable("BD_DATA_SOURCE");
            this.BD_CATALOG = Environment.GetEnvironmentVariable("BD_CATALOG");
            this.BD_USER = Environment.GetEnvironmentVariable("BD_USER");
            this.BD_PASS = Environment.GetEnvironmentVariable("BD_PASS");

        }
        public string JWT_SECRET_KEY { get; private set; }
        public string JWT_AUTH_KEY { get; private set; }
        public string JWT_TIME_OUT_MIN { get; private set; }
        public string BD_DATA_SOURCE { get; private set; }
        public string BD_CATALOG { get; private set; }
        public string BD_USER { get; private set; }
        public string BD_PASS { get; private set; }

    }
}
