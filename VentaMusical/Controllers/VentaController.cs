using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels;

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
            var listaFormasDePago = new List<FormasDePagoViewModel>();

            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {                 
                    var usuarios = db.TB_Usuarios.ToList();
                    var canciones = db.TB_Canciones.ToList();
                    var generosMusicales = db.TB_GenerosMusicales.ToList();
                    var formasDePago = db.TB_FormasDePago.ToList();

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

                    if (formasDePago.Any())
                    {
                        listaFormasDePago = formasDePago.Select(x => new FormasDePagoViewModel
                        {
                            IdFormaPago = x.IdFormaPago,
                            Descripcion = x.Descripcion,
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
            model.FormasDePago = listaFormasDePago;

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

        [HttpPost]
        public ActionResult InsertarFactura(VentaEncabezadoViewModel encabezado)
        {

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}