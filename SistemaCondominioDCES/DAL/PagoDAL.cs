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
                    SELECT p.IdPago,
                           pr.Nombre,
                           pr.Apellido,
                           p.MontoPagado,
                           p.FechaPago,
                           p.MetodoPago
                    FROM Pagos p
                    INNER JOIN Recibos r ON p.IdRecibo = r.IdRecibo
                    INNER JOIN Departamentos d ON r.IdDepartamento = d.IdDepartamento
                    INNER JOIN Propietarios pr ON pr.NumeroDepartamento = d.Numero";

                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Pago()
                    {
                        IdPago = (int)dr["IdPago"],
                        NombrePropietario = dr["Nombre"].ToString() + " " + dr["Apellido"].ToString(),
                        MontoPagado = (decimal)dr["MontoPagado"],
                        FechaPago = (DateTime)dr["FechaPago"],
                        MetodoPago = dr["MetodoPago"].ToString()
                    });
                }
            }
            return lista;
        }

        public bool Registrar(Pago pago)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string validar = "SELECT Monto FROM Recibos WHERE IdRecibo=@IdRecibo";
                SqlCommand cmdValidar = new SqlCommand(validar, conexion);
                cmdValidar.Parameters.AddWithValue("@IdRecibo", pago.IdRecibo);

                conexion.Open();
                decimal montoRecibo = (decimal)cmdValidar.ExecuteScalar();
                conexion.Close();

                if (pago.MontoPagado > montoRecibo)
                {
                    return false;
                }

                string query = @"INSERT INTO Pagos (IdRecibo, MontoPagado, FechaPago, MetodoPago)
                                 VALUES (@IdRecibo, @MontoPagado, @FechaPago, @MetodoPago)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdRecibo", pago.IdRecibo);
                cmd.Parameters.AddWithValue("@MontoPagado", pago.MontoPagado);
                cmd.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                cmd.Parameters.AddWithValue("@MetodoPago", pago.MetodoPago);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }
    }
}
