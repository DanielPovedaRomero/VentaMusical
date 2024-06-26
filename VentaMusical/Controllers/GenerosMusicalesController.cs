using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels;

namespace VentaMusical.Controllers
{
    public class GenerosMusicalesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {

            var listaGenerosMusicales = new List<GenerosMusicalesViewModel>();

            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var generosMusicales = db.TB_GenerosMusicales.ToList();

                    if (generosMusicales.Any())
                    {
                        listaGenerosMusicales = generosMusicales.Select(x => new GenerosMusicalesViewModel
                        {
                            CodigoGenero = x.CodigoGenero,
                            Descripcion = x.Descripcion,
                            Imagen = String.IsNullOrEmpty(x.Imagen) ? Url.Content("~/Content/Images/DefaultAlbum.png") : x.Imagen,
                        }).ToList();                      
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

            return View(model: listaGenerosMusicales);
        }
    }
}