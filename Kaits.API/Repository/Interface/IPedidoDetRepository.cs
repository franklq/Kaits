using Kaits.API.Clases.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Repository.Interface
{
    public interface IPedidoDetRepository
    {
        Task<ResponseModel> Listar(string IDORDENDET, string IDORDEN);
        Task<ResponseModel> Insertar(PEDIDODET ent);
        Task<ResponseModel> Update(PEDIDODET ent);
        Task<ResponseModel> Delete(string IDORDENDET, string USUARIO);
    }
}
