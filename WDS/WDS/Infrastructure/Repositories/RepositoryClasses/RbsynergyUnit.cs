using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.RBSYNERGYEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class RbsynergyUnit : IUnit
    {
        private readonly RBSYNERGYEntities _context;
        private bool _disposed;


        public IDealerInfoRepository DealerInfoRepository { get; set; }
        public IProductMasterRepository ProductMasterRepository { get; set; }
        public ICellPhonePricingRepository CellPhonePricingRepository { get; set; }
        public IProductRegistrationRepository ProductRegistrationRepository { get; set; }
        public ITblDealerDistributionDetailRepository TblDealerDistributionDetailRepository { get; set; }
        public RbsynergyUnit(RBSYNERGYEntities context)
        {
            _context = context;

            DealerInfoRepository = new DealerInfoRepository(_context);
            ProductMasterRepository = new ProductMasterRepository(_context);
            CellPhonePricingRepository = new CellPhonePricingRepository(_context);
            ProductRegistrationRepository = new ProductRegistrationRepository(_context);
            TblDealerDistributionDetailRepository = new TblDealerDistributionDetailRepository(_context);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            IRepository<TEntity> repository = new RbsynergyRepository<TEntity>(_context);
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