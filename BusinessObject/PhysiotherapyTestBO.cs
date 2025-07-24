using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PhysiotherapyTestBO
    {
        public int ID { get; set; }
        public int ItemUnitID { get; set; }
        public int PurchaseCategoryID { get; set; }
        public int QCCategoryID { get; set; }
        public int GSTSubCategoryID { get; set; }
        public int SalesCategoryID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public int StorageCategoryID { get; set; }
        public int ItemTypeID { get; set; }
        public int AccountsCategoryID { get; set; }
        public int BusinessCategoryID { get; set; }
        public int CategoryID { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; }
        public string Description { get; set; }
    }
}
