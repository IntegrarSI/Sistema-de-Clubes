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
    public class FederacionesController : Controller
    {
        public ActionResult obtenerListado(string filter = "", int pageIndex = 1)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.FederacionesBL.obtenerListado(filter, idUsuario, pageIndex);

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

        public ActionResult obtenerListaAutocompletar(string filter)
        {
            var lista = Negocio.FederacionesBL.obtenerListaAutocompletar(filter);

            var json = JsonConvert.SerializeObject(lista,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult obtenerListaPorUsuario()
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.FederacionesBL.obtenerListaPorUsuario(idUsuario);

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
        public ActionResult obtenerLista()
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.FederacionesBL.obtenerLista();

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
        public ActionResult obtener(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                var instancia = Negocio.FederacionesBL.obtener(id);

                var json = JsonConvert.SerializeObject(instancia,
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
        public ActionResult modificar(Negocio.Federaciones instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                if (instancia.id == 0)
                {
                    //es un alta

                    int id = Negocio.FederacionesBL.crear(instancia);
                    if (id > 0)
                    {
                        resultado = new { success = "true", message = id.ToString() };
                        Negocio.EnviosMailsBL.notificacionNuevaFederacion(id, idUsuario);
                    }
                    else
                        resultado = new { success = "false", message = "La creación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                }
                else
                {
                    //es una modificación
                    if (Negocio.FederacionesBL.actualizar(instancia))
                        resultado = new { success = "true", message = instancia.id.ToString() };
                    else
                        resultado = new { success = "false", message = "La modificación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                }


                return Json(resultado);
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var now = DateTime.Now;
            string fileName = "";
            if (file.ContentLength > 0)
            {
                string timestamp = String.Format("{0}{1}{2}{3}{4}{5}{6}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                fileName = String.Format("{0}_{1}", timestamp, Path.GetFileName(file.FileName));
                var path = Path.Combine(Server.MapPath("~/Images/Federaciones/"), fileName);
                file.SaveAs(path);
            }
            return Json(new { success = true, fileName = fileName });
        }

        private ActionResult unauthorized()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            HttpContext.Response.End();
            return Json("Unauthorized", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult obtenerFederaciones()
        {
            var json = JsonConvert.SerializeObject(Negocio.FederacionesBL.obtenerFederaciones(),
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult obtenerAsociaciones(int id)
        {
            var json = JsonConvert.SerializeObject(Negocio.FederacionesBL.obtenerAsociaciones(id),
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult obtenerClubes(int id)
        {
            var json = JsonConvert.SerializeObject(Negocio.FederacionesBL.obtenerClubes(id),
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

            return Content(json, "application/json");
        }
    }
}
