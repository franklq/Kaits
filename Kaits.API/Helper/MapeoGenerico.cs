using AutoMapper;
using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;

namespace Kaits.API.Helper
{
    public class MapeoGenerico : Profile
    {
        public MapeoGenerico()
        {
            #region "CLIENTES"            
            CreateMap<CLIENTES, CLIENTES_DTO>();
            CreateMap<CLIENTES_DTO, CLIENTES>();
            #endregion

            #region "PEDIDO"            
            CreateMap<PEDIDO, PEDIDO_DTO>();
            CreateMap<PEDIDO_DTO, PEDIDO>();
            #endregion

            #region "PEDIDODET"            
            CreateMap<PEDIDODET, PEDIDODET_DTO>();
            CreateMap<PEDIDODET_DTO, PEDIDODET>();
            #endregion

            #region "PRODUCTOS"            
            CreateMap<PRODUCTOS, PRODUCTOS_DTO>();
            CreateMap<PRODUCTOS_DTO, PRODUCTOS>();
            #endregion

            #region "CLIENTES"            
            CreateMap<CLIENTES, CLIENTES_DTO>();
            CreateMap<CLIENTES_DTO, CLIENTES>();
            #endregion
        }
    }
}
