using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.RetailerMotivation;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class RetailerMotivationUnit:IUnit
    {
        private readonly RetailerMotivationProgramEntities _context;
        private bool _disposed;

        public IWinnerRepository WinnerRepository { get; set; }


        public RetailerMotivationUnit(RetailerMotivationProgramEntities context)
        {
            _context = context;

            WinnerRepository = new WinnerRepository(_context);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            IRepository<TEntity> repository = new RetailerMotivationRepository<TEntity>(_context);
            return repository;
        }

        public void Commit()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}