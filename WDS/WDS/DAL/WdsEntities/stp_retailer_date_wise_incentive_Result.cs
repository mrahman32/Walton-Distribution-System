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
    
    public partial class stp_retailer_date_wise_incentive_Result
    {
        public string DistributorNameCellCom { get; set; }
        public string DigitechCode { get; set; }
        public string Zone { get; set; }
        public string RetailerName { get; set; }
        public string RetailerAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentNumber { get; set; }
        public string PaymentNumberType { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> RetailerIncentive { get; set; }
        public Nullable<decimal> Deduction { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
    }
}