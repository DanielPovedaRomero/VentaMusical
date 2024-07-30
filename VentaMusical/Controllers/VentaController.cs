using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using VentaMusical.Constantes;
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
        public ActionResult Factura(int? id = 0)
        {
            var model = new VistaFacturaViewModel();
            var listaUsuarios = new List<UsuarioViewModel>();
            var listaCanciones = new List<CancionesViewModel>();
            var listaGenerosMusicales = new List<GenerosMusicalesViewModel>();
            var listaFormasDePago = new List<FormasDePagoViewModel>();
            var listaImpuestos = new List<ImpuestosViewModel>();

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
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

            model.Usuarios = listaUsuarios;
            model.Canciones = listaCanciones;
            model.FormasDePago = listaFormasDePago;
            model.Impuestos = listaImpuestos;

            if (id != 0)
            {
                var resultado = DevolverEntidadaFactura(id, listaUsuarios);
                model.Encabezado = resultado.Item1;
                model.Lineas = resultado.Item2;
            }



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
        public ActionResult InsertarFactura(VentaEncabezadoViewModel encabezado, List<VentaLineaViewModel> lineas)
        {
            try
            {
                var resultadoValidaciones = ValidarFactura(encabezado, lineas);

                if (!String.IsNullOrEmpty(resultadoValidaciones))
                {
                    return Json(new RespuestaModel { Codigo = HttpStatusCode.OK, Mensaje = resultadoValidaciones, Resultado = false });
                }
                else
                {

                    using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                    {

                        if (encabezado.NumeroFactura == 0)
                        {
                            TB_VentaEncabezado encabezadoDB = new TB_VentaEncabezado()
                            {
                                Fecha = encabezado.Fecha,
                                Subtotal = encabezado.Subtotal,
                                Total = encabezado.Total,
                                NumeroIdentificacion = encabezado.NumeroIdentificacion,
                                IdFormaPago = encabezado.IdFormaPago
                            };

                            db.TB_VentaEncabezado.Add(encabezadoDB);
                            db.SaveChanges();

                            int numeroFactura = encabezadoDB.NumeroFactura;
                            int numeroLinea = 1;

                            foreach (var linea in lineas)
                            {
                                TB_VentaLinea lineaDB = new TB_VentaLinea()
                                {
                                    CodigoCancion = linea.CodigoCancion,
                                    Cantidad = linea.Cantidad,
                                    Precio = linea.Precio,
                                    IdImpuesto = linea.IdImpuesto,
                                    Subtotal = linea.SubTotal,
                                    Total = linea.Total,
                                    Linea = numeroLinea,
                                    NumeroFactura = numeroFactura
                                };

                                db.TB_VentaLinea.Add(lineaDB);

                                numeroLinea++;
                            }

                            db.SaveChanges();

                        }
                        else
                        {
                            var lineasExistentes = db.TB_VentaLinea.Where(l => l.NumeroFactura == encabezado.NumeroFactura).ToList();
                            foreach (var lineaExistente in lineasExistentes)
                            {
                                db.TB_VentaLinea.Remove(lineaExistente);
                            }
                            db.SaveChanges();

                            var encabezadoDB = db.TB_VentaEncabezado.FirstOrDefault(e => e.NumeroFactura == encabezado.NumeroFactura);
                            if (encabezadoDB != null)
                            {
                                encabezadoDB.Fecha = encabezado.Fecha;
                                encabezadoDB.Subtotal = encabezado.Subtotal;
                                encabezadoDB.Total = encabezado.Total;
                                encabezadoDB.NumeroIdentificacion = encabezado.NumeroIdentificacion;
                                encabezadoDB.IdFormaPago = encabezado.IdFormaPago;

                                db.SaveChanges();
                            }


                            int numeroFactura = encabezadoDB.NumeroFactura;
                            int numeroLinea = 1;


                            foreach (var linea in lineas)
                            {
                                TB_VentaLinea lineaDB = new TB_VentaLinea()
                                {
                                    CodigoCancion = linea.CodigoCancion,
                                    Cantidad = linea.Cantidad,
                                    Precio = linea.Precio,
                                    IdImpuesto = linea.IdImpuesto,
                                    Subtotal = linea.SubTotal,
                                    Total = linea.Total,
                                    Linea = numeroLinea,
                                    NumeroFactura = numeroFactura
                                };

                                db.TB_VentaLinea.Add(lineaDB);

                                numeroLinea++;
                            }

                            db.SaveChanges();
                        }

                    }

                    return Json(new RespuestaModel { Codigo = HttpStatusCode.OK, Mensaje = Mensajes.Exito, Resultado = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new RespuestaModel { Codigo = HttpStatusCode.InternalServerError, Mensaje = Mensajes.Error, Resultado = false });
            }
        }

        public Tuple<VentaEncabezadoViewModel, List<VentaLineaViewModel>> DevolverEntidadaFactura(int? id, List<UsuarioViewModel> usuarios)
        {
            if (!id.HasValue || id.Value == 0)
            {
                return new Tuple<VentaEncabezadoViewModel, List<VentaLineaViewModel>>(null, null);
            }

            using (var db = new VentaMusicalDBEntities())
            {
                // Obtén el encabezado de la factura
                var encabezado = db.TB_VentaEncabezado
                    .Where(f => f.NumeroFactura == id.Value)
                    .Select(f => new VentaEncabezadoViewModel
                    {
                        NumeroFactura = f.NumeroFactura,
                        Fecha = f.Fecha,
                        NumeroIdentificacion = f.NumeroIdentificacion,
                        IdFormaPago = f.IdFormaPago,
                        Subtotal = f.Subtotal,
                        Total = f.Total,
                    })
                    .FirstOrDefault();

                if (encabezado == null)
                {
                    return new Tuple<VentaEncabezadoViewModel, List<VentaLineaViewModel>>(null, null);
                }

                encabezado.NombreUsuario = !(usuarios.Where(y => y.NumeroIdentificacion == encabezado.NumeroIdentificacion).Any()) ? "No asociado" : usuarios.Where(y => y.NumeroIdentificacion == encabezado.NumeroIdentificacion).FirstOrDefault().Nombre;

                // Obtén las líneas de la factura
                var lineas = db.TB_VentaLinea
                    .Where(l => l.NumeroFactura == id.Value)
                    .Select(l => new VentaLineaViewModel
                    {
                        Linea = l.Linea,
                        NumeroFactura = l.NumeroFactura,
                        CodigoCancion = l.CodigoCancion,
                        Cantidad = l.Cantidad,
                        Precio = l.Precio,
                        Total = l.Total,
                        SubTotal = l.Subtotal,
                        IdImpuesto = l.IdImpuesto
                    })
                    .ToList();

                return new Tuple<VentaEncabezadoViewModel, List<VentaLineaViewModel>>(encabezado, lineas);
            }
        }

        private string ValidarFactura(VentaEncabezadoViewModel encabezado, List<VentaLineaViewModel> lineas)
        {

            StringBuilder errores = new StringBuilder();

            if (Convert.ToInt32(encabezado.NumeroIdentificacion) <= 0)
                errores.AppendLine("Por favor ingrese un usuario para la venta");

            if (encabezado.IdFormaPago <= 0)
                errores.AppendLine("Por favor seleccione una forma de pago");

            if (lineas is null || lineas.Count() <= 0)
                errores.AppendLine("Por favor ingrese lineas en la venta");

            return errores.ToString();

        }
    }
}