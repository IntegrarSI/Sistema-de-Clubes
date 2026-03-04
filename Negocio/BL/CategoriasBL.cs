using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class CategoriasBL
    {
        public static IEnumerable<Negocio.Categorias> obtenerListado()
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var lista = from l in db.Categorias
                            where l.activa == true
                            select l;

                return lista.ToList();
            }
        }
    }
}
