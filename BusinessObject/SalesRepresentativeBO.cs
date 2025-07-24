using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class SalesRepresentativeBO
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public bool IsSubLevel { get; set; }
        public bool IsChild { get; set; }
        public string FSOName { get; set; }
        public int DesignationID { get; set; }
        public string Designation { get; set; }
        public int AreaID { get; set; }
        public int EmployeeID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public string Area { get; set; }
    }
    public class SalesAreaBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
