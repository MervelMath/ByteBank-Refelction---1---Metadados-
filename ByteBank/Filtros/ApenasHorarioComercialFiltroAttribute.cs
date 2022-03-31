using ByteBank.Infraestrutura.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Filtros
{
    public class ApenasHorarioComercialFiltroAttribute : FiltroAttribute
    {
        public override bool PodeContinuar()
        {
            var hora = DateTime.Now.Hour;

            return true;
        }
    }
}
