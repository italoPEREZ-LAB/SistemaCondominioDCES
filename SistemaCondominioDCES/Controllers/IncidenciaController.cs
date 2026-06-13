using SistemaCondominioDCES.DAL;
using SistemaCondominioDCES.Models;
using System;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class IncidenciaController : Controller
    {
        private IncidenciaDAL incidenciaDAL = new IncidenciaDAL();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            var lista = incidenciaDAL.Listar();
            return View(lista);
        }

        public ActionResult Create()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Incidencia incidencia)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            try
            {
                incidencia.IdUsuario = Convert.ToInt32(Session["IdUsuario"]);

                if (string.IsNullOrEmpty(incidencia.Titulo) || string.IsNullOrEmpty(incidencia.Descripcion))
                {
                    ViewBag.Error = "Por favor, complete todos los campos.";
                    return View(incidencia);
                }

                incidenciaDAL.Registrar(incidencia);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al registrar la incidencia: " + ex.Message;
                return View(incidencia);
            }
        }

        [HttpPost]
        public ActionResult ActualizarEstado(int id, string estado)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            try
            {
                incidenciaDAL.ActualizarEstado(id, estado);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}