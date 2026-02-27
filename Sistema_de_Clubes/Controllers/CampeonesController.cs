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
    public class CampeonesController : Controller
    {
        public ActionResult obtenerListado(int pageIndex = 1, int pageSize = 3, string pTorneo = "")
        {
            var lista = Negocio.CampeonesBL.obtenerListado(pageIndex, pageSize, pTorneo);
            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        public ActionResult obtenerUltimos()
        {
            var lista = Negocio.CampeonesBL.obtenerUltimos();
            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }
    }
}

