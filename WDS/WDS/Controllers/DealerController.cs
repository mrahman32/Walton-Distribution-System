using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.Ajax.Utilities;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Helper;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Controllers
{
    public class DealerController : Controller
    {

        private readonly IDealerService _dealerService;
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly IDashboardService _dashboardService;

        public DealerController(IDealerService dealerService, IAuthService authService, IAdminService adminService, IDashboardService dashboardService)
        {
            _dealerService = dealerService;
            _authService = authService;
            _adminService = adminService;
            _dashboardService = dashboardService;
        }

        // GET: Dealer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Retailers()
        {
            var user = HttpContext.Session["user"] as User;
            List<RetailerInfo> retailerInfos = new List<RetailerInfo>();
            if (user != null)
            {

                Distributor distributor = _dealerService.GetDistributorByPhone(user.MobileNumber);
                retailerInfos = _dealerService.GetRetailersByDealerId(distributor);
            }
            return View(retailerInfos);
        }

        public ActionResult CreateRetailer()
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    List<SelectListItem> dealers = _dealerService.GetDealerSelectList();
                    List<SelectListItem> division = _dealerService.GetDivisionSelectList();
                    List<SelectListItem> brand = _dealerService.GetTopSellingBrandSelectList();
                    //tblDealerInfo dealerInfos = _dealerService.GetDealerByPhoneNumber(user.UserName);

                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    var dealer = dealers.FirstOrDefault(i => i.Value.Contains(distributor.DigitechCode));
                    if (dealer != null)
                    {
                        var items = new List<SelectListItem>
                        {
                            new SelectListItem
                            {
                                Value = dealer.Value,
                                Text = dealer.Text
                            }
                        };

                        ViewBag.Dealers = items;
                        ViewBag.divisionList = division;
                        ViewBag.sellingBrandList = brand;
                    }
                }
            }


            //ViewBag.Dealers = dealers;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRetailer(RetailerInfoModel model)
        {
            List<SelectListItem> dealers = _dealerService.GetDealerSelectList();
            ViewBag.Dealers = dealers;
            List<SelectListItem> division = _dealerService.GetDivisionSelectList();
            List<SelectListItem> brand = _dealerService.GetTopSellingBrandSelectList();
            ViewBag.divisionList = division;
            ViewBag.sellingBrandList = brand;


            if (ModelState.IsValid && model != null)
            {
                var fileManager = new FileManager();
                model.ImagePath = fileManager.UploadImage(model.RetailerImage);
                string result = _dealerService.SaveRetailer(model);
                if (result.Equals("success"))
                {
                    TempData["message"] = "Retailer saved successfully.";
                    TempData["messageType"] = 1;
                    return View(new RetailerInfoModel());
                }
                if (!result.Equals("success"))
                {
                    TempData["message"] = result;
                    TempData["messageType"] = 2;
                    return View(model);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetDistrictList(string divisionId)
        {
            long divisinId;
            List<SelectListItem> districtNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(divisionId))
            {
                divisinId = Convert.ToInt64(divisionId);
                districtNames = _dealerService.GetDistrictSelectListBydivisionId(divisinId);
            }
            return Json(districtNames, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetThanaListByDistrict(string districtId)
        {
            long disId;
            List<SelectListItem> thanaNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(districtId))
            {
                disId = Convert.ToInt64(districtId);
                thanaNames = _dealerService.GetThanaSelectListByDistrictId(disId);
            }
            return Json(thanaNames, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditRetailer(long id)
        {

            var user = HttpContext.Session["user"] as User;
            if (user != null)
            {
                //tblDealerInfo dealerInfos = _dealerService.GetDealerByPhoneNumber(user.UserName);
                Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                var items = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = distributor.DigitechCode + "~" + distributor.Zone + "~" + distributor.DistributorNameCellCom,
                        Text = distributor.DistributorNameCellCom + "- (" + distributor.Zone + ")"
                    }
                };

                ViewBag.Dealers = items;
                List<SelectListItem> division = _dealerService.GetDivisionSelectList();
                List<SelectListItem> district = _dealerService.GetDistrictSelectList();
                List<SelectListItem> thana = _dealerService.GetThanaSelectList();
                List<SelectListItem> brand = _dealerService.GetTopSellingBrandSelectList();
                ViewBag.divisionList = division;
                ViewBag.sellingBrandList = brand;
                ViewBag.districtList = district;
                ViewBag.thanaList = thana;
            }

            RetailerInfo retailer = _dealerService.GetRetailerById(id);

            var config = new MapperConfiguration(c => c.CreateMap<RetailerInfo, RetailerInfoModel>());
            var mapper = config.CreateMapper();
            RetailerInfoModel model = mapper.Map<RetailerInfoModel>(retailer);

            model.DistributorName = model.DistributorCode + "~" + model.District + "~" +
                                       model.DistributorName;
            //List<SelectListItem> dealers = _dealerService.GetDealerSelectList();
            //ViewBag.Dealers = dealers;
            return View(model);
        }


        [HttpPost]
        public ActionResult EditRetailer(RetailerInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = HttpContext.Session["user"] as User;
                var dealerArray = model.DistributorName.Split('~');
                model.DistributorCode = dealerArray[0];
                model.District = dealerArray[1];
                model.DistributorName = dealerArray[2];
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = user.Id;

                //_dealerService.EditRetailer(model);
                string result = _dealerService.SaveRetailerUpdateLog(model);
                if (result.Equals("success"))
                {
                    TempData["message"] = "Update request saved successfully.";
                    TempData["messageType"] = 1;
                    return RedirectToAction("Retailers");
                }
                if (!result.Equals("success"))
                {
                    TempData["message"] = result;
                    TempData["messageType"] = 2;
                    return RedirectToAction("EditRetailer", new { id = model.Id });
                }
                model.DistributorName = model.DistributorCode + "~" + model.District + "~" +
                                       model.DistributorName;
            }
            //List<SelectListItem> dealers = _dealerService.GetDealerSelectList();

            //ViewBag.Dealers = dealers;
            //List<SelectListItem> division = _dealerService.GetDivisionSelectList();
            //List<SelectListItem> district = _dealerService.GetDistrictSelectList();
            //List<SelectListItem> thana = _dealerService.GetThanaSelectList();
            //List<SelectListItem> brand = _dealerService.GetTopSellingBrandSelectList();
            //ViewBag.divisionList = division;
            //ViewBag.sellingBrandList = brand;
            //ViewBag.districtList = district;
            //ViewBag.thanaList = thana;
            return View(model);
        }

        public JsonResult DeactivateRetailer(long id)
        {
            if (HttpContext.Session == null) return new JsonResult
            {
                Data = new AjaxResponseModel
                {
                    Id = 0,
                    Message = "Session time out. Pls logout and login again."
                }
            };

            var responseModel = new AjaxResponseModel();
            bool result = _dealerService.DeactivateRetailer(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "Retailer Deactivate.";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };

        }

        public ActionResult PrintRetailer(long rId)
        {
            Brand brand = new Brand();
            RetailerInfoModel infoModelObj = new RetailerInfoModel();
            RetailerInfo retailer = _dealerService.GetRetailerById(rId);

            infoModelObj.Id = retailer.Id;
            infoModelObj.District = retailer.District;
            infoModelObj.DistributorName = retailer.DistributorName;
            infoModelObj.DistributorCode = retailer.DistributorCode;
            infoModelObj.RetailerName = retailer.RetailerName;
            infoModelObj.RetailerAddress = retailer.RetailerAddress;
            infoModelObj.OwnerName = retailer.OwnerName;
            infoModelObj.PhoneNumber = retailer.PhoneNumber;
            infoModelObj.PaymentNumberType = retailer.PaymentNumberType;
            infoModelObj.PaymentNumber = retailer.PaymentNumber;
            infoModelObj.DistributorCode2 = retailer.DistributorCode2;
            infoModelObj.ThanaName = retailer.ThanaName;
            infoModelObj.Division = retailer.Division;
            infoModelObj.Zone = retailer.Zone;
            infoModelObj.ZoneId = retailer.ZoneId;
            infoModelObj.DistrictId = retailer.DistrictId;
            infoModelObj.ThanaId = retailer.ThanaId;
            infoModelObj.DivisionId = retailer.DivisionId;
            infoModelObj.TopSellingBrandId = retailer.TopSellingBrandId;
            infoModelObj.TotalSale = retailer.TotalSale;
            if (retailer.TopSellingBrandId != null)
            {
                brand = _dealerService.GetABrandbyId(retailer.TopSellingBrandId);

                infoModelObj.BrandName = brand.BrandName;
            }

            return View(infoModelObj);
        }



        public ActionResult RetailerOrderList()
        {
            var orders = TempData["orderData"] as List<VmRetailerOrders>;
            if(orders == null) orders = new List<VmRetailerOrders>();
            return View(orders);
        }

        public ActionResult OrderDetail(long orderId)
        {
            VmImeiDelivery imeiDelivery = _dealerService.GetOrderById(orderId);
            return View(imeiDelivery);
        }


        public JsonResult GetImeiInformation(string imei, long retailerId)
        {
            var user = HttpContext.Session["user"] as User;
            DealerDistribution detail = _dealerService.GetImeiInformation(imei);
            Distributor distributor = _dealerService.GetDistributorByPhone(user.MobileNumber);
            if (detail != null)
            {
                int featurePhoneTotalLimit = 1500;
                int featurePhoneModelLimit = 500;
                int smartPhoneTotalLimit = 100;
                int smartPhoneModelLimit = 40;

                var currentDate = DateTime.Now;
                DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);



                if (detail.DealerCode == distributor.DigitechCode || detail.DealerCode == distributor.ImportCode)
                {
                    if (detail.IsDistributed == true)
                    {
                        return Json(new { Success = false, Message = "Already Distributed" });
                    }




                    bool checkTotalLimit = _dealerService.GetTotalLimitResult(smartPhoneTotalLimit, featurePhoneTotalLimit, startDate, endDate, detail, retailerId);
                    if (checkTotalLimit)
                    {
                        bool checkModelLimit = _dealerService.GetModelLimitResult(smartPhoneModelLimit, featurePhoneModelLimit, startDate, endDate, detail, retailerId);
                        if (checkModelLimit)
                        {
                            ModelPrice price = _dealerService.GetModelPriceByModel(detail.Model);
                            if (price != null)
                            {
                                return Json(new { Success = true, Message = "Successful", Data = detail });
                            }
                            else
                            {
                                return Json(new { Success = false, Message = "This model's (" + detail.Model + ") MSDP and Invoice price in not updated in the System. Please contact with administrator." });
                            }

                        }

                        return Json(new { Success = false, Message = "Your monthly distribution limit is over to this retailer for this model(" + detail.Model + "). You are not allowed to distribute (" + detail.Model + ") anymore to this retailer for this month. Please contact with your area manager." });
                    }
                    if (detail.Model.Contains("Primo"))
                        return Json(new { Success = false, Message = "Your monthly smart phone distribution limit is over to this retailer. You are not allowed to distribute smart phone anymore to this retailer for this month. Please contact with your area manager." });

                    return Json(new { Success = false, Message = "Your monthly feature phone distribution limit is over to this retailer. You are not allowed to distribute feature phone anymore to this retailer for this month. Please contact with your area manager." });


                }

                return Json(new { Success = false, Message = "This product distributed to another distributor" });

            }

            return Json(new { Success = false, Message = "IMEI not Found" });
        }


        public JsonResult SaveDelivery(VmImeiDelivery vmImeiDelivery)
        {
            bool result = _dealerService.SaveOrderDelivery(vmImeiDelivery);
            return new JsonResult { Data = "success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ProductIssue()
        {
            var user = HttpContext.Session["user"] as User;
            if (user == null) return RedirectToAction("Logoff", "Auth");
            //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);

            Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
            List<RetailerInfo> retailers = _dealerService.GetRetailers().Where(i => i.DistributorCode == distributor.DigitechCode).ToList();
            List<SelectListItem> retailerItems = new List<SelectListItem>();// { new SelectListItem { Value = "", Text = "<Select>" } };

            retailerItems.AddRange(
                retailers.Select(i =>
                    new SelectListItem
                    {
                        Text = i.RetailerName + " -- (" + i.RetailerAddress + " )",
                        Value = i.Id.ToString(CultureInfo.InvariantCulture)

                    })
                );



            ViewBag.Retailers = retailerItems;


            List<SalesRepresentative> salesRepresentatives = _dealerService.GetSalesRepresentativeByDealerCode(distributor.DigitechCode, distributor.ImportCode);
            List<SelectListItem> salesItems = new List<SelectListItem>();
            //{
            //    new SelectListItem
            //    {
            //        Value = "", Text = "<Select>"
            //    }
            //};
            salesItems.AddRange(
                salesRepresentatives.Select(representative =>
                    new SelectListItem
                    {
                        Value = representative.Id.ToString(CultureInfo.InvariantCulture),
                        Text = representative.Name
                    })
                );

            ViewBag.SalesRepresentativeSelectList = salesItems;

            return View(new VmProductIssue { SaleDate = DateTime.Now });
        }

        [HttpPost]
        public JsonResult ProductIssue(VmProductIssue vmProductIssue)
        {
            if (HttpContext.Session != null)
            {
                if (ModelState.IsValid)
                {
                    var user = HttpContext.Session["user"] as User;
                    if (user != null)
                    {
                        Tuple<long, string> result = _dealerService.SaveProductIssue(vmProductIssue, user);

                        return Json(new { Success = true, Message = result.Item2, Data = result.Item1 });

                    }
                    return Json(new { Success = false, Message = "Session Expired", Data = 0 });
                }

            }
            return Json(new { Success = false, Message = "Session Expired", Data = 0 });
        }

        public JsonResult GetModelPrice(string model)
        {
            //tblCellPhonePricing pricing = _dealerService.GetCellPhonePriceByModel(model);
            ModelPrice price = _dealerService.GetModelPriceByModel(model);
            return new JsonResult { Data = price, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ProductReturn()
        {
            return View(new List<VmProductReturn>());
        }

        [HttpPost]
        public ActionResult ProductReturn(List<VmProductReturn> vmProductReturns)
        {
            bool result = _dealerService.SaveProductReturn(vmProductReturns);


            return RedirectToAction("ProductReturn");
        }
        [HttpPost]
        public ActionResult GetRetailerImei(string imei)
        {
            VmProductReturn productReturn = _dealerService.GetRetailerImeiInfo(imei);
            return PartialView("~/Views/Shared/_RetailerProductReturn.cshtml", productReturn);
        }

        public ActionResult CreateSr()
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    //tblDealerInfo dealerInfos = _dealerService.GetDealerByPhoneNumber(user.UserName);
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    var items = new List<SelectListItem>
                    {
                        new SelectListItem
                        {
                            Value = distributor.DigitechCode,
                            Text = distributor.DistributorNameCellCom
                        }
                    };

                    ViewBag.Dealers = items;
                }
                else
                {
                    return RedirectToAction("Logoff", "Auth");
                }

            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateSr(SalesRepresentativeModel model)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");
            }
            if (ModelState.IsValid)
            {
                bool result = _dealerService.SaveSr(model);

                if (result)
                {
                    TempData["message"] = "SR saved successfully.";
                    TempData["messageType"] = 1;
                }
                else
                {
                    TempData["message"] = "SR could not saved. Please check your internet connection or log in again.";
                    TempData["messageType"] = 2;
                }

                return RedirectToAction("CreateSr");
            }
            return View(model);
        }


        public ActionResult SrList()
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.MobileNumber);
                    List<SalesRepresentative> salesRepresentatives =
                        _dealerService.GetSalesRepresentativesByDealerCode(distributor);
                    return View(salesRepresentatives);
                }
                else
                {
                    return RedirectToAction("Logoff", "Auth");
                }

            }
            return RedirectToAction("Logoff", "Auth");
        }

        public ActionResult EditSr(long id)
        {
            var user = HttpContext.Session["user"] as User;
            if (user != null)
            {
                //tblDealerInfo dealerInfos = _dealerService.GetDealerByPhoneNumber(user.UserName);
                Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                var items = new List<SelectListItem>
                    {
                        new SelectListItem
                        {
                            Value = distributor.DigitechCode,
                            Text = distributor.DistributorNameCellCom
                        }
                    };

                ViewBag.Dealers = items;
                SalesRepresentativeModel salesRepresentative = _dealerService.GetSalesRepresentativeById(id);
                return View(salesRepresentative);
            }
            return RedirectToAction("Logoff", "Auth");
        }

        [HttpPost]
        public ActionResult EditSr(SalesRepresentativeModel model)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    //tblDealerInfo dealerInfos = _dealerService.GetDealerByPhoneNumber(user.UserName);
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);

                    var items = new List<SelectListItem>
                    {
                        new SelectListItem
                        {
                            Value = distributor.DigitechCode,
                            Text = distributor.DistributorNameCellCom
                        }
                    };

                    ViewBag.Dealers = items;
                    _dealerService.UpdateSr(model);
                    return View(model);
                }
            }
            return RedirectToAction("Logoff", "Auth");
        }


        public JsonResult DeactivateSr(long id)
        {
            if (HttpContext.Session == null) return new JsonResult
            {
                Data = new AjaxResponseModel
                {
                    Id = 0,
                    Message = "Session time out. Pls logout and login again."
                }
            };

            var responseModel = new AjaxResponseModel();
            bool result = _dealerService.DeactivateSr(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "SR Deactivate.";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };

        }
        public ActionResult SalesReport()
        {

            return View();
        }
        [HttpPost]
        public ActionResult SalesReport(string startDate, string endDate)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    DateTime stDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", null);
                    DateTime edDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", null);
                    List<VmDealerSalesReport> list = _dealerService.GetDealerSales(distributor.DigitechCode, stDate, edDate);
                    var result = new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    result.MaxJsonLength = int.MaxValue;
                    return result;
                }
            }
            return null;
        }


        [HttpGet]
        public ActionResult StockReport()
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    List<DealerStockCheck_Result> dealerStock = _dealerService.GetDealerStockReport(distributor.DigitechCode, distributor.ImportCode, distributor.DigitechCode);
                    return View(dealerStock);
                }
            }
            return View(new List<DealerStockCheck_Result>());
        }

        public ActionResult Print(long oid)
        {
            if (oid <= 0) return RedirectToAction("RetailerOrderList");
            var user = HttpContext.Session["user"] as User;
            VmPrintProductIssue printProduct = new VmPrintProductIssue();
            if (user != null)
            {
                bool isSafe = false;
                Distributor distributor = _dealerService.GetDistributorByPhone(user.MobileNumber);
                printProduct = _dealerService.GetProductIssuePrintData(oid);
                if (printProduct.DealerCode.Contains(distributor.DigitechCode)) isSafe = true;
                else if (printProduct.DealerCode.Contains(distributor.ImportCode)) isSafe = true;

                if (!isSafe)
                    return RedirectToAction("RetailerOrderList");
            }


            return View(printProduct);
        }

        public ActionResult AddStock()
        {
            return View();
        }

        public JsonResult GetDistribution(string imei)
        {
            bool isExist = _dealerService.CheckImeiIsExist(imei);
            if (isExist) return new JsonResult { Data = "exist", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            User user = HttpContext.Session["user"] as User;
            VmDealerStockAdd distribution = _dealerService.GetRbsynergyDistribution(imei, user);
            if (distribution == null)
            {
                return new JsonResult { Data = "notExist", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = distribution, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult SaveStock(List<VmDealerStockAdd> vmDealerStockAddList)
        {
            AjaxResponseModel model;

            User user = HttpContext.Session["user"] as User;
            if (user == null)
            {
                model = new AjaxResponseModel
                {
                    Id = 2,
                    Message = "Session timeout please log in again."
                };
                return new JsonResult { Data = model };
            }

            bool result = _dealerService.SaveStock(vmDealerStockAddList, user);
            if (result)
            {
                model = new AjaxResponseModel
                {
                    Id = 1,
                    Message = "Data Saved successfully,"
                };
                return new JsonResult { Data = model };
            }


            model = new AjaxResponseModel
            {
                Id = 2,
                Message = "Data Not Saved. Please Try again."
            };
            return new JsonResult { Data = model };
        }


        public ActionResult SrReport()
        {
            if (HttpContext.Session == null) return RedirectToAction("Logoff", "Auth");
            var user = HttpContext.Session["user"] as User;
            if (user == null) return RedirectToAction("Logoff", "Auth");
            //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);


            Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
            List<SalesRepresentative> salesRepresentatives = _dealerService.GetSalesRepresentativeByDealerCode(distributor.DigitechCode, distributor.ImportCode);
            List<SelectListItem> salesItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "ALL",
                    Text = "ALL"
                }
            };
            salesItems.AddRange(
                salesRepresentatives.Select(representative =>
                    new SelectListItem
                    {
                        Value = representative.Id.ToString(CultureInfo.InvariantCulture),
                        Text = representative.Name
                    })
                );

            ViewBag.SalesRepresentativeSelectList = salesItems;
            return View();
        }


        [HttpPost]
        public JsonResult SrReport(string startDate, string endDate, long srId = 0)
        {
            List<stp_sr_report_Result> srReportData = _dealerService.GetSrReport(startDate, endDate, srId);
            return new JsonResult { Data = srReportData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult RetailerStockReport(long retailerId = 0)
        {
            VmRetailerStock retailerStockModel = new VmRetailerStock { RetailerId = retailerId };
            var user = HttpContext.Session["user"] as User;
            if (user == null) return RedirectToAction("Logoff", "Auth");
            //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);

            Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
            List<RetailerInfo> retailers = _dealerService.GetRetailers().Where(i=>i.DistributorCode == distributor.DigitechCode).ToList();
            retailers = retailers.Where(i => i.DistributorCode == distributor.DigitechCode || i.DistributorCode == distributor.ImportCode || i.DistributorCode2 == distributor.DigitechCode || i.DistributorCode2 == distributor.ImportCode).ToList();
            List<SelectListItem> retailerItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "All" } };

            retailerItems.AddRange(
                retailers.Select(i =>
                    new SelectListItem
                    {
                        Text = i.RetailerName + " -- (" + i.RetailerAddress + ")",
                        Value = i.Id.ToString(CultureInfo.InvariantCulture)

                    })
            );



            ViewBag.Retailers = retailerItems;
            List<stp_retailer_stock_Result> retailerStockResults = _dealerService.GetRetailerStock(retailerId, distributor);
            retailerStockModel.StpRetailerStockResults = retailerStockResults;
            return View(retailerStockModel);
        }


        public ActionResult SrSalesReport(long srId = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            VmSrWiseSales srWiseSalesModel = new VmSrWiseSales
            {
                SrId = srId,
                StartDate = startDate,
                EndDate = endDate
            };
            var user = HttpContext.Session["user"] as User;
            if (user == null) return RedirectToAction("Logoff", "Auth");
            //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);


            Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
            List<SalesRepresentative> salesRepresentatives = _dealerService.GetSalesRepresentativeByDealerCode(distributor.DigitechCode, distributor.ImportCode);
            List<SelectListItem> salesItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "All" } };
            salesItems.AddRange(
                salesRepresentatives.Select(representative =>
                    new SelectListItem
                    {
                        Value = representative.Id.ToString(CultureInfo.InvariantCulture),
                        Text = representative.Name
                    })
            );

            ViewBag.SalesRepresentativeSelectList = salesItems;

            if (startDate == null || endDate == null)
                return View(srWiseSalesModel);


            List<stp_sr_sales_report_Result> srSalesReportResults = _dealerService.GetSrSalesReport(srId, startDate, endDate, distributor);
            srWiseSalesModel.StpSrSalesReportResults = srSalesReportResults;
            return View(srWiseSalesModel);
        }

        public ActionResult ProductReceive()
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;
                //tblDealerInfo dealerInfos = _dealerService.GetDealerByPhoneNumber(user.UserName);
                Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                if (distributor != null)
                {
                    List<DealerDistribution> dealerDistribution = _dealerService.GetProductList(distributor, "");
                    List<SelectListItem> modelItems = new List<SelectListItem>();
                    modelItems.AddRange(dealerDistribution.Select(m =>
                     new SelectListItem
                     {
                         Value = m.Model,
                         Text = m.Model
                     })
                 );
                    ViewBag.modelList = modelItems.DistinctBy(x => x.Value);
                    return View(dealerDistribution);
                }
            }
            return RedirectToAction("Logoff", "Auth");
        }

        public ActionResult GetProductReceiveByModel(string model)
        {

            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;
                //tblDealerInfo dealerInfos = _dealerService.GetDealerByPhoneNumber(user.UserName);
                Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                if (distributor != null)
                {
                    List<DealerDistribution> dealerDistribution = _dealerService.GetProductList(distributor, model);

                    return Json(dealerDistribution, JsonRequestBehavior.AllowGet);
                }
            }
            return RedirectToAction("Logoff", "Auth");
        }

        public ActionResult ProductReceiveSave(List<DealerDistributionModel> dealerDistributionList)
        {
            if (HttpContext.Session != null)
            {
                if (dealerDistributionList.Any())
                {
                    bool result = _dealerService.ProductReceiveUpdate(dealerDistributionList);

                    if (result)
                    {
                        var messageAndReload = new
                        {
                            m = result,
                            isRedirect = true,
                            MassageType = 1,
                            Message = "Product Receiveed successfully.",
                            redirectUrl = Url.Action("ProductReceive")
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
                            redirectUrl = Url.Action("ProductReceive")
                        };
                        return Json(messageAndReload, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            return RedirectToAction("Logoff", "Auth");
        }

        public ActionResult DealerStock()
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;

                if (user == null) return RedirectToAction("Logoff", "Auth");
                List<SelectListItem> items = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "0", Text = "--ALL--"
                }
                };

                Role role = _adminService.GetRoleById(user.RoleId);
                List<Distributor> distributors = new List<Distributor>();
                if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management") ||
                    role.RoleName.ToLower().Contains("nsm") || role.RoleName.ToLower().Contains("rsm") ||
                    role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                {
                    distributors = _dealerService.GetAllDistributors();
                    if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                    {
                        List<string> zoneNames = _dashboardService.GetRoleWiseZone(user.UserName);
                        distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
                    }

                }
                else if (role.RoleName.ToLower().Contains("dealer"))
                {
                    var distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    distributors = new List<Distributor> { distributor };
                    items = new List<SelectListItem>();
                }

                //distributor = _dealerService.GetAllDistributorByZone(user.UserName);
                List<ProductMaster> productMaster = _dealerService.GetAllProductModels();
                List<SelectListItem> modelList = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "0", Text = "--ALL--"
                }
                };

                items.AddRange(distributors.Select(m =>
                    new SelectListItem
                    {
                        Value = m.DigitechCode,
                        Text = m.DistributorNameCellCom
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


                return View();
            }

            return RedirectToAction("Logoff", "Auth");
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

                    return new JsonResult()
                    {
                        Data = dealerStockData,
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



        public ActionResult SRReportData()
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;



                Distributor distributor = _dealerService.GetADistributorByZone(user.UserName);
                List<SalesRepresentative> salesRepresentatives = _dealerService.GetSalesRepresentativeByDigitechCode(distributor.DigitechCode);
                List<ProductMaster> productMaster = _dealerService.GetAllProductModels();
                List<SelectListItem> modelList = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "0", Text = "--ALL--"
                }
                };
                List<SelectListItem> srList = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "0", Text = "--ALL--"
                }
                };
                srList.AddRange(salesRepresentatives.Select(m =>
                    new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name,
                    })
                );

                modelList.AddRange(productMaster.Select(m =>
                    new SelectListItem
                    {
                        Value = m.ProductModel,
                        Text = m.ProductModel
                    })
                );
                ViewBag.sRList = srList;
                ViewBag.modelList = modelList;


                return View();
            }

            return RedirectToAction("Logoff", "Auth");
        }

        [HttpPost]
        public JsonResult SRReportByDate(vmDealerAndRetailerStock objvmDealerAndRetailerStock)
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;
                objvmDealerAndRetailerStock.userName = user.UserName;

                DateTime futurDate = Convert.ToDateTime(objvmDealerAndRetailerStock.EndDate);
                DateTime TodayDate = Convert.ToDateTime(objvmDealerAndRetailerStock.StartDate);
                var numberOfDays = Convert.ToInt32((futurDate - TodayDate).TotalDays);
                if (numberOfDays <= 90)
                {
                    //List<stp_distributor_sales_and_stock_Result> dealerStockData = _dealerService.GetDealerStockAndSaleData(objvmDealerAndRetailerStock);

                    return new JsonResult()
                    {
                        Data = 0,
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

        public ActionResult DoaIndex()
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
                    //bool isUserLikeZoneName = false;
                    distributors = _dealerService.GetAllDistributors();
                    zones = _dealerService.GetAllZones();
                    
                    if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                    {
                        List<string> zoneNames = _dashboardService.GetAllSalesZone().Where(i => i.EmpId == user.UserName).Select(i => i.ZoneName).ToList();
                        zones = zones.Where(i => zoneNames.Contains(i.ZoneName)).ToList();
                        distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
                    }

                    //zoneItems.AddRange(zones.Select(i => new SelectListItem
                    //{
                    //    Value = i.Id.ToString(),
                    //    Text = i.ZoneName
                    //}));
                }
                else
                {
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    zones = _dealerService.GetAllZones().Where(i => i.ZoneName == distributor.DigitechCode).ToList();
                    distributors.Add(distributor);// = _dealerService.GetAllDistributorByZone(user.UserName);
                    zoneItems = new List<SelectListItem>();
                    zoneItems.AddRange(zones.Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.ZoneName
                    }));
                }




                //List<ProductMaster> productMaster = _dealerService.GetAllProductModels();
                //List<SelectListItem> modelList = new List<SelectListItem>(){
                //    new SelectListItem
                //    {
                //        Value = "0", Text = "--ALL--"
                //    }
                //};


                List<SelectListItem> distributorSelectListItems = new List<SelectListItem>();
                //if (distributors.Count > 1)
                //{
                //    items.Add(new SelectListItem{Value = "0", Text = "--ALL--"});
                //}
                distributorSelectListItems.AddRange(distributors.Select(m =>
                    new SelectListItem
                    {
                        Value = m.DigitechCode,
                        Text = m.DistributorNameCellCom + "--" + m.Zone
                    })
                );

                //modelList.AddRange(productMaster.Select(m =>
                //    new SelectListItem
                //    {
                //        Value = m.ProductModel,
                //        Text = m.ProductModel
                //    })
                //);
                ViewBag.Dealers = distributorSelectListItems;
                //ViewBag.modelList = modelList;
                //ViewBag.Zones = zoneItems;

                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetDoaReport(string startDate, string endDate, string dealerCode)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    DateTime stDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", null);
                    DateTime edDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", null);
                    List<DistributorDoaQuantityModel> list = _dealerService.GetDistributorDoaQuantityByApi(dealerCode, stDate, edDate);
                    var result = new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    result.MaxJsonLength = int.MaxValue;
                    return result;
                }
            }
            return null;
        }



        [HttpPost]
        public ActionResult GetDoaReportDetail(string startDate, string endDate, string modelName, string recQty, string adjQty, string penQty, string dealerCode)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    //tblDealerInfo dealer = _dealerService.GetDealerByPhoneNumber(user.UserName);
                    Distributor distributor = _dealerService.GetDistributorByPhone(user.UserName);
                    DateTime stDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", null);
                    DateTime edDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", null);
                    DistributorDoaDetailViewModel distributorDoaDetails =
                        _dealerService.GetDistributorDoaDetails(dealerCode, stDate, edDate, modelName,
                            recQty, adjQty, penQty);
                    //string viewString = RenderViewToString(controlle, "", distributorDoaDetails);
                    //var result = new JsonResult { Data = distributorDoaDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //result.MaxJsonLength = int.MaxValue;
                    return PartialView("~/Views/Shared/_DistributorDoaDetails.cshtml", distributorDoaDetails);
                }
            }
            return null;
        }

        public static string RenderViewToString(Controller controller, string viewName, object model)
        {
            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                controller.ViewData.Model = model;
                var viewCxt = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

    
        public ActionResult GetRetailerOrderListByDate(string startDate, string endDate)
        {
            DateTime stDate = Convert.ToDateTime(startDate);
            DateTime edDate = Convert.ToDateTime(endDate);
            List<VmRetailerOrders> orders = _dealerService.GetRetailerOrderList(stDate, edDate).OrderByDescending(i => i.AddedDate).ToList();
            TempData["orderData"] = orders;
            return RedirectToAction("RetailerOrderList");
        }
    }
}