using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class FiltroJugadores
    {
        public string filtro { get; set; }
        public string federacion { get; set; }
        public string asociacion { get; set; }
        public string club { get; set; }
        public ListaPaginada jugadores { get; set; }
    }
}
