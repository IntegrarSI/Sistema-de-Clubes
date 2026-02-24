using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class LoginTicket
    {
        public int id { get; set; }
        public string nombreUsuario { get; set; }
        public string nombre { get; set; }
        public bool passwordIncorrecta { get; set; }
        public bool administrador { get; set; }
        public bool administraContenido { get; set; }
        public bool administraSeguridad { get; set; }
        public bool administraConfederacion { get; set; }
        public string recaptchaResponse { get; set; }
        public string[] roles { get; set; }
        public int nivel { get; set; }
        public bool activo { get; set; }
    }
}
