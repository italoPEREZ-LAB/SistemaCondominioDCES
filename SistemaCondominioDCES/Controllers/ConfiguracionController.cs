using SistemaCondominioDCES.DAL;
using SistemaCondominioDCES.Models;
using System;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class ConfiguracionController : Controller
    {
        private CondominioDAL condominioDAL = new CondominioDAL();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(Condominio condominio)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Home");

            try
            {
                if (string.IsNullOrEmpty(condominio.NombreCondominio) || string.IsNullOrEmpty(condominio.Direccion))
                {
                    ViewBag.Error = "Por favor, complete todos los campos.";
                    return View(condominio);
                }

                condominioDAL.Actualizar(condominio);
                ViewBag.Success = "Los datos del condominio se actualizaron con éxito.";
                return View(condominio);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al actualizar los datos: " + ex.Message;
                return View(condominio);
            }
        }
    }
}