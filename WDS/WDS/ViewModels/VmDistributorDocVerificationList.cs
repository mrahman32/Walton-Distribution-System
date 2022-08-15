using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmDistributorDocVerificationList
    {
        public long CheckListId { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
        public string FileDetails { get; set; }
        public string FileUrl { get; set; }
        public HttpPostedFileBase HttpPostedFileBase { get; set; }
        public string FileWebServerUrl { get; set; }
        public bool IsApproved { get; set; }
    }
}