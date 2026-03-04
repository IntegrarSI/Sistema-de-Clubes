using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class AsociacionesBL
    {
        public static IEnumerable<Negocio.Asociaciones> obtenerListado(int idFederacion)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Asociaciones
                            where l.idFederacion == idFederacion
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static IEnumerable<Negocio.Asociaciones> obtenerListaAutocompletar(string filtro)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Asociaciones.Include("Federaciones")
                            where l.descripcion.Contains(filtro)
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static Negocio.Asociaciones obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                return db.Asociaciones
                    .Include("Federaciones")
                    .Include("Localidades")
                    .Include("Localidades.Provincias")
                    .FirstOrDefault(x => x.id == id);
            }
        }

        public static ListaPaginada obtenerListado(string busqueda, int idFederacion, int idUsuario, int pageIndex = 1, int pageSize = 10)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                ListaPaginada resultado = new ListaPaginada();
                
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);
                if (usuario.UsuariosRoles.Any(x => x.Roles.administraContenido == true || x.Roles.administraConfederacion == true))
                {
                    var lista = from l in db.Asociaciones
                                    .Include("Federaciones")
                                    .Include("Localidades")
                                    .Include("Localidades.Provincias")
                                select l;

                    if (idFederacion > 0)
                        lista = lista.Where(l => l.idFederacion == idFederacion);

                    if (busqueda.Length > 0)
                        lista = lista.Where(l => l.descripcion.Contains(busqueda) || l.direccion.Contains(busqueda) || l.mail.Contains(busqueda)
                             || l.localidad.Contains(busqueda) || l.Localidades.Provincias.descripcion.Contains(busqueda) || l.Federaciones.descripcion.Contains(busqueda));

                    //depende del rol del usuario, que asociaciones puede ver
                    
                    //rol asociacion: puede ver las asociaciones asociadas al usuario
                    if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "asociacion"))
                    {
                        int[] asociacionesPermitidas = db.UsuariosAsociaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idAsociacion).ToArray();

                        lista = lista.Where(x => asociacionesPermitidas.Contains(x.id));
                    }
                    //rol federacion: puede ver las asociaciones de las federaciones asociadas al usuario
                    else if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion"))
                    {
                        int[] federacionesPermitidas = db.UsuariosFederaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idFederacion).ToArray();

                        lista = lista.Where(x => federacionesPermitidas.Contains(x.Federaciones.id));
                    }

                    resultado.totalItems = lista.Count();
                    resultado.paginaActual = lista.OrderBy(l => l.descripcion).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return resultado;
            }
        }

        public static IEnumerable<Asociaciones> obtenerListaPorUsuario(int idUsuario, string filter)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);

                var lista = from l in db.Asociaciones
                                .Include("Federaciones")
                                .Include("Localidades")
                                .Include("Localidades.Provincias")
                            select l;

                //depende del rol del usuario, que asociaciones puede ver

                //rol asociacion: puede ver las asociaciones asociadas al usuario
                if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "asociacion"))
               {
                    int[] asociacionesPermitidas = db.UsuariosAsociaciones
                        .Where(x => x.idUsuario ==idUsuario)
                        .Select(x => x.idAsociacion).ToArray();

                    lista = lista.Where(x => asociacionesPermitidas.Contains(x.id));

                    
                }
                //rol federacion: puede ver las asociaciones de las federaciones asociadas al usuario
                else if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion"))
                {
                    int[] federacionesPermitidas = db.UsuariosFederaciones
                        .Where(x => x.idUsuario == usuario.id)
                        .Select(x => x.idFederacion).ToArray();

                    lista = lista.Where(x => federacionesPermitidas.Contains(x.Federaciones.id));


                }
                //if (!String.IsNullOrEmpty(filter))
                //    lista = lista.Where(l => l.descripcion.Contains(filter) || l.direccion.Contains(filter) || l.mail.Contains(filter)
                //         || l.localidad.Contains(filter) || l.Localidades.Provincias.descripcion.Contains(filter) || l.Federaciones.descripcion.Contains(filter));
                return lista.OrderBy(l => l.descripcion).ToList();
            }
        }

        public static int crear(Negocio.Asociaciones instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                db.Asociaciones.Add(instancia);

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

        public static bool actualizar(Negocio.Asociaciones instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.Asociaciones
                             where u.id == instancia.id
                             select u;

                Negocio.Asociaciones nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    if (instancia.idFederacion != 0)
                        nuevoObjeto.idFederacion = instancia.idFederacion;

                    nuevoObjeto.descripcion = instancia.descripcion;
                    nuevoObjeto.direccion = instancia.direccion;
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
                    nuevoObjeto.telefono = instancia.telefono;
                    nuevoObjeto.fax = instancia.fax;
                    nuevoObjeto.mail = instancia.mail;
                    nuevoObjeto.facebook = instancia.facebook;
                    nuevoObjeto.twitter = instancia.twitter;
                    nuevoObjeto.coordenadas = instancia.coordenadas;
                    nuevoObjeto.activo = instancia.activo;
                    nuevoObjeto.logo = instancia.logo;

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
