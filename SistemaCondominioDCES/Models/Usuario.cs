namespace SistemaCondominioDCES.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public int IdCondominio { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string PasswordHash { get; set; }

        public bool Estado { get; set; }

        public string NombreRol { get; set; }
    }
}