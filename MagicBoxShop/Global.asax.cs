using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MagicBoxShop.Models;
using System.Data.Entity;
using WebMatrix.WebData;


namespace MagicBoxShop
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        
        protected void Application_Start()
        {
            Database.SetInitializer(new MagicBoxShop.Models.SampleData());

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PresentsEntities>());
            //var context = new PresentsEntities();
            //context.Database.Initialize(true);
           // Database.SetInitializer<PresentsEntities>(new DropCreateDatabaseIfModelChanges<PresentsEntities>());
           // WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

       }
}