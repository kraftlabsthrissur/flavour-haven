using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class WareHouseBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string ItemTypeName { get; set; }
        public int? ItemTypeID { get; set; }
        public string Remarks { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
    }
}
