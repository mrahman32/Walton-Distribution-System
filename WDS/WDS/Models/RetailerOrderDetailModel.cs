using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class RetailerOrderDetailModel
    {
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public long? ProductTypeId { get; set; }
        public int? OrderQuantity { get; set; }
        public int? GivenQuantity { get; set; }
        public string Model { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? OrderTotalPrice { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}