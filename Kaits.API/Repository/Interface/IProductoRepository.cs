using Kaits.API.Clases.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Repository.Interface
{
    public interface IProductoRepository
    {
        Task<ResponseModel> Listar(string IDPRODUCTO, string DSCPRODUCTO);
        Task<ResponseModel> Insertar(PRODUCTOS ent);
        Task<ResponseModel> Update(PRODUCTOS ent);
        Task<ResponseModel> Delete(string IDPRODUCTO, string USUARIO);
    }
}
