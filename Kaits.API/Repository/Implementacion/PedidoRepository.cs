using Kaits.API.Repository.Interface;
using Kaits.API.Repository.Conexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Kaits.API.Clases.Entity;
using Dapper;
using System.Data;

namespace Kaits.API.Repository.Implementacion
{
    public class PedidoRepository : RepositorioBase, IPedidoRepository
    {
        public PedidoRepository(SqlConnection context, SqlTransaction transaction) : base(context, transaction) { }

        public async Task<ResponseModel> Listar(string IDORDEN,DateTime FECDESDE, DateTime FECHASTA, string IDCLIENTE)
        {
            ResponseModel response = new ResponseModel();
            List<PEDIDO> lst = new List<PEDIDO>();
            var parametros = new DynamicParameters();
            parametros.Add("@IDORDEN", IDORDEN);
            parametros.Add("@FECDESDE", FECDESDE);
            parametros.Add("@FECHASTA", FECHASTA);
            parametros.Add("@IDCLIENTE", IDCLIENTE);
            lst = await base.EjecutarTransaccionListaSql<PEDIDO>("LST_PEDIDOS", parametros);
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
        public async Task<ResponseModel> GetPedido(string IDORDEN)
        {
            ResponseModel response = new ResponseModel();
            List<PEDIDO> lst = new List<PEDIDO>();
            var parametros = new DynamicParameters();
            parametros.Add("@IDORDEN", IDORDEN);
   
            lst = await base.EjecutarTransaccionListaSql<PEDIDO>("GET_PEDIDOS", parametros);
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
        public async Task<ResponseModel> Insertar(PEDIDO ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@FECORDEN", ent.FECORDEN);
            parametros.Add("@IDCLIENTE", ent.IDCLIENTE);
            parametros.Add("@PRECIO_ORDEN", ent.PRECIO_ORDEN);
            parametros.Add("@USUARIO", ent.USUREGISTRO);
            parametros.Add("@IDORDEN", dbType: DbType.String, direction: ParameterDirection.Output, size: 12);
            bool respuesta = await base.EjecutarTransaccionSql("INSERT_PEDIDOS", parametros);
            response.success = respuesta;
            if (respuesta)
            {
                ent.IDORDEN = parametros.Get<string>("@IDORDEN");
                response.result = ent;
            }
            else
            {
                response.errorMessage = base.Mensaje;
            }
            return response;
        }

        public async Task<ResponseModel> Update(PEDIDO ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDORDEN", ent.IDORDEN);
            parametros.Add("@FECORDEN", ent.FECORDEN);
            parametros.Add("@IDCLIENTE", ent.IDCLIENTE);
            parametros.Add("@PRECIO_ORDEN", ent.PRECIO_ORDEN);
            parametros.Add("@USUARIO", ent.USUREGISTRO);

            bool respuesta = await base.EjecutarTransaccionSql("UPDATE_PEDIDOS", parametros);
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

        public async Task<ResponseModel> Delete(string IDORDEN, string USUARIO)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDORDEN", IDORDEN);
            parametros.Add("@USUARIO", USUARIO);
            bool respuesta = await base.EjecutarTransaccionSql("DELETE_PEDIDOS", parametros);
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
