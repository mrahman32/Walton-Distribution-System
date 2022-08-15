using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class vmDistributors
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Distributor Name is Required")]
        public string DistributorNameCellCom { get; set; }
        [Required(ErrorMessage = "DigitechCode is Required")]
        public string DigitechCode { get; set; }
        [Required(ErrorMessage = "Zone Name is Required")]
        public long ZoneId { get; set; }
        public string Zone { get; set; }
        public string ImportCode { get; set; }
        [Required(ErrorMessage = "Mobile No is Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Not a valid phone number")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Distributor Type is Required")]
        public int DistributorTypeId { get; set; }
        [Required(ErrorMessage = "Upazila Name is Required")]
        public long UpazilaId { get; set; }
        [Required(ErrorMessage = "District Name is Required")]
        public long DistrictId { get; set; }
        [Required(ErrorMessage = "Division Name is Required")]
        public long DivisionId { get; set; }
        [Required(ErrorMessage = "Brand Name is Required")]
        public int ProductBrandId { get; set; }
        public bool? IsActive { get; set; }
        public long? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}