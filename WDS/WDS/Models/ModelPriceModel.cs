using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class ModelPriceModel
    {
        public long Id { get; set; }
        public string ModelName { get; set; }
        public decimal? Price { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
    }
}