using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class JugadoresHabilitacionesBL
    {
        public static bool existeEstampilla(int numero, DateTime fechaHabilitacion, DateTime fechaVencimiento, string categoria, int idJugador)
        {
            using (CABEntities db = new CABEntities())
            {
                var habilitacion = db.JugadoresHabilitaciones.Count(x => x.numero == numero && x.idJugador == idJugador);           
                if(habilitacion > 0){
                    return false;
                }
                else{
                    return db.JugadoresHabilitaciones.Count(x => x.numero == numero && x.fechaHabilitacion.Year >= fechaHabilitacion.Year && x.fechaInhabilitacion.Year <= fechaVencimiento.Year && x.categoria == categoria) > 0;
                }
            }
        }

        public static bool existeEstampilla(int numero, DateTime fechaHabilitacion, DateTime fechaVencimiento, string categoria)
        {
            using (CABEntities db = new CABEntities())
            {
                return db.JugadoresHabilitaciones.Count(x => x.numero == numero && x.fechaHabilitacion.Year >= fechaHabilitacion.Year && x.fechaInhabilitacion.Year <= fechaVencimiento.Year && x.categoria == categoria) > 0;
            }
        }

        public static bool eliminar(int pId, int idUsuario)
        {
            using (CABEntities db = new CABEntities())
            {
                var entity = db.JugadoresHabilitaciones.FirstOrDefault(p => p.id == pId);
                db.JugadoresHabilitaciones.Remove(entity);

                // Guardar historial
                var historial = new JugadoresHistorial();
                historial.idJugador = entity.idJugador;
                historial.idUsuario = idUsuario;
                historial.fecha = DateTime.Now;
                historial.accion = "Eliminación Estampilla";

                db.JugadoresHistorial.Add(historial);
                db.SaveChanges();

                return true;
            }
        }

    }
}
