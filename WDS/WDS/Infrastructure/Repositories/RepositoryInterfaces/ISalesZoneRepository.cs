using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface ISalesZoneRepository:IRepository<SalesZone>
    {
        List<string> GetZoneNamesBySalesTeam(string userName);
    }
}
