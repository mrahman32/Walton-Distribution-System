using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DashboardImageRepository:Repository<DashboardImage>, IDashboardImageRepository
    {
        private readonly WDSEntities _context;
        public DashboardImageRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<DASHBOARD_SALE_INFO_Result> DashboardSaleInfo(Distributor distributor)
        {
            SqlParameter digitechCode = new SqlParameter
            {
                ParameterName = "@digitech_code",
                Value = distributor.DigitechCode
            };

            SqlParameter importCode = new SqlParameter
            {
                ParameterName = "@import_code",
                Value = distributor.ImportCode??"0"
            };
            List<DASHBOARD_SALE_INFO_Result> saleResult = _context.Database.SqlQuery<DASHBOARD_SALE_INFO_Result>("EXEC DASHBOARD_SALE_INFO @digitech_code, @import_code", digitechCode, importCode).ToList();
            return saleResult;
        }
    }
}