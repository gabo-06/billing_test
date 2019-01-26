var server = require("socket.io").listen(6960);
var sql = require('mssql');

function EstablecerParametrosConexion()
{
	var connection = new sql.Connection({
		user: 'sa',
		password: '123456@abc',
		server: '192.168.3.242',
		database: 'OmnimedBD'
	});

	return connection;
}

var connection;
connection = EstablecerParametrosConexion()
ConectarBaseDatos(connection)

server.sockets.on("connection", Arranque);

function Arranque(usuario)
{
	// Esta función actualiza los pacientes en el proceso "Case Information Sheet" después de haberse registrado un nuevo paciente en el maestro de pacientes.
	usuario.on('funcionRegistroNuevoPaciente', actualizaInformacionPacientesEnCase);	

	// Esta función actualiza la trazabilidad de paciente.
	usuario.on('actualizaTrazabilidadPaciente', actualizaTrazabilidadPaciente);

	// Esta función actualiza la trazabilidad de medico.
	usuario.on('actualizaTrazabilidadMedico', actualizaTrazabilidadMedico);

	// Esta función actualiza la trazabilidad de medico.
	usuario.on('actualizaTrazabilidadAseguradora', actualizaTrazabilidadAseguradora);

	// Esta función actualiza la trazabilidad de medico.
	usuario.on('actualizaTrazabilidadAbogado', actualizaTrazabilidadAbogado);

	// Esta función actualiza la trazabilidad de medico.
	usuario.on('actualizaTrazabilidadAjustador', actualizaTrazabilidadAjustador);

	// Esta función actualiza la trazabilidad de medico.
	usuario.on('actualizaTrazabilidadProveedor', actualizaTrazabilidadProveedor);

	// Esta función actualiza la trazabilidad de actividad.
	usuario.on('actualizaTrazabilidadActividad', actualizaTrazabilidadActividad);

	// Esta función actualiza la trazabilidad de especialidad.
	usuario.on('actualizaTrazabilidadEspecialidad', actualizaTrazabilidadEspecialidad);
}

function actualizaInformacionPacientesEnCase()
{
    server.sockets.emit("funcionPacienteDesdeNode");
}

function actualizaTrazabilidadPaciente(data)
{
	server.sockets.emit('respuestaTrazabilidadPaciente', data)
}

function actualizaTrazabilidadMedico(data)
{
	server.sockets.emit('respuestaTrazabilidadMedico', data)
}

function actualizaTrazabilidadAseguradora(data)
{
	server.sockets.emit('respuestaTrazabilidadAseguradora', data)
}

function actualizaTrazabilidadAbogado(data)
{
	server.sockets.emit('respuestaTrazabilidadAbogado', data)
}

function actualizaTrazabilidadAjustador(data)
{
	server.sockets.emit('respuestaTrazabilidadAjustador', data)
}

function actualizaTrazabilidadProveedor(data)
{
	server.sockets.emit('respuestaTrazabilidadProveedor', data)
}

function actualizaTrazabilidadActividad(data)
{
	server.sockets.emit('respuestaTrazabilidadActividad', data)
}

function actualizaTrazabilidadEspecialidad(data)
{
	server.sockets.emit('respuestaTrazabilidadEspecialidad', data)
}


function prueba(data)
{	
	var request = new sql.Request(connection);
    
	request.query('select top 2 Pat_code, Pat_firstName, Pat_lastName from Patient', function(err, recorset)
	{		
		if (err)
		{
			throw err;
		}
		else
		{
			if(recorset.length > 0)
			{
	      		for (var i = 0; i < recorset.length; i++) 
	      		{
	      	 		/*
	      	 		console.log(recorset[i].Pat_code + ' ' + 
	      	 			 		recorset[i].Pat_firstName + ' ' + 
	      	 			 		recorset[i].Pat_lastName);
					*/
	      		}				
				
	    		server.sockets.emit("funcionEnviadaDesdeNode", recorset);
	      	}
	      	else
	      	{
				console.log('Registro no encontrado');
	      	}
		}
	});

	connection.close();
}

function chat_message(data)
{
	server.sockets.emit("chat_message", data);
}

function ConectarBaseDatos(connection)
{
 	connection.connect(function(error)
	{
		if(error)
		{
			console.log('Falló la conexion.');
		  	connection.close();		
		  // throw error;		  
		}
		else
		{
		  	console.log('Conexion correcta.');
		}
	});
}

console.log('Node esta corriendo')