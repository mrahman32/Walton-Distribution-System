using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.RBSYNERGYEntities;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DealerInfoRepository:RbsynergyRepository<tblDealerInfo>,IDealerInfoRepository
    {
        private readonly RBSYNERGYEntities _context;
        public DealerInfoRepository(RBSYNERGYEntities context) : base(context)
        {
            _context = context;
        }
    }
}