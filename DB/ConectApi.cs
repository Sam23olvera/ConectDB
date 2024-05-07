using ConectDB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConectDB.DB
{
    public class ConectApi
    {
        DataApi hh = new DataApi();
        private string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
        Model_Buscar model = new Model_Buscar();
        List<ViajesSep> lista = new List<ViajesSep>();
        List<Rutas> rutas = new List<Rutas>();
        List<ModelFallas> listaModFal = new List<ModelFallas>();
        List<ItineViajeSPM> viajes = new List<ItineViajeSPM>();

        public Model_Buscar ListasJuntas(DateTime fecha, int cvruta)
        {
            try
            {
                JObject jsdat = JObject.Parse("{\"data\":{\"bdCc\":4,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_ViajesSPM_Monitoreo\"},\"filter\":[{\"property\": \"Fecha\",\"value\":\"" + fecha.ToString("yyyy-MM-dd") + "\"},{\"property\": \"CveRuta\",\"value\":" + cvruta.ToString() + " }]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;

                if (data != null && data.Count > 0)
                {
                    ViajesSep viajes = JsonConvert.DeserializeObject<ViajesSep>(data[0].ToString());
                    viajes.Date = fecha;
                    viajes.cvruta = cvruta;
                    lista.Add(viajes);
                }

                model.Vias = lista;

                jsdat = JObject.Parse("{\"data\":{\"bdCc\" : 4, \"bdSch\" : \"dbo\", \"bdSp\" :\"SPQRY_CatRutasSPM\" }, \"filter\":{ }}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                data = json["data"] as JArray;

                if (data != null && data.Count > 0)
                {
                    JObject dataObject = data[0] as JObject;
                    JArray catRutaArray = dataObject["CatRuta"] as JArray;

                    if (catRutaArray != null)
                    {
                        foreach (var item in catRutaArray)
                        {
                            CatRutum catRuta = JsonConvert.DeserializeObject<CatRutum>(item.ToString());
                            Rutas ruta = new Rutas { CatRuta = new List<CatRutum> { catRuta } };
                            rutas.Add(ruta);
                        }
                    }
                }

                model.Rutas = rutas;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrió una excepción: " + e.Message);
            }

            return model;
        }

        public ModelFallas ObjetoModelOperadores_Rem(string empresa)
        {
            try
            {
                JObject jsdat = new JObject();
                ModelFallas zcioFallas = new ModelFallas();
                JObject json;
                JArray? data = null;
                listaModFal.Clear();

                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatIniMantenimiento\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                json = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                data = json["data"] as JArray;
                zcioFallas = JsonConvert.DeserializeObject<ModelFallas>(data[3].ToString());
                zcioFallas.TBCAT_Unidades = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString()).TBCAT_Unidades;
                zcioFallas.TBCAT_Remolques = JsonConvert.DeserializeObject<ModelFallas>(data[1].ToString()).TBCAT_Remolques;
                zcioFallas.TBCAT_Operador = JsonConvert.DeserializeObject<ModelFallas>(data[2].ToString()).TBCAT_Operador;
                return zcioFallas;
            }
            catch (Exception e)
            {
                ModelFallas f = new ModelFallas();
                return f;
            }
        }
        public List<Rutas> ListarRutas()
        {
            List<Rutas> rutas = new List<Rutas>();
            try
            {
                JObject jsdat = JObject.Parse("{\"data\":{\"bdCc\" : 4, \"bdSch\" : \"dbo\", \"bdSp\" :\"SPQRY_CatRutasSPM\" }, \"filter\":{ }}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;

                if (data != null && data.Count > 0)
                {
                    JObject? dataObject = data[0] as JObject;
                    JArray? catRutaArray = dataObject["CatRuta"] as JArray;

                    if (catRutaArray != null)
                    {
                        foreach (var item in catRutaArray)
                        {
                            CatRutum catRuta = JsonConvert.DeserializeObject<CatRutum>(item.ToString());
                            Rutas ruta = new Rutas { CatRuta = new List<CatRutum> { catRuta } };
                            rutas.Add(ruta);
                        }
                    }
                }
                return rutas;
            }
            catch (Exception e)
            {
                List<Rutas> rutasa = new List<Rutas>();
                Console.WriteLine(e);
                return rutasa;
            }
        }
        public List<ItineViajeSPM> ListaViajeDetalle(int CV, string NR, DateTime FeSel, int cvruta)
        {
            try
            {
                JObject jsdat = JObject.Parse("{\"data\":{\"bdCc\":4,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_ItinerarioSPM_Monitoreo\"},\"filter\":[{\"property\":\"ClaveViaje\",\"value\" :" + CV.ToString() + "}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                ItineViajeSPM itineViajeSPM = JsonConvert.DeserializeObject<ItineViajeSPM>(data[0].ToString());
                viajes.Add(itineViajeSPM);
                viajes[0].FeSel = FeSel;
                viajes[0].NR = NR;
                viajes[0].cvruta = cvruta;
            }
            catch (Exception e)
            {

            }
            return viajes;
        }
        
        public bool Guardar(string json)
        {
            try
            {
                JObject jsonfinal = JObject.Parse("{\"data\":{\"bdCc\":4,\"bdSch\":\"dbo\",\"bdSp\":\"SPUPD_LLegadasSalidasSPM_Monitoreo\"},\"filter\":[{\"property\": \"Json1\",\"value\" :\"" + json + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsonfinal);
                JObject js = JObject.Parse(datos);
                if (js["status"].ToString() == "400")
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
