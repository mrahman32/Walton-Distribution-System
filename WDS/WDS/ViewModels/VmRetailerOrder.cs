using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;

namespace WDS.ViewModels
{
    public class VmRetailerOrder
    {
        public long RetailerId { get; set; }
        public string RetailerName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public long ProductTypeId { get; set; }
        public string ModelName { get; set; }
        public int Quantity { get; set; }
        public decimal GrandTotal { get; set; }
        public List<RetailerOrderDetail> RetailerOrderDetails { get; set; }

        public VmRetailerOrder()
        {
            RetailerOrderDetails = new List<RetailerOrderDetail>();
        }
    }
}