using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface IDistributorCheckListRepository:IRepository<DistributorCheckList>
    {
        List<VmDistributorDocVerificationList> GetDocuments(long checkListId);
    }
}
