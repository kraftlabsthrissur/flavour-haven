using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PaymentReturnVoucherBO
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public int DebitNoteID { get; set; }
        public string TransNo { get; set; }
        public string DebitAccountCode { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string BankReferenceNumber { get; set; }
        public string Remarks { get; set; }
        public string SupplierBankName { get; set; }
        public string SupplierBankACNo { get; set; }
        public string SupplierIFSCNo { get; set; }
    }
    public class PaymentReturnVoucherItemBO
    {
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}
