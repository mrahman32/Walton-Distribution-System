using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    public class RetailerService:IRetailerService
    {
        private readonly Repositories.RepositoryClasses.Unit _unit;
        //private readonly RbsynergyUnit _rbsynergyUnit;
        public RetailerService(Unit unit//, RbsynergyUnit rbsynergyUnit
        )
        {
            _unit = unit;
            //_rbsynergyUnit = rbsynergyUnit;
        }

        public RetailerInfo GetRetailerInfoByPhoneNo(string phoneNumber)
        {
            RetailerInfo retailer = _unit.RetailerInfoRepository.Find(i => i.PhoneNumber == phoneNumber).FirstOrDefault();
            return retailer;
        }

        //public tblDealerInfo GetDealerByCode(string distributorCode)
        //{
        //    tblDealerInfo dealer =  new tblDealerInfo();//_rbsynergyUnit.DealerInfoRepository.Find(i => i.DealerCode == distributorCode).FirstOrDefault();
        //    return dealer;
        //}

        public List<SelectListItem> GetProductTypeListItems()
        {
            List<Product> products = _unit.ProductRepository.GetAll();
            List<SelectListItem> items = products.Select(product => new SelectListItem {Value = product.Id.ToString(), Text = product.ProductType}).ToList();

            return items;
        }

        public List<SelectListItem> GetModelListItemByTypeId(long phoneTypeId)
        {
                Product product = _unit.ProductRepository.Get(phoneTypeId);
                var namearray = product.ProductType.Split(' ');
                string typename = namearray[0];
                List<ProductMaster> productMasters = new List<ProductMaster>();//_rbsynergyUnit.ProductMasterRepository.Find(i => i.Category2 == typename);
                List<SelectListItem> items =
                    productMasters.Select(i => new SelectListItem {Value = i.ProductModel, Text = i.ProductModel}).ToList();

                return items;
        }

        //public List<SelectListItem> GetModelListItemByTypeId(long phoneTypeId)
        //{
        //    Product product = _unit.ProductRepository.Get(phoneTypeId);
        //    var namearray = product.ProductType.Split(' ');
        //    string typename = namearray[0];
        //    List<tblProductMaster> productMasters = new List<tblProductMaster>();//_rbsynergyUnit.ProductMasterRepository.Find(i => i.Category2 == typename);
        //    List<SelectListItem> items =
        //        productMasters.Select(i => new SelectListItem {Value = i.ProductModel, Text = i.ProductModel}).ToList();

        //    return items;
        //}

        public bool SaveRetailerOrder(VmRetailerOrder model)
        {
            var user = HttpContext.Current.Session["user"] as User;
            var order = new RetailerOrder
            {
                RetailerId = model.RetailerId,
                SerialNo = Guid.NewGuid().ToString(),
                DealerCode = model.DealerCode,
                OrderDate = DateTime.Now,
                AddedBy = user.Id,
                AddedDate = DateTime.Now,
                IsCompleted = false, TotalPrice = model.GrandTotal
                
            };
            var a = _unit.RetailerOrderRepository.Add(order);
            _unit.Commit();
            var details = model.RetailerOrderDetails.Select(detail => new RetailerOrderDetail
            {
                Model = detail.Model, 
                OrderQuantity = detail.OrderQuantity, 
                ProductTypeId = detail.ProductTypeId, 
                OrderId = a.Id, 
                AddedDate = DateTime.Now,
                AddedBy = user.Id,
                OrderTotalPrice = detail.OrderTotalPrice, 
                UnitPrice = detail.UnitPrice
            }).ToList();

            _unit.RetailerOrderDetailRepository.AddRange(details);

            _unit.Commit();
            return true;
        }

        public ModelPrice GetModelPrice(string model)
        {
            string trimModel = model.Trim();
            ModelPrice pricing = _unit.ModelPriceRepository.Find(i => i.ModelName == trimModel).FirstOrDefault();
            return pricing;
        }

        public List<Distributor> GetAllDistributorByZone(string zoneName)
        {
            List<Distributor> distributorList =
                _unit.DistributorRepository.Find(i => i.Zone == zoneName).ToList();
            return distributorList;
        }
        public List<ProductMaster> GetAllProductModels()
        {
            List<ProductMaster> modelList =
                  _unit.ProductMasterWdsRepository.GetAll().ToList();
            return modelList;
        }


        public List<stp_retailer_sales_stock_Result> GetRetailerStockAndSaleData(string fromDate, string toDate, string dealerCode, string modelName, long retailerId, User user)
        {
            List<stp_retailer_sales_stock_Result> data =
                 _unit.RetailerInfoRepository.GetRetailerStockAndSaleData(fromDate,toDate,dealerCode,modelName,retailerId,user);
            return data;
        }




        public List<SelectListItem> GetRetailerListbydealer(string dealerCode)
        {
            List<RetailerInfo> reData = _unit.RetailerInfoRepository.Find(x => x.DistributorCode == dealerCode && x.IsActive == true);
            var retailerList = reData.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.RetailerName
            }).ToList();
            return retailerList;
        }

       
    }
}