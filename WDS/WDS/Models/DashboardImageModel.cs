using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class DashboardImageModel
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDescription { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPopUp { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? AddedBy { get; set; }
    }
}