using Dapper;
using Kaits.API.Clases.Entity;
using Kaits.API.Repository.Conexion;
using Kaits.API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Repository.Implementacion
{
    public class ProductoRepository : RepositorioBase, IProductoRepository
    {
        public ProductoRepository(SqlConnection context, SqlTransaction transaction) : base(context, transaction) { }

        public async Task<ResponseModel> Listar(string IDPRODUCTO,  string DSCPRODUCTO)
        {
            ResponseModel response = new ResponseModel();
            List<PRODUCTOS> lst = new List<PRODUCTOS>();
            var parametros = new DynamicParameters();
            parametros.Add("@IDPRODUCTO", IDPRODUCTO);
            parametros.Add("@DSCPRODUCTO", DSCPRODUCTO);
            lst = await base.EjecutarTransaccionListaSql<PRODUCTOS>("LST_PRODUCTOS", parametros);
            if (base.cod_ret_out < 0)
            {
                response.success = false;
                response.errorMessage = base.Mensaje;
            }
            else
            {
                response.success = true;
                response.result = lst;
            }
            return response;
        }

        public async Task<ResponseModel> Insertar(PRODUCTOS ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@DSCPRODUCTO", ent.DSCPRODUCTO);
            parametros.Add("@USUARIO", ent.USUREGISTRO);
            parametros.Add("@IDPRODUCTO", dbType: DbType.String, direction: ParameterDirection.Output, size: 12);
            bool respuesta = await base.EjecutarTransaccionSql("INSERT_PRODUCTOS", parametros);
            response.success = respuesta;
            if (respuesta)
            {
                ent.IDPRODUCTO = parametros.Get<string>("@IDPRODUCTO");
                response.result = ent;
            }
            else
            {
                response.errorMessage = base.Mensaje;
            }
            return response;
        }

        public async Task<ResponseModel> Update(PRODUCTOS ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDPRODUCTO", ent.IDPRODUCTO);
            parametros.Add("@DSCPRODUCTO", ent.DSCPRODUCTO);
            parametros.Add("@USUARIO", ent.USUREGISTRO);

            bool respuesta = await base.EjecutarTransaccionSql("UPDATE_PRODUCTOS", parametros);
            response.success = respuesta;
            if (respuesta)
            {
                response.result = ent;
            }
            else
            {
                response.errorMessage = base.Mensaje;
            }
            return response;
        }

        public async Task<ResponseModel> Delete(string IDPRODUCTO, string USUARIO)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDPRODUCTO", IDPRODUCTO);
            parametros.Add("@USUARIO", USUARIO);
            bool respuesta = await base.EjecutarTransaccionSql("DELETE_PRODUCTOS", parametros);
            response.success = respuesta;
            if (respuesta)
            {
                response.result = "Se Elimino Correctamente";
            }
            else
            {
                response.errorMessage = base.Mensaje;
            }
            return response;
        }
    }
}
