using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class OtherRetailerRepository:Repository<OtherRetailer>, IOtherRetailerRepository
    {
        private readonly WDSEntities _context;
        public OtherRetailerRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}