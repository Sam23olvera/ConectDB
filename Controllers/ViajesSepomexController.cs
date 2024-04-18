using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConectDB.Controllers
{
    public class ViajesSepomexController : Controller
    {
        private string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
        DataApi data = new DataApi();
        ConectApi con = new ConectApi();


        public ActionResult Index(int cveEmp, string XT)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]))
                {
                    return RedirectToAction("Index", "Loging");
                }
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                {
                    return RedirectToAction("Index", "Loging");
                }
                string desusuario = UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]);
                string descontraseña = UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]);

                JObject jsdatos = JObject.Parse("{\"data\": {\"bdCc\": 1,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_EmpUser\"},\"filter\": {\"usr\": \"" + desusuario + "\",\"pwd\": \"" + descontraseña + "\",\"idempresa\":" + cveEmp + "} }");
                var datos = data.HttpWebRequestToken("POST", url, jsdatos, XT);
                if (datos == null)
                {
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    ViewData["UsuarioModel"] = model;;
                    var oLista = con.ListarRutas();
                    //var oLista = con.listarReclu(usuario, contraseña, cveEmp);
                    return View("Index", oLista);
                }
            } 
            catch (Exception e)
            {
                //poner pagina de ERROR
                return View("Index");
            }
        }


    }
}
