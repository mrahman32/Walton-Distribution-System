using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class DistributorDoaQuantityModel
    {
        //"ITEM_NAME": "WALTON Mobile Phone Olvio ML19 (Sky Blue)",
        //"RECEIVED": "1",
        //"PENDING": null,
        //"ADJUST": null

        public string ITEM_NAME { get; set; }
        public int? RECEIVED { get; set; }
        public int? PENDING { get; set; }
        public int? ADJUST { get; set; }
    }
}