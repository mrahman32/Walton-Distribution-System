using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface IRetailerUpdateLogRepository:IRepository<RetailerUpdateLog>
    {
        List<RetailerUpdateModel> GetPendingApproval(User user);
    }
}
