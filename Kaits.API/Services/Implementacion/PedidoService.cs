using AutoMapper;
using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using Kaits.API.Helper;
using Kaits.API.Services.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Services.Implementacion
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILogger<PedidoService> _logger;
        public PedidoService(ILogger<PedidoService> logger)
        {

            _logger = logger;
            uow = new UnitOfWork();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapeoGenerico>());
            mapper = config.CreateMapper();
        }

        public async Task<ResponseModel> Delete(string IDORDEN, string USUARIO)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Eliminando desde Service Pedido.");
                responseModel = await uow.PedidoRepository.Delete(IDORDEN, USUARIO);
                if (responseModel.success)
                {
                    _logger.LogInformation("Eliminado Corretamente");
                    uow.Commit();
                }
                else
                {
                    _logger.LogError("Error al Eliminar:" + responseModel.errorMessage);
                    uow.Rollback();
                    _logger.LogInformation("Rollback ejecutado Correctamente.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al Eliminar:" + responseModel.errorMessage);
                responseModel.success = false;
                responseModel.errorMessage = ex.Message;
                uow.Rollback();
                _logger.LogInformation("Rollback ejecutado Correctamente.");
            }
            return responseModel;
        }

        public async Task<ResponseModel> Insertar(PEDIDO_DTO dto)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Insertando desde Service Pedido.");
                List<PEDIDODET> lst_pedidosdet = mapper.Map<List<PEDIDODET>>(dto.DETALLES);
                responseModel = await uow.PedidoRepository.Insertar(mapper.Map<PEDIDO>(dto));
                if (responseModel.success)
                {
                    _logger.LogInformation("Insertado Corretamente");
                    PEDIDO pedido = (PEDIDO)responseModel.result;
                    ResponseModel rd = new ResponseModel();
                    foreach (PEDIDODET pd in lst_pedidosdet) {
                        pd.USUREGISTRO = pedido.USUREGISTRO;
                        pd.IDORDEN = pedido.IDORDEN;
                        rd = await uow.PedidoDetRepository.Insertar(pd);
                        if (!rd.success) {
                            _logger.LogError("Error al insertar detalle:" + rd.errorMessage);
                            uow.Rollback();
                            _logger.LogInformation("Rollback ejecutado Correctamente.");
                            break;
                        }
                    }
                    uow.Commit();
                }
                else
                {
                    _logger.LogError("Error al insertar:" + responseModel.errorMessage);
                    uow.Rollback();
                    _logger.LogInformation("Rollback ejecutado Correctamente.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al insertar:" + responseModel.errorMessage);
                responseModel.success = false;
                responseModel.errorMessage = ex.Message;
                uow.Rollback();
                _logger.LogInformation("Rollback ejecutado Correctamente.");
            }
            return responseModel;
        }

        public async Task<ResponseModel> Listar(string IDORDEN, DateTime FECDESDE, DateTime FECHASTA, string IDCLIENTE)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Listando desde Service Metadata.");
                if (String.IsNullOrEmpty(IDORDEN))
                {
                    IDORDEN = "";
                    _logger.LogInformation("Validando información enviada");
                }
                if (String.IsNullOrEmpty(IDCLIENTE))
                {
                    IDCLIENTE = "";
                    _logger.LogInformation("Validando información enviada");
                }
                responseModel = await uow.PedidoRepository.Listar(IDORDEN, FECDESDE, FECHASTA, IDCLIENTE);
                if (responseModel.success)
                {                    
                    List<PEDIDO_DTO> lst_pedido_env = new List<PEDIDO_DTO>();
                    List<PEDIDO_DTO> lst_pedido = mapper.Map<List<PEDIDO_DTO>>((List<PEDIDO>)responseModel.result);
                    foreach (PEDIDO_DTO p in lst_pedido) {
                        ResponseModel rm = await uow.ClienteRepository.Listar(p.IDCLIENTE, "","");
                        if (rm.success)
                        {
                            List<CLIENTES> lst_clientes = (List<CLIENTES>)rm.result;
                            p.CLIENTE = mapper.Map<CLIENTES_DTO>(lst_clientes.First());
                            lst_pedido_env.Add(p);
                        }
                        else {
                            _logger.LogError("Error al Listar:" + responseModel.errorMessage);                            
                            uow.Rollback();
                            _logger.LogInformation("Rollback ejecutado Correctamente.");
                            break;
                        }
                    }
                    responseModel.result = lst_pedido_env;

                    _logger.LogInformation("Listado Corretamente");
                    uow.Commit();
                }
                else
                {
                    _logger.LogError("Error al Listar:" + responseModel.errorMessage);
                    uow.Rollback();
                    _logger.LogInformation("Rollback ejecutado Correctamente.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al Listar:" + responseModel.errorMessage);
                responseModel.success = false;
                responseModel.errorMessage = ex.Message;
                uow.Rollback();
                _logger.LogInformation("Rollback ejecutado Correctamente.");
            }
            return responseModel;
        }

        public async Task<ResponseModel> GetPedido(string IDORDEN)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Listando desde Service Metadata.");
                if (String.IsNullOrEmpty(IDORDEN))
                {
                    IDORDEN = "";
                    _logger.LogInformation("Validando información enviada");
                }

                responseModel = await uow.PedidoRepository.GetPedido(IDORDEN);
                if (responseModel.success)
                {
                    List<PEDIDO_DTO> lst_pedido_env = new List<PEDIDO_DTO>();
                    List<PEDIDO_DTO> lst_pedido = mapper.Map<List<PEDIDO_DTO>>((List<PEDIDO>)responseModel.result);
                    foreach (PEDIDO_DTO p in lst_pedido)
                    {
                        ResponseModel rm = await uow.ClienteRepository.Listar(p.IDCLIENTE, "", "");
                        if (rm.success)
                        {
                            List<CLIENTES> lst_clientes = (List<CLIENTES>)rm.result;
                            p.CLIENTE = mapper.Map<CLIENTES_DTO>(lst_clientes.First());
                            lst_pedido_env.Add(p);
                        }
                        else
                        {
                            _logger.LogError("Error al Listar:" + responseModel.errorMessage);
                            uow.Rollback();
                            _logger.LogInformation("Rollback ejecutado Correctamente.");
                            break;
                        }
                    }
                    responseModel.result = lst_pedido_env.First();

                    _logger.LogInformation("Listado Corretamente");
                    uow.Commit();
                }
                else
                {
                    _logger.LogError("Error al Listar:" + responseModel.errorMessage);
                    uow.Rollback();
                    _logger.LogInformation("Rollback ejecutado Correctamente.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al Listar:" + responseModel.errorMessage);
                responseModel.success = false;
                responseModel.errorMessage = ex.Message;
                uow.Rollback();
                _logger.LogInformation("Rollback ejecutado Correctamente.");
            }
            return responseModel;
        }


        public async Task<ResponseModel> Update(PEDIDO_DTO dto)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Update desde Service Pedido.");
                responseModel = await uow.PedidoRepository.Update(mapper.Map<PEDIDO>(dto));
                if (responseModel.success)
                {
                    _logger.LogInformation("Update Corretamente");
                    uow.Commit();
                }
                else
                {
                    _logger.LogError("Error al hacer Update:" + responseModel.errorMessage);
                    uow.Rollback();
                    _logger.LogInformation("Rollback ejecutado Correctamente.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al hacer Update:" + responseModel.errorMessage);
                responseModel.success = false;
                responseModel.errorMessage = ex.Message;
                uow.Rollback();
                _logger.LogInformation("Rollback ejecutado Correctamente.");
            }
            return responseModel;
        }

    }
}
