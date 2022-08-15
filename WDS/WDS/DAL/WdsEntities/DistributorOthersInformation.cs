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
    
    public partial class DistributorOthersInformation
    {
        public long Id { get; set; }
        public Nullable<long> CheckListId { get; set; }
        public string DirectorNID_No { get; set; }
        public string NID_File { get; set; }
        public string DirectorPhoto { get; set; }
        public Nullable<bool> IsDirectorSealedAndSigned { get; set; }
        public string NomineeNID_No { get; set; }
        public string NomineeNID_File { get; set; }
        public Nullable<bool> IsSignedByNominee { get; set; }
        public Nullable<bool> IsSignedNomineeNIDByDirector { get; set; }
        public string NomineePhoto { get; set; }
        public Nullable<bool> IsNomineePhotoSignedByNominee { get; set; }
        public Nullable<bool> IsNomineePhotoSignedByDirector { get; set; }
        public Nullable<int> NomineeChequeBank { get; set; }
        public Nullable<int> NomineeBankBranch { get; set; }
        public string NomineeAccountName { get; set; }
        public string NomineeAccountNo { get; set; }
        public string NomineeChequeNo { get; set; }
        public Nullable<decimal> NomineeChequeAmount { get; set; }
        public Nullable<bool> NomineeMICR_Cheque { get; set; }
        public string NomineeMICR_ChequeFile { get; set; }
        public Nullable<System.DateTime> DistributorShopAgreementDate { get; set; }
        public Nullable<System.DateTime> DistributorShopAgreementEndDate { get; set; }
        public Nullable<bool> IsShopOwnedByDistributor { get; set; }
        public Nullable<long> AddedBy { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdateName { get; set; }
    
        public virtual DistributorCheckList DistributorCheckList { get; set; }
    }
}