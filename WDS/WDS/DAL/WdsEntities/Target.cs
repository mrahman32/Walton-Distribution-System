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
    
    public partial class Target
    {
        public long Id { get; set; }
        public string TargetName { get; set; }
        public string ModelName { get; set; }
        public string TargetFor { get; set; }
        public Nullable<long> TargetPersonId { get; set; }
        public string TargetType { get; set; }
        public Nullable<int> TargetValue { get; set; }
        public string TargetUnit { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Status { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> AddedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
    }
}
