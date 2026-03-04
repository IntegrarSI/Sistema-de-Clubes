using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class FederacionesBL
    {
        public static Negocio.Federaciones obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                return db.Federaciones
                    .Include("Localidades")
                    .Include("Localidades.Provincias")
                    .FirstOrDefault(x => x.id == id);
            }
        }

        public static ListaPaginada obtenerListado(string busqueda, int idUsuario, int pageIndex = 1, int pageSize = 10)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);

                ListaPaginada resultado = new ListaPaginada();
                if (usuario.UsuariosRoles.Any(x => x.Roles.administraContenido == true || x.Roles.administraConfederacion == true))
                {
                    var lista = from l in db.Federaciones
                                    .Include("Localidades")
                                    .Include("Localidades.Provincias")
                                select l;

                    if (busqueda.Length > 0)
                        lista = lista.Where(l => l.descripcion.Contains(busqueda) || l.direccion.Contains(busqueda) || l.mail.Contains(busqueda)
                             || l.localidad.Contains(busqueda) || l.Localidades.Provincias.descripcion.Contains(busqueda));

                    //rol asociacion: no puede ver las federaciones
                    if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "asociacion"))
                    {
                        resultado.totalItems = 0;
                        resultado.paginaActual = new List<Negocio.Federaciones>() { };
                    }
                    else 
                    {
                        //rol federacion: puede ver las federaciones asociadas al usuario
                        if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion"))
                        {
                            int[] federacionesPermitidas = db.UsuariosFederaciones
                                .Where(x => x.idUsuario == usuario.id)
                                .Select(x => x.idFederacion).ToArray();

                            lista = lista.Where(x => federacionesPermitidas.Contains(x.id));
                        }

                        resultado.totalItems = lista.Count();
                        resultado.paginaActual = lista.OrderBy(l => l.descripcion).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }

                return resultado;
            }
        }

        public static IEnumerable<Negocio.Federaciones> obtenerListaAutocompletar(string filtro)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Federaciones
                            where l.descripcion.Contains(filtro)
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static IEnumerable<Negocio.Federaciones> obtenerListaPorUsuario(int idUsuario)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                //esto estaba antes
                var lista = from l in db.Federaciones
                            select l;

                //depende del rol del usuario, que asociaciones puede ver
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);

                //rol asociacion: no puede ver las federaciones
                if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "asociacion"))
                {
                    //esto estaba antes
                    //return new List<Negocio.Federaciones>() { };
                    //Nuevo
                    int idFederacion = db.UsuariosAsociaciones.Include("Asociaciones").Where (a => a.idUsuario == idUsuario).Select(a => a.Asociaciones.idFederacion).FirstOrDefault();
                    
                    var lista2 = from l in db.Federaciones
                                where l.id.Equals(idFederacion)
                                select l;
                    lista = lista2;
                }

                //rol federacion: puede ver las federaciones asociadas al usuario
                else if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion"))
                {
                    int[] federacionesPermitidas = db.UsuariosFederaciones
                        .Where(x => x.idUsuario == usuario.id)
                        .Select(x => x.idFederacion).ToArray();

                    lista = lista.Where(x => federacionesPermitidas.Contains(x.id));
                }

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static IEnumerable<Negocio.Federaciones> obtenerLista()
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Federaciones
                            where l.activo == true
                            select l;
                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static IEnumerable<Negocio.Federaciones> obtenerFederaciones()
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Federaciones
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static IEnumerable<Negocio.Asociaciones> obtenerAsociaciones(int idFederacion)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Asociaciones
                            where l.idFederacion == idFederacion
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static IEnumerable<Negocio.Clubes> obtenerClubes(int idAsociacion)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Clubes
                            where l.idAsociacion == idAsociacion
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static int crear(Negocio.Federaciones instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                db.Federaciones.Add(instancia);

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

        public static bool actualizar(Negocio.Federaciones instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.Federaciones
                             where u.id == instancia.id
                             select u;

                Negocio.Federaciones nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
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
