using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VentaMusical.Constantes;
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
                            Imagen = x.Imagen,
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

        [HttpPost]
        public ActionResult Guardar(string Descripcion, string Imagen)
        {
            try
            {

                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    TB_GenerosMusicales genero = new TB_GenerosMusicales()
                    {
                        Descripcion = Descripcion,
                        Imagen = Imagen,
                    };

                    db.TB_GenerosMusicales.Add(genero);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new RespuestaModel { Codigo = HttpStatusCode.InternalServerError, Mensaje = Mensajes.Error, Resultado = false });
            }

            return Json(new RespuestaModel { Codigo = HttpStatusCode.InternalServerError, Mensaje = Mensajes.Exito, Resultado = true });
        }

        [HttpPost]
        public ActionResult Eliminar(int codigoGenero)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var genero = db.TB_GenerosMusicales.Find(codigoGenero);
                    if (genero == null)
                    {
                        return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.NoEncontrado, Resultado = false });
                    }

                    db.TB_GenerosMusicales.Remove(genero);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new RespuestaModel { Codigo = HttpStatusCode.InternalServerError, Mensaje = Mensajes.Error, Resultado = false });
            }

            return Json(new RespuestaModel { Codigo = HttpStatusCode.OK, Mensaje = Mensajes.Exito, Resultado = true });
        }

        [HttpGet]
        public ActionResult ObtenerGenero(int codigoGenero)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var genero = db.TB_GenerosMusicales.Find(codigoGenero);
                    if (genero == null)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }

                    var result = new
                    {
                        CodigoGenero = genero.CodigoGenero,
                        Descripcion = genero.Descripcion,
                        Imagen = genero.Imagen
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }         
        }
    }
}
