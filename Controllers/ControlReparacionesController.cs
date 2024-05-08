using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConectDB.Controllers
{
    public class ControlReparacionesController : Controller
    {
        private readonly string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
        private readonly ConectMenuUser menu = new ConectMenuUser();
        private readonly DataApi data = new DataApi();
        private readonly ConectApiContrRep con = new ConectApiContrRep();
        private const int pageSize = 2;
        ControlFalla? controlFal = new ControlFalla();
        UsuarioModel? model = new UsuarioModel();
        Error msj = new Error();

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Index(int cveEmp, string XT, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), cveEmp, url, XT);
                model.Token = XT;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                controlFal = con.PrimerCarga_sin_catlog(0, model.Data[0]?.EmpS[0].cveEmp.ToString(), model.Data[0].idus.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), 0, idsub);
                ViewData["Title"] = "Inicio";
                return View("Index", controlFal);

            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PorAsig(int pagina, string Token, string cveEmp, int Buscar, int NumTicket, int ClaveTipoFalla, DateTime FehTick, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (Buscar == 0)
                {
                    if (FehTick == null || FehTick == DateTime.MinValue)
                    {
                        FehTick = DateTime.Now;
                    }
                    controlFal = con.PrimerCarga(1, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 0;
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                    }

                }
                else if (Convert.ToInt32(Buscar) == 1)
                {
                    controlFal = con.PrimerCarga(1, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (controlFal.status == 200)
                    {
                        ViewBag.totalpages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.currentpage = pagina;
                        TempData["Buscar"] = 1;
                        TempData["NumTicket"] = NumTicket;
                        TempData["ClaveTipoFalla"] = ClaveTipoFalla;
                        TempData["FehTick"] = FehTick;
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                if (controlFal.status == 400)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        msj.status = controlFal.status;
                        msj.message = controlFal.message;
                        return View("Error", msj);
                    }
                }
                TempData["FehTick"] = FehTick;
                ViewData["Title"] = "Por Asignar";
                return View("PorAsignar", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        [HttpPost]
        public IActionResult BuscarPorAsig(string Token, string cveEmp, int NumTicket, int ClaveTipoFalla, DateTime FehTick, int pagina, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (NumTicket == 0)
                {
                    controlFal = con.PrimerCarga(1, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, 0, ClaveTipoFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(1, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, 0, ClaveTipoFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                }
                if (controlFal.status == 200)
                {
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 1;
                    TempData["NumTicket"] = NumTicket;
                    TempData["ClaveTipoFalla"] = ClaveTipoFalla;
                    TempData["FehTick"] = FehTick;
                }
                else
                {
                    if (FehTick == null || FehTick == DateTime.MinValue)
                    {
                        FehTick = DateTime.Now;
                        TempData["FehTick"] = FehTick;
                    }
                    else
                    {
                        TempData["FehTick"] = FehTick;
                    }
                    TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                }
                if (controlFal.status != 200)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        msj.status = controlFal.status;
                        msj.message = controlFal.message;
                        return View("Error", msj);
                    }
                }
                ViewData["Title"] = "Por Asignar";
                return View("PorAsignar", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult AsignacionTicket(string Token, string cveEmp, string Asigna, string ticket, int pagina, int idsub, int Diesel, int Grua)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                DateTime FehAsignatiempo = DateTime.Now;
                if (Asigna != "[Selecciona]")
                {
                    controlFal = con.ModificadorFall(1, 2, model.Data[0].EmpS[0].cveEmp.ToString(), ticket, Asigna, 0, 0, "", "", model.Data[0].idus, FehAsignatiempo.ToString("yyyy-MM-dd"), 0, idsub, pagina, pageSize, Diesel, Grua, "", "", "", 0);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 0;
                        TempData["FehTick"] = FehAsignatiempo;
                        TempData["guardado"] = controlFal.status + "¡ \r\n" + controlFal.message + "\r\n!";
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                else
                {
                    controlFal = con.PrimerCarga(1, model.Data[0].EmpS[0].cveEmp.ToString(), FehAsignatiempo.ToString("yyyy-MM-dd"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 0;
                        TempData["FehTick"] = FehAsignatiempo;
                        TempData["guardado"] = controlFal.status + "¡ \r\n" + controlFal.message + "\r\n!";
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                if (controlFal.status != 200)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        msj.status = controlFal.status;
                        msj.message = controlFal.message;
                        return View("Error", msj);
                    }
                }
                ViewData["Title"] = "Por Asignar";
                return View("PorAsignar", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult Asignacion(int pagina, string Token, string cveEmp, string Buscar, int NumTicket, DateTime FehTick, int UsAsignado, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (Convert.ToInt32(Buscar) == 0)
                {
                    if (FehTick == null || FehTick == DateTime.MinValue)
                    {
                        FehTick = DateTime.Now;
                        TempData["FehTick"] = FehTick.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        TempData["FehTick"] = FehTick;
                    }
                    controlFal = con.PrimerCarga(2, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (controlFal.status != 200)
                    {
                        TempData["Mensaje"] = "¡" + controlFal.status + " " + controlFal.message + "!";
                    }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 0;
                }
                else if (Convert.ToInt32(Buscar) == 1)
                {
                    controlFal = con.PrimerCarga(2, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, 0, 0, model.Data[0].idus, UsAsignado, idsub, pagina, pageSize);
                    if (controlFal.status != 200)
                    {
                        TempData["Mensaje"] = "¡" + controlFal.status + " " + controlFal.message + "!";
                    }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 1;
                    TempData["UsAsignado"] = UsAsignado;
                    TempData["FehTick"] = FehTick;
                    TempData["NumTicket"] = NumTicket;
                    ViewData["Title"] = "Asignados";
                    return View("Asignados", controlFal);
                }
                if (controlFal.status != 200)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        msj.status = controlFal.status;
                        msj.message = controlFal.message;
                        return View("Error", msj);
                    }
                }
                ViewData["Title"] = "Asignados";
                return View("Asignados", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [HttpPost]
        public IActionResult BuscarAsignados(string Token, string cveEmp, int UsAsignado, int NumTicket, DateTime FehTick, int pagina, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (NumTicket == 0)
                {
                    controlFal = con.PrimerCarga(2, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, 0, 0, model.Data[0].idus, UsAsignado, idsub, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(2, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, 0, 0, model.Data[0].idus, UsAsignado, idsub, pagina, pageSize);
                }
                if (controlFal.status != 200)
                {
                    TempData["Mensaje"] = "¡" + controlFal.status + " " + controlFal.message + "!";
                }
                if (controlFal.status != 200)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        msj.status = controlFal.status;
                        msj.message = controlFal.message;
                        return View("Error", msj);
                    }
                }
                ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                ViewBag.CurrentPage = pagina;
                TempData["Buscar"] = 1;
                TempData["UsAsignado"] = UsAsignado;
                TempData["FehTick"] = FehTick;
                TempData["NumTicket"] = NumTicket;
                ViewData["Title"] = "Asignados";
                return View("Asignados", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [HttpPost]

        public IActionResult AsigTiempApoyClasif(string XT, string cveEmp, DateTime? TiempAsig, int Apooyo_Asigna, int Clasif_Asigna, int NumTicket, int pagina, int idsub, int CheckDisel, int CheckGrua, string Dot, string Marca, string Medida, int Posis)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, XT);
                model.Token = XT;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                controlFal = con.PrimerCarga(2, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                if (controlFal.status != 200)
                {
                    TempData["Mensaje"] = "¡No se encuentran datos! " + controlFal.status + " " + controlFal.message;
                }
                ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                ViewBag.CurrentPage = pagina;
                TempData["Buscar"] = 0;
                TempData["FehTick"] = DateTime.Now.ToString("yyyy-MM-dd");
                if (Apooyo_Asigna == 0)
                {
                    TempData["Mensaje"] = "¡Seleccione un Apoyo!";
                    return View("Asignados", controlFal);
                }
                if (!TiempAsig.HasValue || TiempAsig <= DateTime.Now)
                {
                    TempData["Mensaje"] = "¡La fecha de asignación no puede ser nulla o anterior.!";
                    return View("Asignados", controlFal);
                }
                else
                {
                    if (Clasif_Asigna != 2)
                    {
                        Dot = "";
                        Marca = "";
                        Medida = "";
                        Posis = 0;
                    }
                    if (Clasif_Asigna == 2)
                    {
                        if (string.IsNullOrEmpty(Dot))
                        {
                            TempData["Mensaje"] = "¡Debes de llenar el Dot!";
                            return View("Asignados", controlFal);
                        }
                        else if (string.IsNullOrEmpty(Marca))
                        {
                            TempData["Mensaje"] = "¡Debes de llenar la Marca!";
                            return View("Asignados", controlFal);
                        }
                        else if (string.IsNullOrEmpty(Medida))
                        {
                            TempData["Mensaje"] = "¡Debes de llenar el Medida!";
                            return View("Asignados", controlFal);
                        }
                        else if (Posis == 0)
                        {
                            TempData["Mensaje"] = "¡Debes de tener una Posición!";
                            return View("Asignados", controlFal);
                        }
                    }
                    controlFal = con.ModificadorFall(2, 4, model.Data?[0].EmpS?[0].cveEmp.ToString(), NumTicket.ToString(), 0.ToString(), Convert.ToInt32(Apooyo_Asigna), Clasif_Asigna, TiempAsig?.ToString("yyyy-MM-dd HH:mm"), "", model.Data[0].idus, TiempAsig?.ToString("yyyy-MM-dd HH:mm"), 0, idsub, pagina, pageSize, CheckDisel, CheckGrua, Dot, Marca, Medida, Posis);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 0;
                        TempData["FehTick"] = DateTime.Now.ToString("yyyy-MM-dd");
                        TempData["guardado"] = "¡Se Guardado Correctamenre!" + controlFal.status + " " + controlFal.message;
                    }
                    else
                    {
                        TempData["Mensaje"] = "¡Error de guardado! " + controlFal.status + " " + controlFal.message;
                    }
                }
                if (controlFal.status != 200)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        msj.status = controlFal.status;
                        msj.message = controlFal.message;
                        return View("Error", msj);
                    }
                }
                ViewData["Title"] = "Asignados";
                return View("Asignados", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult Repara(int pagina, string Token, string cveEmp, string Buscar, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (FehTick == null || FehTick == DateTime.MinValue)
                {
                    FehTick = DateTime.Now;
                    TempData["FehTick"] = FehTick.ToString("yyyy-MM-dd");
                }
                else
                {
                    TempData["FehTick"] = FehTick;
                }
                if (Convert.ToInt32(Buscar) == 0)
                {
                    controlFal = con.PrimerCarga(4, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 0;
                    if (controlFal.status != 200)
                    {
                        if (controlFal.Solicitudes.Count != 0)
                        {
                            msj.status = controlFal.status;
                            msj.message = "!" + controlFal.message + "!";
                            return View("Error", msj);
                        }
                        else
                        {
                            TempData["Mensaje"] = controlFal.status + " !" + controlFal.message + "!";
                        }
                    }
                    else
                    {
                        ViewData["Title"] = "Reparacion";
                    }
                }
                else if (Convert.ToInt32(Buscar) == 1)
                {
                    controlFal = con.PrimerCarga(4, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 1;
                        TempData["NumTicket"] = NumTicket;
                        ViewData["ClaveTipoFalla"] = TipFalla;
                        ViewData["TipTicket"] = TipTicket;
                    }
                    if (controlFal.status != 200)
                    {
                        if (controlFal.Solicitudes.Count != 0)
                        {
                            msj.status = controlFal.status;
                            msj.message = controlFal.message;
                            return View("Error", msj);
                        }
                    }
                }
                ViewData["Title"] = "Reparacion";
                return View("Reparacion", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        [HttpPost]
        public IActionResult BuscarReparacion(string Token, string cveEmp, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int pagina, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;

                if (FehTick == null || FehTick == DateTime.MinValue)
                {
                    FehTick = DateTime.Now;
                    TempData["FehTick"] = FehTick;
                }
                else
                {
                    TempData["FehTick"] = FehTick;
                }
                if (NumTicket == 0)
                {
                    controlFal = con.PrimerCarga(4, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(4, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                }
                if (controlFal.status == 200)
                {
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 1;
                }
                else if (controlFal.status == 400)
                {
                    TempData["Mensaje"] = "¡" + controlFal.status + " " + controlFal.message + "!";
                    TempData["NumTicket"] = NumTicket;
                    TempData["ClaveTipoFalla"] = TipFalla;
                    TempData["TipTicket"] = TipTicket;
                }
                if (controlFal.status > 400)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        msj.status = controlFal.status;
                        msj.message = controlFal.message;
                        return View("Error", msj);
                    }
                }
                ViewData["Title"] = "Reparacion";
                return View("Reparacion", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [HttpPost]
        public IActionResult AsigRepa(string Tok, string cveEmp, string NumTicket, DateTime FechEstima, DateTime FechEstimaComparar, string ComeMotvAsig, int idsub, int pagina, int Diesel, int Grua)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Tok);
                model.Token = Tok;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;

                if (FechEstima > FechEstimaComparar && FechEstima > DateTime.Now)
                {
                    controlFal = con.ModificadorFall(4, 5, model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, "0", 0, 0, FechEstima.ToString("yyyy-MM-dd HH:mm:ss"), ComeMotvAsig, model.Data[0].idus, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, idsub, pagina, pageSize, Diesel, Grua, "", "", "", 0);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 0;
                        ViewData["Title"] = "Reparacion";
                        TempData["guardado"] = controlFal.status + "\r\n¡" + controlFal.message + "!";
                        TempData["FehTick"] = FechEstima;
                        return View("Reparacion", controlFal);
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                        ViewData["Title"] = "Reparacion";
                        TempData["FehTick"] = FechEstima;
                        return View("Reparacion", controlFal);
                    }
                }
                if (FechEstima == FechEstimaComparar)
                {
                    controlFal = con.ModificadorFall(4, 5, model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, "0", 0, 0, FechEstima.ToString("yyyy-MM-dd HH:mm:ss"), ComeMotvAsig, model.Data[0].idus, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, idsub, pagina, pageSize, Diesel, Grua, "", "", "", 0);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 0;
                        ViewData["Title"] = "Reparacion";
                        TempData["guardado"] = controlFal.status + "\r\n¡" + controlFal.message + "!";
                        TempData["FehTick"] = FechEstima;
                        return View("Reparacion", controlFal);
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                        ViewData["Title"] = "Reparacion";
                        TempData["FehTick"] = FechEstima;
                        return View("Reparacion", controlFal);
                    }
                }
                else
                {
                    controlFal = con.PrimerCarga(4, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (controlFal.status != 200)
                    {
                        TempData["Mensaje"] = "¡No se encuentran datos! " + controlFal.status + " " + controlFal.message;
                    }
                    if (controlFal.status > 400)
                    {
                        if (controlFal.Solicitudes.Count != 0)
                        {
                            msj.status = controlFal.status;
                            msj.message = controlFal.message;
                            return View("Error", msj);
                        }
                    }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 0;
                    TempData["FehTick"] = DateTime.Now;
                    TempData["Mensaje"] = "¡Seleccione una fecha mayor que la registrada " + FechEstima.ToString("yyyy-MM-dd HH:mm:ss") + " !";
                    return View("Reparacion", controlFal);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult Fin(int pagina, string Token, string cveEmp, string Buscar, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (Convert.ToInt32(Buscar) == 0)
                {
                    if (FehTick == null || FehTick == DateTime.MinValue)
                    {
                        FehTick = DateTime.Now;
                        TempData["FehTick"] = FehTick;
                    }
                    else
                    {
                        TempData["FehTick"] = FehTick;
                    }
                    controlFal = con.PrimerCarga(5, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 0;
                }
                else if (Convert.ToInt32(Buscar) == 1)
                {
                    if (FehTick == null || FehTick == DateTime.MinValue)
                    {
                        FehTick = DateTime.Now;
                        TempData["FehTick"] = FehTick;
                    }
                    else
                    {
                        TempData["FehTick"] = FehTick;
                    }
                    controlFal = con.PrimerCarga(5, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                    //oLista = con.Primer_crga_con_Catalogos(5, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, FehTick.ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (controlFal.status == 200)
                    {
                        ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                        ViewBag.CurrentPage = pagina;
                        TempData["Buscar"] = 1;
                        ViewData["TipTicket"] = TipTicket;
                        ViewData["TipFalla"] = TipFalla;
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                if (controlFal.status > 400)
                {
                    msj.status = controlFal.status;
                    msj.message = controlFal.message;
                    return View("Error", msj);
                }
                else
                {
                    if (controlFal.Solicitudes.Count == 0)
                    {
                        TempData["Mensaje"] = controlFal.status + " !" + controlFal.message + "¡";
                        ViewData["Title"] = "Finalizado";
                        return View("Finalizado", controlFal);
                    }
                    else
                    {
                        ViewData["Title"] = "Finalizado";
                        return View("Finalizado", controlFal);
                    }
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [HttpPost]
        public IActionResult BuscarFinalizados(string Token, string cveEmp, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int pagina, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (FehTick == null || FehTick == DateTime.MinValue)
                {
                    FehTick = DateTime.Now;
                    TempData["FehTick"] = FehTick;
                }
                else
                {
                    TempData["FehTick"] = FehTick;
                }
                if (NumTicket == 0)
                {
                    controlFal = con.PrimerCarga(5, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(5, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, idsub, pagina, pageSize);
                }
                if (controlFal.status == 200)
                {
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 1;
                }
                else
                {
                    TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                }
                if (controlFal.status > 400)
                {
                    msj.status = controlFal.status;
                    msj.message = controlFal.message;
                    return View("Error", msj);
                }
                ViewData["Title"] = "Finalizado";
                TempData["FehTick"] = FehTick.ToString("yyyy-MM-dd");
                ViewData["TipTicket"] = TipTicket;
                ViewData["TipFalla"] = TipFalla;

                return View("Finalizado", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult Consul(string Token, string cveEmp, int NumTicket, DateTime FehInicio, DateTime FehFin, int idsub, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                TempData["FehInicio"] = FehInicio;
                TempData["FehFin"] = FehFin;
                if (NumTicket == 0)
                {
                    NumTicket = 0;
                }
                else if (FehInicio == null || FehInicio == DateTime.MinValue)
                {
                    FehInicio = DateTime.MinValue;
                }
                else if (FehFin == null || FehFin == DateTime.MinValue)
                {
                    FehInicio = DateTime.MinValue;
                }
                controlFal = con.ConsultaGeneral(cveEmp, NumTicket, FehInicio.ToString("yyyy-MM-dd HH:mm:ss"), FehFin.ToString("yyyy-MM-dd HH:mm:ss"), pagina, pageSize);
                if (controlFal.Solicitudes.Count == 0)
                {
                    if (controlFal.TotalSolicitudes == null) { controlFal.TotalSolicitudes = 0; }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                }
                else
                {
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                }
                ViewData["Title"] = "Consulta";
                return View("Consulta", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [HttpPost]
        public IActionResult BusConsul(string Token, DateTime FehInicio, DateTime FehFin, int NumTicket, string cveEmp, int idsub, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                if (NumTicket == 0)
                {
                    if (FehFin == null || FehFin == DateTime.MinValue)
                    {
                        TempData["Mensaje"] = "¡Seleccione una Fecha de Fin!";
                    }
                    else
                    {
                        controlFal = con.ConsultaGeneral(cveEmp, NumTicket, FehInicio.ToString("yyyy-MM-dd HH:mm:ss"), FehFin.ToString("yyyy-MM-dd HH:mm:ss"), pagina, pageSize);
                    }
                }
                else
                {
                    controlFal = con.ConsultaGeneral(cveEmp, NumTicket, null, null, pagina, pageSize);
                }
                if (controlFal.status == 200)
                {
                    if (controlFal.TotalSolicitudes == null) { controlFal.TotalSolicitudes = 0; }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["FehInicio"] = FehInicio;
                    TempData["FehFin"] = FehFin;
                }
                if (controlFal.status == 400)
                {
                    if (controlFal.TotalSolicitudes == null) { controlFal.TotalSolicitudes = 0; }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Mensaje"] = controlFal.status + "\r\n ¡" + controlFal.message + "\r\n!";
                    TempData["FehInicio"] = FehInicio;
                }
                ViewData["Title"] = "Consulta";
                return View("Consulta", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult Indica(int cveEmp, string Token, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                ViewData["Title"] = "Indicadores";
                controlFal = con.PrimerCarga_sin_catlog(0, model.Data[0]?.EmpS[0].cveEmp.ToString(), model.Data[0].idus.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), 0, idsub);
                return View("Indica", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
    }
}
