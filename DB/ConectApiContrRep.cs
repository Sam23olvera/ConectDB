using ConectDB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConectDB.DB
{
    public class ConectApiContrRep
    {
        private readonly string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
        private readonly DataApi hh = new DataApi();
        ControlFalla controlFalla = new ControlFalla();
        JObject jsdat = new JObject();
        JObject json = new JObject();
        JArray? data = new JArray();
        public ControlFalla PrimerCarga_sin_catlog(int CveEstatus, string empresa, string CveUser, string fecha, int UserFiltro, int IdSubmodulo)
        {
            try
            {
                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + UserFiltro + "},{\"property\":\"IdSubmodulo\",\"value\":" + IdSubmodulo + "}]}");
                json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                data = json["data"] as JArray;
                if (data != null && data.Count > 0)
                {
                    controlFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                }
                return controlFalla;
            }
            catch (Exception e)
            {
                controlFalla.status = 400;
                controlFalla.message = e.Message.ToString();
                return controlFalla;
            }
        }

        private ControlFalla CarCata(JObject jsdat)
        {
            json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
            data = json["data"] as JArray;
            if (data != null && data.Count > 0)
            {
                controlFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                controlFalla.status = Convert.ToInt32(json["status"]);
                controlFalla.message = json["message"].ToString();
            }
            else
            {
                controlFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                controlFalla.status = Convert.ToInt32(json["status"]);
                controlFalla.message = json["message"].ToString();
            }
            return controlFalla;
        }

        public ControlFalla PrimerCarga(int CveEstatus, string empresa, string FehTick, int NumTicket, int TipoTicket, int TipoFalla, int CveUser, int UserFiltro, int IdSubmodulo, int pagina, int tamañomuestra)
        {
            try
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                if (CarCata(jsdat).status == 200)
                {
                    jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + FehTick + "\"},{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"TipoTicket\",\"value\":" + TipoTicket + "},{\"property\":\"TipoFalla\",\"value\": " + TipoFalla + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + UserFiltro + "},{\"property\":\"IdSubmodulo\",\"value\":" + IdSubmodulo + "}]}");
                    json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                    data = json["data"] as JArray;
                    pagina = (pagina - 1) * tamañomuestra;
                    if (data != null && data.Count > 0)
                    {
                        controlFalla.Solicitudes = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).Solicitudes;
                        controlFalla.TotalSolicitudes = controlFalla.Solicitudes.Count;
                        controlFalla.Solicitudes = controlFalla.Solicitudes.Skip(pagina).Take(tamañomuestra).ToList();
                        controlFalla.status = Convert.ToInt32(json["status"]);
                    }
                    else
                    {
                        controlFalla.Solicitudes = new List<Solicitude>();
                        controlFalla.status = Convert.ToInt32(json["status"]);
                        controlFalla.message = json["message"].ToString();
                    }
                }
                else
                {
                    controlFalla.Solicitudes = new List<Solicitude>();
                }
                return controlFalla;
            }
            catch (Exception e)
            {
                controlFalla.status = 400;
                controlFalla.message = e.Message.ToString();
                return controlFalla;
            }
        }
        //public List<ControlFalla> Primer_crga_con_Catalogos(int CveEstatus, string empresa, string fecha, int NumTicket, int TipoTicket, int TipoFalla, string FehTick, int CveUser, int UserFiltro, int IdSubmodulo, int pagina, int tamañomuestra)
        //{
        //    List<ControlFalla> lista = new List<ControlFalla>();
        //    try
        //    {
        //        jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
        //        JObject json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
        //        JArray? datop = json["data"] as JArray;
        //        if (datop != null && datop.Count > 0)
        //        {
        //            if (string.IsNullOrEmpty(fecha))
        //            {
        //                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + FehTick + "\"},{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"TipoTicket\",\"value\":" + TipoTicket + "},{\"property\":\"TipoFalla\",\"value\": " + TipoFalla + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + UserFiltro + "},{\"property\":\"IdSubmodulo\",\"value\":" + IdSubmodulo + "}]}");
        //            }
        //            else
        //            {
        //                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"TipoTicket\",\"value\":" + TipoTicket + "},{\"property\":\"TipoFalla\",\"value\": " + TipoFalla + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + UserFiltro + "},{\"property\":\"IdSubmodulo\",\"value\":" + IdSubmodulo + "}]}");
        //            }
        //            json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
        //            JArray? data = json["data"] as JArray;
        //            pagina = (pagina - 1) * tamañomuestra;
        //            if (data != null && data.Count > 0)
        //            {
        //                ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
        //                catfal.Solicitudes = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).Solicitudes;
        //                catfal.TotalSolicitudes = catfal.Solicitudes.Count;
        //                catfal.Solicitudes = catfal.Solicitudes.Skip(pagina).Take(tamañomuestra).ToList();
        //                catfal.status = Convert.ToInt32(json["status"]);
        //                lista.Add(catfal);
        //            }
        //            else
        //            {
        //                ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
        //                catfal.Solicitudes = new List<Solicitude>();
        //                catfal.status = Convert.ToInt32(json["status"]);
        //                catfal.message = json["message"].ToString();
        //                lista.Add(catfal);
        //            }
        //        }
        //        else
        //        {
        //            ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
        //            catfal.Solicitudes = new List<Solicitude>();
        //            catfal.status = Convert.ToInt32(json["status"]);
        //            catfal.message = json["message"].ToString();
        //            lista.Add(catfal);
        //        }
        //        return lista;
        //    }
        //    catch (Exception e)
        //    {
        //        ControlFalla catfal = new ControlFalla();
        //        catfal.status = 400;
        //        catfal.message = e.Message.ToString();
        //        lista.Add(catfal);
        //        return lista;
        //    }

        //}
        public ControlFalla ModificadorFall(int CveEstatus, int modCveEstatus, string empresa, string NumTicket, string UseAsigna, int TipoApoyo, int TipoFalla, string FechaHoraEstimadaReparacion, string ComentariosCambioVto, int CveUser, string Fecha, int Tipotikcet, int idsub, int pagina, int tamañomuestra, int Diesel, int Grua, string Dot, string Marca, string Medida, int Posis)
        {
            try
            {
                //jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPMTP_SeguimientoReparaciones\"},\"filter\": [{\"property\": \"claveControlReparaciones\",\"value\": " + NumTicket + "},{\"property\":\"ClaveEstatusNvo\",\"value\": " + modCveEstatus + "},{\"property\":\"CveUsuarioAsignado \",\"value\": " + UseAsigna + "},{\"property\": \"ClaveTipoApoyo\",\"value\": " + TipoApoyo + "},{\"property\":\"ClaveTipoClasificacion\",\"value\": " + TipoFalla + "},{\"property\":\"FechaHoraEstimadaReparacion\",\"value\": \"" + FechaHoraEstimadaReparacion + "\"},{\"property\":\"ComentariosCambioVto\", \"value\": \"" + ComentariosCambioVto + "\"},{\"property\":\"CveUsuarioMod\",\"value\": " + CveUser + "},{\"property\": \"Diesel\",\"value\": " + Diesel + " },{\"property\": \"Grua\",\"value\": " + Grua + "}]}");
                jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPMTP_SeguimientoReparaciones\"},\"filter\": [{\"property\": \"claveControlReparaciones\",\"value\": " + NumTicket + "},{\"property\":\"ClaveEstatusNvo\",\"value\": " + modCveEstatus + "},{\"property\":\"CveUsuarioAsignado \",\"value\": " + UseAsigna + "},{\"property\": \"ClaveTipoApoyo\",\"value\": " + TipoApoyo + "},{\"property\":\"ClaveTipoClasificacion\",\"value\": " + TipoFalla + "},{\"property\":\"FechaHoraEstimadaReparacion\",\"value\": \"" + FechaHoraEstimadaReparacion + "\"},{\"property\":\"ComentariosCambioVto\", \"value\": \"" + ComentariosCambioVto + "\"},{\"property\":\"CveUsuarioMod\",\"value\": " + CveUser + "},{\"property\": \"Diesel\",\"value\": " + Diesel + " },{\"property\": \"Grua\",\"value\": " + Grua + "},{\"property\": \"DOT\",\"value\": \"" + Dot + "\"},{\"property\": \"MARCA\",\"value\": \"" + Marca + "\"},{\"property\": \"MEDIDA\",\"value\": \"" + Medida + "\"},{\"property\": \"POSICION\",\"value\": " + Posis + "}]}");
                json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                if (Convert.ToInt32(json["status"]) == 200)
                {
                    string Mensaje = json["message"].ToString();
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                    if (CarCata(jsdat).status == 200)
                    {
                        controlFalla = PrimerCarga(CveEstatus, empresa, Fecha, 0, 0, 0, CveUser, 0, idsub, pagina, tamañomuestra);
                        controlFalla.status = Convert.ToInt32(json["status"]);
                        controlFalla.message = Mensaje;
                    }
                    else
                    {
                        controlFalla.Solicitudes = new List<Solicitude>();
                        controlFalla.status = Convert.ToInt32(json["status"]);
                        controlFalla.message = json["message"].ToString();
                    }
                }
                else
                {
                    controlFalla = PrimerCarga(CveEstatus, empresa, Fecha, 0, 0, 0, CveUser, 0, idsub, pagina, tamañomuestra);
                }
                return controlFalla;
            }
            catch (Exception e)
            {
                controlFalla.status = 400;
                controlFalla.message = e.Message.ToString();
                return controlFalla;
            }
        }
        //public List<ControlFalla> Modfificador_metod(int CveEstatus, int modCveEstatus, string empresa, string NumTicket, string UseAsigna, int TipoApoyo, int TipoFalla, string FechaHoraEstimadaReparacion, string ComentariosCambioVto, string CveUser, string Fecha, int Tipotikcet, int idsub, int pagina, int tamañomuestra)
        //{
        //    List<ControlFalla> lista = new List<ControlFalla>();
        //    try
        //    {
        //        jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
        //        json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
        //        JArray? datop = json["data"] as JArray;
        //        if (datop != null && datop.Count > 0)
        //        {
        //            jsdat = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPMTP_SeguimientoReparaciones\"},\"filter\": [{\"property\": \"claveControlReparaciones\",\"value\": " + NumTicket + "},{\"property\":\"ClaveEstatusNvo\",\"value\": " + modCveEstatus + "},{\"property\":\"CveUsuarioAsignado \",\"value\": " + UseAsigna + "},{\"property\": \"ClaveTipoApoyo\",\"value\": " + TipoApoyo + "},{\"property\":\"ClaveTipoClasificacion\",\"value\": " + TipoFalla + "},{\"property\":\"FechaHoraEstimadaReparacion\",\"value\": \"" + FechaHoraEstimadaReparacion + "\"},{\"property\":\"ComentariosCambioVto\", \"value\": \"" + ComentariosCambioVto + "\"},{\"property\":\"CveUsuarioMod\",\"value\": " + CveUser + "}]}");
        //            json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
        //            if (Convert.ToInt32(json["status"]) == 200)
        //            {
        //                //JObject jasd = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + Fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "}]}");
        //                JObject jasd = JObject.Parse("{\"data\": {\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_ControlReparaciones\"},\"filter\": [{ \"property\": \"cveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"CveEstatus\",\"value\": " + CveEstatus + "},{\"property\":\"Fecha\",\"value\": \"" + Fecha + "\"},{\"property\":\"NumTicket\",\"value\":" + 0 + "},{\"property\":\"TipoTicket\",\"value\":" + 0 + "},{\"property\":\"TipoFalla\",\"value\": " + 0 + "},{\"property\":\"CveUser\",\"value\":" + CveUser + "},{\"property\":\"UserFiltro\",\"value\":" + 0 + "},{\"property\":\"IdSubmodulo\",\"value\":" + idsub + "}]}");
        //                JObject jqan = JObject.Parse(hh.HttpWebRequest("POST", url, jasd));
        //                JArray? dega = jqan["data"] as JArray;
        //                pagina = (pagina - 1) * tamañomuestra;
        //                if (dega != null && dega.Count > 0)
        //                {
        //                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
        //                    ControlFalla carfg = JsonConvert.DeserializeObject<ControlFalla>(dega[0].ToString());
        //                    catfal.TotalSolicitudes = carfg.Solicitudes.Count;
        //                    catfal.Solicitudes = carfg.Solicitudes.Skip(pagina).Take(tamañomuestra).ToList();
        //                    catfal.status = Convert.ToInt32(json["status"]);
        //                    catfal.message = json["message"].ToString();
        //                    lista.Add(catfal);
        //                }
        //                else
        //                {
        //                    ControlFalla catfal = JsonConvert.DeserializeObject<ControlFalla>(datop[0].ToString());
        //                    catfal.Solicitudes = new List<Solicitude>();
        //                    catfal.status = Convert.ToInt32(jqan["status"]);
        //                    catfal.message = jqan["message"].ToString();
        //                    lista.Add(catfal);
        //                }
        //            }
        //        }
        //        return lista;
        //    }
        //    catch (Exception e)
        //    {

        //        List<ControlFalla> op = new List<ControlFalla>();
        //        ControlFalla catfal = new ControlFalla();
        //        catfal.status = 400;
        //        catfal.message = e.Message.ToString();
        //        op.Add(catfal);
        //        return op;
        //    }

        //}
    }
}
