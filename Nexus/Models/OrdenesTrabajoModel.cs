using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus.Models
{
    public class OrdenesTrabajoModel
    {
         public int RowNumber { get; set; }
        public int ID { get; set; }
        public string orden { get; set; }
        public string aviso { get; set; }
        public string ubicacion_tecnica { get; set; }
        public string denominacion { get; set; }
        public string texto_breve { get; set; }
        public string status_sistema { get; set; }
        public string autor { get; set; }
        public string modificado_por { get; set; }
        public string cl_actividad { get; set; }
        public string prioridad { get; set; }
        public string fecha_ini { get; set; }
        public string fecha_fin { get; set; }
        public string fecha_ref { get; set; }
        public string inicio_programa { get; set; }
        public string fin_programado { get; set; }
        public string fin_hora { get; set; }
        public string fecha_inicio_real { get; set; }
        public string hora_inicio_real { get; set; }
        public string fecha_fin_real { get; set; }
        public string hora_fin_real { get; set; }
        public string hora_inicio { get; set; }
        public string hora_inicio1 { get; set; }
        public string liberacion_real { get; set; }
    }
}