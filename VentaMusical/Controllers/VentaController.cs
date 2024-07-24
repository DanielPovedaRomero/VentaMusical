using System.Collections.Generic;
using System;
using System.Web.Mvc;
using VentaMusical.Models.ViewModels;
using VentaMusical.Models;
using System.Linq;
using Microsoft.Ajax.Utilities;

namespace VentaMusical.Controllers
{
    public class VentaController : Controller
    {
        // GET: Venta
        public ActionResult Index()
        {
            var listaVentas = new List<VentaEncabezadoViewModel>();
            var listaUsuarios = new List<VentaEncabezadoViewModel>();

            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var ventas = db.TB_VentaEncabezado.ToList();
                    var usuarios = db.TB_Usuarios.ToList(); 

                    if (ventas.Any())
                    {
                        listaVentas = ventas.Select(x => new VentaEncabezadoViewModel
                        {
                            NumeroFactura = x.NumeroFactura,
                            NumeroIdentificacion = x.NumeroIdentificacion,
                            Total = x.Total,
                            Fecha = x.Fecha,
                            NombreUsuario = !(usuarios.Where(y => y.NumeroIdentificacion == x.NumeroIdentificacion).Any()) ? "No asociado" : usuarios.Where(y => y.NumeroIdentificacion == x.NumeroIdentificacion).FirstOrDefault().Nombre

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

            return View(model: listaVentas);
        }

        // GET: Factura
        public ActionResult Factura()
        {
            var model = new VistaFacturaViewModel();
            var listaUsuarios = new List<UsuarioViewModel>();
            var listaCanciones = new List<CancionesViewModel>();
            var listaGenerosMusicales = new List<GenerosMusicalesViewModel>();

            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {                 
                    var usuarios = db.TB_Usuarios.ToList();
                    var canciones = db.TB_Canciones.ToList();
                    var generosMusicales = db.TB_GenerosMusicales.ToList(); 

                    if (usuarios.Any())
                    {
                        listaUsuarios = usuarios.Select(x => new UsuarioViewModel
                        {
                            Nombre = x.Nombre,
                            NumeroIdentificacion = x.NumeroIdentificacion,                          
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

                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

            model.Usuarios = listaUsuarios;
            model.Canciones = listaCanciones;

            return View(model: model);
        }

        [HttpGet]
        public ActionResult ObtenerImpuestos()
        {
            var listaImpuestos = new List<ImpuestosViewModel>();
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var impuestos = db.TB_Impuestos.ToList();

                    if (impuestos.Any())
                    {
                        listaImpuestos = impuestos.Select(x => new ImpuestosViewModel
                        {
                            IdImpuesto = x.IdImpuesto,
                            Descripcion = x.Descripcion,
                            Porcentaje = x.Porcentaje,
                        }).ToList();
                    }

                    return Json(listaImpuestos, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(listaImpuestos, JsonRequestBehavior.AllowGet);
            }
        }
    }
}