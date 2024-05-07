using ConectDB.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Policy;

namespace ConectDB.DB
{
    public class ConectRegistraFalla
    {
        private string url = "https://webportal.tum.com.mx/wsstmdv/api/execsp";
        DataApi hh = new DataApi();
        ModelFallas zcioFallas = new ModelFallas();
        JObject jsdat = new JObject();
        JArray? data = null;
        JObject js = new JObject();
        public ModelFallas ObjetoModelOperadores_Rem(string empresa)
        {
            try
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPQRY_CatIniMantenimiento\"},\"filter\":[{\"property\": \"ClaveEmpresa\",\"value\":\"" + empresa + "\"}]}");
                js = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                data = js["data"] as JArray;
                zcioFallas = JsonConvert.DeserializeObject<ModelFallas>(data[3].ToString());
                zcioFallas.TBCAT_Unidades = JsonConvert.DeserializeObject<ModelFallas>(data[0].ToString()).TBCAT_Unidades;
                zcioFallas.TBCAT_Remolques = JsonConvert.DeserializeObject<ModelFallas>(data[1].ToString()).TBCAT_Remolques;
                zcioFallas.TBCAT_Operador = JsonConvert.DeserializeObject<ModelFallas>(data[2].ToString()).TBCAT_Operador;
                return zcioFallas;
            }
            catch (Exception e)
            {
                return zcioFallas;
            }
        }

        public JObject GuardarFallas(string json)
        {
            try
            {
                jsdat = JObject.Parse("{\"data\":{\"bdCc\":5,\"bdSch\":\"dbo\",\"bdSp\":\"SPINS_ControlReparaciones\"},\"filter\":[{\"property\": \"Json1\",\"value\":\"" + json + "\"}]}");
                js = JObject.Parse(hh.HttpWebRequest("POST", url, jsdat));
                if (js["status"].ToString() == "400")
                {
                    return js;
                }
                return js;

            }
            catch (Exception e)
            {
                return JObject.Parse("{ \"status\": \"Desconosido\",\"message\":\"" + e.Message.ToString() + "\"}"); 
            }
        }
    }
}
