using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class VarificationEntityModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdateName { get; set; }

        public long CheckListId { get; set; }
        public string Remarks { get; set; }
        public string IsApproved { get; set; }
    }
}