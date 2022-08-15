using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class RetailerDistributionRepository:Repository<RetailerDistribution>, IRetailerDistributionRepository
    {
        private readonly WDSEntities _context;
        public RetailerDistributionRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<stp_retailer_stock_Result> GetRetailerStock(long retailerId, string digitechCode, string importCode)
        {
            var param = new SqlParameter
            {
                ParameterName = "@retailer_id",
                Value = retailerId
            };
            var dCode = new SqlParameter
            {
                ParameterName = "@digitech_code",
                Value = digitechCode
            };
            var iCode = new SqlParameter
            {
                ParameterName = "@import_code",
                Value = importCode??"0"
            };
            List<stp_retailer_stock_Result> retailerStockResults = _context.Database.SqlQuery<stp_retailer_stock_Result>("EXEC stp_retailer_stock @retailer_id, @digitech_code, @import_code", param, dCode, iCode).ToList();

            return retailerStockResults;
        }

        public List<stp_sr_sales_report_Result> GetSrSalesReport(long srId, DateTime? startDate, DateTime? endDate, Distributor distributor)
        {
            var srIdParam = new SqlParameter {ParameterName = "@sr_id", Value = srId};
            var stDateParam = new SqlParameter { ParameterName = "@start_date", Value = startDate };
            var edDateParam = new SqlParameter { ParameterName = "@end_date", Value = endDate };
            var digitechCode = new SqlParameter { ParameterName = "@digitech_code", Value = distributor.DigitechCode };
            var importCode = new SqlParameter { ParameterName = "@import_code", Value = distributor.ImportCode??"0" };

            List<stp_sr_sales_report_Result> srSalesReportResults = _context.Database
                .SqlQuery<stp_sr_sales_report_Result>("EXEC [stp_sr_sales_report] @start_date, @end_date, @sr_id, @digitech_code, @import_code",
                    stDateParam, edDateParam, srIdParam, digitechCode, importCode).ToList();

            return srSalesReportResults;
        }

        public int GetTotalPhoneTypeCount(string phoneType, DateTime startDate, DateTime endDate, long retailerId)
        {
            var cnt = (from retailerDistribution in _context.RetailerDistributions
                join productMaster in _context.ProductMasters on retailerDistribution.Model equals productMaster
                    .ProductModel
                where productMaster.ProductyType == phoneType && retailerDistribution.DistributionDate >= startDate &&
                      retailerDistribution.DistributionDate <= endDate && retailerDistribution.RetailerId == retailerId
                select new { retailerDistribution.Id}).ToList().Count;
            return cnt;
        }
    }
}