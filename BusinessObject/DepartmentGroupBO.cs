using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public partial class DepartmentGroupBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
