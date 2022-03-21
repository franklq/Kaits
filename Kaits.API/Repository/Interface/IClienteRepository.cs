using Kaits.API.Clases.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Repository.Interface
{
    public interface IClienteRepository
    {
        Task<ResponseModel> Listar(string IDCLIENTE, string DNI, string NOMBRES);
        Task<ResponseModel> Insertar(CLIENTES ent);
        Task<ResponseModel> Update(CLIENTES ent);
        Task<ResponseModel> Delete(string IDCLIENTE, string USUARIO);
    }
}
