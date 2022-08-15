using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class TargetModel
    {
        public long Id { get; set; }
        public string TargetName { get; set; }
        public string ModelName { get; set; }
        public string TargetFor { get; set; }
        public long? TargetPersonId { get; set; }
        public string TargetType { get; set; }
        public int? TargetValue { get; set; }
        public string TargetUnit { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public long? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }


        public long? DealerId { get; set; }
        public long? RetailerId { get; set; }
        public string AddedByName { get; set; }
        public string TargetPersonnel { get; set; }
    }
}