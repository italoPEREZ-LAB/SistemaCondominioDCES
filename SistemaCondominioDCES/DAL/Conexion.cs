using System.Configuration;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class Conexion
    {
        private readonly string cadena =
            ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadena);
        }
    }
}