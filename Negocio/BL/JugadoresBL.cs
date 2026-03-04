using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class JugadoresBL
    {
        public static Negocio.Jugadores obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var jugador = db.Jugadores
                                .Include("Clubes")
                                .Include("Clubes.Asociaciones")
                                .Include("Clubes.Asociaciones.Federaciones")
                                .Include("Localidades")
                                .Include("Localidades.Provincias")
                                .Include("Categorias")
                                .Include("JugadoresSolicitudesCarnets")
                                .Include("JugadoresSolicitudesCarnets.Usuarios")
                                .Include("JugadoresHabilitaciones")
                                .Include("JugadoresHabilitaciones.Usuarios")
                                .Include("JugadoresCategorias")
                                .Include("JugadoresCategorias.Usuarios")
                                .Include("JugadoresCategorias.Categorias")
                                .Include("JugadoresPases")
                                .Include("JugadoresPases.PasesEstados")
                                .Include("JugadoresPases.Federaciones")
                                .Include("JugadoresPases.Federaciones1")
                                .Include("JugadoresPases.Asociaciones")
                                .Include("JugadoresPases.Asociaciones1")
                                .Include("JugadoresPases.Clubes")
                                .Include("JugadoresPases.Clubes1")
                                .Include("JugadoresPenalizaciones")
                                .Include("JugadoresPenalizaciones.PenalizacionesEstados")
                                .Include("JugadoresPenalizaciones.Usuarios")
                                .Include("JugadoresHistorial")
                                 .Include("JugadoresHistorial.Usuarios")
                                .AsNoTracking()
                                .FirstOrDefault(x => x.id == id);

                //se verifica si está habilitado
                if (jugador.JugadoresHabilitaciones != null && jugador.JugadoresHabilitaciones.Count > 0)
                {
                    //jugador.estampillaActual = jugador.JugadoresHabilitaciones.FirstOrDefault(x => x.fechaHabilitacion <= DateTime.Now.Date && x.fechaInhabilitacion >= DateTime.Now.Date);
                    jugador.estampillaActual = jugador.JugadoresHabilitaciones.OrderByDescending(x => x.fechaHabilitacion).FirstOrDefault(x => x.fechaHabilitacion <= DateTime.Now.Date && x.fechaInhabilitacion >= DateTime.Now.Date);
                    if (jugador.estampillaActual != null)
                        jugador.habilitado = true;
                    else
                        jugador.habilitado = false;
                }
                else
                    jugador.habilitado = false;

                db.SaveChanges();

                return jugador;
            }
        }


        public static Negocio.Jugadores obtenerPorDni(int dni)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var jugador = db.Jugadores
                                .Include("JugadoresSolicitudesCarnets")
                                .Include("JugadoresSolicitudesCarnets.Usuarios")
                                .AsNoTracking()
                                .FirstOrDefault(x => x.DNI == dni);

                db.SaveChanges();

                return jugador;
            }
        }



        public static ListaPaginada obtenerListado(string busqueda, string busquedaFederacion, string busquedaAsociado, string busquedaClub, int idUsuario, int pageIndex = 1, int pageSize = 10)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);

                ListaPaginada resultado = new ListaPaginada();
                if (usuario.UsuariosRoles.Any(x => x.Roles.administraContenido == true || x.Roles.administraConfederacion == true))
                {

                    long dniFilter = 0;
                    long.TryParse(busqueda, out dniFilter);

                    var lista = from l in db.Jugadores
                                .Include("JugadoresHabilitaciones")
                                .Include("JugadoresSolicitudesCarnets")
                                .Include("Clubes")
                                .Include("Clubes.Asociaciones")
                                .Include("Clubes.Asociaciones.Federaciones")
                                .Include("Localidades")
                                .Include("Localidades.Provincias")
                                .Include("Categorias")
                                .AsNoTracking()
                                select l;

                    if (busqueda.Length > 0)
                        lista = lista.Where(l => (l.apellido + " " + l.nombre).Contains(busqueda) || l.direccion.Contains(busqueda) || l.mail.Contains(busqueda)
                        ||  l.DNI== dniFilter
                        || l.localidad.Contains(busqueda) || l.Localidades.Provincias.descripcion.Contains(busqueda) || l.Clubes.Asociaciones.descripcion.Contains(busqueda)
                             || l.Clubes.Asociaciones.Federaciones.descripcion.Contains(busqueda) || l.Clubes.descripcion.Contains(busqueda));


                    //// Filtra por separado las asociaciones, federaciones y clubes
                    if (busquedaAsociado != null )
                    {
                        lista = lista.Where(l => l.Clubes.Asociaciones.descripcion.Contains(busquedaAsociado));
                    }

                    if (busquedaFederacion != null )
                    {
                        lista = lista.Where(l => l.Clubes.Asociaciones.Federaciones.descripcion.Contains(busquedaFederacion));
                    }

                    if (busquedaClub != null )
                    {
                        lista = lista.Where(l => l.Clubes.descripcion.Contains(busquedaClub));
                    }
                    //depende del rol del usuario, que asociaciones puede ver

                    //rol asociacion: puede ver las asociaciones asociadas al usuario
                    if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "asociacion"))
                    {
                        int[] asociacionesPermitidas = db.UsuariosAsociaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idAsociacion).ToArray();

                        lista = lista.Where(x => asociacionesPermitidas.Contains(x.Clubes.Asociaciones.id));
                    }
                    //rol federacion: puede ver las asociaciones de las federaciones asociadas al usuario
                    else if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion"))
                    {
                        int[] federacionesPermitidas = db.UsuariosFederaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idFederacion).ToArray();

                        lista = lista.Where(x => federacionesPermitidas.Contains(x.Clubes.Asociaciones.Federaciones.id));
                    }

                    
                    resultado.totalItems = lista.Count();
                    resultado.paginaActual = lista.OrderBy(l => l.apellido).ThenBy(l => l.nombre).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    
                }

                return resultado;
            }
        }

        public static int crear(Negocio.Jugadores instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                if (instancia.solicitaCarnet == true)
                {
                    JugadoresSolicitudesCarnets solicitud = new JugadoresSolicitudesCarnets();
                    solicitud.fechaSolicitud = DateTime.Now.Date;
                    solicitud.motivo = "Nuevo";
                    instancia.JugadoresSolicitudesCarnets = new List<JugadoresSolicitudesCarnets>() { solicitud };
                }

                instancia.habilitado = false;
                db.Jugadores.Add(instancia);

                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return instancia.id;
                }
                catch (Exception ex)
                {

                }
            }

            return 0;
        }

        public static bool actualizar(Negocio.Jugadores instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.Jugadores
                             where u.id == instancia.id
                             select u;

                Negocio.Jugadores nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    nuevoObjeto.DNI = instancia.DNI;
                    nuevoObjeto.apellido = instancia.apellido;
                    nuevoObjeto.nombre = instancia.nombre;
                    nuevoObjeto.fechaNacimiento = instancia.fechaNacimiento.Hour == 23 ? instancia.fechaNacimiento.AddHours(1): instancia.fechaNacimiento;
                    //nuevoObjeto.idClub = instancia.idClub;
                    nuevoObjeto.direccion = instancia.direccion;
                    nuevoObjeto.telefonoFijo = instancia.telefonoFijo;
                    nuevoObjeto.celular = instancia.celular;
                    nuevoObjeto.mail = instancia.mail;
                    if (instancia.Localidades != null)
                    {
                        nuevoObjeto.localidad = instancia.Localidades.descripcion;
                        nuevoObjeto.idLocalidad = instancia.Localidades.id;
                    }
                    else
                    {
                        nuevoObjeto.localidad = instancia.localidad;
                        nuevoObjeto.idLocalidad = instancia.idLocalidad;
                    }
                    nuevoObjeto.sexo = instancia.sexo;
                    nuevoObjeto.fechaIngreso = instancia.fechaIngreso;
                    nuevoObjeto.foto = instancia.foto;
                    nuevoObjeto.Puntaje = instancia.Puntaje;
                    nuevoObjeto.nroCarnet = instancia.nroCarnet;
                    nuevoObjeto.Historial = instancia.Historial;

                    try
                    {
                        db.SaveChanges();

                        return true;
                    }
                    catch (Exception ex)
                    {
                    }
                }

                return false;
            }
        }


        public static bool comprobarImagen(Negocio.Jugadores instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.Jugadores
                             where u.id == instancia.id
                             select u;

                Negocio.Jugadores nuevoObjeto = objeto.Single();

                if (nuevoObjeto.foto != instancia.foto)
                {
                    return true;
                }

                return false;
            }
        }

        public static JugadoresSolicitudesCarnets entregarCarnet(Negocio.JugadoresSolicitudesCarnets instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.JugadoresSolicitudesCarnets
                             where u.id == instancia.id
                             select u;

                Negocio.JugadoresSolicitudesCarnets nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    nuevoObjeto.fechaEntrega = DateTime.Now.Date;
                    nuevoObjeto.idUsuarioEntrega = instancia.idUsuarioEntrega;

                    var jugador = db.Jugadores.FirstOrDefault(x => x.id == instancia.idJugador);
                    jugador.solicitaCarnet = false;
                    
                    try
                    {
                        db.SaveChanges();

                        return db.JugadoresSolicitudesCarnets.Include("Usuarios").FirstOrDefault(x => x.id == instancia.id);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                return null;
            }
        }

        public static JugadoresHabilitaciones crearHabilitacion(Negocio.JugadoresHabilitaciones instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                
                db.JugadoresHabilitaciones.Add(instancia);

                if (instancia.fechaHabilitacion.Date <= DateTime.Now.Date && instancia.fechaInhabilitacion >= DateTime.Now.Date)
                {
                    var jugador = db.Jugadores.FirstOrDefault(x => x.id == instancia.idJugador);
                    jugador.habilitado = true;
                }

                try
                {
                    db.SaveChanges();

                    return db.JugadoresHabilitaciones.Include("Usuarios").FirstOrDefault(x => x.id == instancia.id);
                }
                catch (Exception ex)
                {
                }

                return null;
            }
        }

        public static JugadoresPases crearPase(Negocio.JugadoresPases instancia, bool paseAprobado)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                db.JugadoresPases.Add(instancia);

                //si el pase fue aprobado se modifican los datos del jugador
                if (paseAprobado)
                {
                    var jugador = db.Jugadores.FirstOrDefault(x => x.id == instancia.idJugador);
                    jugador.idClub = instancia.idClubDestino;
                }

                try
                {
                    db.SaveChanges();

                    return db.JugadoresPases.Include("PasesEstados").FirstOrDefault(x => x.id == instancia.id);
                }
                catch (Exception ex)
                {
                }

                return null;
            }
        }

        public static JugadoresPenalizaciones crearPenalizacion(Negocio.JugadoresPenalizaciones instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                db.JugadoresPenalizaciones.Add(instancia);

                try
                {
                    db.SaveChanges();

                    return db.JugadoresPenalizaciones.Include("PenalizacionesEstados").FirstOrDefault(x => x.id == instancia.id);
                }
                catch (Exception ex)
                {
                }

                return null;
            }
        }

        public static JugadoresSolicitudesCarnets modificarPenalizacion(Negocio.JugadoresPenalizaciones instancia, Negocio.JugadoresPenalizacionesEstados estado)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.JugadoresPenalizaciones
                             where u.id == instancia.id
                             select u;

                Negocio.JugadoresPenalizaciones nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    nuevoObjeto.fechaInicio = instancia.fechaInicio;
                    nuevoObjeto.dias = instancia.dias;
                    nuevoObjeto.fechaFin = instancia.fechaFin;
                    nuevoObjeto.motivos = instancia.motivos;
                                        
                    db.JugadoresPenalizacionesEstados.Add(estado);
                    
                    try
                    {
                        db.SaveChanges();

                        return db.JugadoresSolicitudesCarnets.Include("Usuarios").FirstOrDefault(x => x.id == instancia.id);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                return null;
            }
        }

        public static JugadoresCategorias cambiarCategoria(Negocio.JugadoresCategorias instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                db.JugadoresCategorias.Add(instancia);

                var jugador = db.Jugadores.FirstOrDefault(x => x.id == instancia.idJugador);
                jugador.idCategoria = instancia.idCategoria;

                try
                {
                    db.SaveChanges();

                    return db.JugadoresCategorias.Include("Usuarios").Include("Categorias").FirstOrDefault(x => x.id == instancia.id);
                }
                catch (Exception ex)
                {
                }

                return null;
            }
        }

        public static JugadoresSolicitudesCarnets solicitarCarnet(Negocio.JugadoresSolicitudesCarnets instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                db.JugadoresSolicitudesCarnets.Add(instancia);

                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    return db.JugadoresSolicitudesCarnets.Include("Usuarios").FirstOrDefault(x => x.id == instancia.id);
                }
                catch (Exception ex)
                {

                }
            }

            return null;
        }

        public static Negocio.JugadoresHabilitaciones obtenerEstampilla(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.JugadoresHabilitaciones
                            select l;

                lista = lista.Where(l => l.id == id);

                return lista.FirstOrDefault();

            }
        }
        

            
        public static List<Negocio.JugadoresHabilitaciones> actualizarHabilitaciones(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.JugadoresHabilitaciones
                            select l;

                lista = lista.Where(l => l.idJugador == id);

                return lista.ToList();

            }
        }

        public static JugadoresHabilitaciones editarHabilitacion(Negocio.JugadoresHabilitaciones instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                 var objeto = from u in db.JugadoresHabilitaciones
                             where u.id == instancia.id
                             select u;

                 Negocio.JugadoresHabilitaciones nuevoObjeto = objeto.Single(); 
                if (nuevoObjeto != null) {
                    nuevoObjeto.numero = instancia.numero;
                    nuevoObjeto.fechaHabilitacion = instancia.fechaHabilitacion;
                    nuevoObjeto.fechaInhabilitacion = instancia.fechaInhabilitacion;
               
                
                }

                // Guardar historial

                db.JugadoresHistorial.Add(new JugadoresHistorial()
                {
                    idJugador = instancia.idJugador,
                    idUsuario = instancia.idUsuario,
                    fecha = DateTime.Now,
                    accion = "Modificación Estampilla"
                });

                try
                {
                    db.SaveChanges();

                  

                    return db.JugadoresHabilitaciones.Include("Usuarios").FirstOrDefault(x => x.id == instancia.id);
                   
                }
                catch (Exception ex)
                {
                }

                return null;
            }
        }

        public static bool eliminar(int id, int idUsuario)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                    var objeto = from u in db.Jugadores
                             where u.id == id
                             select u;

                int idJugador = objeto.FirstOrDefault().id;
                    db.Jugadores.Remove(objeto.FirstOrDefault());
                    
                
                    try
                    {
                        db.SaveChanges();

                        // Guardar el usuario de eliminación, al objeto que generó el trigger
                        var eliminado = db.JugadoresEliminados.FirstOrDefault(p=> p.id == idJugador);
                        eliminado.usuarioEliminacion = idUsuario;

                        db.SaveChanges();
                    return true;
                    }
                    catch (Exception ex)
                    {
                    }
                    return false;
            }
        }


        public static string obtenerEdad(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                //se consulta el jugador para saber si pertenece a la categoria juveniles o mayores
                var jugadores = from j in db.Jugadores
                                where j.id == id
                                select j;



                var jugadoraux = jugadores.FirstOrDefault();


                int edad = DateTime.Now.Year - jugadoraux.fechaNacimiento.Year;
                //DateTime nacimiento = jugadoraux.fechaNacimiento.AddYears(edad);

                //if (DateTime.Now.CompareTo(nacimiento) > 0)
                //{
                //    edad--;
                //}

                if (edad < 80 && edad > 15)
                {
                    return "M";
                }
                else if (edad >= 80)
                {
                    return "A";
                }
                {
                    return "I";
                }
            }
        }

        public static bool existeJugador(int dni)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                return db.Jugadores.Count(x => x.DNI == dni) > 0;
            }
        }
        





    }
}
