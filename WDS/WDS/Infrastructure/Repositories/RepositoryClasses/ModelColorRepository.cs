using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class ModelColorRepository:Repository<ModelColor>,IModelColorRepository
    {
        private readonly WDSEntities _context;
        public ModelColorRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}