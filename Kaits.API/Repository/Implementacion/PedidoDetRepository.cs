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
    public class PedidoDetRepository : RepositorioBase, IPedidoDetRepository
    {
        public PedidoDetRepository(SqlConnection context, SqlTransaction transaction) : base(context, transaction) { }

        public async Task<ResponseModel> Listar(string IDORDENDET, string IDORDEN)
        {
            ResponseModel response = new ResponseModel();
            List<PEDIDODET> lst = new List<PEDIDODET>();
            var parametros = new DynamicParameters();
            parametros.Add("@IDORDENDET", IDORDENDET);
            parametros.Add("@IDORDEN", IDORDEN);

            lst = await base.EjecutarTransaccionListaSql<PEDIDODET>("LST_PEDIDODET", parametros);
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

        public async Task<ResponseModel> Insertar(PEDIDODET ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDPRODUCTO", ent.IDPRODUCTO);
            parametros.Add("@DSCPRODUCTO", ent.DSCPRODUCTO);
            parametros.Add("@CANTIDAD", ent.CANTIDAD);
            parametros.Add("@PRECIO_UNIT", ent.PRECIO_UNIT);
            parametros.Add("@PRECIO_TOTAL", ent.PRECIO_TOTAL);
            parametros.Add("@IDORDEN", ent.IDORDEN);
            parametros.Add("@USUARIO", ent.USUREGISTRO);
            parametros.Add("@IDORDENDET", dbType: DbType.String, direction: ParameterDirection.Output, size: 12);
            bool respuesta = await base.EjecutarTransaccionSql("INSERT_PEDIDODET", parametros);
            response.success = respuesta;
            if (respuesta)
            {
                ent.IDORDEN = parametros.Get<string>("@IDORDENDET");
                response.result = ent;
            }
            else
            {
                response.errorMessage = base.Mensaje;
            }
            return response;
        }

        public async Task<ResponseModel> Update(PEDIDODET ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDORDENDET", ent.IDORDENDET);
            parametros.Add("@IDPRODUCTO", ent.IDPRODUCTO);
            parametros.Add("@DSCPRODUCTO", ent.DSCPRODUCTO);
            parametros.Add("@CANTIDAD", ent.CANTIDAD);
            parametros.Add("@PRECIO_UNIT", ent.PRECIO_UNIT);
            parametros.Add("@PRECIO_TOTAL", ent.PRECIO_TOTAL);
            parametros.Add("@IDORDEN", ent.IDORDEN);
            parametros.Add("@USUARIO", ent.USUREGISTRO);

            bool respuesta = await base.EjecutarTransaccionSql("UPDATE_PEDIDODET", parametros);
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

        public async Task<ResponseModel> Delete(string IDORDENDET, string USUARIO)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDORDENDET", IDORDENDET);
            parametros.Add("@USUARIO", USUARIO);
            bool respuesta = await base.EjecutarTransaccionSql("DELETE_PEDIDODET", parametros);
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
