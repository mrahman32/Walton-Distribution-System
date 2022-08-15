using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;

namespace WDS.Models
{
    public class LogisticsOrderModel
    {
        
        public DistributorOrder DistributorOrder { get; set; }
        public List<DistributorOrderDetail> DistributorOrderDetails { get; set; }

        public LogisticsOrderModel()
        {
            DistributorOrder = new DistributorOrder();
            DistributorOrderDetails = new List<DistributorOrderDetail>();
        }
    }
}