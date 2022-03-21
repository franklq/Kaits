using Kaits.API.Clases.Entity;
using Kaits.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _Config;
        public AuthController(     
            ILogger<AuthController> logger,
            IConfiguration config)
        {
            //_services = services;
            _logger = logger;
            _Config = config;
        }

        [HttpPost]
        public async Task<ActionResult> ClientAuth(AuthToken Auth)
        {
            ResponseModel response = new ResponseModel();
            if (Auth.UniqueID == _Config["JWT_AUTH_KEY"])
            {
                _logger.LogInformation("Ingresando Credenciales");
                string JWT_SECRET_KEY = _Config["JWT_SECRET_KEY"];
                int JWT_TIME_OUT_MIN = Int16.Parse(_Config["JWT_TIME_OUT_MIN"]);
                var jwtHelper = new JWTHelper(JWT_SECRET_KEY, JWT_TIME_OUT_MIN);
                _logger.LogInformation("Creando Token");
                var token = jwtHelper.CreateToken(Auth.Token);
                _logger.LogInformation("Token Creado Exitoso");
                
                response.success = true;
                response.result = token;                

                return Ok(response);
            }
            else {
                response.success = false;
                response.result = null;
                response.errorMessage = "Error en credenciales, no coinciden.";
                _logger.LogInformation("Error en credenciales, no coinciden.");
                return BadRequest(response);
            }           
        }
    }
}
