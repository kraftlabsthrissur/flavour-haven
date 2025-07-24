using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class UnitBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int QOM { get; set; }
        public string UOM { get; set; }
        public int CF { get; set; }
        public Nullable<int> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Value { get; set; }
        public decimal PackSize { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
    }
}
