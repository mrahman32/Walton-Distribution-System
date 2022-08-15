using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class ThanaRepository : Repository<ThanaList>, IThanaRepository
    {
         private readonly WDSEntities _context;
         public ThanaRepository(WDSEntities context)
             : base(context)
        {
            _context = context;
        }
    }
}