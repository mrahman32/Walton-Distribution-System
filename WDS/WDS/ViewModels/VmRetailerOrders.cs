using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmRetailerOrders
    {
        public long OrderId { get; set; }
        public long RetailerId { get; set; }
        public string RetailerName { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime AddedDate { get; set; }
        public string RetailerAddress { get; set; }
        public string RetailerPhone { get; set; }

    }
}