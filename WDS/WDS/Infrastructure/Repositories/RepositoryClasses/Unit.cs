using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class Unit : IUnit
    {
        private readonly WDSEntities _context;
        private bool _disposed;

        public IDealerDistributionRepository DealerDistributionRepository { get; set; }
        public IRetailerDistributionRepository RetailerDistributionRepository { get; set; }
        public IRetailerInfoRepository RetailerInfoRepository { get; set; }
        public IRetailerOrderRepository RetailerOrderRepository { get; set; }
        public IRetailerOrderDetailRepository RetailerOrderDetailRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ISalesRepresentativeRepository SalesRepresentativeRepository { get; set; }
        public IMainMenuRepository MainMenuRepository { get; set; }
        public ISubMenuRepository SubMenuRepository { get; set; }
        public IDashboardImageRepository DashboardImageRepository { get; set; }
        public IPartyRepository PartyRepository { get; set; }
        public IIncentiveDistributionRepository IncentiveDistributionRepository { get; set; }
        public IProductMasterWdsRepository ProductMasterWdsRepository { get; set; }
        public ITargetRepository TargetRepository { get; set; }
        public IDistributorRepository DistributorRepository { get; set; }
        public IModelPriceRepository ModelPriceRepository { get; set; }
        public IDealerDistributionDetailsRepository DealerDistributionDetailsRepository { get; set; }
        public IMarketShareRepository MarketShareRepository { get; set; }
        public IBrandRepository BrandRepository { get; set; }
        public IZoneRepository ZoneRepository { get; set; }
        public IDivisionRepository DivisionRepository { get; set; }
        public IDistrictsRepository DistrictsRepository { get; set; }
        public IThanaRepository ThanaRepository { get; set; }
        public IExtendedLimitRepository ExtendedLimitRepository { get; set; }
        public IDistributorOrderRepository DistributorOrderRepository { get; set; }
        public IDistributorOrderDetailRepository DistributorOrderDetailRepository { get; set; }
        public IDistributorOrderLogRepository DistributorOrderLogRepository { get; set; }
        public IModelColorRepository ModelColorRepository { get; set; }
        public ISalesTeamRepository SalesTeamRepository { get; set; }
        public ISalesZoneRepository SalesZoneRepository { get; set; }
        public IRetailerUpdateLogRepository RetailerUpdateLogRepository { get; set; }
        public IProductBrandRepository ProductBrandRepository { get; set; }
        public IDistributorTypeRepository DistributorTypeRepository { get; set; }
        public IAvailabilityBroadcastRepository AvailabilityBroadcastRepository { get; set; }

        public ISendSmsLogRepository SendSmsLogRepository { get; set; }
        public IMisPersonRepository MisPersonRepository { get; set; }

        public IBankSolvencyRepository BankSolvencyRepository { get; set; }
        public IBankSolvenciesUpdateLogRepository BankSolvenciesUpdateLogRepository { get; set; }
        public IDistributorCheckListRepository DistributorCheckListRepository { get; set; }
        public IDistributorOthersInformationRepository DistributorOthersInformationRepository { get; set; }
        public IDistributorOthersInformationUpdateLogRepository DistributorOthersInformationUpdateLogRepository { get; set; }
        public IMemorandumOfAgreementRepostory MemorandumOfAgreementRepostory { get; set; }
        public IMemorandumOfAgreementUpdateLogRepository MemorandumOfAgreementUpdateLogRepository { get; set; }
        public IMemorandumOfChequeRepostory MemorandumOfChequeRepostory { get; set; }
        public IMemorandumOfChequeUpdateLogRepository MemorandumOfChequeUpdateLogRepository { get; set; }
        public ITaxIdentityRepostory TaxIdentityRepostory { get; set; }
        public ITaxIdentitiesUpdateLogRepository TaxIdentitiesUpdateLogRepository { get; set; }
        public ITradeLicensRepository TradeLicensRepository { get; set; }
        public ITradeLicensesUpdateLogRepository TradeLicensesUpdateLogRepository { get; set; }
        public IValueAddedTaxesRepository ValueAddedTaxesRepository { get; set; }
        public IValueAddedTaxesUpdateLogRepository ValueAddedTaxesUpdateLogRepository { get; set; }
        public IDistributorPersonalInformationRepository DistributorPersonalInformationRepository { get; set; }

        public IVarificationApprovalRepostitory VarificationApprovalRepostitory { get; set; }
        public IVarificationEntityRepository VarificationEntityRepository { get; set; }
        public IMOC_ChequesRepository MocChequesRepository { get; set; }
        public IMOA_StampsRepository MoaStampsRepository { get; set; }
        public Unit(WDSEntities context)
        {
            _context = context;
            _context.Database.CommandTimeout = 180;
            DealerDistributionRepository = new DealerDistributionRepository(_context);
            RetailerDistributionRepository = new RetailerDistributionRepository(_context);
            RetailerInfoRepository = new RetailerInfoRepository(_context);
            RetailerOrderRepository = new RetailerOrderRepository(_context);
            RetailerOrderDetailRepository = new RetailerOrderDetailRepository(_context);
            RoleRepository = new RoleRepository(_context);
            UserRepository = new UserRepository(_context);
            ProductRepository = new ProductRepository(_context);
            SalesRepresentativeRepository = new SalesRepresentativeRepository(_context);
            MainMenuRepository = new MainMenuRepository(_context);
            SubMenuRepository = new SubMenuRepository(_context);
            DashboardImageRepository = new DashboardImageRepository(_context);
            PartyRepository = new PartyRepository(_context);
            IncentiveDistributionRepository = new IncentiveDistributionRepository(_context);
            ProductMasterWdsRepository = new ProductMasterWdsRepository(_context);
            TargetRepository = new TargetRepository(_context);
            DistributorRepository = new DistributorRepository(_context);
            ModelPriceRepository = new ModelPriceRepository(_context);
            DealerDistributionDetailsRepository = new DealerDistributionDetailsRepository(_context);
            MarketShareRepository = new MarketShareRepository(_context);
            BrandRepository = new BrandRepository(_context);
            ZoneRepository = new ZoneRepository(_context);
            DivisionRepository = new DivisionRepository(_context);
            DistrictsRepository = new DistrictsRepository(_context);
            ThanaRepository = new ThanaRepository(_context);
            ExtendedLimitRepository = new ExtendedLimitRepository(_context);
            DistributorOrderRepository = new DistributorOrderRepository(_context);
            DistributorOrderDetailRepository = new DistributorOrderDetailRepository(_context);
            DistributorOrderLogRepository = new DistributorOrderLogRepository(_context);
            ModelColorRepository = new ModelColorRepository(_context);
            SalesTeamRepository = new SalesTeamRepository(_context);
            SalesZoneRepository = new SalesZoneRepository(_context);
            RetailerUpdateLogRepository = new RetailerUpdateLogRepository(_context);

            ProductBrandRepository = new ProductBrandRepository(_context);
            DistributorTypeRepository = new DistributorTypeRepository(_context);
            AvailabilityBroadcastRepository = new AvailabilityBroadcastRepository(_context);
            SendSmsLogRepository = new SendSmsLogRepository(_context);
            MisPersonRepository = new MisPersonRepository(_context);
            BankSolvencyRepository = new BankSolvencyRepository(_context);
            BankSolvenciesUpdateLogRepository = new BankSolvenciesUpdateLogRepository(_context);
            DistributorCheckListRepository = new DistributorCheckListRepository(_context);
            DistributorOthersInformationRepository = new DistributorOthersInformationRepository(_context);
            DistributorOthersInformationUpdateLogRepository =
                new DistributorOthersInformationUpdateLogRepository(_context);
            MemorandumOfAgreementRepostory = new MemorandumOfAgreementRepository(_context);
            MemorandumOfAgreementUpdateLogRepository = new MemorandumOfAgreementUpdateLogRepository(_context);
            MemorandumOfChequeRepostory = new MemorandumOfChequeRepository(_context);
            MemorandumOfAgreementUpdateLogRepository = new MemorandumOfAgreementUpdateLogRepository(_context);
            TaxIdentityRepostory = new TaxIdentityRepository(_context);
            TaxIdentitiesUpdateLogRepository = new TaxIdentitiesUpdateLogRepository(_context);
            TradeLicensRepository = new TradeLicensRepository(_context);
            TradeLicensesUpdateLogRepository = new TradeLicensesUpdateLogRepository(_context);
            ValueAddedTaxesRepository = new ValueAddedTaxRepository(_context);
            ValueAddedTaxesUpdateLogRepository = new ValueAddedTaxesUpdateLogRepository(_context);
            DistributorPersonalInformationRepository = new DistributorPersonalInformationRepository(_context);
            VarificationApprovalRepostitory = new VarificationApprovalRepository(_context);
            VarificationEntityRepository = new VarificationEntityRepository(_context);
            MocChequesRepository = new MOC_ChequesRepository(_context);
            MoaStampsRepository = new MOA_StampsRepository(_context);

        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            IRepository<TEntity> repository = new Repository<TEntity>(_context);
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

        public List<CheckListApprovalPendingModel> GetCustomModelListForDistributorDocumentApprovalPending()
        {
            List<CheckListApprovalPendingModel> models = (from checkList in _context.DistributorCheckLists
                join user in _context.Users on checkList.AddedBy equals user.Id
                join distributor in _context.Distributors on checkList.DealerCode equals distributor.DigitechCode

                select new CheckListApprovalPendingModel
                {
                    CheckListId = checkList.Id,
                    AddedDate = (DateTime) checkList.AddedDate,
                    AddedByName = user.Name,
                    DealerCode = checkList.DealerCode,
                    DealerName = distributor.DistributorNameCellCom,
                    IsApproved = (bool) checkList.IsCreationApproved,
                    IsBs = checkList.BankSolvencies.Any(),
                    IsDother = checkList.DistributorOthersInformations.Any(),
                    IsMoa = checkList.MemorandumOfAgreements.Any(),
                    IsMoc = checkList.MemorandumOfCheques.Any(),
                    IsTin = checkList.TaxIdentities.Any(),
                    IsTl = checkList.TradeLicenses.Any(),
                    IsVat = checkList.ValueAddedTaxes.Any()


                }).ToList();
            return models;
        }
    }
}