using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ConectDB.Models.LogUser;

namespace ConectDB.Controllers
{
    public class LogingController : Controller
    {
        DataApi data = new DataApi();
        LogUser jsdatos = new LogUser();
        public IActionResult Privacy() 
        {
            return View("Privacy");
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            ModelState.Clear();
            return View("Index");
        }
        public IActionResult logeo(Filter log)
        {
            string usuarioCifrado = UrlEncryptor.EncryptUrl(log.usr);
            string contraseñaCifrada = UrlEncryptor.EncryptUrl(log.pwd);

            string url = "https://webportal.tum.com.mx/wsstmdv/api/logau";
            if (log.usr == null)
            {
                return View("Error");
            }
            else if (log.pwd == null)
            {
                return View("Error");
            }
            else
            {
                JObject jsdatos = JObject.Parse("{\"data\": {\"bdCc\": 1,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_User\"},\"filter\": {\"usr\": \"" + log.usr + "\",\"pwd\": \"" + log.pwd + "\" }}");
                var datos = JsonConvert.DeserializeObject<UsuarioModel>(data.HttpWebRequest("POST", url, jsdatos));
                if (datos == null)
                {
                    return View("Error");
                }
                else
                {
                    datos.Data[0].usuario = usuarioCifrado;
                    datos.Data[0].contraseña = contraseñaCifrada;
                    return View(datos);
                }
            }
        }
        public IActionResult Acceder(int CVEM, string US, string XT, string Tok)
        {
            string desusuario = UrlEncryptor.DecryptUrl(US);
            string descontraseña = UrlEncryptor.DecryptUrl(XT);


            string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";

            jsdatos = new LogUser { data = new Data { bdCc = 1, bdSch = "dbo", bdSp = "SPQRY_EmpUser" }, filter = new Filter { usr = desusuario, pwd = descontraseña, idempresa = CVEM } };
            var datos = data.HttpWebRequestTokenLog("POST", url, JsonConvert.SerializeObject(jsdatos), Tok);
            if (datos == null)
            {
                return RedirectToAction("Error");
            }
            else
            {
                var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                model.Token = Tok;
                model.Data[0].usuario = US;
                model.Data[0].contraseña = XT;
                ViewData["UsuarioModel"] = model;
                return View("Acceder", model);
            }
        }
        
        
        public  IActionResult Salir(int cveEmp, string UfS, string xPa, string XT) 
        {
            ModelState.Clear();
            return RedirectToAction("Index");
        }

    }
}