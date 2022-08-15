using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class MarketShareRepository:Repository<MarketShare>, IMarketShareRepository
    {
        private readonly WDSEntities _context;
        public MarketShareRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<stp_management_area_wise_top_brand_Result> AreaWiseTopBrand(int monthNo, int yearNo, long zoneId)
        {
            var mNumber = new SqlParameter
            {
                ParameterName = "@month_number",
                Value = monthNo
            };

            var yNumber = new SqlParameter
            {
                ParameterName = "@year_number",
                Value = yearNo
            };
            var zId = new SqlParameter
            {
                ParameterName = "@zone_id",
                Value = zoneId
            };



            List<stp_management_area_wise_top_brand_Result> data = _context.Database.SqlQuery<stp_management_area_wise_top_brand_Result>("EXEC stp_management_area_wise_top_brand @month_number, @year_number, @zone_id",
                mNumber, yNumber, zId).ToList();
            return data;
        }

        public List<stp_management_area_wise_market_share_Result> GetWaltonMarketShare(int monthNumber, int yearNumber, long zoneId)
        {
            var mNumber = new SqlParameter
            {
                ParameterName = "@monthNumber",
                Value = monthNumber
            };

            var yNumber = new SqlParameter
            {
                ParameterName = "@yearNumber",
                Value = yearNumber
            };

            var ZId = new SqlParameter
            {
                ParameterName = "@zone_id",
                Value = zoneId
            };



            List<stp_management_area_wise_market_share_Result> data = _context.Database.SqlQuery<stp_management_area_wise_market_share_Result>("EXEC stp_management_area_wise_market_share @monthNumber, @yearNumber, @zone_id",
                mNumber, yNumber, ZId).ToList();
            return data;
        }

        public List<stp_management_area_wise_market_share_walton_Result> GetMonthlyMarketSharePie(int monthNumber, int yearNumber, long zoneId)
        {
            var mNumber = new SqlParameter
            {
                ParameterName = "@monthNumber",
                Value = monthNumber
            };

            var yNumber = new SqlParameter
            {
                ParameterName = "@yearNumber",
                Value = yearNumber
            };

            var zId = new SqlParameter
            {
                ParameterName = "@zoneId",
                Value = yearNumber
            };



            List<stp_management_area_wise_market_share_walton_Result> data = _context.Database.SqlQuery<stp_management_area_wise_market_share_walton_Result>("EXEC stp_management_area_wise_market_share_walton @monthNumber, @yearNumber,@zoneId",
                mNumber, yNumber,zId).ToList();
            return data;
        }

        public List<stp_market_size_Result> GetMarketSize(int monthNo, int yearNo, long zoneId)
        {
            var mNumber = new SqlParameter
            {
                ParameterName = "@month_number",
                Value = monthNo
            };

            var yNumber = new SqlParameter
            {
                ParameterName = "@year_number",
                Value = yearNo
            };
            var zId = new SqlParameter
            {
                ParameterName = "@zone_id",
                Value = zoneId
            };



            List<stp_market_size_Result> data = _context.Database.SqlQuery<stp_market_size_Result>("EXEC stp_market_size @month_number, @year_number, @zone_id",
                mNumber, yNumber, zId).ToList();
            return data;
        }

        public List<stp_management_retailer_count_Result> GetRetailerCount(long zoneId)
        {
            
            var zId = new SqlParameter
            {
                ParameterName = "@zone_id",
                Value = zoneId
            };



            List<stp_management_retailer_count_Result> data = _context.Database.SqlQuery<stp_management_retailer_count_Result>("EXEC stp_management_retailer_count @zone_id", zId).ToList();
            return data;
        }

        public List<stp_management_retailer_list_zone_Result> GetRetailers(long zoneId)
        {
            var zId = new SqlParameter
            {
                ParameterName = "@zone_id",
                Value = zoneId
            };



            List<stp_management_retailer_list_zone_Result> data = _context.Database.SqlQuery<stp_management_retailer_list_zone_Result>("EXEC stp_management_retailer_list_zone @zone_id", zId).ToList();
            return data;
        }
    }
}