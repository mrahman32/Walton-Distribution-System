using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class TradeLicensModel
    {
        public long Id { get; set; }
        public long? CheckListId { get; set; }
        public string TradeLicenseNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool? IsBusinessNameMachedWithOracle { get; set; }
        public bool? IsAddressMachedWithOracle { get; set; }
        public bool? IsAddressMatchWithBankSolvencyCertificate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateName { get; set; }
    
    }
}