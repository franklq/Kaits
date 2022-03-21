using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Clases.Entity
{
    public class AuthToken
    {
        public string Token { get; set; }
        public string UniqueID { get; set; }
        public string TokenAuth { get; set; }
    }
}
