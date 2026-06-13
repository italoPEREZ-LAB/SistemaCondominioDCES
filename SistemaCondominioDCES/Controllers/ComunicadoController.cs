using SistemaCondominioDCES.DAL;
using SistemaCondominioDCES.Models;
using System;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class ComunicadoController : Controller
    {
        private ComunicadoDAL comunicadoDAL = new ComunicadoDAL();

        private ActionResult ValidarAdmin()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("AccesoDenegado", "Home");

            return null;
        }

        public ActionResult Index()
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            return View();
        }

        public ActionResult Create()
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            return View();
        }

        [HttpPost]
        public ActionResult Create(Comunicado comunicado)
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            try
            {
                if (string.IsNullOrEmpty(comunicado.Titulo) || string.IsNullOrEmpty(comunicado.Contenido))
                {
                    ViewBag.Error = "Por favor, complete todos los campos.";
                    return View(comunicado);
                }

                comunicadoDAL.Registrar(comunicado);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al registrar el comunicado: " + ex.Message;
                return View(comunicado);
            }
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            comunicadoDAL.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}