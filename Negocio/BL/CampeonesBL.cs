using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public partial class CampeonesBL
    {
        public static ListaPaginada obtenerListado(int pageIndex = 1, int pageSize = 1, string pTorneo = "")
         {
             using (DeportesEntities db = new DeportesEntities())
             {
                ListaPaginada resultado = new ListaPaginada();
                var lista = from l in db.Campeones
                            where l.torneo == pTorneo
                            select l;

                   

                resultado.totalItems = lista.Count();
                resultado.paginaActual = lista.OrderByDescending(l => l.fecha).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                 return resultado;
             }

         }

         public static List<Campeones> obtenerUltimos()
         {
             using (DeportesEntities db = new DeportesEntities())
             {
                 return db.Campeones
                        .OrderByDescending(x => x.fecha).Take(3).ToList();

             }
         }
    }
}
