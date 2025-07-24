using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CustomerReturnVoucherBO
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string BankReferenceNumber { get; set; }
        public string Remarks { get; set; }
        public List<CustomerReturnVoucherItemBO> Items { get; set; }
    }

    public class CustomerReturnVoucherItemBO
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}
