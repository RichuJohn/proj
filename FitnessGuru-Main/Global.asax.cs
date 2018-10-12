using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using FitnessGuru_Main.App_Start;

namespace FitnessGuru_Main
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            //    .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //GlobalConfiguration.Configuration.Formatters
            //    .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }
    }
}
