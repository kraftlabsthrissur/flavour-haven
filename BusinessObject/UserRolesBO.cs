using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class UserRolesBO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public string Code { get; set; }
    }

    public class RolesBO
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string check { get; set; }
    }
}
