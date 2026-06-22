using SistemaCondominioDCES.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class PagoDAL
    {
        Conexion cn = new Conexion();

        public List<Pago> Listar()
        {
            var lista = new List<Pago>();

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"
            SELECT 
                p.IdPago,
                ISNULL(pr.Nombre, 'Sin propietario') AS Nombre,
                ISNULL(pr.Apellido, '') AS Apellido,
                p.MontoPagado,
                p.FechaPago,
                p.MetodoPago,
                ISNULL(p.Estado, 'Pendiente') AS Estado
            FROM Pagos p
            INNER JOIN Recibos r ON p.IdRecibo = r.IdRecibo
           LEFT JOIN Propietarios pr ON r.IdPropietario = pr.IdPropietario
            ORDER BY p.IdPago ASC";

                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Pago()
                    {
                        IdPago = Convert.ToInt32(dr["IdPago"]),
                        NombrePropietario = dr["Nombre"].ToString() + " " + dr["Apellido"].ToString(),
                        MontoPagado = Convert.ToDecimal(dr["MontoPagado"]),
                        FechaPago = Convert.ToDateTime(dr["FechaPago"]),
                        MetodoPago = dr["MetodoPago"].ToString(),
                        Estado = dr["Estado"].ToString()
                    });
                }
            }

            return lista;
        }

        public bool Registrar(Pago pago)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string validar = "SELECT Monto FROM Recibos WHERE IdRecibo = @IdRecibo";

                SqlCommand cmdValidar = new SqlCommand(validar, conexion);
                cmdValidar.Parameters.AddWithValue("@IdRecibo", pago.IdRecibo);

                conexion.Open();

                object resultado = cmdValidar.ExecuteScalar();

                if (resultado == null)
                {
                    return false;
                }

                decimal montoRecibo = Convert.ToDecimal(resultado);

                if (pago.MontoPagado > montoRecibo)
                {
                    return false;
                }

                string query = @"INSERT INTO Pagos 
                                (IdRecibo, MontoPagado, FechaPago, MetodoPago, Estado)
                                VALUES 
                                (@IdRecibo, @MontoPagado, @FechaPago, @MetodoPago, @Estado)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdRecibo", pago.IdRecibo);
                cmd.Parameters.AddWithValue("@MontoPagado", pago.MontoPagado);
                cmd.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                cmd.Parameters.AddWithValue("@MetodoPago", pago.MetodoPago);
                cmd.Parameters.AddWithValue("@Estado", "Pendiente");

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago pago = null;

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"
                    SELECT 
                        IdPago,
                        IdRecibo,
                        MontoPagado,
                        FechaPago,
                        MetodoPago,
                        ISNULL(Estado, 'Pendiente') AS Estado
                    FROM Pagos
                    WHERE IdPago = @IdPago";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdPago", id);

                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    pago = new Pago()
                    {
                        IdPago = Convert.ToInt32(dr["IdPago"]),
                        IdRecibo = Convert.ToInt32(dr["IdRecibo"]),
                        MontoPagado = Convert.ToDecimal(dr["MontoPagado"]),
                        FechaPago = Convert.ToDateTime(dr["FechaPago"]),
                        MetodoPago = dr["MetodoPago"].ToString(),
                        Estado = dr["Estado"].ToString()
                    };
                }
            }

            return pago;
        }

        public void Actualizar(Pago pago)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"
                    UPDATE Pagos
                    SET 
                        MontoPagado = @MontoPagado,
                        FechaPago = @FechaPago,
                        MetodoPago = @MetodoPago,
                        Estado = @Estado
                    WHERE IdPago = @IdPago";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@MontoPagado", pago.MontoPagado);
                cmd.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                cmd.Parameters.AddWithValue("@MetodoPago", pago.MetodoPago);
                cmd.Parameters.AddWithValue("@Estado", pago.Estado ?? "Pendiente");
                cmd.Parameters.AddWithValue("@IdPago", pago.IdPago);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = "DELETE FROM Pagos WHERE IdPago = @IdPago";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdPago", id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}