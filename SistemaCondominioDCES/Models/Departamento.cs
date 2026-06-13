using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaCondominioDCES.Models
{
    public class Departamento
    {
        public int IdDepartamento { get; set; }
        public int IdTorre { get; set; }
        public string NombreTorre { get; set; }
        public string Numero { get; set; }
        public int Piso { get; set; }
        public bool Estado { get; set; }
    }
}