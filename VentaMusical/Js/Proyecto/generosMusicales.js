$(document).ready(function () {

    //[GUARDAR]
    $('#guardar').on('click', function (e) {
        e.preventDefault();

        var descripcion = $("#Descripcion").val();
        var imagenFile = $("#Imagen")[0].files[0];
        var codigo = $("#CodigoGenero").val();

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
                $('#ImagenPreview').attr('src', e.target.result);
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
                    $('#exampleModal').modal('hide');
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
    $('.delete').on('click', function (e) {
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

    //[CONSULTA PARA EDITAR]
    $('.edit').on('click', function () {
        var codigoGenero = $(this).data('id');

        $.ajax({
            url: '/GenerosMusicales/ObtenerGenero',
            type: 'GET',
            data: { codigoGenero: codigoGenero },
            success: function (response) {
                if (response) {
                    $('#CodigoGenero').val(response.CodigoGenero);
                    $('#Descripcion').val(response.Descripcion);

                    if (response.Imagen) {
                        CargarImagenEnPreviewYFile(response.Imagen, '#ImagenPreview', '#Imagen');
                    } else {
                        $('#ImagenPreview').attr('src', '/Content/Images/DefaultAlbum.png').show();
                        LimpiarSeleccionImagen('#Imagen'); 
                    }

                    $('#exampleModalLabel').text('Editar Registro');
                    $('#exampleModal').modal('show');

                } else {
                    alert("Ocurrió un error al intentar obtener el género.");
                }
            },
            error: function () {
                alert("Ocurrió un error al intentar obtener el género.");
            }
        });

        $('#exampleModal').on('hidden.bs.modal', function () {
            $('#myForm')[0].reset();
            $('#CodigoGenero').val(0);
            $('#ImagenPreview').attr('src', '').hide();
            LimpiarSeleccionImagen('#Imagen');
            $('#exampleModalLabel').text('Agregar Nuevo Género');
        });
    });

    //[CAMBIAR IMAGEN]
    $('#Imagen').on('change', function (e) {
        var file = e.target.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (event) {
                $('#ImagenPreview').attr('src', event.target.result);
            };
            reader.readAsDataURL(file);
        } else {
            // Si no se selecciona ningún archivo, mostrar la imagen por defecto
            $('#ImagenPreview').attr('src', '/Content/Images/DefaultAlbum.png');
        }
    });

}); 