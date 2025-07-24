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
    public class SupplierDebitNoteBL : ISupplierDebitNoteContract
    {
        SupplierDebitNoteDAL debitnoteDAL;
        DebitOrCreditBO debitOrCreditBO;
        PrintHelper PHelper;
        PrintBL PrintBL;
        private IGeneralContract generalBL;
        public SupplierDebitNoteBL()
        {
            debitnoteDAL = new SupplierDebitNoteDAL();
            generalBL = new GeneralBL();
            PHelper = new PrintHelper();
            PrintBL = new PrintBL();
        }
        public List<SupplierDebitNoteBO> SupplierDebitNoteList()
        {
            return debitnoteDAL.SupplierDebitNoteList();
        }
        public string Save(SupplierDebitNoteBO Master, List<SupplierDebitNoteTransBO> Details)
        {
            if(Master.ID>0)
            {
                return debitnoteDAL.Update(Master, Details);
            }
            else
            {
                return debitnoteDAL.Save(Master, Details);
            }

        }
        public List<SupplierDebitNoteBO> GetDebitNoteDetail(int DebitNoteID)
        {
            return debitnoteDAL.GetDebitNoteDetail(DebitNoteID);
        }
        public List<SupplierDebitNoteTransBO> GetDebitNoteTransDetail(int DebitNoteID)
        {
            return debitnoteDAL.GetDebitNoteTransDetail(DebitNoteID);
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
                PartyName = m.SupplierName,
                PartyType = "Supplier",
                DebitOrCreditType = "Debit",
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
            debitOrCreditBO.Items = debitnoteDAL.GetDebitNoteTransDetail(ID).Select(m => new DebitOrCreditNoteItemBO()
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
                Remarks = m.Remarks,
                PurchaseReturnNo = m.PurchaseReturnNo,
            }).ToList();
            string TransNo = debitOrCreditBO.TransNo;
            FileName = TransNo + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = path + "/Outputs/SupplierDebitNote/" + FileName;
            PrintHelper.TotalLines = debitOrCreditBO.Items.Count();
            PrintHelper.BodyLines = Convert.ToInt32(35);
            PHelper.SetPageVariables();
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                PrintBL.PrintDebitOrCreditHeader(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditBody(writer, debitOrCreditBO);
                PrintBL.PrintDebitOrCreditFooter(writer, debitOrCreditBO);
            }
            URL = "/Outputs/SupplierDebitNote/" + FileName;
            return URL;
        }

        public DatatableResultBO GetSupplierDebitNoteList(string Type, string TransNo, string TransDate, string Supplier, string ReferenceInvoiceNumber, string ReferenceDocumentDate, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return debitnoteDAL.GetSupplierDebitNoteList( Type, TransNo, TransDate, Supplier, ReferenceInvoiceNumber, ReferenceDocumentDate, TotalAmount,SortField, SortOrder,Offset,  Limit);
        }

    }
}
