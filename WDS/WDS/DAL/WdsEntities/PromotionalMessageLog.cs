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
    
    public partial class PromotionalMessageLog
    {
        public long Id { get; set; }
        public string MessageBody { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverType { get; set; }
        public string SenderNumber { get; set; }
        public string SenderApi { get; set; }
        public string ApiResponse { get; set; }
        public Nullable<long> SenderId { get; set; }
        public string SenderName { get; set; }
        public Nullable<System.DateTime> SendingDate { get; set; }
        public string RequestedFrom { get; set; }
    }
}
