﻿using System.Web;
using System.Web.Mvc;
using WDS.Infrastructure.Helper;

namespace WDS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MemberAuthorization());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
