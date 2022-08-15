using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.Models
{
    public class DistributorDoaDetailModel
    {
        //"ITEM_NAME": "WALTON Mobile Phone Olvio L28s (Sky Blue+Black)",
        //"SERVICE_NO": "1405224106",
        //"RECEIVE_DATE": "14-MAY-22",
        //"BARCODE": "351972571429492",
        //"CUST_NAME": "Mobile Mela",
        //"DEALER_CODE": "59909",
        //"SERVICE_CENTER_ID": "72776345"

        public string ITEM_NAME { get; set; }
        public string SERVICE_NO { get; set; }
        public string RECEIVE_DATE { get; set; }
        public string BARCODE { get; set; }
        public string CUST_NAME { get; set; }
        public string DEALER_CODE { get; set; }
        public string SERVICE_CENTER_ID { get; set; }
    }
}