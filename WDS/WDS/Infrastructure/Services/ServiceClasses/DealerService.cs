using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    public class DealerService : IDealerService
    {
        //private readonly RbsynergyUnit _rbsynergyUnit;
        private readonly Unit _wdsUnit;

        public DealerService(Unit wdsUnit)
        {
            //_rbsynergyUnit = rbsynergyUnit;
            _wdsUnit = wdsUnit;
        }

        public List<SelectListItem> GetDealerSelectList()
        {
            List<Distributor> distributors = _wdsUnit.DistributorRepository.GetAll();
            var dealerSelectList = distributors.Select(i => new SelectListItem
            {
                Value = i.DigitechCode + "~" + i.Zone + "~" + i.DistributorNameCellCom,
                Text = i.DistributorNameCellCom + "- (" + i.Zone + ")"
            }).ToList();
            return dealerSelectList;
        }

        public List<SelectListItem> GetDivisionSelectList()
        {
            List<Division> divisions = _wdsUnit.DivisionRepository.GetAll().OrderBy(x => x.DivisionName).ToList();
            var divisionSelectList = divisions.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.DivisionName
            }).ToList();
            return divisionSelectList;
        }
        public List<SelectListItem> GetDistrictSelectListBydivisionId(long divisionId)
        {
            List<District> district = _wdsUnit.DistrictsRepository.Find(x => x.DivisionID == divisionId);
            var districtSelectList = district.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.DistrictName
            }).ToList();
            return districtSelectList;
        }

        public List<SelectListItem> GetThanaSelectListByDistrictId(long districtId)
        {
            List<ThanaList> thanaLsit = _wdsUnit.ThanaRepository.Find(x => x.DistrictId == districtId);
            var thanaSelectList = thanaLsit.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.ThanaName
            }).ToList();
            return thanaSelectList;
        }

        public List<SelectListItem> GetTopSellingBrandSelectList()
        {
            List<Brand> brands = _wdsUnit.BrandRepository.GetAll().OrderBy(x => x.BrandName).ToList();
            var brandSelectList = brands.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.BrandName
            }).ToList();
            return brandSelectList;
        }

        public Brand GetABrandbyId(long? id)
        {
            Brand brands = _wdsUnit.BrandRepository.Find(x => x.Id == id).FirstOrDefault();

            return brands;
        }


        public List<SelectListItem> GetDistrictSelectList()
        {
            List<District> district = _wdsUnit.DistrictsRepository.GetAll().OrderBy(x => x.DistrictName).ToList(); ;
            var districtSelectList = district.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.DistrictName
            }).ToList();
            return districtSelectList;
        }

        public List<SelectListItem> GetThanaSelectList()
        {
            List<ThanaList> thanaLsit = _wdsUnit.ThanaRepository.GetAll().OrderBy(x => x.ThanaName).ToList(); ;
            var thanaSelectList = thanaLsit.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.ThanaName
            }).ToList();
            return thanaSelectList;
        }

        public string SaveRetailer(RetailerInfoModel model)
        {
            try
            {

                bool isExist = _wdsUnit.RetailerInfoRepository.Find(i => i.PhoneNumber == model.PhoneNumber)
                    .Any();
                if (!isExist)
                {
                    var user = HttpContext.Current.Session["user"] as User;
                    if (user == null) return "Your Session time out. Please Log in again";
                    var dealerArray = model.DistributorName.Split('~');
                    if (dealerArray.Any() && dealerArray.Length >= 3)
                    {
                        model.DistributorCode = dealerArray[0];
                        model.District = dealerArray[1];
                        model.DistributorName = dealerArray[2];
                    }

                    if (model.DistributorCode != null)
                    {
                        Distributor distributor = _wdsUnit.DistributorRepository.Find(i =>
                                i.DigitechCode == model.DistributorCode || i.ImportCode == model.DistributorCode)
                            .FirstOrDefault();
                        if (distributor != null)
                        {
                            model.DistributorCode = distributor.DigitechCode;
                            model.DistributorCode2 = distributor.ImportCode;

                            //zone division,district,thana id and name anaar joono use kora hoise

                            model.Zone = distributor.Zone;
                            Zone zDetails = _wdsUnit.ZoneRepository.Find(x => x.ZoneName == model.Zone).FirstOrDefault();
                            model.ZoneId = Convert.ToInt32(zDetails.Id);
                            Division dDetails = _wdsUnit.DivisionRepository.Find(x => x.Id == model.DivisionId).FirstOrDefault();
                            model.Division = dDetails.DivisionName;
                            District dicDetails = _wdsUnit.DistrictsRepository.Find(x => x.Id == model.DistrictId).FirstOrDefault();
                            model.District = dicDetails.DistrictName;
                            ThanaList thDetails = _wdsUnit.ThanaRepository.Find(x => x.Id == model.ThanaId).FirstOrDefault();
                            model.ThanaName = thDetails.ThanaName;
                            //End

                        }
                    }

                    model.IsActive = false;
                    model.IsCreationApproved = false;

                    model.AddedDate = DateTime.Now;
                    model.AddedBy = user.Id;

                    //TODO: Use AutoMapper to map user defined model to database model

                    var config = new MapperConfiguration(i => i.CreateMap<RetailerInfoModel, RetailerInfo>());
                    IMapper iMapper = config.CreateMapper();
                    var destination = iMapper.Map<RetailerInfoModel, RetailerInfo>(model);

                    //TODO: Use AutoMapper to map user defined model to database model


                    _wdsUnit.RetailerInfoRepository.Add(destination);
                    _wdsUnit.Commit();
                    return "success";
                }
                else
                {
                    return "Retailer Phone Number can not be Duplicate.";
                }
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                {
                    if (exception.InnerException.InnerException != null && exception.InnerException.InnerException.Message.Contains("UNIQUE KEY"))
                    {
                        return "Retailer Phone Number can not be Duplicate.";
                    }

                    if (exception.InnerException.InnerException != null)
                    {
                        var r = exception.Message + " -- " + exception.InnerException.InnerException.Message;
                        return r;
                    }
                }
                return exception.Message;
            }
        }

        public List<RetailerInfo> GetRetailers()
        {
            List<RetailerInfo> retailerInfos = _wdsUnit.RetailerInfoRepository.Find(i => i.IsActive == true);
            return retailerInfos;
        }

        public RetailerInfo GetRetailerById(long id)
        {
            RetailerInfo retailer = _wdsUnit.RetailerInfoRepository.Get(id);
            return retailer;
        }

        public bool EditRetailer(RetailerInfo model)
        {
            try
            {

                _wdsUnit.RetailerInfoRepository.Update(model);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeactivateRetailer(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                RetailerInfo retailerInfo = _wdsUnit.RetailerInfoRepository.Get(id);
                retailerInfo.IsDeletionRequested = true;
                retailerInfo.DeletionRequestedDate = DateTime.Now;
                retailerInfo.DeletionRequestedBy = user.Id;
                _wdsUnit.RetailerInfoRepository.Update(retailerInfo);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<VmRetailerOrders> GetRetailerOrderList(DateTime startDate, DateTime endDate)
        {
            User user = HttpContext.Current.Session["user"] as User;
            //tblDealerInfo info =
            //    _rbsynergyUnit.DealerInfoRepository.Find(i => i.DealerPhoneNumber == user.UserName).FirstOrDefault();


            Distributor distributor =
                _wdsUnit.DistributorRepository.Find(i => i.MobileNo == user.UserName).FirstOrDefault();


            List<RetailerOrder> orders = _wdsUnit.RetailerOrderRepository.Find(i => i.DealerCode == distributor.DigitechCode && i.OrderDate>=startDate && i.OrderDate<= endDate);
            List<RetailerInfo> retailerInfos = _wdsUnit.RetailerInfoRepository.GetAll();

            var retailerOrderList = (from retailerOrder in orders
                                     join retailerInfo in retailerInfos on retailerOrder.RetailerId equals retailerInfo.Id
                                     let orderDate = retailerOrder.OrderDate
                                     //where orderDate != null && orderDate >= startDate && orderDate <= endDate
                                     select new VmRetailerOrders
                                     {
                                         RetailerId = retailerInfo.Id,
                                         RetailerName = retailerInfo.RetailerName,
                                         OrderDate = (DateTime)orderDate,
                                         OrderId = retailerOrder.Id,
                                         IsCompleted = retailerOrder.IsCompleted != null && (bool)retailerOrder.IsCompleted,
                                         AddedDate = Convert.ToDateTime(retailerOrder.AddedDate),
                                         RetailerAddress = retailerInfo.RetailerAddress,
                                         RetailerPhone = retailerInfo.PhoneNumber
                                     }).ToList();




            return retailerOrderList;
        }

        public VmImeiDelivery GetOrderById(long orderId)
        {
            RetailerOrder retailerOrder = _wdsUnit.RetailerOrderRepository.Get(orderId);
            List<RetailerOrderDetail> retailerOrderDetails =
                _wdsUnit.RetailerOrderDetailRepository.Find(i => i.OrderId == orderId);
            RetailerInfo retailerInfo =
                _wdsUnit.RetailerInfoRepository.Find(i => i.Id == retailerOrder.RetailerId).FirstOrDefault();
            //tblDealerInfo dealerInfo =
            //    _rbsynergyUnit.DealerInfoRepository.Find(i => i.DealerCode == retailerOrder.DealerCode).FirstOrDefault();
            Distributor dealerInfo = _wdsUnit.DistributorRepository.Find(i =>
                    i.DigitechCode == retailerOrder.DealerCode || i.ImportCode == retailerOrder.DealerCode)
                .FirstOrDefault();

            var delivery = new VmImeiDelivery
            {
                OrderId = retailerOrder.Id,
                RetailerId = retailerInfo.Id,
                RetailerName = retailerInfo.RetailerName,
                DealerCode = dealerInfo.DigitechCode,
                DealerName = dealerInfo.DistributorNameCellCom,
                GrandTotal = (decimal)retailerOrder.TotalPrice,
                OrderDate = (DateTime)retailerOrder.OrderDate,
                RetailerPhone = retailerInfo.PhoneNumber,
                RetailerOrderDetails = retailerOrderDetails
            };

            return delivery;

        }

        public DealerDistribution GetImeiInformation(string imei)
        {

            User user = HttpContext.Current.Session["user"] as User;
            //tblDealerInfo info = _rbsynergyUnit.DealerInfoRepository.Find(i => i.DealerPhoneNumber == user.UserName).FirstOrDefault();

            Distributor distributor =
                _wdsUnit.DistributorRepository.Find(i => i.MobileNo == user.UserName).FirstOrDefault();

            if (distributor == null) return null;

            //var detail1 =
            //    _wdsUnit.DealerDistributionRepository.Find(
            //        i =>
            //            (i.ImeiOne == imei || i.ImeiTwo == imei)
            //            && (i.IsDistributed == false || i.IsDistributed == null)
            //            && (i.DealerCode == distributor.DigitechCode || i.DealerCode == distributor.ImportCode)
            //            );
            DealerDistribution detail =
                _wdsUnit.DealerDistributionRepository.Find(
                    i =>
                        (i.ImeiOne == imei || i.ImeiTwo == imei)
                //&& (i.IsDistributed == false || i.IsDistributed == null) 
                //&& (i.DealerCode == distributor.DigitechCode || i.DealerCode == distributor.ImportCode)
                        )
                    .FirstOrDefault();
            return detail;
        }

        public bool SaveOrderDelivery(VmImeiDelivery vmImeiDelivery)
        {

            RetailerOrder order = _wdsUnit.RetailerOrderRepository.Get(vmImeiDelivery.OrderId);

            order.IsCompleted = true;
            order.UpdatedDate = DateTime.Now;
            order.UpdatedBy = 1;
            _wdsUnit.RetailerOrderRepository.Update(order);

            foreach (var detail in vmImeiDelivery.RetailerOrderDetails)
            {
                int deliveryQuantity = vmImeiDelivery.ImeiDeliveryModels.Count(deliveryModel => detail.Model == deliveryModel.Model);

                var dbOrderDetail = _wdsUnit.RetailerOrderDetailRepository.Get(detail.Id);


                dbOrderDetail.GivenQuantity = deliveryQuantity;
                dbOrderDetail.UpdatedBy = 1;
                dbOrderDetail.UpdatedDate = DateTime.Now;
                _wdsUnit.RetailerOrderDetailRepository.Update(dbOrderDetail);
            }

            var rDistributions = vmImeiDelivery.ImeiDeliveryModels.Select(model => new RetailerDistribution
            {
                OrderId = vmImeiDelivery.OrderId,
                RetailerId = vmImeiDelivery.RetailerId,
                DistributionDate = DateTime.Now,
                ImeiOne = model.ImeiOne,
                Model = model.Model,
                ImeiTwo = model.ImeiTwo,
                AddedDate = DateTime.Now,
                IsReturn = false,
            }).ToList();
            _wdsUnit.RetailerDistributionRepository.AddRange(rDistributions);

            foreach (var model in vmImeiDelivery.ImeiDeliveryModels)
            {
                ImeiDeliveryModel model1 = model;
                var dd = _wdsUnit.DealerDistributionRepository.Find(i => i.ImeiOne == model1.ImeiOne).FirstOrDefault();
                if (dd != null)
                {
                    dd.IsDistributed = true;
                    dd.DistributionDate = DateTime.Now;

                    _wdsUnit.DealerDistributionRepository.Update(dd);
                }
            }

            _wdsUnit.Commit();
            return true;
        }

        //public tblDealerInfo GetDealerByPhoneNumber(string userName)
        //{
        //    tblDealerInfo dealer = new tblDealerInfo();
        //        //_rbsynergyUnit.DealerInfoRepository.Find(i => i.DealerPhoneNumber == userName).FirstOrDefault();
        //    return dealer;
        //}

        public List<SalesRepresentative> GetSalesRepresentativeByDealerCode(string dealerCode1, string dealerCode2)
        {
            List<SalesRepresentative> salesRepresentatives =
                _wdsUnit.SalesRepresentativeRepository.Find(i => (i.DealerCode == dealerCode1 || i.DealerCode == dealerCode2) && i.IsActive == true);
            return salesRepresentatives;
        }

        public List<SalesRepresentative> GetSalesRepresentativeByDigitechCode(string digitechCode)
        {
            List<SalesRepresentative> salesRepresentatives = _wdsUnit.SalesRepresentativeRepository.Find(i => i.DealerCode == digitechCode && i.IsActive==true);
            return salesRepresentatives;
        }

        //public tblCellPhonePricing GetCellPhonePriceByModel(string model)
        //{
        //    //tblCellPhonePricing pricing =
        //    //    _rbsynergyUnit.CellPhonePricingRepository.Find(i => i.Model == model).FirstOrDefault();
        //    return new tblCellPhonePricing();
        //}

        public Tuple<long, string> SaveProductIssue(VmProductIssue vmProductIssue, User user)
        {
            int dayDiff = Convert.ToInt32((DateTime.Today - vmProductIssue.SaleDate).TotalDays);
            if (dayDiff <= 2)
            {
                bool isDistributionCheck = false;
                List<string> distributionImeiList = new List<string>();
                try
                {
                    //Imei Distributed Asa ki na Check Kora hoise
                    foreach (var checkItem in vmProductIssue.ImeiDeliveryModels)
                    {
                        // DealerDistribution objdealerDistribution = _wdsUnit.DealerDistributionRepository.Find(i => i.ImeiOne == checkItem.ImeiOne || i.ImeiTwo == checkItem.ImeiTwo).FirstOrDefault();
                        RetailerDistribution objRetailerDistribution =
                            _wdsUnit.RetailerDistributionRepository.Find(i => i.ImeiOne == checkItem.ImeiOne)
                                .FirstOrDefault();
                        if (objRetailerDistribution != null && objRetailerDistribution.IsReturn == false)
                        {
                            isDistributionCheck = true;
                            distributionImeiList.Add(checkItem.ImeiOne);
                        }
                    }
                    //End
                    if (isDistributionCheck == false) //jodi Check KOra na Thake  Tahoile Save Hobe
                    {
                        Distributor dealer =
                            _wdsUnit.DistributorRepository.Find(i => i.MobileNo == user.UserName).FirstOrDefault();
                        var retailerOrder = new RetailerOrder
                        {
                            RetailerId = vmProductIssue.RetailerId,
                            PaymentAmount = vmProductIssue.PaymentAmount,
                            SerialNo = Guid.NewGuid().ToString(),
                            PaymentType = vmProductIssue.PaymentType,
                            IsCompleted = true,
                            OrderDate = vmProductIssue.SaleDate,
                            TotalPrice = vmProductIssue.GrandTotal,
                            AddedDate = DateTime.Now,
                            AddedBy = user.Id,
                            SRId = vmProductIssue.SalesRepresentativeId

                        };
                        if (dealer != null)
                        {
                            retailerOrder.DealerCode = dealer.DigitechCode;
                        }


                        var details = vmProductIssue.RetailerOrderDetails.Select(detail => new RetailerOrderDetail
                        {
                            OrderId = retailerOrder.Id,
                            Model = detail.Model,
                            AddedDate = DateTime.Now,
                            AddedBy = user.Id,
                            Discount = detail.Discount,
                            DiscountType =
                                detail.DiscountType == "$" ? "CASH" : detail.DiscountType == "%" ? "PERCENT" : null,
                            OrderQuantity = detail.OrderQuantity,
                            UnitPrice = detail.UnitPrice,
                            GivenQuantity = detail.GivenQuantity,
                            OrderTotalPrice = detail.OrderTotalPrice,
                            Color = detail.Color
                        }).ToList();
                        retailerOrder.RetailerOrderDetails = details;

                        var distributions = vmProductIssue.ImeiDeliveryModels.Select(model => new RetailerDistribution
                        {
                            Model = model.Model,
                            ImeiOne = model.ImeiOne,
                            ImeiTwo = model.ImeiTwo,
                            AddedDate = DateTime.Now,
                            DistributionDate = vmProductIssue.SaleDate,
                            IsReturn = false,
                            OrderId = retailerOrder.Id,
                            RetailerId = retailerOrder.RetailerId,
                            Color = model.Color
                        }).ToList();

                        foreach (var orderDetail in retailerOrder.RetailerOrderDetails)
                        {
                            var lst =
                                distributions.Where(
                                    distribution =>
                                        distribution.Model == orderDetail.Model &&
                                        distribution.Color == orderDetail.Color).ToList();
                            foreach (var distribution in lst)
                            {
                                distribution.RetailerOrderDetailId = orderDetail.Id;
                                distribution.OrderId = orderDetail.OrderId;
                            }
                            orderDetail.RetailerDistributions = lst;
                        }


                        List<string> imeiList = distributions.Select(i => i.ImeiOne).ToList();
                        List<DealerDistribution> dealerDistributions =
                            _wdsUnit.DealerDistributionRepository.Find(i => imeiList.Contains(i.ImeiOne)).ToList();
                        foreach (var dealerDistribution in dealerDistributions)
                        {
                            dealerDistribution.IsDistributed = true;
                            dealerDistribution.DistributionDate = vmProductIssue.SaleDate;
                        }

                        try
                        {
                            _wdsUnit.RetailerOrderRepository.Add(retailerOrder);
                            _wdsUnit.Commit();

                            _wdsUnit.DealerDistributionRepository.UpdateRange(dealerDistributions);
                            _wdsUnit.Commit();
                        }
                        catch (Exception e)
                        {
                            string msg = "Error - " + e.Message;
                            if (e.InnerException != null)
                            {
                                msg = msg + "\n" + e.InnerException.Message;
                                if (e.InnerException.InnerException != null)
                                    msg = msg + "\n" + e.InnerException.InnerException.Message;
                            }
                            return new Tuple<long, string>(retailerOrder.Id, "Operation Failed!!!" + msg);
                        }





                        return new Tuple<long, string>(retailerOrder.Id, "Operation Successful");
                    }

                    string listConcate = String.Join(",", distributionImeiList.ToArray());
                    return new Tuple<long, string>(0, "THESE IMEI ARE ALREADY DISTRIBUTED: " + listConcate);
                }
                catch (Exception e)
                {
                    string msg = "Error - " + e.Message;
                    if (e.InnerException != null)
                    {
                        msg = msg + "\n" + e.InnerException.Message;
                        if (e.InnerException.InnerException != null)
                            msg = msg + "\n" + e.InnerException.InnerException.Message;
                    }
                    return new Tuple<long, string>(0, msg);
                }
            }
            else
            {
                return new Tuple<long, string>(0, "Sale date can not be selected more than 2 days before.");
            }
            

        }

        public VmProductReturn GetRetailerImeiInfo(string imei)
        {

            RetailerDistribution distribution =
                _wdsUnit.RetailerDistributionRepository.Find(i => (i.ImeiOne == imei || i.ImeiTwo == imei) && i.IsReturn == false)
                    .FirstOrDefault();
            RetailerOrder order =
                _wdsUnit.RetailerOrderRepository.Find(i => i.Id == distribution.OrderId).FirstOrDefault();
            RetailerInfo retailerInfo = _wdsUnit.RetailerInfoRepository.Get((long)distribution.RetailerId);
            //tblDealerInfo dealerInfo =
            //    _rbsynergyUnit.DealerInfoRepository.Find(i => i.DealerCode == order.DealerCode).FirstOrDefault();

            Distributor distributor = _wdsUnit.DistributorRepository
                .Find(i => i.DigitechCode == order.DealerCode || i.ImportCode == order.DealerCode).FirstOrDefault();


            var productReturn = new VmProductReturn
            {
                OrderId = (long)distribution.OrderId,
                DealerCode = order.DealerCode,
                DealerName = distributor.DistributorNameCellCom,
                RetailerId = retailerInfo.Id,
                RetailerName = retailerInfo.RetailerName,
                Model = distribution.Model,
                ImeiOne = distribution.ImeiOne,
                ImeiTwo = distribution.ImeiTwo,
                DistributionDate = (DateTime)distribution.DistributionDate

            };
            return productReturn;

        }

        public bool SaveProductReturn(List<VmProductReturn> vmProductReturns)
        {
            foreach (var productReturn in vmProductReturns)
            {
                VmProductReturn @return = productReturn;
                var retailerDistribution =
                    _wdsUnit.RetailerDistributionRepository.Find(
                        i => i.ImeiOne == @return.ImeiOne && i.ImeiTwo == @return.ImeiTwo && i.RetailerId == @return.RetailerId && i.OrderId == @return.OrderId && i.IsReturn == false).FirstOrDefault();

                if (retailerDistribution != null)
                {
                    retailerDistribution.IsReturn = true;
                    retailerDistribution.ReturnDate = DateTime.Now;
                    _wdsUnit.RetailerDistributionRepository.Update(retailerDistribution);
                }


                var dealerDist =
                    _wdsUnit.DealerDistributionRepository.Find(
                        i =>
                            i.ImeiOne == @return.ImeiOne && i.ImeiTwo == @return.ImeiTwo &&
                            i.DealerCode == @return.DealerCode).FirstOrDefault();
                if (dealerDist != null)
                {
                    dealerDist.IsDistributed = false;
                    _wdsUnit.DealerDistributionRepository.Update(dealerDist);
                }
            }
            _wdsUnit.Commit();
            return true;
        }

        public bool SaveSr(SalesRepresentativeModel model)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                model.IsActive = false;
                model.IsCreationApproved = null;
                model.AddedBy = user.Id;
                model.AddedDate = DateTime.Now;
                //TODO: Use AutoMapper to map user defined model to database model

                var config = new MapperConfiguration(i => i.CreateMap<SalesRepresentativeModel, SalesRepresentative>());
                IMapper iMapper = config.CreateMapper();
                var destination = iMapper.Map<SalesRepresentativeModel, SalesRepresentative>(model);

                //TODO: Use AutoMapper to map user defined model to database model
                _wdsUnit.SalesRepresentativeRepository.Add(destination);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SalesRepresentative> GetSalesRepresentativesByDealerCode(Distributor distributor)
        {
            List<SalesRepresentative> salesRepresentatives =
                _wdsUnit.SalesRepresentativeRepository.Find(i =>
                    i.DealerCode == distributor.DigitechCode || i.DealerCode == distributor.ImportCode);
            return salesRepresentatives;
        }

        public SalesRepresentativeModel GetSalesRepresentativeById(long id)
        {
            SalesRepresentative salesRepresentative = _wdsUnit.SalesRepresentativeRepository.Get(id);
            var config = new MapperConfiguration(i => i.CreateMap<SalesRepresentative, SalesRepresentativeModel>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<SalesRepresentative, SalesRepresentativeModel>(salesRepresentative);
            return destination;
        }

        public bool DeactivateSr(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                SalesRepresentative salesRepresentative = _wdsUnit.SalesRepresentativeRepository.Get(id);
                //salesRepresentative.IsActive = true;
                salesRepresentative.IsDeletionRequested = true;
                salesRepresentative.DeletionRequestedDate = DateTime.Now;
                salesRepresentative.DeletionRequestedBy = user.Id;
                _wdsUnit.SalesRepresentativeRepository.Update(salesRepresentative);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void UpdateSr(SalesRepresentativeModel model)
        {
            var config = new MapperConfiguration(i => i.CreateMap<SalesRepresentativeModel, SalesRepresentative>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<SalesRepresentativeModel, SalesRepresentative>(model);
            _wdsUnit.SalesRepresentativeRepository.Update(destination);
            _wdsUnit.Commit();
        }

        public List<VmDealerSalesReport> GetDealerSales(string dealerCode, DateTime startDate, DateTime endDate)
        {
            var reports = new List<VmDealerSalesReport>();
            //List<RetailerOrder> orders =
            //    _wdsUnit.RetailerOrderRepository.Find(
            //        i => i.OrderDate >= startDate && i.OrderDate <= endDate && i.DealerCode == dealerCode);
            //foreach (var order in orders)
            //{
            //    List<RetailerOrderDetail> orderDetails = _wdsUnit.RetailerOrderDetailRepository.Find(i => i.OrderId == order.Id);
            //    reports.AddRange(orderDetails.Select(detail => new VmDealerSalesReport
            //    {
            //        OrderId = order.Id,
            //        Model = detail.Model,
            //        OrderDate = (DateTime)order.OrderDate,
            //        OrderQuantity = (int)detail.GivenQuantity,
            //        RetailerName = _wdsUnit.RetailerInfoRepository.Get((long)order.RetailerId).RetailerName
            //    }));
            //}

            reports = _wdsUnit.RetailerOrderRepository.GetDistributorSalesData(dealerCode, startDate, endDate);
            return reports;
        }

        public List<DealerStockCheck_Result> GetDealerStockReport(string dealerCode, string alternateCode, string ebsCode)
        {
            List<DealerStockCheck_Result> results = _wdsUnit.DealerDistributionRepository.GetDealerStockData(dealerCode, alternateCode, ebsCode);
            return results;
        }

        public VmPrintProductIssue GetProductIssuePrintData(long oid)
        {
            VmPrintProductIssue vmPrintProductIssue = new VmPrintProductIssue();
            RetailerOrder order = _wdsUnit.RetailerOrderRepository.Get(oid);
            if (order != null)
            {
                Distributor party =
                    _wdsUnit.DistributorRepository.Find(
                        i => i.DigitechCode == order.DealerCode || i.ImportCode == order.DealerCode).FirstOrDefault();
                RetailerInfo retailer = _wdsUnit.RetailerInfoRepository.Get((long)order.RetailerId);
                SalesRepresentative representative = _wdsUnit.SalesRepresentativeRepository.Get((long)order.SRId);
                List<RetailerOrderDetail> details =
                    _wdsUnit.RetailerOrderDetailRepository.Find(i => i.OrderId == order.Id);

                vmPrintProductIssue.OrderId = order.Id;
                vmPrintProductIssue.OrderSerial = order.SerialNo;
                vmPrintProductIssue.DealerName = party.DistributorNameCellCom;
                vmPrintProductIssue.DealerCode = party.ImportCode + "/" + party.DigitechCode;
                vmPrintProductIssue.DealerPhone = party.MobileNo;
                vmPrintProductIssue.RetailerName = retailer.RetailerName;
                vmPrintProductIssue.SrName = representative.Name;
                vmPrintProductIssue.GrandTotal = (decimal)order.TotalPrice;
                vmPrintProductIssue.PaidAmount = (decimal)order.PaymentAmount;
                vmPrintProductIssue.DueAmount = vmPrintProductIssue.GrandTotal - vmPrintProductIssue.PaidAmount;
                vmPrintProductIssue.PaymentType = order.PaymentType;
                vmPrintProductIssue.OrderDate = (DateTime)order.OrderDate;
                vmPrintProductIssue.PrintDate = DateTime.Now;
                vmPrintProductIssue.RetailerOrderDetails = details;



            }
            return vmPrintProductIssue;
        }

        public VmDealerStockAdd GetRbsynergyDistribution(string imei, User user)
        {
            Distributor distributor = _wdsUnit.DistributorRepository.Find(i => i.MobileNo == user.MobileNumber)
                .FirstOrDefault();
            VmDealerStockAdd dist = _wdsUnit.DealerDistributionDetailsRepository.GetDealerDistribution(imei, distributor);
            //_rbsynergyUnit.TblDealerDistributionDetailRepository.GetRbsynergyDistibutionByImei(imei);
            return dist;
        }

        public bool CheckImeiIsExist(string imei)
        {
            bool isExist = _wdsUnit.DealerDistributionRepository.Find(i => i.ImeiOne == imei || i.ImeiTwo == imei).Any();
            return isExist;
        }

        public bool SaveStock(List<VmDealerStockAdd> vmDealerStockAddList, User user)
        {
            try
            {
                var dealerDistributions = vmDealerStockAddList.Select(stockAdd => new DealerDistribution
                {
                    DealerCode = stockAdd.DealerCode,
                    Model = stockAdd.Model,
                    ImeiOne = stockAdd.ImeiOne,
                    ImeiTwo = stockAdd.ImeiTwo,
                    Color = stockAdd.Color,
                    DistributionDate = stockAdd.DistributionDate,
                    IsDistributed = false,
                    AddedDate = DateTime.Now,
                    AddedBy = stockAdd.DealerName,
                    AddedById = user.Id
                }).ToList();

                _wdsUnit.DealerDistributionRepository.AddRange(dealerDistributions);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<stp_sr_report_Result> GetSrReport(string startDate, string endDate, long srId)
        {
            List<stp_sr_report_Result> results = _wdsUnit.SalesRepresentativeRepository.GetSrReport(startDate, endDate,
                srId);
            return results;
        }

        public List<RetailerInfo> GetRetailersByDealerId(Distributor distributor)
        {
            List<RetailerInfo> retailerInfos = _wdsUnit.RetailerInfoRepository.Find(i => i.DistributorCode == distributor.DigitechCode && (i.IsDeleted == false || i.IsDeleted == null));
            return retailerInfos;
        }

        public List<stp_retailer_stock_Result> GetRetailerStock(long retailerId, Distributor distributor)
        {
            List<stp_retailer_stock_Result> retailerStockResults =
                _wdsUnit.RetailerDistributionRepository.GetRetailerStock(retailerId, distributor.DigitechCode, distributor.ImportCode);
            return retailerStockResults;
        }

        public List<stp_sr_sales_report_Result> GetSrSalesReport(long srId, DateTime? startDate, DateTime? endDate, Distributor distributor)
        {
            List<stp_sr_sales_report_Result> srSalesReportResults = new List<stp_sr_sales_report_Result>();
            srSalesReportResults = _wdsUnit.RetailerDistributionRepository.GetSrSalesReport(srId, startDate, endDate, distributor);
            return srSalesReportResults;
        }

        public Distributor GetDistributorById(long dealerId)
        {
            Distributor distributor = _wdsUnit.DistributorRepository.Get(dealerId);
            return distributor;
        }

        public Distributor GetDistributorByPhone(string mobileNumber)
        {
            Distributor distributor = _wdsUnit.DistributorRepository.Find(i => i.MobileNo == mobileNumber).FirstOrDefault();
            return distributor;
        }

        public List<Distributor> GetAllDistributorByZone(string zoneName)
        {
            List<Distributor> distributorList =
                _wdsUnit.DistributorRepository.Find(i => i.Zone == zoneName).ToList();
            return distributorList;
        }
        public Distributor GetADistributorByZone(string zoneName)
        {
            Distributor distributor =
                _wdsUnit.DistributorRepository.Find(i => i.Zone == zoneName).FirstOrDefault();
            return distributor;
        }
        public List<ProductMaster> GetAllProductModels()
        {
            List<ProductMaster> modelList =
                  _wdsUnit.ProductMasterWdsRepository.GetAll().ToList();
            return modelList;
        }
        public bool ApproveRetailerDeactivation(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                RetailerInfo retailerInfo = _wdsUnit.RetailerInfoRepository.Get(id);
                retailerInfo.IsDeletionApproved = true;
                retailerInfo.DeletionApprovalDate = DateTime.Now;
                retailerInfo.DeletionApprovedBy = user.Id;
                retailerInfo.IsActive = false;
                retailerInfo.IsDeleted = true;
                _wdsUnit.RetailerInfoRepository.Update(retailerInfo);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RejectRetailerDeactivation(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                RetailerInfo retailerInfo = _wdsUnit.RetailerInfoRepository.Get(id);
                retailerInfo.IsDeletionApproved = false;
                retailerInfo.DeletionApprovalDate = DateTime.Now;
                retailerInfo.DeletionApprovedBy = user.Id;
                retailerInfo.IsActive = true;
                _wdsUnit.RetailerInfoRepository.Update(retailerInfo);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ApproveSrDeactivation(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                SalesRepresentative salesRepresentative = _wdsUnit.SalesRepresentativeRepository.Get(id);
                salesRepresentative.IsDeletionApproved = true;
                salesRepresentative.DeletionApprovalDate = DateTime.Now;
                salesRepresentative.DeletionApprovedBy = user.Id;
                salesRepresentative.IsActive = false;
                _wdsUnit.SalesRepresentativeRepository.Update(salesRepresentative);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RejectSrDeactivation(long id)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                if (user == null) return false;
                SalesRepresentative salesRepresentative = _wdsUnit.SalesRepresentativeRepository.Get(id);
                salesRepresentative.IsDeletionApproved = false;
                salesRepresentative.DeletionApprovalDate = DateTime.Now;
                salesRepresentative.DeletionApprovedBy = user.Id;
                salesRepresentative.IsActive = true;
                _wdsUnit.SalesRepresentativeRepository.Update(salesRepresentative);
                _wdsUnit.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ModelPrice GetModelPriceByModel(string model)
        {
            ModelPrice modelPrice = _wdsUnit.ModelPriceRepository.Find(i => i.ModelName == model).FirstOrDefault();
            return modelPrice;
        }

        public List<DealerDistribution> GetProductList(Distributor distributor, string model)
        {
            List<DealerDistribution> dealerDistributions = new List<DealerDistribution>();

            if (model != "")
            {

                dealerDistributions = _wdsUnit.DealerDistributionRepository.Find(x => x.Model == model && x.IsReceived == null)
                    .Where(i => i.DealerCode == distributor.DigitechCode || i.DealerCode == distributor.ImportCode).ToList(); ;
            }
            else
            {
                dealerDistributions = _wdsUnit.DealerDistributionRepository.Find(
               x => x.IsReceived == false || x.IsReceived == null)
               .Where(i => i.DealerCode == distributor.DigitechCode || i.DealerCode == distributor.ImportCode).ToList();
            }

            return dealerDistributions;
        }
        public bool ProductReceiveUpdate(List<DealerDistributionModel> model)
        {
            try
            {
                //var entityModel = new DealerDistribution();
                foreach (var distribution in model)
                {
                    var entityModel = new DealerDistribution
                    {
                        Id = distribution.Id,
                        DealerCode = distribution.DealerCode,
                        Model = distribution.Model,
                        Color = distribution.Color,
                        ImeiOne = distribution.ImeiOne,
                        ImeiTwo = distribution.ImeiTwo,
                        IsReceived = true,
                        ReceiveDate = DateTime.Now,
                        DistributionDate = Convert.ToDateTime(distribution.DistributionDateString),
                        IsDistributed = distribution.IsDistributed,
                        IsSoldOut = distribution.IsSoldOut,
                        AddedDate = Convert.ToDateTime(distribution.AddedDateString),
                        AddedBy = distribution.AddedBy,
                        AddedById = distribution.AddedById,
                    };

                    _wdsUnit.DealerDistributionRepository.Update(entityModel);
                    _wdsUnit.Commit();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool GetTotalLimitResult(int smartPhoneTotalLimit, int featurePhoneTotalLimit, DateTime startDate, DateTime endDate, DealerDistribution distribution, long retailerId)
        {
            ProductMaster productMaster = _wdsUnit.ProductMasterWdsRepository.Find(i => i.ProductModel == distribution.Model)
                .FirstOrDefault();
            string phoneType = string.Empty;
            if (productMaster != null)
                phoneType = productMaster.ProductyType;



            if (!string.IsNullOrWhiteSpace(phoneType))
            {
                int distributionCount = _wdsUnit.RetailerDistributionRepository.GetTotalPhoneTypeCount(phoneType, startDate, endDate, retailerId);

                var user = HttpContext.Current.Session["user"] as User;
                Distributor distributor = _wdsUnit.DistributorRepository.Find(i => i.MobileNo == user.UserName).FirstOrDefault();
                if (user == null) return false;

                if (phoneType.ToLower().Contains("smart"))
                {
                    var totalLimit = 0;
                    var extentedLimit = _wdsUnit.ExtendedLimitRepository.Find(i =>
                        i.DistributorId == distribution.DealerCode && i.RetailerId == retailerId &&
                        i.LimitType.ToLower().Equals("smart all") && i.EndDate >= endDate).FirstOrDefault();

                    if (extentedLimit != null)
                    {
                        totalLimit = smartPhoneTotalLimit + Convert.ToInt32(extentedLimit.NewLimit);
                    }
                    else
                    {
                        totalLimit = smartPhoneTotalLimit; 
                    }


                    if (distributionCount < totalLimit) return true;
                }

                if (phoneType.ToLower().Contains("feature"))
                {
                    var totalLimit = 0;
                    var extentedLimit = _wdsUnit.ExtendedLimitRepository.Find(i =>
                        i.DistributorId == distribution.DealerCode && i.RetailerId == retailerId &&
                        i.LimitType.ToLower().Equals("feature all") && i.EndDate >= endDate).FirstOrDefault();

                    if (extentedLimit != null)
                    {
                        totalLimit = featurePhoneTotalLimit + Convert.ToInt32(extentedLimit.NewLimit);
                    }
                    else
                    {
                        totalLimit = featurePhoneTotalLimit;
                    }


                    if (distributionCount < totalLimit) return true;
                }
                return false;

               
            }

            return false;
        }

        public bool GetModelLimitResult(int smartPhoneModelLimit, int featurePhoneModelLimit, DateTime startDate, DateTime endDate, DealerDistribution distribution, long retailerId)
        {
            ProductMaster productMaster = _wdsUnit.ProductMasterWdsRepository.Find(i => i.ProductModel == distribution.Model)
                .FirstOrDefault();

            int distributionCount = _wdsUnit.RetailerDistributionRepository.Find(i =>
                    i.DistributionDate >= startDate && i.DistributionDate <= endDate && i.Model == distribution.Model && i.RetailerId == retailerId)
                .Count;

            string phoneType = string.Empty;
            if (productMaster != null)
                phoneType = productMaster.ProductyType;


            if (!string.IsNullOrWhiteSpace(phoneType))
            {

                if (phoneType.ToLower().Contains("smart"))
                {
                    var totalLimit = 0;
                    var extentedLimit = _wdsUnit.ExtendedLimitRepository.Find(i =>
                        i.DistributorId == distribution.DealerCode && i.RetailerId == retailerId &&
                        i.LimitType.ToLower().Equals("smart") && i.EndDate >= endDate).FirstOrDefault();

                    if (extentedLimit != null)
                    {
                        totalLimit = smartPhoneModelLimit + Convert.ToInt32(extentedLimit.NewLimit);
                    }
                    else
                    {
                        totalLimit = smartPhoneModelLimit;
                    }


                    if (distributionCount < totalLimit) return true;
                }

                if (phoneType.ToLower().Equals("feature"))
                {
                    var totalLimit = 0;
                    var extentedLimit = _wdsUnit.ExtendedLimitRepository.Find(i =>
                        i.DistributorId == distribution.DealerCode && i.RetailerId == retailerId &&
                        i.LimitType.ToLower().Contains("feature") && i.EndDate >= endDate).FirstOrDefault();

                    if (extentedLimit != null)
                    {
                        totalLimit = featurePhoneModelLimit + Convert.ToInt32(extentedLimit.NewLimit);
                    }
                    else
                    {
                        totalLimit = featurePhoneModelLimit;
                    }


                    if (distributionCount < totalLimit) return true;
                }
                return false;
            }
            return false;
        }

        public List<Distributor> GetAllDistributors()
        {
            List<Distributor> distributors = _wdsUnit.DistributorRepository.GetAll();
            return distributors;
        }


        public List<stp_distributor_sales_and_stock_Result> GetDealerStockAndSaleData(vmDealerAndRetailerStock objvmDealerAndRetailerStock, Role role)
        {
            List<stp_distributor_sales_and_stock_Result> dealerData =
                 _wdsUnit.DealerDistributionRepository.GetDealerSaleAndStockData(objvmDealerAndRetailerStock, role);
            return dealerData;
        }



        public bool saveDistributorOrderData(DistributorOrder objDistributorOrder, List<DistributorOrderDetail> orderDetailsList, User SeassionUser)
        {
            try
            {
                objDistributorOrder.AddedBy = SeassionUser.Id;
                objDistributorOrder.AddedDate = DateTime.Now;
                objDistributorOrder.UpdatedBy = SeassionUser.Id;
                objDistributorOrder.UpdatedDate = DateTime.Now;
                objDistributorOrder.Status ="R";
                var distributorOrderSave = _wdsUnit.DistributorOrderRepository.Add(objDistributorOrder);
                _wdsUnit.Commit();
                if (distributorOrderSave.Id > 0)
                {
                    foreach (var item in orderDetailsList)
                    {
                        item.DistributorOrderId = objDistributorOrder.Id;
                        item.IsDeleted = false;
                        item.AddedBy = SeassionUser.Id;
                        item.AddedRole = SeassionUser.RoleId.ToString();
                        item.AddedDate = DateTime.Now;
                    }
                    _wdsUnit.DistributorOrderDetailRepository.AddRange(orderDetailsList);
                    _wdsUnit.Commit();
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {

                return false;
            }
        }


        public List<vmDistributorOrder> GetAllDistributorOrderList(Role role)
        {
            List<DistributorOrder> disOrderList = new List<DistributorOrder>();
            List<vmDistributorOrder> vmOrderList = new List<vmDistributorOrder>();
            if (role.RoleName.ToLower().Contains("nsm"))
            {
               disOrderList =_wdsUnit.DistributorOrderRepository.Find(i => i.Status == "R").OrderByDescending(z => z.Id).ToList();
            }
            else if (role.RoleName.ToLower().Contains("gl"))
	        {
		       disOrderList = _wdsUnit.DistributorOrderRepository.Find(i => i.Status == "A").OrderByDescending(z => z.Id).ToList();
	        }
             

            foreach (var item in disOrderList)
            {
                var userdata = _wdsUnit.UserRepository.Find(x => x.Id == item.AddedBy).FirstOrDefault();
                var data = new vmDistributorOrder
                {
                    Id = item.Id,
                    DistributorCode = item.DistributorCode,
                    TotalPrice = item.TotalPrice,
                    AddedBy = item.AddedBy,
                    AddedByName = userdata.Name,
                    AddedDate = item.AddedDate,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedDate = item.UpdatedDate
                };
                vmOrderList.Add(data);

            }

            return vmOrderList;
        }


        public List<vmDistributorOrderDetail> GetAllDistributorOrderDetailsListByOrderId(long orderId)
        {
            List<vmDistributorOrderDetail> vmOrderDetailsList = new List<vmDistributorOrderDetail>();
            List<DistributorOrderDetail> OrderdetailsList = _wdsUnit.DistributorOrderDetailRepository.Find(x => x.DistributorOrderId == orderId && x.IsDeleted==false).ToList();
            if (OrderdetailsList != null)
            {
                foreach (var item in OrderdetailsList)
                {
                    var data = new vmDistributorOrderDetail
                    {
                        Id = item.Id,
                        DistributorOrderId = item.DistributorOrderId,
                        ModelName = item.ModelName,
                        Color = item.Color,
                        UnitPrice = item.UnitPrice,
                        RequestedQuantity = item.RequestedQuantity,
                        NsmApprovedQty = item.NsmApprovedQty,
                        LogisticsApprovedQty = item.LogisticsApprovedQty,
                        AddedRole = item.AddedRole,
                        AddedBy = item.AddedBy,
                        AddedDate = item.AddedDate,
                        IsDeleted = item.IsDeleted
                    };
                    vmOrderDetailsList.Add(data);
                }
            }
            return vmOrderDetailsList;
        }



        public bool updateDistributorOrderData(DistributorOrder objDistributorOrder, List<DistributorOrderDetail> orderDetailsList, User SeassionUser)
        {
            try
            {
                Role role = _wdsUnit.RoleRepository.Find(x => x.Id == SeassionUser.RoleId).FirstOrDefault();
                DistributorOrder onjdbDistributorOrder = new DistributorOrder();
                List<DistributorOrderDetail> orderDetailsAddList = new List<DistributorOrderDetail>();
                List<DistributorOrderDetail> orderDetailsUpdateList = new List<DistributorOrderDetail>();
              


                if (role.RoleName.ToLower().Contains("gl"))
                {
                    onjdbDistributorOrder = _wdsUnit.DistributorOrderRepository.Find(x => x.Id == objDistributorOrder.Id && x.DistributorCode == objDistributorOrder.DistributorCode).FirstOrDefault();
                    onjdbDistributorOrder.TotalPrice = objDistributorOrder.TotalPrice;
                    onjdbDistributorOrder.Status = "C";
                    onjdbDistributorOrder.LogisticCompletedDate = DateTime.Now;

                }
                else if (role.RoleName.ToLower().Contains("nsm"))
                {
                    onjdbDistributorOrder = _wdsUnit.DistributorOrderRepository.Find(x => x.Id == objDistributorOrder.Id && x.DistributorCode == objDistributorOrder.DistributorCode).FirstOrDefault();
                    onjdbDistributorOrder.TotalPrice = objDistributorOrder.TotalPrice;
                    onjdbDistributorOrder.UpdatedBy = SeassionUser.Id;
                    onjdbDistributorOrder.UpdatedDate = DateTime.Now;
                    onjdbDistributorOrder.Status = "A";
                    onjdbDistributorOrder.NsmApprovedDate = DateTime.Now;
                }

                _wdsUnit.DistributorOrderRepository.Update(onjdbDistributorOrder);
                _wdsUnit.Commit();
                if (objDistributorOrder.Id > 0)
                {
                    foreach (var item in orderDetailsList)
                    {
                        if (item.Id==0)
                        {
                            var addItem = new DistributorOrderDetail
                            {
                                DistributorOrderId = objDistributorOrder.Id,
                                ModelName = item.ModelName,
                                Color = item.Color,
                                UnitPrice= item.UnitPrice,
                                RequestedQuantity= item.RequestedQuantity,
                                NsmApprovedQty= item.NsmApprovedQty,
                                LogisticsApprovedQty = item.LogisticsApprovedQty,
                                IsDeleted = item.IsDeleted,
                                AddedBy = SeassionUser.Id,
                                AddedRole = SeassionUser.RoleId.ToString(),
                                AddedDate = DateTime.Now,

                            };
                            orderDetailsAddList.Add(addItem);
                            _wdsUnit.DistributorOrderDetailRepository.AddRange(orderDetailsAddList);
                            _wdsUnit.Commit();
                            //o id er gulo add hobe 
                           // orderDetailsList.Remove(item);
                        }
                        else
                        {
                            var updateItem = new DistributorOrderDetail
                            {
                                Id = item.Id,
                                DistributorOrderId = objDistributorOrder.Id,
                                ModelName = item.ModelName,
                                Color = item.Color,
                                UnitPrice = item.UnitPrice,
                                RequestedQuantity = item.RequestedQuantity,
                                NsmApprovedQty = item.NsmApprovedQty,
                                LogisticsApprovedQty = item.LogisticsApprovedQty,
                                IsDeleted = item.IsDeleted,
                                AddedBy = SeassionUser.Id,
                                AddedRole = SeassionUser.RoleId.ToString(),
                                AddedDate = DateTime.Now,

                            };
                            orderDetailsUpdateList.Add(updateItem);
                        }
                        
                    }
                    _wdsUnit.DistributorOrderDetailRepository.UpdateRange(orderDetailsUpdateList);
                    _wdsUnit.Commit();
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {

                return false;
            }
        }

        public List<Zone> GetAllZones()
        {
            List<Zone> zones = _wdsUnit.ZoneRepository.GetAll();
            return zones;
        }

        public string SaveRetailerUpdateLog(RetailerInfo model)
        {
            try
            {
                RetailerUpdateLog log = new RetailerUpdateLog
                {
                    RetailerId = model.Id,
                    NewAddress = model.RetailerAddress,
                    NewPaymentNumber = model.PaymentNumber,
                    NewPhoneNumber = model.PhoneNumber,
                    NewPaymentType = model.PaymentNumberType,
                    RequestedBy = (long) model.UpdatedBy,
                    RequestedDate = DateTime.Now
                };

                _wdsUnit.RetailerUpdateLogRepository.Add(log);
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

        public List<SalesRepresentative> GetAllSalesRepresentative()
        {
            List<SalesRepresentative> salesRepresentatives = _wdsUnit.SalesRepresentativeRepository.Find(i => i.IsActive == true).OrderBy(i=>i.DealerCode).ToList();
            return salesRepresentatives;
        }

        public List<Distributor> GetDistributorsByZone(string zone)
        {
            List<Distributor> distributors = _wdsUnit.DistributorRepository.Find(i => i.Zone == zone);
            return distributors;
        }

        public List<SelectListItem> GetDistributorSelectListItems()
        {
            



            List<SelectListItem> items =
                _wdsUnit.DistributorRepository.Find(i => i.IsActive == true)
                    .Select(
                        i =>
                            new SelectListItem
                            {
                                Value = i.DigitechCode,
                                Text = i.DistributorNameCellCom + "-(" + i.DigitechCode + "-" + i.Zone + ")"
                            })
                    .ToList();
            return items;
        }

        public List<DistributorDoaQuantityModel> GetDistributorDoaQuantityByApi(string digitechCode, DateTime stDate, DateTime edDate)
        {
            try
            {
                var apiUrl = string.Format(@"https://wapi.waltonbd.com/webApiProduction/mobileApi/dealer_status_count.php");

                string fromDateStr = string.Format("{0:dd-MMM-yyyy}", stDate);
                string toDateStr = string.Format("{0:dd-MMM-yyyy}", edDate);


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpClient = new HttpClient(new HttpClientHandler());
                var formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("DEALER_CODE",digitechCode),
                    new KeyValuePair<string, string>("FROM_DATE",fromDateStr),
                    new KeyValuePair<string, string>("TO_DATE",toDateStr),
                    new KeyValuePair<string, string>("SERVICE_CENTER_ID",""),
                };
                HttpResponseMessage response = httpClient.PostAsync(apiUrl, new FormUrlEncodedContent(formData)).GetAwaiter().GetResult();
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var data = JsonConvert.DeserializeObject<List<DistributorDoaQuantityModel>>(content);
                if (data.Any())
                {
                    foreach (var distributorDoaQuantityModel in data)
                    {
                        if (distributorDoaQuantityModel.RECEIVED == null)
                        {
                            distributorDoaQuantityModel.RECEIVED = 0;
                        }
                        if (distributorDoaQuantityModel.PENDING == null)
                        {
                            distributorDoaQuantityModel.PENDING = 0;
                        } 
                        if (distributorDoaQuantityModel.ADJUST == null)
                        {
                            distributorDoaQuantityModel.ADJUST = 0;
                        }
                    }
                }
                return data;

            }
            catch (Exception exception)
            {
                return new List<DistributorDoaQuantityModel>();
            }
        }

        public DistributorDoaDetailViewModel GetDistributorDoaDetails(string digitechCode, DateTime stDate, DateTime edDate, string modelName, string recQty,
            string adjQty, string penQty)
        {
            
            var model = new DistributorDoaDetailViewModel();
            try
            {
                var receivedApiUrl = string.Format(@"https://wapi.waltonbd.com/webApiProduction/mobileApi/dealer_received_list.php");
                var adjustedApiUrl = string.Format(@"https://wapi.waltonbd.com/webApiProduction/mobileApi/dealer_adjust_list.php");
                var pendingApiUrl = string.Format(@"https://wapi.waltonbd.com/webApiProduction/mobileApi/dealer_pending_list.php");

                string fromDateStr = string.Format("{0:dd-MMM-yyyy}", stDate);
                string toDateStr = string.Format("{0:dd-MMM-yyyy}", edDate);


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpClient = new HttpClient(new HttpClientHandler());
                var formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("DEALER_CODE",digitechCode),
                    new KeyValuePair<string, string>("FROM_DATE",fromDateStr),
                    new KeyValuePair<string, string>("TO_DATE",toDateStr),
                    new KeyValuePair<string, string>("SERVICE_CENTER_ID",""),
                };

                HttpResponseMessage response = httpClient.PostAsync(receivedApiUrl, new FormUrlEncodedContent(formData)).GetAwaiter().GetResult();
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var receivedData = JsonConvert.DeserializeObject<List<DistributorDoaDetailModel>>(content);
                model.ReceiveList = receivedData.Where(i => i.ITEM_NAME == modelName).ToList();


                HttpResponseMessage response1 = httpClient.PostAsync(adjustedApiUrl, new FormUrlEncodedContent(formData)).GetAwaiter().GetResult();
                var content1 = response1.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var adjustedData = JsonConvert.DeserializeObject<List<DistributorDoaDetailModel>>(content1);
                model.AdjustList = adjustedData.Where(i => i.ITEM_NAME == modelName).ToList();


                HttpResponseMessage response2 = httpClient.PostAsync(pendingApiUrl, new FormUrlEncodedContent(formData)).GetAwaiter().GetResult();
                var content2 = response2.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var pendingData = JsonConvert.DeserializeObject<List<DistributorDoaDetailModel>>(content2);
                model.PendingList = pendingData.Where(i=>i.ITEM_NAME == modelName).ToList();

                
                return model;

            }
            catch (Exception exception)
            {
                return new DistributorDoaDetailViewModel();
            }
        }


        public Role GetRoleById(long? roleId)
        {
            if (roleId == null) return null;
            Role role = _wdsUnit.RoleRepository.Get((long)roleId);
            return role;
        }


        public List<SelectListItem> GetProductBrandSelectList()
        {
            List<ProductBrand> productBrand = _wdsUnit.ProductBrandRepository.GetAll().OrderBy(x => x.BrandName).ToList(); ;
            var productBrandList = productBrand.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.BrandName
            }).ToList();
            return productBrandList;
        }

        public List<SelectListItem> GetZoneSelectList()
        {
            List<Zone> zones = _wdsUnit.ZoneRepository.GetAll().OrderBy(x => x.ZoneName).ToList(); ;
            var zonesList = zones.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.ZoneName
            }).ToList();
            return zonesList;
        }

        public List<SelectListItem> GetDistributorTypeSelectList()
        {
            List<DistributorType> distributorTypes = _wdsUnit.DistributorTypeRepository.GetAll().OrderBy(x => x.DistributorTypeName).ToList(); ;
            var distributorTypesList = distributorTypes.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.DistributorTypeName
            }).ToList();
            return distributorTypesList;
        }


        public Zone GetAZoneById(long id)
        {
            if (id == null) return null;
            Zone zone = _wdsUnit.ZoneRepository.Get((long)id);
            return zone;
        }


        public List<vmDistributors> GetAllDistributorList(Role role)
        {
            throw new NotImplementedException();
        }


       
    }
}