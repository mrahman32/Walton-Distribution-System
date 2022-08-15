using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class RoleModel
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
    }
}