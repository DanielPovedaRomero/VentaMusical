
$(document).ready(function () {

    $('table').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        }

    });

});

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

    // Función para cargar una imagen en un elemento de imagen y en un input file
    function CargarImagenEnPreviewYFile(imagenData, elementoImagen, elementoInputFile) {
        if (esBase64(imagenData)) {
            // Mostrar la imagen en el elemento de imagen
            $(elementoImagen).attr('src', 'data:image/png;base64,' + imagenData).show();

            // Crear un Blob a partir de la imagen base64
            var byteCharacters = atob(imagenData);
            var byteNumbers = new Array(byteCharacters.length);
            for (var i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            var blob = new Blob([byteArray], { type: 'image/png' });

            // Crear un File desde el Blob para asignarlo al input file
            var fileName = 'imagen.png'; // Nombre de archivo
            var imagenFile = new File([blob], fileName, { type: 'image/png' });

            // Asignar imagenFile al elemento de entrada de archivo
            AsignarImagenAInputFile(imagenFile, elementoInputFile);
        } else if (typeof imagenData === 'string') {
            // Mostrar la imagen desde la URL relativa
            $(elementoImagen).attr('src', imagenData).show();
            LimpiarSeleccionImagen(elementoInputFile); // Limpiar la selección de archivo
        } else {
            console.error('Tipo de datos de imagen no válido.');
        }
    }

    // Función para verificar si el dato es una cadena base64 válida
    function esBase64(s) {
        try {
            return btoa(atob(s)) == s;
        } catch (e) {
            return false;
        }
    }

    // Función para asignar un archivo al elemento de entrada de archivo
    function AsignarImagenAInputFile(imagenFile, elementoInputFile) {
        // Crear un objeto FileList simulado
        var fileList = new DataTransfer();
        fileList.items.add(imagenFile);

        // Asignar fileList al elemento de entrada de archivo
        $(elementoInputFile)[0].files = fileList.files;
    }

    // Función para limpiar la selección de archivo
    function LimpiarSeleccionImagen(elementoInputFile) {
        $(elementoInputFile).val(''); // Esto limpia la selección de archivo en el input
    }
