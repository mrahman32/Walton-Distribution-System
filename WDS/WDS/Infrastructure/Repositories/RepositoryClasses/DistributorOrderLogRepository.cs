using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DistributorOrderLogRepository:Repository<DistributorOrderLog>, IDistributorOrderLogRepository
    {
        private readonly WDSEntities _context;
        public DistributorOrderLogRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }
    }
}