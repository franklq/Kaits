const pwd = document.getElementById("usuario_login_PASS");
const user = document.getElementById("usuario_login_LOGIN");

pwd.addEventListener("blur",async  function (event) {
    await pwdchange();
}, true);
//***********************************************************************************REVISANDO EN RAMA carlosp (PRUEBA DE COMMIT)**********************************************************
async function pwdchange () {
    try {
        let datoslogueo = new FormData();
        const div_roles = document.getElementById("divRol");
        const div_errorlogin = document.getElementById("bmsgrespuestalogin");
        const btnlogin = document.getElementById("btnInicioSesion");
        const msgerror_login = document.getElementById("msgrespuestalogin");

        datoslogueo.append('usuario_login.LOGIN', user.value);
        datoslogueo.append('usuario_login.PASS', pwd.value);
        const responseModel = await fetch('/Home/PwdChange/', { method: 'POST', body: datoslogueo });
        const response = await responseModel.json();
        if (response.success) {
            msgerror_login.append("");
            myselect = document.getElementById('usuario_login_rolUsuario_IDROLE');
            response.result.userRolesApp.map(function (rol) {
                myOption = document.createElement("option");
                myOption.text = rol.namerole;
                myOption.value = rol.idrole;
                myselect.appendChild(myOption);
            })

            div_roles.style.display = "block";
            div_errorlogin.style.display = "none";
            btnlogin.disabled = false;
            //div_roles.removeAttribute("hidden");
        } else {
            div_roles.style.display = "none";
            div_errorlogin.style.display = "block";
            
            msgerror_login.append("");
            msgerror_login.append(response.errorMessage);
            btnlogin.disabled = true;
        }

        console.log(response);
    } catch (error) {
        console.log(error);
    }
}