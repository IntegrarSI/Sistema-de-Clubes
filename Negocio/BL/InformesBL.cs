using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public partial class InformesBL
    {
        public static DataTable carnetsNoEntregados(InformesFiltros filtros, int idUsuario)
        {
            using (SqlCommand comando = new SqlCommand())
            {
                StringBuilder Sql = new StringBuilder();

                Sql.Append("exec uspInformes_CarnetsNoEntregados @fechaSolicitudDesde, @fechaSolicitudHasta, @filtro, @idUsuario");

                if (filtros.filtro == null)
                    filtros.filtro = "";

                comando.Parameters.Add(new SqlParameter("fechaSolicitudDesde", SqlDbType.Date)).Value = filtros.fechaDesde;
                comando.Parameters.Add(new SqlParameter("fechaSolicitudHasta", SqlDbType.Date)).Value = filtros.fechaHasta;
                comando.Parameters.Add(new SqlParameter("filtro", SqlDbType.VarChar)).Value = "%" + filtros.filtro.ToString() + "%";
                comando.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.Int)).Value = idUsuario;

                comando.CommandText = Sql.ToString();

                return Integrar.Data.Sql.EjecutarConsulta(comando);
            }
        }

        public static DataTable jugadoresHabilitados(InformesFiltros filtros, int idUsuario)
        {
            using (SqlCommand comando = new SqlCommand())
            {
                StringBuilder Sql = new StringBuilder();

                Sql.Append("exec uspInformes_JugadoresHabilitados @filtro, @idUsuario, @idFederacion, @idAsociacion, @idClub, @estampilla");

                if (filtros.filtro == null)
                    filtros.filtro = "";

                comando.Parameters.Add(new SqlParameter("filtro", SqlDbType.VarChar)).Value = "%" + filtros.filtro.ToString() + "%";
                comando.Parameters.Add(new SqlParameter("idFederacion", SqlDbType.Int)).Value = filtros.federacion != null ? filtros.federacion.id : 0;
                comando.Parameters.Add(new SqlParameter("idAsociacion", SqlDbType.Int)).Value = filtros.asociacion != null ? filtros.asociacion.id : 0;
                comando.Parameters.Add(new SqlParameter("idClub", SqlDbType.Int)).Value = filtros.club != null ? filtros.club.id : 0;
                comando.Parameters.Add(new SqlParameter("estampilla", SqlDbType.Int)).Value = filtros.estampilla;
                comando.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.Int)).Value = idUsuario;


                comando.CommandText = Sql.ToString();

                return Integrar.Data.Sql.EjecutarConsulta(comando);
            }
        }

        public static DataTable pases(InformesFiltros filtros, int idUsuario)
        {
            using (SqlCommand comando = new SqlCommand())
            {
                StringBuilder Sql = new StringBuilder();

                Sql.Append("exec uspInformes_pases @idUsuario, @filtro, @fechaDesde, @fechaHasta, @idFederacion, @idAsociacion,@tipoPase");

                if (filtros.filtro == null)
                    filtros.filtro = "";

            

                comando.Parameters.Add(new SqlParameter("filtro", SqlDbType.VarChar)).Value = "%" + filtros.filtro.ToString() + "%";
                comando.Parameters.Add(new SqlParameter("idFederacion", SqlDbType.Int)).Value = filtros.federacion != null ? filtros.federacion.id : 0;
                comando.Parameters.Add(new SqlParameter("idAsociacion", SqlDbType.Int)).Value = filtros.asociacion != null ? filtros.asociacion.id : 0;
                comando.Parameters.Add(new SqlParameter("fechaDesde", SqlDbType.Date)).Value = filtros.fechaDesde;
                comando.Parameters.Add(new SqlParameter("fechaHasta", SqlDbType.Date)).Value = filtros.fechaHasta;
                comando.Parameters.Add(new SqlParameter("tipoPase", SqlDbType.VarChar)).Value=   filtros.tipoPase != null ? filtros.tipoPase.label.ToString() : "" ;
                comando.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.Int)).Value = idUsuario;


                comando.CommandText = Sql.ToString();

                return Integrar.Data.Sql.EjecutarConsulta(comando);
            }
        }

        public static DataTable cambiosDeCategoria(InformesFiltros filtros, int idUsuario)
        {
            using (SqlCommand comando = new SqlCommand())
            {
                StringBuilder Sql = new StringBuilder();

                Sql.Append("exec uspInformes_CambiosCategoria @idUsuario, @filtro, @fechaDesde, @fechaHasta, @idFederacion, @idAsociacion");

                if (filtros.filtro == null)
                    filtros.filtro = "";

                comando.Parameters.Add(new SqlParameter("filtro", SqlDbType.VarChar)).Value = "%" + filtros.filtro.ToString() + "%";
                comando.Parameters.Add(new SqlParameter("idFederacion", SqlDbType.Int)).Value = filtros.federacion != null ? filtros.federacion.id : 0;
                comando.Parameters.Add(new SqlParameter("idAsociacion", SqlDbType.Int)).Value = filtros.asociacion != null ? filtros.asociacion.id : 0;
                comando.Parameters.Add(new SqlParameter("fechaDesde", SqlDbType.Date)).Value = filtros.fechaDesde;
                comando.Parameters.Add(new SqlParameter("fechaHasta", SqlDbType.Date)).Value = filtros.fechaHasta;

                comando.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.Int)).Value = idUsuario;


                comando.CommandText = Sql.ToString();

                return Integrar.Data.Sql.EjecutarConsulta(comando);
            }
        }
        //public static bool jugadoresHabilitadosExport(InformesFiltros filtros, int idUsuario)
        //{
        //    using (SqlCommand comando = new SqlCommand())
        //    {
        //        StringBuilder Sql = new StringBuilder();

        //        Sql.Append("exec uspInformes_JugadoresHabilitados @filtro, @idUsuario, @idFederacion, @idAsociacion, @idClub, @estampilla");

        //        if (filtros.filtro == null)
        //            filtros.filtro = "";

        //        comando.Parameters.Add(new SqlParameter("filtro", SqlDbType.VarChar)).Value = "%" + filtros.filtro.ToString() + "%";
        //        comando.Parameters.Add(new SqlParameter("idFederacion", SqlDbType.Int)).Value = filtros.federacion != null ? filtros.federacion.id : 0;
        //        comando.Parameters.Add(new SqlParameter("idAsociacion", SqlDbType.Int)).Value = filtros.asociacion != null ? filtros.asociacion.id : 0;
        //        comando.Parameters.Add(new SqlParameter("idClub", SqlDbType.Int)).Value = filtros.club != null ? filtros.club.id : 0;
        //        comando.Parameters.Add(new SqlParameter("estampilla", SqlDbType.Int)).Value = filtros.estampilla;
        //        comando.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.Int)).Value = idUsuario;


        //        comando.CommandText = Sql.ToString();

        //        DataTable dtexport = new DataTable();
        //        dtexport = Integrar.Data.Sql.EjecutarConsulta(comando);
        //        //ExportarExcel ex = new ExportarExcel();
        //        //ex.exportarArchivoExcel(dtexport);
        //        DLLExportar.ExportarExcel.exportarArchivoExcel(dtexport);

        //        return true;

        //        //return Integrar.Data.Sql.EjecutarConsulta(comando);
        //    }
        //}

        public static DataTable totalJugadores(InformesFiltros filtros, int idUsuario)
        {
            using (SqlCommand comando = new SqlCommand())
            {
                StringBuilder Sql = new StringBuilder();

                Sql.Append("exec uspInformes_JugadoresTotales @filtro, @idUsuario, @idFederacion, @idAsociacion, @idClub, @habilitado");

                if (filtros.filtro == null)
                    filtros.filtro = "";

                comando.Parameters.Add(new SqlParameter("filtro", SqlDbType.VarChar)).Value = "%" + filtros.filtro.ToString() + "%";
                comando.Parameters.Add(new SqlParameter("idFederacion", SqlDbType.Int)).Value = filtros.federacion != null ? filtros.federacion.id : 0;
                comando.Parameters.Add(new SqlParameter("idAsociacion", SqlDbType.Int)).Value = filtros.asociacion != null ? filtros.asociacion.id : 0;
                comando.Parameters.Add(new SqlParameter("idClub", SqlDbType.Int)).Value = filtros.club != null ? filtros.club.id : 0;
                comando.Parameters.Add(new SqlParameter("habilitado", SqlDbType.Bit)).Value = filtros.habilitado;
                comando.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.Int)).Value = idUsuario;

                comando.CommandText = Sql.ToString();

                return Integrar.Data.Sql.EjecutarConsulta(comando);
            }
        }
      
        public static int guardar()
        {
            using (CABEntities db = new CABEntities())
            {
                var instanciaaux = new ExportacionExel();
                var exp = from l in db.ExportacionExel
                          select l;
                var instancia = exp.OrderByDescending(x => x.id).FirstOrDefault();
                if (instancia != null)
                {
                    instanciaaux.id = instancia.id + 1;
                    instanciaaux.fechaImpresion = DateTime.Now.Date;  
                }
                else {
                    instanciaaux.id = 1;
                    instanciaaux.fechaImpresion = DateTime.Now.Date;
                }
                db.ExportacionExel.Add(instanciaaux);
                try
                {
                    db.SaveChanges();
                    return instanciaaux.id + 1 ;
                }
                catch (Exception ex)
                {
                }
            }

            return 0;
        }
        public static int obtenerNroExportacion()
        {
            using (CABEntities db = new CABEntities())
            {
                var instanciaaux = new ExportacionExel();
                var exp = from l in db.ExportacionExel
                          select l;
                var lista = exp.OrderByDescending(x => x.id).FirstOrDefault();

                if (lista != null)
                {
                    return lista.id;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
