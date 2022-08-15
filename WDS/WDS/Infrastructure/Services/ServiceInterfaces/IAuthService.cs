using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WDS.DAL.WdsEntities;
using WDS.Models;

namespace WDS.Infrastructure.Services.ServiceInterfaces
{
    public interface IAuthService
    {
        string SaveUser(User model);
        List<SelectListItem> GetRoles();
        User LoginAuthorize(string userName, string password);
        Role GetRoleById(long roleId);
        List<PermissionModel> GetUserPermissions(List<long?> roleIdList);
        Tuple<int, string> ChangePassword(ChangePasswordModel model);
    }
}
