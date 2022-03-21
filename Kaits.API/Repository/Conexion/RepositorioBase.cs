using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Kaits.API.Repository.Conexion
{
    public abstract class RepositorioBase
    {
        protected SqlConnection _context;
        protected SqlTransaction _transaction;
        private String dbtype;
        public Boolean gb_respuesta;
        public Int16 cod_ret_out { get; set; }
        private Int32 cod_ope_out;
        private String msg_ope_out;
        private Int16 ProcesoSwitch;
        private String ResultProceso;
        private Int32 ErrorNumero;
        private Int16 ErrorSeveridad;
        private Int16 ErrorEstado;
        private String ErrorProcedimiento;
        private Int32 ErrorLinea;
        public String Mensaje { get; set; }
        private Int32 nro_filas;

        public RepositorioBase(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }
        public async Task<bool> EjecutarScriptSql(String sql)
        {
            bool bolEjecutar = false;
            gb_respuesta = true;
            SqlCommand sqlComando = new SqlCommand();
            try
            {
                _context.Open();

                sqlComando.Connection = _context;
                sqlComando.CommandText = sql;
                sqlComando.CommandType = CommandType.Text;
                sqlComando.CommandTimeout = 100000;
                if (_transaction != null)
                {
                    sqlComando.Transaction = _transaction;
                }
                nro_filas = await sqlComando.ExecuteNonQueryAsync();
                _context.Close();

                bolEjecutar = true;
            }

            catch (Exception ex)
            {
                gb_respuesta = false;
                this.cod_ret_out = -1;
                this.cod_ope_out = ex.GetHashCode();
                this.msg_ope_out = ex.Message;
                this.Mensaje = ex.Message;
                this.ProcesoSwitch = -1;
                this.ErrorNumero = -1;
            }
            finally
            {
                sqlComando = null;
            }
            return bolEjecutar;
        }

        public async Task<bool> EjecutarTransaccionSql(string uspBD, DynamicParameters parameters)
        {
            bool bolEjecutar = false;
            this.gb_respuesta = true;

            try
            {
                // _context.Open();                
                nro_filas = await _context.ExecuteAsync(uspBD, param: parameters, transaction: _transaction, commandType: CommandType.StoredProcedure);
                //_context.Close();

                bolEjecutar = true;
            }

            catch (Exception ex)
            {
                gb_respuesta = false;
                this.cod_ret_out = -1;
                this.cod_ope_out = ex.GetHashCode();
                this.msg_ope_out = ex.Message;
                this.Mensaje = ex.Message;
                this.ProcesoSwitch = -1;
                this.ErrorNumero = -1;
            }
            finally
            {
                // _context.Close();
            }
            return bolEjecutar;
        }

        public async Task<List<T>> EjecutarTransaccionListaSql<T>(string uspBD, DynamicParameters parameters) where T : class, new()
        {
            List<T> dataResult = new List<T>();

            this.gb_respuesta = true;
            try
            {
                //_context.Open();
                var dataBD = await _context.QueryAsync<T>(uspBD, param: parameters, transaction: _transaction, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                dataResult = dataBD.AsList<T>();
            }
            catch (Exception ex)
            {
                gb_respuesta = false;
                this.cod_ret_out = -1;
                this.cod_ope_out = ex.GetHashCode();
                this.msg_ope_out = ex.Message;
                this.Mensaje = ex.Message;
                this.ProcesoSwitch = -1;
                this.ErrorNumero = -1;
            }
            //_context.Close();

            return dataResult;
        }

        public async Task<List<T>> EjecutarScriptListaSql<T>(string sql) where T : class, new()
        {
            List<T> dataResult = new List<T>();
            _context.Open();
            this.gb_respuesta = true;
            try
            {
                var dataBD = await _context.QueryAsync<T>(sql, transaction: _transaction);
                dataResult = dataBD.AsList<T>();
            }
            catch (Exception ex)
            {
                gb_respuesta = false;
                this.cod_ret_out = -1;
                this.cod_ope_out = ex.GetHashCode();
                this.msg_ope_out = ex.Message;
                this.Mensaje = ex.Message;
                this.ProcesoSwitch = -1;
                this.ErrorNumero = -1;
            }
            _context.Close();

            return dataResult;
        }
    }
}

