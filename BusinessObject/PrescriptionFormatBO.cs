using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PrescriptionFormatBO
    {
        public int ID { get; set; }
        public string MedicineCategory { get; set; }
        public int MedicineCategoryID { get; set; }
        public string Prescription { get; set; }
        public int count { get; set; }
        public List<PrescriptionFormatItemBO> PrescriptionItemBO { get; set; }
    }
    public class PrescriptionFormatItemBO
    {
        public int ID { get; set; }
        public string MedicineCategory { get; set; }
        public int MedicineCategoryID { get; set; }
        public string Prescription { get; set; }
        public int count { get; set; }

    }
}
