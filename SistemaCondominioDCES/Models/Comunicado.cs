namespace SistemaCondominioDCES.Models
{
    public class Comunicado
    {
        public int IdComunicado { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public System.DateTime FechaPublicacion { get; set; }
        public string Estado { get; set; }
    }
}