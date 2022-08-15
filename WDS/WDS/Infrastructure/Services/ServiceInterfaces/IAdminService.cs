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
    public interface IAdminService
    {
        List<VmDashboardImage> GetImageList();
        Role GetRoleById(long? roleId);
        bool ApproveRetailer(long id);
        bool RejectRetailer(long id);
        List<RetailerInfo> GetPendingRetailerForApproval(User user);
        List<SalesRepresentative> GetPendingSrApproval(User user);
        bool ApproveSr(long id);
        bool RejectSr(long id);
        bool ChangeStatusUploadedImage(long id, bool status);
        AjaxResponseModel DeleteImage(long id);
        string SaveIncentive(IncentiveDistributionModel model);

        string SaveDistributor(vmDistributors model);
        List<IncentiveDistribution> GetIncentiveList();
        List<ProductMaster> GetModels();
        List<Distributor> GetUniqDistributors();
        string SaveTarget(TargetModel model);
        List<TargetModel> GetAllTargets();
        List<stp_distributor_wise_sale_Result> GetDealerWiseSalesReport(DateTime stDate, DateTime edDate);
        List<RetailerInfo> GetDeactivationListOfRetailers(User user);
        List<SalesRepresentative> GetPendingDeactivationListOfSr(User user);
        List<stp_dealer_target_achievement_report_Result> GetDealerAchievementReport(string targetType);

        List<stp_retailer_date_wise_incentive_Result> GetRetailerIncentiveDataByDate(string fromDate,string toDate, long dType);
        List<stp_sr_date_wise_incentive_Result> GetSRIncentiveDataByDate(string fromDate, string toDate, long dType);
        List<RetailerInfo> GetPendingAllRetailer();
        List<stp_sr_sales_incentive_Result> GetSRIncentiveAdminDataByDate(string fromDate, string toDate, string dCode, long srId, string model);

        string SaveLimitExtension(ExtendedLimitModel model);
        string RecommendRetailerUpdate(long rid , User user);
        List<RetailerUpdateModel> GetRetailerUpdateModel(User user);
        string AvailabilityBroadcastSave(VmAvailabilityBroadcast model);
        List<VmAvailabilityBroadcast> GetAllBrodcastList();
        IncentiveDistributionModel GetIncentiveById(long id);
        long UpdateIncentive(IncentiveDistributionModel model);
    }
}
