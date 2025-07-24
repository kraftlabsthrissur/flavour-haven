using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class MaterialPurificationBO
    {
        public int ID { get; set; }
        public int ItemCategoryID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int PurificationItemID { get; set; }
        public int PurificationUnitID { get; set; }
        public string PurificationItemName { get; set; }
        public string PurificationUnit { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string CategoryName { get; set; }
    }
}
