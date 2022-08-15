using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceInterfaces
{
    public interface IDashboardService
    {
        bool SaveAdminUpload(VmDashboardImage model);
        List<DashboardImage> GetUploadedImage();
        VmDashboardCard GetRetailerDailyActivation();
        VmDashboardCard GetRetailerMonthlyActivation();
        VmDashboardCard GetRetailerDailySales();
        VmDashboardCard GetRetailerMonthlySales();
        vmSRStatusCount GetSRStatusCountData(List<Distributor> distributorlist, string startDate, string endDate);

        vmRetailerStatusCount GetRetailerStatusCountData(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate);

        List<SalesRepresentativeModel> GetAllActiveSr(List<Distributor> distributor, string startDate, string endDate);
        List<SalesRepresentativeModel> GetAllInActiveSr(List<Distributor> distributor, string startDate, string endDate);


        List<RetailerInfoModel> GetAllActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate);
        List<RetailerInfoModel> GetAllInActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate);
        List<TopMobileModel> GetTopTenSellingModel(string phoneType);
        List<DASHBOARD_SALE_INFO_Result> GetDashboardSaleInfo(Distributor distributor);

        List<string> GetRoleWiseZone(string userName);
        List<SalesZone> GetAllSalesZone();
        List<stp_sr_three_months_incentive_Result> GetSrLastThreeMonthIncentive(string dealerCode);
        List<MisPerson> GetMisList(string zoneId, string dealerId, User user);
    }
}
