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
    public class SupplierCreditNoteBL : ISupplierCreditNoteContract
    {
        SupplierCreditNoteDAL creditnoteDAL;
        DebitOrCreditBO debitOrCreditBO;
        PrintHelper PHelper;
        PrintBL PrintBL;
        private IGeneralContract generalBL;
        public SupplierCreditNoteBL()
        {
            creditnoteDAL = new SupplierCreditNoteDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
            PrintBL = new PrintBL();
        }
        public List<SupplierCreditNoteBO> SupplierCreditNoteList()
        {
            return creditnoteDAL.SupplierCreditNoteList();
        }
        public String Save(SupplierCreditNoteBO Master, List<SupplierCreditNoteTransBO> Details)
        {
            if (Master.ID == 0)
            {
                return creditnoteDAL.Save(Master, Details);
            }
            else
            {
                return creditnoteDAL.Update(Master, Details);
            }
        }
        public List<SupplierCreditNoteBO> GetCreditNoteDetail(int CreditNoteID)
        {
            return creditnoteDAL.GetCreditNoteDetail(CreditNoteID);
        }
        public List<SupplierCreditNoteTransBO> GetCreditNoteDetailTrans(int CreditNoteID)
        {
            return creditnoteDAL.GetCreditNoteDetailTrans(CreditNoteID);
        }
        public DatatableResultBO GetSupplierCreditNoteList(string Type, string TransNo, string TransDate, string Supplier, string ReferenceInvoiceNumber, string ReferenceDocumentDate, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return creditnoteDAL.GetSupplierCreditNoteList(Type, TransNo, TransDate, Supplier, ReferenceInvoiceNumber, ReferenceDocumentDate, TotalAmount, SortField, SortOrder, Offset, Limit);
        }
        public string GetPrintTextFile(int ID)
        {
            string FileName;
            string FilePath;
            string URL;
            debitOrCreditBO = new DebitOrCreditBO();
            debitOrCreditBO = creditnoteDAL.GetCreditNoteDetail(ID).Select(m => new DebitOrCreditBO()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = m.Date,
                PartyName = m.SupplierName,
                PartyType = "Supplier",
                DebitOrCreditType = "Credit",
                GSTNo = m.GSTNo,
                Addresses = m.Addresses,
                BillingStateID = m.BillingStateID,
                BillingState = m.BillingState,
                BankName = m.BankName,
                IFSCNo = m.IFSCNo,
                BankACNo = m.BankACNo,
                DiscountAmount = (decimal)m.DiscountAmount,
                TaxableAmount = (decimal)m.TaxableAmount,
                CGSTAmt = (decimal)m.CGSTAmt,
                SGSTAmt = (decimal)m.SGSTAmt,
                IGSTAmt = (decimal)m.IGSTAmt,
                TotalAmount = m.TotalAmount
            }).First();
            debitOrCreditBO.Items = creditnoteDAL.GetCreditNoteDetailTrans(ID).Select(m => new DebitOrCreditNoteItemBO()
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
            FilePath = path + "/Outputs/SupplierCreditNote/" + FileName;
            PrintHelper.TotalLines = debitOrCreditBO.Items.Count();
            PrintHelper.BodyLines = Convert.ToInt32(35);
            PHelper.SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintBL.PrintDebitOrCreditHeader(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditBody(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditFooter(writer, debitOrCreditBO);
            }
            URL = "/Outputs/SupplierCreditNote/" + FileName;
            return URL;
        }
    }
}
