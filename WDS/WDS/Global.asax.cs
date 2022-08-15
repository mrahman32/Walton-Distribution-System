using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using WDS.Infrastructure.Repositories.RepositoryClasses;
using WDS.Infrastructure.Repositories.RepositoryInterfaces;
using WDS.Infrastructure.Services.ServiceClasses;
using WDS.Infrastructure.Services.ServiceInterfaces;

namespace WDS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyInjection();
        }
        private static void DependencyInjection()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = new WebRequestLifestyle();
            container.Register<IUnit, Unit>(Lifestyle.Transient);
            container.Register<IAuthService, AuthService>(Lifestyle.Transient);
            container.Register<IDashboardService, DashboardService>(Lifestyle.Transient);
            container.Register<IDealerService, DealerService>(Lifestyle.Transient);
            container.Register<IRetailerService, RetailerService>(Lifestyle.Transient);
            container.Register<IAdminService, AdminService>(Lifestyle.Transient);
            container.Register<ICommonService, CommonService>(Lifestyle.Transient);
            container.Register<IManagementService, ManagementService>(lifestyle:Lifestyle.Transient);
            container.Register<IDocumentService, DocumentService>(lifestyle:Lifestyle.Transient);



            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

        }
    }
}
