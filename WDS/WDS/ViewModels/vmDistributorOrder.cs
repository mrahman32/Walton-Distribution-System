using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class vmDistributorOrder
    {
        public long Id { get; set; }
        public string DistributorCode { get; set; }
        public decimal TotalPrice { get; set; }
        public long AddedBy { get; set; }

        public string AddedByName { get; set; }
        public DateTime AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}