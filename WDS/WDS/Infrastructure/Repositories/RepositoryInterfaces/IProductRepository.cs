using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        List<TopMobileModel> GetTopModelSold(string phoneType);
        List<DashboardSalesAndActivationCountModel> GetSalesCount(string dayOrMonth);
        List<DashboardSalesAndActivationCountModel> GetActivationCount(string dayOrMonth);
    }
}
