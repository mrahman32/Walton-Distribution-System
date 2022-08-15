using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
   
    public class DistributorOrderRepository:Repository<DistributorOrder>, IDistributorOrderRepository
    {
        private readonly WDSEntities _context;
        public DistributorOrderRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}