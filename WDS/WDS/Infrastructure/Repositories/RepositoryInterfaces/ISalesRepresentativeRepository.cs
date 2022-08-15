using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface ISalesRepresentativeRepository:IRepository<SalesRepresentative>
    {
        List<stp_sr_report_Result> GetSrReport(string startDate, string endDate, long srId);
        List<stp_sr_date_wise_incentive_Result> GetSRIncentiveDataByDate(string fromDate, string toDate, long dType);

        vmSRStatusCount GetSRStatusCountData(string stringDealerCode, string startDate, string endDate);

        List<SalesRepresentativeModel> GetAllActiveSr(string stringDealerCode, string startDate, string endDate);
        List<SalesRepresentativeModel> GetAllInActiveSr(string stringDealerCode, string startDate, string endDate);

        List<stp_sr_sales_incentive_Result> GetSRIncentiveAdminDataByDate(string fromDate, string toDate, string dCode, long srId, string model);
        List<stp_sr_three_months_incentive_Result> GetSrThreeMonthsIncentive(string dealerCode);
    }
}
