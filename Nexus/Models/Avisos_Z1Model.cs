using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus.Models
{
    public class Avisos_Z1Model
    {
        public  int RowNumber { get; set; }
        public int ID { get; set; }
        public string centro_costos { get; set; }
        public string ubicacion_tecnica { get; set; }
        public string aviso { get; set; }
        public string fecha_aviso { get; set; }
        public string clase_aviso { get; set; }
        public string descripcion { get; set; }
        public string orden { get; set; }
        public string modificado_por { get; set; }
        public string fecha_modificado { get; set; }
        public string status_sistema { get; set; }
        public string status_usuario { get; set; }
        public string clase_trabajo { get; set; }
        public string duracion_parada { get; set; }
        public string prioridad { get; set; }
        public string inicio_deseado { get; set; }
        public string hora_inicio_averia { get; set; }
        public string hora_fin_averia { get; set; }
        public string hora_inicio_des { get; set; }
        public string inicio_averia { get; set; }
        public string fin_averia { get; set; }
        public string denominacion { get; set; }
        public string equipo { get; set; }
    }
}