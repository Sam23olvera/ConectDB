using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace ConectDB.Controllers
{
    public class AgenciasController : Controller
    {
        private string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
        DataApi data = new DataApi();
        UsuarioModel model = new UsuarioModel();
        ConectMenuUser menu = new ConectMenuUser();

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Index(int cveEmp, string XT)
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                return RedirectToAction("Index", "Loging");

            string desusuario = UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]);
            string descontraseña = UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]);

            model = menu.RegresMenu(desusuario, descontraseña, cveEmp, url, XT);
            model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
            model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
            ViewData["UsuarioModel"] = model;
            return View(model);
        }
        public ActionResult Agencias() => View();
    }
}