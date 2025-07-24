using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PaymentRepository : IPayment
    {
        private readonly AccountsEntities _entity;


        public PaymentRepository()
        {
            _entity = new AccountsEntities();
        }

        /// <summary>
        /// Save Payment Voucher
        /// </summary>
        /// <param name="purchaseInvoiceBO"></param>
        /// <returns></returns>
        public int SavePaymentVoucher(PaymentVoucherBO paymentVoucherBO)
        {
            ObjectParameter paymentIDOut = new ObjectParameter("PaymentID", typeof(int));
            ObjectParameter Returnvalue = new ObjectParameter("RetVal", typeof(int));

            int paymentID = 0;
            if (paymentVoucherBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {

                        string FormName = "PaymentVoucher";
                        int transactionID = 0;
                        int accountID = 0;
                        decimal paidAmount = (paymentVoucherBO != null && paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null) ?
                            paymentVoucherBO.UnProcessedPurchaseInvoiceItems.Sum(x => x.PayNow) : 0;
                        bool isSettled = false;
                        decimal settlementAmount = 0;

                        int paymentTypeID = 0;

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (paymentVoucherBO.IsDraft)
                        {
                            FormName = "DraftPaymentVoucher";
                        }
                        if (paymentVoucherBO.PaymentMode == "Cash")
                        {
                            _entity.SpUpdateSerialNo(FormName, "Cash", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        }
                        else
                        {
                            _entity.SpUpdateSerialNo(FormName, "Other", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        }
                        //var j = _entity.SpUpdateSerialNo("PaymentVoucher", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        var i = _entity.SpCreatePayment(SerialNo.Value.ToString(), paymentVoucherBO.VoucherDate,
                            paymentVoucherBO.SupplierID, paymentVoucherBO.PaymentTypeID, paymentVoucherBO.BankName,
                            paymentVoucherBO.ReferenceNumber, paidAmount, paymentVoucherBO.Description, paymentVoucherBO.SaveType, isSettled,
                            settlementAmount, transactionID, accountID, paymentVoucherBO.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, paymentIDOut);

                        _entity.SaveChanges();

                        if (paymentIDOut.Value != null && Convert.ToInt32(paymentIDOut.Value) > 0)
                        {
                            paymentID = Convert.ToInt32(paymentIDOut.Value);

                            if (paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null)
                            {
                                DateTime? settledDate = null;
                                decimal discountedAmount = 0;

                                foreach (var unProcessedInvoice in paymentVoucherBO.UnProcessedPurchaseInvoiceItems)
                                {
                                    if (unProcessedInvoice.PayNow != 0)
                                    {

                                        _entity.SpCreatePaymentDet(
                                            unProcessedInvoice.DocumentType,
                                            unProcessedInvoice.DocumentNo,
                                            unProcessedInvoice.AdvanceID,
                                            unProcessedInvoice.DebitNoteID,
                                            unProcessedInvoice.CreditNoteID,
                                            unProcessedInvoice.PayNow,
                                            discountedAmount,
                                            settledDate,
                                            unProcessedInvoice.PayableID,
                                            paymentID,
                                            unProcessedInvoice.IRGID,
                                            unProcessedInvoice.PaymentReturnVoucherTransID,
                                            unProcessedInvoice.AmountToBePayed,
                                            paymentVoucherBO.IsDraft,
                                            unProcessedInvoice.Narration,
                                            GeneralBO.CreatedUserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            Returnvalue);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Returnvalue = paymentIDOut;
                            throw new DatabaseException("Total exceeds credit limit");
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        paymentID = 0;
                        throw ex;
                    }
                }
            }
            return (int)Returnvalue.Value;
        }

        public int UpdatePaymentVoucher(PaymentVoucherBO paymentVoucherBO)
        {
            int paymentID = 0;
            ObjectParameter paymentIDOut = new ObjectParameter("PaymentID", typeof(int));
            ObjectParameter Returnvalue = new ObjectParameter("RetVal", typeof(int));

            if (paymentVoucherBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        decimal paidAmount = (paymentVoucherBO != null && paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null) ?
                            paymentVoucherBO.UnProcessedPurchaseInvoiceItems.Sum(x => x.PayNow) : 0;

                        var i = _entity.SpUpdatePayment(paymentVoucherBO.ID,
                            paymentVoucherBO.PaymentTypeID, paymentVoucherBO.BankName,
                            paymentVoucherBO.ReferenceNumber, paymentVoucherBO.Description,  paidAmount, paymentVoucherBO.CurrencyID, paymentVoucherBO.CurrencyCode,
                            paymentVoucherBO.LocalCurrencyID,
                            paymentVoucherBO.LocalCurrencyCode,
                            paymentVoucherBO.CurrencyExchangeRate,
                            paymentVoucherBO.LocalVoucherAmt, GeneralBO.CreatedUserID, paymentVoucherBO.IsDraft, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, paymentIDOut);

                        _entity.SaveChanges();

                        if (Convert.ToInt32(paymentIDOut.Value) > 0)
                        {
                            if (paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null)
                            {
                                DateTime? settledDate = null;
                                decimal discountedAmount = 0;
                                foreach (var unProcessedInvoice in paymentVoucherBO.UnProcessedPurchaseInvoiceItems)
                                {
                                    if (unProcessedInvoice.PayNow != 0)
                                    {
                                        _entity.SpCreatePaymentDet(
                                            unProcessedInvoice.DocumentType,
                                            unProcessedInvoice.DocumentNo,
                                            unProcessedInvoice.AdvanceID,
                                            unProcessedInvoice.DebitNoteID,
                                            unProcessedInvoice.CreditNoteID,
                                            unProcessedInvoice.PayNow,
                                            discountedAmount,
                                            settledDate,
                                            unProcessedInvoice.PayableID,
                                            paymentVoucherBO.ID,
                                            unProcessedInvoice.IRGID,
                                            unProcessedInvoice.PaymentReturnVoucherTransID,
                                            unProcessedInvoice.AmountToBePayed,
                                            paymentVoucherBO.IsDraft,
                                            unProcessedInvoice.Narration,
                                            GeneralBO.CreatedUserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            Returnvalue);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Returnvalue = paymentIDOut;
                            throw new DatabaseException("Total exceeds credit limit");
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        paymentID = -1;
                        throw e;
                    }
                }
            }
            return (int)Returnvalue.Value;
        }

        public List<PayableDetailsBO> GetPayableDetailsForPaymentVoucher(int supplierID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetPayableDetailsForPaymentVoucher(
                        supplierID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PayableDetailsBO()
                        {
                            DocumentType = a.DocumentType,
                            DocumentNo = a.DocumentNo,
                            PayableID = a.PayableID,
                            AdvanceID = a.AdvanceID,
                            DebitNoteID = a.DebitNoteID,
                            CreatedDate = a.CreatedDate ?? new DateTime(),
                            DocumentAmount = a.DocumentAmount ?? 0,
                            AmountToBePayed = a.AmountToBePayed ?? 0,
                            DueDate = a.DueDate ?? new DateTime(),
                            CreditNoteID = a.CreditNoteID,
                            IRGID = a.IRGID,
                            SupplierName = a.SupplierName,
                            PaymentReturnVoucherTransID=(int)a.PaymentReturnVoucherTransID
                        }
                    ).ToList();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// Get Names by Category
        /// If Category is Supplier, Name will be Supplier names
        /// If category is Employee, Name will be Employee names
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetNameByCategory(int offset, int limit, string category, string search)
        {
            //return new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("31", "Name1" + search), new KeyValuePair<string, string>("19", "Name2" + search) };

            using (MasterEntities dbEntity = new MasterEntities())
            {
                if (category.ToLower().Equals("supplier"))
                {                   //Supplier selected

                    return dbEntity.SpGetSuppliersAutoComplete("All", search, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.Name)).ToList();

                }
                else if (category.ToLower().Equals("employee"))
                {                   //Employee Selected
                    return dbEntity.SpGetEmployeeAutoComplete(GeneralBO.LocationID, GeneralBO.ApplicationID, search, offset, limit).Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.Name)).ToList();
                }
            }
            return new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Get UnProcessed items by category
        /// If category is Supplier then get by supplier
        /// If category is Employee then get by employee
        /// </summary>
        /// <param name="category"></param>
        /// <param name="supplierID"></param>

        public List<ItemBO> GetItemByPurchaseOrder(int purchaseOrderID, string TransNo, string hint)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetItemByPurchaseOrderID(purchaseOrderID, TransNo, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, hint).Select(a => new ItemBO
                    {
                        ItemID = a.ItemID,
                        Name = a.ItemName,

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Save Advance Payment
        /// </summary>
        /// <param name="purchaseInvoiceBO"></param>
        /// <returns></returns>
        public int SaveAdvancePayment(AdvancePaymentBO advancePaymentBO, List<AdvancePaymentPurchaseOrderBO> advancePaymentPurchaseOrderBO)
        {

            int advancePaymentID = 0;
            if (advancePaymentBO != null && advancePaymentBO.ID <= 0)         //Create
                advancePaymentID = CreateAdvancePayment(advancePaymentBO, advancePaymentPurchaseOrderBO);
            else                                                                //Edit
                advancePaymentID = UpdateAdvancePayment(advancePaymentBO, advancePaymentPurchaseOrderBO);

            return advancePaymentID;
        }

        /// <summary>
        /// Create new Advance Payment
        /// </summary>
        /// <param name="advancePaymetBO"></param>
        /// <returns></returns>
        private int CreateAdvancePayment(AdvancePaymentBO advancePaymentBO, List<AdvancePaymentPurchaseOrderBO> advancePaymentPurchaseOrderBO)
        {
            int advancePaymentID = 0;
            using (var transaction = _entity.Database.BeginTransaction())
            {
                try
                {
                    var formname = "AdvancePayment";
                    if (advancePaymentBO.Draft)
                    {
                        formname = "DraftAdvancePayment";
                    }
                    ObjectParameter advanceIDOut = new ObjectParameter("AdvanceID", typeof(int));
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                    var j = _entity.SpUpdateSerialNo(formname, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    advancePaymentID = _entity.SpCreateAdvance(SerialNo.Value.ToString(), advancePaymentBO.Category, advancePaymentBO.AdvancePaymentDate, advancePaymentBO.SupplierID, advancePaymentBO.EmployeeID,
                           advancePaymentBO.ModeOfPaymentID, advancePaymentBO.BankDetail, advancePaymentBO.ReferenceNo, advancePaymentBO.Amt, advancePaymentBO.IsOfficial,
                           advancePaymentBO.IsPayment, advancePaymentBO.Draft, advancePaymentBO.NetAmount, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, advanceIDOut);

                    _entity.SaveChanges();

                    if (advanceIDOut.Value != null && Convert.ToInt32(advanceIDOut.Value) > 0)
                    {
                        advancePaymentID = Convert.ToInt32(advanceIDOut.Value);

                        foreach (var advancePaymentPurchaseOrder in advancePaymentPurchaseOrderBO)
                        {
                            if (advancePaymentPurchaseOrder.Amount > 0)
                            {
                                var result = _entity.SpCreateAdvanceDetails(
                                    advancePaymentID,
                                    advancePaymentPurchaseOrder.TransNo,
                                    advancePaymentPurchaseOrder.PurchaseOrderID,
                                    advancePaymentPurchaseOrder.PurchaseOrderDate,
                                    advancePaymentPurchaseOrder.PurchaseOrderTerms,
                                    advancePaymentPurchaseOrder.ItemID,
                                    advancePaymentPurchaseOrder.Amount,
                                    advancePaymentPurchaseOrder.TDSID,
                                    advancePaymentPurchaseOrder.TDSAmount,
                                    advancePaymentPurchaseOrder.Remarks,
                                    advancePaymentBO.Draft,
                                    advancePaymentPurchaseOrder.Advance,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    );
                            }

                        }
                    }
                    else
                    {
                        throw new DatabaseException("Total exceeds credit limit");
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    advancePaymentID = 0;
                    throw e;
                }

            }
            return advancePaymentID;
        }

        /// <summary>
        /// Update Advance payment
        /// </summary>
        /// <param name="advancePaymentBO"></param>
        /// <returns></returns>
        private int UpdateAdvancePayment(AdvancePaymentBO advancePaymentBO, List<AdvancePaymentPurchaseOrderBO> advancePaymentPurchaseOrderBO)

        {
            int advancePaymentID = advancePaymentBO.ID;
            using (var transaction = _entity.Database.BeginTransaction())
            {
                ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                try
                {
                    _entity.SpUpdateAdvancePayment(advancePaymentBO.ID,
                          advancePaymentBO.ModeOfPaymentID, advancePaymentBO.BankDetail, advancePaymentBO.ReferenceNo, advancePaymentBO.Amt,
                          advancePaymentBO.Draft, advancePaymentBO.NetAmount, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID, RetValue);

                    foreach (var advancePaymentPurchaseOrder in advancePaymentPurchaseOrderBO)
                    {
                        if (advancePaymentPurchaseOrder.Amount > 0)
                        {
                            var result = _entity.SpCreateAdvanceDetails(
                                advancePaymentID,
                                advancePaymentPurchaseOrder.TransNo,
                                advancePaymentPurchaseOrder.PurchaseOrderID,
                                advancePaymentPurchaseOrder.PurchaseOrderDate,
                                advancePaymentPurchaseOrder.PurchaseOrderTerms,
                                advancePaymentPurchaseOrder.ItemID,
                                advancePaymentPurchaseOrder.Amount,
                                advancePaymentPurchaseOrder.TDSID,
                                advancePaymentPurchaseOrder.TDSAmount,
                                advancePaymentPurchaseOrder.Remarks,
                                advancePaymentBO.Draft,
                                advancePaymentPurchaseOrder.Advance,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );
                        }
                    }
                    if (Convert.ToInt32(RetValue.Value) == -1)
                    {
                        throw new DatabaseException("Total exceeds credit limit");
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    advancePaymentID = 0;
                    throw e;
                }
            }
            return advancePaymentID;
        }

        public List<PayableDetailsBO> GetPayableDetailsForPaymentVoucherV3(int AccountHeadID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetPayableDetailsForPaymentVoucherV3(
                        AccountHeadID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new PayableDetailsBO()
                        {
                            DocumentType = a.DocumentType,
                            DocumentNo = a.DocumentNo,
                            PayableID = a.PayableID,
                            AdvanceID = a.AdvanceID,
                            DebitNoteID = a.DebitNoteID,
                            CreatedDate = a.CreatedDate ?? new DateTime(),
                            DocumentAmount = a.DocumentAmount ?? 0,
                            AmountToBePayed = a.AmountToBePayed ?? 0,
                            DueDate = a.DueDate ?? new DateTime(),
                            CreditNoteID = a.CreditNoteID,
                            IRGID = a.IRGID,
                            PaymentReturnVoucherTransID = (int)a.PaymentReturnVoucherTransID
                        }
                    ).ToList();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int SavePaymentVoucherV3(PaymentVoucherBO paymentVoucherBO)
        {
            ObjectParameter paymentIDOut = new ObjectParameter("PaymentID", typeof(int));
            ObjectParameter Returnvalue = new ObjectParameter("RetVal", typeof(int));

            int paymentID = 0;
            if (paymentVoucherBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {

                        string FormName = "PaymentVoucher";
                        int transactionID = 0;
                        int accountID = 0;
                        decimal paidAmount = (paymentVoucherBO != null && paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null) ?
                            paymentVoucherBO.UnProcessedPurchaseInvoiceItems.Sum(x => x.PayNow) : 0;
                        bool isSettled = false;
                        decimal settlementAmount = 0;

                        int paymentTypeID = 0;

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (paymentVoucherBO.IsDraft)
                        {
                            FormName = "DraftPaymentVoucher";
                        }
                        if (paymentVoucherBO.PaymentMode == "Cash")
                        {
                            _entity.SpUpdateSerialNo(FormName, "Cash", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        }
                        else
                        {
                            _entity.SpUpdateSerialNo(FormName, "Other", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        }
                        //var j = _entity.SpUpdateSerialNo("PaymentVoucher", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        var i = _entity.SpCreatePaymentV3(SerialNo.Value.ToString(), paymentVoucherBO.VoucherDate,
                            paymentVoucherBO.AccountHeadID, paymentVoucherBO.PaymentTypeID, paymentVoucherBO.BankName,
                            paymentVoucherBO.ReferenceNumber, paidAmount, paymentVoucherBO.Description, paymentVoucherBO.SaveType, isSettled,
                            settlementAmount, transactionID, accountID, paymentVoucherBO.CurrencyID, paymentVoucherBO.IsDraft, 
                            GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, 
                            paymentIDOut, paymentVoucherBO.ReceiverBankID, paymentVoucherBO.checqueDate, paymentVoucherBO.BankInstrumentNumber, paymentVoucherBO.Bankcharges, 
                            paymentVoucherBO.ReceiverBankName,
                            paymentVoucherBO.CurrencyCode,
                            paymentVoucherBO.LocalCurrencyID,
                            paymentVoucherBO.LocalCurrencyCode,
                            paymentVoucherBO.CurrencyExchangeRate,
                            paymentVoucherBO.LocalVoucherAmt);

                        _entity.SaveChanges();

                        if (paymentIDOut.Value != null && Convert.ToInt32(paymentIDOut.Value) > 0)
                        {
                            paymentID = Convert.ToInt32(paymentIDOut.Value);

                            if (paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null)
                            {
                                DateTime? settledDate = null;
                                decimal discountedAmount = 0;

                                foreach (var unProcessedInvoice in paymentVoucherBO.UnProcessedPurchaseInvoiceItems)
                                {
                                    if (unProcessedInvoice.PayNow != 0)
                                    {

                                        _entity.SpCreatePaymentDet(
                                            unProcessedInvoice.DocumentType,
                                            unProcessedInvoice.DocumentNo,
                                            unProcessedInvoice.AdvanceID,
                                            unProcessedInvoice.DebitNoteID,
                                            unProcessedInvoice.CreditNoteID,
                                            unProcessedInvoice.PayNow,
                                            discountedAmount,
                                            settledDate,
                                            unProcessedInvoice.PayableID,
                                            paymentID,
                                            unProcessedInvoice.IRGID,
                                            unProcessedInvoice.PaymentReturnVoucherTransID,
                                            unProcessedInvoice.AmountToBePayed,
                                            paymentVoucherBO.IsDraft,
                                            unProcessedInvoice.Narration,
                                            GeneralBO.CreatedUserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            Returnvalue);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Returnvalue = paymentIDOut;
                            throw new DatabaseException("Total exceeds credit limit");
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        paymentID = 0;
                        throw ex;
                    }
                }
            }
            return (int)Returnvalue.Value;
        }

        public int UpdatePaymentVoucherV3(PaymentVoucherBO paymentVoucherBO)
        {
            int paymentID = 0;
            ObjectParameter paymentIDOut = new ObjectParameter("PaymentID", typeof(int));
            ObjectParameter Returnvalue = new ObjectParameter("RetVal", typeof(int));

            if (paymentVoucherBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        decimal paidAmount = (paymentVoucherBO != null && paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null) ?
                            paymentVoucherBO.UnProcessedPurchaseInvoiceItems.Sum(x => x.PayNow) : 0;

                        var i = _entity.SpUpdatePayment(paymentVoucherBO.ID,
                            paymentVoucherBO.PaymentTypeID, paymentVoucherBO.BankName,
                            paymentVoucherBO.ReferenceNumber, paymentVoucherBO.Description, paidAmount, 
                            paymentVoucherBO.CurrencyID,
                            paymentVoucherBO.CurrencyCode,
                            paymentVoucherBO.LocalCurrencyID,
                            paymentVoucherBO.LocalCurrencyCode,
                            paymentVoucherBO.CurrencyExchangeRate,
                            paymentVoucherBO.LocalVoucherAmt,
                            GeneralBO.CreatedUserID, paymentVoucherBO.IsDraft, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, paymentIDOut);

                        _entity.SaveChanges();

                        if (Convert.ToInt32(paymentIDOut.Value) > 0)
                        {
                            if (paymentVoucherBO.UnProcessedPurchaseInvoiceItems != null)
                            {
                                DateTime? settledDate = null;
                                decimal discountedAmount = 0;
                                foreach (var unProcessedInvoice in paymentVoucherBO.UnProcessedPurchaseInvoiceItems)
                                {
                                    if (unProcessedInvoice.PayNow != 0)
                                    {
                                        _entity.SpCreatePaymentDet(
                                            unProcessedInvoice.DocumentType,
                                            unProcessedInvoice.DocumentNo,
                                            unProcessedInvoice.AdvanceID,
                                            unProcessedInvoice.DebitNoteID,
                                            unProcessedInvoice.CreditNoteID,
                                            unProcessedInvoice.PayNow,
                                            discountedAmount,
                                            settledDate,
                                            unProcessedInvoice.PayableID,
                                            paymentVoucherBO.ID,
                                            unProcessedInvoice.IRGID,
                                            unProcessedInvoice.PaymentReturnVoucherTransID,
                                            unProcessedInvoice.AmountToBePayed,
                                            paymentVoucherBO.IsDraft,
                                            unProcessedInvoice.Narration,
                                            GeneralBO.CreatedUserID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            Returnvalue);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Returnvalue = paymentIDOut;
                            throw new DatabaseException("Total exceeds credit limit");
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        paymentID = -1;
                        throw e;
                    }
                }
            }
            return (int)Returnvalue.Value;
        }

    }
}
