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
    public class CustomerCreditNoteBL : ICustomerCreditNoteContract
    {
        CustomerCreditNoteDAL creditinoteDAL;
        DebitOrCreditBO debitOrCreditBO;
        PrintHelper PHelper;
        PrintBL PrintBL;
        private IGeneralContract generalBL;
        public CustomerCreditNoteBL()
        {
            creditinoteDAL = new CustomerCreditNoteDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
            PrintBL = new PrintBL();
        }
        public List<CustomerCreditNoteBO> CreditNoteList()
        {
            return creditinoteDAL.CreditNoteList();
        }

        public String Save(CustomerCreditNoteBO Master, List<CustomerCreditNoteTransBO> Details)
        {
            if(Master.ID>0)
            {
                return creditinoteDAL.Update(Master, Details);
            }
            else
            {
                return creditinoteDAL.Save(Master, Details);
            }
            
        }
        public List<CustomerCreditNoteBO> GetCreditNoteDetails(int CreditNoteID)
        {
            return creditinoteDAL.GetCreditNoteDetails(CreditNoteID);
        }
        public List<CustomerCreditNoteTransBO> GetCreditNoteTransDetails(int CreditNoteID)
        {
            return creditinoteDAL.GetCreditNoteTransDetails(CreditNoteID);
        }

        public string GetPrintTextFile(int ID)
        {
            string FileName;
            string FilePath;
            string URL;
            debitOrCreditBO = new DebitOrCreditBO();
            debitOrCreditBO = creditinoteDAL.GetCreditNoteDetails(ID).Select(m => new DebitOrCreditBO()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = m.Date,
                PartyName = m.CustomerName,
                PartyType = "Customer",
                DebitOrCreditType = "Credit",
                GSTNo = m.GSTNo,
                Addresses = m.Addresses,
                BillingStateID = m.BillingStateID,
                BillingState = m.BillingState,
                BankName = "",
                IFSCNo = "",
                BankACNo = "",
                DiscountAmount = (decimal)m.DiscountAmount,
                TaxableAmount = (decimal)m.TaxableAmount,
                CGSTAmt = (decimal)m.CGSTAmt,
                SGSTAmt = (decimal)m.SGSTAmt,
                IGSTAmt = (decimal)m.IGSTAmt,
                TotalAmount = m.TotalAmount
            }).First();
            debitOrCreditBO.Items = creditinoteDAL.GetCreditNoteTransDetails(ID).Select(m => new DebitOrCreditNoteItemBO()
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
            FilePath = path + "/Outputs/CustomerCreditNote/" + FileName;
            PrintHelper.TotalLines = debitOrCreditBO.Items.Count();
            PrintHelper.BodyLines = Convert.ToInt32(35);
            PHelper.SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintBL.PrintDebitOrCreditHeader(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditBody(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditFooter(writer, debitOrCreditBO);
            }
            URL = "/Outputs/CustomerCreditNote/" + FileName;
            return URL;
        }

        public DatatableResultBO GetCustomerCreditNoteList(string Type, string TransNoNoHint, string TransDateHint, string CustomerHint, string InvoiceNoHint, string DocumentDateHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return creditinoteDAL.GetCustomerCreditNoteList(Type, TransNoNoHint, TransDateHint, CustomerHint, InvoiceNoHint, DocumentDateHint, AmountHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
