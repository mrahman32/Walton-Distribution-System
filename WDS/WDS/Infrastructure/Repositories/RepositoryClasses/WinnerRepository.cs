using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.RetailerMotivation;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class WinnerRepository:RetailerMotivationRepository<Winner>, IWinnerRepository
    {
        private readonly RetailerMotivationProgramEntities _context;
        public WinnerRepository(RetailerMotivationProgramEntities context) : base(context)
        {
            _context = context;
        }
    }
}