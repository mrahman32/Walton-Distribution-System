using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class CheckListApprovalPendingModel
    {
        public long CheckListId { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public bool IsMoa { get; set; }
        public bool IsMoc { get; set; }
        public bool IsBs { get; set; }
        public bool IsTin { get; set; }
        public bool IsVat { get; set; }
        public bool IsTl { get; set; }
        public bool IsDother { get; set; }


        public DateTime AddedDate { get; set; }
        public string AddedByName { get; set; }
        public bool IsApproved { get; set; }
        public bool IsBillVerification { get; set; }


        public string InvestigationStatus { get; set; }
    }
}