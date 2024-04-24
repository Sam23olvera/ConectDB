using ConectDB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static ConectDB.Models.LogUser;
using System.Security.Policy;
using System.Collections;
using NuGet.Packaging;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Drawing.Printing;

namespace ConectDB.DB
{
    public class ConectApiContrRep
    {
        private string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
        DataApi hh = new DataApi();
        List<ControlFalla> lista = new List<ControlFalla>();

        public List<ControlFalla> PrimerCarga_sin_catlog(int CveEstatus, string empresa, string CveUser, string fecha, int UserFiltro, int IdSubmodulo)
        {
            JObject jsdat = new JObject();
            try
            {
                //jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "}]}");
                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + UserFiltro + "},{\"property\":\"IdSubmodulo\",\"value\":" + IdSubmodulo + "}]}");
                string datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                if (data != null && data.Count > 0)
                {
                    ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                    lista.Add(cfal);
                }
                return lista;
            }
            catch (Exception e)
            {
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                lista.Add(catfal);
                return lista;
            }
        }

        public List<ControlFalla> Primer_crga_con_Catalogos(int CveEstatus, string empresa, string fecha, int NumTicket, int TipoTicket, int TipoFalla, string FehTick, int CveUser, int UserFiltro,int IdSubmodulo, int pagina, int tamañomuestra)
        {
            JObject jsdat = new JObject();
            try
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                JObject json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                {
                    if (string.IsNullOrEmpty(fecha))
                    {
                        jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + FehTick + "\"},{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"TipoTicket\",\"value\":" + TipoTicket + "},{\"property\":\"TipoFalla\",\"value\": " + TipoFalla + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + UserFiltro + "},{\"property\":\"IdSubmodulo\",\"value\":" + IdSubmodulo + "}]}");
                    }
                    else
                    {
                        jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"TipoTicket\",\"value\":" + TipoTicket + "},{\"property\":\"TipoFalla\",\"value\": " + TipoFalla + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + UserFiltro + "},{\"property\":\"IdSubmodulo\",\"value\":" + IdSubmodulo + "}]}");
                    }
                    json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                    JArray? data = json["data"] as JArray;
                    pagina = (pagina - 1) * tamañomuestra;
                    if (data != null && data.Count > 0)
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        catfal.Solicitudes = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).Solicitudes;
                        catfal.TotalSolicitudes = catfal.Solicitudes.Count;
                        catfal.Solicitudes = catfal.Solicitudes.Skip(pagina).Take(tamañomuestra).ToList();
                        catfal.status = Convert.ToInt32(json["status"]);
                        lista.Add(catfal);
                    }
                    else
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        catfal.Solicitudes = new List<Solicitude>();
                        catfal.status = Convert.ToInt32(json["status"]);
                        catfal.message = json["message"].ToString();
                        lista.Add(catfal);
                    }
                }
                else
                {
                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                    catfal.Solicitudes = new List<Solicitude>();
                    catfal.status = Convert.ToInt32(json["status"]);
                    catfal.message = json["message"].ToString();
                    lista.Add(catfal);
                }
                return lista;
            }
            catch (Exception e)
            {
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                lista.Add(catfal);
                return lista;
            }

        }
        public List<ControlFalla> Modfificador_metod(int CveEstatus, int modCveEstatus, string empresa, string NumTicket, string UseAsigna, int TipoApoyo, int TipoFalla, string FechaHoraEstimadaReparacion, string ComentariosCambioVto, string CveUser, string Fecha, int Tipotikcet,int idsub, int pagina, int tamañomuestra)
        {
            JObject jsdat = new JObject();
            try
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                JObject json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                {
                    jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPINS_SeguimientoReparaciones\"},\"filter\": [{\"property\": \"claveControlReparaciones\",\"value\": " + NumTicket + "},{\"property\":\"ClaveEstatusNvo\",\"value\": " + modCveEstatus + "},{\"property\":\"CveUsuarioAsignado \",\"value\": " + UseAsigna + "},{\"property\": \"ClaveTipoApoyo\",\"value\": " + TipoApoyo + "},{\"property\":\"ClaveTipoClasificacion\",\"value\": " + TipoFalla + "},{\"property\":\"FechaHoraEstimadaReparacion\",\"value\": \""+ FechaHoraEstimadaReparacion + "\"},{\"property\":\"ComentariosCambioVto\", \"value\": \""+ ComentariosCambioVto + "\"},{\"property\":\"CveUsuarioMod\",\"value\": " + CveUser + "}]}");
                    json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                    if (Convert.ToInt32(json["status"]) == 200)
                    {
                        //JObject jasd = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + Fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "}]}");
                        JObject jasd = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + Fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + 0 + "},{\"property\":\"IdSubmodulo\",\"value\":" + idsub + "}]}");
                        JObject jqan = JObject.Parse(hh.HttpWebRequest("POST", url, jasd));
                        JArray? dega = jqan["data"] as JArray;
                        pagina = (pagina - 1) * tamañomuestra;
                        if (dega != null && dega.Count > 0)
                        {
                            ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                            ControlFalla carfg = JsonConvert.DeserializeObject<ControlFalla>(dega[0].ToString());
                            catfal.TotalSolicitudes = carfg.Solicitudes.Count;
                            catfal.Solicitudes = carfg.Solicitudes.Skip(pagina).Take(tamañomuestra).ToList();
                            catfal.status = Convert.ToInt32(json["status"]);
                            catfal.message = json["message"].ToString();
                            lista.Add(catfal);
                        }
                        else
                        {
                            ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                            catfal.Solicitudes = new List<Solicitude>();
                            catfal.status = Convert.ToInt32(jqan["status"]);
                            catfal.message = jqan["message"].ToString();
                            lista.Add(catfal);
                        }
                    }
                }
                return lista;
            }
            catch (Exception e)
            {

                List<ControlFalla> op = new List<ControlFalla>();
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                op.Add(catfal);
                return op;
            }

        }
        
        public List<ControlFalla> PorAsigControlRep(string empresa, string CveUser, string fecha, int pagina, int tamañomuestra)
        {
            JObject jsdat = new JObject();
            try
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                    jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + 1 + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                pagina = (pagina - 1) * tamañomuestra;
                if (data != null && data.Count > 0)
                {
                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                    ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                    catfal.TotalSolicitudes = cfal.Solicitudes.Count;
                    catfal.Solicitudes = cfal.Solicitudes.Skip(pagina).Take(tamañomuestra).ToList();
                    catfal.status = Convert.ToInt32(json["status"]);
                    lista.Add(catfal);
                }
                else
                {
                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                    catfal.Solicitudes = new List<Solicitude>();
                    catfal.status = Convert.ToInt32(json["status"]);
                    catfal.message = json["message"].ToString();
                    lista.Add(catfal);
                }
                return lista;
            }
            catch (Exception e)
            {
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                lista.Add(catfal);
                return lista;
            }

        }

        public List<ControlFalla> FalRegisControlRep(string empresa, string CveUser, string Fecha, string UseAsigna, string ticket)
        {
            JObject jsdat = new JObject();
            try
            {

                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPINS_SeguimientoReparaciones\"},\"filter\": [{\"property\": \"claveControlReparaciones\",\"value\": " + ticket + "},{\"property\":\"ClaveEstatusNvo\",\"value\": " + 2 + "},{\"property\":\"CveUsuarioAsignado \",\"value\": " + UseAsigna + "},{\"property\": \"ClaveTipoApoyo\",\"value\": " + 0 + "},{\"property\":\"ClaveTipoClasificacion\",\"value\": " + 0 + "},{\"property\":\"FechaHoraEstimadaReparacion\",\"value\": null},{\"property\":\"ComentariosCambioVto\", \"value\": \"\"},{\"property\":\"CveUsuarioMod\",\"value\": " + CveUser + "}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                //JArray? status = json["status"] as JArray;
                //JArray? message = json["message"] as JArray;
                if (Convert.ToInt32(json["status"]) == 200)
                {
                    JObject jat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                    var ds = hh.HttpWebRequest("POST", url, jat);
                    JObject jqn = JObject.Parse(ds);
                    JArray? datop = jqn["data"] as JArray;
                    if (datop != null && datop.Count > 0)
                    {
                        JObject jasd = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + 1 + "},{\"property\":\"Fecha\",\"value\": \"" + Fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "}]}");
                        var sa = hh.HttpWebRequest("POST", url, jasd);
                        JObject jqan = JObject.Parse(sa);
                        JArray? dega = jqan["data"] as JArray;
                        if (dega != null && dega.Count > 0)
                        {
                            ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                            ControlFalla carfg = JsonConvert.DeserializeObject<ControlFalla>(dega[0].ToString());
                            catfal.Solicitudes = carfg.Solicitudes;
                            catfal.status = Convert.ToInt32(json["status"]);
                            catfal.message = json["message"].ToString();
                            lista.Add(catfal);
                        }
                        else
                        {
                            ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                            catfal.Solicitudes = new List<Solicitude>();
                            catfal.status = Convert.ToInt32(jqan["status"]);
                            catfal.message = jqan["message"].ToString();
                            lista.Add(catfal);
                        }
                    }
                }
                else
                {
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                    datos = hh.HttpWebRequest("POST", url, jsdat);
                    JObject jn = JObject.Parse(datos);
                    JArray? df = jn["data"] as JArray;
                    if (df != null && df.Count > 0)
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(df[0].ToString());
                        catfal.Solicitudes = new List<Solicitude>();
                        catfal.status = Convert.ToInt32(json["status"]);
                        catfal.message = json["message"].ToString();
                        lista.Add(catfal);
                    }
                }
                return lista;
            }
            catch (Exception e)
            {

                List<ControlFalla> op = new List<ControlFalla>();
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                op.Add(catfal);
                return op;
            }
        }
        public List<ControlFalla> BuscarAsignados(string empresa, string CveUser, string fecha, DateTime FehTick, int NumTicket, string UsAsignadoDiv)
        {
            JObject jsdat = new JObject();
            try
            {

                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                    jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + 2 + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "}]}");
                //jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + 2 + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + 1 + "}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                if (data != null && data.Count > 0)
                {
                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                    ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                    catfal.status = Convert.ToInt32(json["status"]);
                    catfal.Solicitudes = cfal.Solicitudes;
                    lista.Add(catfal);
                }
                else
                {
                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                    catfal.Solicitudes = new List<Solicitude>();
                    catfal.status = Convert.ToInt32(json["status"]);
                    catfal.message = json["message"].ToString();
                    lista.Add(catfal);
                }
                return lista;
            }
            catch (Exception e)
            {

                List<ControlFalla> op = new List<ControlFalla>();
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                op.Add(catfal);
                return op;
            }

        }

        public List<ControlFalla> AsignacionTiempoApoyClasificacion(int NumTicket, string ClaveTipoApoyo, string ClaveTipoClasificacion, string empresa, DateTime Horaestimada, int CveUsuarioMod)
        {
            JObject jsdat = new JObject();
            try
            {

                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPINS_SeguimientoReparaciones\"},\"filter\": [{\"property\": \"claveControlReparaciones \",\"value\":" + NumTicket + "},{\"property\": \"ClaveEstatusNvo\",\"value\": " + 4 + "},{\"property\": \"CveUsuarioAsignado \",\"value\": " + 0 + "},{\"property\": \"ClaveTipoApoyo \",\"value\": " + ClaveTipoApoyo + "},{\"property\": \"ClaveTipoClasificacion\",\"value\": " + ClaveTipoClasificacion + "},{\"property\": \"FechaHoraEstimadaReparacion\",\"value\":\"" + Horaestimada.ToString("yyyy-MM-dd HH:mm:ss") + "\"},{\"property\":\"ComentariosCambioVto\",\"value\": \"\"},{\"property\": \"CveUsuarioMod\",\"value\": " + CveUsuarioMod + "}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                if (Convert.ToInt32(json["status"]) == 200)
                {
                    JObject jst = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + 2 + "},{\"property\":\"Fecha\",\"value\": \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUsuarioMod + "}]}");
                    var dt = hh.HttpWebRequest("POST", url, jst);
                    JObject jkon = JObject.Parse(dt);
                    JArray? daty = jkon["data"] as JArray;
                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                    ControlFalla listfal = JsonConvert.DeserializeObject<ControlFalla>(daty[0].ToString());
                    catfal.status = Convert.ToInt32(json["status"]);
                    catfal.message = json["message"].ToString();
                    catfal.Solicitudes = listfal.Solicitudes;
                    lista.Add(catfal);
                }
                return lista;
            }
            catch (Exception e)
            {

                List<ControlFalla> op = new List<ControlFalla>();
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                op.Add(catfal);
                return op;
            }
        }



        public List<ControlFalla> BuscarRep(string cveEmp, string CveUserlog, DateTime FehTick, int TipTicket, int TipFalla, int NumTicket)
        {
            JObject jsdat = new JObject();
            try
            {

                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\": " + cveEmp + " },{\"property\":\"CveEstatus\",\"value\": 4 },{\"property\":\"Fecha\",\"value\": \"" + FehTick.ToString("yyyy-MM-dd") + "\" },{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"TipoTicket\",\"value\":" + TipTicket + "},{\"property\":\"TipoFalla\",\"value\":" + TipFalla + "},{\"property\":\"CveUser\",\"value\":" + CveUserlog + "}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                if (Convert.ToInt32(json["status"]) == 400)
                {
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + cveEmp + "\"}]}");
                    datos = hh.HttpWebRequest("POST", url, jsdat);
                    json = JObject.Parse(datos);
                    JArray? datop = json["data"] as JArray;
                    if (datop != null && datop.Count > 0)
                        jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + cveEmp + "\"},{\"property\":\"CveEstatus\",\"value\": " + 4 + "},{\"property\":\"Fecha\",\"value\": \"" + FehTick.ToString("yyyy-MM-dd") + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUserlog + "}]}");
                    datos = hh.HttpWebRequest("POST", url, jsdat);
                    json = JObject.Parse(datos);
                    JArray? data = json["data"] as JArray;
                    if (data != null && data.Count > 0)
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                        catfal.Solicitudes = cfal.Solicitudes;
                        catfal.status = 400;
                        catfal.message = "!No se encontraron datos";
                        lista.Add(catfal);
                    }
                    else
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        catfal.Solicitudes = new List<Solicitude>();
                        catfal.status = Convert.ToInt32(json["status"]);
                        catfal.message = json["message"].ToString();
                        lista.Add(catfal);
                    }
                }
                else
                {
                    JArray? datop = json["data"] as JArray;
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + cveEmp + "\"}]}");
                    datos = hh.HttpWebRequest("POST", url, jsdat);
                    json = JObject.Parse(datos);
                    if (Convert.ToInt32(json["status"]) == 200)
                    {
                        JArray? jArray = json["data"] as JArray;
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(jArray[0].ToString());
                        cfal.Solicitudes = catfal.Solicitudes;
                        lista.Add(cfal);
                    }
                }
                return lista;
            }
            catch (Exception e)
            {
                List<ControlFalla> op = new List<ControlFalla>();
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                op.Add(catfal);
                return op;
            }
        }

        public List<ControlFalla> AsigaReparacion(string empresa, string fecha, string CveUser, string ticket, DateTime FechEstima, string ComeMotvAsig)
        {
            JObject jsdat = new JObject();
            try
            {

                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                    jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPINS_SeguimientoReparaciones\"},\"filter\": [{\"property\": \"claveControlReparaciones\",\"value\": " + ticket + "},{\"property\":\"ClaveEstatusNvo\",\"value\": " + 5 + "},{\"property\":\"CveUsuarioAsignado \",\"value\": 0 },{\"property\": \"ClaveTipoApoyo\",\"value\": 0 },{\"property\":\"ClaveTipoClasificacion\",\"value\": 0 },{\"property\":\"FechaHoraEstimadaReparacion\",\"value\": \" " + FechEstima.ToString("yyyy-MM-dd HH:mm:ss") + "\"},{\"property\":\"ComentariosCambioVto\", \"value\": \"" + ComeMotvAsig + "\"},{\"property\":\"CveUsuarioMod\",\"value\": " + CveUser + "}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? data = json["status"] as JArray;
                if (Convert.ToInt32(json["status"]) == 200)
                {
                    JObject jsonre = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + 4 + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "}]}");
                    var re = hh.HttpWebRequest("POST", url, jsonre);
                    JObject Jsona = JObject.Parse(re);
                    JArray? array = Jsona["data"] as JArray;
                    if (array != null && array.Count > 0)
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(array[0].ToString());
                        catfal.status = Convert.ToInt32(json["status"]);
                        catfal.message = json["message"].ToString();
                        catfal.Solicitudes = cfal.Solicitudes;
                        lista.Add(catfal);
                    }
                    else
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        catfal.status = Convert.ToInt32(json["status"]);
                        catfal.message = json["message"].ToString();
                        catfal.Solicitudes = new List<Solicitude>();
                        lista.Add(catfal);
                    }
                }
                else
                {
                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                    catfal.status = Convert.ToInt32(json["status"]);
                    catfal.message = json["message"].ToString();
                    catfal.Solicitudes = new List<Solicitude>();
                    lista.Add(catfal);
                }
                return lista;
            }
            catch (Exception e)
            {
                List<ControlFalla> op = new List<ControlFalla>();
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                op.Add(catfal);
                return op;
            }
        }
        public List<ControlFalla> BuscarAsignados(string cveEmp, string UsAsignado, string NumTicket, DateTime FehTick)
        {
            JObject jsdat = new JObject();
            try
            {

                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\": " + cveEmp + " },{\"property\":\"CveEstatus\",\"value\": 2 },{\"property\":\"Fecha\",\"value\": \"" + FehTick.ToString("yyyy-MM-dd") + "\" },{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"TipoTicket\",\"value\": 0 },{\"property\":\"TipoFalla\",\"value\": 0 },{\"property\":\"CveUser\",\"value\":" + UsAsignado + "}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                //JArray? datop = json["data"] as JArray;
                //if (datop != null && datop.Count > 0)
                if (Convert.ToInt32(json["status"]) == 400)
                {
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + cveEmp + "\"}]}");
                    datos = hh.HttpWebRequest("POST", url, jsdat);
                    json = JObject.Parse(datos);
                    JArray? datop = json["data"] as JArray;
                    if (datop != null && datop.Count > 0)
                        jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + cveEmp + "\"},{\"property\":\"CveEstatus\",\"value\": " + 2 + "},{\"property\":\"Fecha\",\"value\": \"" + FehTick.ToString("yyyy-MM-dd") + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + UsAsignado + "}]}");
                    datos = hh.HttpWebRequest("POST", url, jsdat);
                    json = JObject.Parse(datos);
                    JArray? data = json["data"] as JArray;
                    if (data != null && data.Count > 0)
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                        catfal.Solicitudes = cfal.Solicitudes;
                        catfal.status = 400;
                        catfal.message = "!No se encontraron datos";
                        lista.Add(catfal);
                    }
                    else
                    {
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        catfal.Solicitudes = new List<Solicitude>();
                        catfal.status = Convert.ToInt32(json["status"]);
                        catfal.message = json["message"].ToString();
                        lista.Add(catfal);
                    }
                }
                else
                {
                    JArray? datop = json["data"] as JArray;
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + cveEmp + "\"}]}");
                    datos = hh.HttpWebRequest("POST", url, jsdat);
                    json = JObject.Parse(datos);
                    if (Convert.ToInt32(json["status"]) == 200)
                    {
                        JArray? jArray = json["data"] as JArray;
                        ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
                        ControlFalla cfal = JsonConvert.DeserializeObject<ControlFalla>(jArray[0].ToString());
                        cfal.Solicitudes = catfal.Solicitudes;
                        lista.Add(cfal);
                    }
                }
                return lista;
            }
            catch (Exception e)
            {

                List<ControlFalla> op = new List<ControlFalla>();
                ControlFalla catfal = new ControlFalla();
                catfal.status = 400;
                catfal.message = e.Message.ToString();
                op.Add(catfal);
                return op;
            }
        }
        public List<ControlRep> VistaControl(JObject jsonContl)
        {
            List<ControlRep> controls = new List<ControlRep>();
            try
            {
                var datos = hh.HttpWebRequest("POST", url, jsonContl);
                JObject js = JObject.Parse(datos);
                if (js["status"].ToString() == "400")
                {
                    List<ControlRep> controls1 = new List<ControlRep>();
                    return controls1;
                }
                JArray dai = js["data"] as JArray;
                ControlRep cont = JsonConvert.DeserializeObject<ControlRep>(dai[0].ToString());
                controls.Add(cont);

                return controls;
            }
            catch (Exception e)
            {
                List<ControlRep> controls1 = new List<ControlRep>();
                return controls1;
            }
        }

    }
}
