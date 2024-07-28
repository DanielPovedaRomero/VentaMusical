
namespace VentaMusical.Models.ViewModels.UsuarioModels
{
    public class EditUsuario
    {
        public int NumeroIdentificacion { get; set; }
        public string Nombre { get; set; }
        public int IdGenero { get; set; }
        public string Correo { get; set; }
        public int IdTarjeta { get; set; }
        public string NumeroTarjeta { get; set; }
        public string Contrasena { get; set; }
        public int IdPerfil { get; set; }
    }
}