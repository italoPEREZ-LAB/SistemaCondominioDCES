using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaCondominioDCES.Models
{
    public class Propietario
    {
   public int IdPropietario { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string DNI { get; set; }

    public string Telefono { get; set; }

    public string Correo { get; set; }

    public bool Estado { get; set; }
        public string Torre { get; set; }

        public string NumeroDepartamento { get; set; }
    


}
}