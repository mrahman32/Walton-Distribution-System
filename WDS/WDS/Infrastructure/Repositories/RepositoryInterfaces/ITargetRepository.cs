using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDS.DAL.WdsEntities;
using WDS.Models;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface ITargetRepository:IRepository<Target>
    {
        List<TargetModel> GetTargetModelList();
        List<stp_dealer_target_achievement_report_Result> GetDealerTargetVsAchievementReport(string targetType);
    }
}
