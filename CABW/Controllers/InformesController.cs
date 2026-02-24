using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CABW.Controllers
{
    public class InformesController : Controller
    {
        [HttpPost]
        public ActionResult carnetsNoEntregados(Negocio.InformesFiltros filtros)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.InformesBL.carnetsNoEntregados(filtros, idUsuario);

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult jugadoresHabilitados(Negocio.InformesFiltros filtros)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.InformesBL.jugadoresHabilitados(filtros, idUsuario);

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        //[HttpPost]
        //public ActionResult jugadoresHabilitadosExport(Negocio.InformesFiltros filtros)
        //{
        //    if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
        //    {
        //        int idUsuario = Convert.ToInt32(Session["idUsuario"]);

        //        var lista = Negocio.InformesBL.jugadoresHabilitadosExport(filtros, idUsuario);

        //        var json = JsonConvert.SerializeObject(lista,
        //            Formatting.None,
        //            new JsonSerializerSettings()
        //            {
        //                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        //            });

        //        return Content(json, "application/json");
        //    }
        //    else
        //        return unauthorized();
        //}
        [HttpPost]
        public ActionResult pases(Negocio.InformesFiltros filtros)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.InformesBL.pases(filtros, idUsuario);

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult cambiosDeCategoria(Negocio.InformesFiltros filtros)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.InformesBL.cambiosDeCategoria(filtros, idUsuario);

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult totalJugadores(Negocio.InformesFiltros filtros)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.InformesBL.totalJugadores(filtros, idUsuario);

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult guardar()
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.InformesBL.guardar();

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }


        [HttpPost]
        public ActionResult obtenerNroExportacion()
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.InformesBL.obtenerNroExportacion();

                var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Content(json, "application/json");
            }
            else
                return unauthorized();
        }





        private ActionResult unauthorized()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            HttpContext.Response.End();
            return Json("Unauthorized", JsonRequestBehavior.AllowGet);
        }
    }
}
