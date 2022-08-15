using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class BankSolvenciesUpdateLogRepository : Repository<BankSolvenciesUpdateLog>, IBankSolvenciesUpdateLogRepository
    {
        private readonly WDSEntities _context;
        public BankSolvenciesUpdateLogRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}