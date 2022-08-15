using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class vmSRStatusCount
    {
        public int TotalSR { get; set; }
        public int ActiveSr { get; set; }
        public int inActiveSr { get; set; }
    }
}