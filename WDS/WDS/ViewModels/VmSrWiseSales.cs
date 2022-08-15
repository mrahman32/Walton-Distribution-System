using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;

namespace WDS.ViewModels
{
    public class VmSrWiseSales
    {
        public long SrId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<stp_sr_sales_report_Result> StpSrSalesReportResults { get; set; }
    }
}