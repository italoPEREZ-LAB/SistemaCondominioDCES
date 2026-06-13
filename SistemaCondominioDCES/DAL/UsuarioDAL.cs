using SistemaCondominioDCES.Models;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class UsuarioDAL
    {
        Conexion conexion = new Conexion();

        public Usuario ValidarLogin(string correo, string password)
        {
            Usuario usuario = null;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"
                SELECT TOP 1 
                    u.IdUsuario,
                    u.Nombre,
                    u.Apellido,
                    u.Correo,
                    r.NombreRol
                FROM Usuarios u
                INNER JOIN Roles r ON u.IdRol = r.IdRol
                WHERE u.Correo = @Correo
                AND u.PasswordHash = @Password
                AND u.Estado = 1";

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Password", password);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    usuario = new Usuario()
                    {
                        IdUsuario = (int)dr["IdUsuario"],
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        NombreRol = dr["NombreRol"].ToString()
                    };
                }
            }

            return usuario;
        }
    }
}