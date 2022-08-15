using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class vmRetailerStatusCount
    {
        public int TotalRetailer{ get; set; }
        public int ActiveRetailer { get; set; }
        public int inActiveRetailer { get; set; }
    }
}