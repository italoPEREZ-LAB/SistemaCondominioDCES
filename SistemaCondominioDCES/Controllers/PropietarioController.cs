using SistemaCondominioDCES.DAL;
using SistemaCondominioDCES.Models;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class PropietarioController : Controller
    {
        PropietarioDAL propietarioDAL = new PropietarioDAL();

        public ActionResult Dashboard()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Propietario")
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            return View();
        }

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

            var lista = propietarioDAL.Listar();
            return View(lista);
        }

        public ActionResult Create()
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
        public ActionResult Create(Propietario propietario)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            propietarioDAL.Registrar(propietario);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            var propietario = propietarioDAL.BuscarPorId(id);
            return View(propietario);
        }

        [HttpPost]
        public ActionResult Edit(Propietario propietario)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            propietarioDAL.Actualizar(propietario);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            propietarioDAL.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}