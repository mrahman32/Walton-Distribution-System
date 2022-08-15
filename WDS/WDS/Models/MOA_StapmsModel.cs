using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class MOA_StapmsModel
    {
        public long Id { get; set; }
        public long MOAId { get; set; }
        public string StampNo { get; set; }
        public System.DateTime PurchaseDate { get; set; }
        public string VendorId { get; set; }
        public System.DateTime AddedDate { get; set; }
        public long AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdateName { get; set; }
    }
}