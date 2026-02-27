using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CABW.Controllers
{
    public class JugadoresController : Controller
    {

        public ActionResult obtenerListado(string filtro, string filtroFederacion, string filtroAsociacion, string filtroClub, int pageIndex = 1)
        {
            Negocio.FiltroJugadores filtroJugadores = new Negocio.FiltroJugadores();
            filtroJugadores.filtro = filtro;
            filtroJugadores.asociacion = filtroAsociacion;
            filtroJugadores.federacion = filtroFederacion;
            filtroJugadores.club = filtroClub;

            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.JugadoresBL.obtenerListado(filtro, filtroFederacion, filtroAsociacion, filtroClub, idUsuario, pageIndex);

                filtroJugadores.jugadores = lista;
                var json = JsonConvert.SerializeObject(filtroJugadores,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }

            else
                return unauthorized();
            
            }
          [HttpPost]
        public ActionResult obtenerPorDni(int dni = 0)
        {
            
            int idUsuario = Convert.ToInt32(Session["idUsuario"]);

            var lista = Negocio.JugadoresBL.obtenerPorDni(dni);

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
           
        }

        [HttpPost]
        public ActionResult obtener(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                var instancia = Negocio.JugadoresBL.obtener(id);

                //se calcula la cantidad de días desde el último pase no rechazado
                int idEstadoRechazado = Negocio.PasesEstadosBL.obtener("rechazado").id;
                var ultimoPase = instancia.JugadoresPases.OrderByDescending(x => x.id).FirstOrDefault(x => x.idPaseEstado != idEstadoRechazado);
                if (ultimoPase != null)
                {
                    DateTime fechaPase;
                    if (ultimoPase.fechaPase != null)
                        fechaPase = (DateTime)ultimoPase.fechaPase;
                    else
                        fechaPase = ultimoPase.fechaSolicitud;

                    instancia.diasUltimoPase = (DateTime.Now.Date - fechaPase).Days;
                }

                //se verifica si tiene alguna penalización en trámite
                var ultimaPenalizacion = instancia.JugadoresPenalizaciones.OrderByDescending(x => x.id).FirstOrDefault();
                if (ultimaPenalizacion != null && ultimaPenalizacion.PenalizacionesEstados.estadoFinal == false)
                    instancia.penalizacionActual = ultimaPenalizacion;
     
                var json = JsonConvert.SerializeObject(instancia,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult obtenerPublico(int id)
        {

                var instancia = Negocio.JugadoresBL.obtener(id);

                //se calcula la cantidad de días desde el último pase no rechazado
                int idEstadoRechazado = Negocio.PasesEstadosBL.obtener("rechazado").id;
                var ultimoPase = instancia.JugadoresPases.OrderByDescending(x => x.id).FirstOrDefault(x => x.idPaseEstado != idEstadoRechazado);
                if (ultimoPase != null)
                {
                    DateTime fechaPase;
                    if (ultimoPase.fechaPase != null)
                        fechaPase = (DateTime)ultimoPase.fechaPase;
                    else
                        fechaPase = ultimoPase.fechaSolicitud;

                    instancia.diasUltimoPase = (DateTime.Now.Date - fechaPase).Days;
                }

                //se verifica si tiene alguna penalización en trámite
                var ultimaPenalizacion = instancia.JugadoresPenalizaciones.OrderByDescending(x => x.id).FirstOrDefault();
                if (ultimaPenalizacion != null && ultimaPenalizacion.PenalizacionesEstados.estadoFinal == false)
                    instancia.penalizacionActual = ultimaPenalizacion;

                var json = JsonConvert.SerializeObject(instancia,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
         
        }
        [HttpPost]
        public ActionResult modificar(Negocio.Jugadores instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                if (instancia.id == 0)
                {
                    //es un alta
                    if (!Negocio.JugadoresBL.existeJugador(Convert.ToInt32(instancia.DNI)))
                    {
                        int id = Negocio.JugadoresBL.crear(instancia);
                        if (id > 0)
                        {
                            if (instancia.solicitaCarnet == true)
                            {
                                Negocio.EnviosMailsBL.notificacionSolicitudCarnet(id, idUsuario);

                            }

                            resultado = new { success = "true", message = id.ToString() };
                            Negocio.EnviosMailsBL.notificacionNuevoJugador(id, idUsuario);
                        }
                        else
                            resultado = new { success = "false", message = "La creación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                    }
                    else {
                        resultado = new { success = "false", message = "El numero de documento ya existe" };
                    }

                }
                else
                {
                    //es una modificación
                    if(Negocio.JugadoresBL.comprobarImagen(instancia)){
                        Negocio.EnviosMailsBL.notificacionCambioFoto(instancia.id, idUsuario);
                    }
                    if (Negocio.JugadoresBL.actualizar(instancia))
                        resultado = new { success = "true", message = instancia.id.ToString() };
                         
                    else
                        resultado = new { success = "false", message = "La modificación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                }


                return Json(resultado);
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult entregarCarnet(Negocio.JugadoresSolicitudesCarnets instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                instancia.idUsuarioEntrega = idUsuario;

                instancia = Negocio.JugadoresBL.entregarCarnet(instancia);

                var json = JsonConvert.SerializeObject(instancia,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult crearHabilitacion(Negocio.JugadoresHabilitaciones instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                instancia.idUsuario = idUsuario;
                instancia.fechaCreacion = DateTime.Now;

                //se verifica que ya no exista el número de estampilla
                if (Negocio.JugadoresHabilitacionesBL.existeEstampilla((int)instancia.numero,instancia.fechaHabilitacion,instancia.fechaInhabilitacion,instancia.categoria))
                    return Json(new { success = false, message = "El número de estampilla ya fue utilizado" });
                else
                {
                    instancia = Negocio.JugadoresBL.crearHabilitacion(instancia);

                    var json = JsonConvert.SerializeObject(instancia,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });

                    return Content(json, "application/json");
                }
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult eliminarHabilitacion(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };
                var instancia = Negocio.JugadoresHabilitacionesBL.eliminar(id, idUsuario);

                if (instancia)
                {

                    resultado = new { success = "true", message = id.ToString() };

                }
                else
                {
                    resultado = new { success = "false", message = "El Usuario no se puede eliminar" };
                }


                return Json(resultado);
            }
            else
            {
                return unauthorized();

            }
        }

        [HttpPost]
        public ActionResult crearPase(Negocio.JugadoresPases instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);

                instancia.idUsuarioSolicitud = idUsuario;
                instancia.fechaSolicitud = DateTime.Now;

                //se completan los datos de origen del pase
                var jugador = Negocio.JugadoresBL.obtener(instancia.idJugador);

                //se completan los datos 
                instancia.idClubOrigen = jugador.idClub;
                instancia.idAsociacionOrigen = jugador.Clubes.idAsociacion;
                instancia.idFederacionOrigen = jugador.Clubes.Asociaciones.idFederacion;

                //se crea el estado inicial del pase
                instancia.JugadoresPasesEstados = new List<Negocio.JugadoresPasesEstados>() { };
                Negocio.JugadoresPasesEstados estadoInicial = new Negocio.JugadoresPasesEstados();
                estadoInicial.fecha = DateTime.Now;
                estadoInicial.idPaseEstado = Negocio.PasesEstadosBL.obtener("solicitado").id;
                estadoInicial.idUsuario = idUsuario;
                instancia.JugadoresPasesEstados.Add(estadoInicial);
                
                int nivel = 0;
                if (usuario.UsuariosRoles != null && usuario.UsuariosRoles.Any(x => x.Roles.administraConfederacion == true))
                    nivel = usuario.UsuariosRoles.Where(x => x.Roles.administraConfederacion == true).FirstOrDefault().Roles.nivel; 

                //se crea el estado pendiente de aprobación o aprobado dependiendo los permisos
                Negocio.JugadoresPasesEstados estadoDeAprobacion = new Negocio.JugadoresPasesEstados();
                estadoDeAprobacion.fecha = DateTime.Now;
                estadoDeAprobacion.idUsuario = idUsuario;
                bool aprobado = false;
                if ((instancia.tipoPase == "INTER-FEDERACION" && nivel < 4) || (instancia.tipoPase == "INTER-ASOCIACION" && nivel < 3))
                    //se crea un estado pendiente de aprobación
                    estadoDeAprobacion.idPaseEstado = Negocio.PasesEstadosBL.obtener("pendiente").id;
                else
                {
                    //se aprueba automáticamente el pase
                    estadoDeAprobacion.idPaseEstado = Negocio.PasesEstadosBL.obtener("aprobado").id;
                    aprobado = true;
                    instancia.fechaPase = DateTime.Now.Date;
                }
                
                instancia.JugadoresPasesEstados.Add(estadoDeAprobacion);
                instancia.idPaseEstado = estadoDeAprobacion.idPaseEstado;

                instancia = Negocio.JugadoresBL.crearPase(instancia, aprobado);

                if (!aprobado)
                    //se envía el mail a las personas encargadas de aprobar el pase
                    Negocio.EnviosMailsBL.notificacionPaseParaAprobar(instancia.id, idUsuario);
                else
                    //se envía el mail informando la creación de pase
                    Negocio.EnviosMailsBL.notificacionPaseAprobado(instancia.id, idUsuario);
                
                var json = JsonConvert.SerializeObject(instancia,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult crearPenalizacion(Negocio.JugadoresPenalizaciones instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);

                int idEstadoSolicitado = Negocio.PenalizacionesEstadosBL.obtener("solicitado").id;

                instancia.idUsuario = idUsuario;
                instancia.fechaCreacion = DateTime.Now;
                instancia.idPenalizacionEstado = idEstadoSolicitado;

                //se crea el estado inicial de la penalizacion
                instancia.JugadoresPenalizacionesEstados = new List<Negocio.JugadoresPenalizacionesEstados>() { };
                Negocio.JugadoresPenalizacionesEstados estadoInicial = new Negocio.JugadoresPenalizacionesEstados();
                estadoInicial.fecha = DateTime.Now;
                estadoInicial.idPenalizacionEstado = idEstadoSolicitado;
                estadoInicial.idUsuario = idUsuario;
                estadoInicial.fechaInicio = instancia.fechaInicio;
                estadoInicial.dias = instancia.dias;

                if (instancia.id == 0)
                {
                    //es una creación
                    instancia.JugadoresPenalizacionesEstados.Add(estadoInicial);
                    instancia = Negocio.JugadoresBL.crearPenalizacion(instancia);
                    Negocio.EnviosMailsBL.notificacionNuevaPenalizacion(instancia.id, instancia.requiereAprobacionFederacion);
                }
                else
                {
                    //es una modificación
                    estadoInicial.observaciones = "Modificación de datos";
                    estadoInicial.idJugadorPenalizacion = instancia.id;
                    Negocio.JugadoresBL.modificarPenalizacion(instancia, estadoInicial);
                }

                
                var json = JsonConvert.SerializeObject(instancia,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult cambiarCategoria(Negocio.JugadoresCategorias instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                instancia.idUsuario = idUsuario;
                instancia.fechaCreacion = DateTime.Now;

                instancia = Negocio.JugadoresBL.cambiarCategoria(instancia);

                var json = JsonConvert.SerializeObject(instancia,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult solicitarCarnet(int id, string motivo)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                Negocio.JugadoresSolicitudesCarnets instancia = new Negocio.JugadoresSolicitudesCarnets();
                instancia.fechaSolicitud = DateTime.Now.Date;
                instancia.idJugador = id;
                instancia.motivo = motivo;

                instancia = Negocio.JugadoresBL.solicitarCarnet(instancia);
                Negocio.EnviosMailsBL.notificacionSolicitudCarnet(id,idUsuario);

                var json = JsonConvert.SerializeObject(instancia,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var now = DateTime.Now;
            string fileName = "";
            if (file.ContentLength > 0)
            {
                string timestamp = String.Format("{0}{1}{2}{3}{4}{5}{6}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                fileName = String.Format("{0}_{1}", timestamp, Path.GetFileName(file.FileName));
                var path = Path.Combine(Server.MapPath("~/Images/Jugadores/"), fileName);
                file.SaveAs(path);
            }
            return Json(new { success = true, fileName = fileName });
        }

        [HttpPost]
        public ActionResult obtenerUrl()
        {

            string str = System.Configuration.ConfigurationManager.AppSettings["obtenerUrl"];

            var json = JsonConvert.SerializeObject(str,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        
        [HttpPost]
        public ActionResult obtenerEstampilla(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.JugadoresBL.obtenerEstampilla(id);

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }
        
        [HttpPost]
        public ActionResult actualizarHabilitaciones(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.JugadoresBL.actualizarHabilitaciones(id);

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }


        [HttpPost]
        public ActionResult editarHabilitacion(Negocio.JugadoresHabilitaciones instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                instancia.idUsuario = idUsuario;
                instancia.fechaCreacion = DateTime.Now;

                //se verifica que ya no exista el número de estampilla
                if (Negocio.JugadoresHabilitacionesBL.existeEstampilla((int)instancia.numero, instancia.fechaHabilitacion, instancia.fechaInhabilitacion,instancia.categoria,instancia.idJugador))
                    return Json(new { success = false, message = "El número de estampilla ya fue utilizado" });
                else
                {
                    instancia = Negocio.JugadoresBL.editarHabilitacion(instancia);

                    var json = JsonConvert.SerializeObject(instancia,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });

                    return Content(json, "application/json");
                }
            }
            else
                return unauthorized();
        }
        [HttpPost]
        public ActionResult eliminar(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };
                var instancia = Negocio.JugadoresBL.eliminar(id, idUsuario);

                if (instancia)
                {

                    resultado = new { success = "true", message = id.ToString() };

                }
                else
                {
                    resultado = new { success = "false", message = "El Usuario no se puede eliminar" };
                }

                
                return Json(resultado);
            }
            else
            {
                return unauthorized();

            }
        }

        public ActionResult obtenerEdad(int id)
        {

            int idUsuario = Convert.ToInt32(Session["idUsuario"]);

            var lista = Negocio.JugadoresBL.obtenerEdad(id);

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");

        }

               

        private ActionResult unauthorized()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            HttpContext.Response.End();
            return Json("Unauthorized", JsonRequestBehavior.AllowGet);
        }
    }
}
