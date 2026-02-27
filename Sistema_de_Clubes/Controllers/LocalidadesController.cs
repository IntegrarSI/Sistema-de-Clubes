using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CABW.Controllers
{
    public class LocalidadesController : Controller
    {
        public ActionResult obtenerListaAutocompletar(string filter)
        {
            var lista = Negocio.LocalidadesBL.obtenerListaAutocompletar(filter);

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
