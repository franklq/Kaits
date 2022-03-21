using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Entities.DTO
{
    public class PEDIDO_DTO
    {
        public string IDORDEN { get; set; }
        public DateTime FECORDEN { get; set; }
        public string IDCLIENTE { get; set; }        
        public decimal PRECIO_ORDEN { get; set; }
        public bool ESTADO { get; set; }        
        public DateTime FECREGISTRO { get; set; }
        public string USUREGISTRO { get; set; }
        public DateTime FECMODIFICA { get; set; }
        public string USUMODIFICA { get; set; }
        public CLIENTES_DTO CLIENTE { get; set; }
        public string DSCCLIENTE { get; set; }
        public DateTime FECDESDE { get; set; }
        public DateTime FECHASTA { get; set; }
        public string ACCION { get; set; }
        public PEDIDODET_DTO PEDIDODET { get; set; }
        public List<PEDIDODET_DTO> DETALLES { get; set; }
    }
}
