using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Services.Interface
{
    public interface IPedidoDetService
    {
        Task<ResponseModel> Listar(string IDORDENDET, string IDORDEN);
        Task<ResponseModel> Insertar(PEDIDODET_DTO dto);
        Task<ResponseModel> Update(PEDIDODET_DTO dto);
        Task<ResponseModel> Delete(string IDORDEN, string USUARIO);
    }
}
