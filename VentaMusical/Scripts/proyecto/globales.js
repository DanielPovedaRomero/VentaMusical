
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

