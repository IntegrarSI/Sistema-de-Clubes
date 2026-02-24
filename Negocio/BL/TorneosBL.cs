using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class TorneosBL
    {
        public static IEnumerable<Negocio.Torneos> obtenerListado()
        {
            using (CABEntities db = new CABEntities())
            {
                var lista = from l in db.Torneos
                            where l.activo == true
                            select l;

                return lista.OrderBy(l => l.fechaDesde).ToList();
            }
        }

        public static IEnumerable<Negocio.Torneos> proximosEventos()
        {
            using (CABEntities db = new CABEntities())
            {
                DateTime fechaActual = DateTime.Now.Date;
                var lista = from l in db.Torneos
                            where l.activo == true && l.fechaHasta > fechaActual
                            select l;

                return lista.OrderBy(l => l.fechaDesde).Take(3).ToList();
            }
        }

        public static IEnumerable<Negocio.Torneos> ultimosEventos()
        {
            using (CABEntities db = new CABEntities())
            {
                DateTime fechaActual = DateTime.Now.Date;
                var lista = from l in db.Torneos
                            where l.activo == true && l.fechaDesde > fechaActual
                            select l;
                if (lista != null)
                {
                    return lista.OrderBy(l => l.fechaDesde).Take(3).ToList().OrderBy(l => l.fechaDesde);
                }
                else {
                    var lista2 = from l in db.Torneos
                                select l;

                    return lista2.OrderByDescending(l => l.fechaDesde).Take(3).ToList().OrderBy(l => l.fechaDesde);
                
                }
            }
        }

        public static ListaPaginada obtenerListadoAdmin(string busqueda, int pageIndex = 1, int pageSize = 10)
        {
            using (CABEntities db = new CABEntities())
            {
                var lista = from l in db.Torneos
                            select l;

                if (busqueda.Length > 0)
                    lista = lista.Where(l => l.tipoCompeticion.Contains(busqueda) || l.categoria.Contains(busqueda) || l.especialidad.Contains(busqueda));

                ListaPaginada resultado = new ListaPaginada();
                resultado.totalItems = lista.Count();
                resultado.paginaActual = lista.OrderByDescending(l => l.fechaDesde).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return resultado;
            }
        }

        public static Negocio.Torneos obtener(int id)
        {
            using (CABEntities db = new CABEntities())
            {
                return db.Torneos
                    .Include("TorneosActividades")
                    .Include("TorneosZonas")
                    .Include("TorneosEquipos")
                    .Include("TorneosPartidos")
                    .Include("TorneosEspecialidades")
                    .Include("TorneosEspecialidades.Especialidades")
                    .Include("TorneosPartidos.TorneosEquipos")
                    .Include("TorneosPartidos.TorneosEquipos1").AsNoTracking().FirstOrDefault(x => x.id == id);
            }
        }

        public static int crear(Negocio.Torneos instancia)
        {
            using (CABEntities db = new CABEntities())
            {
                db.Torneos.Add(instancia);

                try
                {
                    db.SaveChanges();
                    return instancia.id;
                }
                catch (Exception ex)
                {
                    
                }
            }

            return 0;
        }

        public static bool actualizar(Negocio.Torneos instancia)
        {
            using (CABEntities db = new CABEntities())
            {
                var objeto = from u in db.Torneos
                             where u.id == instancia.id
                             select u;

                Negocio.Torneos nuevoObjeto = objeto.Single();
                if (nuevoObjeto != null)
                {
                    nuevoObjeto.tipoCompeticion = instancia.tipoCompeticion;
                    nuevoObjeto.fechaDesde = instancia.fechaDesde;
                    nuevoObjeto.fechaHasta = instancia.fechaHasta;
                    nuevoObjeto.categoria = instancia.categoria;
                    nuevoObjeto.especialidad = instancia.especialidad;
                    nuevoObjeto.localidad = instancia.localidad;
                    nuevoObjeto.provincia = instancia.provincia;
                    nuevoObjeto.activo = instancia.activo;

                    //detalle
                    nuevoObjeto.detalleMostrar = instancia.detalleMostrar;
                    nuevoObjeto.detalleTitulo = instancia.detalleTitulo;
                    nuevoObjeto.detalleDescripcion = instancia.detalleDescripcion;
                    nuevoObjeto.detalleJueces = instancia.detalleJueces;
                    nuevoObjeto.detalleMostrarAdvertenciaDocumentacion = instancia.detalleMostrarAdvertenciaDocumentacion;
                    nuevoObjeto.detalleMostrarPorgramaActividades = instancia.detalleMostrarPorgramaActividades;
                    nuevoObjeto.lugar = instancia.lugar;
                    nuevoObjeto.direccion = instancia.direccion;
                    nuevoObjeto.telefono = instancia.telefono;
                    nuevoObjeto.urlMaps = instancia.urlMaps;
                    
                    nuevoObjeto.resultadosMostrar = instancia.resultadosMostrar;
                    nuevoObjeto.resultadosFotosFacebook = instancia.resultadosFotosFacebook;
                    nuevoObjeto.resultadosTitulo = instancia.resultadosTitulo;
                    nuevoObjeto.resultadosDescripcion = instancia.resultadosDescripcion;

                    //se eliminan las actividades y se vuelven a crear
                    foreach (var clon in db.TorneosActividades.Where(x => x.idTorneo == instancia.id).ToList())
                        db.Entry(clon).State = EntityState.Deleted;

                    if (instancia.TorneosActividades != null && instancia.TorneosActividades.Count > 0)
                    {
                        Negocio.TorneosActividades relacion;
                        foreach (var item in instancia.TorneosActividades)
                        {
                            relacion = new TorneosActividades();
                            relacion.idTorneo = instancia.id;
                            relacion.fecha = item.fecha;
                            relacion.hora = item.hora;
                            relacion.minutos = item.minutos;
                            relacion.descripcion = item.descripcion;
                            db.TorneosActividades.Add(relacion);
                        }
                    }

                    //se eliminan los partidos, zonas y equipos del torneo y se vuelven a crear
                    foreach (var clon in db.TorneosPartidos.Where(x => x.idTorneo == instancia.id).ToList())
                        db.Entry(clon).State = EntityState.Deleted;
                    foreach (var clon in db.TorneosEquipos.Where(x => x.idTorneo == instancia.id).ToList())
                        db.Entry(clon).State = EntityState.Deleted;
                    foreach (var clon in db.TorneosZonas.Where(x => x.idTorneo == instancia.id).ToList())
                        db.Entry(clon).State = EntityState.Deleted;
                    /*foreach (var clon in db.TorneosEspecialidades.Where(x => x.idTorneo == instancia.id).ToList())
                        db.Entry(clon).State = EntityState.Deleted;*/

                    if (instancia.TorneosZonas != null && instancia.TorneosZonas.Count > 0)
                    {
                        Negocio.TorneosZonas relacion;
                        foreach (var item in instancia.TorneosZonas)
                        {
                            relacion = new TorneosZonas();
                            relacion.idTorneo = instancia.id;
                            relacion.descripcion = item.descripcion;
                            relacion.idEspecialidad = item.idEspecialidad;
                            db.TorneosZonas.Add(relacion);
                        }
                    }

                    if (instancia.TorneosEquipos != null && instancia.TorneosEquipos.Count > 0)
                    {
                        Negocio.TorneosEquipos relacion;
                        foreach (var item in instancia.TorneosEquipos)
                        {
                            relacion = new TorneosEquipos();
                            relacion.idTorneo = instancia.id;
                            relacion.nombre = item.nombre;
                            relacion.tecnico = item.tecnico;
                            relacion.delegado = item.delegado;
                            relacion.jugadores = item.jugadores;
                            relacion.idEquipo = item.idEquipo;
                            relacion.zona = item.zona;
                            relacion.nombreCorto = item.nombreCorto;
                            relacion.idEspecialidad = item.idEspecialidad;
                            db.TorneosEquipos.Add(relacion);
                        }
                    }

                    /*if (instancia.TorneosEspecialidades != null && instancia.TorneosEspecialidades.Count > 0)
                    {
                        Negocio.TorneosEspecialidades relacion;
                        foreach (var item in instancia.TorneosEspecialidades)
                        {
                            relacion = new TorneosEspecialidades();
                            relacion.idTorneo = instancia.id;
                            relacion.idEspecialidad = item.idEspecialidad;
                            db.TorneosEspecialidades.Add(relacion);
                        }
                    }*/

                    try
                    {
                        db.SaveChanges();

                        //ejecutar un SP que actualice los datos de los equipos que se hayan buscado

                        if (instancia.TorneosEquipos != null && instancia.TorneosEquipos.Count > 0)
                        {
                            Negocio.TorneosPartidos relacion;
                            foreach (var item in instancia.TorneosPartidos)
                            {
                                relacion = new TorneosPartidos();
                                relacion.idTorneo = instancia.id;
                                relacion.idTorneoEquipo1 = db.TorneosEquipos.FirstOrDefault(x => x.idTorneo == instancia.id && x.nombre == item.TorneosEquipos.nombre).id;
                                relacion.puntosEquipo1 = item.puntosEquipo1;
                                relacion.idTorneoEquipo2 = db.TorneosEquipos.FirstOrDefault(x => x.idTorneo == instancia.id && x.nombre == item.TorneosEquipos1.nombre).id;
                                relacion.puntosEquipo2 = item.puntosEquipo2;
                                relacion.fase = item.fase;
                                relacion.idEspecialidad = item.idEspecialidad;
                                db.TorneosPartidos.Add(relacion);
                            }
                        }

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
