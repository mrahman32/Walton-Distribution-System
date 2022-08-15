using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly IDealerService _dealerService;
        private readonly IDashboardService _dashboardService;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public DocumentController(IAdminService adminService, IDealerService dealerService, IDashboardService dashboardService, ICommonService commonService, IDocumentService documentService, IAuthService authService)
        {
            _adminService = adminService;
            _dealerService = dealerService;
            _dashboardService = dashboardService;
            _commonService = commonService;
            _documentService = documentService;
            _authService = authService;
        }
        // GET: Document
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateDistributorCheckList(string dealerCode = null)
        {
            var user = HttpContext.Session["user"] as User;
            long roleId = Convert.ToInt64(user.RoleId);
            Role role = _adminService.GetRoleById(roleId);
            var zones = new List<Zone>();
            var zoneNames = new List<string>();
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
            var distributors = _dealerService.GetDistributorSelectListItems();
            var distriListItems = new List<SelectListItem>();
            foreach (var item in distributors)
            {
                distriListItems.AddRange(from zoneName in zoneNames where item.Text.Contains(zoneName) select new SelectListItem {Value = item.Value, Text = item.Text});
            }

            ViewBag.Distributors = distriListItems;



            List<Bank> banks = _commonService.GetBanks();

            var bankItems = new List<SelectListItem>();
            bankItems.AddRange(banks.Select(i => new SelectListItem { Value = i.Id.ToString(CultureInfo.InvariantCulture), Text = i.BankName }));
            ViewBag.Banks = bankItems;


            List<BankBranch> bankBranches = _commonService.GetBankBranches();
            var branchItems = new List<SelectListItem>();
            branchItems.AddRange(bankBranches.Select(i => new SelectListItem { Value = i.Id.ToString(CultureInfo.InvariantCulture), Text = i.BranchName }));
            ViewBag.Branches = branchItems;








            var model = new DistributorCheckListModel();

            if (!string.IsNullOrWhiteSpace(dealerCode))
            {
                model = _documentService.GetDistributorCheckListByDistributorCode(dealerCode) ??
                        new DistributorCheckListModel{DealerCode = dealerCode, IsCreationApproved = false};
            }
            if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management"))
            {
                model.IsApprover = true;
            }
            else
            {
                model.IsApprover = false;
            }
            return View(model);
        }

        //[HttpPost]
        //public ActionResult CreateDistributorCheckList(DistributorCheckListModel model)
        //{
        //    var a = model;
        //    return null;
        //}

        [HttpPost]
        public ActionResult CreateMOA(DistributorCheckListModel model)
        {

            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateNewMOA(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.MemorandumOfAgreementModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateMoa(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateNewMOA(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] =  2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        [HttpPost]
        public ActionResult CreateMOC(DistributorCheckListModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateNewMOC(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.MemorandumOfChequeModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateMoc(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateNewMOC(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        [HttpPost]
        public ActionResult CreateBS(DistributorCheckListModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateNewBS(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.BankSolvencyModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateBS(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateNewBS(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        [HttpPost]
        public ActionResult CreateTIN(DistributorCheckListModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateNewTIN(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.TaxIdentityModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateTIN(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateNewTIN(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        [HttpPost]
        public ActionResult CreateVAT(DistributorCheckListModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateNewVAT(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.ValueAddedTaxModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateVAT(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateNewVAT(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        [HttpPost]
        public ActionResult CreateTradeL(DistributorCheckListModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateNewTrade(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.TradeLicensModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateTrade(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateNewTrade(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        [HttpPost]
        public ActionResult CreateDistOtherInfo(DistributorCheckListModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateNewOther(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.DistributorOthersInformationModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateOther(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateNewOther(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        public ActionResult CheckListApprovalPending()
        {
            List<CheckListApprovalPendingModel> models = _documentService.GetApprovalPendingCheckList();
            foreach (var model in models)
            {
                List<VarificationEntityModel> lst = _documentService.GetDocumentLst(model.CheckListId);
                if ((lst.Any(i => i.IsApproved == "Approve") && lst.Any(i => i.IsApproved == "Decline"))
                || (lst.Any(i => i.IsApproved == "Approve") && lst.Any(i => i.IsApproved == null))
                || (lst.Any(i => i.IsApproved == null) && lst.Any(i => i.IsApproved == "Decline")))
                { model.InvestigationStatus = "Partial"; }

                else if (lst.Any(i => i.IsApproved == "Approve") 
                    && (lst.All(i => i.IsApproved != "Decline") || lst.All(i => i.IsApproved != null))) 
                { model.InvestigationStatus = "Approved"; }


                else if ((lst.Any(i => i.IsApproved != "Approve") || lst.Any(i => i.IsApproved != null)) &&
                         lst.All(i => i.IsApproved == "Decline"))
                {
                    model.InvestigationStatus = "Declined";
                }
                else
                {
                    model.InvestigationStatus = "No action taken";
                }
            }
            return View(models);
        }


        public JsonResult ApproveDoc(long id)
        {
            AjaxResponseModel result =  _documentService.ApproveDocuments(id);
            //TempData["message"] = result.Message;
            //TempData["messageType"] = result.Id;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public PartialViewResult GetStampPartial()
        {
            var model = new MOA_StapmsModel();
            //model.MoaStapmsModels.Add(new MOA_StapmsModel());
            return PartialView("_moa_stamp", model);
        }

        public PartialViewResult GetChequePartial()
        {
            var model = new MOC_ChequesModel();
            return PartialView("_moc_cheque", model);
        }

        public ActionResult DistributorPersonalInfo()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreatePersonalInfo(DistributorCheckListModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DealerCode))
            {
                if (model.Id == 0)
                {
                    //create new check list
                    DistributorCheckList checkList = _documentService.CreateNewCheckList(model);
                    if (checkList != null)
                    {
                        string result = _documentService.CreateDistributorPersonalInfo(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }

                }
                else
                {
                    DistributorCheckList checkList = _documentService.UpdateDistributorCheckList(model);
                    if (model.DistributorPersonalInformationModel.Id > 0)
                    {
                        //update
                        string result = _documentService.UpdateDistributorPersonalInfo(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;
                    }
                    else
                    {
                        //create
                        string result = _documentService.CreateDistributorPersonalInfo(model, checkList);
                        TempData["message"] = result;
                        TempData["messageType"] = result.ToLower().Contains("success") ? 1 : 2;

                    }
                }
            }
            else
            {
                TempData["message"] = "Please select a distributor. Page refreshed so data has to be input again";
                TempData["messageType"] = 2;
            }

            return RedirectToAction("CreateDistributorCheckList", new { dealerCode = model.DealerCode });
        }

        public ActionResult VerificatonPending()
        {
            List<CheckListApprovalPendingModel> models = _documentService.GetVerificationPendingList();
            

            return View(models);
        }

        public ActionResult BillFilesDetails(long checkListId)
        {
            //List<VmDistributorDocVerificationList> lst = _documentService.GetDocList(checkListId);
            List<VarificationEntityModel> lst = _documentService.GetDocumentLst(checkListId);
            return View(lst);
        }


        public JsonResult SaveInvestigationOpinion(long id, long checkListId, string description, string remarks, string decision)
        {

            string result = _documentService.SaveInvestigationResult(id, checkListId, description, remarks, decision);

            return new JsonResult{Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
    }
}