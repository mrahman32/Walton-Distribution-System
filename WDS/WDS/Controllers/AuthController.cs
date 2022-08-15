using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
//using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper.QueryableExtensions;
using Microsoft.Ajax.Utilities;
using WDS.DAL.WdsEntities;
using WDS.Infrastructure.Services.ServiceInterfaces;
using WDS.Models;

namespace WDS.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            User user = new User{AddedDate = DateTime.Today};
            /**
                                     * get assembly information to decide wather it is WALTON or MARCEL**/

            string company = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
            var session = HttpContext.Session;
            session["company"] = company;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(User model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _authService.LoginAuthorize(model.UserName, model.Password);
                    var roleIdList = new List<long?>();
                    if (user != null)
                    {
                        if (user.RoleId != null)
                        {
                            roleIdList.Add((long)user.RoleId);
                            if (!string.IsNullOrWhiteSpace(user.ExtendedRoleId))
                            {
                                List<string> l = user.ExtendedRoleId.Split(',').ToList();
                                foreach (var x in l)
                                {
                                    long ll = 0;
                                    long.TryParse(x, out ll);
                                    roleIdList.Add(ll);
                                }
                            }
                            if (user.IsActive == true)
                            {
                                var session = HttpContext.Session;
                                if (session != null)
                                {
                                    session["user"] = user;
                                    session["permissions"] = _authService.GetUserPermissions(roleIdList);
                                    string company = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
                                    session["company"] = company;
                                    ViewBag.LoginSuccessed = true;
                                    var usr = HttpContext.User.Identity.Name;
                                    Tuple<string, string> redirectTuple = GetLogInDirection((long)user.RoleId);
                                    var action = redirectTuple.Item1;
                                    var controller = redirectTuple.Item2;

                                    

                                    return RedirectToAction(action, controller, new { area = "" });
                                }
                            }
                            else
                            {
                                TempData["comName"] = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
                                TempData["message"] = "This User is not active. Please Contact with administrator !!!";
                                TempData["mType"] = 2;
                            }
                        }
                    }
                    else
                    {
                        TempData["comName"] =  ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
                        TempData["message"] = "Username or Password not valid !!!";
                        TempData["mType"] = 2;
                    }
                }
                ViewBag.LoginSuccessed = false;
                Session.Clear();
                Session.Abandon();
                ModelState.Clear();
                return View();
            }
            catch (Exception ex)
            {
                ViewData["loginSuccessed"] = false;
                return View(new User());
            }
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            var cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "") {Expires = DateTime.Now.AddYears(-1)};
            Response.Cookies.Add(cookie1);
            var cookie2 = new HttpCookie("ASP.NET_SessionId", "") {Expires = DateTime.Now.AddYears(-1)};
            Response.Cookies.Add(cookie2);
            return RedirectToAction("Login");
        }

        private Tuple<string, string> GetLogInDirection(long roleId)
        {
            Role role = _authService.GetRoleById(roleId);
            if (role.RoleName.ToLower().Equals("superadmin"))
                return new Tuple<string, string>("SaDashboard", "Dashboard");
            if (role.RoleName.ToLower().Equals("admin"))
                return new Tuple<string, string>("AdminDashboard", "Dashboard");
            if (role.RoleName.ToLower().Equals("dealer"))
                return new Tuple<string, string>("DealerDashboard", "Dashboard");
            if (role.RoleName.ToLower().Equals("retailer"))
                return new Tuple<string, string>("RetailerDashboard", "Dashboard");
            if (role.RoleName.ToLower().Equals("asm") || role.RoleName.ToLower().Equals("rsm") || role.RoleName.ToLower().Equals("nsm") || role.RoleName.ToLower().Equals("tso"))
                return new Tuple<string, string>("GeneralInfoDashboard", "Dashboard");
            if (role.RoleName.ToLower().Equals("comanagement"))
                return new Tuple<string, string>("ManagementDashboard", "Dashboard");
            return new Tuple<string, string>("Index", "Dashboard"); ;
        }

        public ActionResult CreateUser()
        {
            List<SelectListItem> roles = _authService.GetRoles();
            ViewBag.Roles = roles;
            return View(new User());
        }

        [HttpPost]
        public ActionResult CreateUser(User model)
        {
            if (ModelState.IsValid)
            {
                model.AddedDate = DateTime.Now;
                var result = _authService.SaveUser(model);

                if (result == "success")
                {
                    TempData["message"] = "User creted successfully.";
                    TempData["mType"] = 1;
                }
                else
                {
                    TempData["message"] = result;
                    TempData["mType"] = 2;
                }
            }
            List<SelectListItem> roles = _authService.GetRoles();
            ViewBag.Roles = roles;
            return View(model);
        }
        [HttpGet]
        public ActionResult EditUser(long userId)
        {
            return View();
        }

        public ActionResult EditUser(User model)
        {
            return View();
        }

        public ActionResult AuthFailed()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            User user = HttpContext.Session["user"] as User;
            ChangePasswordModel model = new ChangePasswordModel
            {
                UserId = user.Id
            };
            return View(model);
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            Tuple<int, string> resulTuple = _authService.ChangePassword(model);
            int messageType = resulTuple.Item1;
            string message = resulTuple.Item2;
            TempData["message"] = message;
            TempData["messageType"] = messageType;
            if(messageType == 1) return RedirectToAction("ChangeSuccess");
            return RedirectToAction("ChangePassword");
        }

        public ActionResult ChangeSuccess()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            var cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(cookie1);
            var cookie2 = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(cookie2);
            return View();
        }
    }
}