using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Entities.DTO
{
    public class PRODUCTOS_DTO
    {
        public string IDPRODUCTO { get; set; }
        public string DSCPRODUCTO { get; set; }
        public bool ESTADO { get; set; }
        public DateTime FECREGISTRO { get; set; }
        public string USUREGISTRO { get; set; }
        public DateTime FECMODIFICA { get; set; }
        public string USUMODIFICA { get; set; }
    }
}
