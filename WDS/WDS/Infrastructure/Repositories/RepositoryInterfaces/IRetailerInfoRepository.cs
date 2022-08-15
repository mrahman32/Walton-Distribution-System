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
    public interface IRetailerInfoRepository:IRepository<RetailerInfo>
    {
        List<stp_retailer_date_wise_incentive_Result> GetRetailerIncentiveDataByDate(string fromDate, string toDate, long dType);
        List<stp_retailer_sales_stock_Result> GetRetailerStockAndSaleData(string fromDate, string toDate, string dealerCode, string modelName,long retailerId, User user);
        vmRetailerStatusCount GetRetailerStatusCountData(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate);

        List<RetailerInfoModel> GetAllActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate);
        List<RetailerInfoModel> GetAllInActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate);
    }
}
