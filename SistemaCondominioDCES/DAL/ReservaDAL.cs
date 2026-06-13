using SistemaCondominioDCES.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SistemaCondominioDCES.DAL
{
    public class ReservaDAL
    {
        Conexion cn = new Conexion();

        public List<Reserva> Listar()
        {
            var lista = new List<Reserva>();
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"SELECT r.IdReserva, u.Nombre, ac.NombreArea AS AreaComun,
                                        r.FechaReserva, r.HoraInicio, r.HoraFin, r.Estado
                                 FROM Reservas r
                                 INNER JOIN Usuarios u ON r.IdUsuario = u.IdUsuario
                                 INNER JOIN AreasComunes ac ON r.IdAreaComun = ac.IdAreaComun";

                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Reserva()
                    {
                        IdReserva = (int)dr["IdReserva"],
                        NombreUsuario = dr["Nombre"].ToString(),
                        NombreAreaComun = dr["AreaComun"].ToString(),
                        FechaReserva = (DateTime)dr["FechaReserva"],
                        HoraInicio = (TimeSpan)dr["HoraInicio"],
                        HoraFin = (TimeSpan)dr["HoraFin"],
                        Estado = dr["Estado"].ToString()
                    });
                }
            }
            return lista;
        }

        public bool Registrar(Reserva reserva)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string validar = @"SELECT COUNT(*) 
                                   FROM Reservas 
                                   WHERE IdAreaComun=@IdAreaComun 
                                     AND FechaReserva=@FechaReserva 
                                     AND (HoraInicio < @HoraFin AND HoraFin > @HoraInicio)";
                SqlCommand cmdValidar = new SqlCommand(validar, conexion);
                cmdValidar.Parameters.AddWithValue("@IdAreaComun", reserva.IdAreaComun);
                cmdValidar.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);
                cmdValidar.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio);
                cmdValidar.Parameters.AddWithValue("@HoraFin", reserva.HoraFin);

                conexion.Open();
                int existe = (int)cmdValidar.ExecuteScalar();
                conexion.Close();

                if (existe > 0)
                {
                    return false; 
                }

                string query = @"INSERT INTO Reservas (IdUsuario, IdAreaComun, FechaReserva, HoraInicio, HoraFin, Estado)
                                 VALUES (@IdUsuario, @IdAreaComun, @FechaReserva, @HoraInicio, @HoraFin, @Estado)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", reserva.IdUsuario);
                cmd.Parameters.AddWithValue("@IdAreaComun", reserva.IdAreaComun);
                cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);
                cmd.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", reserva.HoraFin);
                cmd.Parameters.AddWithValue("@Estado", reserva.Estado ?? "Confirmado");

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }
    }
}
