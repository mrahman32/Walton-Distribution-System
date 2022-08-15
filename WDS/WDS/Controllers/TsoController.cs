using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;

namespace WDS.Controllers
{
    public class TsoController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IDealerService _dealerService;
        private readonly ICommonService _commonService;
        private readonly IDashboardService _dashboardService;

        public TsoController(IDealerService dealerService, ICommonService commonService, IAdminService adminService, IDashboardService dashboardService)
        {
            _dealerService = dealerService;
            _commonService = commonService;
            _adminService = adminService;
            _dashboardService = dashboardService;
        }
        // GET: Tso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogisticOrder()
        {
            var user = HttpContext.Session["user"] as User;
            Role role = _adminService.GetRoleById(user.RoleId);
            //List<Distributor> distributors = _dealerService.GetAllDistributorByZone(user.UserName);


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

            List<SelectListItem> distributorItems = new List<SelectListItem>{new SelectListItem{Value = "", Text = "--Select--"}};
            distributorItems.AddRange(distributors.Select(distributor => 
                new SelectListItem
                {
                    Value = distributor.DigitechCode, 
                    Text = distributor.DistributorNameCellCom + "--" + distributor.Zone + "--" + distributor.Address
                }));

            ViewBag.Distributors = distributorItems;


            List<ModelPrice> models = _commonService.GetModelList();
            List<SelectListItem> modelItems = new List<SelectListItem>{new SelectListItem{Value = "", Text = "--Select--"}};
            modelItems.AddRange(models.Select(modelItem =>
                new SelectListItem
                {
                    Value = modelItem.ModelName,
                    Text = modelItem.ModelName
                }));

            ViewBag.Models = modelItems;
            return View();
        }

        public JsonResult GetModelColor(string model)
        {
            ModelPrice modelprice = _commonService.GetModelWisePrice(model);
            List<ModelColor> colors = _commonService.GetModelWiseColor(model);
            List<SelectListItem>colorItems = new List<SelectListItem>{new SelectListItem{Value = "", Text = "--Select--"}};
            colorItems.AddRange(colors.Select(color => new SelectListItem {Value = color.Color, Text = color.Color}));

            var returnObj = new
            {
                colorItems = colorItems,
                modelprice = modelprice,
            };

            return new JsonResult()
            {
                Data = returnObj,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

          // return new JsonResult{Data = colorItems, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }


        [HttpPost]
        public JsonResult SaveDate(DistributorOrder objDistributorOrder, List<DistributorOrderDetail> orderDetailsList)
        {
            var user = HttpContext.Session["user"] as User;
            var res = _dealerService.saveDistributorOrderData(objDistributorOrder, orderDetailsList, user);

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetAllLogisticOrder()
        {
            var user = HttpContext.Session["user"] as User;
            if (user == null) return RedirectToAction("Logoff", "Auth");
            Role role = _dealerService.GetRoleById(user.RoleId);
            var listData = _dealerService.GetAllDistributorOrderList(role);
            ViewBag.orderList = listData;
            ViewBag.roleInfo = role;
            return View();
        }

        [HttpPost]
        public JsonResult GetDistributorOrderDetailsByOrderId(string orderId)
        {
            var user = HttpContext.Session["user"] as User;
            long ditributorOrderId = Convert.ToInt64(orderId);
            var orderDetailsList = _dealerService.GetAllDistributorOrderDetailsListByOrderId(ditributorOrderId);
            List<ModelPrice> models = _commonService.GetModelList();
            List<SelectListItem> modelItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "--Select--" } };
            modelItems.AddRange(models.Select(modelItem =>
                new SelectListItem
                {
                    Value = modelItem.ModelName,
                    Text = modelItem.ModelName
                }));

            var returnObj = new
            {
                orderDetailsList = orderDetailsList,
                modelItems = modelItems
            };

            return new JsonResult()
            {
                Data = returnObj,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonResult UpdateOrderDetails(DistributorOrder objDistributorOrder, List<DistributorOrderDetail> orderDetailsList)
        {
            var user = HttpContext.Session["user"] as User;
            var res = _dealerService.updateDistributorOrderData(objDistributorOrder, orderDetailsList, user);

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}