using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ConectDB.DB
{
    public class DataApi
    {
        public string HttpWebRequest(string method, string url, JObject datos)
        {
            string r = "";
            WebRequest request = WebRequest.Create(url);
            request.Method = method;
            if (method == "POST")
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        //client.Headers.Add("Authorization", toq);
                        client.Headers.Add("Content-Type", "text/json");
                        r = client.UploadString(url, datos.ToString());
                        //r = response;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex; Passsing exception up the line.
                    r = "";
                }
            }
            return r;
        }
        public string HttpWebRequestTokenLog(string method, string url, string datos, string token) 
        {
            string r = "";
            WebRequest request = WebRequest.Create(url);
            request.Method = method;
            if (method == "POST")
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("Authorization", token);
                        client.Headers.Add("Content-Type", "text/json");
                        r = client.UploadString(url, datos);
                        //r = response;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex; Passsing exception up the line.
                    r = "";
                }
            }
            return r;
        }
        
        public string HttpWebRequestToken(string method, string url, JObject datos, string token)
        {
            string r = "";
            WebRequest request = WebRequest.Create(url);
            request.Method = method;
            if (method == "POST")
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("Authorization", token);
                        client.Headers.Add("Content-Type", "text/json");
                        r = client.UploadString(url, datos.ToString());
                        //r = response;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex; Passsing exception up the line.
                    r = "";
                }
            }
            return r;
        }

    }
}