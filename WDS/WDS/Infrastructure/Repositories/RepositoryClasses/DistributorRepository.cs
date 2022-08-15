using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DistributorRepository:Repository<Distributor>, IDistributorRepository
    {
        private readonly WDSEntities _context;
        public DistributorRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<stp_distributor_wise_sale_Result> GetDistributorWiseSalesReport(DateTime stDate, DateTime edDate)
        {
            var startDate = new SqlParameter
            {
                ParameterName = "@start_date",
                Value = stDate
            };

            var endDate = new SqlParameter
            {
                ParameterName = "@end_date",
                Value = edDate
            };


            List<stp_distributor_wise_sale_Result> distributorWiseSaleResults = _context.Database.SqlQuery<stp_distributor_wise_sale_Result>("EXEC stp_distributor_wise_sale @start_date, @end_date",
                startDate, endDate).ToList();
            return distributorWiseSaleResults;
        }
    }
}