using SistemaCondominioDCES.DAL;
using SistemaCondominioDCES.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class ReservaController : Controller
    {
        ReservaDAL reservaDAL = new ReservaDAL();
        Conexion cn = new Conexion();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            var lista = reservaDAL.Listar();
            return View(lista);
        }

        public ActionResult Create()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            CargarCombos();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Reserva reserva)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            bool ok = reservaDAL.Registrar(reserva);

            if (!ok)
            {
                ViewBag.Error = "El sistema validó la disponibilidad y no permitió registrar una reserva duplicada.";
                CargarCombos();
                return View(reserva);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            Reserva reserva = null;

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"SELECT IdReserva, IdUsuario, IdAreaComun, FechaReserva, HoraInicio, HoraFin, Estado 
                                 FROM Reservas 
                                 WHERE IdReserva = @Id";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                conexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reserva = new Reserva()
                    {
                        IdReserva = (int)dr["IdReserva"],
                        IdUsuario = (int)dr["IdUsuario"],
                        IdAreaComun = (int)dr["IdAreaComun"],
                        FechaReserva = (DateTime)dr["FechaReserva"],
                        HoraInicio = (TimeSpan)dr["HoraInicio"],
                        HoraFin = (TimeSpan)dr["HoraFin"],
                        Estado = dr["Estado"].ToString()
                    };
                }
            }

            CargarCombos();
            return View(reserva);
        }

        [HttpPost]
        public ActionResult Edit(Reserva reserva)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"UPDATE Reservas SET 
                                 IdUsuario = @IdUsuario, 
                                 IdAreaComun = @IdAreaComun,
                                 FechaReserva = @FechaReserva, 
                                 HoraInicio = @HoraInicio,
                                 HoraFin = @HoraFin, 
                                 Estado = @Estado
                                 WHERE IdReserva = @IdReserva";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", reserva.IdUsuario);
                cmd.Parameters.AddWithValue("@IdAreaComun", reserva.IdAreaComun);
                cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva.Date);
                cmd.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", reserva.HoraFin);
                cmd.Parameters.AddWithValue("@Estado", reserva.Estado ?? "Pendiente");
                cmd.Parameters.AddWithValue("@IdReserva", reserva.IdReserva);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = "UPDATE Reservas SET Estado = 'Cancelado' WHERE IdReserva = @Id";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        private void CargarCombos()
        {
            var usuarios = new List<SelectListItem>();

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string queryUsuarios = "SELECT IdUsuario, Nombre FROM Usuarios";

                SqlCommand cmd = new SqlCommand(queryUsuarios, conexion);
                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    usuarios.Add(new SelectListItem
                    {
                        Value = dr["IdUsuario"].ToString(),
                        Text = dr["Nombre"].ToString()
                    });
                }
            }

            ViewBag.Usuarios = usuarios;

            var areas = new List<SelectListItem>();

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string queryAreas = "SELECT IdAreaComun, NombreArea FROM AreasComunes";

                SqlCommand cmd = new SqlCommand(queryAreas, conexion);
                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    areas.Add(new SelectListItem
                    {
                        Value = dr["IdAreaComun"].ToString(),
                        Text = dr["NombreArea"].ToString()
                    });
                }
            }

            ViewBag.Areas = areas;
        }
    }
}