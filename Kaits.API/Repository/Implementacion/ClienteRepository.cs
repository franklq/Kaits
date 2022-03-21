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
    public class ClienteRepository : RepositorioBase, IClienteRepository
    {
        public ClienteRepository(SqlConnection context, SqlTransaction transaction) : base(context, transaction) { }

        public async Task<ResponseModel> Listar(string IDCLIENTE, string DNI, string NOMBRES)
        {
            ResponseModel response = new ResponseModel();
            List<CLIENTES> lst = new List<CLIENTES>();
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", IDCLIENTE);
            parametros.Add("@DNI", DNI);
            parametros.Add("@NOMBRES", NOMBRES);
            lst = await base.EjecutarTransaccionListaSql<CLIENTES>("LST_CLIENTES", parametros);
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

        public async Task<ResponseModel> Insertar(CLIENTES ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@NOMBRES", ent.NOMBRES);
            parametros.Add("@APELLIDOS", ent.APELLIDOS);
            parametros.Add("@DNI", ent.DNI);
            parametros.Add("@USUARIO", ent.USUREGISTRO);
            parametros.Add("@IDCLIENTE", dbType: DbType.String, direction: ParameterDirection.Output, size: 12);
            bool respuesta = await base.EjecutarTransaccionSql("INSERT_CLIENTES", parametros);
            response.success = respuesta;
            if (respuesta)
            {
                ent.IDCLIENTE = parametros.Get<string>("@IDCLIENTE");
                response.result = ent;
            }
            else
            {
                response.errorMessage = base.Mensaje;
            }
            return response;
        }

        public async Task<ResponseModel> Update(CLIENTES ent)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", ent.IDCLIENTE);
            parametros.Add("@NOMBRES", ent.NOMBRES);
            parametros.Add("@APELLIDOS", ent.APELLIDOS);
            parametros.Add("@DNI", ent.DNI);
            parametros.Add("@USUARIO", ent.USUREGISTRO);

            bool respuesta = await base.EjecutarTransaccionSql("UPDATE_CLIENTES", parametros);
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

        public async Task<ResponseModel> Delete(string IDCLIENTE, string USUARIO)
        {
            ResponseModel response = new ResponseModel();
            var parametros = new DynamicParameters();
            parametros.Add("@IDCLIENTE", IDCLIENTE);
            parametros.Add("@USUARIO", USUARIO);
            bool respuesta = await base.EjecutarTransaccionSql("DELETE_CLIENTES", parametros);
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
