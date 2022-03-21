
using Kaits.API.Helper;
using Kaits.API.Repository.Implementacion;
using Kaits.API.Repository.Interface;
using System;
using System.Data.SqlClient;

namespace Kaits.API
{
    public class UnitOfWork : IUnitOfWork
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposed;

        #region Repositories
        public IPedidoRepository PedidoRepository { get; }
        public IPedidoDetRepository PedidoDetRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public IProductoRepository ProductoRepository { get; }

        #endregion
        public UnitOfWork()
        {
            JsonHelper helper = new JsonHelper();
            #region Conexion            
            _connection = new SqlConnection("Data Source=" + helper.BD_DATA_SOURCE + "; Initial Catalog=" + helper.BD_CATALOG + ";Persist Security Info = True; User ID=" + helper.BD_USER + " ; Password=" + helper.BD_PASS + ";");
            //_connection = new SqlConnection("Data Source=172.16.0.216\\SISTEMAS; Initial Catalog=BDGESDOC;Persist Security Info = True; User ID=usrSIG ; Password=3YpFULAhzYyb2swq2HBc;");
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            #endregion

            #region Repositorios
            PedidoDetRepository = new PedidoDetRepository(_connection, _transaction);
            PedidoRepository = new PedidoRepository(_connection, _transaction);
            ClienteRepository = new ClienteRepository(_connection, _transaction);
            ProductoRepository = new ProductoRepository(_connection, _transaction);
            #endregion
        }
        public void Commit()
        {
            _transaction.Commit();
        }
        public void Rollback()
        {
            _transaction.Rollback();
        }
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }
        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                    }
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();

                    }
                }
                _disposed = true;
            }
        }
    }
}
