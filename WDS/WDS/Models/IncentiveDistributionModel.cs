using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class IncentiveDistributionModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public decimal TotalIncentiveAmount { get; set; }
        [Required]
        public decimal DealerAmount { get; set; }
        [Required]
        public decimal RetailerAmount { get; set; }
        [Required]
        public decimal SRAmount { get; set; }

        public System.DateTime AddedDate { get; set; }
        [Required]
        public System.DateTime StartDate { get; set; }
        [Required]
        public System.DateTime EndDate { get; set; }
        public long AddedBy { get; set; }
        public string Remarks { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}