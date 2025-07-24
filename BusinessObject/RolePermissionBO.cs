using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RolePermissionBO
    {
        public int RoleModuleID { get; set; }
        public int   RoleID { get; set; }
        public string RoleName { get; set; }
        public int ModuleID { get; set; }
        public string   Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Key { get; set; }
        public bool IsHaveAccess { get; set; }
    }
    public partial class ModuleBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Key { get; set; }
        public string TableName { get; set; }
    }
}
