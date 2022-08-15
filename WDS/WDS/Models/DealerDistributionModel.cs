using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class DealerDistributionModel
    {
        public long Id { get; set; }
        public string DealerCode { get; set; }
        public string Model { get; set; }
        public string ImeiOne { get; set; }
        public string ImeiTwo { get; set; }
        public string Color { get; set; }
        public DateTime? DistributionDate { get; set; }
        public bool? IsDistributed { get; set; }
        public bool? IsSoldOut { get; set; }
        public bool? IsReceived { get; set; }
        public DateTime? AddedDate { get; set; }
        public string AddedBy { get; set; }
        public long? AddedById { get; set; }
        public DateTime? ReceiveDate { get; set; }

        public string DistributionDateString { get; set; }
        public string AddedDateString { get; set; }
    }
}