using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Helper;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;
using WDS.ViewModels;
using System.Net;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    
    public class AdminService:IAdminService
    {
        private readonly Unit _wdsUnit;

        //private readonly RbsynergyUnit _rbsynergyUnit;
        public AdminService(Unit wdsUnit)
        {
            _wdsUnit = wdsUnit;
            //_rbsynergyUnit = rbsynergyUnit;
        }

        public List<VmDashboardImage> GetImageList()
        {
            var manager = new FileManager();
            List<DashboardImage> images = _wdsUnit.DashboardImageRepository.GetAll();
            var dashboardImages = images.Select(dashboardImage => new VmDashboardImage
            {
                Id = dashboardImage.Id, 
                ImageDescription = dashboardImage.ImageDescription, 
                FilePath = manager.GetFile(dashboardImage.ImageUrl),
                IsActive = (bool) dashboardImage.IsActive
            }).ToList();

            return dashboardImages;
        }

        public Role GetRoleById(long? roleId)
        {
            if (roleId == null) return null;
            Role role = _wdsUnit.RoleRepository.Get((long) roleId);
            return role;
        }

        public bool ApproveRetailer(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                RetailerInfo retailerInfo = _wdsUnit.RetailerInfoRepository.Get(id);
                retailerInfo.IsActive = true;
                retailerInfo.IsCreationApproved = true;
                retailerInfo.CreationApprovalDate = DateTime.Now;
                retailerInfo.CreationApprovedBy = user.Id;
                _wdsUnit.RetailerInfoRepository.Update(retailerInfo);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RejectRetailer(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                RetailerInfo retailerInfo = _wdsUnit.RetailerInfoRepository.Get(id);
                retailerInfo.IsActive = false;
                retailerInfo.IsCreationApproved = false;
                retailerInfo.IsDeletionApproved = false;
                retailerInfo.DeletionApprovalDate = DateTime.Now;
                retailerInfo.DeletionApprovedBy = user.Id;
                _wdsUnit.RetailerInfoRepository.Update(retailerInfo);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<RetailerInfo> GetPendingRetailerForApproval(User user)
        {
            var distributors = _wdsUnit.DistributorRepository.Find(i => i.Zone == user.UserName).ToList();
            List<RetailerInfo> retailerList = new List<RetailerInfo>();
            foreach (var distributor in distributors)
            {
                var pendingRetailer = _wdsUnit.RetailerInfoRepository.Find(i =>
                        (i.DistributorCode == distributor.DigitechCode || i.DistributorCode2 == distributor.ImportCode ) && i.IsCreationApproved != true)
                    .ToList();

                retailerList.AddRange(pendingRetailer);
            }

            return retailerList;

            //var dealerPhoneList = new List<string>();
            //foreach (var party in distributors)
            //{
            //    Distributor party1 = party;
            //    var dealers =
            //        _wdsUnit.DistributorRepository.Find(
            //            i =>
            //                (i.ImportCode == party1.ImportCode  ||
            //                i.DigitechCode == party1.DigitechCode) && i.MobileNo != null).ToList();
            //    if (dealers.Any())
            //    {
            //        dealerPhoneList.AddRange(dealers.Select(dealer => dealer.MobileNo));
            //    }
            //    dealerPhoneList = dealerPhoneList.Distinct().ToList();



            //}
            //var appUserIds = new List<long>();
            //if (dealerPhoneList.Any())
            //{
            //    foreach (var phone in dealerPhoneList)
            //    {
            //        string phone1 = phone;
            //        var firstOrDefault = _wdsUnit.UserRepository.Find(i => i.UserName == phone1).FirstOrDefault();
            //        if (firstOrDefault != null)
            //        {
            //            var appUserId = firstOrDefault.Id;
            //            appUserIds.Add(appUserId);
            //        }
            //    }
            //}
            //appUserIds = appUserIds.Distinct().ToList();
            //var finalRetailerInfos = new List<RetailerInfo>();
            //foreach (var userId in appUserIds)
            //{
            //    long id = userId;
            //    List<RetailerInfo> retailerInfos = _wdsUnit.RetailerInfoRepository.Find(i => i.AddedBy == id && i.IsCreationApproved != true).ToList();
            //    finalRetailerInfos.AddRange(retailerInfos);
            //}

            //return finalRetailerInfos;
        }

        public List<SalesRepresentative> GetPendingSrApproval(User user)
        {
            Role role = _wdsUnit.RoleRepository.Get((long)user.RoleId);
            List<string> zoneNames;

            var zones = _wdsUnit.ZoneRepository.GetAll();
            if (role.RoleName.ToLower().Contains("rsm") || role.RoleName.ToLower().Contains("asm") || role.RoleName.ToLower().Contains("tso"))
            {
                zoneNames = _wdsUnit.SalesZoneRepository.Find(i => i.EmpId == user.UserName).Select(i => i.ZoneName).ToList();
                if (zoneNames.Count<=0)
                {
                    zoneNames.Add(user.UserName);
                }
            }
            else
            {
                zoneNames = zones.Select(i => i.ZoneName).ToList();
            }


            var distributors = _wdsUnit.DistributorRepository.Find(i => zoneNames.Contains(i.Zone)).ToList();
            var salesRepresentatives = new List<SalesRepresentative>();
            foreach (var distributor in distributors)
            {
                var pendingRepresentatives = _wdsUnit.SalesRepresentativeRepository
                    .Find(i => 
                        (i.DealerCode == distributor.DigitechCode) 
                        && (i.IsCreationApproved == null || i.IsCreationApproved == false)
                        ).ToList();


                salesRepresentatives.AddRange(pendingRepresentatives);
            }

            return salesRepresentatives;
           
        }

        public bool ApproveSr(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                SalesRepresentative salesRepresentative = _wdsUnit.SalesRepresentativeRepository.Get(id);
                salesRepresentative.IsActive = true;
                salesRepresentative.IsCreationApproved = true;
                salesRepresentative.CreationApprovalDate = DateTime.Now;
                salesRepresentative.CreationApprovedBy = user.Id;
                _wdsUnit.SalesRepresentativeRepository.Update(salesRepresentative);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RejectSr(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                SalesRepresentative salesRepresentative = _wdsUnit.SalesRepresentativeRepository.Get(id);
                salesRepresentative.IsActive = false;
                salesRepresentative.IsCreationApproved = false;
                salesRepresentative.IsDeletionApproved = true;
                salesRepresentative.DeletionApprovalDate = DateTime.Now;
                salesRepresentative.DeletionApprovedBy = user.Id;
                _wdsUnit.SalesRepresentativeRepository.Update(salesRepresentative);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeStatusUploadedImage(long id, bool status)
        {
            try
            {
                DashboardImage image = _wdsUnit.DashboardImageRepository.Get(id);
                image.IsActive = status;
                _wdsUnit.DashboardImageRepository.Update(image);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public AjaxResponseModel DeleteImage(long id)
        {
            AjaxResponseModel ajaxResponse = new AjaxResponseModel();

            try
            {
                DashboardImage dashboardImage = _wdsUnit.DashboardImageRepository.Get(id);
                FileManager fileManager = new FileManager();
                bool isDeleted = fileManager.DeleteFile(dashboardImage.ImageUrl);
                if (isDeleted)
                {
                    _wdsUnit.DashboardImageRepository.Remove(dashboardImage);
                    _wdsUnit.Commit();
                    ajaxResponse.Id = 1;
                    ajaxResponse.Message = "Image Deletion Successful";
                }
                else
                {
                    ajaxResponse.Id = 0;
                    ajaxResponse.Message = "Delete Operation could not completed.";
                }
            }
            catch (Exception e)
            {
                ajaxResponse.Id = 0;
                ajaxResponse.Message = "Delete Operation could not completed.";
            }


            return ajaxResponse;
        }

        public string SaveIncentive(IncentiveDistributionModel model)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return "Session Time out. Please Logout and Login again.";
                model.AddedBy = user.Id;
                model.AddedDate = DateTime.Now;
                //TODO: Use AutoMapper to map user defined model to database model

                var config = new MapperConfiguration(i => i.CreateMap<IncentiveDistributionModel, IncentiveDistribution>());
                IMapper iMapper = config.CreateMapper();
                var destination = iMapper.Map<IncentiveDistributionModel, IncentiveDistribution>(model);

                //TODO: Use AutoMapper to map user defined model to database model
                _wdsUnit.IncentiveDistributionRepository.Add(destination);
                _wdsUnit.Commit();
                return "success";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public List<IncentiveDistribution> GetIncentiveList()
        {
            List<IncentiveDistribution> incentiveDistributionModels = _wdsUnit.IncentiveDistributionRepository.Find(i=>i.IsExpired == null || i.IsExpired == false);
            return incentiveDistributionModels;
        }

        public List<ProductMaster> GetModels()
        {
            List<ProductMaster> productMasters = _wdsUnit.ProductMasterWdsRepository.GetAll();
            return productMasters;
        }

        public List<Distributor> GetUniqDistributors()
        {
            List<Distributor> distributors = _wdsUnit.DistributorRepository.GetAll().Distinct().ToList();
            return distributors;
        }

        public string SaveTarget(TargetModel model)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return "Session Time out. Please Logout and Login again.";
                model.AddedBy = user.Id;
                model.AddedDate = DateTime.Now;

                switch (model.TargetType)
                {
                    case "CASH":
                        model.TargetUnit = "CASH";
                        break;
                    case "LIFTING":
                    case "QUANTITY":
                        model.TargetUnit = "PCS";
                        break;
                }
                //TODO: Use AutoMapper to map user defined model to database model

                var config = new MapperConfiguration(i => i.CreateMap<TargetModel, Target>());
                IMapper iMapper = config.CreateMapper();
                var destination = iMapper.Map<TargetModel, Target>(model);

                //TODO: Use AutoMapper to map user defined model to database model
                _wdsUnit.TargetRepository.Add(destination);
                _wdsUnit.Commit();
                return "success";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public List<TargetModel> GetAllTargets()
        {
            List<TargetModel> targetModels = _wdsUnit.TargetRepository.GetTargetModelList();
            return targetModels;
        }

        public List<stp_distributor_wise_sale_Result> GetDealerWiseSalesReport(DateTime stDate, DateTime edDate)
        {
            List<stp_distributor_wise_sale_Result> stpDistributorWiseSaleResults =
                _wdsUnit.DistributorRepository.GetDistributorWiseSalesReport(stDate, edDate);
            return stpDistributorWiseSaleResults;
        }

        public List<RetailerInfo> GetDeactivationListOfRetailers(User user)
        {
            var extendeRoleList = user.ExtendedRoleId != null ? user.ExtendedRoleId.Split(',').ToList() : new List<string>();
            List<long> roleId = new List<long> { (long)user.RoleId };
            roleId.AddRange(extendeRoleList.Select(r => Convert.ToInt64(r)));
            var role = _wdsUnit.RoleRepository.Find(i => roleId.Contains(i.Id)).Select(i => i.RoleName).ToList();



            List<string> zoneNameList = _wdsUnit.SalesZoneRepository.Find(i => i.EmpId == user.UserName)
                .Select(i => i.ZoneName).ToList();


            List<Distributor> distributors = _wdsUnit.DistributorRepository.Find(i => zoneNameList.Contains(i.Zone));
            List<string> distributorCodes = distributors.Select(i => i.DigitechCode).Distinct().ToList();

            List<RetailerInfo> retailerInfos = _wdsUnit.RetailerInfoRepository.Find(i =>
                (distributorCodes.Contains(i.DistributorCode)) &&
                i.IsDeletionRequested == true && (i.IsDeletionApproved == null));
            return retailerInfos;
        }

        public List<SalesRepresentative> GetPendingDeactivationListOfSr(User user)
        {
            List<long> salesZones = _wdsUnit.SalesZoneRepository.Find(i => i.EmpId == user.UserName).Select(i => i.ZoneId).ToList();
            List<Distributor> distributors = _wdsUnit.DistributorRepository.Find(i => salesZones.Contains((long)i.ZoneId));
            List<string> distributorCodes = distributors.Select(i => i.DigitechCode).Distinct().ToList();
            List<string> importCodes = distributors.Select(i => i.ImportCode).Distinct().ToList();
            List<SalesRepresentative> salesRepresentatives = _wdsUnit.SalesRepresentativeRepository.Find(i =>
                (distributorCodes.Contains(i.DealerCode) || importCodes.Contains(i.DealerCode)) &&
                i.IsDeletionRequested == true && i.IsDeletionApproved == null);
            return salesRepresentatives;
        }

        public List<stp_dealer_target_achievement_report_Result> GetDealerAchievementReport(string targetType)
        {
            List<stp_dealer_target_achievement_report_Result> stpDealerTargetAchievementReportResults =
                _wdsUnit.TargetRepository.GetDealerTargetVsAchievementReport(targetType);
            return stpDealerTargetAchievementReportResults;
        }


        public List<stp_retailer_date_wise_incentive_Result> GetRetailerIncentiveDataByDate(string fromDate, string toDate, long dType)
        {
            List<stp_retailer_date_wise_incentive_Result> datalist = _wdsUnit.RetailerInfoRepository.GetRetailerIncentiveDataByDate(fromDate,toDate, dType);
            return datalist;
        }
        public List<stp_sr_date_wise_incentive_Result> GetSRIncentiveDataByDate(string fromDate, string toDate, long dType)
        {
            List<stp_sr_date_wise_incentive_Result> datalist = _wdsUnit.SalesRepresentativeRepository.GetSRIncentiveDataByDate(fromDate, toDate, dType);
            return datalist;
        }

        public List<RetailerInfo> GetPendingAllRetailer()
        {

            var retailerList = new List<RetailerInfo>();
            var pendingRetailer = _wdsUnit.RetailerInfoRepository.
                Find(i =>
                    (i.IsCreationApproved == null || i.IsCreationApproved == false)
                    && (i.IsActive == false)).ToList();

            retailerList.AddRange(pendingRetailer);

            return retailerList;
        }


        public List<stp_sr_sales_incentive_Result> GetSRIncentiveAdminDataByDate(string fromDate, string toDate, string dCode, long srId, string model)
        {
            List<stp_sr_sales_incentive_Result> datalist = _wdsUnit.SalesRepresentativeRepository.GetSRIncentiveAdminDataByDate(fromDate, toDate, dCode, srId,model);
            return datalist;
        }

        public string SaveLimitExtension(ExtendedLimitModel model)
        {
            try
            {
                var check = _wdsUnit.ExtendedLimitRepository.Find(i =>
                    i.DistributorId == model.DistributorId && i.RetailerId == model.RetailerId &&
                    i.EndDate >= model.EndDate && i.LimitType == model.LimitType);
                if (check.Any())
                    return "You have already set and extended limit for this dealer's retailer for current month.";
                ExtendedLimit extendedLimit = new ExtendedLimit
                {
                    AddedDate = model.AddedDate,
                    AddedBy = model.AddedBy,
                    DistributorId = model.DistributorId,
                    EndDate = model.EndDate,
                    LimitType = model.LimitType,
                    NewLimit = model.NewLimit,
                    Remarks = model.Remarks,
                    RetailerId = model.RetailerId
                };

                _wdsUnit.ExtendedLimitRepository.Add(extendedLimit);
                _wdsUnit.Commit();
                return "success";

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    if (e.InnerException.InnerException != null)
                        return e.Message + " " + e.InnerException.InnerException.Message;
                return e.Message;
            }

        }

        public string RecommendRetailerUpdate(long rid, User user)
        {
            try
            {
                RetailerUpdateLog log = _wdsUnit.RetailerUpdateLogRepository.Get(rid);
                RetailerInfo retailer = _wdsUnit.RetailerInfoRepository.Get(log.RetailerId);
                retailer.RetailerAddress = log.NewAddress;
                retailer.PhoneNumber = log.NewPhoneNumber;
                retailer.PaymentNumberType = log.NewPaymentType;
                retailer.PaymentNumber = log.NewPaymentNumber;


                log.RecommendBy = user.Id;
                log.RecommendDate = DateTime.Now;
                log.ApprovalDate = DateTime.Now;
                log.ApproveBy = user.Id;


                _wdsUnit.RetailerInfoRepository.Update(retailer);
                _wdsUnit.RetailerUpdateLogRepository.Update(log);
                _wdsUnit.Commit();
                return "success";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    if (e.InnerException.InnerException != null)
                        return e.Message + " " + e.InnerException.InnerException.Message;

                return e.Message;
            }
        }

        public List<RetailerUpdateModel> GetRetailerUpdateModel(User user)
        {
            var extendeRoleList = user.ExtendedRoleId != null ? user.ExtendedRoleId.Split(',').ToList():new List<string>();
            List<long> roleId = new List<long> {(long) user.RoleId};
            roleId.AddRange(extendeRoleList.Select(r => Convert.ToInt64(r)));
            var role = _wdsUnit.RoleRepository.Find(i=> roleId.Contains(i.Id)).Select(i=>i.RoleName).ToList();
            if (role.Contains("RSM"))
            {
                List<RetailerUpdateModel> models = _wdsUnit.RetailerUpdateLogRepository.GetPendingApproval(user);
                return models;
            }
            
            return new List<RetailerUpdateModel>();
        }


        public string SaveDistributor(vmDistributors model)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return "Session Time out. Please Logout and Login again.";

                //TODO: Check distributor is already exist 
                var isDistributorExist = _wdsUnit.DistributorRepository.Find(i => i.DigitechCode == model.DigitechCode)
                    .Any();
                if (isDistributorExist) return "Distributor is already exist. Please check carefully.";

                //TODO: Use AutoMapper to map user defined model to database model

                var config = new MapperConfiguration(i => i.CreateMap<vmDistributors, Distributor>());
                IMapper iMapper = config.CreateMapper();
                var destination = iMapper.Map<vmDistributors, Distributor>(model);

                //TODO: Use AutoMapper to map user defined model to database model
                _wdsUnit.DistributorRepository.Add(destination);
                _wdsUnit.Commit();
                return "success";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }



        public string AvailabilityBroadcastSave(VmAvailabilityBroadcast model)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return "Session Time out. Please Logout and Login again.";
                model.CreatedBy = user.Id;
                model.AddedDate = DateTime.Now;
                //TODO: Use AutoMapper to map user defined model to database model

                var config = new MapperConfiguration(i => i.CreateMap<VmAvailabilityBroadcast, AvailabilityBroadcast>());
                IMapper iMapper = config.CreateMapper();
                var availabilityBroadcast = iMapper.Map<VmAvailabilityBroadcast, AvailabilityBroadcast>(model);

                //TODO: Use AutoMapper to map user defined model to database model
                _wdsUnit.AvailabilityBroadcastRepository.Add(availabilityBroadcast);
                _wdsUnit.Commit();
                var saleTeamData = _wdsUnit.SalesTeamRepository.GetAll();
                foreach (var item in saleTeamData)
                {
                    string checkValidPh = CheckIsPhoneNumber(item.MobileNo);
                    string messageConcate = model.Remarks + "." + "\n" + "Available Models Are:" + "\n" + model.ModelName;
                    if (checkValidPh != "NAN")
                    {
                        string smsSendId = SendSmsViaRobi(checkValidPh, messageConcate);
                        if (smsSendId != null)
                        {
                            var logModel = new SendSmsLog
                            {
                                MessageID = smsSendId,
                                AvailabilityBroadcastID = availabilityBroadcast.Id,
                                MessageBody = messageConcate,
                                ReceiverPhone = item.MobileNo,
                                SendDate = DateTime.Now,
                                AddedBy = user.Id.ToString(),
                                IsSend = true,
                            };

                            _wdsUnit.SendSmsLogRepository.Add(logModel);
                            _wdsUnit.Commit();
                        }
                    }
                }
                //string messageConcate = model.Remarks + "." + "\n" + "Available Models Are:" + "\n" + model.ModelName;
                //string smsSendId = SendSmsViaRobi("01840790242", messageConcate);
                //if (smsSendId != null)
                //{
                //    var logModel = new SendSmsLog
                //    {
                //        MessageID = smsSendId,
                //        AvailabilityBroadcastID = availabilityBroadcast.Id,
                //        MessageBody = messageConcate,
                //        ReceiverPhone = "01840790242",
                //        SendDate = DateTime.Now,
                //        AddedBy = user.Id.ToString(),
                //        IsSend = true,
                //    };

                //    _wdsUnit.SendSmsLogRepository.Add(logModel);
                //    _wdsUnit.Commit();
                //}
               
             
                return "success";
            }
            catch (Exception exception)
            {

                return exception.Message;
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public string SendSmsViaRobi(string receiver, string message)
        {
            try
            {
                var apiUrl = string.Format(
                    @"https://api.mobireach.com.bd/SendTextMessage?Username=waltonhitech&Password=Walton@1234&From=8801686690009&To={0}&Message={1}", receiver, message);
                var request = (HttpWebRequest)WebRequest.Create(apiUrl);
                using (var response = (HttpWebResponse)request.GetResponse())
                {

                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream == null) return null;
                        using (var reader = new StreamReader(stream))
                        {
                            string html = reader.ReadToEnd();
                            ServiceClass serviceClass = GetXmlSerializeData(html);
                            if (serviceClass.ErrorCode == 0)
                            {
                                return serviceClass.MessageId.ToString(CultureInfo.InvariantCulture);
                            }
                            
                            return null;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Sms sending exception from robi api" + receiver + "Err Msg: " + exception.Message + "\n");

                var logModel = new SendSmsLog
                {
                    MessageID = "0",
                    MessageBody = message,
                    ExceptionMessage = exception.Message,
                    ReceiverPhone = receiver,
                    SendDate = DateTime.Now,
                    AddedBy = "",
                    IsSend = false,
                };

                _wdsUnit.SendSmsLogRepository.Add(logModel);
                _wdsUnit.Commit();

                return null;
            }
        }

        public static string CheckIsPhoneNumber(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return "NAN";
            var numberArray = str.Split(',');

            var num = new String(numberArray[0].Where(Char.IsDigit).ToArray());

            var len = num.Length;
            if (len < 10 || len > 13 || len == 12) return "NAN";
            if (len == 13)
            {
                string firstTow = num.Substring(0, 2);
                if (firstTow == "88")
                {
                    string firstFive = num.Substring(0, 5);
                    if (firstFive == "88016" || firstFive == "88015" || firstFive == "88017" || firstFive == "88018" ||
                        firstFive == "88019" || firstFive == "88011" || firstFive == "88013" || firstFive == "88014")
                    {
                        return num.Substring(2, 11);
                    }
                    return "NAN";
                }
                return "NAN";

            }
            if (len == 10)
            {
                var fristTwo = num.Substring(0, 2);
                if (fristTwo == "16" || fristTwo == "15" || fristTwo == "17" || fristTwo == "18" ||
                    fristTwo == "19" || fristTwo == "11" || fristTwo == "13" || fristTwo == "14")
                {
                    return "0" + num;
                }
                return "NAN";
            }
            if (len == 11)
            {
                var firstThree = num.Substring(0, 3);
                if (firstThree == "016" || firstThree == "015" || firstThree == "017" || firstThree == "018" ||
                    firstThree == "019" || firstThree == "011" || firstThree == "013" || firstThree == "014")
                {
                    return num;
                }
                return "NAN";
            }
            return "NAN";
        }
        public static ServiceClass GetXmlSerializeData(string xmlOuput)
        {
            var serializer = new XmlSerializer(typeof(ArrayOfServiceClass));
            using (TextReader reader = new StringReader(xmlOuput))
            {
                var result = (ArrayOfServiceClass)serializer.Deserialize(reader);
                ServiceClass serviceClass = result.ServiceClasses[0];
                return serviceClass;
            }
        }


        public List<VmAvailabilityBroadcast> GetAllBrodcastList()
        {
            List<AvailabilityBroadcast> dataList = _wdsUnit.AvailabilityBroadcastRepository.GetAll();
            var availabilityBroadcastList = dataList.Select(x => new VmAvailabilityBroadcast
            {
                Id = x.Id,
                CreatedBy=x.CreatedBy,
                ModelName=x.ModelName,
                Remarks =x.Remarks,
                AddedDate =x.AddedDate
                
            }).ToList();

            return availabilityBroadcastList;
        }

        public IncentiveDistributionModel GetIncentiveById(long id)
        {
            IncentiveDistribution dbModel = _wdsUnit.IncentiveDistributionRepository.Get(id);

            //TODO: Use AutoMapper to map user defined model to database model

            var config = new MapperConfiguration(i => i.CreateMap<IncentiveDistribution, IncentiveDistributionModel>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<IncentiveDistribution, IncentiveDistributionModel>(dbModel);

            //TODO: Use AutoMapper to map user defined model to database model

            return destination;
        }

        public long UpdateIncentive(IncentiveDistributionModel model)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                IncentiveDistribution dbModel = _wdsUnit.IncentiveDistributionRepository.Get(model.Id);
                var incentiveDistribution = new IncentiveDistribution
                {
                    Title = model.Title,
                    ModelName = model.ModelName,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    TotalIncentiveAmount = model.TotalIncentiveAmount,
                    DealerAmount = model.DealerAmount,
                    RetailerAmount = model.RetailerAmount,
                    SRAmount = model.SRAmount,
                    AddedBy = user.Id,
                    AddedDate = DateTime.Now
                    , Remarks = model.Remarks
                };
                _wdsUnit.IncentiveDistributionRepository.Add(incentiveDistribution);

                dbModel.EndDate = model.StartDate.Date.AddMinutes(-1);
                dbModel.IsExpired = true;
                dbModel.UpdatedBy = user.Id;
                dbModel.UpdatedDate = DateTime.Now;
                _wdsUnit.IncentiveDistributionRepository.Update(dbModel);


                _wdsUnit.Commit();
                return incentiveDistribution.Id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}