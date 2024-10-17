using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus.Models
{
    public class UserModel
    {
        public int RowNumber { get; set; }
        public int id_user { get; set; }
        public string cwid { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string department { get; set; }
        public string title { get; set; }
        public int is_enable { get; set; }
    }
}