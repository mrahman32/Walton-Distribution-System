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
    
    public partial class DealerDistributionDetail
    {
        public long Id { get; set; }
        public string DealerCode { get; set; }
        public string Model { get; set; }
        public string ImeiOne { get; set; }
        public string ImeiTwo { get; set; }
        public string Color { get; set; }
        public Nullable<System.DateTime> DistributionDate { get; set; }
    }
}
