using System;

namespace SistemaCondominioDCES.Models
{
    public class Reserva
    {
        public int IdReserva { get; set; }
        public int IdUsuario { get; set; }
        public int IdAreaComun { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreAreaComun { get; set; }
        public DateTime FechaReserva { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Estado { get; set; }
    }
}
