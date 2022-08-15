using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class SalesRepresentativeModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(01[13456789])\d{8}$", ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; }
        [Required]
        public string PaymentNumberType { get; set; }
        [Required]
        [RegularExpression(@"^(01[13456789])\d{8}$", ErrorMessage = "Phone number is not valid")]
        public string PaymentNumber { get; set; }
        public string DealerCode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCreationApproved { get; set; }
        public long? CreationApprovedBy { get; set; }
        public DateTime? CreationApprovalDate { get; set; }
        public bool? IsDeletionApproved { get; set; }
        public long? DeletionApprovedBy { get; set; }
        public DateTime? DeletionApprovalDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public bool? IsDeletionRequested { get; set; }
        public DateTime? DeletionRequestedDate { get; set; }
    }
}