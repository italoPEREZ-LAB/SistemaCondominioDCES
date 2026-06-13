using SistemaCondominioDCES.DAL;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class HomeController : Controller
    {
        UsuarioDAL usuarioDAL = new UsuarioDAL();
        Conexion cn = new Conexion();

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string correo, string password)
        {
            var usuario = usuarioDAL.ValidarLogin(correo, password);
            if (usuario != null)
            {
                Session["Usuario"] = usuario.Nombre;
                Session["Rol"] = usuario.NombreRol;
                Session["IdUsuario"] = usuario.IdUsuario;
                Session["Correo"] = usuario.Correo;

                // Redirección según rol
                if (usuario.NombreRol == "Administrador")
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else if (usuario.NombreRol == "Propietario")
                {
                    return RedirectToAction("Dashboard", "Propietario");
                }

                return RedirectToAction("Login");
            }
            ViewBag.Error = "Correo o contraseña incorrectos";
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login");

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                conexion.Open();

                string qPropietarios = "SELECT COUNT(*) FROM Propietarios WHERE Estado = 1";
                ViewBag.TotalPropietarios = (int)new SqlCommand(qPropietarios, conexion).ExecuteScalar();

                string qPagosPendientes = @"
                    SELECT COUNT(*) FROM Recibos r
                    WHERE r.Monto > ISNULL(
                        (SELECT SUM(p.MontoPagado) FROM Pagos p WHERE p.IdRecibo = r.IdRecibo), 0)";
                ViewBag.PagosPendientes = (int)new SqlCommand(qPagosPendientes, conexion).ExecuteScalar();

                string qIncidencias = "SELECT COUNT(*) FROM Incidencias WHERE Estado = 'Abierto'";
                try
                {
                    ViewBag.TotalIncidencias = (int)new SqlCommand(qIncidencias, conexion).ExecuteScalar();
                }
                catch
                {
                    ViewBag.TotalIncidencias = 0; 
                }

                string qReservas = "SELECT COUNT(*) FROM Reservas WHERE FechaReserva = CAST(GETDATE() AS DATE)";
                ViewBag.ReservasHoy = (int)new SqlCommand(qReservas, conexion).ExecuteScalar();
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult AccesoDenegado()
        {
            return View();
        }
    }


}