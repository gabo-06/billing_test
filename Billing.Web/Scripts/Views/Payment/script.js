$(Inicio)

function calculaSuma()
{
    var miVariable1 = 10;
    var miVariable2 = 20;
    var miVariable3 = 30;
    var miVariable4 = 40;
    var miVariable5 = 50;

    var resultado = miVariable1 + miVariable2;
    var resultado2 = resultado + miVariable3;
    var resultado3 = resultado2 + miVariable4;
    var resultado4 = resultado3 + miVariable4;
    // var resultado5 = resultado4 + e.data.miParametro;
}

function Inicio()
{
    // $('#btnPrueba').on('click', { 'miParametro': 85 }, calculaSuma);
    calculaSuma();
    console.log('aaa');
}