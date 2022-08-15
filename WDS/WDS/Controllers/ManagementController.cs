using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.ViewModels;

namespace WDS.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        // GET: Management
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MarketShare()
        {
            List<Zone> zons = _managementService.GetZons();
            List<Brand> brands = _managementService.GetBrands();

            List<SelectListItem> zonsList = new List<SelectListItem> {
                new SelectListItem() {Value = "", Text = "--Select Zone--"},
            }; 
            zonsList.AddRange(zons.Select(i => new SelectListItem{
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
        [HttpPost]
        public ActionResult SaveMarketShareData(List<MarketShare> marketShareList)
        {
            if (HttpContext.Session != null)
            {
                if (marketShareList.Any())
                {
                    bool result = _managementService.SaveMarketShareData(marketShareList);

                    if (result)
                    {
                        var messageAndReload = new
                        {
                            m = result,
                            isRedirect = true,
                            MassageType = 1,
                            Message = "Saved successfully.",
                            redirectUrl = Url.Action("MarketShare")
                        };
                        return Json(messageAndReload, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var messageAndReload = new
                        {
                            m = result,
                            isRedirect = false,
                            MassageType = 2,
                            Message = "Something went wrong!.",
                            redirectUrl = Url.Action("MarketShare")
                        };
                        return Json(messageAndReload, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            return RedirectToAction("Logoff", "Auth");
       }
          [HttpPost]
        public ActionResult DubValueCheck(MarketShare objMarketShare)
        {
            bool result = _managementService.DubValueCheck(objMarketShare);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AreaWiseTopBrandData(string period, string zone)
        {
            long zoneId = 0;
            long.TryParse(zone, out zoneId);

            int monthNo = 0;
            int yearNo = 0;

            if (!string.IsNullOrWhiteSpace(period))
            {
                var periodArr = period.Split(' ');
                monthNo = DateTime.ParseExact(periodArr[0], "MMMM", CultureInfo.InvariantCulture).Month; ;
                yearNo = 0;
                int.TryParse(periodArr[1], out yearNo);
            }

            List<stp_management_area_wise_top_brand_Result> data = _managementService.AreaWiseTopBrand(monthNo, yearNo, zoneId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreaWiseWaltonMarketShare(string period, string zone)
        {
            long zoneId = 0;
            long.TryParse(zone, out zoneId);

            int monthNo = 0;
            int yearNo = 0;

            if (!string.IsNullOrWhiteSpace(period))
            {
                var periodArr = period.Split(' ');
                monthNo = DateTime.ParseExact(periodArr[0], "MMMM", CultureInfo.InvariantCulture).Month; ;
                yearNo = 0;
                int.TryParse(periodArr[1], out yearNo);
            }

            List<stp_management_area_wise_market_share_Result> data = _managementService.GetAreaWiseWaltonMarketShare(monthNo, yearNo, zoneId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreaWisePieData(string period, string zone, string zoneTxt)
        {
            long zoneId = 0;
            long.TryParse(zone, out zoneId);

            int monthNo = 0;
            int yearNo = 0;

            if (!string.IsNullOrWhiteSpace(period))
            {
                var periodArr = period.Split(' ');
                monthNo = DateTime.ParseExact(periodArr[0], "MMMM", CultureInfo.InvariantCulture).Month; ;
                yearNo = 0;
                int.TryParse(periodArr[1], out yearNo);
            }
            

            List<stp_management_area_wise_market_share_walton_Result> data = _managementService.GetAreaWisePieData(monthNo, yearNo,zoneId);
            foreach (var ite in data)
            {
                if (ite.ZoneName== zoneTxt)
                {
                    ite.ZoneStatus = true; 
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMarketSizeTilesData(string period, string zone)
        {
            long zoneId = 0;
            long.TryParse(zone, out zoneId);

            int monthNo = 0;
            int yearNo = 0;

            if (!string.IsNullOrWhiteSpace(period))
            {
                var periodArr = period.Split(' ');
                monthNo = DateTime.ParseExact(periodArr[0], "MMMM", CultureInfo.InvariantCulture).Month; ;
                yearNo = 0;
                int.TryParse(periodArr[1], out yearNo);
            }


            List<stp_market_size_Result> data = _managementService.GetMarketSizeTile(monthNo, yearNo, zoneId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }




        public ActionResult RetailerInformations()
        {
            List<Zone> zons = _managementService.GetZons();

            List<SelectListItem> zonsList = new List<SelectListItem> {
                new SelectListItem() {Value = "", Text = "--Select Zone--"},
            };
            zonsList.AddRange(zons.Select(i => new SelectListItem
                {
                    Text = i.ZoneName,
                    Value = i.Id.ToString()
                })
            );
            ViewBag.AllZoneList = zonsList;
            return View();
        }

        public ActionResult GetRetailerCount(string zone)
        {
            long zoneId = 0;
            long.TryParse(zone, out zoneId);

            

            List<stp_management_retailer_count_Result> data = _managementService.GetRetailerCount(zoneId);

            if (data.Any())
            {
                stp_management_retailer_count_Result result = data.FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRetailers(string zone)
        {
            long zoneId = 0;
            long.TryParse(zone, out zoneId);



            List<stp_management_retailer_list_zone_Result> data = _managementService.GetRetailers(zoneId);
            return new JsonResult()
            {
                Data = data,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}