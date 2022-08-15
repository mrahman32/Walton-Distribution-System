using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DistributorOrderDetailRepository:Repository<DistributorOrderDetail>, IDistributorOrderDetailRepository
    {
        private readonly WDSEntities _context;
        public DistributorOrderDetailRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}