namespace VentaMusical.Models.ViewModels
{
    public class VentaLineaViewModel
    {
        public int NumeroFactura { get; set; }
        public int Linea { get; set; }
        public int CodigoCancion { get; set; }
        public decimal Precio { get; set; }
        public int IdImpuesto { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

    }
}