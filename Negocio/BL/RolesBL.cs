using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class RolesBL
    {
        public static ListaPaginada obtenerListado(string busqueda, int pageIndex = 1, int pageSize = 10)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Roles
                            select l;

                if (busqueda.Length > 0)
                    lista = lista.Where(l => l.descripcion.Contains(busqueda) || l.descripcionAmpliada.Contains(busqueda));

                ListaPaginada resultado = new ListaPaginada();
                resultado.totalItems = lista.Count();
                resultado.paginaActual = lista.OrderBy(x => x.id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return resultado;
            }
        }

        public static IEnumerable<Roles> obtenerLista()
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Roles
                            orderby l.id
                            select l;
                
                return lista.ToList();
            }
        }

        public static IEnumerable<Roles> obtenerFederacion()
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Roles
                            where l.codigoInterno=="asociacion"
                            select l;

                return lista.ToList();
            }
        }
        public static Negocio.Roles obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                return db.Roles.FirstOrDefault(x => x.id == id);
            }
        }

        public static Negocio.Roles obtener(string codigoInterno)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                return db.Roles.FirstOrDefault(x => x.codigoInterno == codigoInterno);
            }
        }
    }
}
