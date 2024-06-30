let frmDescripcion = "#Descripcion";
let frmImagen = "#Imagen";
let frmCodigoGenero = "#CodigoGenero";
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

        var descripcion = $(frmDescripcion).val();
        var imagenFile = $(frmImagen)[0].files[0];
        var codigo = $(frmCodigoGenero).val();

        if (!descripcion) {
            MostrarAlertaAdvertencia("Por favor, ingrese una descripción");
            return;
        }

        var imagenBase64;

        // Verificar si se seleccionó un archivo de imagen
        if (imagenFile) {
            var reader = new FileReader();
            reader.onload = function (e) {
                imagenBase64 = e.target.result.split(',')[1];
                $(frmImagenPreview).attr('src', e.target.result);
                GuardarGenero(descripcion, imagenBase64, codigo);
            };

            reader.readAsDataURL(imagenFile);
        } else {
            GuardarGenero(descripcion, null, codigo);
        }
    });

    function GuardarGenero(descripcion, imagenBase64, codigo) {

        var data = {
            Descripcion: descripcion,
            Imagen: imagenBase64,
            Codigo: codigo
        };

        $.ajax({
            type: "POST",
            url: "/GenerosMusicales/Guardar",
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
        var codigoGenero = $(this).data('id');
        MostrarAlertaPregunta(() => {
            $.ajax({
                url: '/GenerosMusicales/Eliminar',
                type: 'POST',
                data: { codigoGenero: codigoGenero },
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
        var codigoGenero = $(this).data('id');

        $.ajax({
            url: '/GenerosMusicales/ObtenerGenero',
            type: 'GET',
            data: { codigoGenero: codigoGenero },
            success: function (response) {
                console.log(response);
                if (response)
                {                   
                   $(frmCodigoGenero).val(response.CodigoGenero);
                   $(frmDescripcion).val(response.Descripcion);
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
            $(frmCodigoGenero).val(0);
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