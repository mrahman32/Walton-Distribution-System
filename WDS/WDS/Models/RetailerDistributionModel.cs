using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class RetailerDistributionModel
    {
        public long Id { get; set; }
        public long? RetailerId { get; set; }
        public long? OrderId { get; set; }
        public string Model { get; set; }
        public string ImeiOne { get; set; }
        public string ImeiTwo { get; set; }
        public bool? IsReturn { get; set; }
        public DateTime? DistributionDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}