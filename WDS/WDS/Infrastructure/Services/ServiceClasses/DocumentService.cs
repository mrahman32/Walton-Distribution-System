using System;
using System.Activities.DynamicUpdate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Helper;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Models;
using WDS.ViewModels;
using Unit = WDS.Infrastructure.Repositories.RepositoryClasses.Unit;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    public class DocumentService : IDocumentService
    {
        private readonly Unit _wdsUnit;

        public DocumentService(Unit wdsUnit)
        {
            _wdsUnit = wdsUnit;
        }

        public DistributorCheckListModel GetDistributorCheckListByDistributorCode(string dealerCode)
        {
            FileManager fileManager = new FileManager();
            var distributorCheckList = _wdsUnit.DistributorCheckListRepository.Find(i => i.DealerCode == dealerCode).FirstOrDefault();
            if (distributorCheckList != null)
            {
                var model = new DistributorCheckListModel
                {
                    Id = distributorCheckList.Id,
                    DealerCode = distributorCheckList.DealerCode,
                    IsCreationApproved = distributorCheckList.IsCreationApproved,
                    CreationApprovedBy = distributorCheckList.CreationApprovedBy,
                    CreationApprovedDate = distributorCheckList.CreationApprovedDate,
                    IsTheDealerBlocked = distributorCheckList.IsTheDealerBlocked,
                    IsTemporaryOpen = distributorCheckList.IsTemporaryOpen,
                    TempExpireDate = distributorCheckList.TempExpireDate,
                    TempOpenDate = distributorCheckList.TempOpenDate,
                    AddedBy = distributorCheckList.AddedBy,
                    AddedDate = distributorCheckList.AddedDate,
                    UpdateName = distributorCheckList.UpdateName,
                    UpdatedBy = distributorCheckList.UpdatedBy,
                    UpdatedDate = distributorCheckList.UpdatedDate,
                    BankSolvencyModel = distributorCheckList.BankSolvencies.Any() ? distributorCheckList.BankSolvencies.Select(i => new BankSolvencyModel
                    {
                        AddedDate = i.AddedDate,
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate,
                        Id = i.Id,
                        AddedBy = i.AddedBy,
                        AccountName = i.AccountName,
                        AccountNo = i.AccountNo,
                        BankId = i.BankId,
                        BranchId = i.BranchId,
                        CheckListId = i.CheckListId,
                        SolvencyDate = i.SolvencyDate
                    }).FirstOrDefault() : new BankSolvencyModel(),

                    DistributorOthersInformationModel = distributorCheckList.DistributorOthersInformations.Any() ? distributorCheckList.DistributorOthersInformations.Select(i => new DistributorOthersInformationModel
                    {
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate,
                        AddedDate = i.AddedDate,
                        Id = i.Id,
                        AddedBy = i.AddedBy,
                        CheckListId = i.CheckListId,
                        DirectorNID_No = i.DirectorNID_No,
                        DirectorPhoto = i.DirectorPhoto,
                        DistributorShopAgreementDate = i.DistributorShopAgreementDate,
                        DistributorShopAgreementEndDate = i.DistributorShopAgreementEndDate,
                        IsDirectorSealedAndSigned = i.IsDirectorSealedAndSigned,
                        IsNomineePhotoSignedByDirector = i.IsNomineePhotoSignedByDirector,
                        IsNomineePhotoSignedByNominee = i.IsNomineePhotoSignedByNominee,
                        IsShopOwnedByDistributor = i.IsShopOwnedByDistributor,
                        IsSignedByNominee = i.IsSignedByNominee,
                        IsSignedNomineeNIDByDirector = i.IsSignedNomineeNIDByDirector,
                        MomineeMICR_ChequeFile = i.NomineeMICR_ChequeFile,
                        NID_File = i.NID_File,
                        NomineeAccountNo = i.NomineeAccountNo,
                        NomineeBankBranch = i.NomineeBankBranch,
                        NomineeChequeAmount = i.NomineeChequeAmount,
                        NomineeChequeBank = i.NomineeChequeBank,
                        NomineeChequeNo = i.NomineeChequeNo,
                        NomineeMICR_Cheque = i.NomineeMICR_Cheque,
                        NomineeNID_File = i.NomineeNID_File,
                        NomineeNID_No = i.NomineeNID_No,
                        NomineePhoto = i.NomineePhoto,
                        BlankCheckWebserverUrl = fileManager.GetFile(i.NomineeMICR_ChequeFile),
                        DirectorNIDWebserverUrl = fileManager.GetFile(i.NomineeNID_File),
                        DirectorPhotoWebserverUrl = fileManager.GetFile(i.DirectorPhoto),
                        NomineeNIDWebserverUrl = fileManager.GetFile(i.NomineeNID_File),
                        NomineePhotoWebserverUrl = fileManager.GetFile(i.NomineePhoto)
                    }).FirstOrDefault() : new DistributorOthersInformationModel(),

                    MemorandumOfAgreementModel = distributorCheckList.MemorandumOfAgreements.Any() ? distributorCheckList.MemorandumOfAgreements.Select(i => new MemorandumOfAgreementModel
                    {
                        Id = i.Id,
                        CheckListId = i.CheckListId,
                        InvestmnetAmount = i.InvestmnetAmount,
                        AgreementEndDate = i.AgreementEndDate,
                        AgreementStartDate = i.AgreementStartDate,
                        //StampNo = i.StampNo,
                        //PurchaseDate = i.PurchaseDate,
                        //VendorId = i.VendorId,
                        AddedBy = i.AddedBy,
                        AddedDate = i.AddedDate,
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate,
                        MoaStapmsModels = i.MOA_Stamps.Select(j=> new MOA_StapmsModel
                        {
                            AddedBy = j.AddedBy,
                            AddedDate = j.AddedDate,
                            Id = j.Id,
                            MOAId = j.MOAId,
                            PurchaseDate = j.PurchaseDate,
                            VendorId = j.VendorId,
                            StampNo = j.StampNo,
                            UpdateName = j.UpdateName,
                            UpdatedBy = j.UpdatedBy,
                            UpdatedDate = j.UpdatedDate
                        }).ToList()
                    }).FirstOrDefault() : new MemorandumOfAgreementModel(),

                    MemorandumOfChequeModel = distributorCheckList.MemorandumOfCheques.Any() ? distributorCheckList.MemorandumOfCheques.Select(i => new MemorandumOfChequeModel
                    {
                        
                        AccountName = i.AccountName,
                        AccountNo = i.AccountNo,
                        StampNo = i.StampNo,
                        AddedBy = i.AddedBy,
                        Amount = i.Amount,
                        BankId = i.BankId,
                        BranchId = i.BranchId,
                        ChequeAttachment = i.ChequeAttachment,
                        ChequeDate = i.ChequeDate,
                        CheueNo = i.CheueNo,
                        MICR = i.MICR,
                        AddedDate = i.AddedDate,
                        CheckListId = i.CheckListId,
                        DistributorCode = i.DistributorCode,
                        Id = i.Id,
                        PurchaseDate = i.PurchaseDate,
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate,
                        VendorId = i.VendorId,
                        ChequeAttachmentWebUrl = fileManager.GetFile(i.ChequeAttachment),
                        MocChequesModels = i.MOC_Cheques.Select(j => new MOC_ChequesModel
                        {
                            AddedBy = j.AddedBy,
                            AddedDate = j.AddedDate,
                            Id = j.Id,
                            MOCId = j.MOCId,
                            UpdateName = j.UpdateName,
                            UpdatedBy = j.UpdatedBy,
                            UpdatedDate = j.UpdatedDate,
                            ChecqueAmount = j.ChecqueAmount,
                            ChequeWebUrl = fileManager.GetFile(j.ChequeFileUrl),
                            ChequeFileUrl = j.ChequeFileUrl,
                            ChequeNo = j.ChequeNo
                        }).ToList(),
                        AccountStatement = i.AccountStatement,
                        StampType = i.StampType
                    }).FirstOrDefault() : new MemorandumOfChequeModel(),

                    TaxIdentityModel = distributorCheckList.TaxIdentities.Any() ? distributorCheckList.TaxIdentities.Select(i => new TaxIdentityModel
                    {
                        AddedBy = i.AddedBy,
                        CheckListId = i.CheckListId,
                        TIN_No = i.TIN_No,
                        TaxCircle = i.TaxCircle,
                        TaxPayerName = i.TaxPayerName,
                        TaxZone = i.TaxZone,
                        AddedDate = i.AddedDate,
                        Id = i.Id,
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate
                    }).FirstOrDefault() : new TaxIdentityModel(),

                    TradeLicensModel = distributorCheckList.TradeLicenses.Any() ? distributorCheckList.TradeLicenses.Select(i => new TradeLicensModel
                    {
                        AddedBy = i.AddedBy,
                        CheckListId = i.CheckListId,
                        AddedDate = i.AddedDate,
                        Id = i.Id,
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate,
                        ExpireDate = i.ExpireDate,
                        IsAddressMachedWithOracle = i.IsAddressMachedWithOracle,
                        IsBusinessNameMachedWithOracle = i.IsBusinessNameMachedWithOracle,
                        IssueDate = i.IssueDate,
                        TradeLicenseNo = i.TradeLicenseNo
                    }).FirstOrDefault() : new TradeLicensModel(),




                    ValueAddedTaxModel = distributorCheckList.ValueAddedTaxes.Any() ? distributorCheckList.ValueAddedTaxes.Select(i => new ValueAddedTaxModel
                    {
                        AddedBy = i.AddedBy,
                        CheckListId = i.CheckListId,
                        AddedDate = i.AddedDate,
                        Id = i.Id,
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate,
                        BIN_No = i.BIN_No,
                        Name = i.Name
                    }).FirstOrDefault() : new ValueAddedTaxModel(),

                    DistributorPersonalInformationModel = distributorCheckList.DistributorPersonalInformations.Any() ? distributorCheckList.DistributorPersonalInformations.Select(i => new DistributorPersonalInformationModel
                    {
                        AddedBy = i.AddedBy,
                        AddedDate = i.AddedDate,
                        BankId = i.BankId,
                        BranchId = i.BranchId,
                        BrothersName = i.BrothersName,
                        BrothersOccupation = i.BrothersOccupation,
                        BusinessAddress = i.BusinessAddress,
                        Id = i.Id,
                        BusinessType = i.BusinessType,
                        CheckListId = i.CheckListId,
                        ContactName = i.ContactName,
                        ContactNumber = i.ContactNumber,
                        EducationDetail = i.EducationDetail,
                        DistributorDOB = i.DistributorDOB,
                        EmergencyAddress1 = i.EmergencyAddress1,
                        EmergencyAddress2 = i.EmergencyAddress2,
                        EmergencyName1 = i.EmergencyName1,
                        EmergencyName2 = i.EmergencyName2,
                        EmergencyNumber1 = i.EmergencyNumber1,
                        EmergencyNumber2 = i.EmergencyNumber2,
                        ExperienceDetail = i.ExperienceDetail,
                        FathersName = i.FathersName,
                        FathersOccupation = i.FathersOccupation,
                        FullName = i.FullName,
                        LoanAmount = i.LoanAmount,
                        MothersName = i.MothersName,
                        MothersOccupation = i.MothersOccupation,
                        Ownership = i.Ownership,
                        PartyCategory = i.PartyCategory,
                        PermanentAddress = i.PermanentAddress,
                        PresentAddress = i.PresentAddress,
                        PropertyDetail = i.PropertyDetail,
                        Reference = i.Reference,
                        RunningBusinessDetail_OthersAge = i.RunningBusinessDetail_OthersAge,
                        RunningBusinessDetail_OthersInvestment = i.RunningBusinessDetail_OthersInvestment,
                        RunningBusinessDetail_WaltonAge = i.RunningBusinessDetail_WaltonAge,
                        RunningBusinessDetail_WaltonInvestment = i.RunningBusinessDetail_WaltonInvestment,
                        ShopPictureUrl = "",
                        SistersName = i.SistersName,
                        SistersOccupation = i.SistersOccupation,
                        UpdateName = i.UpdateName,
                        UpdatedBy = i.UpdatedBy,
                        UpdatedDate = i.UpdatedDate,
                        ZoneId = i.ZoneId,
                        ZoneName = i.ZoneName,
                        ShopPictureWebServerUrl = fileManager.GetFile(i.ShopPictureUrl)

                    }).FirstOrDefault() : new DistributorPersonalInformationModel()
                };
                return model;
            }


            return null;
        }

        public DistributorCheckList CreateNewCheckList(DistributorCheckListModel model)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {
                var obj = new DistributorCheckList
                {
                    DealerCode = model.DealerCode,
                    IsCreationApproved = false,
                    AddedBy = user.Id,
                    AddedDate = DateTime.Now
                };

                _wdsUnit.DistributorCheckListRepository.Add(obj);
                _wdsUnit.Commit();

                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string CreateNewMOA(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {
                var agreement = new MemorandumOfAgreement
                {
                    CheckListId = checkList.Id,
                    InvestmnetAmount = model.MemorandumOfAgreementModel.InvestmnetAmount,
                    AgreementEndDate = model.MemorandumOfAgreementModel.AgreementEndDate,
                    AgreementStartDate = model.MemorandumOfAgreementModel.AgreementStartDate,
                    //StampNo = model.MemorandumOfAgreementModel.StampNo,
                    //PurchaseDate = model.MemorandumOfAgreementModel.PurchaseDate,
                    //VendorId = model.MemorandumOfAgreementModel.VendorId,
                    AddedDate = DateTime.Now,
                    AddedBy = user.Id
                };
                foreach (var moaStapmsModel in model.MemorandumOfAgreementModel.MoaStapmsModels)
                {
                    var s = new MOA_Stamps
                    {
                        StampNo = moaStapmsModel.StampNo,
                        PurchaseDate = moaStapmsModel.PurchaseDate,
                        VendorId =  moaStapmsModel.VendorId,
                        AddedBy = user.Id,
                        AddedDate = DateTime.Now
                    };
                    agreement.MOA_Stamps.Add(s);
                    
                }
                
                _wdsUnit.MemorandumOfAgreementRepostory.Add(agreement);
                _wdsUnit.Commit();

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public DistributorCheckList UpdateDistributorCheckList(DistributorCheckListModel model)
        {
            var user = HttpContext.Current.Session["user"] as User;
            DistributorCheckList obj = _wdsUnit.DistributorCheckListRepository.Get(model.Id);
            if (obj != null)
            {
                obj.UpdatedDate = DateTime.Now;
                obj.UpdatedBy = user.Id;
                _wdsUnit.DistributorCheckListRepository.Update(obj);
                _wdsUnit.Commit();
                return obj;
            }
            return null;
        }

        public string UpdateMoa(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            var config = new MapperConfiguration(i => i.CreateMap<MemorandumOfAgreementModel, MemorandumOfAgreement>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<MemorandumOfAgreementModel, MemorandumOfAgreement>(model.MemorandumOfAgreementModel);
            destination.UpdatedBy = user.Id;
            destination.UpdatedDate = DateTime.Now;
            //TODO: Use AutoMapper to map user defined model to database model

            foreach (var moaStapmsModel in model.MemorandumOfAgreementModel.MoaStapmsModels)
            {
                if (moaStapmsModel.Id > 0)
                {
                    //UPDATE
                    config = new MapperConfiguration(i => i.CreateMap<MOA_StapmsModel, MOA_Stamps>());
                    iMapper = config.CreateMapper();
                    var moaStamps = iMapper.Map<MOA_StapmsModel, MOA_Stamps>(moaStapmsModel);
                    //destination.MOC_Cheques.Add(mocCheque);
                    _wdsUnit.MoaStampsRepository.Update(moaStamps);
                }
                else
                {
                    //CREATE
                    config = new MapperConfiguration(i => i.CreateMap<MOA_StapmsModel, MOA_Stamps>());
                    iMapper = config.CreateMapper();
                    var stamps = iMapper.Map<MOA_StapmsModel, MOA_Stamps>(moaStapmsModel);
                    stamps.MOAId = destination.Id;
                    stamps.AddedBy = user.Id;
                    stamps.AddedDate = DateTime.Now;
                    _wdsUnit.MoaStampsRepository.Add(stamps);
                }
            }


            _wdsUnit.MemorandumOfAgreementRepostory.Update(destination);
            _wdsUnit.Commit();
            return "success";
        }

        public string CreateNewMOC(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {
                var fileManager = new FileManager();
                if (model.MemorandumOfChequeModel.AccountStatementFileBase != null)
                {
                    //model.MemorandumOfChequeModel.ChequeAttachment = fileManager.UploadImage(model.MemorandumOfChequeModel.CheckAttachmentFile);
                    model.MemorandumOfChequeModel.AccountStatement = fileManager.UploadImage(model.MemorandumOfChequeModel.AccountStatementFileBase);
                }
                if (model.MemorandumOfChequeModel.MocChequesModels.Any())
                {
                    foreach (var mocChequesModel in model.MemorandumOfChequeModel.MocChequesModels)
                    {
                        mocChequesModel.ChequeFileUrl = fileManager.UploadImage(mocChequesModel.ChequeFileBase);
                    }
                }

                var memorandumOfCheque = new MemorandumOfCheque
                {
                    CheckListId = checkList.Id,
                    StampNo = model.MemorandumOfChequeModel.StampNo,
                    PurchaseDate = model.MemorandumOfChequeModel.PurchaseDate,
                    VendorId = model.MemorandumOfChequeModel.VendorId,
                    ChequeDate = model.MemorandumOfChequeModel.ChequeDate,
                    BankId = model.MemorandumOfChequeModel.BankId,
                    BranchId = model.MemorandumOfChequeModel.BranchId,
                    AccountNo = model.MemorandumOfChequeModel.AccountNo,
                    CheueNo = model.MemorandumOfChequeModel.CheueNo,
                    Amount = model.MemorandumOfChequeModel.Amount,
                    MICR = model.MemorandumOfChequeModel.MICR,
                    ChequeAttachment = model.MemorandumOfChequeModel.ChequeAttachment,
                    AccountName = model.MemorandumOfChequeModel.AccountName,
                    AccountStatement = model.MemorandumOfChequeModel.AccountStatement,
                    AddedDate = DateTime.Now,
                    AddedBy = user.Id
                };


                foreach (var mocChequese in model.MemorandumOfChequeModel.MocChequesModels)
                {
                    var cheques = new MOC_Cheques
                    {
                        
                        AddedDate = DateTime.Now,
                        AddedBy = user.Id,
                        ChecqueAmount = mocChequese.ChecqueAmount,
                        ChequeFileUrl = mocChequese.ChequeFileUrl,
                        ChequeNo = mocChequese.ChequeNo
                    };
                    memorandumOfCheque.MOC_Cheques.Add(cheques);
                }

                

                _wdsUnit.MemorandumOfChequeRepostory.Add(memorandumOfCheque);
                _wdsUnit.Commit();

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public string UpdateMoc(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            var fileManager = new FileManager();
            if (model.MemorandumOfChequeModel.CheckAttachmentFile != null)
            {
                
                model.MemorandumOfChequeModel.ChequeAttachment = fileManager.UploadImage(model.MemorandumOfChequeModel.CheckAttachmentFile);
                model.MemorandumOfChequeModel.AccountStatement =
                    fileManager.UploadImage(model.MemorandumOfChequeModel.AccountStatementFileBase);
            }


            var config = new MapperConfiguration(i => i.CreateMap<MemorandumOfChequeModel, MemorandumOfCheque>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<MemorandumOfChequeModel, MemorandumOfCheque>(model.MemorandumOfChequeModel);
            destination.UpdatedBy = user.Id;
            destination.UpdatedDate = DateTime.Now;
            //TODO: Use AutoMapper to map user defined model to database model
            

            foreach (var mocChequesModel in model.MemorandumOfChequeModel.MocChequesModels)
            {
                if (mocChequesModel.Id > 0)
                {
                    //UPDATE
                    config = new MapperConfiguration(i=>i.CreateMap<MOC_ChequesModel, MOC_Cheques>());
                    iMapper = config.CreateMapper();
                    var mocCheque = iMapper.Map<MOC_ChequesModel, MOC_Cheques>(mocChequesModel);
                    mocCheque.ChequeFileUrl = mocChequesModel.ChequeFileBase != null ? fileManager.UploadImage(mocChequesModel.ChequeFileBase) : mocChequesModel.ChequeFileUrl;
                    //destination.MOC_Cheques.Add(mocCheque);
                    _wdsUnit.MocChequesRepository.Update(mocCheque);
                }
                else
                {
                    //CREATE
                    config = new MapperConfiguration(i => i.CreateMap<MOC_ChequesModel, MOC_Cheques>());
                    iMapper = config.CreateMapper();
                    var mocCheque = iMapper.Map<MOC_ChequesModel, MOC_Cheques>(mocChequesModel);
                    mocCheque.MOCId = destination.Id;
                    mocCheque.AddedBy = user.Id;
                    mocCheque.AddedDate = DateTime.Now;
                    if (mocChequesModel.ChequeFileBase != null)
                    {
                        mocCheque.ChequeFileUrl = fileManager.UploadImage(mocChequesModel.ChequeFileBase);
                    }
                    //destination.MOC_Cheques.Add(mocCheque);
                    _wdsUnit.MocChequesRepository.Add(mocCheque);
                }
            }
            _wdsUnit.MemorandumOfChequeRepostory.Update(destination);
            _wdsUnit.Commit();
            return "success";
        }

        public string CreateNewBS(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {

                var bankSolvency = new BankSolvency
                {
                    CheckListId = checkList.Id,
                    BankId = model.BankSolvencyModel.BankId,
                    BranchId = model.BankSolvencyModel.BranchId,
                    AccountNo = model.BankSolvencyModel.AccountNo,
                    AccountName = model.BankSolvencyModel.AccountName,
                    AddedDate = DateTime.Now,
                    AddedBy = user.Id, BusinessAddress = model.BankSolvencyModel.BusinessAddress
                };
                _wdsUnit.BankSolvencyRepository.Add(bankSolvency);
                _wdsUnit.Commit();

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public string UpdateBS(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;


            var config = new MapperConfiguration(i => i.CreateMap<BankSolvencyModel, BankSolvency>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<BankSolvencyModel, BankSolvency>(model.BankSolvencyModel);
            destination.UpdatedBy = user.Id;
            destination.UpdatedDate = DateTime.Now;
            //TODO: Use AutoMapper to map user defined model to database model
            _wdsUnit.BankSolvencyRepository.Update(destination);
            _wdsUnit.Commit();
            return "success";
        }

        public string CreateNewTIN(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {

                var taxIdentity = new TaxIdentity
                {
                    CheckListId = checkList.Id,
                    TIN_No = model.TaxIdentityModel.TIN_No,
                    TaxPayerName = model.TaxIdentityModel.TaxPayerName,
                    TaxCircle = model.TaxIdentityModel.TaxCircle,
                    TaxZone = model.TaxIdentityModel.TaxZone,

                    AddedDate = DateTime.Now,
                    AddedBy = user.Id
                };
                _wdsUnit.TaxIdentityRepostory.Add(taxIdentity);
                _wdsUnit.Commit();

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public string UpdateTIN(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;


            var config = new MapperConfiguration(i => i.CreateMap<TaxIdentityModel, TaxIdentity>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<TaxIdentityModel, TaxIdentity>(model.TaxIdentityModel);
            destination.UpdatedBy = user.Id;
            destination.UpdatedDate = DateTime.Now;
            //TODO: Use AutoMapper to map user defined model to database model
            _wdsUnit.TaxIdentityRepostory.Update(destination);
            _wdsUnit.Commit();
            return "success";
        }

        public string CreateNewVAT(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {

                var valueAddedTax = new ValueAddedTax
                {
                    CheckListId = checkList.Id,
                    BIN_No = model.ValueAddedTaxModel.BIN_No,
                    Name = model.ValueAddedTaxModel.Name,


                    AddedDate = DateTime.Now,
                    AddedBy = user.Id
                };
                _wdsUnit.ValueAddedTaxesRepository.Add(valueAddedTax);
                _wdsUnit.Commit();

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public string UpdateVAT(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;


            var config = new MapperConfiguration(i => i.CreateMap<ValueAddedTaxModel, ValueAddedTax>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<ValueAddedTaxModel, ValueAddedTax>(model.ValueAddedTaxModel);
            destination.UpdatedBy = user.Id;
            destination.UpdatedDate = DateTime.Now;
            //TODO: Use AutoMapper to map user defined model to database model
            _wdsUnit.ValueAddedTaxesRepository.Update(destination);
            _wdsUnit.Commit();
            return "success";
        }

        public string CreateNewTrade(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {

                var tradeLicens = new TradeLicens
                {
                    CheckListId = checkList.Id,
                    TradeLicenseNo = model.TradeLicensModel.TradeLicenseNo,
                    IssueDate = model.TradeLicensModel.IssueDate,
                    ExpireDate = model.TradeLicensModel.ExpireDate,
                    IsBusinessNameMachedWithOracle = model.TradeLicensModel.IsBusinessNameMachedWithOracle,
                    IsAddressMachedWithOracle = model.TradeLicensModel.IsAddressMachedWithOracle,
                    IsAddressMatchWithBankSolvencyCertificate = model.TradeLicensModel.IsAddressMatchWithBankSolvencyCertificate,

                    AddedDate = DateTime.Now,
                    AddedBy = user.Id
                };
                _wdsUnit.TradeLicensRepository.Add(tradeLicens);
                _wdsUnit.Commit();

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public string UpdateTrade(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;


            var config = new MapperConfiguration(i => i.CreateMap<TradeLicensModel, TradeLicens>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<TradeLicensModel, TradeLicens>(model.TradeLicensModel);
            destination.UpdatedBy = user.Id;
            destination.UpdatedDate = DateTime.Now;
            //TODO: Use AutoMapper to map user defined model to database model
            _wdsUnit.TradeLicensRepository.Update(destination);
            _wdsUnit.Commit();
            return "success";
        }

        public string CreateNewOther(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {
                var fileManager = new FileManager();
                if (model.DistributorOthersInformationModel.DirectorNidFile != null)
                {

                    model.DistributorOthersInformationModel.NID_File = fileManager.UploadImage(model.DistributorOthersInformationModel.DirectorNidFile);
                }

                if (model.DistributorOthersInformationModel.DirectorPhotoFile != null)
                {

                    model.DistributorOthersInformationModel.DirectorPhoto = fileManager.UploadImage(model.DistributorOthersInformationModel.DirectorPhotoFile);
                }

                if (model.DistributorOthersInformationModel.NomineeNidFile != null)
                {

                    model.DistributorOthersInformationModel.NomineeNID_File = fileManager.UploadImage(model.DistributorOthersInformationModel.NomineeNidFile);
                }


                if (model.DistributorOthersInformationModel.NominieePhotoFile != null)
                {

                    model.DistributorOthersInformationModel.NomineePhoto = fileManager.UploadImage(model.DistributorOthersInformationModel.NominieePhotoFile);
                }
                if (model.DistributorOthersInformationModel.BlankMicrChequeFile != null)
                {

                    model.DistributorOthersInformationModel.MomineeMICR_ChequeFile = fileManager.UploadImage(model.DistributorOthersInformationModel.BlankMicrChequeFile);
                }

                var distributorOthersInformation = new DistributorOthersInformation
                {
                    CheckListId = checkList.Id,
                    DirectorNID_No = model.DistributorOthersInformationModel.DirectorNID_No,
                    NID_File = model.DistributorOthersInformationModel.NID_File,
                    DirectorPhoto = model.DistributorOthersInformationModel.DirectorPhoto,
                    IsDirectorSealedAndSigned = model.DistributorOthersInformationModel.IsDirectorSealedAndSigned,
                    NomineeNID_No = model.DistributorOthersInformationModel.NomineeNID_No,
                    NomineeNID_File = model.DistributorOthersInformationModel.NomineeNID_File,
                    IsSignedByNominee = model.DistributorOthersInformationModel.IsSignedByNominee,
                    IsSignedNomineeNIDByDirector = model.DistributorOthersInformationModel.IsSignedNomineeNIDByDirector,
                    NomineeChequeBank = model.DistributorOthersInformationModel.NomineeChequeBank,
                    NomineeBankBranch = model.DistributorOthersInformationModel.NomineeBankBranch,
                    NomineeAccountNo = model.DistributorOthersInformationModel.NomineeAccountNo,
                    NomineeChequeNo = model.DistributorOthersInformationModel.NomineeChequeNo,
                    NomineeChequeAmount = model.DistributorOthersInformationModel.NomineeChequeAmount,
                    NomineeMICR_Cheque = model.DistributorOthersInformationModel.NomineeMICR_Cheque,
                    NomineeMICR_ChequeFile = model.DistributorOthersInformationModel.MomineeMICR_ChequeFile,
                    DistributorShopAgreementDate = model.DistributorOthersInformationModel.DistributorShopAgreementDate,
                    DistributorShopAgreementEndDate = model.DistributorOthersInformationModel.DistributorShopAgreementEndDate,
                    IsShopOwnedByDistributor = model.DistributorOthersInformationModel.IsShopOwnedByDistributor,
                    IsNomineePhotoSignedByDirector = model.DistributorOthersInformationModel.IsNomineePhotoSignedByDirector,
                    IsNomineePhotoSignedByNominee = model.DistributorOthersInformationModel.IsNomineePhotoSignedByNominee,
                    NomineePhoto = model.DistributorOthersInformationModel.NomineePhoto,
                    AddedDate = DateTime.Now,
                    AddedBy = user.Id
                };
                _wdsUnit.DistributorOthersInformationRepository.Add(distributorOthersInformation);
                _wdsUnit.Commit();

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public string UpdateOther(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            var fileManager = new FileManager();
            if (model.DistributorOthersInformationModel.DirectorNidFile != null)
            {

                model.DistributorOthersInformationModel.NID_File = fileManager.UploadImage(model.DistributorOthersInformationModel.DirectorNidFile);
            }

            if (model.DistributorOthersInformationModel.DirectorPhotoFile != null)
            {

                model.DistributorOthersInformationModel.DirectorPhoto = fileManager.UploadImage(model.DistributorOthersInformationModel.DirectorPhotoFile);
            }

            if (model.DistributorOthersInformationModel.NomineeNidFile != null)
            {

                model.DistributorOthersInformationModel.NomineeNID_File = fileManager.UploadImage(model.DistributorOthersInformationModel.NomineeNidFile);
            }


            if (model.DistributorOthersInformationModel.NominieePhotoFile != null)
            {

                model.DistributorOthersInformationModel.NomineePhoto = fileManager.UploadImage(model.DistributorOthersInformationModel.NominieePhotoFile);
            }
            if (model.DistributorOthersInformationModel.BlankMicrChequeFile != null)
            {

                model.DistributorOthersInformationModel.MomineeMICR_ChequeFile = fileManager.UploadImage(model.DistributorOthersInformationModel.BlankMicrChequeFile);
            }

            var config = new MapperConfiguration(i => i.CreateMap<DistributorOthersInformationModel, DistributorOthersInformation>());
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<DistributorOthersInformationModel, DistributorOthersInformation>(model.DistributorOthersInformationModel);
            destination.UpdatedBy = user.Id;
            destination.UpdatedDate = DateTime.Now;
            //TODO: Use AutoMapper to map user defined model to database model
            _wdsUnit.DistributorOthersInformationRepository.Update(destination);
            _wdsUnit.Commit();
            return "success";
        }

        public List<CheckListApprovalPendingModel> GetApprovalPendingCheckList()
        {
            List<CheckListApprovalPendingModel> models = _wdsUnit.GetCustomModelListForDistributorDocumentApprovalPending();
            return models;
        }

        public AjaxResponseModel ApproveDocuments(long id)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            var user = HttpContext.Current.Session["user"] as User;
            try
            {
                DistributorCheckList obj = _wdsUnit.DistributorCheckListRepository.Get(id);
                if (obj == null)
                {
                    model.Message = "Data not found to approve. Please contact with system administrator.";
                    model.Id = 2;
                }
                else
                {
                    obj.IsCreationApproved = true;
                    obj.CreationApprovedBy = user.Id;
                    obj.CreationApprovedDate = DateTime.Now;
                    obj.UpdatedBy = user.Id;
                    obj.UpdatedDate = DateTime.Now;
                    obj.UpdateName = user.Name;
                    _wdsUnit.DistributorCheckListRepository.Update(obj);
                    _wdsUnit.Commit();


                    model.Message = "Saved Successfully.";
                    model.Id = 1;

                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                model.Message = msg;
                model.Id = 2;
            }

            return model;
        }

        public string CreateDistributorPersonalInfo(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            var user = HttpContext.Current.Session["user"] as User;
            try
            {
                var fileManager = new FileManager();
                if (model.DistributorPersonalInformationModel.ShopPictureFile != null)
                {
                    model.DistributorPersonalInformationModel.ShopPictureUrl = fileManager.UploadImage(model.DistributorPersonalInformationModel.ShopPictureFile);
                }

                var config = new MapperConfiguration(i => i.CreateMap<DistributorPersonalInformationModel, DistributorPersonalInformation>());
                IMapper iMapper = config.CreateMapper();
                var destination = iMapper.Map<DistributorPersonalInformationModel, DistributorPersonalInformation>(model.DistributorPersonalInformationModel);
                destination.CheckListId = model.Id;
                destination.AddedBy = user.Id;
                destination.AddedDate = DateTime.Now;


                _wdsUnit.DistributorPersonalInformationRepository.Add(destination);
                _wdsUnit.Commit();
                
                

                return "Successfully Saved.";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public string UpdateDistributorPersonalInfo(DistributorCheckListModel model, DistributorCheckList checkList)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;
                var fileManager = new FileManager();
                if (model.DistributorPersonalInformationModel.ShopPictureFile != null)
                {

                    model.DistributorPersonalInformationModel.ShopPictureUrl = fileManager.UploadImage(model.DistributorPersonalInformationModel.ShopPictureFile);
                }


                var config = new MapperConfiguration(i => i.CreateMap<DistributorPersonalInformationModel, DistributorPersonalInformation>());
                IMapper iMapper = config.CreateMapper();
                var destination = iMapper.Map<DistributorPersonalInformationModel, DistributorPersonalInformation>(model.DistributorPersonalInformationModel);
                destination.UpdatedBy = user.Id;
                destination.UpdatedDate = DateTime.Now;
                //TODO: Use AutoMapper to map user defined model to database model
                _wdsUnit.DistributorPersonalInformationRepository.Update(destination);
                _wdsUnit.Commit();
                return "success";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    msg = msg + "-" + e.InnerException.InnerException.Message;
                }
                return msg;
            }
        }

        public List<CheckListApprovalPendingModel> GetVerificationPendingList()
        {
            throw new NotImplementedException();
        }

        public List<VmDistributorDocVerificationList> GetDocList(long checkListId)
        {
            List<VmDistributorDocVerificationList> lst = _wdsUnit.DistributorCheckListRepository.GetDocuments(checkListId);
           
            return null;
        }

        public List<VarificationEntityModel> GetDocumentLst(long checkListId)
        {
            List<VarificationEntity> varificationEntities = _wdsUnit.VarificationEntityRepository.GetAll();
            var models = new List<VarificationEntityModel>();


            var config = new MapperConfiguration(i => i.CreateMap<VarificationEntity, VarificationEntityModel>());
            IMapper iMapper = config.CreateMapper();
            var destinations = iMapper.Map<List<VarificationEntity>, List<VarificationEntityModel>>(varificationEntities);



            foreach (var varificationEntity in destinations)
            {
                varificationEntity.CheckListId = checkListId;
                VarificationEntityModel entity = varificationEntity;
                long entityId = entity.Id;
                var approvalEntity = _wdsUnit.VarificationApprovalRepostitory.Find(
                        i => (i.CheckListId == checkListId && i.VarificationEntityId == entityId))
                        .FirstOrDefault();

                if (approvalEntity == null) continue;
                if (approvalEntity.IsApproved == true && (approvalEntity.IsDeclined == null || approvalEntity.IsDeclined == false))
                {
                    varificationEntity.IsApproved = "Approve";
                    varificationEntity.Remarks = approvalEntity.ApprovalRemarks;
                }
                else if ((approvalEntity.IsApproved == null || approvalEntity.IsApproved == false) &&
                         approvalEntity.IsDeclined == true)
                {
                    varificationEntity.IsApproved = "Decline";
                    varificationEntity.Remarks = approvalEntity.DeclinedRemarks;
                }
            }
            return destinations;
        }

        public string SaveInvestigationResult(long id, long checkListId, string description, string remarks, string decision)
        {
            try
            {
                var user = HttpContext.Current.Session["user"] as User;

                var dbObject =
                    _wdsUnit.VarificationApprovalRepostitory.Find(
                        i => i.CheckListId == checkListId && i.VarificationEntityId == id).FirstOrDefault();

                if (dbObject != null)
                {
                    switch (decision)
                    {
                        case "Approve":
                            dbObject.UpdatedBy = user.Id;
                            dbObject.UpdatedDate = DateTime.Now;
                            dbObject.IsApproved = true;
                            dbObject.IsDeclined = false;
                            dbObject.ApprovalDate = DateTime.Now;
                            dbObject.ApprovedBy = user.Id;
                            dbObject.ApprovalRemarks = remarks;
                            break;
                        case "Decline":
                            dbObject.UpdatedBy = user.Id;
                            dbObject.UpdatedDate = DateTime.Now;
                            dbObject.IsApproved = false;
                            dbObject.IsDeclined = true;
                            dbObject.DeclinedDate = DateTime.Now;
                            dbObject.DeclinedBy = user.Id;
                            dbObject.DeclinedRemarks = remarks;
                            break;
                    }

                    _wdsUnit.VarificationApprovalRepostitory.Update(dbObject);
                    _wdsUnit.Commit();

                }
                else
                {
                    var approval = new VarificationApproval
                    {
                        CheckListId = checkListId,
                        VarificationEntityId = id,
                        VarificationEntityDesc = description,
                        AddedDate = DateTime.Now,
                        AddedBy = user.Id
                    };

                    switch (decision)
                    {
                        case "Approve":
                            approval.IsApproved = true;
                            approval.ApprovalDate = DateTime.Now;
                            approval.ApprovedBy = user.Id;
                            approval.ApprovalRemarks = remarks;
                            break;
                        case "Decline":
                            approval.IsDeclined = true;
                            approval.DeclinedDate = DateTime.Now;
                            approval.DeclinedBy = user.Id;
                            approval.DeclinedRemarks = remarks;
                            break;
                    }

                    _wdsUnit.VarificationApprovalRepostitory.Add(approval);
                    _wdsUnit.Commit();
                }
                return "Transaction Succeeded";
            }
            catch (Exception exception)
            {
                var msge = exception.Message;
                if (exception.InnerException != null && exception.InnerException.InnerException != null)
                    msge = msge + " in-message: " + exception.InnerException.InnerException.Message;
                return msge;
            }
            
        }
    }
}