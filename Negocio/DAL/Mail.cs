using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class Mail
    {
        public string asunto { get; set; }
        public string nombre { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public string mail { get; set; }
        public string mensaje { get; set; }
    }
}
