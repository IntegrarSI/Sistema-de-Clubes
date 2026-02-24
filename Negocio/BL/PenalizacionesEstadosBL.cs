using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class PenalizacionesEstadosBL
    {
        public static Negocio.PenalizacionesEstados obtener(string codigoInterno)
        {
            using (CABEntities db = new CABEntities())
            {
                return db.PenalizacionesEstados.FirstOrDefault(x => x.codigoInterno == codigoInterno);
            }
        }
    }
}
