using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels;

namespace VentaMusical.Controllers
{
    public class CancionesController : Controller
    {
        // GET: Canciones
        public ActionResult Index()
        {
            var listaCanciones = new List<CancionesViewModel>();
            var listaGenerosMusicales = new List<GenerosMusicalesViewModel>();
            var model = new VistaCancionesViewModel();

            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var canciones = db.TB_Canciones.ToList();

                    if (canciones.Any())
                    {
                        listaCanciones = canciones.Select(x => new CancionesViewModel
                        {
                            CodigoCancion = x.CodigoCancion,
                            CodigoGenero = x.CodigoGenero,
                            Nombre = x.Nombre,
                            Precio = x.Precio,
                            Imagen = x.Imagen,

                        }).ToList();
                    }

                    var generosMusicales = db.TB_GenerosMusicales.ToList();

                    if (generosMusicales.Any())
                    {
                        listaGenerosMusicales = generosMusicales.Select(x => new GenerosMusicalesViewModel
                        {
                            CodigoGenero = x.CodigoGenero,
                            Descripcion = x.Descripcion,
                            Imagen = x.Imagen,
                        }).ToList();
                    }

                    model = new VistaCancionesViewModel { Canciones = listaCanciones, GenerosMusicales = listaGenerosMusicales };
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

            return View(model: model);
        }
    }
}