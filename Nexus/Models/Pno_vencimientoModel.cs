using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus.Models
{
    public class Pno_vencimientoModel
    {
        public int RowNumber { get; set; }
        public int ID { get; set; }
        public string cwid { get; set; } 
        public string last_name { get; set; }
        public string fist_name { get; set; }
        public string super_last_name { get; set; }
        public string super_first_name { get; set; }
        public string item_ID { get; set; }
        public string requiered { get; set; }
        public string departamento { get; set; }
    }
}