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
    
    public partial class OtherRetailer
    {
        public long Id { get; set; }
        public string District { get; set; }
        public string ZoneName { get; set; }
        public string RetailerName { get; set; }
        public string RetailerAddress { get; set; }
        public string OwnerName { get; set; }
        public Nullable<double> PhoneNumber { get; set; }
        public string PaymentNumberType { get; set; }
        public string PaymentNumber { get; set; }
        public string PoliceStation { get; set; }
        public string Division { get; set; }
    }
}