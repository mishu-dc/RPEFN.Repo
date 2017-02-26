using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RPEFN.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var setting = config.Formatters.JsonFormatter.SerializerSettings;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            setting.Formatting = Formatting.Indented;

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //CORS
            EnableCrossSiteRequests(config);
        }

        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(origins: "*",
                            headers: "*",
                            methods: "*");
            config.EnableCors(cors);
        }
    }
}
