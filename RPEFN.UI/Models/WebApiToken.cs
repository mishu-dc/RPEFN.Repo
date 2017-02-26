using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace RPEFN.UI.Models
{
    public class WebApiToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string UserName { get; set; }

        public static WebApiToken Parse(string json)
        {
            WebApiToken token = new WebApiToken();
            JObject jObject = JObject.Parse(json);
            token.AccessToken = (string)jObject["access_token"];
            token.TokenType = (string)jObject["token_type"];
            token.UserName = (string)jObject["userName"];
            token.ExpiresIn = (int) jObject["expires_in"];

            return token;
        }
    }
}