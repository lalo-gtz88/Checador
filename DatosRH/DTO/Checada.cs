using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosRH.DTO
{
    public class Checada
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int Empleado { get; set; }
        public int Usuario { get; set; }
        public string Dispositivo { get; set; }
    }
}
