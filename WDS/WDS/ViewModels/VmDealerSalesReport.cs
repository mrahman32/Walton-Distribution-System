using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmDealerSalesReport
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Model { get; set; }
        public string RetailerName { get; set; }
        public string RetailerAddress { get; set; }
        public string RetailerPhone { get; set; }
        public int OrderQuantity { get; set; }
    }
}