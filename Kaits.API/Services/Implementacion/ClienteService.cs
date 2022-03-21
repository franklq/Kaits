using AutoMapper;
using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using Kaits.API.Helper;
using Kaits.API.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Services.Implementacion
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILogger<ClienteService> _logger;
        public ClienteService(ILogger<ClienteService> logger)
        {

            _logger = logger;
            uow = new UnitOfWork();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapeoGenerico>());
            mapper = config.CreateMapper();
        }

        public async Task<ResponseModel> Delete(string IDCLIENTE, string USUARIO)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Eliminando desde Service Cliente.");
                responseModel = await uow.ClienteRepository.Delete(IDCLIENTE, USUARIO);
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

        public async Task<ResponseModel> Insertar(CLIENTES_DTO dto)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Insertando desde Service Cliente.");
                responseModel = await uow.ClienteRepository.Insertar(mapper.Map<CLIENTES>(dto));
                if (responseModel.success)
                {
                    _logger.LogInformation("Insertado Corretamente");
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

        public async Task<ResponseModel> Listar(string IDCLIENTE,  string DNI, string NOMBRES)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Listando desde Service Metadata.");
                if (String.IsNullOrEmpty(IDCLIENTE))
                {
                    IDCLIENTE = "";
                    _logger.LogInformation("Validando información enviada");
                }
                if (String.IsNullOrEmpty(DNI))
                {
                    DNI = "";
                    _logger.LogInformation("Validando información enviada");
                }
                if (String.IsNullOrEmpty(NOMBRES))
                {
                    NOMBRES = "";
                    _logger.LogInformation("Validando información enviada");
                }

                responseModel = await uow.ClienteRepository.Listar(IDCLIENTE, DNI, NOMBRES);
                if (responseModel.success)
                {
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

        public async Task<ResponseModel> Update(CLIENTES_DTO dto)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                _logger.LogInformation("Update desde Service Cliente.");
                responseModel = await uow.ClienteRepository.Update(mapper.Map<CLIENTES>(dto));
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
