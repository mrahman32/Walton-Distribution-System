using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class SalesZoneRepository:Repository<SalesZone>, ISalesZoneRepository
    {
        private readonly WDSEntities _context;
        public SalesZoneRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<string> GetZoneNamesBySalesTeam(string userName)
        {
            List<string> zoneNames =
                _context.SalesZones.Where(i => i.EmpId == userName).Select(i => i.ZoneName).ToList();
            return zoneNames;
        }
    }
}