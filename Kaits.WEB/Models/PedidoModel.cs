using Kaits.WEB.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Models
{
    public class PedidoModel
    {
        public PEDIDO_DTO pedido_filtro { get; set; }
        public PEDIDO_DTO pedido_mant { get; set; }
        public List<CLIENTES_DTO> cliente_list { get; set; }
        public CLIENTES_DTO cliente_filtro { get; set; }
        public List<PRODUCTOS_DTO> producto_list { get; set; }
        public PRODUCTOS_DTO producto_filtro { get; set; }
    }
}
