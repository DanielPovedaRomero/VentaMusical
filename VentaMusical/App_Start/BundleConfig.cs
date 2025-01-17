﻿using System.Web.Optimization;

namespace VentaMusical
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Js creados en el proyecto

            bundles.Add(new ScriptBundle("~/bundles/globales").Include(
                "~/Js/Proyecto/globales.js"));

            bundles.Add(new ScriptBundle("~/bundles/generosMusicales").Include(
                "~/Js/Proyecto/generosMusicales.js"));

            bundles.Add(new ScriptBundle("~/bundles/canciones").Include(
               "~/Js/Proyecto/canciones.js"));

            bundles.Add(new ScriptBundle("~/bundles/ventas").Include(
              "~/Js/Proyecto/ventas.js"));

            bundles.Add(new ScriptBundle("~/bundles/factura").Include(
            "~/Js/Proyecto/factura.js"));

        }
    }
}
