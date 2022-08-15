using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmAvailabilityBroadcast
    {
        public long Id { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public string ModelName { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
    }
}