//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WDS.DAL.WdsEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class RetailerInfo
    {
        public long Id { get; set; }
        public string District { get; set; }
        public string DistributorName { get; set; }
        public string DistributorCode { get; set; }
        public string RetailerName { get; set; }
        public string RetailerAddress { get; set; }
        public string OwnerName { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentNumberType { get; set; }
        public string PaymentNumber { get; set; }
        public string MonthlyAverageSale { get; set; }
        public string MonthlyAverageSaleOfWalton { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsCreationApproved { get; set; }
        public Nullable<long> CreationApprovedBy { get; set; }
        public Nullable<System.DateTime> CreationApprovalDate { get; set; }
        public Nullable<bool> IsDeletionRequested { get; set; }
        public Nullable<System.DateTime> DeletionRequestedDate { get; set; }
        public Nullable<long> DeletionRequestedBy { get; set; }
        public Nullable<bool> IsDeletionApproved { get; set; }
        public Nullable<long> DeletionApprovedBy { get; set; }
        public Nullable<System.DateTime> DeletionApprovalDate { get; set; }
        public Nullable<long> DletionApprovedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string ImagePath { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<long> AddedBy { get; set; }
        public string DistributorCode2 { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public string ThanaName { get; set; }
        public string Division { get; set; }
        public string Zone { get; set; }
        public Nullable<int> ZoneId { get; set; }
        public Nullable<int> DistrictId { get; set; }
        public Nullable<int> ThanaId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<long> TopSellingBrandId { get; set; }
        public Nullable<decimal> TotalSale { get; set; }
        public Nullable<int> ProductBrandId { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public string TransferredDealerCode { get; set; }
    }
}
