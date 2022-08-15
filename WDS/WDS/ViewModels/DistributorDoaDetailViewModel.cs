using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.Models;

namespace WDS.ViewModels
{
    public class DistributorDoaDetailViewModel
    {
        public DistributorDoaDetailViewModel()
        {
            ReceiveList = new List<DistributorDoaDetailModel>();
            AdjustList = new List<DistributorDoaDetailModel>();
            PendingList = new List<DistributorDoaDetailModel>();
        }
        public List<DistributorDoaDetailModel> ReceiveList { get; set; }
        public List<DistributorDoaDetailModel> AdjustList { get; set; }
        public List<DistributorDoaDetailModel> PendingList { get; set; }
    }
}