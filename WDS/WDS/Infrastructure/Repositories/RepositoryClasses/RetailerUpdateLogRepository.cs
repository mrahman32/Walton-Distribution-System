using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class RetailerUpdateLogRepository : Repository<RetailerUpdateLog>, IRetailerUpdateLogRepository
    {
        private readonly WDSEntities _context;
        public RetailerUpdateLogRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<RetailerUpdateModel> GetPendingApproval(User user)
        {
            string query = string.Format(@"SELECT 
l.*, i.RetailerName, i.RetailerAddress RetailerOldAddress, i.PhoneNumber RetailerOldPhoneNumber, i.PaymentNumber RetailerOldPaymentNumber, i.PaymentNumberType RetailerOldPaymentNumberType
 FROM WDS.dbo.RetailerUpdateLog l
join wds.dbo.RetailerInfo i on l.RetailerId = i.Id
join wds.dbo.Distributors d on i.DistributorCode = d.DigitechCode
join wds.dbo.SalesZone z on d.ZoneId = z.ZoneId
where Z.SalesRole = 'RSM' AND Z.EmpId = '{0}' and l.RecommendBy is null", user.UserName);


            var data1 = _context.Database.SqlQuery<RetailerUpdateModel>(query).ToList();
            return data1;
        }
    }
}