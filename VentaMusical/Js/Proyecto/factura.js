let tableUsuario = '#usuarioTable';
let codigoUsuarioTxt = '#CodigoUsuario';
let nombreUsuarioTxt = '#NombreUsuario';
let modalUsuario = '#modalUsuario';

$(document).ready(function () {

    //SELECCIONAR USUARIO
    $(tableUsuario).on('click', '.select-user', function () {

        var usuarioId = $(this).data('id');
        var usuarioNombre = $(this).data('nombre');

        $(codigoUsuarioTxt).val(usuarioId);
        $(nombreUsuarioTxt).val(usuarioNombre);

        $(modalUsuario).modal('hide');
    });
        
});

