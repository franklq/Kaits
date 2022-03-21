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
    public class ClienteController : ControllerBase
    {
        IClienteService _services;
        private readonly ILogger<ClienteController> _logger;
        public ClienteController(
            IClienteService services,
            ILogger<ClienteController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Listar(string IDCLIENTE, string DNI, string NOMBRES)
        {
            _logger.LogInformation("Inicio Listar Controller Cliente.");
            ResponseModel response = await _services.Listar(IDCLIENTE, DNI, NOMBRES);
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
        public async Task<ActionResult> Insert(CLIENTES_DTO dto)
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
        public async Task<ActionResult> Update(CLIENTES_DTO dto)
        {
            _logger.LogInformation("Inicio Update Controller Cliente.");
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
        public async Task<ActionResult> Delete(string IDCLIENTE, string USUARIO)
        {
            _logger.LogInformation("Inicio Delete Controller Cliente.");
            ResponseModel response = await _services.Delete(IDCLIENTE, USUARIO);
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
