using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;

namespace WDS.Infrastructure.Services.ServiceClasses
{
    public class AuthService:IAuthService
    {
        private readonly Repositories.RepositoryClasses.Unit _unit;

        public AuthService(Repositories.RepositoryClasses.Unit unit)
        {
            _unit = unit;
        }
        public string SaveUser(User model)
        {
            try
            {
                var isExistUser = _unit.UserRepository.Find(i => i.UserName == model.UserName);
                if (!isExistUser.Any())
                {
                    model.IsActive = true;

                    _unit.UserRepository.Add(model);
                    _unit.Commit();
                    return "success";
                }
                return "User already exist";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public List<SelectListItem> GetRoles()
        {
            List<Role> roles = _unit.RoleRepository.GetAll();
            var roleSelectListItems = roles.Select(role => new SelectListItem
            {
                Value = role.Id.ToString(CultureInfo.InvariantCulture), Text = role.RoleName
            }).ToList();
            return roleSelectListItems;
        }

        public User LoginAuthorize(string userName, string password)
        {
            try
            {
                User user = _unit.UserRepository.Find(i => i.UserName == userName && i.Password == password).FirstOrDefault();
                if (user != null)
                {
                    string roleName = _unit.RoleRepository.Get((long) user.RoleId).RoleName;
                    var ticket = new FormsAuthenticationTicket(
                        1, // Ticket version
                        Convert.ToString(user.Id), // Username associated with ticket
                        DateTime.Now, // Date/time issued
                        DateTime.Now.AddMinutes(540), // Date/time to expire
                        true, // "true" for a persistent user cookie
                        roleName, // User-data, in this case the roles
                        FormsAuthentication.FormsCookiePath); // Path cookie valid for

                    // Encrypt the cookie using the machine key for secure transport,
                    string hash = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(
                        FormsAuthentication.FormsCookieName, // Name of auth cookie
                        hash); // Hashed ticket

                    // Set the cookie's expiration time to the tickets expiration time
                    if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

                    // Add the cookie to the list for outgoing response
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    return user;
                }
                return null;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public Role GetRoleById(long roleId)
        {
            Role role = _unit.RoleRepository.Get(roleId);
            return role;
        }

       

        public List<PermissionModel> GetUserPermissions(List<long?> roleIdList)
        {
            var permissionModels = new List<PermissionModel>();
            List<SubMenu> subMenus = _unit.SubMenuRepository.Find(i => i.IsActive == true && roleIdList.Contains(i.RoleId)).OrderBy(i=>i.MainMenuId).ToList();
            if (roleIdList.Contains(1))
            {
                subMenus = _unit.SubMenuRepository.Find(i => i.IsActive == true);
            }
            var mainMenuIds = subMenus.Select(i => i.MainMenuId).Distinct().ToList();
            foreach (var mainMenuId in mainMenuIds)
            {
                var permissionModel = new PermissionModel();
                MainMenu mainMenu = _unit.MainMenuRepository.Get((int)mainMenuId);
                List<SubMenu> subs = subMenus.Where(i => i.MainMenuId == mainMenuId).ToList();
                permissionModel.MainMenu = mainMenu;
                permissionModel.SubMenus = subs;
                permissionModels.Add(permissionModel);
            }
            return permissionModels;
        }

        public Tuple<int, string> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                User users = _unit.UserRepository.Get(model.UserId);
                if (users.Password != model.CurrentPassword)
                    return new Tuple<int, string>(2, "Your Current Password does not match.");
                users.Password = model.Password;
                _unit.UserRepository.Update(users);
                _unit.Commit();
                return new Tuple<int, string>(1, "Password Changed Successfully");

            }
            catch (Exception e)
            {
                return new Tuple<int, string>(2, "Error Occured");
            }
        }
    }
}