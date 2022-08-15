using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class DealerPriceModel
    {
        public long Id { get; set; }
        public string ModelName { get; set; }
        public decimal? DealerSellingPrice { get; set; }
    }
}