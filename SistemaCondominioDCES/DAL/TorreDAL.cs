using SistemaCondominioDCES.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class TorreDAL
    {
        Conexion cn = new Conexion();

        public List<Torre> Listar()
        {
            List<Torre> lista = new List<Torre>();

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = "SELECT IdTorre, NombreTorre FROM Torres";

                SqlCommand cmd = new SqlCommand(query, conexion);

                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Torre()
                    {
                        IdTorre = (int)dr["IdTorre"],
                        NombreTorre = dr["NombreTorre"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}