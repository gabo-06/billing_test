var UsuarioGlobal; // Variable global que obtiene los datos del usuario logueado cuando inicia la aplicación y que son consumidos desde cualquier parte de la aplicación cuando se necesita por ejemplo el código del usuario.
var CodigoUsuarioGlobal; // Variable global que almacena el código del usuario logueado.
var NombreUsuarioGlobal; // Variable global que almacena el nombre del usuario logueado.
var ApellidoUsuarioGlobal; // Variable global que almacena el apellido del usuario logueado.
var RolUsuarioGlobal; // Variable global que almacena el rol del usuario logueado.
var BillManager; // Variable global que especifica si el usario logueado tienen rol "Bill Manager"  1: si / 0: no.

var carpeta = "";
//var carpeta = "/billing.web";//carpeta del proyecto
 
$.ajax({
    async: false, 
    url: carpeta + '/Account/ObtenerUsuarioLogueado',
    contentType: "application/json",
    dataType: 'json',
    success: function (Data)
    {
        UsuarioGlobal = Data;
        CodigoUsuarioGlobal = UsuarioGlobal.Use_code;
        NombreUsuarioGlobal = UsuarioGlobal.Use_firstName;
        ApellidoUsuarioGlobal = UsuarioGlobal.Use_lastName;
        RolUsuarioGlobal = UsuarioGlobal.Use_rolName;
        BillManager = UsuarioGlobal.BillManager;
    }
});