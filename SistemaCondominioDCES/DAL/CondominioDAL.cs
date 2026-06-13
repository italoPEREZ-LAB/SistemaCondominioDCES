using SistemaCondominioDCES.Models;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class CondominioDAL
    {
        Conexion cn = new Conexion();

        public Condominio Obtener()
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "SELECT TOP 1 IdCondominio, NombreCondominio, Direccion, Estado FROM Condominios";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        return new Condominio
                        {
                            IdCondominio = (int)dr["IdCondominio"],
                            NombreCondominio = dr["NombreCondominio"].ToString(),
                            Direccion = dr["Direccion"].ToString(),
                            Estado = (bool)dr["Estado"]
                        };
                    }
                }
            }
            return null;
        }

        public void Actualizar(Condominio condominio)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "UPDATE Condominios SET NombreCondominio = @Nombre, Direccion = @Direccion WHERE IdCondominio = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Nombre", condominio.NombreCondominio);
                    cmd.Parameters.AddWithValue("@Direccion", condominio.Direccion);
                    cmd.Parameters.AddWithValue("@Id", condominio.IdCondominio);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}