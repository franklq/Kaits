using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Services.Interface
{
    public interface IProductoService
    {
        Task<ResponseModel> Listar(string IDPRODUCTO,  string DSCPRODUCTO);
        Task<ResponseModel> Insertar(PRODUCTOS_DTO ent);
        Task<ResponseModel> Update(PRODUCTOS_DTO ent);
        Task<ResponseModel> Delete(string IDPRODUCTO, string USUARIO);
    }
}
