using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Infrastructure.Services.ServiceInterfaces;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    public class CommonService:ICommonService
    {
        private readonly Unit _wdsUnit;

        public CommonService(Unit unit)
        {
            _wdsUnit = unit;
        }

        public List<stp_check_imei_status_Result> GetImeiDetail(string imei)
        {
            List<stp_check_imei_status_Result> checkImeiStatus =
                _wdsUnit.DealerDistributionRepository.GetImeiInformation(imei);
            return checkImeiStatus;
        }

        public List<ModelPrice> GetModelList()
        {
            List<ModelPrice> models = _wdsUnit.ModelPriceRepository.GetAll();
            return models;
        }

        public List<ModelColor> GetModelWiseColor(string model)
        {
            List<ModelColor> colors = _wdsUnit.ModelColorRepository.Find(i => i.ProductModel == model);
            return colors;
        }


        public ModelPrice GetModelWisePrice(string model)
        {
            ModelPrice price = _wdsUnit.ModelPriceRepository.Find(x=>x.ModelName==model).FirstOrDefault();
            return price;
        }

        public List<SelectListItem> GetDistributorTypeSelectListItems()
        {
            List<DistributorType> distributorTypes = _wdsUnit.DistributorTypeRepository.GetAll();
            List<SelectListItem> result = new List<SelectListItem>{new SelectListItem{Value = "", Text = "--Select--"}};
            result.AddRange(distributorTypes.Select(distributorType => new SelectListItem {Value = distributorType.Id.ToString(), Text = distributorType.DistributorTypeName}));
            return result;
        }

        public List<Bank> GetBanks()
        {
            return _wdsUnit.BankSolvencyRepository.GetBankList();
        }

        public List<BankBranch> GetBankBranches()
        {
            return _wdsUnit.BankSolvencyRepository.GetBankBranchList();
        }
    }
}