using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.RBSYNERGYEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class ProductMasterRepository:RbsynergyRepository<tblProductMaster>,IProductMasterRepository
    {
        private readonly RBSYNERGYEntities _context;
        public ProductMasterRepository(RBSYNERGYEntities context) : base(context)
        {
            _context = context;
        }
    }
}