using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class TaxIdentityModel
    {
        public long Id { get; set; }
        public long? CheckListId { get; set; }
        public string TIN_No { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxCircle { get; set; }
        public string TaxZone { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateName { get; set; }
    }
}