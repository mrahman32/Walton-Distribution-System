using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceInterfaces
{
    public interface IRetailerService
    {
        RetailerInfo GetRetailerInfoByPhoneNo(string phoneNumber);
        //tblDealerInfo GetDealerByCode(string distributorCode);
        List<SelectListItem> GetProductTypeListItems();
        List<SelectListItem> GetModelListItemByTypeId(long phoneTypeId);
        bool SaveRetailerOrder(VmRetailerOrder model);
        ModelPrice GetModelPrice(string model);

        List<Distributor> GetAllDistributorByZone(string zoneName);
        List<ProductMaster> GetAllProductModels();

        List<SelectListItem> GetRetailerListbydealer(string dealerCode);
        List<stp_retailer_sales_stock_Result> GetRetailerStockAndSaleData(string fromDate, string toDate, string dealerCode, string modelName, long retailerId, User user);
    }
}
