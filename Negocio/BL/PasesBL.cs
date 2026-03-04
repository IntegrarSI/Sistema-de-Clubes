using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class PasesBL
    {
        public static ListaPaginada obtenerListadoPendientes(int idUsuario, int pageIndex = 1, int pageSize = 10)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);

                ListaPaginada resultado = new ListaPaginada();
                if (usuario.UsuariosRoles.Any(x => x.Roles.administraConfederacion == true))
                {
                    var lista = from l in db.JugadoresPases
                                .Include("Jugadores")
                                .Include("Clubes")
                                .Include("Asociaciones")
                                .Include("Federaciones")
                                .Include("Clubes1")
                                .Include("Asociaciones1")
                                .Include("Federaciones1")
                                .Include("PasesEstados")
                                .Include("Usuarios")
                                .Include("Categorias")
                                select l;

                    //depende del rol del usuario, que pases puede ver

                    //rol asociacion: puede ver los pases tipo INTER-CLUB de las asociaicones relacionadas al usuario
                    string tipoPase = "";
                    if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "asociacion"))
                    {
                        int[] asociacionesPermitidas = db.UsuariosAsociaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idAsociacion).ToArray();

                        tipoPase = "INTER-CLUB";

                        lista = lista.Where(x => asociacionesPermitidas.Contains(x.idAsociacionOrigen));
                    }
                    //rol federacion: puede ver los pases tipos INTER-ASOCIACION de las federaciones relacionadas al usuario
                    else if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion"))
                    {
                        int[] federacionesPermitidas = db.UsuariosFederaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idFederacion).ToArray();

                        tipoPase = "INTER-ASOCIACION";

                        lista = lista.Where(x => federacionesPermitidas.Contains(x.idFederacionOrigen));
                    }
                    //rol confederacion: puede ver los pases tipos INTER-FEDERACION
                    else if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "confederacion"))
                        tipoPase = "INTER-FEDERACION";

                    lista = lista.Where(x => x.tipoPase == tipoPase && x.PasesEstados.estadoIntermedio == true);

                    resultado.totalItems = lista.Count();
                    resultado.paginaActual = lista.OrderBy(l => l.fechaSolicitud).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return resultado;
            }
        }

        public static bool aprobarPase(int idJugadorPase, int idUsuario)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.JugadoresPases
                             where u.id == idJugadorPase
                             select u;

                Negocio.JugadoresPases nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    //se busca el id de pase aprobado
                    int idEstado = Negocio.PasesEstadosBL.obtener("aprobado").id;

                    nuevoObjeto.fechaPase = DateTime.Now.Date;
                    nuevoObjeto.idPaseEstado = idEstado;

                    Negocio.JugadoresPasesEstados cambioEstado = new JugadoresPasesEstados();
                    cambioEstado.fecha = DateTime.Now;
                    cambioEstado.idJugadorPase = idJugadorPase;
                    cambioEstado.idPaseEstado = idEstado;
                    cambioEstado.idUsuario = idUsuario;
                    db.JugadoresPasesEstados.Add(cambioEstado);

                    var jugador = (from j in db.Jugadores
                                   where j.id == nuevoObjeto.idJugador
                                   select j).FirstOrDefault();
                    jugador.idClub = nuevoObjeto.idClubDestino;

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

        public static bool revisarPase(int idJugadorPase, int idUsuario)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.JugadoresPases
                             where u.id == idJugadorPase
                             select u;

                Negocio.JugadoresPases nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    //se busca el id de pase aprobado
                    int idEstado = Negocio.PasesEstadosBL.obtener("revision").id;

                    nuevoObjeto.idPaseEstado = idEstado;

                    Negocio.JugadoresPasesEstados cambioEstado = new JugadoresPasesEstados();
                    cambioEstado.fecha = DateTime.Now;
                    cambioEstado.idJugadorPase = idJugadorPase;
                    cambioEstado.idPaseEstado = idEstado;
                    cambioEstado.idUsuario = idUsuario;
                    db.JugadoresPasesEstados.Add(cambioEstado);

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

        public static bool rechazarPase(int idJugadorPase, string motivosRechazo, int idUsuario)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.JugadoresPases
                             where u.id == idJugadorPase
                             select u;

                Negocio.JugadoresPases nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    //se busca el id de estado
                    int idEstado = Negocio.PasesEstadosBL.obtener("rechazado").id;

                    nuevoObjeto.fechaPase = DateTime.Now.Date;
                    nuevoObjeto.idPaseEstado = idEstado;
                    nuevoObjeto.motivosRechazo = motivosRechazo;

                    Negocio.JugadoresPasesEstados cambioEstado = new JugadoresPasesEstados();
                    cambioEstado.fecha = DateTime.Now;
                    cambioEstado.idJugadorPase = idJugadorPase;
                    cambioEstado.idPaseEstado = idEstado;
                    cambioEstado.idUsuario = idUsuario;
                    cambioEstado.comentarios = motivosRechazo;
                    db.JugadoresPasesEstados.Add(cambioEstado);

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
    }
}
