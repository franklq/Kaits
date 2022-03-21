using Kaits.API.Clases.Entity;
using System;
using System.Threading.Tasks;

namespace Kaits.API.Repository.Interface
{
    public interface IPedidoRepository
    {
        Task<ResponseModel> Listar(string IDORDEN, DateTime FECDESDE, DateTime FECHASTA, string IDCLIENTE);
        Task<ResponseModel> GetPedido(string IDORDEN);
        Task<ResponseModel> Insertar(PEDIDO ent);
        Task<ResponseModel> Update(PEDIDO ent);
        Task<ResponseModel> Delete(string IDORDEN, string USUARIO);
    }
}
