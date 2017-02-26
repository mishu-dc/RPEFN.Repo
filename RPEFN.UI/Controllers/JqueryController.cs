using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RPEFN.UI.Models;

namespace RPEFN.UI.Controllers
{
    [Authorize]
    public class JqueryController : Controller
    {
        // GET: Jquery
        public async Task<ViewResult> Index()
        {
            var userName = ConfigurationManager.AppSettings["WebApiUserName"];
            var password = ConfigurationManager.AppSettings["WebApiPassword"];
            var url = ConfigurationManager.AppSettings["WebApiURL"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });

                var response = await client.PostAsync("/token", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonToken = await response.Content.ReadAsStringAsync();
                    WebApiToken token = WebApiToken.Parse(jsonToken);

                    return View(token);
                }
            }

            return View(new WebApiToken());
        }
    }
}