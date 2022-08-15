using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class ExtendedLimitRepository:Repository<ExtendedLimit>, IExtendedLimitRepository
    {
        private readonly WDSEntities _context;
        public ExtendedLimitRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}