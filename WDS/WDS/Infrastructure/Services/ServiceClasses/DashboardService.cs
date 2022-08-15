using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    public class DashboardService : IDashboardService
    {
        //private readonly RbsynergyUnit _rbsynergyUnit;
        private readonly Unit _wdsUnit;

        public DashboardService( Unit wdsUnit)
        {
            //_rbsynergyUnit = rbsynergyUnit;
            _wdsUnit = wdsUnit;
        }

        public bool SaveAdminUpload(VmDashboardImage model)
        {
            try
            {
                var dashboardImage = new DashboardImage
                {
                    ImageDescription = model.ImageDescription,
                    ImageUrl = model.FilePath,
                    IsActive = true,
                    AddedDate = DateTime.Now,
                    AddedBy = 1,
                    IsPopUp = model.IsPopUp
                };
                _wdsUnit.DashboardImageRepository.Add(dashboardImage);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<DashboardImage> GetUploadedImage()
        {
            List<DashboardImage> dashboardImages = _wdsUnit.DashboardImageRepository.Find(i => i.IsActive == true);
            return dashboardImages;
        }

        public VmDashboardCard GetRetailerDailyActivation()
        {
            var dashboardCard = new VmDashboardCard
            {
                CardTitle = "Retailer Today's Activation"
            };
            //DateTime today = DateTime.Today;
            //DateTime yesterDay = today.AddDays(-1);
            //List<tblProductRegistration> registrations = _rbsynergyUnit.ProductRegistrationRepository.Find(i => i.RegistrationDate >= today);
            //List<RetailerDistribution> retailerDistributions = _wdsUnit.RetailerDistributionRepository.GetAll();


            //decimal todayCount = registrations.Where(registration => retailerDistributions.Any(i => i.ImeiOne == registration.Imei_One)).Aggregate<tblProductRegistration, decimal>(0, (current, registration) => current + 1);

            //List<tblProductRegistration> registrationsYesterday =
            //    _rbsynergyUnit.ProductRegistrationRepository.Find(
            //        i => i.RegistrationDate >= yesterDay && i.RegistrationDate < today);

            //decimal yesterdayCount = registrationsYesterday.Where(registration => retailerDistributions.Any(i => i.ImeiOne == registration.Imei_One)).Aggregate<tblProductRegistration, decimal>(0, (current, registration) => current + 1);

            List<DashboardSalesAndActivationCountModel> dashboardSalesAndActivation = _wdsUnit.ProductRepository.GetActivationCount("d");

            var countFirst = dashboardSalesAndActivation.FirstOrDefault();

            if (countFirst != null)
            {
                if (countFirst.TodaysCount > 0)
                {
                    var percentUpDown = ((countFirst.TodaysCount - countFirst.YesterdaysCount) * 100) / countFirst.TodaysCount;
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = percentUpDown;
                }
                else
                {
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = -100;
                }
            }

            return dashboardCard;
        }

        public VmDashboardCard GetRetailerMonthlyActivation()
        {
            var dashboardCard = new VmDashboardCard
            {
                CardTitle = "Retailer Monthly Activation"
            };
            //DateTime now = DateTime.Now;
            //var currentStart = new DateTime(now.Year, now.Month, 1);
            //var currentEnd = currentStart.AddMonths(1).AddDays(-1);

            //var last = DateTime.Now.AddMonths(-1);
            //var lastStart = new DateTime(last.Year, last.Month, 1);
            //var lasttEnd = lastStart.AddMonths(1).AddDays(-1);

            //List<tblProductRegistration> registrations = _rbsynergyUnit.ProductRegistrationRepository.Find(i => i.RegistrationDate >= currentStart && i.RegistrationDate <= currentEnd);
            //List<RetailerDistribution> retailerDistributions = _wdsUnit.RetailerDistributionRepository.GetAll();
            //decimal currentMonthQuantity = registrations.Where(registration => retailerDistributions.Any(i => i.ImeiOne == registration.Imei_One)).Aggregate<tblProductRegistration, decimal>(0, (current, registration) => current + 1);

            //List<tblProductRegistration> lastMonthQuantity = _rbsynergyUnit.ProductRegistrationRepository.Find(i => i.RegistrationDate >= lastStart && i.RegistrationDate <= lasttEnd);
            //decimal yesterdayCount = lastMonthQuantity.Where(registration => retailerDistributions.Any(i => i.ImeiOne == registration.Imei_One)).Aggregate<tblProductRegistration, decimal>(0, (current, registration) => current + 1);

            List<DashboardSalesAndActivationCountModel> dashboardSalesAndActivation = _wdsUnit.ProductRepository.GetActivationCount("m");

            var countFirst = dashboardSalesAndActivation.FirstOrDefault();

            if (countFirst != null)
            {
                if (countFirst.TodaysCount > 0)
                {
                    var percentUpDown = ((countFirst.TodaysCount - countFirst.YesterdaysCount) * 100) / countFirst.TodaysCount;
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = percentUpDown;
                }
                else
                {
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = -100;
                }
            }

            return dashboardCard;
        }

        public VmDashboardCard GetRetailerDailySales()
        {
            var dashboardCard = new VmDashboardCard
            {
                CardTitle = "Retailer Today's Sale"
            };
            //DateTime today = DateTime.Today;
            //DateTime yesterDay = today.AddDays(-1);
            //List<Winner> winners = _retailerMotivationUnit.WinnerRepository.Find(i => i.AddedDate >= today);
            //List<RetailerDistribution> retailerDistributions = _wdsUnit.RetailerDistributionRepository.GetAll();
            //decimal todayCount = winners.Where(winner => retailerDistributions.Any(i => i.ImeiOne == winner.Imei || i.ImeiTwo == winner.Imei)).Aggregate<Winner, decimal>(0, (current, registration) => current + 1);

            //List<Winner> winnerYesterday = _retailerMotivationUnit.WinnerRepository.Find(i => i.AddedDate >= yesterDay && i.AddedDate < today);
            //decimal yesterdayCount = winnerYesterday.Where(winner => retailerDistributions.Any(i => i.ImeiOne == winner.Imei || i.ImeiTwo == winner.Imei)).Aggregate<Winner, decimal>(0, (current, registration) => current + 1);


            List<DashboardSalesAndActivationCountModel> dashboardSalesAndActivation = _wdsUnit.ProductRepository.GetSalesCount("d");

            var countFirst = dashboardSalesAndActivation.FirstOrDefault();

            if (countFirst != null)
            {
                if (countFirst.TodaysCount > 0)
                {
                    var percentUpDown = ((countFirst.TodaysCount - countFirst.YesterdaysCount) * 100) / countFirst.TodaysCount;
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = percentUpDown;
                }
                else
                {
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = -100;
                }
            }


            return dashboardCard;
        }

        public VmDashboardCard GetRetailerMonthlySales()
        {
            var dashboardCard = new VmDashboardCard
            {
                CardTitle = "Retailer Monthly Sale"
            };
            //DateTime now = DateTime.Now;
            //var currentStart = new DateTime(now.Year, now.Month, 1);
            //var currentEnd = currentStart.AddMonths(1).AddDays(-1);

            //var last = DateTime.Now.AddMonths(-1);
            //var lastStart = new DateTime(last.Year, last.Month, 1);
            //var lasttEnd = lastStart.AddMonths(1).AddDays(-1);


            //List<Winner> winners = _retailerMotivationUnit.WinnerRepository.Find(i => i.AddedDate >= currentStart && i.AddedDate <= currentEnd);
            //List<RetailerDistribution> retailerDistributions = _wdsUnit.RetailerDistributionRepository.GetAll();
            //decimal currentMonth = winners.Where(winner => retailerDistributions.Any(i => i.ImeiOne == winner.Imei || i.ImeiTwo == winner.Imei)).Aggregate<Winner, decimal>(0, (current, registration) => current + 1);

            //List<Winner> winnerYesterday = _retailerMotivationUnit.WinnerRepository.Find(i => i.AddedDate >= lastStart && i.AddedDate <= lasttEnd);
            //decimal lastMonthCount = winnerYesterday.Where(winner => retailerDistributions.Any(i => i.ImeiOne == winner.Imei || i.ImeiTwo == winner.Imei)).Aggregate<Winner, decimal>(0, (current, registration) => current + 1);

            List<DashboardSalesAndActivationCountModel> dashboardSalesAndActivation = _wdsUnit.ProductRepository.GetSalesCount("m");

            var countFirst = dashboardSalesAndActivation.FirstOrDefault();

            if (countFirst != null)
            {
                if (countFirst.TodaysCount > 0)
                {
                    var percentUpDown = ((countFirst.TodaysCount - countFirst.YesterdaysCount) * 100) / countFirst.TodaysCount;
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = percentUpDown;
                }
                else
                {
                    dashboardCard.CardQuantity = countFirst.TodaysCount;
                    dashboardCard.Percentage = -100;
                }
            }


            return dashboardCard;
        }

        public List<TopMobileModel> GetTopTenSellingModel(string phoneType)
        {
            //in future store proc need to call
            DateTime now = DateTime.Now;
            var currentStart = new DateTime(now.Year, now.Month, 1);
            var currentEnd = currentStart.AddMonths(1).AddDays(-1);
            List<TopMobileModel> topMobile = _wdsUnit.ProductRepository.GetTopModelSold(phoneType);
            return topMobile;
        }


        public List<DASHBOARD_SALE_INFO_Result> GetDashboardSaleInfo(Distributor distributor)
        {
            var data = _wdsUnit.DashboardImageRepository.DashboardSaleInfo(distributor);
            return data;

        }

        public List<string> GetRoleWiseZone(string userName)
        {
            List<string> zoneNames = _wdsUnit.SalesZoneRepository.GetZoneNamesBySalesTeam(userName);
            return zoneNames;
        }

        public List<SalesZone> GetAllSalesZone()
        {
            return _wdsUnit.SalesZoneRepository.GetAll();
        }

        public List<stp_sr_three_months_incentive_Result> GetSrLastThreeMonthIncentive(string dealerCode)
        {
            return _wdsUnit.SalesRepresentativeRepository.GetSrThreeMonthsIncentive(dealerCode);
        }

        public List<MisPerson> GetMisList(string zoneId, string dealerId, User user)
        {
            Role role = _wdsUnit.RoleRepository.Find(i => i.Id == user.RoleId).FirstOrDefault();
            List<long>zoneIdList;
            List<MisPerson> misPersons;

            if (dealerId != "0")
            {
                misPersons = _wdsUnit.MisPersonRepository.Find(i => i.DealerDigitechCode == dealerId && i.IsActive == true);
                return misPersons;
            }

            if (zoneId != "0")
            {
                long zId = 0;
                long.TryParse(zoneId, out zId);
                zoneIdList = _wdsUnit.SalesZoneRepository.Find(i => i.ZoneId == zId).Select(i => i.ZoneId)
                    .ToList().Distinct().ToList();

            }
            else
            {
                if (role != null && !role.RoleName.ToLower().Contains("admin") &&
                    !role.RoleName.ToLower().Contains("management") && !role.RoleName.ToLower().Contains("nsm"))
                {
                    zoneIdList = _wdsUnit.SalesZoneRepository.Find(i => i.EmpId == user.UserName).Select(i => i.ZoneId)
                        .ToList();
                }
                else
                {
                    zoneIdList = _wdsUnit.ZoneRepository.GetAll().Select(i => i.Id).ToList();
                }

                
            }
            var dealerCodeList = _wdsUnit.DistributorRepository.GetAll()
                .Where(i => zoneIdList.Contains((long)i.ZoneId)).Select(i => i.DigitechCode).ToList();
            misPersons = _wdsUnit.MisPersonRepository.Find(i => dealerCodeList.Contains(i.DealerDigitechCode) && i.IsActive == true);
            return misPersons;
        }


        public vmSRStatusCount GetSRStatusCountData(List<Distributor> distributorlist, string startDate, string endDate)
        {
            var list = new List<string>();
            foreach (var item in distributorlist)
            {
                list.Add(item.DigitechCode);
            }
            string joinedDealerCode = String.Join("','", list.ToArray());
            vmSRStatusCount objvmSRStatusCount = _wdsUnit.SalesRepresentativeRepository.GetSRStatusCountData(joinedDealerCode, startDate, endDate);
            return objvmSRStatusCount;
        }


        public vmRetailerStatusCount GetRetailerStatusCountData(string zoneName, Role role, string zoneId, string dealerId, string startDate, string endDate)
        {
            vmRetailerStatusCount data = _wdsUnit.RetailerInfoRepository.GetRetailerStatusCountData(zoneName, role, zoneId, dealerId, startDate, endDate);
            return data;
        }


        public List<SalesRepresentativeModel> GetAllActiveSr(List<Distributor> distributor, string startDate, string endDate)
        {
            var list = new List<string>();
            foreach (var item in distributor)
            {
                list.Add(item.DigitechCode);
            }
            string joinedDealerCode = String.Join("','", list.ToArray());
            List<SalesRepresentativeModel> data = _wdsUnit.SalesRepresentativeRepository.GetAllActiveSr(joinedDealerCode, startDate, endDate);
            return data;
        }

        public List<SalesRepresentativeModel> GetAllInActiveSr(List<Distributor> distributor, string startDate, string endDate)
        {
            var list = new List<string>();
            foreach (var item in distributor)
            {
                list.Add(item.DigitechCode);
            }
            string joinedDealerCode = String.Join("','", list.ToArray());
            List<SalesRepresentativeModel> data = _wdsUnit.SalesRepresentativeRepository.GetAllInActiveSr(joinedDealerCode, startDate, endDate);
            return data;
        }


        public List<RetailerInfoModel> GetAllActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate)
        {
            
            List<RetailerInfoModel> data = _wdsUnit.RetailerInfoRepository.GetAllActiveRetailer(userName, role,zoneId, dealerId,  startDate, endDate);
            return data;
        }

        public List<RetailerInfoModel> GetAllInActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate)
        {
            List<RetailerInfoModel> data = _wdsUnit.RetailerInfoRepository.GetAllInActiveRetailer(userName, role, zoneId, dealerId, startDate, endDate);
            return data;
        }
    }
}