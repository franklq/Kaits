using Kaits.API.Clases.DTO;
using Kaits.API.Clases.Entity;
using Kaits.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidoDetController : ControllerBase
    {
        IPedidoDetService _services;
        private readonly ILogger<PedidoDetController> _logger;
        public PedidoDetController(
            IPedidoDetService services,
            ILogger<PedidoDetController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Listar(string IDORDENDET, string IDORDEN)
        {
            _logger.LogInformation("Inicio Listar Controller PedidoDet.");
            ResponseModel response = await _services.Listar(IDORDENDET,IDORDEN);
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
        public async Task<ActionResult> Insert(PEDIDODET_DTO dto)
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
        public async Task<ActionResult> Update(PEDIDODET_DTO dto)
        {
            _logger.LogInformation("Inicio Update Controller PedidoDet.");
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
        public async Task<ActionResult> Delete(string IDORDENDET, string USUARIO)
        {
            _logger.LogInformation("Inicio Delete Controller PedidoDet.");
            ResponseModel response = await _services.Delete(IDORDENDET, USUARIO);
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
