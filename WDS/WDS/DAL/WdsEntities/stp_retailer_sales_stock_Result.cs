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
    
    public partial class stp_retailer_sales_stock_Result
    {
        public string Dealername { get; set; }
        public string DigitechCode { get; set; }
        public string RetailerName { get; set; }
        public string RetailerAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<long> RetailerId { get; set; }
        public string Model { get; set; }
        public int StockQty { get; set; }
        public int LiftingQty { get; set; }
        public int SaleQty { get; set; }
        public Nullable<decimal> StockValue { get; set; }
        public Nullable<decimal> LiftingValue { get; set; }
        public Nullable<decimal> SaleValue { get; set; }
    }
}
