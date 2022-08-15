using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class BankSolvencyRepository : Repository<BankSolvency>, IBankSolvencyRepository
    {
        private readonly WDSEntities _context;
        public BankSolvencyRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<Bank> GetBankList()
        {
            return _context.Banks.ToList();
        }

        public List<BankBranch> GetBankBranchList()
        {
            return _context.BankBranches.ToList();
        }
    }
}