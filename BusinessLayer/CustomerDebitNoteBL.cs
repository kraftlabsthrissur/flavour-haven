using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CustomerDebitNoteBL : ICustomerDebitNoteContract
    {
        CustomerDebitNoteDAL debitnoteDAL;
        DebitOrCreditBO debitOrCreditBO;
        PrintHelper PHelper;
        PrintBL PrintBL;
        private IGeneralContract generalBL;
        public CustomerDebitNoteBL()
        {
            debitnoteDAL = new CustomerDebitNoteDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
            PrintBL = new PrintBL();
        }
        public List<CustomerDebitNoteBO> CustomerDebitNoteList()
        {
            return debitnoteDAL.CustomerDebitNoteList();
        }
        public string Save(CustomerDebitNoteBO Master,List<CustomerDebitNoteTransBO> Details)
        {
            if (Master.ID > 0)
            {
                return debitnoteDAL.Update(Master, Details);
            }
            else
            {
                return debitnoteDAL.Save(Master, Details);
            }
            
        }
        public List<CustomerDebitNoteBO> GetDebitNoteDetail(int DebitNoteID)
        {
            return debitnoteDAL.GetDebitNoteDetail(DebitNoteID);
        }
        public List<CustomerDebitNoteTransBO> GetDebitNoteDetailTrans(int DebitNoteID)
        {
            return debitnoteDAL.GetDebitNoteDetailTrans(DebitNoteID);
        }

        public string GetPrintTextFile(int ID)
        {
            string FileName;
            string FilePath;
            string URL;
            debitOrCreditBO = new DebitOrCreditBO();
            debitOrCreditBO = debitnoteDAL.GetDebitNoteDetail(ID).Select(m => new DebitOrCreditBO()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = m.Date,
                PartyName = m.CustomerName,
                PartyType = "Customer",
                DebitOrCreditType = "Debit",
                GSTNo = m.GSTNo,
                Addresses = m.Addresses,
                BillingStateID = m.BillingStateID,
                BillingState = m.BillingState,
                DiscountAmount = (decimal)m.DiscountAmount,
                TaxableAmount = (decimal)m.TaxableAmount,
                CGSTAmt = (decimal)m.CGSTAmt,
                SGSTAmt = (decimal)m.SGSTAmt,
                IGSTAmt = (decimal)m.IGSTAmt,
                TotalAmount = m.TotalAmount
            }).First();
            debitOrCreditBO.Items = debitnoteDAL.GetDebitNoteDetailTrans(ID).Select(m => new DebitOrCreditNoteItemBO()
            {
                ReferenceInvoiceNumber = m.ReferenceInvoiceNumber,
                ReferenceDocumentDate = m.ReferenceDocumentDate,
                Item = m.Item,
                HSNCode = m.HSNCode,
                Unit = m.Unit,
                Qty = m.Qty,
                Rate = m.Rate,
                Amount = m.Amount,
                DiscountAmount = (decimal)m.DiscountAmount,
                TaxableAmount = (decimal)m.TaxableAmount,
                CGSTAmt = (decimal)m.CGSTAmt,
                SGSTAmt = (decimal)m.SGSTAmt,
                IGSTAmt = (decimal)m.IGSTAmt,
                Remarks = m.Remarks
            }).ToList();
            string TransNo = debitOrCreditBO.TransNo;
            FileName = TransNo + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = path + "/Outputs/CustomerDebitNote/" + FileName;
            PrintHelper.TotalLines = debitOrCreditBO.Items.Count();
            PrintHelper.BodyLines = Convert.ToInt32(35);
            PHelper.SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintBL.PrintDebitOrCreditHeader(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditBody(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditFooter(writer, debitOrCreditBO);
            }
            URL = "/Outputs/CustomerDebitNote/" + FileName;
            return URL;
        }

        public DatatableResultBO GetCustomerDebitNoteList(string Type, string TransNoNoHint, string TransDateHint, string CustomerHint, string InvoiceNoHint, string DocumentDateHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return debitnoteDAL.GetCustomerDebitNoteList(Type, TransNoNoHint, TransDateHint, CustomerHint, InvoiceNoHint, DocumentDateHint, AmountHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
