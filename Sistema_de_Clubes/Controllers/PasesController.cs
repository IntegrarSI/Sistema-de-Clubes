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
    public class PasesController : Controller
    {
        public ActionResult obtenerListadoPendientes()
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.PasesBL.obtenerListadoPendientes(idUsuario, 1, 100);

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
        public ActionResult aprobarPase(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                if (Negocio.PasesBL.aprobarPase(id, idUsuario))
                {
                    resultado = new { success = "true", message = id.ToString() };
                    Negocio.EnviosMailsBL.notificacionPaseAprobado(id, idUsuario);
                }
                else
                    resultado = new { success = "false", message = "Ocurrió un error al intentar aprobar el pase. Por favor vuelva a intentar en unos minutos" };

                return Json(resultado);
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult revisarPase(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                if (Negocio.PasesBL.revisarPase(id, idUsuario))
                {
                    resultado = new { success = "true", message = id.ToString() };
                    Negocio.EnviosMailsBL.notificacionPaseAprobado(id, idUsuario);
                }
                else
                    resultado = new { success = "false", message = "Ocurrió un error al intentar aprobar el pase. Por favor vuelva a intentar en unos minutos" };

                return Json(resultado);
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult rechazarPase(int id, string motivos)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                if (Negocio.PasesBL.rechazarPase(id, motivos, idUsuario))
                {
                    resultado = new { success = "true", message = id.ToString() };
                    Negocio.EnviosMailsBL.notificacionPaseRechazado(id, idUsuario, motivos);
                }
                else
                    resultado = new { success = "false", message = "Ocurrió un error al intentar rechazar el pase. Por favor vuelva a intentar en unos minutos" };

                return Json(resultado);
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
