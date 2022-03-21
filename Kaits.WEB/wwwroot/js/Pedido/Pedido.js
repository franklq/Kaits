var btnBuscar = document.getElementById("btnBuscar");
var btnNuevo = document.getElementById("Nuevo");
var btnRetroceder = document.getElementById("Retroceder");
var btnGuardar = document.getElementById("Guardar");
var Listado = document.getElementById("Listado");
var Mantenimiento = document.getElementById("Mantenimiento");
var spinner = new bootstrap.Modal(document.getElementById("Cargamodal"), {});
var MsgAlertafiltro = new bootstrap.Modal(document.getElementById("MsgAlertafiltro"), {});
var MsgAlerta = new bootstrap.Modal(document.getElementById("MsgAlerta"), {});
var MsgAlertaDet = new bootstrap.Modal(document.getElementById("MsgAlertaDet"), {});
var ConfirmaEliminar = new bootstrap.Modal(document.getElementById("ConfirmaEliminar"), {});
var btnAceptarEliminar = document.getElementById("btnAceptarEliminar");
var btnCancelarEliminar = document.getElementById("btnCancelarEliminar");

var btnNuevoDet = document.getElementById("NuevoDet");
var btnRetrocederDet = document.getElementById("RetrocederDet");
var btnGuardarDet = document.getElementById("GuardarDet");
var ListadoDet = document.getElementById("ListadoDet");
var MantenimientoDet = document.getElementById("MantenimientoDet");
var tblPedido = document.getElementById("tblPedido");

var btnBuscarCliente = document.getElementById("btnBuscarCliente");
var btnBuscarClienteMant = document.getElementById("btnBuscarClienteMant");
var btnBuscarClienteModal = document.getElementById("btnBuscarClienteModal");
var btnBuscarProducto = document.getElementById("btnBuscarProducto");
var ClienteModal = new bootstrap.Modal(document.getElementById("ClientesModal"), {});
var idcliente_filtro = document.getElementById("pedido_filtro_IDCLIENTE");
var idcliente_mant = document.getElementById("pedido_mant_IDCLIENTE");

var ProductosModal = new bootstrap.Modal(document.getElementById("ProductosModal"), {});
var idproducto_mant = document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO");

fnValidaSesion();
document.getElementById("pedido_filtro_FECDESDE").value = fnGetPrimerDiaAnio();
document.getElementById("pedido_filtro_FECHASTA").value = fnGetUltimoDia();
var datatable = new DataTable(tblPedido);
fnLista();

function fnValidaFiltro() {
    let validoFiltro = true;    
    const fechaini = document.getElementById("pedido_filtro_FECDESDE").value;
    const fechafin = document.getElementById("pedido_filtro_FECHASTA").value;

    if (fechaini != null && fechaini != 'null' && fechaini.length >= 10) {
        document.getElementById("fechainival").hidden = true;
    } else {
        document.getElementById("fechainival").hidden = false;
        validoFiltro = false;
    }

    if (fechafin != null && fechafin != 'null' && fechafin.length >= 10) {
        document.getElementById("fechafinval").hidden = true;
    } else {
        document.getElementById("fechafinval").hidden = false;
        validoFiltro = false;
    }
    return validoFiltro;
}

btnBuscar.onclick = async function () {
    await fnValidaSesion();
    const validafiltro = fnValidaFiltro();
    if (validafiltro) {
        datatable.clear();
        datatable.destroy();
        await fnLista();
        tblPedido = document.getElementById("tblPedido")
        datatable = new DataTable(tblPedido);
    } else {
        MsgAlertafiltro.show();
    }
}

btnNuevo.onclick = async function () {
    await fnValidaSesion();
    let accion = document.getElementById("pedido_mant_ACCION");

    accion.value = "NEW";
    Listado.hidden = true;
    Mantenimiento.hidden = false;

    btnNuevo.hidden = true;
    btnRetroceder.hidden = false;
    btnGuardar.hidden = false;
    await fnLimpiarSesion();

    ListadoDet.hidden = false;
    MantenimientoDet.hidden = true;

    btnNuevoDet.hidden = false;
    btnRetrocederDet.hidden = true;
    btnGuardarDet.hidden = true;
    fnLimpiarDet()
    await fnListaDet();
}

btnRetroceder.onclick = async function () {
    await fnValidaSesion();
    Listado.hidden = false;
    Mantenimiento.hidden = true;

    btnNuevo.hidden = false;
    btnRetroceder.hidden = true;
    btnGuardar.hidden = true;
    document.getElementById("frmMantenimiento").reset();
    document.getElementById("pedido_mant_IDORDEN").value = "";
    datatable.clear();
    datatable.destroy();
    await fnLista();
    tblPedido = document.getElementById("tblPedido")
    datatable = new DataTable(tblPedido);
    await fnLimpiarSesion();
}

btnGuardar.onclick = async function () {
    await fnValidaSesion();
    const ValidaOrden = fnValidaOrden();
    if (ValidaOrden) {
        await fnSave();
        await fnLimpiarSesion();
    } else {
        MsgAlerta.show();
    }
        
}

async function fnLista() {
    try {
        const body = document.getElementById('gdListaBody');
        let html_body = '';
        let filtro = new FormData(document.getElementById("frmListado"));        
        spinner.show();
        const responseModel = await fetch('/Pedido/Lista/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            response.result.map(function (pedido) {

                const botones = "<a rel='" + pedido.idorden + "' onclick='fnUpdate(rel)' class='btn btn-warning text-white' title='Editar'><i class='bi bi-pencil-fill'></i></a>\n\
                                    <a rel='" + pedido.idorden + "' onclick = 'fnDelete(rel)'   class='btn btn-danger text-white'  title ='Eliminar'><i class='bi bi-trash'></i></a> ";
                //const botones = "<a rel='" + pedido.idorden + "' onclick = 'fnDelete(rel)'   class='btn btn-danger text-white'  title ='Eliminar'><i class='bi bi-trash'></i></a> ";

                html_body += '<tr>\n\
                                    <td>' + pedido.idorden + '</td>\n\
                                    <td>' + pedido.cliente.dni+'-'+pedido.cliente.nombres + ' ' + pedido.cliente.apellidos + '</td>\n\
                                    <td>' + pedido.preciO_ORDEN + '</td>\n\
                                    <td>' + fnDateSqltoVista(pedido.fecorden) + '</td>\n\
                                    <td>' + pedido.usuregistro + '</td>\n\
                                    <td>'+ botones + '</td>\n\
                                 </tr>';
            });

            body.innerHTML = html_body;            
            spinner.hide();
        } else {
            html_body += '<tr>\n\
                                    <td colspan="5"> No se encontraron resultados </td>\n\
                                 </tr>';
            body.innerHTML = html_body;
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

async function fnDelete(IDORDEN) {
    try {
        fnValidaSesion();
        document.getElementById("pedido_mant_IDORDEN").value = IDORDEN;

        let data = new FormData(document.getElementById("frmMantenimiento"));
        spinner.show();
        const responseModel = await fetch('/Pedido/Delete/', { method: 'POST', body: data });
        const response = await responseModel.json();
        if (response.success) {
            await fnListaRepositorio();
            spinner.hide();
            toastr.info('Registro eliminado.', 'Mensaje de Confirmacion', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
            document.getElementById("frmMantenimiento").reset();
        } else {
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
    document.getElementById("pedido_mant_IDORDEN").value = "";
}

async function fnSave() {
    let filtro = new FormData(document.getElementById("frmMantenimiento"));
    try {               
        spinner.show();
        const responseModel = await fetch('/Pedido/Save/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {

            Listado.hidden = false;
            Mantenimiento.hidden = true;

            btnNuevo.hidden = false;
            btnRetroceder.hidden = true;
            btnGuardar.hidden = true;


            if (document.getElementById("pedido_mant_ACCION").value == "NEW") {
                toastr.info('Orden de pedido generada correctamente.', 'Mensaje de Confirmacion', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
            } else {
                toastr.info('Orden de pedido modificada correctamente.', 'Mensaje de Confirmacion', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
            }
            document.getElementById("frmMantenimiento").reset();
            fnLista();
            spinner.hide();
        } else {
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }              
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

function fnDelete(rel) {
    document.getElementById("pedido_mant_IDORDEN").value = rel;
    ConfirmaEliminar.show();
}

btnAceptarEliminar.onclick = async function () {
    let filtro = new FormData(document.getElementById("frmMantenimiento"));
    try {
        spinner.show();
        const responseModel = await fetch('/Pedido/Delete/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            fnLista();
            spinner.hide();
        } else {
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

btnCancelarEliminar.onclick = async function () {
    document.getElementById("pedido_mant_IDORDEN").value = "";
}

async function fnUpdate(rel) {
    document.getElementById("pedido_mant_IDORDEN").value = rel;
    let filtro = new FormData(document.getElementById("frmMantenimiento"));
    try {
        spinner.show();
        const responseModel = await fetch('/Pedido/GetPedido/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {

            Listado.hidden = true;
            Mantenimiento.hidden = false;

            btnNuevo.hidden = true;
            btnRetroceder.hidden = false;
            btnGuardar.hidden = false;

            document.getElementById("pedido_mant_ACCION").value = response.result.accion;
            document.getElementById("pedido_mant_IDCLIENTE").value = response.result.idcliente;
            document.getElementById("pedido_mant_DSCCLIENTE").value = response.result.cliente.nombres + "" + response.result.cliente.apellidos;
            document.getElementById("pedido_mant_FECORDEN").value = fnDateVistatoSql(response.result.fecorden);
            document.getElementById("pedido_mant_PRECIO_ORDEN").value = response.result.preciO_ORDEN;

            await fnListaDet();
            spinner.hide();
        } else {
            document.getElementById("pedido_mant_IDORDEN").value = "";
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        document.getElementById("pedido_mant_IDORDEN").value = "";
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

function fnValidaOrden() {
    let validoOrden = true;
    const cliente = document.getElementById("pedido_mant_IDCLIENTE").value;
    const fecha = document.getElementById("pedido_mant_FECORDEN").value;
    if (cliente != null && cliente != 'null' && cliente.length == 12) {
        document.getElementById("clienteval").hidden = true;       
    } else {
        document.getElementById("clienteval").hidden = false;
        validoOrden = false;
    }

    if (fecha != null && fecha != 'null' &&  fecha.length >= 10) {
        document.getElementById("fechaval").hidden = true;        
    } else {
        document.getElementById("fechaval").hidden = false;
        validoOrden = false;
    }
 
    return validoOrden;
}

//ORDEN DETALLE
btnNuevoDet.onclick = async function () {
    await fnValidaSesion();
    fnLimpiarDet();  
    ListadoDet.hidden = true;
    MantenimientoDet.hidden = false;

    btnNuevoDet.hidden = true;
    btnRetrocederDet.hidden = false;
    btnGuardarDet.hidden = false;
}

btnRetrocederDet.onclick = async function () {
    await fnValidaSesion();
    ListadoDet.hidden = false;
    MantenimientoDet.hidden = true;

    btnNuevoDet.hidden = false;
    btnRetrocederDet.hidden = true;
    btnGuardarDet.hidden = true;
    fnLimpiarDet()
    fnListaDet();
}

btnGuardarDet.onclick = async function () {
    await fnValidaSesion();
    const validacion = fnValidaOrdenDet();
    if (validacion) {
        await fnSaveDet();
        fnLimpiarDet();
    } else {
        MsgAlertaDet.show();
    }
}

async function fnListaDet() {
    try {
        const body = document.getElementById('gdListaDetBody');
        const foot = document.getElementById('gdListaDetFoot');
        let html_body = '';
        let precio_total = 0
        let filtro = new FormData(document.getElementById("frmMantenimiento"));
        spinner.show();
        const responseModel = await fetch('/Pedido/ListaDet/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            response.result.map(function (pedidodet) {

                const botones = "<a rel='" + pedidodet.idordendet + "&" + pedidodet.idproducto+ "' onclick='fnUpdateDet(rel)' class='btn btn-warning text-white' title='Editar'><i class='bi bi-pencil-fill'></i></a>\n\
                                    <a rel='" + pedidodet.idordendet + "&" + pedidodet.idproducto + "' onclick = 'fnDeleteDet(rel)'   class='btn btn-danger text-white'  title ='Eliminar'><i class='bi bi-trash'></i></a>";

                html_body += '<tr>\n\
                                    <td class="text-center">' + botones + '</td>\n\
                                    <td>' + pedidodet.idproducto + '</td>\n\
                                    <td>' + pedidodet.dscproducto + '</td>\n\
                                    <td class="text-center">' + pedidodet.cantidad + ' UND</td>\n\
                                    <td class="text-center"> S/ ' + fnVistaContable(pedidodet.preciO_UNIT) + '</td>\n\
                                    <td class="text-center"> S/ ' + fnVistaContable(pedidodet.preciO_TOTAL) + '</td>\n\
                                 </tr>';

                precio_total = precio_total + pedidodet.preciO_TOTAL;
            });

            body.innerHTML = html_body;
            foot.innerHTML = '<tr><td colspan="5" class="text-center"><strong>Precio Total</strong></td><td class="text-center"> S/ ' + fnVistaContable(precio_total)+'</td></tr>';
            spinner.hide();
        } else {
            html_body += '<tr><td colspan="6" class="text-center"> No se encontraron resultados </td></tr>';
            body.innerHTML = html_body;
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

async function fnLimpiarSesion() {
    try {
        spinner.show();
        const responseModel = await fetch('/Pedido/LimpiarSession/', { method: 'POST', body: {} });
        const response = await responseModel.json();
        if (response.success) {            
            spinner.hide();
        } else {
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

async function fnUpdateDet(rel) {
    const datos = rel.split('&');    
    document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = datos[1];
    document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = datos[0];
    let filtro = new FormData(document.getElementById("frmMantenimiento"));
    try {
        spinner.show();
        const responseModel = await fetch('/Pedido/GetPedidoDet/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            fnLimpiarDet();
            ListadoDet.hidden = true;
            MantenimientoDet.hidden = false;

            btnNuevoDet.hidden = true;
            btnRetrocederDet.hidden = false;
            btnGuardarDet.hidden = false;
            const pedidodet = response.result.pedido_mant.pedidodet;
            document.getElementById("pedido_mant_PEDIDODET_ACCION").value = "UPD";
            document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = pedidodet.idproducto;
            document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = pedidodet.dscproducto;
            document.getElementById("pedido_mant_PEDIDODET_CANTIDAD").value = pedidodet.cantidad;
            document.getElementById("pedido_mant_PEDIDODET_PRECIO_UNIT").value = fnVistaContable(pedidodet.preciO_UNIT);
            document.getElementById("pedido_mant_PEDIDODET_PRECIO_TOTAL").value = fnVistaContable(pedidodet.preciO_TOTAL);
            document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = pedidodet.idordendet;
            spinner.hide();
        } else {
            document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = "";
            document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = "";
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = "";
        document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = "";
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }

}

async function fnDeleteDet(rel) {
    const datos = rel.split('&');
    document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = datos[1];
    document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = datos[0];
    let filtro = new FormData(document.getElementById("frmMantenimiento"));
    try {
        spinner.show();
        const responseModel = await fetch('/Pedido/DeleteDet/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            await fnListaDet();
            spinner.hide();
        } else {
            document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = "";
            document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = "";
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = "";
        document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = "";
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

async function fnSaveDet() {
    let filtro = new FormData(document.getElementById("frmMantenimiento"));
    try {       
        spinner.show();
        const responseModel = await fetch('/Pedido/SaveDet/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {

            ListadoDet.hidden = false;
            MantenimientoDet.hidden = true;

            btnNuevoDet.hidden = false;
            btnRetrocederDet.hidden = true;
            btnGuardarDet.hidden = true;

            if (document.getElementById("pedido_mant_ACCION").value == "NEW") {
                toastr.info('Orden de pedido generada correctamente.', 'Mensaje de Confirmacion', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
            } else {
                toastr.info('Orden de pedido modificada correctamente.', 'Mensaje de Confirmacion', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
            }
            fnLimpiarDet();
            fnListaDet();
            spinner.hide();
        } else {
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }             
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

function fnLimpiarDet() {
    document.getElementById("pedido_mant_PEDIDODET_ACCION").value = "NEW";
    document.getElementById("pedido_mant_PEDIDODET_IDORDENDET").value = "";
    document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = "";
    document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = "";
    document.getElementById("pedido_mant_PEDIDODET_CANTIDAD").value = 0;
    document.getElementById("pedido_mant_PEDIDODET_PRECIO_UNIT").value = 0;
    document.getElementById("pedido_mant_PEDIDODET_PRECIO_TOTAL").value = 0;
}

function fnValidaOrdenDet() {
    let valido = true;
    const idproducto = document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value;
    const cantidad = document.getElementById("pedido_mant_PEDIDODET_CANTIDAD").value;
    const preciounit = document.getElementById("pedido_mant_PEDIDODET_PRECIO_UNIT").value;
    if (idproducto != null && idproducto != 'null' && idproducto.length == 12) {
        document.getElementById("codproductoval").hidden = true;
    } else {
        document.getElementById("codproductoval").hidden = false;
        valido = false;
    }

    if (cantidad != null && cantidad != 'null' && cantidad > 0) {
        document.getElementById("cantidadval").hidden = true;
    } else {
        document.getElementById("cantidadval").hidden = false;
        valido = false;
    }

    if (preciounit != null && preciounit != 'null' && preciounit > 0) {
        document.getElementById("preciounitval").hidden = true;
    } else {
        document.getElementById("preciounitval").hidden = false;
        valido = false;
    }

    return valido;
}

// CLIENTES
btnBuscarCliente.onclick = async function () {
    ClienteModal.show();
    document.getElementById("cliente_filtro_TIPOFILTRO").value = "LISTA";
    await fnListaClientes();
}
btnBuscarClienteMant.onclick = async function () {
    ClienteModal.show();
    document.getElementById("cliente_filtro_TIPOFILTRO").value = "MANT";
    await fnListaClientes();
}
btnBuscarClienteModal.onclick = async function () {        
    await fnListaClientes();
}
idcliente_filtro.addEventListener("blur", async function (event) {
    const clientevalor = this.value;
    if (this.value !== "" && clientevalor.length==12) {
        await fnGetCliente();
    } else {
        document.getElementById("pedido_filtro_DSCCLIENTE").value = "";
    }
}, true);
idcliente_mant.addEventListener("blur", async function (event) {
    const clientevalor2 = this.value;
    if (this.value !== "" && clientevalor2.length == 12) {
        await fnGetCliente();
    } else {
        document.getElementById("pedido_mant_DSCCLIENTE").value = "";
    }
}, true);

async function fnListaClientes() {
    try {
        const body = document.getElementById('gdClientesBody');
        let html_body = '';
        document.getElementById("cliente_filtro_IDCLIENTE").value = "";
        let filtro = new FormData(document.getElementById("frmListadoClientes"));
        spinner.show();
        const responseModel = await fetch('/Pedido/ListaClientes/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            response.result.map(function (cliente) {

                const botones = "<a rel='" + cliente.idcliente + "&" + cliente.nombres + "&" + cliente.apellidos + "' onclick='fnSelectCliente(rel)' class='btn btn-info text-white' title='Seleccionar'><i class='bi bi-check'></i></a> ";

                html_body += '<tr>\n\
                                    <td>' + cliente.idcliente + '</td>\n\
                                    <td>' + cliente.dni + '-' + cliente.nombres + ' ' + cliente.apellidos + '</td>\n\
                                    <td>'+ botones + '</td>\n\
                                 </tr>';
            });

            body.innerHTML = html_body;
            spinner.hide();
        } else {
            html_body += '<tr>\n\
                                    <td colspan="3"> No se encontraron resultados </td>\n\
                                 </tr>';
            body.innerHTML = html_body;
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}
async function fnSelectCliente(CLIENTE) {
    const datos = CLIENTE.split('&');
    const TIPOFILTRO = document.getElementById("cliente_filtro_TIPOFILTRO").value;
    if (TIPOFILTRO == "LISTA") {
        idcliente_filtro.value = datos[0];
        document.getElementById("pedido_filtro_DSCCLIENTE").value = datos[1] + " " + datos[2];
    } else {
        idcliente_mant.value = datos[0];
        document.getElementById("pedido_mant_DSCCLIENTE").value = datos[1] + " " + datos[2];
    }
    ClienteModal.hide();
}
async function fnGetCliente() {
    const TIPOFILTRO = document.getElementById("cliente_filtro_TIPOFILTRO").value;
    try {        
        if (TIPOFILTRO == "LISTA") {
            document.getElementById("cliente_filtro_IDCLIENTE").value = idcliente_filtro.value;
        } else {
            document.getElementById("cliente_filtro_IDCLIENTE").value = idcliente_mant.value;
        }

        let filtro = new FormData(document.getElementById("frmListadoClientes"));
        spinner.show();
        const responseModel = await fetch('/Pedido/ListaClientes/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {            
            const cliente = response.result[0];
            spinner.hide();

            if (TIPOFILTRO == "LISTA") {
                idcliente_filtro.value = cliente.idcliente;
                document.getElementById("pedido_filtro_DSCCLIENTE").value = cliente.nombres + " " + cliente.apellidos;
            } else {
                idcliente_mant.value = cliente.idcliente;
                document.getElementById("pedido_mant_DSCCLIENTE").value = cliente.nombres + " " + cliente.apellidos;
            }
        
        } else {
            if (TIPOFILTRO == "LISTA") {
                idcliente_filtro.value = "";
                document.getElementById("pedido_filtro_DSCCLIENTE").value = "";
                document.getElementById("cliente_filtro_IDCLIENTE").value = "";
            } else {
                idcliente_mant.value = "";
                document.getElementById("pedido_mant_DSCCLIENTE").value = "";
                document.getElementById("cliente_filtro_IDCLIENTE").value = "";
            }
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        if (TIPOFILTRO == "LISTA") {
            idcliente_filtro.value = "";
            document.getElementById("pedido_filtro_DSCCLIENTE").value = "";
            document.getElementById("cliente_filtro_IDCLIENTE").value = "";
        } else {
            idcliente_mant.value = "";
            document.getElementById("pedido_mant_DSCCLIENTE").value = "";
            document.getElementById("cliente_filtro_IDCLIENTE").value = "";
        }
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}

// PRODUCTOS
idproducto_mant.addEventListener("blur", async function (event) {
    const idprodval = this.value;
    if (this.value !== "" && idprodval.length == 12) {
        await fnGetProducto();
    } else {
        document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = "";
    }
}, true);
btnBuscarProducto.onclick = async function () {
    ProductosModal.show();
    await fnListaProductos();
}
btnBuscarProductoModal.onclick = async function () {
    await fnListaProductos();
}

document.getElementById("pedido_mant_PEDIDODET_CANTIDAD").addEventListener("blur", async function (event) {
    let precio_unit = document.getElementById("pedido_mant_PEDIDODET_PRECIO_UNIT");
    let precio_total = document.getElementById("pedido_mant_PEDIDODET_PRECIO_TOTAL");

    if (this.value > 0 && this.value != null) {
        if (precio_unit.value > 0 && precio_unit.value != null) {
            precio_total.value = this.value * precio_unit.value;
        } else {
            precio_total.value = 0;
        }
    } else {
        precio_total.value = 0;
    }
}, true)

document.getElementById("pedido_mant_PEDIDODET_PRECIO_UNIT").addEventListener("blur", async function (event) {
    let precio_total = document.getElementById("pedido_mant_PEDIDODET_PRECIO_TOTAL");
    let cantidad = document.getElementById("pedido_mant_PEDIDODET_CANTIDAD");
    if (this.value > 0 && this.value != null) {
        if (cantidad.value > 0 && cantidad.value != null) {
            precio_total.value = this.value * cantidad.value;
        } else {
            precio_total.value = 0;
        }
    } else {
        precio_total.value = 0;
    }
}, true)

async function fnListaProductos() {
    try {
        const body = document.getElementById('gdProductosBody');
        let html_body = '';
        idproducto_mant.value = "";
        document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = "";
        let filtro = new FormData(document.getElementById("frmListadoProductos"));
        spinner.show();
        const responseModel = await fetch('/Pedido/ListaProductos/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            response.result.map(function (producto) {

                const botones = "<a rel='" + producto.idproducto + "&" + producto.dscproducto + "' onclick='fnSelectProducto(rel)' class='btn btn-info text-white' title='Seleccionar'><i class='bi bi-check'></i></a> ";

                html_body += '<tr>\n\
                                    <td>' + producto.idproducto + '</td>\n\
                                    <td>' + producto.dscproducto + '</td>\n\
                                    <td>'+ botones + '</td>\n\
                                 </tr>';
            });

            body.innerHTML = html_body;
            spinner.hide();
        } else {
            html_body += '<tr>\n\
                                    <td colspan="3"> No se encontraron resultados </td>\n\
                                 </tr>';
            body.innerHTML = html_body;
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}
async function fnGetProducto() {
    try {
        document.getElementById("producto_filtro_IDPRODUCTO").value = document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value;
        document.getElementById("producto_filtro_DSCPRODUCTO").value = document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value;
        let filtro = new FormData(document.getElementById("frmListadoProductos"));
        spinner.show();
        const responseModel = await fetch('/Pedido/ListaProductos/', { method: 'POST', body: filtro });
        const response = await responseModel.json();
        if (response.success) {
            const producto = response.result[0];
            spinner.hide();
            document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = producto.dscproducto;
            document.getElementById("producto_filtro_IDPRODUCTO").value = "";
            document.getElementById("producto_filtro_DSCPRODUCTO").value = "";
        } else {
            document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = "";
            document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = "";
            document.getElementById("producto_filtro_IDPRODUCTO").value = "";
            document.getElementById("producto_filtro_DSCPRODUCTO").value = "";
            spinner.hide();
            toastr.error(response.errorMessage, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
        }
    } catch (error) {
        document.getElementById("pedido_mant_PEDIDODET_IDPRODUCTO").value = "";
        document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = "";
        document.getElementById("producto_filtro_IDPRODUCTO").value = "";
        document.getElementById("producto_filtro_DSCPRODUCTO").value = "";
        spinner.hide();
        toastr.error(error, 'Error', { positionClass: 'toastr toast-top-right', containerId: 'toast-top-right', "closeButton": true });
    }
}
async function fnSelectProducto(PRODUCTO) {
    const datos = PRODUCTO.split('&');
    idproducto_mant.value = datos[0];
    document.getElementById("pedido_mant_PEDIDODET_DSCPRODUCTO").value = datos[1];
    ProductosModal.hide();
}

async function fnValidaSesion() {
    var URLactual = parent.window.location;
    var urlValida = URLactual.href;
    var host = URLactual.host;
    var UrlInicio = "http://" + host + "/Home";

    const responseModel = await fetch('/Home/ValidaSesion/', { method: 'POST', body: {} });
    const response = await responseModel.json();
    if (!response.success) {
        window.parent.location.replace(UrlInicio)
        window.parent.opener.document.location = UrlInicio;
        parent.location.reload();
    } 
}
function fnGetPrimerDiaAnio() {
    let date = new Date();
    let primerDia = new Date(date.getFullYear(), 0, 1);
    return formatDate(primerDia)

}
function fnGetPrimerDia() {
    let date = new Date();
    let primerDia = new Date(date.getFullYear(), date.getMonth(), 1);
    return formatDate(primerDia)

}
function fnGetUltimoDia() {
    let date = new Date();
    let ultimoDia = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    return formatDate(ultimoDia)
}
function formatDate(date) {
    if (!fnValidaString(date)) {
        return date
    } else {
        let d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }
}
function fnDateSqltoVista(fecha) {
    let fechares;
    if (!fnValidaString(fecha)) {
        fechares = "";
    } else {
        let day = fecha.substring(8, 10);
        let month = fecha.substring(5, 7);
        let year = fecha.substring(0, 4);

        fechares = day + "/" + month + "/" + year;
    }
    return fechares;
}
function fnDateVistatoSql(fecha) {
    let fechares;
    if (!fnValidaString(fecha)) {
        fechares = "";
    } else {
        let day = fecha.substring(8, 10);
        let month = fecha.substring(5, 7);
        let year = fecha.substring(0, 4);

        fechares = year + "-" + month + "-" + day;
    }
    return fechares;
}
function fnValidaString(campo) {
    if (campo == null || campo == 'null' || campo == ' ' || campo == '' || campo == 'undefined') {
        return false;
    } else {
        return true;
    }
}
function fnVistaContable(x) {
    return Number.parseFloat(x).toFixed(3);
}