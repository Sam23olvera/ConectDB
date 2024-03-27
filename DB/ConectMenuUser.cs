using ConectDB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ConectDB.Models.LogUser;
using System.Security.Policy;

namespace ConectDB.DB
{
    public class ConectMenuUser
    {
        DataApi data = new DataApi();
        UsuarioModel model = new UsuarioModel();
        public UsuarioModel RegresMenu(string desusuario, string descontraseña, int cveEmp, string url,string XT)
        {
            JObject jsdatos = JObject.Parse("{\"data\": {\"bdCc\": 1,\"bdSch\": \"dbo\",\"bdSp\": \"SPQRY_EmpUser\"},\"filter\": {\"usr\": \"" + desusuario + "\",\"pwd\": \"" + descontraseña + "\",\"idempresa\":" + cveEmp + "} }");
            string datos = data.HttpWebRequestToken("POST", url, jsdatos, XT);
            if (datos == null)
            {
                model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
                model.Message = "¡No se Encontro ningun valor!";
                model.Status = 400;
                return model;
            }
            else
            {
                return model = JsonConvert.DeserializeObject<UsuarioModel>(datos);
            }
        }
    }
}
