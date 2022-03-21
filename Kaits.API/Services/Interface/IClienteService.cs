using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Services.Interface
{
    public interface IClienteService
    {
        Task<ResponseModel> Listar(string IDCLIENTE, string DNI, string NOMBRES);
        Task<ResponseModel> Insertar(CLIENTES_DTO dto);
        Task<ResponseModel> Update(CLIENTES_DTO dto);
        Task<ResponseModel> Delete(string IDCLIENTE, string USUARIO);
    }
}
