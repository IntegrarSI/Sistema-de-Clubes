using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;



namespace CABW.Controllers
{
    public class UsuariosController : Controller
    {
        public ActionResult obtenerListado(string filter = "", int pageIndex = 1)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var lista = Negocio.UsuariosBL.obtenerListado(filter, idUsuario, pageIndex);

                 
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
                var instancia = Negocio.UsuariosBL.obtener(id);

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
        public ActionResult borrarUsuario(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {

                var resultado = new { success = "true", message = "" };
                var instancia = Negocio.UsuariosBL.borrarUsuario(id);

                if (instancia)
                {
                    resultado = new { success = "true", message = id.ToString() };


                }
                else
                {

                    resultado = new { success = "false", message = "No se puede eliminar el usuario. Por favor vuelva a intentar en unos minutos" };
                }
                return Json(resultado);
            }
            else {
                return unauthorized();
            }
            

        }

        [HttpPost]
        public ActionResult modificar(Negocio.Usuarios instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                if (instancia.id == 0)
                {
                    //es un alta
                    instancia.fechaAlta = DateTime.Now;
                    instancia.idUsuarioCreador = idUsuario;
                    instancia.password = Negocio.Utils.Encriptacion.encriptarSHA1(instancia.nombre.ToLower() + "1234");


                    int id = Negocio.UsuariosBL.crear(instancia);
                    if (id > 0)
                        resultado = new { success = "true", message = id.ToString() };
                    else
                        resultado = new { success = "false", message = "La creación no pudo ser realizada. Por favor vuelva a intentar en unos minutos" };
                }
                else
                {
                    //es una modificación
                    if (Negocio.UsuariosBL.actualizar(instancia))
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
        public ActionResult resetPassword(int id)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                var resultado = new { success = "true", message = "" };

                string nuevaPassword = Negocio.UsuariosBL.resetPassword(id);
                if (nuevaPassword != "")
                    resultado = new { success = "true", message = nuevaPassword };
                else
                    resultado = new { success = "false", message = "La contraseña no pudo ser reseteada. Por favor vuelva a intentar en unos minutos" };

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
