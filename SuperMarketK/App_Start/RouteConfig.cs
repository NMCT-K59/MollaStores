using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SuperMarketK
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
              new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Add cart",
                url: "them-vao-gio",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Events",
                url: "blog",
                defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Detail News",
                url: "blog/{metatitle}--{id}",
                defaults: new { controller = "Content", action = "DetailNew", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Check Order",
                url: "tracking-order",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Search Product",
                url: "search-product",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "About Us",
                url: "about-us",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Contact Us",
                url: "contact",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Success",
                url: "success",
                defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Category Product",
                url: "category/{metatitle}",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Detail Product",
                url: "products/{metatitle}/{id}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );
        }
    }
}
