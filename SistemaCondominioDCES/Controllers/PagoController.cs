using SistemaCondominioDCES.DAL;
using SistemaCondominioDCES.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class PagoController : Controller
    {
        PagoDAL pagoDAL = new PagoDAL();
        Conexion cn = new Conexion();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            var lista = pagoDAL.Listar();
            return View(lista);
        }

        public ActionResult Create()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("AccesoDenegado", "Home");

            CargarRecibos();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Pago pago)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("AccesoDenegado", "Home");

            bool ok = pagoDAL.Registrar(pago);

            if (!ok)
            {
                ViewBag.Error = "El monto excede el recibo.";
                CargarRecibos();
                return View(pago);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("AccesoDenegado", "Home");

            Pago pago = pagoDAL.ObtenerPorId(id);

            if (pago == null)
                return RedirectToAction("Index");

            return View(pago);
        }

        [HttpPost]
        
        public ActionResult Edit(Pago pago)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("AccesoDenegado", "Home");

            pagoDAL.Actualizar(pago);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("AccesoDenegado", "Home");

            pagoDAL.Eliminar(id);

            return RedirectToAction("Index");
        }

        private void CargarRecibos()
        {
            var listaRecibos = new List<SelectListItem>();

            using (SqlConnection conexion = cn.ObtenerConexion())
            {
                string query = @"SELECT r.IdRecibo,
                                p.Nombre + ' ' + p.Apellido AS Propietario,
                                r.Concepto, 
                                r.Monto
                         FROM Recibos r
                        INNER JOIN Propietarios p ON r.IdPropietario = p.IdPropietario
                         WHERE r.Estado = 'Pendiente'";

                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaRecibos.Add(new SelectListItem
                    {
                        Value = dr["IdRecibo"].ToString(),
                        Text = $"{dr["Propietario"]} - {dr["Concepto"]} - S/.{dr["Monto"]}"
                    });
                }
            }

            ViewBag.Recibos = listaRecibos;
        }
    }
}