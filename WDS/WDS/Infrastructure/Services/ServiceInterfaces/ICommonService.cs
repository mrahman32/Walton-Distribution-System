using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;

namespace WDS.Infrastructure.Services.ServiceInterfaces
{
    public interface ICommonService
    {
        List<stp_check_imei_status_Result> GetImeiDetail(string imei);
        List<ModelPrice> GetModelList();
        List<ModelColor> GetModelWiseColor(string model);
        ModelPrice GetModelWisePrice(string model);
        List<SelectListItem> GetDistributorTypeSelectListItems();
        List<Bank> GetBanks();
        List<BankBranch> GetBankBranches();
    }
}