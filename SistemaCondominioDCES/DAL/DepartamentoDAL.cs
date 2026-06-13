using SistemaCondominioDCES.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class DepartamentoDAL
    {
        Conexion cn = new Conexion();

        public List<Departamento> Listar()
        {
            List<Departamento> lista = new List<Departamento>();

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"SELECT d.IdDepartamento, d.IdTorre, t.NombreTorre,
                                        d.Numero, d.Piso, d.Estado
                                 FROM Departamentos d
                                 INNER JOIN Torres t ON d.IdTorre = t.IdTorre
                                 ORDER BY t.NombreTorre, d.Numero";

                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Departamento()
                    {
                        IdDepartamento = (int)dr["IdDepartamento"],
                        IdTorre = (int)dr["IdTorre"],
                        NombreTorre = dr["NombreTorre"].ToString(),
                        Numero = dr["Numero"].ToString(),
                        Piso = (int)dr["Piso"],
                        Estado = (bool)dr["Estado"]
                    });
                }
            }

            return lista;
        }

        public void Registrar(Departamento departamento)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"INSERT INTO Departamentos
                                (IdTorre, Numero, Piso, Estado)
                                VALUES
                                (@IdTorre, @Numero, @Piso, 1)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdTorre", departamento.IdTorre);
                cmd.Parameters.AddWithValue("@Numero", departamento.Numero);
                cmd.Parameters.AddWithValue("@Piso", departamento.Piso);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Departamento BuscarPorId(int id)
        {
            Departamento departamento = null;

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"SELECT d.IdDepartamento, d.IdTorre, t.NombreTorre,
                                        d.Numero, d.Piso, d.Estado
                                 FROM Departamentos d
                                 INNER JOIN Torres t ON d.IdTorre = t.IdTorre
                                 WHERE d.IdDepartamento = @IdDepartamento";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdDepartamento", id);

                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    departamento = new Departamento()
                    {
                        IdDepartamento = (int)dr["IdDepartamento"],
                        IdTorre = (int)dr["IdTorre"],
                        NombreTorre = dr["NombreTorre"].ToString(),
                        Numero = dr["Numero"].ToString(),
                        Piso = (int)dr["Piso"],
                        Estado = (bool)dr["Estado"]
                    };
                }
            }

            return departamento;
        }

        public void Actualizar(Departamento departamento)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"UPDATE Departamentos
                                 SET IdTorre = @IdTorre,
                                     Numero = @Numero,
                                     Piso = @Piso
                                 WHERE IdDepartamento = @IdDepartamento";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdDepartamento", departamento.IdDepartamento);
                cmd.Parameters.AddWithValue("@IdTorre", departamento.IdTorre);
                cmd.Parameters.AddWithValue("@Numero", departamento.Numero);
                cmd.Parameters.AddWithValue("@Piso", departamento.Piso);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"UPDATE Departamentos
                                 SET Estado = 0
                                 WHERE IdDepartamento = @IdDepartamento";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdDepartamento", id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}