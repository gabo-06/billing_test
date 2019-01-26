

$(document).on("ready", InicioFunciones);


function InicioFunciones() {
    CargaCiudades();
    //ValidaSesion();
    //window.history.forward(1);
    /*if (history.forward(1)) {
        console.log("at");
        location.replace(history.forward(1));
    }*/
   

}
//function ValiaUrl() {
//    var l = window.location;
//    var host = l.host;
//    carpeta = l.pathname.split('/')[1];
//    controlador = l.pathname.split('/')[2];
    
//}

function ValidaSesion()
{

//    var carpeta = $("#subCaperta").val();
    $.ajax({
        type: "POST",
        async: false,
        url: carpeta + "/Account/SesionValidate",
        //url: $("#UrlSesionValidate").val(),
        
        contentType: "application/json",
        dataType: 'json',
        success: function (response) {
            //console.log("validandoXX");
            if (response.sessionlost)
            {
                localStorage.setItem("user_info", "");
                console.log("/" + carpeta + "/Account/Login");
                console.log("aki");

                window.location = "/" + carpeta + "/Account/Login";
            }
            else
            {
                localStorage.setItem("user_info", response.usuario);
                $("#usuario_logueado").html(response.usuario);
            }
        }
    });

}

function CargaCiudades()
{
    
    var carpeta = $("#archivoCiudades").val();
    
$.ajax({
    async: true,
    url:  carpeta ,
    contentType: "application/json",
    dataType: 'json',
    cache: true,
    headers: {
        'Cache-Control': 'max-age=604800, public'
    },
    success: function (data) {
        // console.log("carga cache ciudades");      
    }
});
}


//$('input.numeros').live('keypress', function (e) {
//    // permite numeros y espacios
//    // 40 y 41 => ()
//    // 32 => espacio
//    // 8 => backespace
//    // 0 => suprimir y teclas direccion    
//    if (!((e.which >= 48 && e.which <= 57) || (e.which == 40 || e.which == 41) || (e.which == 32 || e.which == 8 || e.which == 0))) {
//        e.preventDefault();
//        return false;
//    }
//});

//$('input.letter').live('keypress', function (e) {
//    // permite letras y espacios
//    // 32 => espacio
//    // 8 => backespace
//    // 0 => suprimir y teclas direccion
//    // ñ y Ñ => 209 y 241
//    // áéíóú => 225,233,237,243,250
//    // ÁÉÍÓÚ => 193,201,205,211,218
//    if ((e.which == 209 || e.which == 241) || (e.which == 225 || e.which == 233 || e.which == 237 || e.which == 243 || e.which == 250) || (e.which == 193 || e.which == 201 || e.which == 205 || e.which == 211 || e.which == 218)) {

//    } else {
//        if (!((e.which >= 65 && e.which <= 90) || (e.which == 32 || e.which == 8 || e.which == 0) || (e.which >= 97 && e.which <= 122))) {
//            e.preventDefault();
//            return false;
//        }
//    }
//});