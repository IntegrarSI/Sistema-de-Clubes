using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class PasesEstadosBL
    {
        public static Negocio.PasesEstados obtener(string codigoInterno)
        {
            using (CABEntities db = new CABEntities())
            {
                return db.PasesEstados.FirstOrDefault(x => x.codigoInterno == codigoInterno);
            }
        }
    }
}
