using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebPFI
{
    public class RouteConfig
    {
        public static string DefaultAction()
        {
            return "/Shared/_Layout.cshtml";
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Students", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
