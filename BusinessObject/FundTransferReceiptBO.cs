using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class FundTransferReceiptBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public int FromLocationID { get; set; }
        public int FromBankID { get; set; }
        public int ToLocationID { get; set; }
        public int ToBankID { get; set; }
        public string FromLocationName { get; set; }
        public string ToLocationName { get; set; }
        public string FromBankName { get; set; }
        public string ToBankName { get; set; }
        public string Payment { get; set; }
        public int ModeOfPayment { get; set; }
        public DateTime InstrumentDate { get; set; }
        public string InstrumentNumber { get; set; }
        public string Remarks { get; set; }
    }

    public class FundTransferItemBO
    {
        public int IssueTransID { get; set; }
        public int FromLocationID { get; set; }
        public int FromBankID { get; set; }
        public int ToLocationID { get; set; }
        public int ToBankID { get; set; }
        public int ModeOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string FromLocationName { get; set; }
        public string ToLocationName { get; set; }
        public string Payment { get; set; }
        public string FromBankName { get; set; }
        public string ToBankName { get; set; }
    }
}
