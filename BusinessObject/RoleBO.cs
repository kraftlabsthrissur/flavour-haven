using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class RoleBO
    {
        public string Code { get; set; }
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string Remarks { get; set; }
        public string Actions { get; set; }
        public string Tabs { get; set; }
        public string Controller { get; set; }
    }

    public class ActionIDBO
    {
        public int ID { get; set; }
    }
}
