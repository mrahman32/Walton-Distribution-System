using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class MemorandumOfChequeModel
    {
        public MemorandumOfChequeModel()
        {
            MocChequesModels = new List<MOC_ChequesModel>();
        }
        public long Id { get; set; }
        public long? CheckListId { get; set; }
        public string DistributorCode { get; set; }
        [Required(ErrorMessage = "Stamp No. is required")]
        public string StampNo { get; set; }
        public string StampType { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string VendorId { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? BankId { get; set; }
        public int? BranchId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountStatement { get; set; }
        public string CheueNo { get; set; }
        public decimal? Amount { get; set; }
        public bool? MICR { get; set; }
        public string ChequeAttachment { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateName { get; set; }



        public HttpPostedFileBase CheckAttachmentFile { get; set; }
        public string ChequeAttachmentWebUrl { get; set; }

        public HttpPostedFileBase AccountStatementFileBase { get; set; }
        public string AccountStatementWebUrl { get; set; }


        public List<MOC_ChequesModel> MocChequesModels { get; set; }
    }
}