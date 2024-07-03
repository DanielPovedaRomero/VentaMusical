using System.Collections.Generic;

namespace VentaMusical.Models.ViewModels
{
    public class VistaCancionesViewModel
    {
        public List<CancionesViewModel> Canciones { get; set; }
        public List<GenerosMusicalesViewModel> GenerosMusicales { get; set; }
    }
}