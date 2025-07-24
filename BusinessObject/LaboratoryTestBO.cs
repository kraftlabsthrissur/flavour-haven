using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class LaboratoryTestBO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public DateTime AddedDate { get; set; }

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
        public int SpecimenID { get; set; }
        public int GSTCategoryID { get; set; }
        public string Method { get; set; }
        public decimal Rate { get; set; }
        public string Specimen { get; set; }
        public decimal GSTCategory { get; set; }
        public bool IsAlsoGroup { get; set; }
    }
    public class LabItemBO
    {
        public int LabTestID { get; set; }
        public string LabTest { get; set; }
    }
        
}
