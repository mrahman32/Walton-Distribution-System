using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Helper;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IDealerService _dealerService;
        private readonly IManagementService _managementService;
        private readonly IAdminService _adminService;

        public DashboardController(IDashboardService dashboardService, IDealerService dealerService, IManagementService managementService, IAdminService adminService)
        {
            _dashboardService = dashboardService;
            _dealerService = dealerService;
            _managementService = managementService;
            _adminService = adminService;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaDashboard()
        {
            return View();
        }
        public ActionResult AdminDashboard()
        {
            return View();
        }

        public ActionResult AdminUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminUpload(VmDashboardImage model)
        {
            var fileManager = new FileManager();
            model.FilePath = fileManager.UploadImage(model.FileBase);
            bool result = _dashboardService.SaveAdminUpload(model);

            return View();
        }
        public ActionResult DealerDashboard()
        {
            FileManager fileManager = new FileManager();
            List<DashboardImage> dashboardImages = _dashboardService.GetUploadedImage();
            //ViewBag.saleResult = _dashboardService.GetDashboardSaleInfo();
            foreach (var dashboardImage in dashboardImages)
            {
                var webServerPath = fileManager.GetFile(dashboardImage.ImageUrl);
                dashboardImage.ImageUrl = webServerPath;
            }
            return View(dashboardImages);
        }
        public ActionResult RetailerDashboard()
        {
            
            return View();
        }

        public ActionResult DashBoardSaleInfo()
        {
            var user = HttpContext.Session["user"] as User;
            if (user == null) return RedirectToAction("Logoff", "Auth");
            //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);

            Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
            var result = _dashboardService.GetDashboardSaleInfo(distributor);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AsmDashboard()
        {
            return View();
        }

        public ActionResult ManagementDashboard()
        {
            List<Zone> zons = _managementService.GetZons();
            List<Brand> brands = _managementService.GetBrands();

            List<SelectListItem> zonsList = new List<SelectListItem> {
                new SelectListItem() {Value = "", Text = "--Select Zone--"},
            };
            zonsList.AddRange(zons.Select(i => new SelectListItem
            {
                Text = i.ZoneName,
                Value = i.Id.ToString()
            })
            );
            ///////////////////////////////////////////

            List<SelectListItem> brandList = new List<SelectListItem> {
                new SelectListItem() {Value = "", Text = "--Select Brand--"},
            };
            brandList.AddRange(brands.Select(i => new SelectListItem
            {
                Text = i.BrandName,
                Value = i.Id.ToString()
            })
            );

            ViewBag.AllZoneList = zonsList;
            ViewBag.AllBrandList = brandList;
            return View();
        }


        public ActionResult GeneralInfoDashboard()
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;

                if (user == null) return RedirectToAction("Logoff", "Auth");


                Role role = _adminService.GetRoleById(user.RoleId);
                List<Distributor> distributors = new List<Distributor>();
                List<Zone> zones = new List<Zone>();
                List<SelectListItem> zoneItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "--All--" } };
                if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management") ||
                    role.RoleName.ToLower().Contains("nsm") || role.RoleName.ToLower().Contains("rsm") ||
                    role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                {
                    bool isUserLikeZoneName = false;
                    distributors = _dealerService.GetAllDistributors();
                    zones = _dealerService.GetAllZones();
                    if (role.RoleName.ToLower().Contains("tso"))
                    {
                        isUserLikeZoneName = zones.Any(i => i.ZoneName == user.UserName);
                        zoneItems = new List<SelectListItem>();
                    }
                    if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                    {
                        List<string> zoneNames = _dashboardService.GetAllSalesZone().Where(i=>i.EmpId== user.UserName).Select(i=>i.ZoneName).ToList();
                        zones = zones.Where(i => zoneNames.Contains(i.ZoneName)).ToList();
                        distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
                    }
                    //if(role.RoleName.ToLower().Contains("tso") && isUserLikeZoneName)
                    //{
                    //    List<string> zoneNames = _dashboardService.GetAllSalesZone().Where(i => i.ZoneName == user.UserName).Select(i => i.ZoneName).ToList();
                    //    zones = zones.Where(i => zoneNames.Contains(i.ZoneName)).ToList();
                    //    distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
                    //}
                    //else
                    //{
                    //    List<string> zoneNames = new List<string>{user.UserName};
                    //    zones = zones.Where(i => zoneNames.Contains(i.ZoneName)).ToList();
                    //    distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
                    //}
                    zoneItems.AddRange(zones.Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.ZoneName
                    }));
                }
                else
                {
                    zones = _dealerService.GetAllZones().Where(i=>i.ZoneName == user.UserName).ToList();
                    distributors = _dealerService.GetAllDistributorByZone(user.UserName);
                    zoneItems=new List<SelectListItem>();
                    zoneItems.AddRange(zones.Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.ZoneName
                    }));
                }

                
                
                
                List<ProductMaster> productMaster = _dealerService.GetAllProductModels();
                List<SelectListItem> modelList = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "0", Text = "--ALL--"
                }
                };
                List<SelectListItem> items = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "0", Text = "--ALL--"
                }
                };
                items.AddRange(distributors.Select(m =>
                    new SelectListItem
                    {
                        Value = m.DigitechCode,
                        Text = m.DistributorNameCellCom + "--" + m.Zone
                    })
                );

                modelList.AddRange(productMaster.Select(m =>
                    new SelectListItem
                    {
                        Value = m.ProductModel,
                        Text = m.ProductModel
                    })
                );
                ViewBag.Dealers = items;
                ViewBag.modelList = modelList;
                ViewBag.Zones = zoneItems;

                return View();
            }

            return RedirectToAction("Logoff", "Auth");
        }

         [HttpPost]
        public JsonResult SRStatusData(string zoneId, string dealerId, string startDate, string endDate)
        {
            User user = HttpContext.Session["user"] as User;
            if (user == null) return null;
            long zoneIdParam = 0;
            long.TryParse(zoneId, out zoneIdParam);
            DateTime startD = Convert.ToDateTime(startDate);
            DateTime endD = Convert.ToDateTime(endDate);
            Role role = _adminService.GetRoleById(user.RoleId);
            //List<SalesZone> salesZones = _dashboardService.GetAllSalesZone();
            List<Distributor> distributors = new List<Distributor>();
            distributors = _dealerService.GetAllDistributors();
            if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management") ||
                role.RoleName.ToLower().Contains("nsm") || role.RoleName.ToLower().Contains("rsm") ||
                role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
            {

                if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                {
                    List<string> zoneNames = _dashboardService.GetRoleWiseZone(user.UserName);
                    if (zoneIdParam > 0)
                    {
                        distributors = distributors.Where(i => i.ZoneId == zoneIdParam).ToList();
                    }
                    else
                    {
                        distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
                    }

                }
                else
                {
                    if (zoneIdParam > 0)
                    {
                        distributors = distributors.Where(i => i.ZoneId == zoneIdParam).ToList();
                    }
                }

            }
            else
            {
                distributors = _dealerService.GetAllDistributorByZone(user.UserName);
            }

            if (dealerId != "0")
            {
                distributors = distributors.Where(i => i.DigitechCode == dealerId).ToList();
            }
            var statusdata = _dashboardService.GetSRStatusCountData(distributors, startDate, endDate);
            return new JsonResult()
            {
                Data = statusdata,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

         [HttpPost]
         public JsonResult DealerStockData(vmDealerAndRetailerStock objvmDealerAndRetailerStock)
         {
             if (HttpContext.Session != null)
             {
                 User user = HttpContext.Session["user"] as User;
                 if (user == null) return null;
                 Role role = _adminService.GetRoleById(user.RoleId);


                 objvmDealerAndRetailerStock.userName = user.UserName;

                 DateTime futurDate = Convert.ToDateTime(objvmDealerAndRetailerStock.EndDate);
                 DateTime TodayDate = Convert.ToDateTime(objvmDealerAndRetailerStock.StartDate);
                 var numberOfDays = Convert.ToInt32((futurDate - TodayDate).TotalDays);
                 if (numberOfDays <= 90)
                 {
                     List<stp_distributor_sales_and_stock_Result> dealerStockData = _dealerService.GetDealerStockAndSaleData(objvmDealerAndRetailerStock, role);

                     List<stp_distributor_sales_and_stock_Result> dealerStockDataSum = new List<stp_distributor_sales_and_stock_Result>();
                     if (objvmDealerAndRetailerStock.Category =="0")
                     {
                         dealerStockDataSum.Add(new stp_distributor_sales_and_stock_Result()
                         {
                             ProductyType = "Smart",
                             SalesQuantity = dealerStockData.Where(x => x.ProductyType == "Smart").Sum(item => Convert.ToInt32(item.SalesQuantity)),
                             SalesValue = dealerStockData.Where(x => x.ProductyType == "Smart").Sum(item => Convert.ToInt32(item.SalesValue)),
                             PresentStock = dealerStockData.Where(x => x.ProductyType == "Smart").Sum(item => Convert.ToInt32(item.PresentStock)),
                             StockValue = dealerStockData.Where(x => x.ProductyType == "Smart").Sum(item => Convert.ToInt32(item.StockValue))
                         });
                         dealerStockDataSum.Add(new stp_distributor_sales_and_stock_Result()
                         {
                             ProductyType = "Feature",
                             SalesQuantity = dealerStockData.Where(x => x.ProductyType == "Feature").Sum(item => Convert.ToInt32(item.SalesQuantity)),
                             SalesValue = dealerStockData.Where(x => x.ProductyType == "Feature").Sum(item => Convert.ToInt32(item.SalesValue)),
                             PresentStock = dealerStockData.Where(x => x.ProductyType == "Feature").Sum(item => Convert.ToInt32(item.PresentStock)),
                             StockValue = dealerStockData.Where(x => x.ProductyType == "Feature").Sum(item => Convert.ToInt32(item.StockValue))
                         });
                     }
                     else
                     {
                         dealerStockDataSum.Add(new stp_distributor_sales_and_stock_Result()
                         {
                             ProductyType = objvmDealerAndRetailerStock.Category,
                             SalesQuantity = dealerStockData.Sum(item => Convert.ToInt32(item.SalesQuantity)),
                             SalesValue = dealerStockData.Sum(item => Convert.ToInt32(item.SalesValue)),
                             PresentStock = dealerStockData.Sum(item => Convert.ToInt32(item.PresentStock)),
                             StockValue = dealerStockData.Sum(item => Convert.ToInt32(item.StockValue))
                         });
                     }

                    
                     return new JsonResult()
                     {
                         Data = dealerStockDataSum,
                         MaxJsonLength = 86753090,
                         JsonRequestBehavior = JsonRequestBehavior.AllowGet
                     };
                 }
                 else
                 {
                     return new JsonResult()
                     {
                         Data = 0,
                         MaxJsonLength = 86753090,
                         JsonRequestBehavior = JsonRequestBehavior.AllowGet
                     };
                 }

             }
             else
             {
                 return new JsonResult()
                 {
                     Data = 0,
                     MaxJsonLength = 86753090,
                     JsonRequestBehavior = JsonRequestBehavior.AllowGet
                 };
             }

         }



         [HttpPost]
         public JsonResult RetailerStatusData(string zoneId, string dealerId, string startDate, string endDate)
         {
             User user = HttpContext.Session["user"] as User;
             if (user == null) return null;

             Role role = _adminService.GetRoleById(user.RoleId);

             var statusdata = _dashboardService.GetRetailerStatusCountData(user.UserName, role, zoneId, dealerId, startDate, endDate);
             return new JsonResult()
             {
                 Data = statusdata,
                 MaxJsonLength = 86753090,
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
         }

         [HttpPost]
         public JsonResult GetAllSrActiveAndInActiveDetailsData(string zoneId, string dealerId, string startDate, string endDate)
         {
             long zId = 0;
             long.TryParse(zoneId, out zId);
             User user = HttpContext.Session["user"] as User;
             Role role = _adminService.GetRoleById(user.RoleId);
             List<Distributor> distributors = new List<Distributor>();
             distributors = _dealerService.GetAllDistributors();
             if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management") ||
                 role.RoleName.ToLower().Contains("nsm") || role.RoleName.ToLower().Contains("rsm") ||
                 role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
             {

                 if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                 {
                     if (zId > 0)
                     {
                         distributors = distributors.Where(i => i.ZoneId == zId).ToList();
                     }
                     else
                     {
                         List<string> zoneNames = _dashboardService.GetRoleWiseZone(user.UserName);
                         distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
                     }
                 }
                 else
                 {
                     if (zId > 0)
                     {
                         distributors = distributors.Where(i => i.ZoneId == zId).ToList();
                     }
                 }

             }
             else
             {
                 distributors = _dealerService.GetAllDistributorByZone(user.UserName);
                 
             }

             if (dealerId != "0")
             {
                 distributors = distributors.Where(i => i.DigitechCode == dealerId).ToList();
             }
             var activeData = _dashboardService.GetAllActiveSr(distributors, startDate, endDate);
             var inactiveData = _dashboardService.GetAllInActiveSr(distributors, startDate, endDate);
             var returnObj = new
             {
                 activeData = activeData,
                 inactiveData = inactiveData

             };
             return new JsonResult()
             {
                 Data = returnObj,
                 MaxJsonLength = 86753090,
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
         }

         [HttpPost]
         public JsonResult GetAllRetailerActiveAndInActiveDetailsData(string zoneId, string dealerId, string startDate, string endDate)
         {
             User user = HttpContext.Session["user"] as User;
             if (user == null) return null;
             Role role = _adminService.GetRoleById(user.RoleId);

             var activeData = _dashboardService.GetAllActiveRetailer(user.UserName,role, zoneId, dealerId, startDate, endDate);
             var inactiveData = _dashboardService.GetAllInActiveRetailer(user.UserName, role, zoneId, dealerId,  startDate, endDate);
             var returnObj = new
             {
                 activeData = activeData,
                 inactiveData = inactiveData

             };
             return new JsonResult()
             {
                 Data = returnObj,
                 MaxJsonLength = 86753090,
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
         }


        #region Dashboard Common tiles and graphs

        [HttpPost]
        public ActionResult GetRetailerDailySales()
        {
            VmDashboardCard dashboardCard = _dashboardService.GetRetailerDailySales();
            return PartialView("~/Views/Shared/_DbTotalRetailerSalesDaily.cshtml", dashboardCard);
        }


        [HttpPost]
        public ActionResult GetRetailerMonthlySalesQty()
        {
            VmDashboardCard dashboardCard = _dashboardService.GetRetailerMonthlySales();
            return PartialView("~/Views/Shared/_DbTotalRetailerSalesMonthly.cshtml", dashboardCard);
        }

        [HttpPost]
        public ActionResult GetRetailerDailyActivationQuantity()
        {
            VmDashboardCard dashboardCard = _dashboardService.GetRetailerDailyActivation();
            return PartialView("~/Views/Shared/_DbTotalRetailerActivationDaily.cshtml", dashboardCard);
        }

        [HttpPost]
        public ActionResult GetRetailerMonthlyActivationQuantity()
        {
            VmDashboardCard dashboardCard = _dashboardService.GetRetailerMonthlyActivation();
            return PartialView("~/Views/Shared/_DbTotalRetailerActivationMonthly.cshtml", dashboardCard);
        }



        [HttpPost]
        public JsonResult GetTopTenSmartPhone()
        {
            var topMobiles = new List<TopMobileModel>();
            topMobiles = _dashboardService.GetTopTenSellingModel("Smart");
            return new JsonResult{Data = topMobiles, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [HttpPost]
        public JsonResult GetTopTenFeaturePhone()
        {
            var topMobiles = new List<TopMobileModel>();
            topMobiles = _dashboardService.GetTopTenSellingModel("Feature");
            return new JsonResult { Data = topMobiles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        [HttpPost]
        public JsonResult GetZoneWiseDistributors(string zoneId)
        {
            User user = HttpContext.Session["user"] as User;
            if (user == null) return null;
            Role role = _adminService.GetRoleById(user.RoleId);
            long zId = 0;
            Int64.TryParse(zoneId, out zId);
            List<Distributor> distributors;
            if (zId==0)
            {
                if(role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management") || role.RoleName.ToLower().Contains("nsm"))
                {
                    distributors = _dealerService.GetAllDistributors().ToList();
                }
                else
                {
                    var isUserNameLikeZoneName = _dealerService.GetAllZones().Any(i => i.ZoneName == user.UserName);
                    if(role.RoleName.ToLower().Contains("tso") && isUserNameLikeZoneName)
                    {
                        distributors = _dealerService.GetAllDistributors().Where(i => i.Zone == user.UserName).ToList();
                    }
                    else
                    {
                        List<string> zoneListByUserRole = _dashboardService.GetRoleWiseZone(user.UserName);
                        distributors = _dealerService.GetAllDistributors()
                            .Where(i => zoneListByUserRole.Contains(i.Zone)).ToList();
                    }
                }
               
            }
            else
            {
                distributors = _dealerService.GetAllDistributors().Where(i => i.ZoneId == zId).ToList();
            }

           
            List<SelectListItem> distributorItems = new List<SelectListItem>{new SelectListItem{Value = "0", Text = "All"}};
            distributorItems.AddRange(distributors.Select(i=>new SelectListItem
            {
                Value = i.DigitechCode, Text = i.DistributorNameCellCom + "--" + i.Zone
            }));
            return new JsonResult {Data = distributorItems, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [HttpPost]
        public JsonResult GetSrIncentiveDetail(string dealerCode)
        {
            User user = HttpContext.Session["user"] as User;
            if (user == null) return null;
            Role role = _adminService.GetRoleById(user.RoleId);
            List<stp_sr_three_months_incentive_Result> results = new List<stp_sr_three_months_incentive_Result>();
            if (dealerCode == "0" && !role.RoleName.ToLower().Contains("admin") && !role.RoleName.ToLower().Contains("management") && !role.RoleName.ToLower().Contains("nsm"))
            {
                List<string> dealerCodeList = new List<string>();
                List<string> zoneList = _dashboardService.GetRoleWiseZone(user.UserName);
                if (zoneList.Count <= 0)
                {
                    zoneList.Add(user.UserName);
                }
                foreach (var zone in zoneList)
                {
                    var dealers = _dealerService.GetDistributorsByZone(zone).Select(i => i.DigitechCode).ToList();
                    dealerCodeList.AddRange(dealers);
                }

                dealerCodeList = dealerCodeList.Distinct().ToList();
                foreach (var dCode in dealerCodeList)
                {
                    var res = _dashboardService.GetSrLastThreeMonthIncentive(dCode);
                    results.AddRange(res);
                }
            }
            else
            {
                results = _dashboardService.GetSrLastThreeMonthIncentive(dealerCode);
            }
            return new JsonResult{Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
        [HttpPost]
        public JsonResult GetMisDetail(string zoneId, string dealerId)
        {
            User user = HttpContext.Session["user"] as User;
            if (user != null)
            {
                List<MisPerson> misPersons = _dashboardService.GetMisList(zoneId, dealerId, user);
                return new JsonResult{Data = misPersons, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            }

            return null;

        }
    }
}