//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WDS.DAL.WdsEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubMenu
    {
        public long Id { get; set; }
        public string SubMenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Nullable<int> MainMenuId { get; set; }
        public Nullable<long> RoleId { get; set; }
        public Nullable<bool> IsMenu { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
