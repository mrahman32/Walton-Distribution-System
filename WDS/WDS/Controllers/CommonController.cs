using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;

namespace WDS.Controllers
{
    public class CommonController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IAuthService _authService;
        private readonly IDealerService _dealerService;
        private readonly IDashboardService _dashboardService;

        public CommonController(ICommonService commonService, IAuthService authService, IDealerService dealerService, IDashboardService dashboardService)
        {
            _commonService = commonService;
            _authService = authService;
            _dealerService = dealerService;
            _dashboardService = dashboardService;
        }
        public ActionResult ImeiSearch()
        {

            return View();
        }

        [HttpPost]
        public JsonResult GetImeiDetail(string imei)
        {
            List<stp_check_imei_status_Result> checkImeiStatusResults = _commonService.GetImeiDetail(imei);

            var user = HttpContext.Session["user"] as User;
            var role = _authService.GetRoleById(Convert.ToInt64(user.RoleId));
            Distributor distributor = _dealerService.GetDistributorByPhone(user.MobileNumber);
            if (checkImeiStatusResults.Any() && role.RoleName.Contains("Dealer"))
            {
                if (checkImeiStatusResults[0].DealerCode == distributor.DigitechCode)
                {
                    return new JsonResult { Data = checkImeiStatusResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                return new JsonResult { Data = "err-IMEI not found in WDS stock.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };


            }
            List<Distributor> distributors = new List<Distributor>();

            distributors = _dealerService.GetAllDistributors();
            if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm")||role.RoleName.ToLower().Contains("tso"))
            {
                List<string> zoneNames = _dashboardService.GetRoleWiseZone(user.UserName);
                distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
            }
            else if(role.RoleName.ToLower().Contains("tso"))
            {
                distributors = _dealerService.GetAllDistributorByZone(user.UserName);
            }
            if (!distributors.Any() && role.RoleName.ToLower().Contains("tso"))
            {
                distributors = _dealerService.GetAllDistributors();
                List<string> zoneNames = _dashboardService.GetRoleWiseZone(user.UserName);
                distributors = distributors.Where(i => zoneNames.Contains(i.Zone)).ToList();
            }



            var distributorCode = checkImeiStatusResults.Select(i => i.DealerCode).FirstOrDefault();
            if(distributors.Any(i=>i.DigitechCode == distributorCode))
                return new JsonResult { Data = checkImeiStatusResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return new JsonResult { Data = "err-IMEI not found in stock.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            //if (checkImeiStatusResults.Any())
            //{
            //    return new JsonResult { Data = checkImeiStatusResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //}
            //return new JsonResult { Data = "err-IMEI not found in WDS stock.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}