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
    
    public partial class MOA_Stamps
    {
        public long Id { get; set; }
        public long MOAId { get; set; }
        public string StampNo { get; set; }
        public System.DateTime PurchaseDate { get; set; }
        public string VendorId { get; set; }
        public System.DateTime AddedDate { get; set; }
        public long AddedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public string UpdateName { get; set; }
    
        public virtual MemorandumOfAgreement MemorandumOfAgreement { get; set; }
    }
}