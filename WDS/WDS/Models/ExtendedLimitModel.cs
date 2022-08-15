using System;
using System.ComponentModel.DataAnnotations;

namespace WDS.Models
{
    public class ExtendedLimitModel
    {

        public long Id { get; set; }
        [Required]
        public string DistributorId { get; set; }
        [Required]
        public long? RetailerId { get; set; }
        [Required]
        public string LimitType { get; set; }
        [Required]
        public int? NewLimit { get; set; }
        public DateTime? EndDate { get; set; }
        public string Remarks { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}