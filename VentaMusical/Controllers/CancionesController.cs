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
                    var generosMusicales = db.TB_GenerosMusicales.ToList();

                    if (canciones.Any())
                    {
                        listaCanciones = canciones.Select(x => new CancionesViewModel
                        {
                            CodigoCancion = x.CodigoCancion,
                            CodigoGenero = x.CodigoGenero,
                            Nombre = x.Nombre,
                            Precio = x.Precio,
                            Imagen = x.Imagen,
                            NombreGenero = !(generosMusicales.Where(y => y.CodigoGenero == x.CodigoGenero).Any()) ? "No asociado" : generosMusicales.Where(y => y.CodigoGenero == x.CodigoGenero).FirstOrDefault().Descripcion

                        }).ToList();
                    }



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
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

            return View(model: model);
        }

        [HttpPost]
        public ActionResult Eliminar(int codigoCancion)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var cancion = db.TB_Canciones.Find(codigoCancion);
                    if (cancion == null)
                    {
                        return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.NoEncontrado, Resultado = false });
                    }

                    db.TB_Canciones.Remove(cancion);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new RespuestaModel { Codigo = HttpStatusCode.InternalServerError, Mensaje = Mensajes.Error, Resultado = false });
            }

            return Json(new RespuestaModel { Codigo = HttpStatusCode.OK, Mensaje = Mensajes.Eliminar_Exito, Resultado = true });
        }

        [HttpPost]
        public ActionResult Guardar(int CodigoCancion, int CodigoGenero, string Nombre, decimal Precio, string Imagen)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {

                    if (CodigoCancion == 0)
                    {

                        var canciones = db.TB_Canciones.ToList();

                        var validarCancion = canciones.Where(x => x.Nombre.ToUpper() == Nombre.ToUpper() && x.CodigoGenero == CodigoGenero).FirstOrDefault();

                        if (validarCancion != null)
                        {
                            return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.CancionConMismaDescripcion, Resultado = false });
                        }

                        TB_Canciones nuevaCancion = new TB_Canciones()
                        {
                            CodigoCancion = CodigoCancion,
                            CodigoGenero = CodigoGenero,
                            Precio = Precio,
                            Nombre = Nombre,
                            Imagen = Imagen
                        };

                        db.TB_Canciones.Add(nuevaCancion);
                    }
                    else
                    {

                        TB_Canciones cancionExistente = db.TB_Canciones.Find(CodigoCancion);
                        if (cancionExistente != null)
                        {
                            cancionExistente.Nombre = Nombre;
                            cancionExistente.Precio = Precio;
                            cancionExistente.CodigoGenero = CodigoGenero;
                            cancionExistente.Imagen = Imagen;
                        }
                        else
                        {
                            return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.NoEncontrado, Resultado = false });
                        }
                    }

                    db.SaveChanges();
                }

                return Json(new RespuestaModel { Codigo = HttpStatusCode.OK, Mensaje = Mensajes.Exito, Resultado = true });
            }
            catch (Exception ex)
            {
                return Json(new RespuestaModel { Codigo = HttpStatusCode.InternalServerError, Mensaje = Mensajes.Error, Resultado = false });
            }
        }

        [HttpGet]
        public ActionResult ObtenerCancion(int codigoCancion)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var cancion = db.TB_Canciones.Find(codigoCancion);
                    if (cancion == null)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }

                    var result = new
                    {
                        CodigoCancion = cancion.CodigoCancion,
                        CodigoGenero = cancion.CodigoGenero,
                        Nombre = cancion.Nombre,
                        Precio = cancion.Precio,
                        Imagen = cancion.Imagen,
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