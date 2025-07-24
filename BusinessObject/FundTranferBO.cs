using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class FundTranferBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime Date { get; set; }
        public int FromLocationID { get; set; }
        public int ToLocationID { get; set; }
        public bool IsDraft { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation{ get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public decimal Amount { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string ModeOfPayment { get; set; }      
        public decimal TotalAmount { get; set; }
        public decimal CreditBalance { get; set; }
        public List<FundTranferTransBO> Items { get; set; }
       
    }
    public class FundTranferTransBO
    {
        public int ID { get; set; }
        public int FundTransferID { get; set; }
        public int FromLocationID { get; set; }
        public int ToLocationID { get; set; }
        public String FromLocation { get; set; }
        public String ToLocation { get; set; }
        public int FromBankID { get; set; }
        public int ToBankID { get; set; }
        public string FromBank { get; set; }
        public string ToBank { get; set; }
        public decimal Amount { get; set; }
        public string ModeOfPayment { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime InstrumentDate { get; set; }
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public decimal CreditBalance { get; set; }
    }
}
