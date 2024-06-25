using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Reflection;

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
        public IActionResult PorAsig(string menu, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);

                controlFal = con.CargarCat(model.Data[0].EmpS[0].cveEmp.ToString(), 1);
                ViewData["UsuarioModel"] = model;
                ViewData["Title"] = "Por Asignar";
                TempData["FehTick"] = DateTime.Now;
                return View("PorAsignar", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        [HttpGet, HttpPost]
        public IActionResult BuscarPorAsig(string UsDat, int NumTicket, int ClaveTipoFalla, DateTime? FehTick, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                //model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model = JsonConvert.DeserializeObject<UsuarioModel>(UsDat);
                model.Token = model.Token;
                ViewData["UsuarioModel"] = model;
                if (NumTicket == 0)
                {
                    controlFal = con.PrimerCarga(1, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick?.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, 0, ClaveTipoFalla, 0, 0, 0, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(1, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, 0, 0, 0, 0, 0, pagina, pageSize);
                }
                if (controlFal.status == 200)
                {
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                    TempData["Buscar"] = 1;
                    TempData["NumTicket"] = NumTicket;
                    TempData["ClaveTipoFalla"] = ClaveTipoFalla;
                    if (FehTick == null || FehTick == DateTime.MinValue)
                    {
                        FehTick = DateTime.Now;
                    }
                    TempData["FehTick"] = FehTick;
                }
                else
                {
                    TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                    TempData["FehTick"] = DateTime.Now;
                }
                if (controlFal.status != 200)
                {
                    if (controlFal.Solicitudes.Count != 0)
                    {
                        TempData["Mensaje"] = controlFal.status + ' ' + controlFal.message;
                        return RedirectToAction("PorAsig", new { menu, model.idsub });
                    }
                }
                ViewData["UsuarioModel"] = model;
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

        public IActionResult AsignacionTicket(string UsDat, string Asigna, string ticket, int pagina, int idsub, int Diesel, int Grua)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                //model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model = JsonConvert.DeserializeObject<UsuarioModel>(UsDat);
                //model.Token = Token;
                //model.idsub = idsub;
                ViewData["UsuarioModel"] = model;
                DateTime FehAsignatiempo = DateTime.Now;
                if (Asigna != "[Selecciona]")
                {
                    controlFal = con.ModificadorFall(1, 2, model.Data[0].EmpS[0].cveEmp.ToString(), ticket, Asigna, 0, 0, "", "", model.Data[0].idus, FehAsignatiempo.ToString("yyyy-MM-dd"), 0, idsub, pagina, pageSize, Diesel, Grua, "", "", "", 0);
                    if (controlFal.status == 200)
                    {
                        TempData["guardado"] = controlFal.status + "¡ \r\n" + controlFal.message + "\r\n!";
                        return RedirectToAction("BuscarPorAsig", new { UsDat, ticket, ClaveTipoFalla = 0, DateTime.Now, pagina });
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                        return RedirectToAction("PorAsig", new { UsDat, model.idsub, DateTime.Now, pagina });
                    }
                }
                else
                {
                    return RedirectToAction("PorAsig", new { UsDat, model.idsub, DateTime.Now, pagina });

                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult Asignacion(string menu, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                //model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                ViewData["UsuarioModel"] = model;

                controlFal = con.CargarCat(model.Data[0].EmpS[0].cveEmp.ToString(), 2);
                TempData["Buscar"] = 0;
                TempData["FehTick"] = DateTime.Now;
                if (controlFal.status == 200)
                {
                    ViewData["Title"] = "Asignados";

                }
                else if (controlFal.Solicitudes.Count != 0)
                {
                    msj.status = controlFal.status;
                    msj.message = controlFal.message;
                }
                return View("Asignados", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [HttpGet, HttpPost]
        public IActionResult BuscarAsignados(string menu, int UsAsignado, int NumTicket, DateTime FehTick, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");
                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                ViewData["UsuarioModel"] = model;
                if (NumTicket == 0)
                {
                    controlFal = con.PrimerCarga(2, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, 0, 0, model.Data[0].idus, UsAsignado, model.idsub.Value, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(2, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, 0, 0, model.Data[0].idus, UsAsignado, model.idsub.Value, pagina, pageSize);
                }
                if (controlFal.status != 200)
                {
                    TempData["Mensaje"] = "¡" + controlFal.status + " " + controlFal.message + "!";
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
                if (FehTick == null || FehTick == DateTime.MinValue)
                {
                    FehTick = DateTime.Now;
                    TempData["FehTick"] = FehTick.ToString("yyyy-MM-dd");
                }
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
        public IActionResult AsigTiempApoyClasif(string menu, DateTime? TiempAsig, int Apooyo_Asigna, int Clasif_Asigna, int NumTicket, int pagina, int CheckDisel, int CheckGrua, string Dot, string Marca, string Medida, int Posis)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                ViewData["UsuarioModel"] = model;
                TempData["FehTick"] = DateTime.Now.ToString("yyyy-MM-dd");
                if (Apooyo_Asigna == 0)
                {
                    TempData["Mensaje"] = "¡Seleccione un Apoyo!";
                    return RedirectToAction("BuscarAsignados", new { menu, UsAsignado = 0, NumTicket = 0, FehTick = DateTime.Now, pagina });
                }
                if (!TiempAsig.HasValue || TiempAsig <= DateTime.Now)
                {
                    TempData["Mensaje"] = "¡La fecha de asignación no puede ser nulla o anterior.!";
                    return RedirectToAction("BuscarAsignados", new { menu, UsAsignado = 0, NumTicket = 0, FehTick = DateTime.Now, pagina });
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
                            return RedirectToAction("BuscarAsignados", new { menu, UsAsignado = 0, NumTicket = 0, FehTick = DateTime.Now, pagina });
                        }
                        else if (string.IsNullOrEmpty(Marca))
                        {
                            TempData["Mensaje"] = "¡Debes de llenar la Marca!";
                            return RedirectToAction("BuscarAsignados", new { menu, UsAsignado = 0, NumTicket = 0, FehTick = DateTime.Now, pagina });
                        }
                        else if (string.IsNullOrEmpty(Medida))
                        {
                            TempData["Mensaje"] = "¡Debes de llenar el Medida!";
                            return RedirectToAction("BuscarAsignados", new { menu, UsAsignado = 0, NumTicket = 0, FehTick = DateTime.Now, pagina });
                        }
                        else if (Posis == 0)
                        {
                            TempData["Mensaje"] = "¡Debes de tener una Posición!";
                            return RedirectToAction("BuscarAsignados", new { menu, UsAsignado = 0, NumTicket = 0, FehTick = DateTime.Now, pagina });
                        }
                    }
                    controlFal = con.ModificadorFall(2, 4, model.Data?[0].EmpS?[0].cveEmp.ToString(), NumTicket.ToString(), 0.ToString(), Convert.ToInt32(Apooyo_Asigna), Clasif_Asigna, TiempAsig?.ToString("yyyy-MM-dd HH:mm"), "", model.Data[0].idus, TiempAsig?.ToString("yyyy-MM-dd HH:mm"), 0, model.idsub.Value, pagina, pageSize, CheckDisel, CheckGrua, Dot, Marca, Medida, Posis);
                    if (controlFal.status == 200)
                    {
                        TempData["FehTick"] = DateTime.Now.ToString("yyyy-MM-dd");
                        TempData["guardado"] = "¡Se Guardado Correctamenre!" + controlFal.status + " " + controlFal.message;
                        return RedirectToAction("BuscarAsignados", new { menu, UsAsignado = 0, NumTicket = 0, FehTick = DateTime.Now, pagina });
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

        public IActionResult Repara(string menu, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                ViewData["UsuarioModel"] = model;
                TempData["FehTick"] = DateTime.Now.ToString("yyyy-MM-dd");
                controlFal = con.CargarCat(model.Data[0].EmpS[0].cveEmp.ToString(), 4);
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

        [HttpGet, HttpPost]
        public IActionResult BuscarReparacion(string menu, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
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
                    controlFal = con.PrimerCarga(4, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, model.idsub.Value, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(4, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, model.idsub.Value, pagina, pageSize);
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
                TempData["NumTicket"] = NumTicket;
                TempData["ClaveTipoFalla"] = TipFalla;
                TempData["TipTicket"] = TipTicket;
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
        public IActionResult AsigRepa(string menu, string NumTicket, DateTime FechEstima, DateTime FechEstimaComparar, string ComeMotvAsig, int pagina, int Diesel, int Grua)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                ViewData["UsuarioModel"] = model;

                if (FechEstima > FechEstimaComparar && FechEstima > DateTime.Now)
                {
                    controlFal = con.ModificadorFall(4, 5, model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, "0", 0, 0, FechEstima.ToString("yyyy-MM-dd HH:mm:ss"), ComeMotvAsig, model.Data[0].idus, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, model.idsub.Value, pagina, pageSize, Diesel, Grua, "", "", "", 0);
                    if (controlFal.status == 200)
                    {
                        TempData["Buscar"] = 0;
                        ViewData["Title"] = "Reparacion";
                        TempData["guardado"] = controlFal.status + "\r\n¡" + controlFal.message + "!";
                        TempData["FehTick"] = FechEstima;
                        return RedirectToAction("BuscarReparacion", new { menu, FehTick = DateTime.Now, TipTicket = 0, TipFalla = 0, NumTicket = 0, pagina });
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                        ViewData["Title"] = "Reparacion";
                        TempData["FehTick"] = FechEstima;
                        return RedirectToAction("BuscarReparacion", new { menu, FehTick = DateTime.Now, TipTicket = 0, TipFalla = 0, NumTicket = 0, pagina });
                    }
                }
                if (FechEstima == FechEstimaComparar)
                {
                    controlFal = con.ModificadorFall(4, 5, model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, "0", 0, 0, FechEstima.ToString("yyyy-MM-dd HH:mm:ss"), ComeMotvAsig, model.Data[0].idus, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, model.idsub.Value, pagina, pageSize, Diesel, Grua, "", "", "", 0);
                    if (controlFal.status == 200)
                    {
                        TempData["Buscar"] = 0;
                        ViewData["Title"] = "Reparacion";
                        TempData["guardado"] = controlFal.status + "\r\n¡" + controlFal.message + "!";
                        TempData["FehTick"] = FechEstima;
                        return RedirectToAction("BuscarReparacion", new { menu, FehTick = DateTime.Now, TipTicket = 0, TipFalla = 0, NumTicket = 0, pagina });
                    }
                    else
                    {
                        TempData["Mensaje"] = controlFal.status + " ¡" + controlFal.message + "\r\n Intenta mas Tarde! \r\n ";
                        ViewData["Title"] = "Reparacion";
                        TempData["FehTick"] = FechEstima;
                        return RedirectToAction("BuscarReparacion", new { menu, FehTick = DateTime.Now, TipTicket = 0, TipFalla = 0, NumTicket = 0, pagina });
                    }
                }
                else
                {
                    TempData["Buscar"] = 0;
                    TempData["FehTick"] = DateTime.Now;
                    TempData["Mensaje"] = "¡Seleccione una fecha mayor que la registrada " + FechEstima.ToString("yyyy-MM-dd HH:mm:ss") + " !";
                    return RedirectToAction("BuscarReparacion", new { menu, FehTick = DateTime.Now, TipTicket = 0, TipFalla = 0, NumTicket = 0, pagina });
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }

        public IActionResult Fin(string menu, int idsub)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                //model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                //model.Token = Token;
                //model.idsub = idsub;
                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                ViewData["UsuarioModel"] = model;
                TempData["FehTick"] = DateTime.Now;
                controlFal = con.CargarCat(model.Data[0].EmpS[0].cveEmp.ToString(), 5);
                ViewData["Title"] = "Finalizado";
                return View("Finalizado", controlFal);
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                return View("Error", msj);
            }
        }
        [HttpGet, HttpPost]
        public IActionResult BuscarFinalizados(string menu, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                //model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                //model.Token = Token;
                //model.idsub = idsub;
                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
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
                    controlFal = con.PrimerCarga(5, model.Data[0].EmpS[0].cveEmp.ToString(), FehTick.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, model.idsub.Value, pagina, pageSize);
                }
                else
                {
                    controlFal = con.PrimerCarga(5, model.Data[0].EmpS[0].cveEmp.ToString(), null, NumTicket, TipTicket, TipFalla, model.Data[0].idus, 0, model.idsub.Value, pagina, pageSize);
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

        public IActionResult Consul(string menu, int NumTicket, DateTime FehInicio, DateTime FehFin, int idsub, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                //model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                //model.Token = Token;
                //model.idsub = idsub;

                if (NumTicket == 0)
                {
                    NumTicket = 0;
                }
                if (FehInicio == null || FehInicio == DateTime.MinValue)
                {
                    FehInicio = DateTime.MinValue;
                }
                if (FehFin == null || FehFin == DateTime.MinValue)
                {
                    FehFin = DateTime.Now;
                }
                controlFal = con.ConsultaGeneral(model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, FehFin.ToString("yyyy-MM-dd HH:mm:ss"), FehInicio.ToString("yyyy-MM-dd HH:mm:ss"), pagina, pageSize);
                if (controlFal.Solicitudes.Count == 0)
                {
                    if (controlFal.TotalSolicitudes == null) { controlFal.TotalSolicitudes = 0; }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                }
                else
                {
                    for (int i = 0; i < controlFal.Solicitudes.Count; i++)
                    {
                        controlFal.Solicitudes[i].PathArchivo = ObtenerArchivosPorTicket(controlFal.Solicitudes[i].NumTicket);
                    }
                    ViewBag.TotalPages = (int)Math.Ceiling((double)controlFal.TotalSolicitudes / pageSize);
                    ViewBag.CurrentPage = pagina;
                }
                ViewData["UsuarioModel"] = model;
                TempData["FehInicio"] = FehInicio;
                TempData["FehFin"] = FehFin;
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

        [HttpGet, HttpPost]
        public IActionResult BusConsul(string menu, DateTime FehInicio, DateTime FehFin, int NumTicket, int pagina)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["usuario"]) || string.IsNullOrEmpty(HttpContext.Request.Cookies["contra"]))
                    return RedirectToAction("Index", "Loging");

                //model = menu.RegresMenu(UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["usuario"]), UrlEncryptor.DecryptUrl(HttpContext.Request.Cookies["contra"]), Convert.ToInt32(cveEmp), url, Token);
                //model.Token = Token;
                //model.idsub = idsub;
                model = JsonConvert.DeserializeObject<UsuarioModel>(menu);
                ViewData["UsuarioModel"] = model;
                if (NumTicket == 0)
                {
                    if (FehInicio == null || FehInicio == DateTime.MinValue)
                    {
                        TempData["Mensaje"] = "¡Seleccione una Fecha de Inicio!";
                    }
                    else
                    {
                        controlFal = con.ConsultaGeneral(model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, FehInicio.ToString("yyyy-MM-dd HH:mm:ss"), FehFin.ToString("yyyy-MM-dd HH:mm:ss"), pagina, pageSize);
                    }
                }
                else
                {
                    controlFal = con.ConsultaGeneral(model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, null, null, pagina, pageSize);
                }
                if (controlFal.status == 200)
                {
                    if (controlFal.TotalSolicitudes == null) { controlFal.TotalSolicitudes = 0; }
                    for (int i = 0; i < controlFal.Solicitudes.Count; i++)
                    {
                        controlFal.Solicitudes[i].PathArchivo = ObtenerArchivosPorTicket(controlFal.Solicitudes[i].NumTicket);
                    }
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
                    TempData["FehFin"] = FehFin;
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
        private List<archivo> ObtenerArchivosPorTicket(int numTicket)
        {
            List<archivo> listarch = new List<archivo>();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\GMI\\" + numTicket.ToString());

            if (Directory.Exists(path))
            {
                string[] fileNames = Directory.GetFiles(path);

                foreach (var fileName in fileNames)
                {
                    listarch.Add(new archivo { carpet = numTicket, exte = Path.GetExtension(fileName).ToString(), rutFile = numTicket.ToString() + "/" + Path.GetFileName(fileName) });
                }
            }
            return listarch;
        }

        [HttpPost]
        public async Task<ActionResult> SubirArchivo(List<IFormFile> Files, int pagina, string Token, string cveEmp, DateTime FehInicio, DateTime FehFin, int NumTicket, int idsub)
        {
            try
            {
                foreach (var archivo in Files)
                {
                    if (archivo != null && archivo.Length > 0)
                    {
                        var extension = Path.GetExtension(archivo.FileName).ToLower();

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\GMI\\" + NumTicket.ToString());

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);


                        string fileName = NumTicket.ToString() + "_" + Guid.NewGuid().ToString() + extension;

                        string fileNameWithPath = Path.Combine(path, fileName);

                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                        {
                            using (var image = Image.Load(archivo.OpenReadStream()))
                            {
                                image.Mutate(x => x.Resize(800, 600));
                                image.Save(fileNameWithPath);
                            }
                        }
                        else if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
                        {
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                await archivo.CopyToAsync(stream);
                            }
                        }
                        else
                        {
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                archivo.CopyTo(stream);
                            }
                        }
                    }
                }
                TempData["guardado"] = "Se subieron correctamente las imagenes";
                return RedirectToAction("BusConsul", new { Token, FehInicio, FehFin, NumTicket = 0, cveEmp, idsub, pagina });
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
