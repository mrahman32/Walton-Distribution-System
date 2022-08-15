using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class BankSolvencyModel
    {
        public long Id { get; set; }
        [Required]
        public long? CheckListId { get; set; }
        [Required]
        public DateTime? SolvencyDate { get; set; }
        [Required]
        public int? BankId { get; set; }
        [Required]
        public int? BranchId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }

        public string BusinessAddress { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateName { get; set; }
    }
}