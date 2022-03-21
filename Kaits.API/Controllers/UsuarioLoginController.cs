using Kaits.API.Clases.Entity;
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
    public class UsuarioLoginController : ControllerBase
    {
        private readonly ILogger<UsuarioLoginController> _logger;
        public UsuarioLoginController(
            ILogger<UsuarioLoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult UsuarioLogin(Usuario usuario)
        {
            _logger.LogInformation("Logueo de Usuario.");
            ResponseModel response = new ResponseModel();
            if (String.IsNullOrEmpty(usuario.LOGIN) || String.IsNullOrEmpty(usuario.PASS))
            {
                response.success = false;
                response.errorMessage = "Error en inicio de Sesión.";
                return BadRequest(response);
            }
            else {
                List<Rol> lst_roles = new List<Rol>();
                lst_roles.Add(new Rol() {IDROLE = "1",NAMEROLE="Administrador",EST="ACT" });
                lst_roles.Add(new Rol() {IDROLE = "2",NAMEROLE="Asistente",EST="ACT" });


                usuario.PASS = "";
                usuario.FIRSTNAME = "Kaits";
                usuario.LASTNAME = "Consulting S.A.C";
                usuario.EMAIL = usuario.LOGIN + "@kaits.com";
                usuario.IDARE = 1;
                usuario.EST = "Activo";
                usuario.UserRolesApp = lst_roles;

                response.success = true;
                response.result = usuario;
                return Ok(response);
            }
        }
    }
}
