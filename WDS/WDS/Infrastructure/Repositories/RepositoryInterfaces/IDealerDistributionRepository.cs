using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface IDealerDistributionRepository:IRepository<DealerDistribution>
    {
        List<DealerStockCheck_Result> GetDealerStockData(string dealerCode, string alternateCode, string ebsCode);
        List<stp_check_imei_status_Result> GetImeiInformation(string imei);
        List<stp_distributor_sales_and_stock_Result> GetDealerSaleAndStockData(vmDealerAndRetailerStock objvmDealerAndRetailerStock, Role role);
    }
}
