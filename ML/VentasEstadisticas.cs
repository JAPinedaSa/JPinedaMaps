using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class VentasEstadisticas
    {
        public decimal VentasTotales { get; set; }
        public decimal VentasNorte = 0;  
        public decimal VentasSur { get; set; }
        public decimal VentasEste { get; set; }
        public decimal VentasOeste { get; set; }

        public List<object> VentasList { get; set; }
    }
}
