using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Entities.DTO
{
    public class PEDIDODET_DTO
    {
        public string IDORDENDET { get; set; }
        public string IDPRODUCTO { get; set; }
        public string DSCPRODUCTO { get; set; }
        public int CANTIDAD { get; set; }
        public decimal PRECIO_UNIT { get; set; }
        public decimal PRECIO_TOTAL { get; set; }
        public string IDORDEN { get; set; }
        public bool ESTADO { get; set; }
        public DateTime FECREGISTRO { get; set; }
        public string USUREGISTRO { get; set; }
        public DateTime FECMODIFICA { get; set; }
        public string USUMODIFICA { get; set; }
        public string ACCION { get; set; }
    }
}
