using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class Jugadores
    {
        public Negocio.JugadoresHabilitaciones estampillaActual { get; set; }
        public Negocio.JugadoresPenalizaciones penalizacionActual { get; set; }
        public int? diasUltimoPase { get; set; }
    }
}
