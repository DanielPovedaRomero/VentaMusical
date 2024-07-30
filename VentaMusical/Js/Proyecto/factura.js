let tableUsuario = '#usuarioTable';
let codigoUsuarioTxt = '#CodigoUsuario';
let nombreUsuarioTxt = '#NombreUsuario';
let modalUsuario = '#modalUsuario';


let tableCanciones = '#cancionTable';
let modalCancion = '#modalCancion';

let tableFactura = '#facturaTable';
let numeroFactura = '#NumeroFactura';
let subTotal = '#subtotal';
let total = '#total';
let formaPago = '#selectFormasDePago';

let btnGuardar = '#btnGuardar';

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

        // Cargar impuestos en el select
        CargarImpuestos(function (selectImpuestoHtml) {
            // Calcular el subtotal y el impuesto
            var subtotal = parseFloat(precio);
            var impuestoPorcentaje = $(selectImpuestoHtml).find('option:selected').data('porcentaje') || 0;
            var montoImpuesto = subtotal * (impuestoPorcentaje / 100);
            var total = subtotal + montoImpuesto;

            // Agregar una nueva fila a la tabla de la factura
            var newRow = `
                <tr class="text-center">
                    <td class='d-none'>${codigo}</td>
                    <td>${nombre}</td>
                    <td><input type="number" class="form-control form-control-sm quantity" value="1" min="1" /></td>
                    <td>${precio}</td>
                    <td>${selectImpuestoHtml}</td>
                    <td class="subtotal">${subtotal.toFixed(2)}</td>
                    <td class="total">${total.toFixed(2)}</td>
                    <td>
                        <button class="btn btn-danger btn-sm remove-row">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </td>
                </tr>
            `;

            $(`${tableFactura} tbody`).append(newRow);

            // Actualizar los totales
            CalcularTotales();

            // Cerrar el modal
            $(modalCancion).modal('hide');
        });
    });

    // Evento para actualizar el total cuando cambia la cantidad o el impuesto
    $(document).on('input', '.quantity, .impuesto-select', function () {

        var $row = $(this).closest('tr');
        var quantity = parseFloat($row.find('.quantity').val());
        var price = parseFloat($row.find('td:nth-child(4)').text());
        var taxPercentage = parseFloat($row.find('.impuesto-select option:selected').data('porcentaje'));
        var subtotal = price * quantity;
        var taxAmount = subtotal * (taxPercentage / 100);
        var total = subtotal + taxAmount;

        $row.find('.subtotal').text(subtotal.toFixed(2));
        $row.find('.total').text(total.toFixed(2));

        CalcularTotales();
    });

    // Evento para eliminar una fila
    $(document).on('click', '.remove-row', function () {
        $(this).closest('tr').remove();
        CalcularTotales();
    });

    function CargarImpuestos(callback) {
        $.ajax({
            url: '/Venta/ObtenerImpuestos',
            type: 'GET',
            success: function (response) {
                if (response && response.length > 0) {
                    var options = response.map(function (impuesto) {
                        return `<option value="${impuesto.IdImpuesto}" data-porcentaje="${impuesto.Porcentaje}">${impuesto.Descripcion} ${impuesto.Porcentaje}%</option>`;
                    }).join('');

                    var selectImpuestoHtml = `<select class="form-control form-control-sm impuesto-select">${options}</select>`;
                    if (callback) callback(selectImpuestoHtml);
                } else {
                    MostrarAlertaError("Ocurrió un error al consultar impuestos.");
                }
            },
            error: function () {
                MostrarAlertaError("Ocurrió un error.");
            }
        });
    }

    function CalcularTotales() {

        let subtotal = 0;
        let total = 0;

        // Recorremos cada fila de la tabla
        $('#facturaTable tbody tr').each(function () {
            // Obtenemos los valores de las celdas
            let cantidad = parseFloat($(this).find('input.quantity').val()) || 0;
            let precioUnitario = parseFloat($(this).find('td:eq(3)').text()) || 0;
            let impuestoPorcentaje = $(this).find('select option:selected').data('porcentaje') || 0;

            // Calculamos el subtotal y el total
            let filaSubtotal = cantidad * precioUnitario;
            let filaImpuesto = filaSubtotal * (impuestoPorcentaje / 100);
            let filaTotal = filaSubtotal + filaImpuesto;

            // Sumamos al subtotal y total generales
            subtotal += filaSubtotal;
            total += filaTotal;

            // Actualizamos los valores en las celdas correspondientes
            $(this).find('.subtotal').text(filaSubtotal.toFixed(2));
            $(this).find('.total').text(filaTotal.toFixed(2));
        });

        // Actualizamos los valores del totalizador
        $('#subtotal').text(subtotal.toFixed(2));
        $('#total').text(total.toFixed(2));
    }

    $(btnGuardar).on('click', function (e) {
        e.preventDefault();

        //ENCABEZADO
        var v_usuario = $(codigoUsuarioTxt).val();
        var v_formaPago = $(formaPago).val();
        var v_subTotal = $(subTotal).text();
        var v_total = $(total).text();
        var v_numeroFactura = $(numeroFactura).val();

        let encabezado = [];
        let lineas = [];

        encabezado = {   
            NumeroFactura : v_numeroFactura,
            Fecha: new Date(), 
            Subtotal: v_subTotal,
            Total: v_total,
            NumeroIdentificacion: v_usuario,          
            IdFormaPago: v_formaPago
        };

        lineas = ObtenerLineas();

        var venta = {
            Encabezado: encabezado,
            Lineas: lineas
        };

        $.ajax({
            type: "POST",
            url: "/Venta/InsertarFactura",
            data: JSON.stringify(venta),
            contentType: "application/json",
            success: function (response)
            {
                if (response.Resultado) {
                    MostrarAlertaExitosa(response.Mensaje);    
                } else {
                    MostrarAlertaError(response.Mensaje);
                }
            },
            error: function (xhr, status, error) {
                MostrarAlertaError("Ocurrió un error.");
            }
        });

    });

    function LineaEntidad(numeroFactura, codigoCancion, cantidad, precio, impuesto, subtotal, total) {
        this.NumeroFactura = numeroFactura;
        this.CodigoCancion = codigoCancion,
        this.Cantidad = cantidad;
        this.Precio = precio;
        this.IdImpuesto = impuesto;
        this.Subtotal = subtotal;
        this.Total = total;
    }

    function ObtenerLineas() {
        var lineas = [];

        $('#facturaTable tbody tr').each(function () {          
            var codigoCancion = $(this).find('td').eq(0).text();
            var cantidad = $(this).find('.quantity').val();
            var precio = $(this).find('td').eq(3).text();
            var impuesto = $(this).find('td').eq(4).find('select').val(); 
            var subtotal = $(this).find('td').eq(5).text();
            var total = $(this).find('td').eq(6).text();
     

            // Crear una nueva instancia de la entidad y agregarla a la lista
            var linea = new LineaEntidad(numeroFactura, codigoCancion, cantidad, precio, impuesto, subtotal, total);
            lineas.push(linea);
        });

        return lineas;
    }

});

