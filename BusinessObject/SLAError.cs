using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SLAError
    {
        public int SLAMappingID { get; set; }
        public string TrnType { get; set; }
        public string KeyValue { get; set; }
        public string TableName { get; set; }
        public int TransID { get; set; }
        public int SupplierID { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public int TablePrimaryID { get; set; }
        public string DocumentTable { get; set; }
        public string DocumentNo { get; set; }

    }
}
