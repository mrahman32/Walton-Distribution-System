using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Repositories.RepositoryClasses
{
    public class DealerDistributionRepository : Repository<DealerDistribution>, IDealerDistributionRepository
    {
        private readonly WDSEntities _context;

        public DealerDistributionRepository(WDSEntities context)
            : base(context)
        {
            _context = context;
        }

        public List<DealerStockCheck_Result> GetDealerStockData(string dealerCode, string alternateCode, string newEbsCode)
        {
            var dCode = new SqlParameter
            {
                ParameterName = "@dealer_code",
                Value = dealerCode
            };

            var alDCode = new SqlParameter
            {
                ParameterName = "@alternate_dealer_code",
                Value = dealerCode
            };

            var ebsCode = new SqlParameter
            {
                ParameterName = "@new_ebs_dealer_code",
                Value = dealerCode
            };


            List<DealerStockCheck_Result> dealerStockChekResults = _context.Database.SqlQuery<DealerStockCheck_Result>("EXEC DealerStockCheck @dealer_code, @alternate_dealer_code, @new_ebs_dealer_code",
                dCode, alDCode, ebsCode).ToList();
            return dealerStockChekResults;
        }
        public List<stp_distributor_sales_and_stock_Result> GetDealerSaleAndStockData(vmDealerAndRetailerStock objvmDealerAndRetailerStock, Role role)
        {
            _context.Database.CommandTimeout = 600000;
            string zoneName = string.Empty;
            if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management")
                                                              || role.RoleName.ToLower().Contains("nsm")
                                                              || role.RoleName.ToLower().Contains("rsm")
                                                              || role.RoleName.ToLower().Contains("asm")
                                                              || role.RoleName.ToLower().Contains("tso"))

            {

                List<SalesZone> salesZones = _context.SalesZones.ToList();
                List<string> zones;

                long zoneId = 0;
                long.TryParse(objvmDealerAndRetailerStock.ZoneId, out zoneId);

                if (role.RoleName.ToLower().Contains("rsm")
                    || role.RoleName.ToLower().Contains("asm")
                    || role.RoleName.ToLower().Contains("tso"))
                {
                    if (zoneId > 0)
                    {
                        zones = salesZones.Where(i => i.EmpId == objvmDealerAndRetailerStock.userName && i.ZoneId == zoneId).Select(i => i.ZoneName).ToList();
                    }
                    else
                    {
                        zones = salesZones.Where(i => i.EmpId == objvmDealerAndRetailerStock.userName).Select(i => i.ZoneName).ToList();
                    }

                }
                else
                {
                    if (zoneId > 0)
                    {
                        zones = salesZones.Where(i => i.ZoneId == zoneId).Select(i => i.ZoneName).ToList().Distinct().ToList();
                    }
                    else
                    {
                        zones = salesZones.Select(i => i.ZoneName).ToList().Distinct().ToList();
                    }


                }

                foreach (var zone in zones)
                {
                    if (zone.ToLower().Contains("cox's"))
                    {
                        string z = zone.Replace("'", "''");
                        zoneName = zoneName + z + ",";
                    }
                    else
                    {
                        zoneName = zoneName + zone + ",";
                    }

                }

                if (zoneName.Length > 0)
                {
                    zoneName = zoneName.Remove(zoneName.Length - 1, 1);
                }


            }
            else if (role.RoleName.ToLower().Contains("dealer"))
            {
                zoneName = _context.Distributors.FirstOrDefault(i => i.MobileNo == objvmDealerAndRetailerStock.userName).Zone;
            }
            else
            {
                zoneName = objvmDealerAndRetailerStock.userName;
            }




            var sDate = new SqlParameter
            {
                ParameterName = "@start_date",
                Value = objvmDealerAndRetailerStock.StartDate
            };

            var Edate = new SqlParameter
            {
                ParameterName = "@end_date",
                Value = objvmDealerAndRetailerStock.EndDate
            };

            var DCode = new SqlParameter
            {
                ParameterName = "@dealer_code",
                Value = objvmDealerAndRetailerStock.Dealer
            };
            var md = new SqlParameter
            {
                ParameterName = "@model_name",
                Value = objvmDealerAndRetailerStock.Model
            };
            var typName = new SqlParameter
            {
                ParameterName = "@type_name",
                Value = objvmDealerAndRetailerStock.Category
            };
            var zoneParam = new SqlParameter
            {
                ParameterName = "@zone_name",
                Value = zoneName
            };


            List<stp_distributor_sales_and_stock_Result> data = _context.Database.SqlQuery<stp_distributor_sales_and_stock_Result>("EXEC stp_distributor_sales_and_stock @start_date, @end_date, @dealer_code, @model_name, @type_name, @zone_name",
                sDate, Edate, DCode, md, typName, zoneParam).ToList();
            return data;
        }

        public List<stp_check_imei_status_Result> GetImeiInformation(string imei)
        {
            var imeiParam = new SqlParameter
            {
                ParameterName = "@imei",
                Value = imei
            };

            List<stp_check_imei_status_Result> checkImeiStatusResults = _context.Database.SqlQuery<stp_check_imei_status_Result>("EXEC stp_check_imei_status @imei",
                imeiParam).ToList();
            return checkImeiStatusResults;
        }
    }
}