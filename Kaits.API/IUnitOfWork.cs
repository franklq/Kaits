using Kaits.API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API
{
    public interface IUnitOfWork
    {
        IPedidoRepository PedidoRepository { get; }
        IPedidoDetRepository PedidoDetRepository { get; }
        IClienteRepository ClienteRepository { get; }
        IProductoRepository ProductoRepository { get; }
        void Commit();
        void Rollback();
    }
}
