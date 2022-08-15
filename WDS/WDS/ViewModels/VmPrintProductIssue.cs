using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using WDS.DAL.WdsEntities;

namespace WDS.ViewModels
{
    public class VmPrintProductIssue
    {
        public long OrderId { get; set; }
        public string OrderSerial { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string DealerPhone { get; set; }
        public string RetailerName { get; set; }
        public string SrName { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string PaymentType { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PrintDate { get; set; }
        public List<RetailerOrderDetail> RetailerOrderDetails { get; set; } 
    }
}