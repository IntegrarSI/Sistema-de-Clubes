using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Data;
namespace Negocio
{
    public partial class UsuariosBL
    {
        static DeportesEntities db2 = new DeportesEntities();

        public static Negocio.Usuarios verificarLogin(string nombreUsuario, string password, out bool esPasswordCorrecta)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                nombreUsuario = nombreUsuario.ToLower();
                Negocio.Usuarios usuarioaux = db.Usuarios.Where(x => x.nombreUsuario.Equals(nombreUsuario)).FirstOrDefault();
                Negocio.Usuarios usuario = db.Usuarios.Include("UsuariosRoles").Include("UsuariosRoles.Roles").Where(u => u.nombreUsuario.Equals(nombreUsuario)).FirstOrDefault();
                esPasswordCorrecta = usuario != null && usuario.password.Equals(Utils.Encriptacion.encriptarSHA1(password));
                return usuario;
            }
        }

        public static bool cambiarPassword(string nombreUsuario, string nuevaPassword)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                nombreUsuario = nombreUsuario.ToLower();

                var objeto = from u in db.Usuarios
                             where u.nombreUsuario.ToLower() == nombreUsuario
                             select u;

                Negocio.Usuarios nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    nuevoObjeto.password = nuevaPassword;

                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch
                    {
                    }
                }

                return false;
            }
        }

        public static ListaPaginada obtenerListado(string busqueda, int idUsuario, int pageIndex = 1, int pageSize = 10)
        {
           
                //depende del rol del usuario, que usuarios puede ver
                //rol seguridad y confederación pueden ver todo
                //rol federación puede ver los datos de las asociaciones
                Negocio.Usuarios usuario = obtener(idUsuario);

                ListaPaginada resultado = new ListaPaginada();
                if (usuario.UsuariosRoles.Any(x => x.Roles.administraSeguridad == true))
                {
                    var lista = from l in db2.Usuarios
                                    .Include("UsuariosRoles")
                                    .Include("UsuariosRoles.Roles")
                                    .Include("UsuariosFederaciones.Federaciones")
                                    .Include("UsuariosAsociaciones.Asociaciones")
                                select l;

                    if (busqueda.Length > 0)
                        lista = lista.Where(l => l.nombreUsuario.Contains(busqueda) || l.nombre.Contains(busqueda) || l.mail.Contains(busqueda)
                             || l.UsuariosRoles.Any(x => x.Roles.descripcion.Contains(busqueda)));

                    //si es federación (y no seguridad que ve todo) se buscan los ids de todos los usuarios de las federanciones del usuario actual
                    List<int> idsUsuarios = new List<int>() { };
                    if (usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "federacion") && !usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "seguridad"))
                    {
                        //se carga la lista de usuarios
                        DataTable dtUsuarios = obtenerUsuariosDeFederaciones(idUsuario);
                        if (dtUsuarios.Rows.Count > 0)
                        {
                            foreach (DataRow item in dtUsuarios.Rows)
                            {
                                idsUsuarios.Add(Convert.ToInt32(item["id"]));
                            }

                            lista = lista.Where(x => idsUsuarios.Contains(x.id));
                        }
                    }
                    
                    resultado.totalItems = lista.Count();
                    resultado.paginaActual = lista.OrderBy(l => l.nombre).Skip((pageIndex - 1) * pageSize).Take(pageSize);//.ToList();
                }

                return resultado;
            
        }

        private static DataTable obtenerUsuariosDeFederaciones(int idUsuario)
        {
            using (SqlCommand comando = new SqlCommand())
            {
                StringBuilder Sql = new StringBuilder();

                Sql.Append(@"
                    select u.id
                    from Usuarios u
                    inner join UsuariosAsociaciones ua on ua.idUsuario = u.id
                    inner join (
	                    select a.id idAsociacion
	                    from Asociaciones a
	                    inner join UsuariosFederaciones uf on uf.idFederacion = a.idFederacion
	                    where uf.idUsuario = @idUsuario
	                    group by a.id
                    ) a on a.idAsociacion = ua.idAsociacion
                    group by u.id");

                comando.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.Int)).Value = idUsuario;

                comando.CommandText = Sql.ToString();

                return Integrar.Data.Sql.EjecutarConsulta(comando);
            }
        }

        public static Negocio.Usuarios obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                return db.Usuarios
                    .Include("UsuariosRoles")
                    .Include("UsuariosRoles.Roles")
                    .Include("UsuariosFederaciones")
                    .Include("UsuariosFederaciones.Federaciones")
                    .Include("UsuariosAsociaciones.Asociaciones")
                    .Include("UsuariosAsociaciones.Asociaciones.Federaciones")
                    .FirstOrDefault(x => x.id == id);

               
            }
        }

        public static List<string> obtenerRol(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var aux = from l in db.UsuariosRoles
                         .Include("Roles")
                         where l.idUsuario == id
                         select l.Roles.descripcion;
               
                return aux.ToList();
           
            }
        }
        public static int crear(Negocio.Usuarios instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                if (instancia.UsuariosRoles != null && instancia.UsuariosRoles.Count > 0)
                {
                    foreach (var item in instancia.UsuariosRoles)
                        item.Roles = null;
                }

                if (instancia.UsuariosAsociaciones != null && instancia.UsuariosAsociaciones.Count > 0)
                {
                    foreach (var item in instancia.UsuariosAsociaciones)
                        item.Asociaciones = null;
                }

                if (instancia.UsuariosFederaciones != null && instancia.UsuariosFederaciones.Count > 0)
                {
                    foreach (var item in instancia.UsuariosFederaciones)
                        item.Federaciones = null;
                }

                db.Usuarios.Add(instancia);

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

        public static bool actualizar(Negocio.Usuarios instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.Usuarios
                             where u.id == instancia.id
                             select u;

                Negocio.Usuarios nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    nuevoObjeto.nombre = instancia.nombre;
                    nuevoObjeto.mail = instancia.mail;
                    nuevoObjeto.activo = instancia.activo;

                    foreach (var clon in db.UsuariosRoles.Where(x => x.idUsuario == instancia.id).ToList())
                        db.Entry(clon).State = System.Data.Entity.EntityState.Deleted;

                    if (instancia.UsuariosRoles != null && instancia.UsuariosRoles.Count > 0)
                    {
                        Negocio.UsuariosRoles relacion;
                        foreach (var item in instancia.UsuariosRoles)
                        {
                            relacion = new UsuariosRoles();
                            relacion.idUsuario = instancia.id;
                            relacion.idRol = item.idRol;
                            db.UsuariosRoles.Add(relacion);
                        }
                    }

                    foreach (var clon in db.UsuariosAsociaciones.Where(x => x.idUsuario == instancia.id).ToList())
                        db.Entry(clon).State = System.Data.Entity.EntityState.Deleted;

                    if (instancia.UsuariosAsociaciones != null && instancia.UsuariosAsociaciones.Count > 0)
                    {
                        Negocio.UsuariosAsociaciones relacion;
                        foreach (var item in instancia.UsuariosAsociaciones)
                        {
                            relacion = new UsuariosAsociaciones();
                            relacion.idUsuario = instancia.id;
                            relacion.idAsociacion = item.idAsociacion;
                            db.UsuariosAsociaciones.Add(relacion);
                        }
                    }

                    foreach (var clon in db.UsuariosFederaciones.Where(x => x.idUsuario == instancia.id).ToList())
                        db.Entry(clon).State = System.Data.Entity.EntityState.Deleted;

                    if (instancia.UsuariosFederaciones != null && instancia.UsuariosFederaciones.Count > 0)
                    {
                        Negocio.UsuariosFederaciones relacion;
                        foreach (var item in instancia.UsuariosFederaciones)
                        {
                            relacion = new UsuariosFederaciones();
                            relacion.idUsuario = instancia.id;
                            relacion.idFederacion = item.idFederacion;
                            db.UsuariosFederaciones.Add(relacion);
                        }
                    }

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

        public static string resetPassword(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.Usuarios
                             where u.id == id
                             select u;

                Negocio.Usuarios nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    string nuevaPassword = nuevoObjeto.nombreUsuario.ToLower() + "1234";

                    nuevoObjeto.password = Negocio.Utils.Encriptacion.encriptarSHA1(nuevaPassword);

                    try
                    {
                        db.SaveChanges();

                        return nuevaPassword;
                    }
                    catch (Exception ex)
                    {
                    }
                }

                return "";
            }
        }

        public static List<Usuarios> obtenerUsuariosPorRol(string codigoRol, int idUsuarioExceptuado = 0, int idFederacion = 0, int idAsociacion = 0)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                int idRol = RolesBL.obtener(codigoRol).id;

                var lista = from l in db.Usuarios
                                .Include("UsuariosRoles")
                                .Include("UsuariosFederaciones")
                                .Include("UsuariosAsociaciones")
                            where l.activo == true && l.UsuariosRoles.Any(x => x.idRol == idRol)
                            select l;

                //se verifica si hay un usuario que no hay que devolver
                if (idUsuarioExceptuado > 0)
                    lista = lista.Where(l => l.id != idUsuarioExceptuado);

                if (idFederacion > 0)
                    lista = lista.Where(l => l.UsuariosFederaciones.Any(x => x.idFederacion == idFederacion));

                if (idAsociacion > 0)
                    lista = lista.Where(l => l.UsuariosAsociaciones.Any(x => x.idAsociacion == idAsociacion));

                var lista2 = lista.ToList();

                return lista2;
            }
        }
        //Elimina un usuario si no contiene ninguna relacion 

        public static List<Usuarios> obtenerUsuairoSinRelaciones(int idUsuario)
        {
            using (DeportesEntities db = new DeportesEntities())
            {


                var listaUsuario = from l in db.Usuarios
                                   .Include("JugadoresPases")
                                   .Include("JugadoresPenalizaciones")
                                   .Include("JugadoresSolicitudesCarnets")
                                   .Include("JugadoresCategorias")
                                   .Include("JugadoresHabilitaciones")
                                   .Include("Noticias")
                                   .Include("Torneos")
                                   .Include("UsuariosAsociaciones")
                                   .Include("UsuariosFederaciones")
                            where l.id == idUsuario
                            && l.JugadoresHabilitaciones.Count < 1 && l.JugadoresPases.Count < 1 && l.JugadoresCategorias.Count < 1
                            && l.JugadoresPenalizaciones.Count < 1 && l.JugadoresSolicitudesCarnets.Count < 1 && l.Noticias.Count < 1
                            && l.Torneos.Count < 1

                            select l;

                var lista1 = listaUsuario.ToList();
                return lista1;
            }
        }

        public static bool borrarUsuario(int idUsuario)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var usr = obtenerUsuairoSinRelaciones(idUsuario);

                if (usr.Count > 0) { 

                    var usuario = from u in db.Usuarios
                        where u.id.Equals(idUsuario)
                        select u;

                    var usr2 = usuario.ToList();
                    db.Usuarios.Remove(usr2.FirstOrDefault());

                     try
                     {
                         db.SaveChanges();
                         return true;

                     }
                     catch (Exception)
                     {
                       
                         throw;
                     }
                      
                }
                
                return false;
            }
 
        }
    }
}
