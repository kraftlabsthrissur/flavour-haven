using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
    public class SalesInquiryBO
    {
        public int ID {  get; set; }
        public string SalesInquiryNo { get; set; }
        public DateTime? SalesInquiryDate { get; set; }
        public string RequestedCustomerName { get; set; }
        public DateTime? RequestedDelivaryDate { get; set; }
        public DateTime? RequestExpiryDate { get; set; }
        public string Status { get; set; }
        public string RequestedCustomerAddress { get; set; }
        public string Remarks { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string SIOrVINNumber { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public bool IsDraft { get; set; }
    }

}
