using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SecondaryUnitBO
    {
        public int? ID { get; set; }
        public string Name { get; set; }

        public int UnitID { get; set; }

        public string Unit { get; set; }
        public int UnitGroupID { get; set; }
        public string UnitGroup { get; set; }

        public decimal PackSize { get; set; }
    }
}
