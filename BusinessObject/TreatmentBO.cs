using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
   public class TreatmentListBO
    {
        public int ID { get; set; }
        public string TreatmentCode { get; set; }
        public string TreatmentName { get; set; }
        public DateTime AddedDate { get; set; }
        public string Description { get; set; }
        public int TreatmentGroupID { get; set; }
        public string TreatmentGroup { get; set; }
        public SelectList TreatmentGroupList { get; set; }
        
    }
}
