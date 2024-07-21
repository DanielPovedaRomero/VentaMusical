using System;

namespace VentaMusical.Models.ViewModels
{
    public class VentaEncabezadoViewModel
    {
        public int NumeroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string NumeroIdentificacion { get; set; }

        public string NombreUsuario { get; set; }
    }
}