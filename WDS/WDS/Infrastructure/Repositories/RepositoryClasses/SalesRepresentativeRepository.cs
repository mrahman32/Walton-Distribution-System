using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class SalesRepresentativeRepository:Repository<SalesRepresentative>, ISalesRepresentativeRepository
    {
        private readonly WDSEntities _context;
        public SalesRepresentativeRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<stp_sr_report_Result> GetSrReport(string startDate, string endDate, long srId)
        {
            var stDate = new SqlParameter
            {
                ParameterName = "@start_date",
                Value = startDate
            };

            var edDate = new SqlParameter
            {
                ParameterName = "@end_date",
                Value = endDate
            };

            var sId = new SqlParameter
            {
                ParameterName = "@sr_id",
                Value = srId
            };


            List<stp_sr_report_Result> srReport = _context.Database.SqlQuery<stp_sr_report_Result>("EXEC stp_sr_report @start_date, @end_date, @sr_id",
                stDate, edDate, sId).ToList();
            return srReport;
        }

        public List<stp_sr_date_wise_incentive_Result> GetSRIncentiveDataByDate(string fromDate, string toDate, long dType)
        {
            var targetType = new SqlParameter
            {
                ParameterName = "@start_date",
                Value = fromDate
            };

            var targetFor = new SqlParameter
            {
                ParameterName = "@end_date",
                Value = toDate
            };
            var distType = new SqlParameter
            {
                ParameterName = "@dist_type",
                Value = dType
            };

            List<stp_sr_date_wise_incentive_Result> listData = _context.Database.SqlQuery<stp_sr_date_wise_incentive_Result>("EXEC stp_sr_date_wise_incentive @start_date, @end_date, @dist_type",
                targetFor, targetType, distType).ToList();
            return listData;
        }


        public vmSRStatusCount GetSRStatusCountData(string stringDealerCode, string startDate, string endDate)
        {
            try
            {
                _context.Configuration.LazyLoadingEnabled = false;
                _context.Database.CommandTimeout = 20000;
                string query = string.Format(@" SELECT  COUNT(sr.Id) AS TotalSR, 
 COUNT(so.SRId) AS ActiveSr, 
 (COUNT(sr.Id)- COUNT(so.SRId))AS inActiveSr  
 FROM [SalesRepresentatives] sr 
 left JOIN (SELECT DISTINCT SRID FROM  [RetailerOrder] where convert(date, OrderDate) between '{0}' and '{1}') so 
 ON (sr.Id=so.SRId)
 WHERE sr.IsActive=1 AND DealerCode in ('{2}')", startDate, endDate, stringDealerCode);
                var data = _context.Database.SqlQuery<vmSRStatusCount>(query).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<SalesRepresentativeModel> GetAllActiveSr(string stringDealerCode, string startDate, string endDate)
        {
            try
            {
                _context.Configuration.LazyLoadingEnabled = false;
                _context.Database.CommandTimeout = 20000;
                string query = string.Format(@" SELECT  *  FROM [SalesRepresentatives] WHERE  IsActive=1 AND DealerCode in ('{0}') AND id in (SELECT DISTINCT SRID FROM  [RetailerOrder] where convert(date, OrderDate) between '{1}' and '{2}')", stringDealerCode, startDate, endDate);
                var data = _context.Database.SqlQuery<SalesRepresentativeModel>(query).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<SalesRepresentativeModel> GetAllInActiveSr(string stringDealerCode, string startDate, string endDate)
        {
            try
            {
                _context.Configuration.LazyLoadingEnabled = false;
                _context.Database.CommandTimeout = 20000;
                string query = string.Format(@" SELECT  *  FROM [SalesRepresentatives] WHERE  IsActive=1 AND DealerCode in ('{0}') AND  id NOT IN (SELECT DISTINCT SRID FROM  [RetailerOrder] where convert(date, OrderDate) between '{1}' and '{2}')", stringDealerCode, startDate, endDate);
                var data = _context.Database.SqlQuery<SalesRepresentativeModel>(query).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<stp_sr_sales_incentive_Result> GetSRIncentiveAdminDataByDate(string fromDate, string toDate, string dCode, long srId, string model)
        {
            var stDate = new SqlParameter {ParameterName = "@start_date",Value = fromDate};
            var edDate = new SqlParameter{ParameterName = "@end_date",Value = toDate};
            var sId = new SqlParameter{ParameterName = "@sr_id",Value = srId};
            var mdl = new SqlParameter{ParameterName = "@model_name",Value = model};
            var digitechCode = new SqlParameter { ParameterName = "@digitech_code", Value = dCode };


            List<stp_sr_sales_incentive_Result> data =
                _context.Database.SqlQuery<stp_sr_sales_incentive_Result>("EXEC stp_sr_sales_incentive @start_date, @end_date, @sr_id, @model_name, @digitech_code",
                stDate, edDate, sId,mdl, digitechCode).ToList();
            return data;
        }

        public List<stp_sr_three_months_incentive_Result> GetSrThreeMonthsIncentive(string dealerCode)
        {
            var dCode = new SqlParameter
            {
                ParameterName = "@dealer_code",
                Value = dealerCode
            };


            List<stp_sr_three_months_incentive_Result> data = _context.Database.SqlQuery<stp_sr_three_months_incentive_Result>("EXEC stp_sr_three_months_incentive @dealer_code", dCode).ToList();
            return data;
        }
    }
}