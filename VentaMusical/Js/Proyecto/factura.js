let tableUsuario = '#usuarioTable';
let codigoUsuarioTxt = '#CodigoUsuario';
let nombreUsuarioTxt = '#NombreUsuario';
let modalUsuario = '#modalUsuario';


let tableCanciones = '#cancionTable';
let modalCancion = '#modalCancion';

$(document).ready(function () {

    //SELECCIONAR USUARIO
    $(tableUsuario).on('click', '.select-user', function () {

        var usuarioId = $(this).data('id');
        var usuarioNombre = $(this).data('nombre');

        $(codigoUsuarioTxt).val(usuarioId);
        $(nombreUsuarioTxt).val(usuarioNombre);

        $(modalUsuario).modal('hide');
    });

    //SELECCIONAR FILA
    $(tableCanciones).on('click', '.select-cancion', function () {
        // Obtener los datos del botón seleccionado
        var codigo = $(this).data('codigo');
        var nombre = $(this).data('nombre');
        var precio = $(this).data('precio');

        // Agregar una nueva fila a la tabla de la factura
        var newRow = `<tr>
                    <td>${codigo}</td>
                    <td>${nombre}</td>
                    <td><input type="number" class="form-control cantidad" value="1" min="1"></td>
                    <td>${precio}</td>
                    <td class="subtotal">${precio}</td>
                    <td>
                        <button class="btn btn-danger btn-sm remove-row">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </td>
                </tr>`;

        $('#facturaTable tbody').append(newRow);

        // Cerrar el modal
        $(modalCancion).modal('hide');

    });
        
});

