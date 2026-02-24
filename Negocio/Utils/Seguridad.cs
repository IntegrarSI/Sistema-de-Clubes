using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Net;

namespace Negocio.Utils
{
    public class Seguridad
    {
        public static bool validarJsonSession(HttpSessionStateBase session, string[] keys)
        {
            if (session != null)
            {
                foreach (var key in keys)
                {
                    if (session[key] == null || session[key].ToString().Length == 0)
                        return false;
                }
            }
            else
                return false;

            return true;
        }

        public static bool validarRecaptcha(string response)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["recaptcha_url"].ToString());

            if (ConfigurationManager.AppSettings["REST_proxy"] != null && ConfigurationManager.AppSettings["REST_proxy"].ToString() == "true")
            {
                client.Proxy = new WebProxy(ConfigurationManager.AppSettings["REST_proxy_url"].ToString());

                if (ConfigurationManager.AppSettings["REST_proxy_usuario"] != null && ConfigurationManager.AppSettings["REST_proxy_usuario"].ToString().Length > 0
                    && ConfigurationManager.AppSettings["REST_proxy_password"] != null && ConfigurationManager.AppSettings["REST_proxy_password"].ToString().Length > 0)
                {
                    client.Proxy.Credentials = new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["REST_proxy_usuario"].ToString()
                        , ConfigurationManager.AppSettings["REST_proxy_password"].ToString()
                    );
                }
            }

            //configuración del request
            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;

            //request.Parameters.Add(new Parameter(
            request.AddParameter("secret", ConfigurationManager.AppSettings["recaptcha_secret_key"].ToString());
            request.AddParameter("response", response);

            var respuestaREST = client.Execute(request) as RestResponse;

            if (respuestaREST.Content != null && respuestaREST.Content.Length > 0)
            {
                var json = JsonConvert.DeserializeObject(respuestaREST.Content, typeof(Negocio.RecaptchaResponse));
                return ((Negocio.RecaptchaResponse)json).success;
            }
            else
                return false;
        }
    }
}
