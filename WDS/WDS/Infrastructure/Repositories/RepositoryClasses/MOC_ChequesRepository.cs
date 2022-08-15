using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class MOC_ChequesRepository:Repository<MOC_Cheques>, IMOC_ChequesRepository
    {
        private readonly WDSEntities _context;
        public MOC_ChequesRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}