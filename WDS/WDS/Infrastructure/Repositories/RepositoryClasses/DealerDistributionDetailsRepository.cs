using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DealerDistributionDetailsRepository:Repository<DealerDistributionDetail>, IDealerDistributionDetailsRepository
    {
        private readonly WDSEntities _context;
        public DealerDistributionDetailsRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public VmDealerStockAdd GetDealerDistribution(string imei, Distributor distributor)
        {
            VmDealerStockAdd dealerStockAdd = (from x in _context.DealerDistributionDetails
                where (x.ImeiTwo == imei || x.ImeiOne == imei) && (x.DealerCode == distributor.DigitechCode || x.DealerCode == distributor.ImportCode)
                select new VmDealerStockAdd
                {
                    DealerName = distributor.DistributorNameCellCom,
                    DealerCode = distributor.DigitechCode,
                    Model = x.Model,
                    ImeiOne = x.ImeiOne,
                    ImeiTwo = x.ImeiTwo,
                    Color = x.Color,
                    DistributionDate = x.DistributionDate

                }).FirstOrDefault();
            //if (dealerStockAdd == null)
            //{
            //    dealerStockAdd = (from x in _context.DealerDistributionDetails
            //        join y in _context.Distributors
            //            on x.DealerCode equals y.ImportCode
            //        where x.ImeiTwo == imei || x.ImeiOne == imei
            //        select new VmDealerStockAdd
            //        {
            //            DealerName = y.DistributorName,
            //            DealerCode = y.DigitechCode,
            //            Model = x.Model,
            //            ImeiOne = x.ImeiOne,
            //            ImeiTwo = x.ImeiTwo,
            //            Color = x.Color,
            //            DistributionDate = x.DistributionDate

            //        }).FirstOrDefault();
            //}
            return dealerStockAdd;
        }
    }
}