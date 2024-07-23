using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus.Models
{
    public class TemplatesModel
    {
        public int RowNumber { get; set; }
        public int id_detail { get; set; }
        public int id_template { get; set; }
        public int id_user { get; set; }

        public string name_template { get; set; }
    }
}