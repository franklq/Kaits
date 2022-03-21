var divInactividad = document.getElementById("divInactividad");
var usuarioNombre = document.getElementById("usuario-nombre");
var userNombre = document.getElementById("user-nombre");
var usuarioEmail = document.getElementById("usuario-email");
var timeout = 0;
var validar = 0;

window.setInterval(async function () {
    if (validar == 0) {
        fnValidaSesionMenu();

        validar++;
    }
    timeout++;
    if (timeout >= 830) {
        if (timeout == 830) {
            divInactividad.removeAttr
            fnAlertaSesion();
        }
        document.getElementById("time_inactivo").innerHTML = fnsecondsToString(timeout);
    }
}, 1000);

document.onmousemove = function () {
    timeout = 0;
    divInactividad.hidden = true
}

const fnLogut = async () => {
    validar = 0;
    var URLactual = parent.window.location;
    var urlValida = URLactual.href;
    var host = URLactual.host;
    var UrlInicio = "http://" + host + "/Home";

    const responseModel = await fetch('/Home/Logout/', { method: 'POST', body: {} });
    const response = await responseModel.json();
    if (response.success) {
        window.parent.location.replace(UrlInicio)
        window.parent.opener.document.location = UrlInicio;
        parent.location.reload();
    }
}
const fnValidaSesionMenu = async () => {
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
    } else {
        usuarioNombre.innerHTML = response.result.lastname + " " + response.result.firstname;
        userNombre.innerHTML = response.result.lastname + " " + response.result.firstname;
        usuarioEmail.innerHTML = response.result.email;
    }
}
function fnsecondsToString(seconds) {
    let hour = Math.floor(seconds / 3600);
    hour = (hour < 10) ? '0' + hour : hour;
    let minute = Math.floor((seconds / 60) % 60);
    minute = (minute < 10) ? '0' + minute : minute;
    let second = seconds % 60;
    second = (second < 10) ? '0' + second : second;
    return hour + ':' + minute + ':' + second;
}
function fnAlertaSesion() {
    $.confirm({
        title: 'Alerta de Inactividad!',
        content: 'La sesión va a expirar.',
        autoClose: 'expirar|10000',
        type: 'red',
        icon: 'fa fa-spinner fa-spin',
        buttons: {
            expirar: {
                text: 'Cerrar Sesión',
                btnClass: 'btn-red',
                action: function () {
                    fnLogut();
                }
            }
        }
    });
}
