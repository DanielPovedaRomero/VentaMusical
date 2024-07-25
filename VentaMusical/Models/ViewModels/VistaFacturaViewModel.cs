using System.Collections.Generic;

namespace VentaMusical.Models.ViewModels
{
    public class VistaFacturaViewModel
    {
        public List<UsuarioViewModel> Usuarios { get; set; }
        public VentaEncabezadoViewModel VentaEncabezado { get; set; }
        public List<CancionesViewModel> Canciones { get; set; }
        public List<FormasDePagoViewModel> FormasDePago { get; set; }
    }
}