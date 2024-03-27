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

namespace ConectDB.DB
{
    public class ConectApi
    {
        DataApi hh = new DataApi();

        public Model_Buscar ListasJuntas(DateTime fecha, int cvruta)
        {
            Model_Buscar model = new Model_Buscar();
            List<ViajesSep> lista = new List<ViajesSep>();
            List<Rutas> rutas = new List<Rutas>();

            try
            {
                JObject jsdat = JObject.Parse("{\"data\":{\"bdCc\":4,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_ViajesSPM_Monitoreo\"},\"filter\":[{\"property\": \"Fecha\",\"value\":\"" + fecha.ToString("yyyy-MM-dd") + "\"},{\"property\": \"CveRuta\",\"value\":" + cvruta.ToString() + " }]}");
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
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
                url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
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
                // Manejar la excepción de acuerdo a la lógica de tu aplicación
                Console.WriteLine("Ocurrió una excepción: " + e.Message);
                // Aquí podrías registrar el error en algún log o realizar alguna otra acción necesaria
            }

            return model;
        }
        
        
        public List<ModelFallas> ListaOperadores_Rem(string empresa)
        {
            JObject jsdat = new JObject();
            try
            {
                List<ModelFallas> lista = new List<ModelFallas>();
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosOperadores\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                if (data != null && data.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosUnidades\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? datu = json["data"] as JArray;
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatRemolque\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":" + empresa + "}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? dai = json["data"] as JArray;
                if (dai != null && dai.Count > 0)
                {
                    ModelFallas mfal = JsonConvert.DeserializeObject<ModelFallas>(datop[0].ToString());
                    ModelFallas mau = JsonConvert.DeserializeObject<ModelFallas>(datu[0].ToString());
                    ModelFallas mu = JsonConvert.DeserializeObject<ModelFallas>(dai[0].ToString());
                    ModelFallas modelFallas = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString());

                    mau = JsonConvert.DeserializeObject<ModelFallas>(datu[0].ToString());
                    modelFallas = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString());
                    mfal.TBCAT_SeleccionEquipo = modelFallas.TBCAT_SeleccionEquipo;
                    mfal.TBCAT_TipoClasificacion = modelFallas.TBCAT_TipoClasificacion;
                    mfal.TBCAT_TipoTicket = modelFallas.TBCAT_TipoTicket;
                    mfal.TBCAT_TipoApoyo = modelFallas.TBCAT_TipoApoyo;
                    mfal.TBCAT_TipoCarga = modelFallas.TBCAT_TipoCarga;
                    mfal.TBCAT_TipoFalla = modelFallas.TBCAT_TipoFalla;
                    mfal.TBCAT_Unidades = mau.TBCAT_Unidades;
                    mfal.TBCAT_Remolques = mu.TBCAT_Remolques;
                    mfal.TBCAT_TipoEquipo = modelFallas.TBCAT_TipoEquipo;
                    mfal.TBCAT_Ruta = modelFallas.TBCAT_Ruta;
                    lista.Add(mfal);
                }
                return lista;
            }
            catch (Exception e)
            {
                List<ModelFallas> op = new List<ModelFallas>();
                return op;
            }
        }
        public List<ModelFallas> ListaOperadores(string empresa)
        {
            JObject jsdat = new JObject();
            try
            {
                List<ModelFallas> lista = new List<ModelFallas>();
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosOperadores\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                if (data != null && data.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosUnidades\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? datu = json["data"] as JArray;
                if (datu != null && datu.Count > 0)
                {
                    ModelFallas mfal = JsonConvert.DeserializeObject<ModelFallas>(datop[0].ToString());
                    ModelFallas mau = JsonConvert.DeserializeObject<ModelFallas>(datu[0].ToString());
                    ModelFallas modelFallas = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString());

                    mau = JsonConvert.DeserializeObject<ModelFallas>(datu[0].ToString());
                    modelFallas = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString());
                    mfal.TBCAT_SeleccionEquipo = modelFallas.TBCAT_SeleccionEquipo;
                    mfal.TBCAT_TipoApoyo = modelFallas.TBCAT_TipoApoyo;
                    mfal.TBCAT_TipoCarga = modelFallas.TBCAT_TipoCarga;
                    mfal.TBCAT_TipoFalla = modelFallas.TBCAT_TipoFalla;
                    mfal.TBCAT_Unidades = mau.TBCAT_Unidades;
                    mfal.TBCAT_TipoEquipo = modelFallas.TBCAT_TipoEquipo;
                    mfal.TBCAT_Ruta = modelFallas.TBCAT_Ruta;
                    lista.Add(mfal);
                }
                return lista;
            }
            catch (Exception e)
            {
                List<ModelFallas> op = new List<ModelFallas>();
                return op;
            }
        }
        public List<ModelFallas> ListaOperadoresCom(string empresa, string unidad, string alias, string ClaveOperacion)
        {
            if (Evaluar(unidad))
            {
            }
            if (Evaluar(alias))
            {

            }
            if (Evaluar(ClaveOperacion))
            {
            }
            else
            {
                ListaOperadores(empresa);
            }
            try
            {
                JObject jsdat = new JObject();
                JObject json = new JObject();
                List<ModelFallas> lista = new List<ModelFallas>();
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosOperadores\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? datop = json["data"] as JArray;
                if (datop != null && datop.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosMantto\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                if (data != null && data.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatalogosUnidades\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? datu = json["data"] as JArray;
                if (datu != null && datu.Count > 0)
                    jsdat = JObject.Parse("{\"data\":{\"bdCc\" : 5, \"bdSch\" : \"dbo\", \"bdSp\" :\"SPQRY_UltimaPosicion\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"},{\"property\":\"Unidad\",\"value\": \"" + unidad + "\"},{\"property\":\"Alias\",\"value\": \"" + alias + "\"},{\"property\":\"ClaveOperacion\",\"value\": " + ClaveOperacion + "}]}");
                datos = hh.HttpWebRequest("POST", url, jsdat);
                json = JObject.Parse(datos);
                JArray? dot = json["data"] as JArray;
                if (dot != null && dot.Count > 0)
                {
                    ModelFallas mfal = JsonConvert.DeserializeObject<ModelFallas>(datop[0].ToString());
                    ModelFallas mau = JsonConvert.DeserializeObject<ModelFallas>(datu[0].ToString());
                    ModelFallas mu = JsonConvert.DeserializeObject<ModelFallas>(dot[0].ToString());
                    ModelFallas modelFallas = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString());
                    mau = JsonConvert.DeserializeObject<ModelFallas>(datu[0].ToString());
                    mu = JsonConvert.DeserializeObject<ModelFallas>(dot[0].ToString());
                    modelFallas = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString());
                    mfal.TBCAT_SeleccionEquipo = modelFallas.TBCAT_SeleccionEquipo;
                    mfal.TBCAT_TipoApoyo = modelFallas.TBCAT_TipoApoyo;
                    mfal.TBCAT_TipoCarga = modelFallas.TBCAT_TipoCarga;
                    mfal.TBCAT_TipoFalla = modelFallas.TBCAT_TipoFalla;
                    mfal.TBCAT_Unidades = mau.TBCAT_Unidades;
                    mfal.TBCAT_TipoEquipo = modelFallas.TBCAT_TipoEquipo;
                    mfal.TBCAT_Ruta = modelFallas.TBCAT_Ruta;
                    mfal.UltimaPosicion = mu.UltimaPosicion;
                    lista.Add(mfal);
                }
                return lista;
            }
            catch (Exception e)
            {
                List<ModelFallas> op = new List<ModelFallas>();
                return op;
            }
        }
        public bool Evaluar(string dato)
        {
            if (string.IsNullOrEmpty(dato))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Rutas> ListarRutas()
        {
            Respuesta resp;
            List<Rutas> rutas = new List<Rutas>();
            try
            {
                JObject jsdat = JObject.Parse("{\"data\":{\"bdCc\" : 4, \"bdSch\" : \"dbo\", \"bdSp\" :\"SPQRY_CatRutasSPM\" }, \"filter\":{ }}");
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
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
        public List<ViajesSep> listaViajes(DateTime fecha, int cvruta)
        {
            List<ViajesSep> lista = new List<ViajesSep>();
            try
            {
                JObject jsdat = JObject.Parse("{\"data\":{\"bdCc\":4,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_ViajesSPM_Monitoreo\"},\"filter\":[{\"property\": \"Fecha\",\"value\":\"" + fecha.ToString("yyyy-MM-dd") + "\"},{\"property\": \"CveRuta\",\"value\":" + cvruta.ToString() + " }]}");
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
                var datos = hh.HttpWebRequest("POST", url, jsdat);
                JObject json = JObject.Parse(datos);
                JArray? data = json["data"] as JArray;
                if (data != null && data.Count > 0)
                {
                    //JObject dataObject = data[0] as JObject;
                    ViajesSep Viajes = JsonConvert.DeserializeObject<ViajesSep>(data[0].ToString());
                    lista.Add(Viajes);
                    lista[0].Date = fecha;
                    lista[0].cvruta = cvruta;
                }
                return lista;
            }
            catch (Exception e)
            {
                lista = null;
            }
            return lista;
        }
        public List<ItineViajeSPM> ListaViajeDetalle(int CV, string NR, DateTime FeSel, int cvruta)
        {
            List<ItineViajeSPM> viajes = new List<ItineViajeSPM>();
            try
            {
                JObject jsdat = JObject.Parse("{\"data\":{\"bdCc\":4,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_ItinerarioSPM_Monitoreo\"},\"filter\":[{\"property\":\"ClaveViaje\",\"value\" :" + CV.ToString() + "}]}");
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
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
        public JObject GuardarFallas(string json)
        {
            try
            {
                JObject jsonfinal = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPINS_ControlReparaciones\"},\"filter\":[{\"property\": \"Json1\",\"value\":\"" + json + "\"}]}");
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
                var datos = hh.HttpWebRequest("POST", url, jsonfinal);
                JObject js = JObject.Parse(datos);
                if (js["status"].ToString() == "400")
                {
                    return js;
                }
                return js;

            }
            catch (Exception e)
            {
                JObject js = JObject.Parse("{ \"status\": \"Desconosido\",\"message\":\"" + e.Message.ToString() + "\"}");
                return js;
            }
        }
        public bool Guardar(string json)
        {
            try
            {
                JObject jsonfinal = JObject.Parse("{\"data\":{\"bdCc\":4,\"bdSch\":\"dbo\",\"bdSp\":\"SPUPD_LLegadasSalidasSPM_Monitoreo\"},\"filter\":[{\"property\": \"Json1\",\"value\" :\"" + json + "\"}]}");
                string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
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
