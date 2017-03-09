using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPEFN.Data.Entities;
using RPEFN.Data.Infrastructure;
using RPEFN.WebService.Infrastructure.Implementations;

namespace RPEFN.UnitTests.Controllers
{
    
    public class BaseTest
    {
        public UnitOfWork UnitOfWork { get; set; }

        public ApiController Controller { get; set; }
        
        public BaseTest()
        {
            ApplicationDbContext context = new ApplicationDbContext(Effort.DbConnectionFactory.CreateTransient());
            UnitOfWork = new UnitOfWork(context);

            InitializeDrugRepository();
        }

        public void MockControllerProperties(ApiController controller, string name)
        {
            Controller = controller;

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost/api/{name}");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", name } });

            Controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            Controller.Request = request;
            Controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            Controller.User = new GenericPrincipal(new GenericIdentity("Bob", "Passport"), new[] { "managers" });
        }

        private void InitializeDrugRepository()
        {
            UnitOfWork.Drugs.Add(new Drug() { Id = 1 , BrandName = "Pegasys", GenericName = "peginterferon alfa-2a", NdcId = "000040350", Price = 250, Strength = "500 Mg" });
            UnitOfWork.Drugs.Add(new Drug() { Id = 2, BrandName = "Lipitor", GenericName = "atorvastatin", NdcId = "8596040350", Price = 25, Strength = "50 Mg" });
            UnitOfWork.Drugs.Add(new Drug() { Id = 3, BrandName = "Azit", GenericName = "azithromaicin", NdcId = "0096040350", Price = 50, Strength = "500 Mg" });
            UnitOfWork.Complete();
        }
    }
}
