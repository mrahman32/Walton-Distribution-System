using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmProductReturn
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public long RetailerId { get; set; }
        public string RetailerName { get; set; }
        public string Model { get; set; }
        public string ImeiOne { get; set; }
        public string ImeiTwo { get; set; }
        public DateTime DistributionDate { get; set; }
    }
}