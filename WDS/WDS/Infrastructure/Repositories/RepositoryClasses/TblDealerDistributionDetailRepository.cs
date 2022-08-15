using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.RBSYNERGYEntities;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class TblDealerDistributionDetailRepository:RbsynergyRepository<tblDealerDistributionDetail>, ITblDealerDistributionDetailRepository
    {
        private readonly RBSYNERGYEntities _context;
        public TblDealerDistributionDetailRepository(RBSYNERGYEntities context) : base(context)
        {
            _context = context;
        }

        public VmDealerStockAdd GetRbsynergyDistibutionByImei(string imei)
        {
            VmDealerStockAdd dealerStockAdd = (from detail in _context.tblDealerDistributionDetails
                join codeInv in _context.tblBarCodeInvs on detail.BarCode equals codeInv.BarCode
                join dealerInfo in _context.tblDealerInfoes on detail.DealerCode equals dealerInfo.DealerCode

                where detail.BarCode == imei || detail.BarCode2 == imei
                select new VmDealerStockAdd
                {
                    DealerName = dealerInfo.DealerName,
                    DealerCode = dealerInfo.DealerCode,
                    Model = codeInv.Model,
                    ImeiOne = codeInv.BarCode,
                    ImeiTwo = codeInv.BarCode2,
                    Color = codeInv.Color,
                    DistributionDate = detail.DistributionDate
                    
                }).FirstOrDefault();
            return dealerStockAdd;
        }
    }
}