namespace VentaMusical.Constantes
{
    public static class Mensajes
    {
        public static string Error = "Error en la solicitud";
        public static string Exito = "Registro exitoso";
        public static string Eliminar_Exito = "Registro eliminado";
        public static string NoEncontrado = "Registro no encontrado";
        public static string ErrorEliminarGenero = "Existen registros de canciones asociados a este género. Por favor, elimine esas canciones antes de eliminar este registro.";
        public static string GeneroConMismaDescripcion = "Ya existe un género musical con esta descripción.";
        public static string CancionConMismaDescripcion = "Ya existe una canción con ese nombre asociada a este género musical.";
        public static string ErrorEliminarUsuario = "No existen de usuarios asociados a este número de Identificación. Por favor verificar si el número de Identificación es el correcto";
        public static string UsuariosConMismaCedula = "Ya existe un usuario registrado con ese número de cédula en el sistema";
        public static string CorreoNoValido = "La dirección de correo electronico no cumple con las politicas establecidas";

    }
}