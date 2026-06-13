namespace SistemaCondominioDCES.Models
{
    public class Incidencia
    {
        public int IdIncidencia { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaReporte { get; set; }
        public string Estado { get; set; }
    }
}