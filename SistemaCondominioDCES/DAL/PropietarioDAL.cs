using SistemaCondominioDCES.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class PropietarioDAL
    {
        Conexion cn = new Conexion();

        public List<Propietario> Listar()
        {
            List<Propietario> lista = new List<Propietario>();

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = "SELECT * FROM Propietarios";

                SqlCommand cmd = new SqlCommand(query, conexion);

                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Propietario()
                    {
                        IdPropietario = (int)dr["IdPropietario"],
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString(),
                        DNI = dr["DNI"].ToString(),
                        Telefono = dr["Telefono"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Estado = (bool)dr["Estado"],
                        Torre = dr["Torre"].ToString(),
                        NumeroDepartamento = dr["NumeroDepartamento"].ToString(),
                    });
                }
            }

            return lista;
        }


        public void Registrar(Propietario propietario)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"INSERT INTO Propietarios
                        (Nombre, Apellido, DNI, Telefono, Correo, Torre, NumeroDepartamento, Estado)
                        VALUES
                        (@Nombre, @Apellido, @DNI, @Telefono, @Correo, @Torre, @NumeroDepartamento, 1)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                cmd.Parameters.AddWithValue("@DNI", propietario.DNI);
                cmd.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                cmd.Parameters.AddWithValue("@Correo", propietario.Correo);
                cmd.Parameters.AddWithValue("@Torre", propietario.Torre);
                cmd.Parameters.AddWithValue("@NumeroDepartamento", propietario.NumeroDepartamento);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Propietario BuscarPorId(int id)
        {
            Propietario propietario = null;

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = "SELECT * FROM Propietarios WHERE IdPropietario = @IdPropietario";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdPropietario", id);

                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    propietario = new Propietario()
                    {
                        IdPropietario = (int)dr["IdPropietario"],
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString(),
                        DNI = dr["DNI"].ToString(),
                        Telefono = dr["Telefono"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Estado = (bool)dr["Estado"],
                        Torre = dr["Torre"].ToString(),
                        NumeroDepartamento = dr["NumeroDepartamento"].ToString()
                    };
                }
            }

            return propietario;
        }

        public void Actualizar(Propietario propietario)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"UPDATE Propietarios SET
                        Nombre = @Nombre,
                        Apellido = @Apellido,
                        DNI = @DNI,
                        Telefono = @Telefono,
                        Correo = @Correo,
                        Torre = @Torre,
                        NumeroDepartamento = @NumeroDepartamento
                        WHERE IdPropietario = @IdPropietario";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdPropietario", propietario.IdPropietario);
                cmd.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                cmd.Parameters.AddWithValue("@DNI", propietario.DNI);
                cmd.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                cmd.Parameters.AddWithValue("@Correo", propietario.Correo);
                cmd.Parameters.AddWithValue("@Torre", propietario.Torre);
                cmd.Parameters.AddWithValue("@NumeroDepartamento", propietario.NumeroDepartamento);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"UPDATE Propietarios
                         SET Estado = 0
                         WHERE IdPropietario = @IdPropietario";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdPropietario", id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
    