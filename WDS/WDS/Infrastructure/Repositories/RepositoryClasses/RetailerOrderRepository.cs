using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class RetailerOrderRepository:Repository<RetailerOrder>, IRetailerOrderRepository
    {
        private readonly WDSEntities _context;
        public RetailerOrderRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<VmDealerSalesReport> GetDistributorSalesData(string dealerCode, DateTime startDate, DateTime endDate)
        {
            List<VmDealerSalesReport> modeList = (from retailerOrder in _context.RetailerOrders
                join retailerOrderDetail in _context.RetailerOrderDetails on retailerOrder.Id equals
                    retailerOrderDetail.OrderId

                join retailerInfo in _context.RetailerInfoes on retailerOrder.RetailerId equals retailerInfo.Id
                where retailerOrder.OrderDate>=startDate && retailerOrder.OrderDate<=endDate && retailerOrder.DealerCode == dealerCode
                select new VmDealerSalesReport
                {
                    Model = retailerOrderDetail.Model,
                    OrderDate = (DateTime) retailerOrder.OrderDate,
                    OrderId = retailerOrder.Id,
                    OrderQuantity = (int) retailerOrderDetail.GivenQuantity,
                    RetailerAddress = retailerInfo.RetailerAddress,
                    RetailerPhone = retailerInfo.PhoneNumber,
                    RetailerName = retailerInfo.RetailerName
                }).ToList();
            return modeList;
        }


        
    }
}