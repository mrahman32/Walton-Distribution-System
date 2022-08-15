using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    public class ManagementService:IManagementService
    {
        private readonly Unit _wdsUnit;
        private IManagementService _managementServiceImplementation;

        public ManagementService(Unit wdsUnit)
        {
            _wdsUnit = wdsUnit;
        }
        public List<Zone> GetZons()
        {
            List<Zone> zons = _wdsUnit.ZoneRepository.GetAll();
            return zons;
        }
        public List<Brand> GetBrands()
        {
            List<Brand> brands = _wdsUnit.BrandRepository.GetAll();
            return brands;
        }


        public bool SaveMarketShareData(List<MarketShare> modelList)
        {
            try
            {
                foreach (var data in modelList)
                {
                   
                    _wdsUnit.MarketShareRepository.Add(data);
                    _wdsUnit.Commit();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }




        public List<stp_management_area_wise_top_brand_Result> AreaWiseTopBrand(int monthNo, int yearNo, long zoneId)
        {
            var MarketShare = _wdsUnit.MarketShareRepository.AreaWiseTopBrand(monthNo, yearNo, zoneId);
            return MarketShare;
        }

        public List<stp_management_area_wise_market_share_Result> GetAreaWiseWaltonMarketShare(int monthNumber, int yearNumber,long zoneId)
        {
            List<stp_management_area_wise_market_share_Result> data =
                _wdsUnit.MarketShareRepository.GetWaltonMarketShare(monthNumber, yearNumber, zoneId);
            return data;
        }

        public List<stp_management_area_wise_market_share_walton_Result> GetAreaWisePieData(int monthNumber, int yearNumber, long zoneId)
        {
            List<stp_management_area_wise_market_share_walton_Result> results =
                _wdsUnit.MarketShareRepository.GetMonthlyMarketSharePie(monthNumber, yearNumber,zoneId);
            return results;
        }

        public List<stp_market_size_Result> GetMarketSizeTile(int monthNo, int yearNo, long zoneId)
        {
            List<stp_market_size_Result> results =
                _wdsUnit.MarketShareRepository.GetMarketSize(monthNo, yearNo, zoneId);
            return results;
        }

        public List<stp_management_retailer_count_Result> GetRetailerCount(long zoneId)
        {
            List<stp_management_retailer_count_Result> results =
                _wdsUnit.MarketShareRepository.GetRetailerCount(zoneId);
            return results;
        }

        public List<stp_management_retailer_list_zone_Result> GetRetailers(long zoneId)
        {
            List<stp_management_retailer_list_zone_Result> results =
                _wdsUnit.MarketShareRepository.GetRetailers(zoneId);
            return results;
        }


        public bool DubValueCheck(MarketShare objModel)
        {
            try
            {

             var data =  _wdsUnit.MarketShareRepository.Find(x=>x.MonthName == objModel.MonthName &&
                    x.YearNumber == objModel.YearNumber && 
                    x.ZoneId==objModel.ZoneId &&
                    x.BrandId == objModel.BrandId);
             if (data.Count >0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
                
            }
            catch (Exception e)
            {
                return true;
            }
        }
    }
}