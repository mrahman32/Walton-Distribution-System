using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class DistributorPersonalInformationModel
    {
        public long Id { get; set; }
        public long CheckListId { get; set; }
        public string FullName { get; set; }
        public DateTime? DistributorDOB { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string BrothersName { get; set; }
        public string SistersName { get; set; }
        public string FathersOccupation { get; set; }
        public string MothersOccupation { get; set; }
        public string BrothersOccupation { get; set; }
        public string SistersOccupation { get; set; }
        public int? BankId { get; set; }
        public int? BranchId { get; set; }
        public decimal? LoanAmount { get; set; }
        public string PropertyDetail { get; set; }
        public string EducationDetail { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ContactNumber { get; set; }
        public string EmergencyName1 { get; set; }
        public string EmergencyAddress1 { get; set; }
        public string EmergencyNumber1 { get; set; }
        public string EmergencyName2 { get; set; }
        public string EmergencyAddress2 { get; set; }
        public string EmergencyNumber2 { get; set; }
        public string BusinessType { get; set; }
        public string PartyCategory { get; set; }
        public long? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string BusinessAddress { get; set; }
        public string ShopPictureUrl { get; set; }
        public HttpPostedFileBase ShopPictureFile { get; set; }
        public string ExperienceDetail { get; set; }
        public double? RunningBusinessDetail_OthersAge { get; set; }
        public decimal? RunningBusinessDetail_OthersInvestment { get; set; }
        public double? RunningBusinessDetail_WaltonAge { get; set; }
        public decimal? RunningBusinessDetail_WaltonInvestment { get; set; }
        public string Reference { get; set; }
        public string Ownership { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdateName { get; set; }
        public string ContactName { get; set; }


        public string ShopPictureWebServerUrl { get; set; }
    }
}