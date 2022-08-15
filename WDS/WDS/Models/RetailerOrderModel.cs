using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class RetailerOrderModel
    {
        public long Id { get; set; }
        public string SerialNo { get; set; }
        public long? RetailerId { get; set; }
        public long? SRId { get; set; }
        public string DealerCode { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool? IsCompleted { get; set; }
        public decimal? TotalPrice { get; set; }
        public string PaymentType { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}