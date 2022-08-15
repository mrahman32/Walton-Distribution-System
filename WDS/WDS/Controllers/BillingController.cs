using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;

namespace WDS.Controllers
{
    public class BillingController : Controller
    {
        
        private readonly IAdminService _adminService;
        private readonly IDealerService _dealerService;
        private readonly IDashboardService _dashboardService;
        private readonly ICommonService _commonService;

        public BillingController(IAdminService adminService, IDealerService dealerService, IDashboardService dashboardService, ICommonService commonService)
        {
            _adminService = adminService;
            _dealerService = dealerService;
            _dashboardService = dashboardService;
            _commonService = commonService;
        }
        // GET: Billing
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AllRetailerList()
        {
            List<RetailerInfo> retailerInfos = _dealerService.GetRetailers().OrderBy(i=>i.DistributorCode).ToList();
            return View(retailerInfos);
        }

        public async Task<ActionResult> AllSrList()
        {
            List<SalesRepresentative> salesRepresentatives = _dealerService.GetAllSalesRepresentative();
            return View(salesRepresentatives);
        }
    }
}