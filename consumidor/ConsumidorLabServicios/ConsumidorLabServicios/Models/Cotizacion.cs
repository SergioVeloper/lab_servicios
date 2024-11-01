using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumidorLabServicios.Models
{
    public class Cotizacion
    {
        public int id { get; set; }
        public string fecha { get; set; }
        public double cotizacion { get; set; }
        public double cotizacionOficial { get; set; }
    }

}
