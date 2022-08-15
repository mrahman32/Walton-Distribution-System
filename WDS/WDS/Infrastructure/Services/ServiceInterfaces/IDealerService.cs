using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceInterfaces
{
    public interface IDealerService
    {
        List<SelectListItem> GetDealerSelectList();
        List<SelectListItem> GetDivisionSelectList();
        List<SelectListItem> GetDistrictSelectListBydivisionId(long divisionId);
        List<SelectListItem> GetThanaSelectListByDistrictId(long districtId);

        List<SelectListItem> GetDistrictSelectList();
        List<SelectListItem> GetThanaSelectList();
        List<SelectListItem> GetTopSellingBrandSelectList();

        List<SelectListItem> GetProductBrandSelectList();

        List<SelectListItem> GetZoneSelectList();

        List<SelectListItem> GetDistributorTypeSelectList();

        Role GetRoleById(long? roleId);
        string SaveRetailer(RetailerInfoModel model);
        List<RetailerInfo> GetRetailers();
        RetailerInfo GetRetailerById(long id);
        Brand GetABrandbyId(long? id);
        bool EditRetailer(RetailerInfo model);
        bool DeactivateRetailer(long id);
        List<VmRetailerOrders> GetRetailerOrderList(DateTime startDate, DateTime endDate);
        VmImeiDelivery GetOrderById(long orderId);
        DealerDistribution GetImeiInformation(string imei);

        List<DealerDistribution> GetProductList(Distributor distributor, string model);

        bool SaveOrderDelivery(VmImeiDelivery vmImeiDelivery);

        //tblDealerInfo GetDealerByPhoneNumber(string userName);
        List<SalesRepresentative> GetSalesRepresentativeByDealerCode(string dealerCode1, string dealerCode2);

        List<SalesRepresentative> GetSalesRepresentativeByDigitechCode(string digitechCode);

        //tblCellPhonePricing GetCellPhonePriceByModel(string model);
        Tuple<long, string> SaveProductIssue(VmProductIssue vmProductIssue, User user);
        VmProductReturn GetRetailerImeiInfo(string imei);
        bool SaveProductReturn(List<VmProductReturn> vmProductReturns);
        List<Distributor> GetAllDistributorByZone(string zoneName);
        Distributor GetADistributorByZone(string zoneName);
        List<ProductMaster> GetAllProductModels();
        bool SaveSr(SalesRepresentativeModel model);
        List<SalesRepresentative> GetSalesRepresentativesByDealerCode(Distributor distributor);
        SalesRepresentativeModel GetSalesRepresentativeById(long id);
        bool DeactivateSr(long id);
        void UpdateSr(SalesRepresentativeModel model);

        bool ProductReceiveUpdate(List<DealerDistributionModel> model);

        List<VmDealerSalesReport> GetDealerSales(string dealerCode, DateTime startDate, DateTime endDate);

        List<DealerStockCheck_Result> GetDealerStockReport(string dealerCode, string alternateCode,
            string ebsNewServerCode);

        List<stp_distributor_sales_and_stock_Result> GetDealerStockAndSaleData(
            vmDealerAndRetailerStock objvmDealerAndRetailerStock, Role role);

        VmPrintProductIssue GetProductIssuePrintData(long oid);
        VmDealerStockAdd GetRbsynergyDistribution(string imei, User user);
        bool CheckImeiIsExist(string imei);
        bool SaveStock(List<VmDealerStockAdd> vmDealerStockAddList, User user);
        List<stp_sr_report_Result> GetSrReport(string startDate, string endDate, long srId);
        List<RetailerInfo> GetRetailersByDealerId(Distributor distributor);
        List<stp_retailer_stock_Result> GetRetailerStock(long retailerId, Distributor distributor);

        List<stp_sr_sales_report_Result> GetSrSalesReport(long srId, DateTime? startDate, DateTime? endDate,
            Distributor distributor);

        Distributor GetDistributorById(long dealerId);
        Distributor GetDistributorByPhone(string mobileNumber);
        bool ApproveRetailerDeactivation(long id);
        bool RejectRetailerDeactivation(long id);
        bool ApproveSrDeactivation(long id);
        bool RejectSrDeactivation(long id);
        ModelPrice GetModelPriceByModel(string model);

        bool GetTotalLimitResult(int smartPhoneTotalLimit, int featurePhoneTotalLimit, DateTime startDate,
            DateTime endDate, DealerDistribution distribution, long retailerId);

        bool GetModelLimitResult(int smartPhoneModelLimit, int featurePhoneModelLimit, DateTime startDate,
            DateTime endDate, DealerDistribution distribution, long retailerId);

        List<Distributor> GetAllDistributors();

        List<vmDistributorOrder> GetAllDistributorOrderList(Role role);
        List<vmDistributorOrderDetail> GetAllDistributorOrderDetailsListByOrderId(long orderId);

        bool saveDistributorOrderData(DistributorOrder objDistributorOrder, List<DistributorOrderDetail> orderDetailsList,User SeassionUser);
        bool updateDistributorOrderData(DistributorOrder objDistributorOrder, List<DistributorOrderDetail> orderDetailsList, User SeassionUser);
        List<Zone> GetAllZones();

        Zone GetAZoneById(long id);

        string SaveRetailerUpdateLog(RetailerInfo model);

        List<SalesRepresentative> GetAllSalesRepresentative();
        List<Distributor> GetDistributorsByZone(string zone);
        List<SelectListItem> GetDistributorSelectListItems();
        List<DistributorDoaQuantityModel> GetDistributorDoaQuantityByApi(string digitechCode, DateTime stDate, DateTime edDate);
        DistributorDoaDetailViewModel GetDistributorDoaDetails(string digitechCode, DateTime stDate, DateTime edDate, string modelName, string recQty, string adjQty, string penQty);
    }
}
