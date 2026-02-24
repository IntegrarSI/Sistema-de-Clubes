using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class InformesFiltros
    {
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public string filtro { get; set; }
        public Federaciones federacion { get; set; }
        public Asociaciones asociacion { get; set; }
        public Clubes club { get; set; }
        public bool habilitado { get; set; }
        public int estampilla { get; set; }
        public TipoPase tipoPase { get; set; }
    }
}
