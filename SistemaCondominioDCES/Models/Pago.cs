using System;

namespace SistemaCondominioDCES.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdRecibo { get; set; }
        public string NombrePropietario { get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; }
        public string Estado { get; set; }
    }

}
