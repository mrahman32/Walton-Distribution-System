using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DistributorCheckListRepository : Repository<DistributorCheckList>, IDistributorCheckListRepository
    {
        private readonly WDSEntities _context;
        public DistributorCheckListRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<VmDistributorDocVerificationList> GetDocuments(long checkListId)
        {
            var data = (from checkList in _context.DistributorCheckLists
                where checkList.Id == checkListId
                select new
                {
                    checkList
                }

                ).FirstOrDefault();

            if (data != null)
            {
                VmDistributorDocVerificationList obj = new VmDistributorDocVerificationList();
                    obj.CheckListId = data.checkList.Id;
                    obj.DistributorCode = data.checkList.DealerCode;
                obj.FileDetails = "Bank Solvency Certificate";
                //obj.FileUrl = data.checkList.BankSolvencies.FirstOrDefault().
            }
            return null
                ;
        }
    }
}