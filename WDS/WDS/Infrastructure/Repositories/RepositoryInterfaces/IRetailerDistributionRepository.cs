using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface IRetailerDistributionRepository:IRepository<RetailerDistribution>
    {
        List<stp_retailer_stock_Result> GetRetailerStock(long retailerId, string digitechCode, string importCode);
        List<stp_sr_sales_report_Result> GetSrSalesReport(long srId, DateTime? startDate, DateTime? endDate, Distributor distributor);
        int GetTotalPhoneTypeCount(string phoneType, DateTime startDate, DateTime endDate, long retailerId);
    }
}
