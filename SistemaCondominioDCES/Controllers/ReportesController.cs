using SistemaCondominioDCES.DAL;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class ReportesController : Controller
    {
        private Conexion cn = new Conexion();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("AccesoDenegado", "Home");

            int totalPropietarios = 0;
            int totalDepartamentos = 0;
            int totalIncidenciasAbiertas = 0;
            int totalReservasActivas = 0;
            decimal totalPorCobrar = 0;
            decimal totalRecaudado = 0;

            try
            {
                using (SqlConnection conexion = cn.ObtenerConexion())
                {
                    conexion.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Propietarios WHERE Estado = 1", conexion))
                        totalPropietarios = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Departamentos WHERE Estado = 1", conexion))
                        totalDepartamentos = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Incidencias WHERE Estado IN ('Abierto', 'En Proceso')", conexion))
                        totalIncidenciasAbiertas = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Reservas WHERE Estado = 'Confirmado'", conexion))
                        totalReservasActivas = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Monto), 0) FROM Recibos WHERE Estado = 'Pendiente'", conexion))
                        totalPorCobrar = Convert.ToDecimal(cmd.ExecuteScalar());

                    using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(MontoPagado), 0) FROM Pagos WHERE Estado = 'Aprobado'", conexion))
                        totalRecaudado = Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar reportes: " + ex.Message;
            }

            ViewBag.TotalPropietarios = totalPropietarios;
            ViewBag.TotalDepartamentos = totalDepartamentos;
            ViewBag.TotalIncidenciasAbiertas = totalIncidenciasAbiertas;
            ViewBag.TotalReservasActivas = totalReservasActivas;
            ViewBag.TotalPorCobrar = totalPorCobrar;
            ViewBag.TotalRecaudado = totalRecaudado;

            return View();
        }
    }
}