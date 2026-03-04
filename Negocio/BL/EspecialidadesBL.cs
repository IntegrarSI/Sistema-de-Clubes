using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class EspecialidadesBL
    {
        public static IEnumerable<Negocio.Especialidades> obtenerListado()
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Especialidades
                            where l.activo == true
                            select l;

                return lista.OrderBy(l => l.orden).ToList();
            }
        }
    }
}
