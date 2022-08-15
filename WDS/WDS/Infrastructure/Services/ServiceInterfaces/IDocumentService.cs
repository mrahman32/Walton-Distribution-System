using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.Models;
using WDS.ViewModels;

namespace WDS.Infrastructure.Services.ServiceInterfaces
{
    public interface IDocumentService
    {
        DistributorCheckListModel GetDistributorCheckListByDistributorCode(string dealerCode);
        DistributorCheckList CreateNewCheckList(DistributorCheckListModel model);
        string CreateNewMOA(DistributorCheckListModel model, DistributorCheckList checkList);
        DistributorCheckList UpdateDistributorCheckList(DistributorCheckListModel model);
        string UpdateMoa(DistributorCheckListModel model, DistributorCheckList checkList);
        string CreateNewMOC(DistributorCheckListModel model, DistributorCheckList checkList);
        string UpdateMoc(DistributorCheckListModel model, DistributorCheckList checkList);
        string CreateNewBS(DistributorCheckListModel model, DistributorCheckList checkList);
        string UpdateBS(DistributorCheckListModel model, DistributorCheckList checkList);
        string CreateNewTIN(DistributorCheckListModel model, DistributorCheckList checkList);
        string UpdateTIN(DistributorCheckListModel model, DistributorCheckList checkList);
        string CreateNewVAT(DistributorCheckListModel model, DistributorCheckList checkList);
        string UpdateVAT(DistributorCheckListModel model, DistributorCheckList checkList);
        string CreateNewTrade(DistributorCheckListModel model, DistributorCheckList checkList);
        string UpdateTrade(DistributorCheckListModel model, DistributorCheckList checkList);
        string CreateNewOther(DistributorCheckListModel model, DistributorCheckList checkList);
        string UpdateOther(DistributorCheckListModel model, DistributorCheckList checkList);
        List<CheckListApprovalPendingModel> GetApprovalPendingCheckList();
        AjaxResponseModel ApproveDocuments(long id);
        string CreateDistributorPersonalInfo(DistributorCheckListModel model, DistributorCheckList checkList);
        string UpdateDistributorPersonalInfo(DistributorCheckListModel model, DistributorCheckList checkList);
        List<CheckListApprovalPendingModel> GetVerificationPendingList();
        List<VmDistributorDocVerificationList> GetDocList(long checkListId);
        List<VarificationEntityModel> GetDocumentLst(long checkListId);
        string SaveInvestigationResult(long id, long checkListId, string description, string remarks, string decision);
    }
}
