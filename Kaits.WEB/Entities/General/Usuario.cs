using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Entities.General
{
    public class Usuario
    {
        public string EST { get; set; }        
        public int? IDARE { get; set; }
        public int? CODSEDE { get; set; }
        public IEnumerable<Rol> UserRolesApp { get; set; }                
        public string EMAIL { get; set; }
        public string LASTNAME { get; set; }
        public string FIRSTNAME { get; set; }
        public string PASS { get; set; }
        public string LOGIN { get; set; }        
    }
}
