
    function MostrarAlertaExitosa(mensaje = "Proceso realizado con éxito") {
        Swal.fire({
            icon: 'success',
            html: mensaje,
            showConfirmButton: true,
            timer: 10000,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#28a745',
            confirmButtonTextColor: '#ffffff'
        }).then(() => {
            location.reload();
        });
    }


    function MostrarAlertaError(mensaje = "Proceso realizado con error") {
        Swal.fire({
            icon: 'error',
            html: mensaje,
            showConfirmButton: true,
            timer: 10000,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#dc3545',
            confirmButtonTextColor: '#ffffff'
        });
    }

    function MostrarAlertaPregunta(funcionAEjecutar, ...params) {
        Swal.fire({
            icon: 'question',
            title: '¿Confirmar?',
            text: '¿Está seguro de ejecutar esta acción?',
            showCancelButton: true,
            confirmButtonColor: '#28a745',
            cancelButtonColor: '#dc3545',
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                funcionAEjecutar(params);
            }
        });
    }

    function MostrarAlertaAdvertencia(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: mensaje,
            showConfirmButton: true,
            timer: 10000,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#F8BB86',
            confirmButtonTextColor: '#ffffff'
        });
    }

    function CargarImagenEnPreviewYFile(imagenBase64, elemento, elementoImagen) {
        // Mostrar la imagen en elemento
        $(elemento).attr('src', 'data:image;base64,' + imagenBase64).show();

        // Crear un Blob a partir de la imagen base64
        var byteCharacters = atob(imagenBase64);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: 'image/png' });

        // Crear un File desde el Blob para asignarlo a imagenFile
        var fileName = 'imagen.png'; // Nombre de archivo
        var imagenFile = new File([blob], fileName, { type: 'image/png' });

        // Asignar imagenFile al elemento de entrada de archivo #Imagen
        AsignarImagenAInputFile(imagenFile, elementoImagen);
    }

    // Función para asignar un archivo al elemento de entrada de archivo #Imagen
    function AsignarImagenAInputFile(imagenFile, elementoImagen) {
        // Crear un objeto FileList simulado
        var fileList = new DataTransfer();
        fileList.items.add(imagenFile);

        // Asignar fileList al elemento de entrada de archivo #Imagen
        $(elementoImagen)[0].files = fileList.files;
    }

    // Función para limpiar la selección de archivo #Imagen
    function LimpiarSeleccionImagen(elementoImagen) {
        $(elementoImagen).val(''); // Esto limpia la selección de archivo en el input
    }