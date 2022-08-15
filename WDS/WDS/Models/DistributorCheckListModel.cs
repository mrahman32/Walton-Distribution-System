using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using WDS.DAL.WdsEntities;

namespace WDS.Models
{
    public class DistributorCheckListModel
    {
        public DistributorCheckListModel()
        {
            BankSolvencyModel = new BankSolvencyModel();
            DistributorOthersInformationModel = new DistributorOthersInformationModel();
            MemorandumOfAgreementModel = new MemorandumOfAgreementModel();
            MemorandumOfChequeModel = new MemorandumOfChequeModel();
            TaxIdentityModel = new TaxIdentityModel();
            TradeLicensModel = new TradeLicensModel();
            ValueAddedTaxModel = new ValueAddedTaxModel();
            DistributorOthersInformationModel = new DistributorOthersInformationModel();
            DistributorPersonalInformationModel = new DistributorPersonalInformationModel();
        }

        public long Id { get; set; }
        public string DealerCode { get; set; }
        public bool? IsCreationApproved { get; set; }
        public long? CreationApprovedBy { get; set; }
        public DateTime? CreationApprovedDate { get; set; }
        public bool? IsTheDealerBlocked { get; set; }
        public bool? IsTemporaryOpen { get; set; }
        public DateTime? TempOpenDate { get; set; }
        public DateTime? TempExpireDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateName { get; set; }


        public BankSolvencyModel BankSolvencyModel { get; set; }
        public DistributorOthersInformationModel DistributorOthersInformationModel { get; set; }
        public MemorandumOfAgreementModel MemorandumOfAgreementModel { get; set; }
        public MemorandumOfChequeModel MemorandumOfChequeModel { get; set; }
        public TaxIdentityModel TaxIdentityModel { get; set; }
        public TradeLicensModel TradeLicensModel { get; set; }
        
        public ValueAddedTaxModel ValueAddedTaxModel { get; set; }
        public DistributorPersonalInformationModel DistributorPersonalInformationModel { get; set; }


        public bool IsApprover { get; set; }


        public List<MOC_ChequesModel> MocChequesModels { get; set; }

    }
}