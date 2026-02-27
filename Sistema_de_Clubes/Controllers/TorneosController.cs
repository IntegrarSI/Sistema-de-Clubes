using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CABW.Controllers
{
    public class TorneosController : Controller
    {
        public ActionResult obtenerListadoAdmin(string filter = "", int pageIndex = 1)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                var lista = Negocio.TorneosBL.obtenerListadoAdmin(filter, pageIndex);

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

        public ActionResult obtenerListado()
        {
            var lista = Negocio.TorneosBL.obtenerListado();

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        public ActionResult proximosEventos()
        {
            var lista = Negocio.TorneosBL.proximosEventos();

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        public ActionResult ultimosEventos()
        {
            var lista = Negocio.TorneosBL.ultimosEventos();

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult obtener(int id)
        {
            var instancia = Negocio.TorneosBL.obtener(id);

            var json = JsonConvert.SerializeObject(instancia,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult obtenerAdmin(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                var instancia = Negocio.TorneosBL.obtener(id);

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
        public ActionResult modificar(Negocio.Torneos instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                instancia.idUsuarioCreador = idUsuario;

                if (instancia.id == 0)
                {
                    //es un alta
                    int id = Negocio.TorneosBL.crear(instancia);
                    if (id > 0)
                        resultado = new { success = "true", message = id.ToString() };
                    else
                        resultado = new { success = "false", message = "La creación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                }
                else
                {
                    //es una modificación
                    if (Negocio.TorneosBL.actualizar(instancia))
                        resultado = new { success = "true", message = instancia.id.ToString() };
                    else
                        resultado = new { success = "false", message = "La modificación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                }


                return Json(resultado);
            }
            else
                return unauthorized();
        }

        public ActionResult obtenerListaEspecialidades()
        {
            var lista = Negocio.EspecialidadesBL.obtenerListado();

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        private ActionResult unauthorized()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            HttpContext.Response.End();
            return Json("Unauthorized", JsonRequestBehavior.AllowGet);
        }
    }
}
