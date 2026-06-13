using SistemaCondominioDCES.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class IncidenciaDAL
    {
        Conexion cn = new Conexion();

        public List<Incidencia> Listar()
        {
            List<Incidencia> lista = new List<Incidencia>();
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "SELECT IdIncidencia, IdUsuario, Titulo, Descripcion, FechaReporte, Estado FROM Incidencias ORDER BY FechaReporte DESC";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        lista.Add(new Incidencia
                        {
                            IdIncidencia = (int)dr["IdIncidencia"],
                            IdUsuario = (int)dr["IdUsuario"],
                            Titulo = dr["Titulo"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            FechaReporte = (System.DateTime)dr["FechaReporte"],
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public void Registrar(Incidencia incidencia)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "INSERT INTO Incidencias (IdUsuario, Titulo, Descripcion, FechaReporte, Estado) VALUES (@IdUsuario, @Titulo, @Descripcion, GETDATE(), 'Abierto')";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", incidencia.IdUsuario);
                    cmd.Parameters.AddWithValue("@Titulo", incidencia.Titulo);
                    cmd.Parameters.AddWithValue("@Descripcion", incidencia.Descripcion);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEstado(int id, string estado)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();
                string query = "UPDATE Incidencias SET Estado = @Estado WHERE IdIncidencia = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}