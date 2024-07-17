using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus.Models
{
    public class CapaModule
    {
        public int RowNumber { get; set; }
        public int ID { get; set; }
        public string number { get; set; }
        public string type { get; set; }
        public string start_date { get; set; }
        public string entry_date { get; set; }
        public string short_description { get; set; }
        public string deliverables { get; set; }
        public string client { get; set; }
        public string deadline_final_aprobal  { get;set; } 
        public string workflow_status { get; set; }
        public string success { get; set; }
        public string result { get; set; }
        public string remark { get; set; }
        public string finishdate { get; set; }
        public string origin { get; set; }
        public string number_extensions { get; set; }
        public string effectiveness_check { get; set; }
        public string justification { get; set; }
        public string implementation_date { get; set; }
        public string planned_date { get; set; }
        public string v_et { get; set; }
        public string evento { get; set; }
    }
}