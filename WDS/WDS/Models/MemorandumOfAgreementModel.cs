using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class MemorandumOfAgreementModel
    {
        public MemorandumOfAgreementModel()
        {
            MoaStapmsModels = new List<MOA_StapmsModel>();
        }
        public long Id { get; set; }
        public long? CheckListId { get; set; }
        //[Required(ErrorMessage = "Amount is required")]
        public decimal? InvestmnetAmount { get; set; }
        public DateTime? AgreementStartDate { get; set; }
        public DateTime? AgreementEndDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdateName { get; set; }

        public List<MOA_StapmsModel> MoaStapmsModels { get; set; }
    }
}