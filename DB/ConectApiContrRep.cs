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

        private ControlFalla CarCata(JObject jsdat, int CveEstatus)
        {
            json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
            data = json["data"] as JArray;
            if (data != null && data.Count > 0)
            {
                if (CveEstatus == 1)
                {
                    //TBCAT_TipoFalla
                    controlFalla.TBCAT_TipoFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_TipoFalla;
                    //TBCAT_UserAsignaReparacion
                    controlFalla.TBCAT_UserAsignaReparacion = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_UserAsignaReparacion;
                }
                else if (CveEstatus == 2)
                {
                    //TBCAT_UserAsignaReparacion
                    controlFalla.TBCAT_UserAsignaReparacion = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_UserAsignaReparacion;
                    //TBCAT_TipoClasificacion
                    controlFalla.TBCAT_TipoClasificacion = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_TipoClasificacion;
                    //TBCAT_TipoApoyo
                    controlFalla.TBCAT_TipoApoyo = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_TipoApoyo;
                }
                else if (CveEstatus == 4)
                {
                    //TBCAT_TipoTicket
                    controlFalla.TBCAT_TipoTicket = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_TipoTicket;
                    //TBCAT_TipoFalla
                    controlFalla.TBCAT_TipoFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_TipoFalla;
                }
                else if (CveEstatus == 5)
                {
                    //TBCAT_TipoTicket
                    controlFalla.TBCAT_TipoTicket = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_TipoTicket;
                    //TBCAT_TipoFalla
                    controlFalla.TBCAT_TipoFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString()).TBCAT_TipoFalla;
                }
                //controlFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
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

        public ControlFalla PrimerCarga(int CveEstatus, string empresa, string? FehTick, int NumTicket, int TipoTicket, int TipoFalla, int CveUser, int UserFiltro, int IdSubmodulo, int pagina, int tamañomuestra)
        {
            try
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                if (CarCata(jsdat, CveEstatus).status == 200)
                {
                    if (string.IsNullOrEmpty(FehTick))
                    {
                        FehTick = null;
                    }
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
                    if (CarCata(jsdat, CveEstatus).status == 200)
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
        public ControlFalla ConsultaGeneral(string cveEmp, int NumTicket, string FechaIni, string FechaFin, int pagina, int tamañomuestra) 
        {
            try 
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\": 5,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_Reparaciones\"},\"filter\": [{\"property\": \"cveEmpresa\",\"value\":\"" + cveEmp + "\"},{\"property\":\"NumTicket\",\"value\":" + NumTicket + "},{\"property\":\"FechaIni\",\"value\": \"" + FechaIni + "\"},{\"property\":\"FechaFin\",\"value\": \"" + FechaFin + "\"}]}");
                json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                data = json["data"] as JArray;
                pagina = (pagina - 1) * tamañomuestra;
                if (data != null && data.Count > 0)
                {
                    controlFalla = JsonConvert.DeserializeObject<ControlFalla>(data[0].ToString());
                    controlFalla.TotalSolicitudes = controlFalla.Solicitudes.Count;
                    controlFalla.Solicitudes = controlFalla.Solicitudes.Skip(pagina).Take(tamañomuestra).ToList();
                    controlFalla.status = Convert.ToInt32(json["status"]);

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
    }
}
