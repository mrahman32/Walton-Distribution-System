using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class vmDistributorOrderDetail
    {
        public long Id { get; set; }
        public long DistributorOrderId { get; set; }
        public string ModelName { get; set; }
        public string Color { get; set; }
        public decimal UnitPrice { get; set; }
        public int RequestedQuantity { get; set; }
        public Nullable<int> NsmApprovedQty { get; set; }
        public Nullable<int> LogisticsApprovedQty { get; set; }
        public string AddedRole { get; set; }
        public long AddedBy { get; set; }
        public System.DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}