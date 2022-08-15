using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IDealerService _dealerService;
        private readonly IDashboardService _dashboardService;
        private readonly ICommonService _commonService;

        public AdminController(IAdminService adminService, IDealerService dealerService, IDashboardService dashboardService, ICommonService commonService)
        {
            _adminService = adminService;
            _dealerService = dealerService;
            _dashboardService = dashboardService;
            _commonService = commonService;
        }


        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadList()
        {
            List<VmDashboardImage> images = _adminService.GetImageList();
            return View(images);
        }

        //[ActionName("RAPL")]
        public ActionResult RetailerApprovalPendingList()
        {
            if (HttpContext.Session == null) return RedirectToAction("Logoff", "Auth");
            var user = HttpContext.Session["user"] as User;

            Role role = _adminService.GetRoleById(user.RoleId);
            List<Distributor> distributors = new List<Distributor>();
            List<Zone> zones = new List<Zone>();
            List<string> zoneNames = new List<string>();
            List<SelectListItem> zoneItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "--All--" } };
            zones = _dealerService.GetAllZones();
            if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
            {
                zoneNames = _dashboardService.GetRoleWiseZone(user.UserName);
                //zones = zones.Where(i => zoneNames.Contains(i.ZoneName)).ToList();
                //distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
            }
            else
            {
                zoneNames = zones.Select(i => i.ZoneName).ToList();
            }
            List<RetailerInfo> retailerInfos = _adminService.GetPendingAllRetailer();
            var finalRetailerList = retailerInfos.Where(i => zoneNames.Contains(i.Zone));

            

            return View(finalRetailerList);
        }

        public JsonResult ApproveRetailer(long id)
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
            bool result = _adminService.ApproveRetailer(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "Retailer Approved.";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }


        public JsonResult RejectRetailer(long id)
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
            bool result = _adminService.RejectRetailer(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "Retailer Approved.";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }

        public ActionResult SrPendingList()
        {
            if (HttpContext.Session == null) return RedirectToAction("Logoff", "Auth");
            var user = HttpContext.Session["user"] as User;
            if (user != null)
            {
                List<SalesRepresentative> retailerInfos = _adminService.GetPendingSrApproval(user);
                return View(retailerInfos);

                //Role role = _adminService.GetRoleById(user.RoleId);
                //if (role.RoleName == "ASM" || role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("tso"))
                //{
                    
                //}

            }

            return View(new List<SalesRepresentative>());
        }

        public JsonResult ApproveSalesRepresentative(long id)
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
            bool result = _adminService.ApproveSr(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "SR Approved.";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }

        public JsonResult RejectSalesRepresentative(long id)
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
            bool result = _adminService.RejectSr(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "SR Rejected.";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }


        public JsonResult ChangeImageStatus(long id, bool status)
        {
            bool isSaved = _adminService.ChangeStatusUploadedImage(id, status);
            var responseModel = new AjaxResponseModel();
            if (isSaved)
            {
                responseModel.Id = 1;
                responseModel.Message = "Image Status Changed";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }

        public ActionResult DeleteImage(long id)
        {
            AjaxResponseModel ajaxResponse = _adminService.DeleteImage(id);
            if (ajaxResponse.Message != null)
            {
                if (ajaxResponse.Id > 0)
                {
                    TempData["message"] = ajaxResponse.Message;
                    TempData["messageType"] = 1;
                }
                else if (ajaxResponse.Id == 0)
                {
                    TempData["message"] = ajaxResponse.Message;
                    TempData["messageType"] = 2;
                }
            }
            else
            {
                TempData["message"] = "Error Occured";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("UploadList");
        }

        public ActionResult IncentiveList()
        {
            var user = HttpContext.Session["user"] as User;
            List<IncentiveDistribution> incentiveDistribution = new List<IncentiveDistribution>();
            if (user != null)
            {
                incentiveDistribution = _adminService.GetIncentiveList();
            }
            return View(incentiveDistribution);
        }


        public ActionResult CreateIncentive()
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");
                Role role = _adminService.GetRoleById(user.RoleId);
                if (!role.RoleName.Contains("Admin")) return RedirectToAction("Logoff", "Auth");

                List<ProductMaster> productMasters = _adminService.GetModels();

                List<SelectListItem> prductItems = new List<SelectListItem>();// { new SelectListItem { Value = "", Text = "<Select>" } };

                prductItems.AddRange(
                    productMasters.Select(i =>
                        new SelectListItem
                        {
                            Text = i.ProductModel,
                            Value = i.ProductModel

                        })
                );

                ViewBag.ModelList = prductItems;



            }
            return View(new IncentiveDistributionModel());
        }
        [HttpPost]
        public ActionResult CreateIncentive(IncentiveDistributionModel model)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");
                Role role = _adminService.GetRoleById(user.RoleId);
                if (!role.RoleName.Contains("Admin")) return RedirectToAction("Logoff", "Auth");
            }

            if (ModelState.IsValid)
            {
                string result = _adminService.SaveIncentive(model);

                if (result == "success")
                {
                    TempData["message"] = "Saved successfully.";
                    TempData["messageType"] = 1;
                }
                else
                {
                    TempData["message"] = result;
                    TempData["messageType"] = 2;
                }

                return RedirectToAction("CreateIncentive");
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditIncentive(long id)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");
                Role role = _adminService.GetRoleById(user.RoleId);
                if (!role.RoleName.Contains("Admin")) return RedirectToAction("Logoff", "Auth");

                List<ProductMaster> productMasters = _adminService.GetModels();

                List<SelectListItem> prductItems = new List<SelectListItem>();// { new SelectListItem { Value = "", Text = "<Select>" } };

                prductItems.AddRange(
                    productMasters.Select(i =>
                        new SelectListItem
                        {
                            Text = i.ProductModel,
                            Value = i.ProductModel

                        })
                );

                ViewBag.ModelList = prductItems;

                IncentiveDistributionModel model = _adminService.GetIncentiveById(id);

                return View(model);
            }
            return View(new IncentiveDistributionModel());
        }
        [HttpPost]
        public ActionResult EditIncentive(IncentiveDistributionModel model)
        {
            var user = HttpContext.Session["user"] as User;
            if (user == null) return RedirectToAction("Logoff", "Auth");
            Role role = _adminService.GetRoleById(user.RoleId);
            if (!role.RoleName.Contains("Admin")) return RedirectToAction("Logoff", "Auth");
            List<ProductMaster> productMasters = _adminService.GetModels();

            List<SelectListItem> prductItems = new List<SelectListItem>();// { new SelectListItem { Value = "", Text = "<Select>" } };

            prductItems.AddRange(
                productMasters.Select(i =>
                    new SelectListItem
                    {
                        Text = i.ProductModel,
                        Value = i.ProductModel

                    })
            );

            ViewBag.ModelList = prductItems;
            if (ModelState.IsValid)
            {
                long result = _adminService.UpdateIncentive(model);

                if (result > 0)
                {
                    TempData["message"] = "Saved successfully.";
                    TempData["messageType"] = 1;
                    return RedirectToAction("EditIncentive", new { id = result });
                }
                else
                {
                    TempData["message"] = result;
                    TempData["messageType"] = 2;
                    return RedirectToAction("EditIncentive", new { id = model.Id });
                }


            }
            return RedirectToAction("EditIncentive", new { id = model.Id });
        }

        public ActionResult CreateTarget()
        {
            List<ProductMaster> productMasters = _adminService.GetModels();

            List<SelectListItem> productItems = new List<SelectListItem>();// { new SelectListItem { Value = "", Text = "<Select>" } };

            productItems.AddRange(
                productMasters.Select(i =>
                    new SelectListItem
                    {
                        Text = i.ProductModel,
                        Value = i.ProductModel

                    })
            );

            ViewBag.ModelList = productItems;


            List<SelectListItem> dealers = new List<SelectListItem> { new SelectListItem { Value = "", Text = "<Select>" } };
            List<Distributor> distributors = _adminService.GetUniqDistributors();
            dealers.AddRange(distributors.Select(i => new SelectListItem { Text = i.DigitechCode + "--" + i.DistributorNameCellCom, Value = i.Id.ToString() }));


            ViewBag.DealerList = dealers;
            ViewBag.TargePersonList = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Select" } };
            return View();
        }
        [HttpPost]
        public ActionResult CreateTarget(TargetModel model)
        {

            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");
                Role role = _adminService.GetRoleById(user.RoleId);
                if (!role.RoleName.Contains("Admin")) return RedirectToAction("Logoff", "Auth");
            }

            if (model.TargetFor == "DEALER")
            {
                if (model.TargetPersonId != null) model.DealerId = (long)model.TargetPersonId;
            }
            if (ModelState.IsValid)
            {
                string result = _adminService.SaveTarget(model);

                if (result == "success")
                {
                    TempData["message"] = "Saved successfully.";
                    TempData["messageType"] = 1;
                }
                else
                {
                    TempData["message"] = result;
                    TempData["messageType"] = 2;
                }

                return RedirectToAction("CreateTarget");
            }
            return View(model);
        }


        public JsonResult GetTargetPersonnel(string targetType, string dealerId)
        {
            long dId = 0;
            long.TryParse(dealerId, out dId);
            List<SelectListItem> targetPersonItem = new List<SelectListItem> { new SelectListItem { Value = "", Text = "<Select>" } };
            Distributor distributor = _dealerService.GetDistributorById(dId);
            switch (targetType)
            {
                case "DEALER":
                    List<Distributor> distributors = _adminService.GetUniqDistributors();
                    targetPersonItem.AddRange(distributors.Select(i => new SelectListItem { Text = i.DigitechCode + "--" + i.DistributorNameCellCom, Value = i.Id.ToString() }));
                    break;
                case "RETAILER":

                    if (distributor != null)
                    {
                        List<RetailerInfo> retailers = _dealerService.GetRetailers().Where(i =>
                                i.DistributorCode == distributor.DigitechCode ||
                                i.DistributorCode2 == distributor.DigitechCode ||
                                i.DistributorCode == distributor.ImportCode ||
                                i.DistributorCode2 == distributor.ImportCode)
                            .ToList();


                        targetPersonItem.AddRange(retailers.Select(i => new SelectListItem { Text = i.RetailerName, Value = i.Id.ToString() }));
                    }

                    break;
                case "SR":
                    if (distributor != null)
                    {
                        List<SalesRepresentative> salesRepresentatives = _dealerService.GetSalesRepresentativeByDealerCode(distributor.DigitechCode,
                                distributor.ImportCode);

                        targetPersonItem.AddRange(salesRepresentatives.Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() }));
                    }
                    break;
            }


            var json = JsonConvert.SerializeObject(targetPersonItem);

            return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Targets()
        {
            List<TargetModel> targetModels = _adminService.GetAllTargets();
            return View(targetModels);
        }


        public ActionResult DistributorSalesReport()
        {
            return View();
        }

        public JsonResult GetDistributorWiseSale(string startDate, string endDate)
        {
            if (HttpContext.Session != null)
            {
                var user = HttpContext.Session["user"] as User;
                if (user != null)
                {
                    DateTime stDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", null);
                    DateTime edDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", null);
                    List<stp_distributor_wise_sale_Result> list = _adminService.GetDealerWiseSalesReport(stDate, edDate);
                    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return null;
        }

        public ActionResult RetailerDeactivationList()
        {
            var user = HttpContext.Session["user"] as User;
            if (user != null)
            {

                //Role role = _adminService.GetRoleById(user.RoleId);

                List<RetailerInfo> retailerInfos = _adminService.GetDeactivationListOfRetailers(user);
                return View(retailerInfos);
            }

            return View(new List<RetailerInfo>());
        }
        public JsonResult ApproveDeactivation(long id)
        {
            var responseModel = new AjaxResponseModel();
            bool result = _dealerService.ApproveRetailerDeactivation(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "Deactivation Successful";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }
        public JsonResult RejectDeactivation(long id)
        {
            var responseModel = new AjaxResponseModel();
            bool result = _dealerService.RejectRetailerDeactivation(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "Rejection Successful";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }


        public ActionResult SRDeactivationList()
        {
            var user = HttpContext.Session["user"] as User;
            if (user != null)
            {
                Role role = _adminService.GetRoleById(user.RoleId);
                if (role.RoleName == "RSM" || role.RoleName == "ASM")
                {
                    List<SalesRepresentative> salesRepresentatives = _adminService.GetPendingDeactivationListOfSr(user);
                    return View(salesRepresentatives);
                }
            }

            return View(new List<SalesRepresentative>());
        }
        public JsonResult ApproveDeActivationSr(long id)
        {
            var responseModel = new AjaxResponseModel();
            bool result = _dealerService.ApproveSrDeactivation(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "Deactivation Successful";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }
        public JsonResult RejectDeActivationSr(long id)
        {
            var responseModel = new AjaxResponseModel();
            bool result = _dealerService.RejectSrDeactivation(id);
            if (result)
            {
                responseModel.Id = 1;
                responseModel.Message = "Rejection Successful";
            }
            else
            {
                responseModel.Id = 0;
                responseModel.Message = "Something went wrong. Please contact with administrator.";
            }
            return new JsonResult { Data = responseModel };
        }

        public ActionResult DistributorAchievement(string targetType)
        {
            return View();
        }

        public JsonResult GetDistributionAchievements(string targetType)
        {
            if (!string.IsNullOrWhiteSpace(targetType))
            {
                List<stp_dealer_target_achievement_report_Result> dealerTargetAchievementReportResults =
                    _adminService.GetDealerAchievementReport(targetType);
                return new JsonResult { Data = dealerTargetAchievementReportResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = new List<stp_dealer_target_achievement_report_Result>(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult RetailerIncentive()
        {
            List<SelectListItem> distributorItems = _commonService.GetDistributorTypeSelectListItems();
            ViewBag.DistributorTypes = distributorItems;
            return View();
        }

        public ActionResult GetRetailerIncentive(string fromDate, string toDate, long dType)
        {
            if (HttpContext.Session != null)
            {
                var ListData = _adminService.GetRetailerIncentiveDataByDate(fromDate, toDate, dType);

                return new JsonResult()
                {
                    Data = ListData,
                    MaxJsonLength = 86753090,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                //return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Logoff", "Auth");
        }
        public ActionResult SRIncentive()
        {
            List<SelectListItem> distributorItems = _commonService.GetDistributorTypeSelectListItems();
            ViewBag.DistributorTypes = distributorItems;
            return View();
        }

        public ActionResult GetSRIncentive(string fromDate, string toDate, long dType)
        {
            if (HttpContext.Session != null)
            {
                var ListData = _adminService.GetSRIncentiveDataByDate(fromDate, toDate, dType);

                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Logoff", "Auth");
        }


        public ActionResult SRIncentiveAdminReport()
        {

            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");

                Role role = _adminService.GetRoleById(user.RoleId);
                List<ProductMaster> productMaster = _dealerService.GetAllProductModels();
                List<SelectListItem> modelList = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "", Text = "--ALL--"
                }
                };

                modelList.AddRange(productMaster.Select(m =>
                    new SelectListItem
                    {
                        Value = m.ProductModel,
                        Text = m.ProductModel
                    })
                );
                ViewBag.modelList = modelList;
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
                else
                {
                    distributors = _dealerService.GetAllDistributorByZone(user.UserName);
                }
                var distributorListItems = new List<SelectListItem>{new SelectListItem{Value = "0", Text = "--ALL--"}};
                distributorListItems.AddRange(distributors.Select(i=> new SelectListItem{ Value = i.DigitechCode, Text = i.DistributorNameCellCom + "("+i.DigitechCode + ")"}));
                var salesRepresentatives = new List<SalesRepresentative>();
                foreach (var de in distributors)
                {
                    var srdatabyDealer = _dealerService.GetSalesRepresentativeByDealerCode(de.DigitechCode, de.ImportCode);
                    salesRepresentatives.AddRange(srdatabyDealer);
                }
                var srList = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Value = "0", Text = "--ALL--"
                    }
                };
                srList.AddRange(
                    salesRepresentatives.Select(representative =>
                        new SelectListItem
                        {
                            Value = representative.Id.ToString(CultureInfo.InvariantCulture),
                            Text = representative.Name
                        })
                    );

                ViewBag.SalesRepresentativeSelectList = srList;
                ViewBag.DistributorSelectList = distributorListItems;
                return View();
            }

            return RedirectToAction("Logoff", "Auth");
        }

        [HttpPost]
        public ActionResult GetSRIncentiveAdminData(string fromDate, string toDate, string dCode, long srId, string model)
        {
            if (HttpContext.Session != null)
            {
                var ListData = _adminService.GetSRIncentiveAdminDataByDate(fromDate, toDate, dCode, srId, model);

                return new JsonResult()
                {
                    Data = ListData,
                    MaxJsonLength = 86753090,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return RedirectToAction("Logoff", "Auth");
        }

        public ActionResult ExtendLimit()
        {
            List<Distributor> distributors = _dealerService.GetAllDistributors();
            List<SelectListItem> distributorItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "--Select--" } };
            distributorItems.AddRange(distributors.Select(distributor => new SelectListItem { Value = distributor.DigitechCode, Text = distributor.DigitechCode + " -- " + distributor.DistributorNameCellCom + " -- " + distributor.Zone + " -- " + distributor.Address }));

            ViewBag.Distributors = distributorItems;

            return View();
        }
        [HttpPost]
        public ActionResult ExtendLimit(ExtendedLimitModel model)
        {
            if (ModelState.IsValid)
            {
                User user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");
                model.AddedBy = user.Id;
                model.AddedDate = DateTime.Now;
                DateTime firstDate = new DateTime(Convert.ToDateTime(model.AddedDate).Year, Convert.ToDateTime(model.AddedDate).Month, 1);
                DateTime lastDate = firstDate.AddMonths(1).AddSeconds(-1);
                model.EndDate = lastDate;

                string result = _adminService.SaveLimitExtension(model);
                if (result == "success")
                {
                    TempData["message"] = "Saved successfully.";
                    TempData["messageType"] = 1;
                }
                else
                {
                    TempData["message"] = result;
                    TempData["messageType"] = 2;
                }
            }

            return RedirectToAction("ExtendLimit");
        }

        public ActionResult GetRetailerByDealer(string distributorId)
        {
            List<RetailerInfo> retailerInfos = _dealerService.GetRetailersByDealerId(new Distributor { DigitechCode = distributorId });
            List<SelectListItem> retailerItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "--Select--" } };
            retailerItems.AddRange(retailerInfos.Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.RetailerName + "--" + i.RetailerAddress }));
            return new JsonResult { Data = retailerItems, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult RetailerUpdateRequests()
        {
            User user = HttpContext.Session["user"] as User;
            var model = _adminService.GetRetailerUpdateModel(user);
            return View(model);
        }

        public ActionResult RecommendRetailerUpdate(long rid)
        {
            User user = HttpContext.Session["user"] as User;
            if (user == null) return new JsonResult { Data = "Your session has been expired. Please Logout, and Login again", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            string result = _adminService.RecommendRetailerUpdate(rid, user);
            if (result == "success")
            {
                TempData["message"] = "Saved successfully.";
                TempData["messageType"] = 1;
            }
            else
            {
                TempData["message"] = result;
                TempData["messageType"] = 2;
            }

            return RedirectToAction("RetailerUpdateRequests");
        }

        public ActionResult CreateDistributors()
        {
            if (HttpContext.Session == null) return RedirectToAction("Logoff", "Auth");
            List<SelectListItem> division = _dealerService.GetDivisionSelectList();
            List<SelectListItem> district = _dealerService.GetDistrictSelectList();
            List<SelectListItem> thana = _dealerService.GetThanaSelectList();
            List<SelectListItem> brand = _dealerService.GetProductBrandSelectList();
            List<SelectListItem> zone = _dealerService.GetZoneSelectList();
            List<SelectListItem> distributorType = _dealerService.GetDistributorTypeSelectList();
            ViewBag.divisionList = division;
            ViewBag.brandList = brand;
            ViewBag.districtList = district;
            ViewBag.thanaList = thana;
            ViewBag.zoneList = zone;
            ViewBag.distributorTypeList = distributorType;
            return View();
        }


        [HttpPost]
        public ActionResult CreateDistributors(vmDistributors model)
        {
            var user = HttpContext.Session["user"] as User;
            if (HttpContext.Session != null)
            {

                if (user == null) return RedirectToAction("Logoff", "Auth");
                Role role = _adminService.GetRoleById(user.RoleId);
                if (!role.RoleName.Contains("Admin")) return RedirectToAction("Logoff", "Auth");
            }

            if (ModelState.IsValid)
            {

                var zone = _dealerService.GetAZoneById(model.ZoneId);
                if (zone != null)
                {
                    model.Zone = zone.ZoneName;
                }

                model.AddedBy = user.Id;
                model.AddedDate = DateTime.Now;
                string result = _adminService.SaveDistributor(model);

                if (result == "success")
                {
                    TempData["message"] = "Saved successfully.";
                    TempData["messageType"] = 1;
                }
                else
                {
                    TempData["message"] = result;
                    TempData["messageType"] = 2;
                }

                return RedirectToAction("CreateDistributors");
            }
            return View();
        }

        public ActionResult DistributorList()
        {
            if (HttpContext.Session == null) return RedirectToAction("Logoff", "Auth");
            List<Distributor> distributor = _dealerService.GetAllDistributors();
            ViewBag.distributorList = distributor;
            return View();
        }
        public ActionResult AvailabiltyBrodcast()
        {
            if (HttpContext.Session == null) return RedirectToAction("Logoff", "Auth");
            List<ProductMaster> productMaster = _dealerService.GetAllProductModels();
            var availabiltyBrodcastList = _adminService.GetAllBrodcastList();
            List<SelectListItem> modelList = new List<SelectListItem>(){
                new SelectListItem
                {
                    Value = "", Text = "--ALL--"
                }
                };

            modelList.AddRange(productMaster.Select(m =>
                new SelectListItem
                {
                    Value = m.ProductModel,
                    Text = m.ProductModel
                })
            );
            ViewBag.modelList = modelList;
            ViewBag.avaBroadcastList = availabiltyBrodcastList;
            return View();
        }

        public ActionResult AvailabiltyBrodcastSave(VmAvailabilityBroadcast objModel)
        {
            var data = _adminService.AvailabilityBroadcastSave(objModel);
            var messageAndReload = new
            {
                m = data,
                isRedirect = true,
                redirectUrl = Url.Action("AvailabiltyBrodcast")
            };
            return Json(messageAndReload, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSrByDealerCode(string dCode)
        {
            var salesRepresentatives = new List<SalesRepresentative>();
            var srdatabyDealer = _dealerService.GetSalesRepresentativeByDealerCode(dCode, dCode);
            salesRepresentatives.AddRange(srdatabyDealer);
            var srList = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Value = "0", Text = "--ALL--"
                    }
                };
            srList.AddRange(
                salesRepresentatives.Select(representative =>
                    new SelectListItem
                    {
                        Value = representative.Id.ToString(CultureInfo.InvariantCulture),
                        Text = representative.Name
                    })
                );



            return new JsonResult{Data =  srList, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            
        }

    }
}