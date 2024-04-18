using ConectDB.DB;
using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Reflection;
using static ConectDB.Models.LogUser;

namespace ConectDB.Controllers
{
    public class ControlReparacionesController : Controller
    {
        private string url = "https://webportal.tum.com.mx/wsstmdv/api/accesyst";
        DataApi data = new DataApi();
        ConectApiContrRep con = new ConectApiContrRep();
        ConectMenuUser menu = new ConectMenuUser();
        const int pageSize = 5;
        List<ControlFalla> oLista = new List<ControlFalla>();
        UsuarioModel model = new UsuarioModel();
        List<Error> mensaje = new List<Error>();
        Error msj = new Error();

        public ActionResult Index(int cveEmp, string XT, int idsub)
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

                oLista.Clear();
                model = menu.RegresMenu(desusuario, descontraseña, cveEmp, url, XT);
                model.Token = XT;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                oLista = con.PrimerCarga_sin_catlog(0, model.Data[0]?.EmpS[0].cveEmp.ToString(), model.Data[0].idus.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), 0, idsub);
                //Console.WriteLine("prueba");
                ViewData["Title"] = "Resumen";
                return View("Index", oLista);

            } 
            catch (Exception e) 
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        
        public IActionResult PorAsig(int pagina,  string k, string Token, string cveEmp, string Buscar, int NumTicket, int ClaveTipoFalla, string FehTick, int idsub)
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

                oLista.Clear();
                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                if (Convert.ToInt32(Buscar) == 0)
                {
                    oLista = con.Primer_crga_con_Catalogos(1, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 0;

                    }
                    else
                    {
                        TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                else if (Convert.ToInt32(Buscar) == 1)
                {
                    oLista = con.Primer_crga_con_Catalogos(1, model.Data[0].EmpS[0].cveEmp.ToString(), "", NumTicket, 0, ClaveTipoFalla, Convert.ToDateTime(FehTick).ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 1;
                        ViewData["NumTicket"] = NumTicket;
                        ViewData["ClaveTipoFalla"] = ClaveTipoFalla;
                        ViewData["FehTick"] = FehTick;
                    }
                    else
                    {
                        TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Por Asignar";
                    return View("PorAsignar", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }

        [HttpPost]
        public IActionResult BuscarPorAsig( string Token, string cveEmp, int NumTicket, int ClaveTipoFalla, DateTime FehTick, int pagina, int idsub)
        {
            try 
            {
                oLista.Clear();
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

                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                oLista = con.Primer_crga_con_Catalogos(1, model.Data[0].EmpS[0].cveEmp.ToString(), "", NumTicket, 0, ClaveTipoFalla, FehTick.ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, 0, idsub, pagina, pageSize);
                if (oLista[0].status == 200)
                {
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 1;
                    ViewData["NumTicket"] = NumTicket;
                    ViewData["ClaveTipoFalla"] = ClaveTipoFalla;
                    ViewData["FehTick"] = FehTick;
                    ViewData["Title"] = "Por Asignar";
                }
                else
                {
                    TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                }

                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    return View("PorAsignar", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        
        public IActionResult AsignacionTicket( string Token, string cveEmp, string Asigna, string ticket, int pagina, int idsub)
        {
            try 
            {
                oLista.Clear();
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

                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                if (Asigna != "[Selecciona]")
                {
                    oLista = con.Modfificador_metod(1, 2, model.Data[0].EmpS[0].cveEmp.ToString(), ticket, Asigna, 0, 0, "", "", model.Data[0].idus.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), 0, idsub, pagina, pageSize);
                    //oLista = con.Asigna_Ticket(model.Data[0].EmpS[0].cveEmp.ToString(), model.Data[0].idus.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), Asigna, ticket, pagina, pageSize);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 0;
                        TempData["guardado"] = "¡ " + oLista[0].status + " " + oLista[0].message.ToString() + " !";
                    }
                    else
                    {
                        TempData["Mensaje"] = "¡Error de guardado! " + oLista[0].status + " " + oLista[0].message.ToString();
                        ViewData["Buscar"] = 0;
                    }
                }
                else
                {
                    oLista = con.Primer_crga_con_Catalogos(1, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 0;
                        TempData["Mensaje"] = "¡Seleccione a un paersona a asignar!";
                    }
                    else
                    {
                        TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Por Asignar";
                    return View("PorAsignar", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        
        public IActionResult Asignacion(int pagina,  string k, string Token, string cveEmp, string Buscar, int NumTicket, DateTime FehTick, int UsAsignado, int idsub)
        {
            try 
            {
                oLista.Clear();
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

                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                if (Convert.ToInt32(Buscar) == 0)
                {
                    oLista = con.Primer_crga_con_Catalogos(2, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status != 200)
                    {
                        TempData["Mensaje"] = "¡No se encuentran datos! " + oLista[0].status + " " + oLista[0].message.ToString();
                    }
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 0;
                }
                else if (Convert.ToInt32(Buscar) == 1)
                {
                    oLista = con.Primer_crga_con_Catalogos(2, model.Data[0].EmpS[0].cveEmp.ToString(), "", NumTicket, 0, 0, FehTick.ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, UsAsignado, idsub, pagina, pageSize);
                    if (oLista[0].status != 200)
                    {
                        TempData["Mensaje"] = "¡No se encuentran datos! " + oLista[0].status + " " + oLista[0].message.ToString();
                    }
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 1;
                    ViewData["UsAsignado"] = UsAsignado;
                    ViewData["FehTick"] = FehTick;
                    ViewData["NumTicket"] = NumTicket;
                    ViewData["Title"] = "Asignados";
                    return View("Asignados", oLista);
                }
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Asignados";
                    return View("Asignados", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        [HttpPost]
        
        public IActionResult BuscarAsignados(string Token, string cveEmp, int UsAsignado, int NumTicket, DateTime FehTick, int pagina, int idsub)
        {
            try 
            {
                oLista.Clear();
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


                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                //oLista = con.Primer_crga_con_Catalogos(int CveEstatus, string empresa, string fecha, int NumTicket, int TipoTicket, int TipoFalla, string FehTick, string CveUser, int pagina, int tamañomuestra)
                oLista = con.Primer_crga_con_Catalogos(2, model.Data[0].EmpS[0].cveEmp.ToString(), "", NumTicket, 0, 0, FehTick.ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, UsAsignado, idsub, pagina, pageSize);
                if (oLista[0].status != 200)
                {
                    TempData["Mensaje"] = "¡No se encuentran datos! " + oLista[0].status + " " + oLista[0].message.ToString();
                }
                int totalSolicitudes = oLista[0].TotalSolicitudes;
                int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = pagina;
                ViewData["Buscar"] = 1;
                ViewData["UsAsignado"] = UsAsignado;
                ViewData["FehTick"] = FehTick;
                ViewData["NumTicket"] = NumTicket;
                ViewData["Title"] = "Asignados";
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    return View("Asignados", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        [HttpPost]
        
        public IActionResult AsigTiempApoyClasif( string XT, string cveEmp, DateTime? TiempAsig, int Apooyo_Asigna, int Clasif_Asigna, int NumTicket, int pagina, int idsub)
        {
            try 
            {
                oLista.Clear();
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

                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, XT);

                model.Token = XT;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;

                if (Apooyo_Asigna == 0)
                {
                    oLista = con.Primer_crga_con_Catalogos(2, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status != 200)
                    {
                        TempData["Mensaje"] = "¡No se encuentran datos! " + oLista[0].status + " " + oLista[0].message.ToString();
                    }
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 0;
                    TempData["Mensaje"] = "¡Seleccione un Apoyo!";
                    return View("Asignados", oLista);
                }
                if (!TiempAsig.HasValue || TiempAsig <= DateTime.Now)
                {
                    oLista = con.Primer_crga_con_Catalogos(2, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status != 200)
                    {
                        TempData["Mensaje"] = "¡No se encuentran datos! " + oLista[0].status + " " + oLista[0].message.ToString();
                    }
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 0;
                    TempData["Mensaje"] = "¡La fecha de asignación no puede ser nulla o anterior.!";
                    return View("Asignados", oLista);
                }
                else
                {
                    oLista = con.Modfificador_metod(2, 4, model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket.ToString(), 0.ToString(), Convert.ToInt32(Apooyo_Asigna), Clasif_Asigna, TiempAsig?.ToString("yyyy-MM-dd HH:mm"), "", model.Data[0].idus.ToString(), TiempAsig?.ToString("yyyy-MM-dd HH:mm"), 0, idsub, pagina, pageSize);
                    //oLista = con.AsignacionTiempoApoyClasificacion(NumTicket, Apooyo_Asigna, Clasif_Asigna, model.Data[0].EmpS[0].cveEmp.ToString(), TiempAsig, model.Data[0].idus);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 0;
                        TempData["guardado"] = "¡Se Guardado Correctamenre!" + oLista[0].status + " " + oLista[0].message.ToString();
                    }
                    else
                    {
                        TempData["Mensaje"] = "¡Error de guardado! " + oLista[0].status + " " + oLista[0].message.ToString();
                    }
                }
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Asignados";
                    return View("Asignados", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        
        public IActionResult Repara(string k, string Token, string cveEmp, int pagina, int idsub)
        {
            try 
            {
                oLista.Clear();
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


                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                //model.Data[0].usuario = usuario;
                //model.Data[0].contraseña = contraseña;
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                oLista = con.Primer_crga_con_Catalogos(4, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                int totalSolicitudes = oLista[0].TotalSolicitudes;
                int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = pagina;
                ViewData["Buscar"] = 0;
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Reparacion";
                    return View("Reparacion", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }

        [HttpPost]
        
        public IActionResult BuscarReparacion(string Token, string cveEmp, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int pagina, int idsub)
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

                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                //model.Data[0].usuario = usuario;
                //model.Data[0].contraseña = contraseña;
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                oLista = con.Primer_crga_con_Catalogos(4, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, FehTick.ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, 0, idsub, pagina, pageSize);
                if (oLista[0].status == 200)
                {
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 1;
                }
                else
                {
                    TempData["Mensaje"] = "¡" + oLista[0].status + " " + oLista[0].message.ToString() + "!";
                }
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Reparacion";
                    return View("Reparacion", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        [HttpPost]
        
        public IActionResult AsigRepa(string Tok, string cveEmp, string NumTicket, DateTime FechEstima,DateTime FechEstimaComparar ,string ComeMotvAsig, int idsub, int pagina)
        {
            try 
            {
                oLista.Clear();
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

                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Tok);
                //model.Data[0].usuario = User;
                //model.Data[0].contraseña = Contra;
                model.Token = Tok;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;

                if (FechEstima > FechEstimaComparar && FechEstima > DateTime.Now)
                {
                    oLista = con.Modfificador_metod(4, 5, model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, "0", 0, 0, FechEstima.ToString("yyyy-MM-dd HH:mm:ss"), ComeMotvAsig, model.Data[0].idus.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, idsub, pagina, pageSize);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 0;
                        ViewData["Title"] = "Reparacion";
                        TempData["guardado"] = +oLista[0].status + "\r\n¡" + oLista[0].message + "!";
                        return View("Reparacion", oLista);
                    }
                    else
                    {
                        TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                        ViewData["Title"] = "Reparacion";
                        return View("Reparacion", oLista);
                    }
                }
                if (FechEstima == FechEstimaComparar)
                {
                    oLista = con.Modfificador_metod(4, 5, model.Data[0].EmpS[0].cveEmp.ToString(), NumTicket, "0", 0, 0, FechEstima.ToString("yyyy-MM-dd HH:mm:ss"), ComeMotvAsig, model.Data[0].idus.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, idsub, pagina, pageSize);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 0;
                        ViewData["Title"] = "Reparacion";
                        TempData["guardado"] = +oLista[0].status + "\r\n¡" + oLista[0].message + "!";
                        return View("Reparacion", oLista);
                    }
                    else
                    {
                        TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                        ViewData["Title"] = "Reparacion";
                        return View("Reparacion", oLista);
                    }
                }
                else
                {
                    oLista = con.Primer_crga_con_Catalogos(4, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status != 200)
                    {
                        TempData["Mensaje"] = "¡No se encuentran datos! " + oLista[0].status + " " + oLista[0].message.ToString();
                    }
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 0;
                    TempData["Mensaje"] = "¡Seleccione una fecha mayor que la registrada " + FechEstima.ToString("yyyy-MM-dd HH:mm:ss") + " !";
                    return View("Reparacion", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        
        public IActionResult Fin(int pagina, string k, string Token, string cveEmp, string Buscar, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int idsub)
        {
            try 
            {
                oLista.Clear();
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

                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                //model.Data[0].usuario = usuario;
                //model.Data[0].contraseña = contraseña;
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                if (Convert.ToInt32(Buscar) == 0)
                {
                    oLista = con.Primer_crga_con_Catalogos(5, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, 0, 0, "", model.Data[0].idus, 0, idsub, pagina, pageSize);
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 0;
                }
                else if (Convert.ToInt32(Buscar) == 1)
                {
                    oLista = con.Primer_crga_con_Catalogos(5, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, FehTick.ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, 0, idsub, pagina, pageSize);
                    if (oLista[0].status == 200)
                    {
                        int totalSolicitudes = oLista[0].TotalSolicitudes;
                        int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                        ViewBag.TotalPages = totalPages;
                        ViewBag.CurrentPage = pagina;
                        ViewData["Buscar"] = 1;
                    }
                    else
                    {
                        TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                    }
                }
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Finalizado";
                    return View("Finalizado", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
        [HttpPost]
        
        public IActionResult BuscarFinalizados(string Token, string cveEmp, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket, int pagina, int idsub)
        {
            try 
            {
                oLista.Clear();
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


                model = menu.RegresMenu(desusuario, descontraseña, Convert.ToInt32(cveEmp), url, Token);
                //model.Data[0].usuario = usuario;
                //model.Data[0].contraseña = contraseña;
                model.Token = Token;
                model.idsub = idsub;
                HttpContext.Session.SetString("UsuarioModel", JsonConvert.SerializeObject(model));
                ViewData["UsuarioModel"] = model;
                oLista = con.Primer_crga_con_Catalogos(5, model.Data[0].EmpS[0].cveEmp.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), NumTicket, TipTicket, TipFalla, FehTick.ToString("yyyy-MM-dd HH:mm:ss"), model.Data[0].idus, 0, idsub, pagina, pageSize);
                if (oLista[0].status == 200)
                {
                    int totalSolicitudes = oLista[0].TotalSolicitudes;
                    int totalPages = (int)Math.Ceiling((double)totalSolicitudes / pageSize);
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pagina;
                    ViewData["Buscar"] = 1;
                }
                else
                {
                    TempData["Mensaje"] = +oLista[0].status + " ¡" + oLista[0].message + "\r\n Intenta mas Tarde! \r\n ";
                }
                if (oLista.Count == 0)
                {
                    msj.status = 400;
                    msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo";
                    mensaje.Add(msj);
                    return View("Error", mensaje);
                }
                else
                {
                    ViewData["Title"] = "Finalizado";
                    return View("Finalizado", oLista);
                }
            }
            catch (Exception e)
            {
                msj.status = 400;
                msj.message = "Error de Conexion | Erorr Desconocido Notificar a Sistemas Desarrollo" + e.Message.ToString();
                mensaje.Add(msj);
                return View("Error", mensaje);
            }
        }
    }
}
