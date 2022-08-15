using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class MOA_StampsRepository:Repository<MOA_Stamps>, IMOA_StampsRepository
    {
        private readonly WDSEntities _context;
        public MOA_StampsRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}