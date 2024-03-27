using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConectDB.Controllers
{
    public class TiposdeOperaciónController : Controller
    {
        DataApi data = new DataApi();
        
        
        public ActionResult Index(int cveEmp, string UfS, string xPa, string XT)
        {
            string desusuario = UrlEncryptor.DecryptUrl(UfS);
            string descontraseña = UrlEncryptor.DecryptUrl(xPa);

            string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
            JObject jsdatos = JObject.Parse("{\"data\": {\"bdCc\": 1,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_EmpUser\"},\"filter\": {\"usr\": \"" + desusuario + "\",\"pwd\": \"" + descontraseña + "\",\"idempresa\":" + cveEmp + "} }");
            var datos = data.HttpWebRequestToken("POST", url, jsdatos, XT);
            if (datos == null)
            {
                return RedirectToAction("Error");
            }
            else
            {
                var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                model.Data[0].usuario = UfS;
                model.Data[0].contraseña = xPa;
                ViewData["UsuarioModel"] = model;
                return View(model);
            }
        }
    }
}