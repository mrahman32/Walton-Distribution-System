using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.ViewModels;
using WebGrease.Css.Ast;
using ClosedXML.Excel;
using System.IO;
using WDS.Models;

namespace WDS.Controllers
{
    public class RetailerController : Controller
    {
        private readonly IRetailerService _retailerService;
        private readonly IAuthService _authService;
        private readonly IDealerService _dealerService;
        private readonly IAdminService _adminService;
        private readonly IDashboardService _dashboardService;

        public RetailerController(IRetailerService retailerService, IAuthService authService, IDealerService dealerService, IAdminService adminService, IDashboardService dashboardService)
        {
            _retailerService = retailerService;
            _authService = authService;
            _dealerService = dealerService;
            _adminService = adminService;
            _dashboardService = dashboardService;
        }

        // GET: Retailer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RetailerOrder()
        {
            
            var order = new VmRetailerOrder();
            User user = null;
            if (HttpContext.Request.RequestContext.HttpContext.Session != null)
            {
                user = HttpContext.Request.RequestContext.HttpContext.Session["user"] as User;
                if (user != null)
                {
                    if (user.RoleId != null)
                    {
                        Role role = _authService.GetRoleById((long) user.RoleId);
                        if (role.RoleName.ToLower().Equals("retailer"))
                        {
                            RetailerInfo retailerInfo = _retailerService.GetRetailerInfoByPhoneNo(user.UserName);
                            order.RetailerId = retailerInfo.Id;
                            order.RetailerName = retailerInfo.RetailerName;

                            //tblDealerInfo dealer = _retailerService.GetDealerByCode(retailerInfo.DistributorCode);
                            //order.DealerCode = dealer.DealerCode;
                            //order.DealerName = dealer.DealerName;

                        }
                        else
                        {
                            return RedirectToAction("Login", "Auth", new { model = user });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Logoff", "Auth");
                    }
                }
                else
                {
                    return RedirectToAction("Logoff", "Auth");
                }
            }
            else
            {
                return RedirectToAction("Logoff", "Auth");
            }
            List<SelectListItem> selectListItems = _retailerService.GetProductTypeListItems();
            ViewBag.ProductTypes = selectListItems;
            return View(order);
        }

        [HttpPost]
        public ActionResult RetailerOrder(VmRetailerOrder model)
        {
            if (model.RetailerOrderDetails.Any())
            {
                bool result = _retailerService.SaveRetailerOrder(model);
            }

            List<SelectListItem> selectListItems = _retailerService.GetProductTypeListItems();
            ViewBag.ProductTypes = selectListItems;
            return View(model);
        }
        public JsonResult GetModel(long phoneTypeId)
        {
            List<SelectListItem> modelItems = _retailerService.GetModelListItemByTypeId(phoneTypeId);
            return new JsonResult { Data = modelItems, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult AddRow(long typeId, string model, int quantity)
        {
            var detail = new RetailerOrderDetail
            {
                ProductTypeId = typeId,
                Model = model, OrderQuantity = quantity
            };

            ModelPrice pricing = _retailerService.GetModelPrice(model);
            if (pricing != null)
            {
                detail.UnitPrice = pricing.Price;
                detail.OrderTotalPrice = quantity*detail.UnitPrice;
            }
            else
            {
                detail.UnitPrice = (decimal?) 0.0;
                detail.OrderTotalPrice = (decimal)0.0;
            }
            
            return PartialView("~/Views/Shared/_RetailerOrderDetail.cshtml", detail);
        }

        public ActionResult RetailerStockData()
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;
                if (user == null) return RedirectToAction("Logoff", "Auth");


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
                else
                {
                    distributors = _dealerService.GetAllDistributorByZone(user.UserName);
                }
                //List<Distributor> distributor = _retailerService.GetAllDistributorByZone(user.UserName);
                List<ProductMaster> productMaster = _retailerService.GetAllProductModels();
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
                ViewBag.retailerList = modelList;

                return View(new RetailerStockReportSearchBoxModel());
            }

            return RedirectToAction("Logoff", "Auth");
        }


        [HttpPost]
        public ActionResult RetailerStockData(RetailerStockReportSearchBoxModel model)//string fromDate, string toDate, string dealerCode, string modelName, long retailerId)
        {
            if (HttpContext.Session != null)
            {
                User user = HttpContext.Session["user"] as User;
                List<stp_retailer_sales_stock_Result> retailerSaleStockData = _retailerService.GetRetailerStockAndSaleData(model.StartDate.ToString(), model.EndDate.ToString(), model.DealerCode, model.ModelName, model.RetailerId, user);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string timeString = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                string fileName = "RetailerStockData_"+ timeString +".xlsx";// + "_" + fromDate + "_" + toDate + ".xlsx";

                try
                {
                    using (var workBook = new XLWorkbook())
                    {
                        IXLWorksheet worksheet = workBook.Worksheets.Add("RetailerStockData");
                        worksheet.Columns("A", "Z").AdjustToContents();
                        worksheet.Cell("A1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("A1").Value = "Dealer Name";

                        worksheet.Cell("B1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("B1").Value = "Dealer Code";

                        worksheet.Cell("C1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("C1").Value = "Retailer Name";

                        worksheet.Cell("D1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("D1").Value = "Retailer Address";

                        worksheet.Cell("E1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("E1").Value = "Phone No.";

                        worksheet.Cell("F1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("F1").Value = "Model";

                        worksheet.Cell("G1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("G1").Value = "Unit Price";

                        worksheet.Cell("H1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("H1").Value = "Stock Qty";

                        worksheet.Cell("I1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("I1").Value = "Stock Value";

                        worksheet.Cell("J1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("J1").Value = "Lifting Qty";

                        worksheet.Cell("K1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("K1").Value = "Lifting Value";

                        worksheet.Cell("L1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("L1").Value = "Sale Qty";

                        worksheet.Cell("M1").Style.Font.SetBold().Font.FontSize = 10;
                        worksheet.Cell("M1").Value = "Sales Value";
                        var cnt = retailerSaleStockData.Count;
                        for (int index = 1; index <= cnt; index++)
                        {
                            worksheet.Cell(index + 1, 1).Value = retailerSaleStockData[index - 1].Dealername;
                            worksheet.Cell(index + 1, 2).Value = retailerSaleStockData[index - 1].DigitechCode;
                            worksheet.Cell(index + 1, 3).Value = retailerSaleStockData[index - 1].RetailerName;
                            worksheet.Cell(index + 1, 4).Value = retailerSaleStockData[index - 1].RetailerAddress;
                            worksheet.Cell(index + 1, 5).Value = retailerSaleStockData[index - 1].PhoneNumber;
                            worksheet.Cell(index + 1, 6).Value = retailerSaleStockData[index - 1].Model;
                            worksheet.Cell(index + 1, 7).Value = retailerSaleStockData[index - 1].UnitPrice;
                            worksheet.Cell(index + 1, 8).Value = retailerSaleStockData[index - 1].StockQty;
                            worksheet.Cell(index + 1, 9).Value = retailerSaleStockData[index - 1].StockValue;
                            worksheet.Cell(index + 1, 10).Value = retailerSaleStockData[index - 1].LiftingQty;
                            worksheet.Cell(index + 1, 11).Value = retailerSaleStockData[index - 1].LiftingValue;
                            worksheet.Cell(index + 1, 12).Value = retailerSaleStockData[index - 1].SaleQty;
                            worksheet.Cell(index + 1, 13).Value = retailerSaleStockData[index - 1].SaleValue;
                        }

                        var stream = new MemoryStream();
                        workBook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
                catch (Exception ex) {
                    return null;
                }


            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetRetailerListbydealer(string dealerCode)
        {
            List<SelectListItem> retailer = new List<SelectListItem>();

            retailer = _retailerService.GetRetailerListbydealer(dealerCode);

            return Json(retailer, JsonRequestBehavior.AllowGet);
        }
    }
}