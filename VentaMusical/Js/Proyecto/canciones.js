let frmNombre = "#Nombre";
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

        console.log(nombre);
        console.log(codigoGenero);
        console.log(precio);

        if (!nombre || !codigoGenero || !precio ) {
            MostrarAlertaAdvertencia("Por favor, complete los campos de nombre, precio y seleccione un genero musical");
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
    
});



