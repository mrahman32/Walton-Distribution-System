using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class TargetRepository:Repository<Target>, ITargetRepository
    {
        private readonly WDSEntities _context;
        public TargetRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<TargetModel> GetTargetModelList()
        {
            List<TargetModel> models = new List<TargetModel>();
            models = (from target in _context.Targets
                join user in _context.Users on target.AddedBy equals user.Id
                join distributor in _context.Distributors on target.TargetPersonId equals distributor.Id
                where target.TargetFor == "DEALER"
                select new TargetModel
                {
                    Id = target.Id,
                    TargetName = target.TargetName,
                    ModelName = target.ModelName,
                    TargetFor = target.TargetFor,
                    TargetPersonnel = distributor.DistributorNameCellCom,
                    TargetValue = target.TargetValue,
                    TargetType = target.TargetType,
                    FromDate = target.FromDate,
                    ToDate = target.ToDate,
                    AddedDate = target.AddedDate,
                    AddedByName = user.Name
                }).ToList();


            List<TargetModel> models1 = new List<TargetModel>();
            models1 = (from target in _context.Targets
                join user in _context.Users on target.AddedBy equals user.Id
                join retailerInfo in _context.RetailerInfoes on target.TargetPersonId equals retailerInfo.Id
                where target.TargetFor == "RETAILER"
                select new TargetModel
                {
                    Id = target.Id,
                    TargetName = target.TargetName,
                    ModelName = target.ModelName,
                    TargetFor = target.TargetFor,
                    TargetPersonnel = retailerInfo.RetailerName,
                    TargetValue = target.TargetValue,
                    TargetType = target.TargetType,
                    FromDate = target.FromDate,
                    ToDate = target.ToDate,
                    AddedDate = target.AddedDate,
                    AddedByName = user.Name
                }).ToList();

            List<TargetModel> models2 = new List<TargetModel>();
            models2 = (from target in _context.Targets
                join user in _context.Users on target.AddedBy equals user.Id
                join salesRepresentative in _context.SalesRepresentatives on target.TargetPersonId equals salesRepresentative.Id
                where target.TargetFor == "SR"
                select new TargetModel
                {
                    Id = target.Id,
                    TargetName = target.TargetName,
                    ModelName = target.ModelName,
                    TargetFor = target.TargetFor,
                    TargetPersonnel = salesRepresentative.Name,
                    TargetValue = target.TargetValue,
                    TargetType = target.TargetType,
                    FromDate = target.FromDate,
                    ToDate = target.ToDate,
                    AddedDate = target.AddedDate,
                    AddedByName = user.Name
                }).ToList();

            List<TargetModel>finalList = new List<TargetModel>();
            finalList.AddRange(models);
            finalList.AddRange(models1);
            finalList.AddRange(models2);

            return finalList;

        }

        public List<stp_dealer_target_achievement_report_Result> GetDealerTargetVsAchievementReport(string targetT)
        {
            var targetType = new SqlParameter
            {
                ParameterName = "@target_type", Value = targetT
            };

            var targetFor = new SqlParameter
            {
                ParameterName = "@target_for",
                Value = "DEALER"
            };


            List<stp_dealer_target_achievement_report_Result> distributorWiseSaleResults = _context.Database.SqlQuery<stp_dealer_target_achievement_report_Result>("EXEC stp_dealer_target_achievement_report @target_for, @target_type",
                targetFor, targetType).ToList();
            return distributorWiseSaleResults;
        }
    }
}