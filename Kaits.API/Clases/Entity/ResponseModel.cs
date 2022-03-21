using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Clases.Entity
{
    public class ResponseModel
    {
        public bool success { get; set; }
        public object result { get; set; }
        public string errorMessage { get; set; }
        public bool RepeatOption { get; set; }
        public string MethodToRepeat { get; set; }
    }
}
