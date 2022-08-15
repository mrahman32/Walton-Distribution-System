using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly WDSEntities _context;
        public ProductRepository(WDSEntities context)
            : base(context)
        {
            _context = context;
        }

        public List<TopMobileModel> GetTopModelSold(string phoneType)
        {
            var phoneTypeParam = new SqlParameter
            {
                ParameterName = "@PhoneType",
                Value = phoneType
            };
            

            List<TopMobileModel> mmms = _context.Database.SqlQuery<TopMobileModel>("EXEC TopTenModelSold @PhoneType",
                phoneTypeParam).ToList();
            return mmms;
        }

        public List<DashboardSalesAndActivationCountModel> GetSalesCount(string dayOrMonth)
        {
            var param = new SqlParameter
            {
                ParameterName = "@dayOrMonth",
                Value = dayOrMonth
            };


            List<DashboardSalesAndActivationCountModel> mmms = _context.Database.SqlQuery<DashboardSalesAndActivationCountModel>("EXEC RetailerDailySalesCount @dayOrMonth",
                param).ToList();
            return mmms;
        }

        public List<DashboardSalesAndActivationCountModel> GetActivationCount(string dayOrMonth)
        {
            var param = new SqlParameter
            {
                ParameterName = "@dayOrMonth",
                Value = dayOrMonth
            };


            List<DashboardSalesAndActivationCountModel> mmms = _context.Database.SqlQuery<DashboardSalesAndActivationCountModel>("EXEC RetailerActivationCount @dayOrMonth",
                param).ToList();
            return mmms;
        }
    }
}