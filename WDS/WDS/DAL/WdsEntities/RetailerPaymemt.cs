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
    
    public partial class RetailerPaymemt
    {
        public long Id { get; set; }
        public Nullable<long> RetailerId { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
    }
}
