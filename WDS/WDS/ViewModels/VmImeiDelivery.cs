using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebSockets;
using WDS.DAL.WdsEntities;
using WDS.Models;

namespace WDS.ViewModels
{
    public class VmImeiDelivery
    {
        public long OrderId { get; set; }
        public long RetailerId { get; set; }
        public string RetailerName { get; set; }
        public string RetailerPhone { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal GrandTotal { get; set; }
        public List<RetailerOrderDetail> RetailerOrderDetails { get; set; }
        public List<ImeiDeliveryModel> ImeiDeliveryModels { get; set; }

        public VmImeiDelivery()
        {
            RetailerOrderDetails = new List<RetailerOrderDetail>();
            ImeiDeliveryModels = new List<ImeiDeliveryModel>();
        }
    }
}