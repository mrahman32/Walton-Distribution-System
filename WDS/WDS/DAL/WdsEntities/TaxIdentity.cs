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
    
    public partial class TaxIdentity
    {
        public long Id { get; set; }
        public Nullable<long> CheckListId { get; set; }
        public string TIN_No { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxCircle { get; set; }
        public string TaxZone { get; set; }
        public Nullable<long> AddedBy { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdateName { get; set; }
    
        public virtual DistributorCheckList DistributorCheckList { get; set; }
    }
}
