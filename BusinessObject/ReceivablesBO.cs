using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ReceivablesBO
    {
        public int ID { get; set; }
        public int ReferenceID { get; set; }
        public int PartyID { get; set; }
        public string ReceivableType { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string DueDateStr;
        public string TransDateStr;
        public decimal ReceivableAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public decimal Discount { get; set; }
        public int CreditNoteID { get; set; }
        public int DebitNoteID { get; set; }
        public int AdvanceID { get; set; }
        public string PendingDays { get; set; }
        public int SalesReturnID { get; set; }
        public int CustomerReturnVoucherID { get; set; }
    }
}
