using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Constantes;
using VentaMusical.Models.ViewModels;
using VentaMusical.Models.ViewModels.UsuarioModels;
using VentaMusical.Models;

namespace VentaMusical.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [HttpGet]
        public ActionResult Index()
        {

            var listaUsuarios = new List<ListarUsuario>();

            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var Usuarios = db.TB_Usuarios.ToList();

                    if (Usuarios.Any())
                    {
                        listaUsuarios = Usuarios.Select(x => new ListarUsuario
                        {
                            NumeroIdentificacion = x.NumeroIdentificacion,
                            Nombre = x.Nombre,
                            IdGenero = x.IdGenero,
                            Correo = x.Correo,
                            IdTarjeta = x.IdTarjeta,
                            NumeroTarjeta = x.NumeroTarjeta,
                            Contrasena = x.Contrasena,
                            IdPerfil = x.IdPerfil
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

            return View(model: listaUsuarios);
        }

        [HttpPost]
        public ActionResult Guardar(string NumeroIdentificacion,string Nombre, int IdGenero, string Correo, int IdTarjeta, string NumeroTarjeta, string Contrasena, int IdPerfil)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    if (NumeroIdentificacion == "")
                    {
                        var Usuarios = db.TB_Usuarios.ToList();
                        var validarUsuario = Usuarios.Where(x => x.NumeroIdentificacion.ToUpper() == NumeroIdentificacion.ToUpper()).FirstOrDefault();

                        if (validarUsuario != null)
                        {
                            return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.UsuariosConMismaCedula, Resultado = false });
                        }

                        TB_Usuarios nuevoUsuario = new TB_Usuarios()
                        {
                            NumeroIdentificacion = NumeroIdentificacion,
                            Nombre = Nombre,
                            IdGenero = IdGenero,
                            Correo = Correo,
                            IdTarjeta = IdTarjeta,
                            NumeroTarjeta = NumeroTarjeta,
                            Contrasena  = Contrasena,
                            IdPerfil = IdPerfil
                        };

                        db.TB_Usuarios.Add(nuevoUsuario);
                    }
                    else
                    {

                        TB_Usuarios UsuarioExistente = db.TB_Usuarios.Find(NumeroIdentificacion);
                        if (UsuarioExistente != null)
                        {
                            UsuarioExistente.NumeroIdentificacion = NumeroIdentificacion;
                            UsuarioExistente.Nombre = Nombre;
                            UsuarioExistente.IdGenero = IdGenero;
                            UsuarioExistente.Correo = Correo;
                            UsuarioExistente.IdTarjeta = IdTarjeta;
                            UsuarioExistente.NumeroTarjeta= NumeroTarjeta;
                            UsuarioExistente.Contrasena = Contrasena;
                            UsuarioExistente.IdPerfil = IdPerfil;
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

        [HttpPost]
        public ActionResult Editar(string NumeroIdentificacion, string Nombre, int IdGenero, string Correo, int IdTarjeta, string NumeroTarjeta, string Contrasena, int IdPerfil)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    if (NumeroIdentificacion == "")
                    {
                        var Usuarios = db.TB_Usuarios.ToList();
                        var validarUsuario = Usuarios.Where(x => x.NumeroIdentificacion.ToUpper() == NumeroIdentificacion.ToUpper()).FirstOrDefault();

                        if (validarUsuario != null)
                        {
                            return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.UsuariosConMismaCedula, Resultado = false });
                        }

                        TB_Usuarios nuevoUsuario = new TB_Usuarios()
                        {
                            NumeroIdentificacion = NumeroIdentificacion,
                            Nombre = Nombre,
                            IdGenero = IdGenero,
                            Correo = Correo,
                            IdTarjeta = IdTarjeta,
                            NumeroTarjeta = NumeroTarjeta,
                            Contrasena = Contrasena,
                            IdPerfil = IdPerfil
                        };

                        db.TB_Usuarios.Add(nuevoUsuario);
                    }
                    else
                    {

                        TB_Usuarios UsuarioExistente = db.TB_Usuarios.Find(NumeroIdentificacion);
                        if (UsuarioExistente != null)
                        {
                            UsuarioExistente.NumeroIdentificacion = NumeroIdentificacion;
                            UsuarioExistente.Nombre = Nombre;
                            UsuarioExistente.IdGenero = IdGenero;
                            UsuarioExistente.Correo = Correo;
                            UsuarioExistente.IdTarjeta = IdTarjeta;
                            UsuarioExistente.NumeroTarjeta = NumeroTarjeta;
                            UsuarioExistente.Contrasena = Contrasena;
                            UsuarioExistente.IdPerfil = IdPerfil;
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

        [HttpPost]
        public ActionResult Eliminar(string NumeroIdentificacion)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {

                    var UsuariosResgistrados = db.TB_Usuarios.Where(x => x.NumeroIdentificacion == NumeroIdentificacion).FirstOrDefault();

                    if (UsuariosResgistrados != null)
                    {
                        var Usuario = db.TB_Usuarios.Find(NumeroIdentificacion);

                        if (Usuario == null)
                        {
                            return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.NoEncontrado, Resultado = false });
                        }

                        db.TB_Usuarios.Remove(Usuario);
                        db.SaveChanges();
                    }
                    else 
                    {
                        return Json(new RespuestaModel { Codigo = HttpStatusCode.NotFound, Mensaje = Mensajes.ErrorEliminarUsuario, Resultado = false });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new RespuestaModel { Codigo = HttpStatusCode.InternalServerError, Mensaje = Mensajes.Error, Resultado = false });
            }

            return Json(new RespuestaModel { Codigo = HttpStatusCode.OK, Mensaje = Mensajes.Eliminar_Exito, Resultado = true });
        }

        [HttpGet]
        public ActionResult ObtenerUsuario(string NumeroIdentificacion)
        {
            try
            {
                using (VentaMusicalDBEntities db = new VentaMusicalDBEntities())
                {
                    var Usuario = db.TB_Usuarios.Find(NumeroIdentificacion);
                    if (Usuario == null)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }

                    var result = new
                    {
                       NumeroIdentificacion = Usuario.NumeroIdentificacion,
                       Nombre = Usuario.Nombre,
                       IdGenero = Usuario.IdGenero,
                       Correo = Usuario.Correo,
                       IdTarjeta = Usuario.IdTarjeta,
                       NumeroTarjeta = Usuario.NumeroTarjeta,
                       Contraseña = Usuario.Contrasena,
                       IdPerfil = Usuario.IdPerfil
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