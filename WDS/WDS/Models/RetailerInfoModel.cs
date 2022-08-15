using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class RetailerInfoModel
    {
        public long Id { get; set; }
        public string District { get; set; }
        public string DistributorName { get; set; }
        public string DistributorCode { get; set; }
        [Required]
        public string RetailerName { get; set; }
        [Required]
        public string RetailerAddress { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        [RegularExpression(@"^(01[13456789])\d{8}$", ErrorMessage = "Phone number is not valid")]
        public string PhoneNumber { get; set; }
        [Required]
        public string PaymentNumberType { get; set; }
        [Required]
        [RegularExpression(@"^(01[13456789])\d{8}$", ErrorMessage = "Phone number is not valid")]
        public string PaymentNumber { get; set; }
        public string MonthlyAverageSale { get; set; }
        public string MonthlyAverageSaleOfWalton { get; set; }
        public string Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCreationApproved { get; set; }
        public long? CreationApprovedBy { get; set; }
        public DateTime? CreationApprovalDate { get; set; }
        public bool? IsDeletionApproved { get; set; }
        public long? DeletionApprovedBy { get; set; }
        public DateTime? DeletionApprovalDate { get; set; }
        public System.DateTime AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public string DistributorCode2 { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? IsDeletionRequested { get; set; }
        public DateTime? DeletionRequestedDate { get; set; }

        public long? DeletionRequestedBy { get; set; }
        public long? DletionApprovedBy { get; set; }

        public string ImagePath { get; set; }
        [Required]
        public HttpPostedFileBase RetailerImage { get; set; }


        public string ThanaName { get; set; }
        public string Division { get; set; }
        public string Zone { get; set; }
        public int? ZoneId { get; set; }
        [Required]
        public int? DistrictId { get; set; }
        [Required]
        public int? ThanaId { get; set; }
        [Required]
        public int? DivisionId { get; set; }
        [Required]
        public long? TopSellingBrandId { get; set; }

        public string BrandName { get; set; }
        [Required]
        public decimal? TotalSale { get; set; }
        public int? ProductBrandId { get; set; }
    }
}