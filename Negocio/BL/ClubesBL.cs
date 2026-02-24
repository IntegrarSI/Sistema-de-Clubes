using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class ClubesBL
    {
        public static IEnumerable<Negocio.Clubes> obtenerListado(int idAsociacion)
        {
            using (CABEntities db = new CABEntities())
            {
                var lista = from l in db.Clubes
                            where l.idAsociacion == idAsociacion
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static IEnumerable<Negocio.Clubes> obtenerListaAutocompletar(string filtro)
        {
            using (CABEntities db = new CABEntities())
            {
                var lista = from l in db.Clubes
                            where l.descripcion.Contains(filtro)
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }

        public static Negocio.Clubes obtener(int id)
        {
            using (CABEntities db = new CABEntities())
            {
                return db.Clubes
                    .Include("Asociaciones")
                    .Include("Asociaciones.Federaciones")
                    .Include("Localidades")
                    .Include("Localidades.Provincias")
                    .FirstOrDefault(x => x.id == id);
            }
        }

        public static ListaPaginada obtenerListado(string busqueda, int idAsociacion, int idUsuario, int pageIndex = 1, int pageSize = 10)
        {
            using (CABEntities db = new CABEntities())
            {
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);

                ListaPaginada resultado = new ListaPaginada();
                if (usuario.UsuariosRoles.Any(x => x.Roles.administraContenido == true || x.Roles.administraConfederacion == true))
                {
                    var lista = from l in db.Clubes
                                    .Include("Asociaciones")
                                    .Include("Asociaciones.Federaciones")
                                    .Include("Localidades")
                                    .Include("Localidades.Provincias")
                                select l;

                    if (busqueda.Length > 0)
                        lista = lista.Where(l => l.descripcion.Contains(busqueda) || l.direccion.Contains(busqueda) || l.mail.Contains(busqueda)
                             || l.localidad.Contains(busqueda) || l.Localidades.Provincias.descripcion.Contains(busqueda) 
                             || l.Asociaciones.descripcion.Contains(busqueda) || l.Asociaciones.Federaciones.descripcion.Contains(busqueda));

                    if (idAsociacion > 0)
                        lista = lista.Where(x => x.idAsociacion == idAsociacion);

                    //depende del rol del usuario, que clubes puede ver

                    //rol asociacion: puede ver los clubes de las asociaciones asociadas al usuario
                    if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "asociacion"))
                    {
                        int[] asociacionesPermitidas = db.UsuariosAsociaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idAsociacion).ToArray();

                        lista = lista.Where(x => asociacionesPermitidas.Contains(x.Asociaciones.id));
                    }
                    //rol federacion: puede ver los clubes de las asociaciones de las federaciones asociadas al usuario
                    else if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion"))
                    {
                        int[] federacionesPermitidas = db.UsuariosFederaciones
                            .Where(x => x.idUsuario == usuario.id)
                            .Select(x => x.idFederacion).ToArray();

                        lista = lista.Where(x => federacionesPermitidas.Contains(x.Asociaciones.Federaciones.id));
                    }

                    resultado.totalItems = lista.Count();
                    resultado.paginaActual = lista.OrderBy(l => l.descripcion).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return resultado;
            }
        }

        public static int crear(Negocio.Clubes instancia)
        {
            using (CABEntities db = new CABEntities())
            {
                db.Clubes.Add(instancia);

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

        public static bool actualizar(Negocio.Clubes instancia)
        {
            using (CABEntities db = new CABEntities())
            {
                var objeto = from u in db.Clubes
                             where u.id == instancia.id
                             select u;

                Negocio.Clubes nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    if (instancia.idAsociacion != 0)
                        nuevoObjeto.idAsociacion = instancia.idAsociacion;

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
