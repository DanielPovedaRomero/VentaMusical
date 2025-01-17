﻿let frmNombre = "#Nombre";
let frmImagen = "#Imagen";
let frmPrecio = "#Precio";
let frmGenero = "#Genero";
let frmCodigoCancion = "#CodigoCancion";
let frmImagenPreview = "#ImagenPreview";
let frmModal = "#exampleModal";
let frmTextoModal = "#exampleModalLabel";
let frmFormulario = "#myForm";

let btnGuardar = "#guardar";
let btnEditar = ".edit";
let btnEliminar = ".delete";

let rutaImagenDefault = "/Content/Images/DefaultAlbum.png";
let textoAgregar = "Agregar Nuevo Registro";
let textoEditar = "Editar Registro";

$(document).ready(function () {

    //[GUARDAR]
    $(btnGuardar).on('click', function (e) {
        e.preventDefault();

        var nombre = $(frmNombre).val();
        var imagenFile = $(frmImagen)[0].files[0];
        var codigoCancion = $(frmCodigoCancion).val();
        var codigoGenero = $(frmGenero).val();
        var precio = $(frmPrecio).val();

        if (!ValidarCampos(codigoGenero, nombre, precio)) {
            MostrarAlertaAdvertencia("Por favor, complete los campos de nombre, precio y seleccione un género musical.");
            return;
        }

        var imagenBase64;

        // Verificar si se seleccionó un archivo de imagen
        if (imagenFile) {
            var reader = new FileReader();
            reader.onload = function (e) {
                imagenBase64 = e.target.result.split(',')[1];
                $(frmImagenPreview).attr('src', e.target.result);
                GuardarCancion(nombre, imagenBase64, codigoCancion, codigoGenero, precio);
            };

            reader.readAsDataURL(imagenFile);
        } else {
            GuardarCancion(nombre, null, codigoCancion, codigoGenero, precio);
        }
    });

    function GuardarCancion(nombre, imagenBase64, codigoCancion, codigoGenero, precio) {
       
        var data = {
            Nombre: nombre,
            Imagen: imagenBase64,
            CodigoGenero: codigoGenero,
            CodigoCancion: codigoCancion,
            Precio : precio
        };

        $.ajax({
            type: "POST",
            url: "/Canciones/Guardar",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (response) {

                if (response.Resultado) {
                    MostrarAlertaExitosa(response.Mensaje);
                    $(frmModal).modal('hide');
                } else {
                    MostrarAlertaError(response.Mensaje);
                }
            },
            error: function (xhr, status, error) {
                MostrarAlertaError("Ocurrió un error.");
            }
        });
    }

    //[ELIMINAR]
    $(btnEliminar).on('click', function (e) {
        var codigoCancion = $(this).data('id');
        MostrarAlertaPregunta(() => {
            $.ajax({
                url: '/Canciones/Eliminar',
                type: 'POST',
                data: { codigoCancion: codigoCancion },
                success: function (response) {
                    if (response.Resultado) {
                        MostrarAlertaExitosa(response.Mensaje);
                    } else {
                        MostrarAlertaError(response.Mensaje);
                    }
                },
                error: function () {
                    MostrarAlertaError("Ocurrió un error.");
                }
            });
        });
    });

    //[CONSULTAR]
    $(btnEditar).on('click', function () {
        var codigoCancion = $(this).data('id');

        $.ajax({
            url: '/Canciones/ObtenerCancion',
            type: 'GET',
            data: { codigoCancion: codigoCancion },
            success: function (response) {
                if (response) {
                    $(frmCodigoCancion).val(response.CodigoCancion);
                    $(frmNombre).val(response.Nombre);
                    $(frmPrecio).val(response.Precio);
                    $(frmGenero).val(response.CodigoGenero);
                    if (response.Imagen) {
                        CargarImagenEnPreviewYFile(response.Imagen, frmImagenPreview, frmImagen);
                    } else {
                        $(frmImagenPreview).attr('src', rutaImagenDefault).show();
                        LimpiarSeleccionImagen(frmImagen);
                    }
                    $(frmTextoModal).text(textoEditar);
                    $(frmModal).modal('show');

                } else {
                    MostrarAlertaError("Ocurrió un error.");
                }
            },
            error: function () {
                MostrarAlertaError("Ocurrió un error.");
            }
        });

        $(frmModal).on('hidden.bs.modal', function () {
            $(frmFormulario)[0].reset();
            $(frmCodigoCancion).val(0);
            $(frmGenero).val(0);
            $(frmNombre).val("");
            $(frmPrecio).val("");
            $(frmImagenPreview).attr('src', '').hide();
            LimpiarSeleccionImagen(frmImagen);
            $(frmTextoModal).text(textoAgregar);
        });
    });

    //[CAMBIAR IMAGEN]
    $(frmImagen).on('change', function (e) {
        var file = e.target.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (event) {
                $(frmImagenPreview).attr('src', event.target.result);
            };
            reader.readAsDataURL(file);
        } else {
            // Si no se selecciona ningún archivo, mostrar la imagen por defecto
            $(frmImagenPreview).attr('src', rutaImagenDefault);
        }
    });

    //[FORMATO PRECIO]
    $(frmPrecio).on('blur', function () {
 
        var valor = $(this).val();

        if (valor == 'NaN' || valor == '' ) { $(this).val('0.00'); return; }

        // Convertir a número y formatear a 2 decimales
        var valorFormateado = parseFloat(valor).toFixed(2);

        $(this).val(valorFormateado);
    });

    //[FORMATO DECIMALES]
    $(frmPrecio).on('keyup', function () {
        
        var valor = $(this).val();

        // Remover caracteres no permitidos (dejar solo números y un punto decimal)
        valor = valor.replace(/[^0-9.]/g, '');

        // Si hay más de un punto decimal, eliminar los extras
        if (valor.split('.').length > 2) {
            valor = valor.replace(/\.+$/, '');
        }

        // Asignar el valor limpio de vuelta al campo de precio
        $(this).val(valor);
    });

    //[VALIDAR CAMPOS]
    function ValidarCampos(CodigoGenero, Nombre, Precio) {
        return CodigoGenero > 0 && Nombre && Precio > 0;
    }
    
});



