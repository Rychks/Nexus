using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus.Models
{
    public class DashboardModel
    {
        public int RowNumber {  get; set; }
        public int id_dashboard {  get; set; }
        public string code_department { get; set; }

        public string title { get; set; }
        public string guia { get; set; }
        public string link { get; set; }
        public string type { get; set; }
        public int id_department { get; set; }
        public string department { get; set; }
        public string previus_image { get; set; }
        public int is_enable { get; set; }
        public string insert_time_stamp { get; set; }
        public string update_time_stamp { get; set; }
        public string dashboard_type { get; set; }
        public int id_dashboard_type { get; set; }

    }
}