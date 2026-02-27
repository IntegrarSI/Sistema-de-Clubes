using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CABW.Controllers
{
    public class SharedController : Controller
    {

        [HttpPost]
        public ActionResult sendMail(Negocio.Mail instancia)
        {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(instancia.mail);

            string[] mailsTo = System.Configuration.ConfigurationManager.AppSettings["contactoTO"].ToString().Split(';');
            foreach (string item in mailsTo)
                mensaje.To.Add(new MailAddress(item));

            mensaje.Subject = "Contacto desde la web de " + instancia.nombre;
            mensaje.IsBodyHtml = true;
            mensaje.Body = String.Format(@"
                <ul>
                    <li><strong>Asuento: </strong></li>{0}
                    <li><strong>Nombre: </strong></li>{1}
                    <li><strong>Localidad: </strong></li>{2}
                    <li><strong>Provincia: </strong></li>{3}
                    <li><strong>Correo electrónico: </strong></li>{4}
                    <li><strong>Mensaje: </strong></li>{5}
                </ul>
            ", instancia.asunto, instancia.nombre, instancia.localidad, instancia.provincia, instancia.mail, instancia.mensaje.Replace("\n", "<br/>"));

            Integrar.Mail.Mail.EnviarMail(mensaje);


            return Json(new { success = "true", message = "" });
        }

    }
}
