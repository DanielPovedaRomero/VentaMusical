$(document).ready(function () {

    /*[GUARDAR]*/
    $('#guardar').on('click', function (e) {
        e.preventDefault();

        var descripcion = $("#Descripcion").val();
        var imagenFile = $("#Imagen")[0].files[0];
        var codigo = $("#CodigoGenero").val();

        if (!descripcion || !imagenFile) {
            alert("Por favor, complete la descripción y seleccione una imagen.");
            return;
        }

        var reader = new FileReader();
        reader.onload = function (e) {
            var imagenBase64 = e.target.result.split(',')[1];
            var data = {
                Descripcion: descripcion,
                Imagen: imagenBase64,
                Codigo: codigo
            };

            // Mostrar la imagen seleccionada en el preview
            $('#ImagenPreview').attr('src', e.target.result);

            $.ajax({
                type: "POST",
                url: "/GenerosMusicales/Guardar",
                data: JSON.stringify(data),
                contentType: "application/json",
                success: function (response) {
                    console.log(response);
                    // Manejar la respuesta del servidor aquí
                    // Por ejemplo, cerrar el modal después de guardar
                    $('#exampleModal').modal('hide');
                    location.reload(); // O recargar la página
                },
                error: function (xhr, status, error) {
                    console.error("Error en la solicitud", error);
                    alert("Ocurrió un error al intentar guardar el género.");
                }
            });
        };

        // Leer el archivo como URL base64
        reader.readAsDataURL(imagenFile);
    });


    /*[ELIMINAR]*/
    $('.delete').on('click', function (e) {
        var codigoGenero = $(this).data('id');

        if (confirm("¿Estás seguro de que deseas eliminar este género?")) {
            $.ajax({
                url: '/GenerosMusicales/Eliminar',
                type: 'POST',
                data: { codigoGenero: codigoGenero },
                success: function (response) {
                    if (response.Resultado) {
                        location.reload(); // Recarga la página después de eliminar
                    } else {
                        alert(response.Mensaje);
                    }
                },
                error: function () {
                    alert("Ocurrió un error al intentar eliminar el género.");
                }
            });
        }
    });


    /*[CONSULTA PARA EDITAR]*/
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
                        $('#ImagenPreview').attr('src', 'data:image;base64,' + response.Imagen).show();
                    } else {
                        $('#ImagenPreview').attr('src', '@Url.Content("~/Content/Images/DefaultAlbum.png")').show();
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
            $('#exampleModalLabel').text('Agregar Nuevo Género');
        });

    });


    /*[CAMBIAR IMAGEN]*/
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
            $('#ImagenPreview').attr('src', '@Url.Content("~/Content/Images/DefaultAlbum.png")');
        }
    });

}); //document ready