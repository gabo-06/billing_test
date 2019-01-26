
var IPQueAccede = "192.168"
var RedQueAccede = ""
var ReferenciaGlobalNode = "http://127.0.0.1:6962/socket.io/socket.io.js";
var ServidorPuertoNode = "http://127.0.0.1:6962";

//var ruta = $("#divRmdDlg").attr("url_obtenerip");
//alert(url_obtenerip);
$.ajax({
    async: false,
    url: url_obtenerip,
    contentType: "application/json",
    dataType: 'json',
    success: function (Data)
    {
        IPQueAccede = Data;
    }
});

RedQueAccede = IPQueAccede.substring(0, 7);
console.log("------------------------------------------------------------");
console.log("------------------------------------------------------------");
console.log("------------------------------------------------------------");

console.log("IP que está tratando de acceder: ");
console.log(IPQueAccede);
console.log("------------------------------------------------------------");
console.log("Rede de la IP que está tratando de acceder: ");
console.log(RedQueAccede)
console.log("------------------------------------------------------------");

if (RedQueAccede == "192.168" || RedQueAccede == "::1")
// if (RedQueAccede == "192.168" )
{
    
    console.log("Se está accediendo desde la red local");
     //ReferenciaGlobalNode = "http://localhost:6960/socket.io/socket.io.js";
     //ServidorPuertoNode = "http://localhost:6960";

    ReferenciaGlobalNode = "http://192.168.3.242:6969/socket.io/socket.io.js";
    ServidorPuertoNode = "http://192.168.3.242:6969";
}
/*else
{
    console.log("Se está accediendo desde una red remota");

    ReferenciaGlobalNode = "https://50.78.117.117:6962/socket.io/socket.io.js";
    ServidorPuertoNode = "https://50.78.117.117:6962";
}*/
