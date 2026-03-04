using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class LocalidadesBL
    {
        public static IEnumerable<Negocio.Localidades> obtenerListaAutocompletar(string filtro)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Localidades
                                .Include("Provincias")
                            where l.descripcion.Contains(filtro)
                            select l;

                return lista.OrderBy(x => x.descripcion).ToList();
            }
        }
    }
}
