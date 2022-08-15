using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDS.DAL.WdsEntities;

namespace WDS.Models
{
    public class PermissionModel
    {
        public MainMenu MainMenu { get; set; }
        public List<SubMenu> SubMenus { get; set; }
    }
}