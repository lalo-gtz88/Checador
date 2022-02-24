using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosRH
{
    public class Personal
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int Area { get; set; }
        public string Calle { get; set; }
        public int Numero { get; set; }
        public string NumInt { get; set; }
        public string Fracc { get; set; }
        public int CP { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public DateTime FechaIng { get; set; }
        public string Puesto { get; set; }
        public int Turno { get; set; }
        public string Contrato { get; set; }
        public string TurnoOpcional { get; set; }
        public int DenomPuesto { get; set; }
        public byte[] Huella { get; set; }
        public string Status { get; set; }
        public string Pass { get; set; }
        public string Servicio { get; set; }
        public string Cedula { get; set; }
    }
}
