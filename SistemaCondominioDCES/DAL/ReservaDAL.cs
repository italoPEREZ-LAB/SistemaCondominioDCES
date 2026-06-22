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

        public bool ExisteReservaDuplicada(int idAreaComun, DateTime fechaReserva, TimeSpan horaInicio, TimeSpan horaFin)
        {
            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"SELECT COUNT(*) 
                                 FROM Reservas 
                                 WHERE IdAreaComun = @IdAreaComun
                                 AND CAST(FechaReserva AS DATE) = CAST(@FechaReserva AS DATE)
                                 AND (HoraInicio < @HoraFin AND HoraFin > @HoraInicio)
                                 AND Estado <> 'Cancelado'
                                 AND Estado <> 'CANCELADA'
                                 AND Estado <> 'CANCELADA'";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdAreaComun", idAreaComun);
                cmd.Parameters.AddWithValue("@FechaReserva", fechaReserva.Date);
                cmd.Parameters.AddWithValue("@HoraInicio", horaInicio);
                cmd.Parameters.AddWithValue("@HoraFin", horaFin);

                conexion.Open();

                int cantidad = (int)cmd.ExecuteScalar();

                return cantidad > 0;
            }
        }

        public bool Registrar(Reserva reserva)
        {
            if (ExisteReservaDuplicada(
                reserva.IdAreaComun,
                reserva.FechaReserva,
                reserva.HoraInicio,
                reserva.HoraFin))
            {
                return false;
            }

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"INSERT INTO Reservas 
                                (IdUsuario, IdAreaComun, FechaReserva, HoraInicio, HoraFin, Estado)
                                VALUES 
                                (@IdUsuario, @IdAreaComun, @FechaReserva, @HoraInicio, @HoraFin, @Estado)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", reserva.IdUsuario);
                cmd.Parameters.AddWithValue("@IdAreaComun", reserva.IdAreaComun);
                cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva.Date);
                cmd.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", reserva.HoraFin);
                cmd.Parameters.AddWithValue("@Estado", reserva.Estado ?? "Pendiente");

                conexion.Open();
                cmd.ExecuteNonQuery();
            }

            return true;
        }
    }
}