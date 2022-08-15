using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmDashboardImage
    {
        public long Id { get; set; }
        public string ImageDescription { get; set; }
        public HttpPostedFileBase FileBase { get; set; }
        public string FilePath { get; set; }
        public bool IsPopUp { get; set; }
        public bool IsActive { get; set; }
    }
}