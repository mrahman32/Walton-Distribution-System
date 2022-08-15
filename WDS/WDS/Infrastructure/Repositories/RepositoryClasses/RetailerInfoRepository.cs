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
    public class RetailerInfoRepository:Repository<RetailerInfo>, IRetailerInfoRepository
    {
        private readonly WDSEntities _context;
        public RetailerInfoRepository(WDSEntities context) : base(context)
        {
            _context = context;
        }

        public List<stp_retailer_date_wise_incentive_Result> GetRetailerIncentiveDataByDate(string fromDate, string toDate, long dType)
        {
            var targetType = new SqlParameter
            {
                ParameterName = "@start_date",
                Value = fromDate
            };

            var targetFor = new SqlParameter
            {
                ParameterName = "@end_date",
                Value = toDate
            };
            var distType = new SqlParameter
            {
                ParameterName = "@dist_type",
                Value = dType
            };

            List<stp_retailer_date_wise_incentive_Result> listData = _context.Database.SqlQuery<stp_retailer_date_wise_incentive_Result>("EXEC stp_retailer_date_wise_incentive @start_date, @end_date, @dist_type",
                targetFor, targetType, distType).ToList();
            return listData;
        }


        public List<stp_retailer_sales_stock_Result> GetRetailerStockAndSaleData(string fromDate, string toDate, string dealerCode, string modelName, long retailerId, User user)
        {
            try
            {
                string zoneName = string.Empty;
                string empId = string.Empty;
                var role = _context.Roles.FirstOrDefault(i => i.Id == user.RoleId);
                if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management")
                                                                  || role.RoleName.ToLower().Contains("nsm")
                                                                  || role.RoleName.ToLower().Contains("rsm")
                                                                  || role.RoleName.ToLower().Contains("asm")
                                                                  || role.RoleName.ToLower().Contains("tso"))
                {
                    zoneName = "RSM";
                    empId = "0";
                    List<SalesZone> salesZones = _context.SalesZones.ToList();
                    List<string> zones;

                    

                    if (role.RoleName.ToLower().Contains("rsm")
                        || role.RoleName.ToLower().Contains("asm")
                        || role.RoleName.ToLower().Contains("tso"))
                    {
                        zoneName = role.RoleName.ToUpper();
                        empId = user.UserName;
                        //salesZones.Where(i => i.EmpId == user.UserName).Select(i => i.ZoneName).ToList();

                    }
                    //else
                    //{
                    //    zones = salesZones.Select(i => i.ZoneName).ToList().Distinct().ToList();

                    //}

                    //foreach (var zone in zones)
                    //{
                    //    if (zone.ToLower().Contains("cox's"))
                    //    {
                    //        string z = zone.Replace("'", "''");
                    //        zoneName = zoneName + z + ",";
                    //    }
                    //    else
                    //    {
                    //        zoneName = zoneName + zone + ",";
                    //    }

                    //}

                    //if (zoneName.Length > 0)
                    //{
                    //    zoneName = zoneName.Remove(zoneName.Length - 1, 1);
                    //}


                }
                else
                {
                    zoneName = user.UserName;
                }




                var sDate = new SqlParameter { ParameterName = "@start_date", Value = fromDate };
                var eDate = new SqlParameter { ParameterName = "@end_date", Value = toDate };
                var dCode = new SqlParameter { ParameterName = "@dealer_code", Value = dealerCode };
                var mName = new SqlParameter { ParameterName = "@model_name", Value = modelName };
                var retaId = new SqlParameter { ParameterName = "@retaile_id", Value = retailerId };
                var zoneN = new SqlParameter { ParameterName = "@zone_name", Value = zoneName };
                var employeeId = new SqlParameter { ParameterName = "@emp_id", Value = empId };

                List<stp_retailer_sales_stock_Result> listData = _context.Database.SqlQuery<stp_retailer_sales_stock_Result>("EXEC stp_retailer_sales_stock @start_date, @end_date, @dealer_code, @model_name, @retaile_id, @zone_name, @emp_id",
                    sDate, eDate, dCode, mName, retaId, zoneN, employeeId).ToList();
                var finalLst = new List<stp_retailer_sales_stock_Result>();
                foreach (var v in listData)
                {
                    var obj = new stp_retailer_sales_stock_Result
                    {
                        Dealername = v.Dealername,
                        DigitechCode = v.DigitechCode,
                        LiftingQty = v.LiftingQty,//  v.LiftingQty ?? 0,
                        LiftingValue = v.LiftingValue??0,
                        Model = v.Model,
                        PhoneNumber = v.PhoneNumber,
                        RetailerAddress = v.RetailerAddress,
                        RetailerId = v.RetailerId,
                        RetailerName = v.RetailerName,
                        SaleQty = v.SaleQty,
                        SaleValue = v.SaleValue??0,
                        StockQty = v.StockQty,
                        StockValue = v.StockValue??0,
                        UnitPrice = v.UnitPrice
                    };

                    finalLst.Add(obj);
                }
                return finalLst;
            }
            catch (Exception e)
            {
                
                throw;
            }
           
        }


        public vmRetailerStatusCount GetRetailerStatusCountData(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate)
        {
            try
            {
                long zId = 0;
                long.TryParse(zoneId, out zId);

                string whereExtension = string.Empty;
                if (dealerId != "0")
                {
                    whereExtension = " AND DistributorCode = '" + dealerId + "'";
                }

                if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management")
                                                              || role.RoleName.ToLower().Contains("nsm")
                                                              || role.RoleName.ToLower().Contains("rsm")
                                                              || role.RoleName.ToLower().Contains("asm")
                                                              || role.RoleName.ToLower().Contains("tso"))
                {
                    string zoneName = string.Empty;
                    List<SalesZone> salesZones = _context.SalesZones.ToList();
                    List<string>zones;
                    if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                    {
                        if (zId > 0)
                        {
                            zones = salesZones.Where(i => i.ZoneId==zId).Select(i => i.ZoneName).ToList();
                        }
                        else
                        {
                            zones = salesZones.Where(i => i.EmpId == userName).Select(i => i.ZoneName).ToList();
                        }
                        
                    }
                    else
                    {
                        if (zId > 0)
                        {
                            zones = salesZones.Where(i => i.ZoneId == zId).Select(i => i.ZoneName).ToList();
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
                            zoneName = zoneName + "'" + z + "',";
                        }
                        else
                        {
                            zoneName = zoneName + "'" + zone + "',";
                        }

                        
                    }

                    if (zoneName.Length > 0)
                    {
                        
                        zoneName = zoneName.Remove(zoneName.Length - 1, 1);
                        _context.Configuration.LazyLoadingEnabled = false;
                        _context.Database.CommandTimeout = 50;
                        

                        string query1 = string.Format(@"SELECT  COUNT(ri.Id) AS TotalRetailer, 
COUNT(so.RetailerId) AS ActiveRetailer, 
(COUNT(ri.Id)- COUNT(so.RetailerId))AS inActiveRetailer  
FROM [RetailerInfo] ri 
left JOIN (SELECT DISTINCT [RetailerId] FROM  [RetailerOrder] where convert(date, OrderDate) between '{0}' and '{1}') so 
ON (ri.Id=so.RetailerId) WHERE ri.IsActive=1 And Zone in({2}) {3}", startDate, endDate, zoneName, whereExtension);

                        var data1 = _context.Database.SqlQuery<vmRetailerStatusCount>(query1).FirstOrDefault();
                        return data1;
                    }

                }


                _context.Configuration.LazyLoadingEnabled = false;
                _context.Database.CommandTimeout = 50;

                string query = string.Format(@"SELECT  COUNT(ri.Id) AS TotalRetailer, 
COUNT(so.RetailerId) AS ActiveRetailer, 
(COUNT(ri.Id)- COUNT(so.RetailerId))AS inActiveRetailer  
FROM [RetailerInfo] ri 
left JOIN (SELECT DISTINCT [RetailerId] FROM  [RetailerOrder] where convert(date, OrderDate) between '{0}' and '{1}') so 
ON (ri.Id=so.RetailerId) WHERE ri.IsActive=1 And Zone in('{2}') {3}", startDate, endDate, userName, whereExtension);
                var data = _context.Database.SqlQuery<vmRetailerStatusCount>(query).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<RetailerInfoModel> GetAllActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate)
        {
            try
            {
                long zId = 0;
                long.TryParse(zoneId, out zId);
                string whereExtension = string.Empty;
                if (dealerId != "0")
                {
                    whereExtension = " AND DistributorCode = '" + dealerId + "'";
                }

                if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management")
                                                              || role.RoleName.ToLower().Contains("nsm")
                                                              || role.RoleName.ToLower().Contains("rsm")
                                                              || role.RoleName.ToLower().Contains("asm")
                                                              || role.RoleName.ToLower().Contains("tso"))
                {
                    string zoneName = string.Empty;
                    List<SalesZone> salesZones = _context.SalesZones.ToList();
                    List<string> zones;
                    if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                    {
                        if (zId > 0)
                        {
                            zones = salesZones.Where(i => i.ZoneId == zId).Select(i => i.ZoneName).ToList();
                        }
                        else
                        {
                            zones = salesZones.Where(i => i.EmpId == userName).Select(i => i.ZoneName).ToList();
                        }
                        
                    }
                    else
                    {
                        if (zId > 0)
                        {
                            zones = salesZones.Where(i => i.ZoneId == zId).Select(i => i.ZoneName).ToList();
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
                            zoneName = zoneName + "'" + z + "',";
                        }
                        else
                        {
                            zoneName = zoneName + "'" + zone + "',";
                        }

                    }

                    if (zoneName.Length > 0)
                    {
                        
                        zoneName = zoneName.Remove(zoneName.Length - 1, 1);
                        _context.Configuration.LazyLoadingEnabled = false;
                        _context.Database.CommandTimeout = 20000;
                        string query1 = string.Format(@"SELECT  *  FROM [RetailerInfo] WHERE  IsActive=1 AND Zone in({0}) AND  id  IN (SELECT DISTINCT RetailerId FROM  [RetailerOrder] where convert(date, OrderDate) between '{1}' and '{2}') {3}", zoneName, startDate, endDate, whereExtension);
                        var data1 = _context.Database.SqlQuery<RetailerInfoModel>(query1).ToList();
                        return data1;
                    }

                }
                _context.Configuration.LazyLoadingEnabled = false;
                _context.Database.CommandTimeout = 20000;
                string query = string.Format(@"SELECT  *  FROM [RetailerInfo] WHERE  IsActive=1 AND Zone in('{0}') AND  id  IN (SELECT DISTINCT RetailerId FROM  [RetailerOrder] where convert(date, OrderDate) between '{1}' and '{2}') {3}", userName, startDate, endDate, whereExtension);
                var data = _context.Database.SqlQuery<RetailerInfoModel>(query).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }


            //try
            //{
            //    _context.Configuration.LazyLoadingEnabled = false;
            //    _context.Database.CommandTimeout = 20000;
            //    string query = string.Format(@"  SELECT  *  FROM [RetailerInfo] WHERE  IsActive=1 AND Zone in('{0}') AND  id  IN (SELECT DISTINCT RetailerId FROM  [RetailerOrder]) ", userName);
            //    var data = _context.Database.SqlQuery<RetailerInfoModel>(query).ToList();
            //    return data;
            //}
            //catch (Exception e)
            //{
            //    return null;
            //}
        }

        public List<RetailerInfoModel> GetAllInActiveRetailer(string userName, Role role, string zoneId, string dealerId, string startDate, string endDate)
        {
            try
            {
                long zId = 0;
                long.TryParse(zoneId, out zId);
                string whereExtension = string.Empty;
                if (dealerId != "0")
                {
                    whereExtension = " AND DistributorCode = '" + dealerId + "'";
                }
                if (role.RoleName.ToLower().Contains("admin") || role.RoleName.ToLower().Contains("management")
                                                              || role.RoleName.ToLower().Contains("nsm")
                                                              || role.RoleName.ToLower().Contains("rsm")
                                                              || role.RoleName.ToLower().Contains("asm")
                                                              || role.RoleName.ToLower().Contains("tso"))
                {
                    string zoneName = string.Empty;
                    List<SalesZone> salesZones = _context.SalesZones.ToList();
                    List<string> zones;
                    if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
                    {
                        if (zId > 0)
                        {
                            zones = salesZones.Where(i => i.ZoneId == zId).Select(i => i.ZoneName).ToList();
                        }
                        else
                        {
                            zones = salesZones.Where(i => i.EmpId == userName).Select(i => i.ZoneName).ToList();
                        }

                    }
                    else
                    {
                        if (zId > 0)
                        {
                            zones = salesZones.Where(i => i.ZoneId == zId).Select(i => i.ZoneName).ToList();
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
                            zoneName = zoneName + "'" + z + "',";
                        }
                        else
                        {
                            zoneName = zoneName + "'" + zone + "',";
                        }

                    }

                    if (zoneName.Length > 0)
                    {
                        zoneName = zoneName.Remove(zoneName.Length - 1, 1);
                        _context.Configuration.LazyLoadingEnabled = false;
                        _context.Database.CommandTimeout = 20000;
                        string query1 = string.Format(@" SELECT  *  FROM [RetailerInfo] WHERE  IsActive=1 AND Zone in({0}) AND id NOT IN (SELECT DISTINCT RetailerId FROM  [RetailerOrder] where convert(date, OrderDate) between '{1}' and '{2}') {3}", zoneName, startDate, endDate, whereExtension);
                        var data1 = _context.Database.SqlQuery<RetailerInfoModel>(query1).ToList();
                        return data1;
                    }
                    
                }
                _context.Configuration.LazyLoadingEnabled = false;
                _context.Database.CommandTimeout = 20000;
                string query = string.Format(@" SELECT  *  FROM [RetailerInfo] WHERE  IsActive=1 AND Zone in('{0}') AND id NOT IN (SELECT DISTINCT RetailerId FROM  [RetailerOrder] where convert(date, OrderDate) between '{1}' and '{2}') {3}", userName, startDate, endDate, whereExtension);
                var data = _context.Database.SqlQuery<RetailerInfoModel>(query).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}