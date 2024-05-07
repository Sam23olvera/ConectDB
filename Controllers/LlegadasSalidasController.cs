using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace ConectDB.Controllers
{
    public class LlegadasSalidasController : Controller
    {
        private string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
        ConectApi con = new ConectApi();
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
            model.Token = XT;
            ViewData["UsuarioModel"] = model;
            ViewData["Token"] = XT;
            ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
            ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
            ViewData["cveEmp"] = cveEmp;
            var oLista = con.ListarRutas();
            return View("Index", oLista);
        }

        [HttpPost]
        public IActionResult Buscar(DateTime fecha, int cvruta, string Token, string cveEmp)
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
            string datos = "";
            if (fecha == DateTime.MinValue)
            {
                datos = data.HttpWebRequestToken("POST", url, jsdatos, Token);
                if (datos == null)
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Token;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Token;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return View("Error", jso);
                }
            }
            else if (cvruta == 0)
            {
                datos = data.HttpWebRequestToken("POST", url, jsdatos, Token);
                if (datos == null)
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Token;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Token;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return View("Error", jso);
                }
            }
            else
            {
                datos = data.HttpWebRequestToken("POST", url, jsdatos, Token);
                if (datos == null)
                {
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Token;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    ViewData["cvruta"] = cvruta;
                    ViewData["fecha"] = fecha;
                    var tab = con.ListasJuntas(fecha, cvruta);
                    //var tab = con.listaViajes(fecha, cvruta);
                    //if (tab.Count == 0)
                    //{
                    //    var oLista = con.ListarRutas();
                    //    return View("Index", oLista);
                    //}
                    return View("Mod_busca", tab);
                }
            }
        }
        public IActionResult bus(string FeSel, int cvruta, string Tox, string cveEmp)
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
            string datos = "";
            if (Convert.ToDateTime(FeSel) == DateTime.MinValue)
            {
                datos = data.HttpWebRequestToken("POST", url, jsdatos, Tox);
                if (datos == null)
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Tox;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Tox;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return View("Error", jso);
                }
            }
            else if (cvruta == 0)
            {
                datos = data.HttpWebRequestToken("POST", url, jsdatos, Tox);
                if (datos == null)
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Tox;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Tox;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    var jso = con.ListarRutas();
                    if (jso.Count == 0)
                    {
                        var oLista = con.ListarRutas();
                        return View("Index", oLista);
                    }
                    return View("Error", jso);
                }
            }
            else
            {
                datos = data.HttpWebRequestToken("POST", url, jsdatos, Tox);
                if (datos == null)
                {
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Tox;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    ViewData["cvruta"] = cvruta;
                    ViewData["fecha"] = Convert.ToDateTime(FeSel);
                    var tab = con.ListasJuntas(Convert.ToDateTime(FeSel), cvruta);
                    //var tab = con.listaViajes(fecha, cvruta);
                    //if (tab.Count == 0)
                    //{
                    //    var oLista = con.ListarRutas();
                    //    return View("Index", oLista);
                    //}
                    return View("Mod_busca", tab);
                }
            }
        }
        public IActionResult LlegadasSalidas()
        {
            return View();
        }
        public IActionResult Desplegar(string FV, int CV, string NR, string FeFolVi, string FeSel, int cvruta, string Tox, string cveEmp, string UN, string ET, string OP)
        {
            string idus = "";
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

            //string desusuario = UrlEncryptor.DecryptUrl(UF);
            //string descontraseña = UrlEncryptor.DecryptUrl(xPaS);

            string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
            JObject jsdatos = JObject.Parse("{\"data\": {\"bdCc\": 1,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_EmpUser\"},\"filter\": {\"usr\": \"" + desusuario + "\",\"pwd\": \"" + descontraseña + "\",\"idempresa\":" + cveEmp + "} }");
            try
            {
                string datos = data.HttpWebRequestToken("POST", url, jsdatos, Tox);
                if (datos == null)
                {
                    return RedirectToAction("Error");
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                    idus = model.Data[0].idus.ToString();
                    model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                    model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                    ViewData["UsuarioModel"] = model;
                    ViewData["Token"] = Tox;
                    ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                    ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                    ViewData["cveEmp"] = cveEmp;
                    ViewData["CV"] = CV;
                    ViewData["NR"] = NR;
                    ViewData["FV"] = FV;
                    ViewData["UN"] = UN;
                    ViewData["ET"] = ET;
                    ViewData["OP"] = OP;
                    ViewData["cvruta"] = cvruta;
                    ViewData["FeFolVi"] = FeFolVi;
                    ViewData["iduse"] = idus;
                    //ModelState.AddModelError("Mensaje", "");
                    var tab = con.ListaViajeDetalle(CV, NR, Convert.ToDateTime(FeSel), cvruta);
                    //var tab = con.ListaViajeDetalle(CV, NR, Convert.ToDateTime(Date), cvruta);
                    return View("Desplegar", tab);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }
        public IActionResult Error()
        {
            // Aquí puedes proporcionar detalles de error según sea necesario.
            ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtalo de nuevo más tarde.";
            return View();
        }
        [HttpPost]
        public IActionResult Guardar(List<ItineViajeSPM> sPMs, string UN, string ET, string OP, string FV, string NR, int CV, string Tox, string FeFolVi, string FeSel, int cvruta, string cveEmp, string idus)
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]))
            {
                return RedirectToAction("Index", "Loging");
            }
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
            {
                return RedirectToAction("Index", "Loging");
            }


            foreach (var itinerario in sPMs)
            {
                foreach (var viaje in itinerario.ItinerarioViajesSPM)
                {
                    //DateTime.TryParse(viaje.FEG + "T" + viaje.HLG, out DateTime fechaLlegada);
                    //DateTime.TryParse(viaje.FSG + "T" + viaje.HSG, out DateTime fechaSalida);
                    DateTime fechaLlegada = viaje.FEG;
                    DateTime fechaSalida = viaje.FSG;

                    if ((viaje.CLL && fechaLlegada > DateTime.Now) || (viaje.CSA && fechaSalida > DateTime.Now))
                    {
                        var model = ObtenerUsuarioModel(HttpContext.Request.Cookies["usuario"], HttpContext.Request.Cookies["contra"], cveEmp, Tox);
                        model.Data[0].usuario = HttpContext.Request.Cookies["usuario"];
                        model.Data[0].contraseña = HttpContext.Request.Cookies["contra"];
                        ViewData["UsuarioModel"] = model;
                        ViewData["Token"] = Tox;
                        ViewData["Usuario"] = HttpContext.Request.Cookies["usuario"];
                        ViewData["Contraseña"] = HttpContext.Request.Cookies["contra"];
                        ViewData["cveEmp"] = cveEmp;
                        ViewData["CV"] = CV;
                        ViewData["NR"] = NR;
                        ViewData["FV"] = FV;
                        ViewData["UN"] = UN;
                        ViewData["ET"] = ET;
                        ViewData["OP"] = OP;
                        ViewData["cvruta"] = cvruta;
                        ViewData["FeFolVi"] = FeFolVi;
                        ViewData["iduse"] = idus;
                        sPMs[0].NR = NR;
                        //var tab = con.ListaViajeDetalle(CV, NR, Convert.ToDateTime(FeSel), cvruta);
                        if (fechaLlegada > DateTime.Now)
                        {
                            ViewBag.ErrorMessage = "No se pueden ingresar fechas futuras " + fechaLlegada;
                            return View("Desplegar", sPMs);
                        }
                        else if (fechaSalida > DateTime.Now)
                        {
                            ViewBag.ErrorMessage = "No se pueden ingresar fechas futuras." + fechaSalida;
                            return View("Desplegar", sPMs);
                        }
                    }
                }
            }
            string jsonEnvio = "{'ItinerarioViajesSPM':[";
            foreach (var itinerario in sPMs)
            {
                foreach (var viaje in itinerario.ItinerarioViajesSPM)
                {
                    string inci = "";
                    if (viaje.INCIDENCIA == "[Select]")
                    {
                        inci = "";
                    }
                    else
                    {
                        inci = viaje.INCIDENCIA;
                    }
                    jsonEnvio += "{'CVR':" + viaje.CVR + ","
                        //+ "'FLR':'" + viaje.FEG + "T" + viaje.HLG + "',"
                        + "'FLR':'" + viaje.FEG.ToString("yyyy-MM-dd HH:mm:ss") + "',"
                      + "'CLL':" + (viaje.CLL ? 1 : 0) + ","
                        //+ "'SAR':'" + viaje.FSG + "T" + viaje.HSG + "',"
                        + "'SAR':'" + viaje.FSG.ToString("yyyy-MM-dd HH:mm:ss") + "',"
                        + "'CSA':" + (viaje.CSA ? 1 : 0) + ","
                        + "'idus':'" + idus + "',"
                        + "'Incidencia': '" + inci + "'"
                      + "},";
                }
            }
            jsonEnvio += "]}";
            jsonEnvio = jsonEnvio.Replace(",]}", "]}").Replace("\\", "");
            if (jsonEnvio == null)
            {
                string xPaS = HttpContext.Request.Cookies["contra"];
                string uf = HttpContext.Request.Cookies["usuario"];
                ViewBag.ErrorMessage = "Error al guardar datos.";
                return RedirectToAction("Desplegar", new { message = "Error al guardar datos.", uf, xPaS, Tox, cveEmp, CV, NR, FV, UN, ET, OP, FeFolVi, FeSel, cvruta });

            }
            if (con.Guardar(jsonEnvio))
            {
                return LlenarViewDataAndReturn(sPMs, HttpContext.Request.Cookies["usuario"], HttpContext.Request.Cookies["contra"], Tox, cveEmp, CV, NR, FV, UN, ET, OP, FeFolVi, FeSel, cvruta);
            }
            else
            {
                return LlenarViewDataAndReturn(sPMs, HttpContext.Request.Cookies["usuario"], HttpContext.Request.Cookies["contra"], Tox, cveEmp, CV, NR, FV, UN, ET, OP, FeFolVi, FeSel, cvruta);
            }
        }

        private ViewResult LlenarViewDataAndReturn(List<ItineViajeSPM> sPMs, string usuario, string contraseña, string Token, string cveEmp, int CV, string NR, string FV, string UN, string ET, string OP, string FeFolVi, string FeSel, int cvruta)
        {
            var model = ObtenerUsuarioModel(usuario, contraseña, cveEmp, Token);
            model.Data[0].usuario = usuario;
            model.Data[0].contraseña = contraseña;
            ViewData["UsuarioModel"] = model;
            ViewData["Token"] = Token;
            ViewData["Usuario"] = usuario;
            ViewData["Contraseña"] = contraseña;
            ViewData["cveEmp"] = cveEmp;
            ViewData["CV"] = CV;
            ViewData["NR"] = NR;
            ViewData["FV"] = FV;
            ViewData["UN"] = UN;
            ViewData["ET"] = ET;
            ViewData["OP"] = OP;
            ViewData["cvruta"] = cvruta;
            ViewData["FeFolVi"] = FeFolVi;
            ViewData["iduse"] = model.Data[0].idus;
            ViewBag.Guardado = "¡Se Guardaron Correctamente los datos proporcionados!";
            Response.Headers.Add("Pragma", "no-cache");
            var tab = con.ListaViajeDetalle(CV, NR, Convert.ToDateTime(FeSel), cvruta);
            return View("Desplegar", tab);
        }

        private UsuarioModel ObtenerUsuarioModel(string usuario, string contraseña, string cveEmp, string Token)
        {
            string desusuario = UrlEncryptor.DecryptUrl(usuario);
            string descontraseña = UrlEncryptor.DecryptUrl(contraseña);
            string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
            JObject jsdatos = JObject.Parse("{\"data\": {\"bdCc\": 1,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_EmpUser\"},\"filter\": {\"usr\": \"" + desusuario + "\",\"pwd\": \"" + descontraseña + "\",\"idempresa\":" + cveEmp + "} }");
            string datos = data.HttpWebRequestToken("POST", url, jsdatos, Token);
            return JsonConvert.DeserializeObject<UsuarioModel>(datos);
        }

    }
}
