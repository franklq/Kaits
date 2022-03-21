using Kaits.WEB.Data;
using Kaits.WEB.Entities.General;
using Kaits.WEB.Helper;
using Kaits.WEB.Models;
using Kaits.WEB.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Kaits.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly JsonHelper helper;

        public HomeController(IConfiguration config, ILogger<HomeController> logger)
        {
            _logger = logger;
            _config = config;
            helper = new JsonHelper(_config);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(HomeModel model)
        {
            model = new HomeModel();
            model.usuario_login = new Usuario();
            return View(model);
        }
        public async Task<JsonResult> PwdChange(HomeModel model)
        {
            ResponseModel response = new ResponseModel();
            AuthToken.URL_BASE_API = helper.URL_BASE_API;
            response = await HttpClientAuth.GetTokenClient(helper.JWT_AUTH_KEY,helper.JWT_AUTH_TOKEN);
            if (response.success)
            {
                AuthToken.Token = response.result.ToString();
                response = await HttpClientAuth.LoginUsuario(model.usuario_login.LOGIN, model.usuario_login.PASS);
                if (response.success)
                {
                    Usuario oUser = JsonConvert.DeserializeObject<Usuario>(response.result.ToString());
                    if (oUser != null)
                    {
                        AuthToken.LoginName = oUser.LOGIN;
                        response.result = oUser;
                        _logger.LogInformation("Inicio de Sesion Correcto.");
                    }
                    else
                    {
                        response.success = false;
                        response.errorMessage = "Usuario o clave inválida, ingrese nuevamente.";
                        _logger.LogError("Usuario o clave inválida, ingrese nuevamente.");
                    }
                }
                else {
                    _logger.LogError(response.errorMessage);
                }                
            }
            else {               
                _logger.LogError(response.errorMessage);
            }            
            return Json(response);
        }

        public async Task<IActionResult> Inicio(HomeModel model)
        {
            ResponseModel response = new ResponseModel();
            response = await HttpClientAuth.LoginUsuario(model.usuario_login.LOGIN, model.usuario_login.PASS);
            if (response.success)
            {
                Usuario oUser = JsonConvert.DeserializeObject<Usuario>(response.result.ToString());
                if (oUser != null)
                {
                    AuthToken.LoginName = oUser.LOGIN;
                    _logger.LogInformation("Inicio de Sesion Correcto.");
                    model.usuario_login = oUser;
                    HttpContext.Session.SetComplexData("usuario_login", model.usuario_login);
                }
                else
                {
                    response.success = false;
                    response.errorMessage = "Usuario o clave inválida, ingrese nuevamente.";
                    _logger.LogError("Usuario o clave inválida, ingrese nuevamente.");
                }
            }
            else
            {
                _logger.LogError(response.errorMessage);
            }
            return RedirectToAction("Index", "Pedido");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.RemoveComplexData("usuario_login");
            HttpContext.Session.RemoveComplexData("lst_pedidodet");
            return RedirectToAction("Login", "Home");
        }
        public async Task<JsonResult> ValidaSesion()
        {
            Usuario userlogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            ResponseModel response = new ResponseModel();
            if (userlogin == null)
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, inicie nuevamente.";
            }
            else
            {
                response.success = true;
                response.result = userlogin;
            }
            return Json(response);
        }
    }
}
