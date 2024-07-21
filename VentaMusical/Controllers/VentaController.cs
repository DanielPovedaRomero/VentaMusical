using System.Collections.Generic;
using System;
using System.Web.Mvc;
using VentaMusical.Models.ViewModels;
using VentaMusical.Models;
using System.Linq;

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
                return Json(ex);
            }

            return View(model: listaVentas);
        }

        // GET: Factura
        public ActionResult Factura()
        {
           return View();
        }
    }
}