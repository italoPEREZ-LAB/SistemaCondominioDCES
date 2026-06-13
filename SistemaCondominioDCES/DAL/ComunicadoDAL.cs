using SistemaCondominioDCES.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class ComunicadoDAL
    {
        Conexion cn = new Conexion();

        public List<Comunicado> Listar()
        {
            List<Comunicado> lista = new List<Comunicado>();
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "SELECT IdComunicado, Titulo, Contenido, FechaPublicacion, Estado FROM Comunicados ORDER BY FechaPublicacion DESC";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        lista.Add(new Comunicado
                        {
                            IdComunicado = (int)dr["IdComunicado"],
                            Titulo = dr["Titulo"].ToString(),
                            Contenido = dr["Contenido"].ToString(),
                            FechaPublicacion = (System.DateTime)dr["FechaPublicacion"],
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public void Registrar(Comunicado comunicado)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "INSERT INTO Comunicados (Titulo, Contenido, FechaPublicacion, Estado) VALUES (@Titulo, @Contenido, GETDATE(), 'Activo')";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Titulo", comunicado.Titulo);
                    cmd.Parameters.AddWithValue("@Contenido", comunicado.Contenido);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "DELETE FROM Comunicados WHERE IdComunicado = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}