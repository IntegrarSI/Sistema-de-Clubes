using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace CABW.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public ActionResult Login(string nombreUsuario, string password)
        {
            Negocio.LoginTicket ticket = new Negocio.LoginTicket();
            ticket.passwordIncorrecta = true;
            ticket.nombreUsuario = "";

            bool passwordCorrecta = false;
            Negocio.Usuarios usuario = Negocio.UsuariosBL.verificarLogin(nombreUsuario, password, out passwordCorrecta);
            if (usuario != null)
            {
                ticket.id = usuario.id;
                ticket.roles = usuario.UsuariosRoles.Select(p => p.Roles.codigoInterno).ToArray();
                ticket.nombre = usuario.nombre;
                ticket.nombreUsuario = usuario.nombreUsuario;
                ticket.passwordIncorrecta = !passwordCorrecta;
                ticket.activo = usuario.activo;

                if (usuario.UsuariosRoles != null && usuario.UsuariosRoles.Any(x => x.Roles.codigoInterno == "administrador"))
                    ticket.administrador = true;
                else
                    ticket.administrador = false;

                if (usuario.UsuariosRoles != null && usuario.UsuariosRoles.Any(x => x.Roles.administraContenido == true))
                    ticket.administraContenido = true;
                else
                    ticket.administraContenido = false;

                if (usuario.UsuariosRoles != null && usuario.UsuariosRoles.Any(x => x.Roles.administraSeguridad == true))
                    ticket.administraSeguridad = true;
                else
                    ticket.administraSeguridad = false;

                if (usuario.UsuariosRoles != null && usuario.UsuariosRoles.Any(x => x.Roles.administraConfederacion == true))
                {
                    ticket.administraConfederacion = true;
                    ticket.nivel = usuario.UsuariosRoles.Where(x => x.Roles.administraConfederacion == true).FirstOrDefault().Roles.nivel;
                }
                else
                    ticket.administraConfederacion = false;

                if (passwordCorrecta)
                {
                    Session["idUsuario"] = usuario.id;
                    Session["ticket"] = ticket;

                    var cookie = new HttpCookie("_authtoken", JsonConvert.SerializeObject(ticket.nombreUsuario));
                    cookie.Expires = DateTime.Now.AddMinutes(60);
                    this.Response.SetCookie(cookie);
                }
            }

            var json = JsonConvert.SerializeObject(ticket,
                  Formatting.None,
                  new JsonSerializerSettings()
                  {
                      ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                  });

            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult CambiarPassword(Negocio.CambiarPasswordActual instancia)
        {
            if (Negocio.Utils.Seguridad.validarJsonSession(Session, new string[] { "idUsuario" }))
            {
                Negocio.LoginTicket ticket = (Negocio.LoginTicket)Session["ticket"];

                var resultado = new { success = "true", message = "" };

                //se actualiza la password
                if (instancia.PasswordNueva == instancia.ConfirmacionPassword)
                {
                    bool passwordCorrecta = false;
                    Negocio.Usuarios usuario = Negocio.UsuariosBL.verificarLogin(ticket.nombreUsuario, instancia.PasswordActual, out passwordCorrecta);
                    if (usuario != null && passwordCorrecta)
                    {
                        if (!Negocio.UsuariosBL.cambiarPassword(ticket.nombreUsuario, Integrar.Encriptar.EncriptarSHA1(instancia.PasswordNueva)))
                            resultado = new { success = "false", message = "Ocurrió un error y la contraseña no pudo ser cambiada" };
                    }
                    else
                        resultado = new { success = "false", message = "La contraseña actual no es correcta" };
                }
                else
                    resultado = new { success = "false", message = "Las contraseñas no coinciden" };

                return Json(resultado);
            }
            else
                return unauthorized();
        }

        [HttpPost]
        public ActionResult RecuperarPassword(string nombreUsuario, string recaptchaResponse)
        {
            var resultado = new { success = "true", message = "" };

            //se valida el recaptcha
            //if (!Negocio.Utils.Seguridad.validarRecaptcha(recaptchaResponse))
            //    resultado = new { success = "false", message = "Recaptcha no válido" };
            //else
            //{
                //se verifica que exista el usuario
                bool passwordCorrecta = false;
                Negocio.Usuarios usuario = Negocio.UsuariosBL.verificarLogin(nombreUsuario, "", out passwordCorrecta);
                if (usuario != null)
                {
                    string nuevaPassword = generarPassword(nombreUsuario);
                    if (Negocio.UsuariosBL.cambiarPassword(nombreUsuario, Integrar.Encriptar.EncriptarSHA1(nuevaPassword)))
                    {
                        //se envía el mail con la nueva clave.
                        MailMessage mensaje = new MailMessage();
                        mensaje.From = new MailAddress(ConfigurationManager.AppSettings["RecuperarPasswordFrom"].ToString());
                        mensaje.To.Add(new MailAddress(usuario.mail));
                        mensaje.Subject = ConfigurationManager.AppSettings["RecuperarPasswordSubject"].ToString();
                        mensaje.IsBodyHtml = true;
                        mensaje.Body = String.Format(@"
                        <p>Nueva contraseña: {0}</p>", nuevaPassword);

                        Integrar.Mail.Mail.EnviarMail(mensaje);

                        resultado = new { success = "true", message = "Su nueva contraseña fue enviada a la siguiente dirección de correo electrónico: " + usuario.mail };
                    }
                    else
                        resultado = new { success = "false", message = "Ha ocurrido un problema al intentar modificar la contraseña" };
                }
                else
                    resultado = new { success = "false", message = "El nombre de usuario es incorrecto" };
            //}

            return Json(resultado);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            this.Response.Cookies.Remove("_authtoken");
            Session["idUsuario"] = "";
            Session["ticket"] = "";
            return this.Redirect("/#/login");
        }

        private ActionResult unauthorized()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            HttpContext.Response.End();
            return Json("Unauthorized", JsonRequestBehavior.AllowGet);
        }

        private string generarPassword(string nombreUsuario)
        {
            string caracteres = "0123456789";
            Random rd = new Random(DateTime.Now.Millisecond);
            string password = nombreUsuario.ToLower();

            for (int i = 0; i < 6; i++)
            {
                password += caracteres[rd.Next(10)].ToString();
            }

            return password;
        }
    }
}
