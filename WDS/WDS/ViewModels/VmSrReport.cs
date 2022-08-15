using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmSrReport
    {
        public long OrderId { get; set; }
        public string SerialNo { get; set; }
        public long SRId { get; set; }
        public string SrName { get; set; }
        public string Model { get; set; }
        public int GivenQty { get; set; }
        public string Color { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
    }
}