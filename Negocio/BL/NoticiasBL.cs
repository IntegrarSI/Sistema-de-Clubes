using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class NoticiasBL
    {

        public static ListaPaginada obtenerListado(string busqueda, int idUsuario, int pageIndex = 1, int pageSize = 10)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                Negocio.Usuarios usuario = UsuariosBL.obtener(idUsuario);

                ListaPaginada resultado = new ListaPaginada();
                if (usuario.UsuariosRoles.Any(x => x.Roles.administraContenido == true || x.Roles.administraConfederacion == true))
                {
                    var lista = from l in db.Noticias
                                    .Include("Usuarios")
                                select l;

                    if (busqueda.Length > 0)
                        lista = lista.Where(l => l.titulo.Contains(busqueda) || l.copete.Contains(busqueda) || l.texto.Contains(busqueda));


                    resultado.totalItems = lista.Count();
                    resultado.paginaActual = lista.OrderBy(l => l.fecha).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return resultado;
            }

        }

        public static ListaPaginada obtenerTodas(int pageIndex = 1, int pageSize = 3)
        {

            using (DeportesEntities db = new DeportesEntities())
            {
                ListaPaginada resultado = new ListaPaginada();

                var lista = from l in db.Noticias
                            select l;

                resultado.totalItems = lista.Count();
                resultado.paginaActual = lista.OrderByDescending(l => l.fecha).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                
                return resultado;
            }

        }


        public static Negocio.Noticias obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                return db.Noticias
                    .Include("Usuarios")
                    .FirstOrDefault(x => x.id == id);
            }
        }


        public static int crear(Negocio.Noticias instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                db.Noticias.Add(instancia);

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



        public static bool actualizar(Negocio.Noticias instancia)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var objeto = from u in db.Noticias
                             where u.id == instancia.id
                             select u;

                Negocio.Noticias nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {

                    nuevoObjeto.fecha = instancia.fecha;
                    nuevoObjeto.titulo = instancia.titulo;
                    nuevoObjeto.copete = instancia.copete;
                    nuevoObjeto.fotoPeque = instancia.fotoPeque;
                    nuevoObjeto.destacada = instancia.destacada;
                    nuevoObjeto.texto = instancia.texto;
                    //nuevoObjeto.fechaCreacion = instancia.fechaCreacion;
                    nuevoObjeto.idUsuario = instancia.idUsuario;
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

        public static ListaPaginada obtenerDestacadas()
        {
            using (DeportesEntities db = new DeportesEntities())
            {

                ListaPaginada resultado = new ListaPaginada();
                var lista = from l in db.Noticias
                            where l.destacada == true
                            select l;

                    
                resultado.totalItems = lista.Count();
                resultado.paginaActual = lista.OrderByDescending(l => l.fecha).Take(4).ToList();
                

                return resultado;
            }

        }

    }
}
