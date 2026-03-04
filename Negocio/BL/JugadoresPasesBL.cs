using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class JugadoresPasesBL
    {
        public static Negocio.JugadoresPases obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var instancia = db.JugadoresPases
                                .Include("Jugadores")
                                .Include("Jugadores.Localidades")
                                .Include("Jugadores.Localidades.Provincias")
                                .Include("Jugadores.Categorias")
                                .Include("Federaciones")
                                .Include("Federaciones1")
                                .Include("Asociaciones")
                                .Include("Asociaciones1")
                                .Include("Clubes")
                                .Include("Clubes1")
                                .Include("Categorias")
                                .AsNoTracking()
                                .FirstOrDefault(x => x.id == id);

                return instancia;
            }
        }
    }
}
