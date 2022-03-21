using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Services.Interface
{
    public interface IPedidoService
    {
        Task<ResponseModel> Listar(string IDORDEN, DateTime FECDESDE, DateTime FECHASTA, string IDCLIENTE);
        Task<ResponseModel> GetPedido(string IDORDEN);
        Task<ResponseModel> Insertar(PEDIDO_DTO dto);
        Task<ResponseModel> Update(PEDIDO_DTO dto);
        Task<ResponseModel> Delete(string IDORDEN, string USUARIO);
    }
}
