using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class RetailerStockReportSearchBoxModel
    {
        public RetailerStockReportSearchBoxModel()
        {
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public string DealerCode { get; set; }
        public long RetailerId { get; set; }
        public string ModelName { get; set; }
    }
}