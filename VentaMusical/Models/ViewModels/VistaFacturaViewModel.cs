using System.Collections.Generic;

namespace VentaMusical.Models.ViewModels
{
    public class VistaFacturaViewModel
    {
        public List<UsuarioViewModel> Usuarios { get; set; }
        public VentaEncabezadoViewModel Encabezado { get; set; }
        public List<CancionesViewModel> Canciones { get; set; }
        public List<FormasDePagoViewModel> FormasDePago { get; set; }
        public List<VentaLineaViewModel> Lineas { get; set; }
    }
}