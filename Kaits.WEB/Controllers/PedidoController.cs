using Kaits.WEB.Data;
using Kaits.WEB.Entities.DTO;
using Kaits.WEB.Entities.General;
using Kaits.WEB.Models;
using Kaits.WEB.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;
        public PedidoController(ILogger<PedidoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(PedidoModel model)
        {
            _logger.LogInformation("Iniciando Pedido Web.");
            model = new PedidoModel();
            return View(model);
        }
        public async Task<JsonResult> Lista(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            _logger.LogInformation("Listando Pedidos.");
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                if (model.pedido_filtro == null)
                {
                    model.pedido_filtro = new PEDIDO_DTO();
                    model.pedido_filtro.IDORDEN = "";
                    model.pedido_filtro.IDCLIENTE = "";
                }
                else
                {
                    if (model.pedido_filtro.IDORDEN == null)
                    {
                        model.pedido_filtro.IDORDEN = "";
                    }

                    if (model.pedido_filtro.IDCLIENTE == null)
                    {
                        model.pedido_filtro.IDCLIENTE = "";
                    }
                }
                response = await HttpClientPedido.Listar(model.pedido_filtro.IDORDEN,
                                                        Util.Utils.DatetimeTOString(model.pedido_filtro.FECDESDE),
                                                        Util.Utils.DatetimeTOString(model.pedido_filtro.FECHASTA),
                                                        model.pedido_filtro.IDCLIENTE);
                if (response.success)
                {
                    response.result = JsonConvert.DeserializeObject<List<PEDIDO_DTO>>(response.result.ToString());
                }
            }
            else {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }            
            return Json(response);
        }
        public async Task<JsonResult> ListaDet(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            _logger.LogInformation("Listando Pedidos.");
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                if (model.pedido_mant.IDORDEN != null && !String.IsNullOrEmpty(model.pedido_mant.IDORDEN))
                {
                    response = await HttpClientPedidoDet.Listar(model.pedido_mant.PEDIDODET.IDORDENDET, model.pedido_mant.IDORDEN);
                    if (response.success)
                    {
                        response.result = JsonConvert.DeserializeObject<List<PEDIDODET_DTO>>(response.result.ToString());
                    }                    
                }
                else { 
                    response.result = HttpContext.Session.GetComplexData<List<PEDIDODET_DTO>>("lst_pedidodet");
                    if (response.result == null) {
                        response.result = new List<PEDIDODET_DTO>();
                    }
                    response.success = true;
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> ListaClientes(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            _logger.LogInformation("Listando Clientes.");
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                if (model.cliente_filtro == null)
                {
                    model.cliente_filtro = new CLIENTES_DTO();
                    model.cliente_filtro.IDCLIENTE = "";
                    model.cliente_filtro.DNI = "";
                    model.cliente_filtro.NOMBRES = "";
                }
                else
                {
                    if (model.cliente_filtro.IDCLIENTE == null)
                    {
                        model.cliente_filtro.IDCLIENTE = "";
                    }

                    if (model.cliente_filtro.DNI == null)
                    {
                        model.cliente_filtro.DNI = "";
                    }
                    if (model.cliente_filtro.NOMBRES == null)
                    {
                        model.cliente_filtro.NOMBRES = "";
                    }
                }
                response = await HttpClientCliente.Listar(model.cliente_filtro.IDCLIENTE, model.cliente_filtro.DNI, model.cliente_filtro.NOMBRES);
                if (response.success)
                {
                    List<CLIENTES_DTO> lst = JsonConvert.DeserializeObject<List<CLIENTES_DTO>>(response.result.ToString());
                    if (lst != null && lst.Count > 0)
                    {
                        response.result = lst;
                    }
                    else {
                        response.success = false;
                        response.errorMessage = "No se encontro Cliente";
                    }
                     
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> ListaProductos(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            _logger.LogInformation("Listando Productos.");
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                if (model.producto_filtro == null)
                {
                    model.producto_filtro = new PRODUCTOS_DTO();
                    model.producto_filtro.IDPRODUCTO = "";
                    model.producto_filtro.DSCPRODUCTO = "";
                }
                else
                {
                    if (model.producto_filtro.IDPRODUCTO == null)
                    {
                        model.producto_filtro.IDPRODUCTO = "";
                    }

                    if (model.producto_filtro.DSCPRODUCTO == null)
                    {
                        model.producto_filtro.DSCPRODUCTO = "";
                    }
                }
                response = await HttpClientProducto.Listar(model.producto_filtro.IDPRODUCTO, model.producto_filtro.DSCPRODUCTO);
                if (response.success)
                {                    
                    List<PRODUCTOS_DTO> lst = JsonConvert.DeserializeObject<List<PRODUCTOS_DTO>>(response.result.ToString());
                    if (lst != null && lst.Count > 0)
                    {
                        response.result = lst;
                    }
                    else
                    {
                        response.success = false;
                        response.errorMessage = "No se encontro Producto";
                    }
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> Save(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                 
                if (model.pedido_mant.ACCION == "NEW")
                {
                    List<PEDIDODET_DTO> lst_pedidodet = HttpContext.Session.GetComplexData<List<PEDIDODET_DTO>>("lst_pedidodet");
                    if (lst_pedidodet != null && lst_pedidodet.Count > 0)
                    {
                        decimal precio_total = 0;
                        foreach (PEDIDODET_DTO pd in lst_pedidodet) {
                            precio_total += pd.PRECIO_TOTAL;
                        }
                        model.pedido_mant.PRECIO_ORDEN = precio_total;
                        model.pedido_mant.DETALLES = lst_pedidodet;
                    }
                    else {
                        response.success = false;
                        response.errorMessage = "Debe agregar un item, antes de guardar.";
                        return Json(response);
                    }
                    _logger.LogInformation("Insertando Pedido.");
                    model.pedido_mant.USUREGISTRO = usuarioLogin.LOGIN;
                    response = await HttpClientPedido.Insert(model.pedido_mant);
                }
                else
                {
                    _logger.LogInformation("Modificando Pedido.");
                    model.pedido_mant.USUMODIFICA = usuarioLogin.LOGIN;
                    response = await HttpClientPedido.Update(model.pedido_mant);
                }

                if (response.success)
                {
                    response.result = JsonConvert.DeserializeObject<PEDIDO_DTO>(response.result.ToString());
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> GetPedido(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                _logger.LogInformation("Get Pedido.");
                response = await HttpClientPedido.GetPedido(model.pedido_mant.IDORDEN);
                if (response.success)
                {
                    response.result = JsonConvert.DeserializeObject<PEDIDO_DTO>(response.result.ToString());
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> Delete(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                _logger.LogInformation("Delete Pedido.");
                response = await HttpClientPedido.Delete(model.pedido_mant.IDORDEN, usuarioLogin.LOGIN);
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> SaveDet(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                if (model.pedido_mant.ACCION == "NEW")
                {
                    if (String.IsNullOrEmpty(model.pedido_mant.PEDIDODET.IDPRODUCTO))
                    {
                        response.success = false;
                        response.errorMessage = "Debe ingresar un codigo de producto.";
                        return Json(response);
                    }

                    List<PEDIDODET_DTO> lst_pedidodet = HttpContext.Session.GetComplexData<List<PEDIDODET_DTO>>("lst_pedidodet");
                    if (lst_pedidodet != null && lst_pedidodet.Count > 0)
                    {
                        if (lst_pedidodet.Any(x => x.IDPRODUCTO == model.pedido_mant.PEDIDODET.IDPRODUCTO))
                        {
                            response.success = false;
                            response.errorMessage = "Debe producto ya fue ingresado.";
                            return Json(response);
                        }
                        else {
                            model.pedido_mant.PEDIDODET.PRECIO_TOTAL = model.pedido_mant.PEDIDODET.CANTIDAD * model.pedido_mant.PEDIDODET.PRECIO_UNIT;
                            lst_pedidodet.Add(model.pedido_mant.PEDIDODET);
                            model.pedido_mant.DETALLES = lst_pedidodet;
                            response.result = model;
                            response.success = true;
                            HttpContext.Session.SetComplexData("lst_pedidodet", lst_pedidodet);
                        }
                    }
                    else
                    {
                        lst_pedidodet = new List<PEDIDODET_DTO>();
                        model.pedido_mant.PEDIDODET.PRECIO_TOTAL = model.pedido_mant.PEDIDODET.CANTIDAD * model.pedido_mant.PEDIDODET.PRECIO_UNIT;
                        lst_pedidodet.Add(model.pedido_mant.PEDIDODET);
                        model.pedido_mant.DETALLES = lst_pedidodet;
                        response.result = model;
                        response.success = true;
                        HttpContext.Session.SetComplexData("lst_pedidodet", lst_pedidodet);
                    }
                }
                else
                {
                    _logger.LogInformation("Modificando Pedido.");
                    model.pedido_mant.PEDIDODET.USUMODIFICA = usuarioLogin.LOGIN;
                    response = await HttpClientPedidoDet.Update(model.pedido_mant.PEDIDODET);                    
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> GetPedidoDet(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                if (String.IsNullOrEmpty(model.pedido_mant.IDORDEN))
                {
                    List<PEDIDODET_DTO> lst_pedidodet = HttpContext.Session.GetComplexData<List<PEDIDODET_DTO>>("lst_pedidodet");
                    if (lst_pedidodet != null && lst_pedidodet.Count > 0)
                    {
                        PEDIDODET_DTO pedidodet = lst_pedidodet.Where(x => x.IDPRODUCTO == model.pedido_mant.PEDIDODET.IDPRODUCTO).First()??null;
                        if (pedidodet == null)
                        {
                            response.success = false;
                            response.errorMessage = "Error al Editar producto.";
                            return Json(response);
                        }
                        else
                        {                            
                            model.pedido_mant.PEDIDODET = pedidodet;
                            response.result = model;
                            response.success = true;                         
                        }
                    }
                    else
                    {
                        response.success = false;
                        response.errorMessage = "Error al Editar producto, Session producto.";
                        return Json(response);
                    }
                }
                else {
                    _logger.LogInformation("Get Pedido.");
                    response = await HttpClientPedidoDet.Listar(model.pedido_mant.PEDIDODET.IDORDENDET, model.pedido_mant.IDORDEN);
                    if (response.success)
                    {
                        List<PEDIDODET_DTO> lst_pedidodet = JsonConvert.DeserializeObject<List<PEDIDODET_DTO>>(response.result.ToString());
                        model.pedido_mant.PEDIDODET = lst_pedidodet.First();
                        response.result = model;
                    }
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }
        public async Task<JsonResult> DeleteDet(PedidoModel model)
        {
            ResponseModel response = new ResponseModel();
            Usuario usuarioLogin = HttpContext.Session.GetComplexData<Usuario>("usuario_login");
            if (usuarioLogin != null)
            {
                if (String.IsNullOrEmpty(model.pedido_mant.IDORDEN))
                {
                    List<PEDIDODET_DTO> lst_pedidodet = HttpContext.Session.GetComplexData<List<PEDIDODET_DTO>>("lst_pedidodet");
                    if (lst_pedidodet != null && lst_pedidodet.Count > 0)
                    {
                        PEDIDODET_DTO pedidodet = lst_pedidodet.Where(x => x.IDPRODUCTO == model.pedido_mant.PEDIDODET.IDPRODUCTO).First() ?? null;
                        if (pedidodet == null)
                        {
                            response.success = false;
                            response.errorMessage = "Error al Eliminar producto.";
                            return Json(response);
                        }
                        else
                        {
                            if (lst_pedidodet.Remove(pedidodet)) {
                                HttpContext.Session.SetComplexData("lst_pedidodet", lst_pedidodet);
                                response.result = model;
                                response.success = true;
                            }
                            
                        }
                    }
                    else
                    {
                        response.success = false;
                        response.errorMessage = "Error al Editar producto, Session producto.";
                        return Json(response);
                    }
                }
                else
                {
                    _logger.LogInformation("Delete Pedido Det.");
                    response = await HttpClientPedidoDet.Delete(model.pedido_mant.PEDIDODET.IDORDENDET,usuarioLogin.LOGIN);
                    if (response.success)
                    {                        
                        response.result = model;
                    }
                }
            }
            else
            {
                response.success = false;
                response.errorMessage = "Se ha perdido la Sesion, por favor iniciar sesion nuevamente.";
            }
            return Json(response);
        }

        public async Task<JsonResult> LimpiarSession() {
            ResponseModel response = new ResponseModel();
            try
            {                
                HttpContext.Session.Remove("lst_pedidodet");
                response.success = true;
                response.result = "Sesion removida correctamente.";
            }
            catch (Exception ex) {
                response.success = false;
                response.errorMessage = ex.Message;
            }
            return Json(response);
            
        }
    }
}
