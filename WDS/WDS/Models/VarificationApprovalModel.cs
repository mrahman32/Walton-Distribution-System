using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class VarificationApprovalModel
    {
        public long Id { get; set; }
        public long? CheckListId { get; set; }
        public long? VarificationEntityId { get; set; }
        public string VarificationEntityDesc { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public long? ApprovedBy { get; set; }
        public bool? IsDeclined { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public long? DeclinedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdateName { get; set; }
        public string ApprovalRemarks { get; set; }
        public string DeclinedRemarks { get; set; }
    }
}