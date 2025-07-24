using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CategoryBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int CategoryGroupID { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }       
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string  Type { get; set; }
        public string Code { get; set; }
        public string CategoryType { get; set; }
        public int CategoryTypeID { get; set; }

    }
}
