using SistemaCondominioDCES.DAL;
using SistemaCondominioDCES.Models;
using System.Web.Mvc;

namespace SistemaCondominioDCES.Controllers
{
    public class DepartamentoController : Controller
    {
        DepartamentoDAL departamentoDAL = new DepartamentoDAL();
        TorreDAL torreDAL = new TorreDAL();

        private ActionResult ValidarAdmin()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            return null;
        }

        public ActionResult Index()
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            var lista = departamentoDAL.Listar();
            return View(lista);
        }

        public ActionResult Create()
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            ViewBag.Torres = torreDAL.Listar();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Departamento departamento)
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            departamentoDAL.Registrar(departamento);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            ViewBag.Torres = torreDAL.Listar();
            var departamento = departamentoDAL.BuscarPorId(id);
            return View(departamento);
        }

        [HttpPost]
        public ActionResult Edit(Departamento departamento)
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            departamentoDAL.Actualizar(departamento);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var validar = ValidarAdmin();
            if (validar != null) return validar;

            departamentoDAL.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}