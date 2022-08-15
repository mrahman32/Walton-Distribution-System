using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class RetailerUpdateModel
    {
        public long Id { get; set; }
        public long RetailerId { get; set; }
        public string NewAddress { get; set; }
        public string NewPhoneNumber { get; set; }
        public string NewPaymentNumber { get; set; }
        public string NewPaymentType { get; set; }
        public long RequestedBy { get; set; }
        public System.DateTime RequestedDate { get; set; }
        public long? RecommendBy { get; set; }
        public DateTime? RecommendDate { get; set; }
        public long? ApproveBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
       


        public string RetailerName { get; set; }
        public string RetailerOldAddress { get; set; }
        public string RetailerOldPhoneNumber { get; set; }
        public string RetailerOldPaymentNumber { get; set; }
        public string RetailerOldPaymentNumberType { get; set; }
    }
}