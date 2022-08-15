using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class DistributorOthersInformationModel
    {
        public long Id { get; set; }
        public long? CheckListId { get; set; }
        public string DirectorNID_No { get; set; }
        public string NID_File { get; set; }
        public string DirectorPhoto { get; set; }
        public bool? IsDirectorSealedAndSigned { get; set; }
        public string NomineeNID_No { get; set; }
        public string NomineeNID_File { get; set; }
        public bool? IsSignedByNominee { get; set; }
        public bool? IsSignedNomineeNIDByDirector { get; set; }
        public string NomineePhoto { get; set; }
        public bool? IsNomineePhotoSignedByNominee { get; set; }
        public bool? IsNomineePhotoSignedByDirector { get; set; }
        public int? NomineeChequeBank { get; set; }
        public int? NomineeBankBranch { get; set; }
        public string NomineeAccountNo { get; set; }
        public string NomineeAccountName { get; set; }
        public string NomineeChequeNo { get; set; }
        public decimal? NomineeChequeAmount { get; set; }
        public bool? NomineeMICR_Cheque { get; set; }
        public string MomineeMICR_ChequeFile { get; set; }
        public DateTime? DistributorShopAgreementDate { get; set; }
        public DateTime? DistributorShopAgreementEndDate { get; set; }
        public bool? IsShopOwnedByDistributor { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateName { get; set; }



        public HttpPostedFileBase DirectorNidFile { get; set; }
        public HttpPostedFileBase DirectorPhotoFile { get; set; }
        public HttpPostedFileBase NomineeNidFile { get; set; }
        public HttpPostedFileBase NominieePhotoFile { get; set; }
        public HttpPostedFileBase BlankMicrChequeFile { get; set; }


        public string DirectorPhotoWebserverUrl { get; set; }
        public string DirectorNIDWebserverUrl { get; set; }
        public string NomineeNIDWebserverUrl { get; set; }
        public string NomineePhotoWebserverUrl { get; set; }
        public string BlankCheckWebserverUrl { get; set; }
    }
}