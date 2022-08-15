using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;

namespace WDS.ViewModels
{
    public class VmRetailerStock
    {
        public long RetailerId { get; set; }
        public List<stp_retailer_stock_Result> StpRetailerStockResults { get; set; }
    }
}