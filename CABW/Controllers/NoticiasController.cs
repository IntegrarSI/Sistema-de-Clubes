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
    public class NoticiasController : Controller
    {
        public ActionResult obtenerListado(string filter = "", int pageIndex = 1)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                var lista = Negocio.NoticiasBL.obtenerListado(filter, idUsuario, pageIndex);

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



        public ActionResult obtenerTodas(int pageIndex = 1, int pageSize=3)
        {
            var lista = Negocio.NoticiasBL.obtenerTodas(pageIndex, pageSize);

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        public ActionResult obtenerNoticia(int id)
        {
            var lista = Negocio.NoticiasBL.obtener(id);

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult obtenerUrl()
        {

            string str = System.Configuration.ConfigurationManager.AppSettings["obtenerUrl"];

            var json = JsonConvert.SerializeObject(str,
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
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                var instancia = Negocio.NoticiasBL.obtener(id);

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
        public ActionResult modificar(Negocio.Noticias instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };
                
                instancia.idUsuario = idUsuario;
               
                if (instancia.id == 0)
                {
                    //es un alta
                    instancia.fechaCreacion = instancia.fecha;
                    int id = Negocio.NoticiasBL.crear(instancia);
                    if (id > 0)
                    {
                        resultado = new { success = "true", message = id.ToString() };
                    }
                    else
                        resultado = new { success = "false", message = "La creación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                }
                else
                {
                    //es una modificación
                    if (Negocio.NoticiasBL.actualizar(instancia))
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
                var path = Path.Combine(Server.MapPath("~/Images/noticias/"), fileName);
                file.SaveAs(path);
            }
            return Json(new { success = true, fileName = fileName });
        }


        public ActionResult obtenerDestacadas()
        {
            var lista = Negocio.NoticiasBL.obtenerDestacadas();

            var json = JsonConvert.SerializeObject(lista,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
    
        }

        public ActionResult unauthorized()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            HttpContext.Response.End();
            return Json("Unauthorized", JsonRequestBehavior.AllowGet);
        }

    }
}
