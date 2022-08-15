using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface IDistributorRepository:IRepository<Distributor>
    {
        List<stp_distributor_wise_sale_Result> GetDistributorWiseSalesReport(DateTime stDate, DateTime edDate);
    }
}
