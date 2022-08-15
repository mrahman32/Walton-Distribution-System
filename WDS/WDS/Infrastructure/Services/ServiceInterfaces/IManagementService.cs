using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceInterfaces
{
    public interface IManagementService
    {
        List<Zone> GetZons();
        List<Brand> GetBrands();
        bool SaveMarketShareData(List<MarketShare> modelList);
        bool DubValueCheck(MarketShare objModel);
        List<stp_management_area_wise_top_brand_Result> AreaWiseTopBrand(int monthNo, int yearNo, long zoneId);
        List<stp_management_area_wise_market_share_Result> GetAreaWiseWaltonMarketShare(int monthNumber, int yearNumber, long zoneId);
        List<stp_management_area_wise_market_share_walton_Result> GetAreaWisePieData(int monthNumber, int yearNumber, long zoneId);
        List<stp_market_size_Result> GetMarketSizeTile(int monthNo, int yearNo, long zoneId);
        List<stp_management_retailer_count_Result> GetRetailerCount(long zoneId);
        List<stp_management_retailer_list_zone_Result> GetRetailers(long zoneId);
    }
}
