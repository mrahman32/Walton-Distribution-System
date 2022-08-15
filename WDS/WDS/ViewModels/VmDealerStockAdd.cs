using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmDealerStockAdd
    {
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string Model { get; set; }
        public string ImeiOne { get; set; }
        public string ImeiTwo { get; set; }
        public DateTime? DistributionDate { get; set; }
        public bool? IsDistributed { get; set; }
        public DateTime? AddedDate { get; set; }
        public string Color { get; set; }
        public string AddedBy { get; set; }
        public long? AddedById { get; set; }

        public string Remarks { get; set; }
    }
}