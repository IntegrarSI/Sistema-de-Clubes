using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class JugadoresPenalizacionesBL
    {
        public static Negocio.JugadoresPenalizaciones obtener(int id)
        {
            using (DeportesEntities db = new DeportesEntities())
            {
                var instancia = db.JugadoresPenalizaciones
                                .Include("Usuarios")
                                .Include("Jugadores")
                                .Include("Jugadores.Localidades")
                                .Include("Jugadores.Localidades.Provincias")
                                .Include("Jugadores.Categorias")
                                .Include("Jugadores.Clubes")
                                .Include("Jugadores.Clubes.Asociaciones")
                                .Include("Jugadores.Clubes.Asociaciones.Federaciones")
                                .AsNoTracking()
                                .FirstOrDefault(x => x.id == id);

                return instancia;
            }
        }
    }
}
