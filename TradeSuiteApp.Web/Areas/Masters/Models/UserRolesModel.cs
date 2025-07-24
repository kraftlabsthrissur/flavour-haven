using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class UserRolesModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public SelectList LocationList { get; set; }
        public int LocationID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<RolesModel> UserRoles { get; set; }
        public string Code { get; set; }
    }

    public class RolesModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public int LocationID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}