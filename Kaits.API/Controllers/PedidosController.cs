using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using Kaits.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Kaits.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        IPedidoService _services;
        private readonly ILogger<PedidosController> _logger;
        public PedidosController(
            IPedidoService services,
            ILogger<PedidosController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Listar(string IDORDEN, string FECDESDE, string FECHASTA, string IDCLIENTE)
        {
            _logger.LogInformation("Inicio Listar Controller Pedido.");
            ResponseModel response = await _services.Listar(IDORDEN,Util.Utils.StringToDatetime(FECDESDE), Util.Utils.StringToDatetime(FECHASTA),IDCLIENTE);
            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("GetPedido")]
        public async Task<ActionResult> GetPedido(string IDORDEN)
        {
            _logger.LogInformation("Inicio Listar Controller Pedido.");
            ResponseModel response = await _services.GetPedido(IDORDEN);
            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Insert(PEDIDO_DTO dto)
        {
            _logger.LogInformation("Inicio Insert Controller Plantilla.");
            ResponseModel response = await _services.Insertar(dto);
            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(PEDIDO_DTO dto)
        {
            _logger.LogInformation("Inicio Update Controller Pedido.");
            ResponseModel response = await _services.Update(dto);
            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string IDORDEN, string USUARIO)
        {
            _logger.LogInformation("Inicio Delete Controller Pedido.");
            ResponseModel response = await _services.Delete(IDORDEN, USUARIO);
            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
