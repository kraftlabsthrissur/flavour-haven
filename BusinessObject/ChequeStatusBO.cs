using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ChequeStatusBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime ReceiptDateFrom { get; set; }
        public DateTime ReceiptDateTo { get; set; }
        public int InstrumentStatusID { get; set; }
        public string InstrumentStatus { get; set; }
        public bool IsDraft { get; set; }
        public List<ChequeStatusTransBO> Items { get; set; }
        public string CustomerName { get; set; }

    }
    public class ChequeStatusTransBO
    {
        public int ID { get; set; }     
        public string VoucherNo { get; set; }
        public string InstrumentNumber { get; set; }
        public int CustomerID { get; set; }
        public DateTime InstrumentDate { get; set; }    
        public DateTime StatusChangeDate { get; set; }
        public DateTime ChequeReceivedDate { get; set; }
        public string CustomerName { get; set; }
        public decimal InstrumentAmount { get; set; }
        public decimal BankCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ChargesToCustomer { get; set; }
        public string Remarks { get; set; }
        public string VoucherDate { get; set; }
        public int ChequeStatusID { get; set; }
        public string ChequeStatus { get; set; }
        public bool IsActive { get; set; }
        public int VoucherID { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public int StateID { get; set; }
    }
}
