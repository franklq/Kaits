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
    public class ProductoController : ControllerBase
    {
        IProductoService _services;
        private readonly ILogger<ProductoController> _logger;
        public ProductoController(
            IProductoService services,
            ILogger<ProductoController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Listar(string IDPRODUCTO, string DSCPRODUCTO)
        {
            _logger.LogInformation("Inicio Listar Controller Producto.");
            ResponseModel response = await _services.Listar(IDPRODUCTO,  DSCPRODUCTO);
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
        public async Task<ActionResult> Insert(PRODUCTOS_DTO dto)
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
        public async Task<ActionResult> Update(PRODUCTOS_DTO dto)
        {
            _logger.LogInformation("Inicio Update Controller Producto.");
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
        public async Task<ActionResult> Delete(string IDPRODUCTO, string USUARIO)
        {
            _logger.LogInformation("Inicio Delete Controller Producto.");
            ResponseModel response = await _services.Delete(IDPRODUCTO, USUARIO);
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
