using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class ReceiptVoucherDAL
    {
        public bool Save(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, string Settlements)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {

                        string FormName = "ReceiptVoucher";
                        ObjectParameter ReceiptID = new ObjectParameter("ReceiptVoucherID", typeof(int));

                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (receiptVoucherBO.IsDraft)
                        {
                            FormName = "DraftReceiptVoucher";
                        }

                        var j = dEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dEntity.SpCreateReceiptVoucher(
                                    SerialNo.Value.ToString(),
                                    receiptVoucherBO.ReceiptDate,
                                    receiptVoucherBO.CustomerID,
                                    receiptVoucherBO.ReceiptAmount,
                                    receiptVoucherBO.PaymentTypeID,
                                    receiptVoucherBO.BankID,
                                    receiptVoucherBO.Date,
                                    receiptVoucherBO.BankReferanceNumber,
                                    receiptVoucherBO.Remarks,
                                    receiptVoucherBO.IsDraft,
                                    Settlements,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    ReceiptID,
                                      receiptVoucherBO.ReceiverBankName,
                                     receiptVoucherBO.BankInstrumentNumber,
                                    receiptVoucherBO.checqueDate);

                        if (ReceiptID.Value != null)
                        {
                            foreach (var itm in receiptItemBO)
                            {
                                dEntity.SpCreateReceiptVoucherTrans(
                                          itm.CreditNoteID,
                                          itm.DebitNoteID,
                                          receiptVoucherBO.ReceiptDate,
                                          receiptVoucherBO.CustomerID,
                                          receiptVoucherBO.PaymentTypeID,
                                          receiptVoucherBO.BankID,
                                          receiptVoucherBO.BankReferanceNumber,
                                          itm.AdvanceReceivedAmount,
                                          Convert.ToInt32(ReceiptID.Value),
                                          itm.ReceivableID,
                                          itm.AdvanceID,
                                          itm.DocumentType,
                                          itm.DocumentNo,
                                          itm.ReceivableDate,
                                          itm.Amount,
                                          itm.Balance,
                                          itm.AmountToBeMatched,
                                          itm.Status,
                                          itm.PendingDays,
                                          itm.SalesReturnID,
                                          itm.CustomerReturnVoucherID,
                                          GeneralBO.FinYear,
                                          GeneralBO.LocationID,
                                          GeneralBO.ApplicationID,
                                          RetValue
                                          );
                                if (Convert.ToInt32(RetValue.Value) == -1)
                                {
                                    throw new Exception("Already settled");
                                }
                            }
                        };
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public bool Update(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, string Settlements)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        ObjectParameter ReceiptID = new ObjectParameter("ReceiptVoucherID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));

                        var i = dEntity.SpUpdateReceiptVoucher(
                             receiptVoucherBO.ID,
                             receiptVoucherBO.ReceiptDate,
                             receiptVoucherBO.CustomerID,
                             receiptVoucherBO.ReceiptAmount,
                             receiptVoucherBO.PaymentTypeID,
                             receiptVoucherBO.BankID,
                             receiptVoucherBO.Date,
                             receiptVoucherBO.BankReferanceNumber,
                             receiptVoucherBO.Remarks,
                             receiptVoucherBO.IsDraft,
                             Settlements,
                             GeneralBO.CreatedUserID,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID
                          );

                        foreach (var itm in receiptItemBO)
                        {

                            dEntity.SpCreateReceiptVoucherTrans(
                                              itm.CreditNoteID,
                                              itm.DebitNoteID,
                                              receiptVoucherBO.ReceiptDate,
                                              receiptVoucherBO.CustomerID,
                                              receiptVoucherBO.PaymentTypeID,
                                              receiptVoucherBO.BankID,
                                              receiptVoucherBO.BankReferanceNumber,
                                              itm.AdvanceReceivedAmount,
                                              receiptVoucherBO.ID,
                                              itm.ReceivableID,
                                              itm.AdvanceID,
                                              itm.DocumentType,
                                              itm.DocumentNo,
                                              itm.ReceivableDate,
                                              itm.Amount,
                                              itm.Balance,
                                              itm.AmountToBeMatched,
                                              itm.Status,
                                              itm.PendingDays,
                                              itm.SalesReturnID,
                                              itm.CustomerReturnVoucherID,
                                              GeneralBO.FinYear,
                                              GeneralBO.LocationID,
                                              GeneralBO.ApplicationID,
                                              RetValue

                                          );
                            if (Convert.ToInt32(RetValue.Value) == -1)
                            {
                                throw new Exception("Already settled");
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<ReceiptVoucherBO> GetReceiptList()
        {
            List<ReceiptVoucherBO> list = new List<ReceiptVoucherBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceiptVoucher(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptVoucherBO
                {
                    ID = a.ID,
                    ReceiptNo = a.VoucherNo,
                    ReceiptDate = (DateTime)a.VoucherDate,
                    ReceiptAmount = (decimal)a.ReceiptAmount,
                    CustomerName = a.Customer,
                    BankName = a.BankName,
                    PaymentTypeName = a.Mode,
                    Remarks = a.Remarks,
                    BankReferanceNumber = a.ReferenceNo,
                    IsDraft = (bool)a.IsDraft,



                }).ToList();


                return list;
            }

        }

        public ReceiptVoucherBO GetReceiptDetails(int ID)
        {
            ReceiptVoucherBO ReceiptVoucher = new ReceiptVoucherBO();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                ReceiptVoucher = dEntity.SpGetReceiptVoucherDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptVoucherBO
                {
                    ID = a.ID,
                    ReceiptNo = a.VoucherNo,
                    ReceiptDate = (DateTime)a.VoucherDate,
                    ReceiptAmount = (decimal)a.ReceiptAmount,
                    CustomerName = a.Customer.Trim(),
                    BankName = a.BankName,
                    BankID = (int)a.BankID,
                    PaymentTypeID = (int)a.PaymentTypeID,
                    PaymentTypeName = a.Mode,
                    Remarks = a.Remarks,
                    BankReferanceNumber = a.ReferenceNo,
                    IsDraft = (bool)a.IsDraft,
                    CustomerID = (int)a.CustomerID,
                    Date = a.IDate,
                }).FirstOrDefault();
            }
            return ReceiptVoucher;
        }

        public List<ReceiptItemBO> GetReceiptTrans(int ID)
        {
            List<ReceiptItemBO> list = new List<ReceiptItemBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceiptVoucherTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptItemBO
                {
                    DocumentNo = a.DocumentNo,
                    DocumentType = a.DocumentType,
                    ReceivableDate = (DateTime)a.ReceivableDate,
                    Amount = (decimal)a.Amount,
                    Balance = (decimal)a.Balance,
                    AmountToBeMatched = (decimal)a.AmountToBeMatched,
                    ReceivableID = (int)a.ReceivableID,
                    VoucherID = (int)a.ReceiptVoucherID,
                    Status = a.Status,
                    CreditNoteID = (int)a.CreditNoteID,
                    DebitNoteID = (int)a.DebitNoteID,
                    AdvanceID = (int)a.AdvanceReceiptID,
                    PendingDays = (int)a.PendingDays

                }).ToList();
            }
            return list;

        }

        public List<ReceiptItemBO> GetReceiptTransForEdit(int ID)
        {
            List<ReceiptItemBO> list = new List<ReceiptItemBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceiptVoucherTransForEdit(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptItemBO
                {
                    DocumentNo = a.DocumentNo,
                    DocumentType = a.DocumentType,
                    ReceivableDate = (DateTime)a.ReceivableDate,
                    Amount = (decimal)a.Amount,
                    Balance = (decimal)a.Balance,
                    AmountToBeMatched = (decimal)a.AmountToBeMatched,
                    ReceivableID = (int)a.ReceivableID,
                    VoucherID = (int)a.ReceiptVoucherID,
                    Status = a.Status,
                    CreditNoteID = (int)a.CreditNoteID,
                    DebitNoteID = (int)a.DebitNoteID,
                    AdvanceID = (int)a.AdvanceReceiptID,
                    ClassType = a.ClassType,
                    PendingDays = (int)a.PendingDays,
                    SalesReturnID = a.SalesReturnID,
                    CustomerReturnVoucherID = (int)a.CustomerReturnVoucherID
                }).ToList();
                return list;
            }

        }

        public List<SalesInvoiceBO> GetInvoiceForReceiptVoucher(int CustomerID)
        {
            List<SalesInvoiceBO> list = new List<SalesInvoiceBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceivables(CustomerID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new SalesInvoiceBO
                {
                    InvoiceNo = m.DocumentNo,
                    InvoiceDate = (DateTime)m.TransDate,
                    NetAmount = (decimal)m.ReceivableAmount,
                    Balance = (decimal)m.Balance,
                    InvoiceType = m.DocumentType,
                    ID = m.ID,
                    CreditNoteID = m.CreditNoteID,
                    DebitNoteID = m.DebitNoteID,
                    SalesReturnID = m.SalesReturnID
                }).ToList();
                return list;
            }

        }

        public DatatableResultBO GetReceiptVoucherList(string Type, string ReceiptNoHint, string InvoiceDateHint, string CustomerHint, string ReceiptAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetReceiptVoucherList(Type, ReceiptNoHint, InvoiceDateHint, CustomerHint, ReceiptAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                ReceiptNo = item.VoucherNo,
                                ReceiptDate = ((DateTime)item.VoucherDate).ToString("dd-MMM-yyyy"),
                                ReceiptAmount = (decimal)item.ReceiptAmount,
                                CustomerName = item.Customer,
                                BankName = item.BankName,
                                PaymentTypeName = item.Mode,
                                Remarks = item.Remarks,
                                BankReferanceNumber = item.ReferenceNo,
                                IsDraft = (bool)item.IsDraft,
                                Status = item.Status,
                                CustomerID = item.CustomerID

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public List<ReceiptItemBO> GetAdvanceReceiptTrans(int CustomerID)
        {
            List<ReceiptItemBO> list = new List<ReceiptItemBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetAdvanceReceiptTrans(CustomerID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptItemBO
                {
                    DocumentType = a.DocumentType,
                    ReceivableDate = (DateTime)a.AdvanceDate,
                    Amount = (decimal)a.Amount

                }).ToList();
            }
            return list;

        }

        //created for version3

        public bool SaveV3(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, string Settlements)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {

                        string FormName = "ReceiptVoucher";
                        ObjectParameter ReceiptID = new ObjectParameter("ReceiptVoucherID", typeof(int));

                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        ObjectParameter AccountEntryMasterID = new ObjectParameter("AccountEntryMasterID", typeof(string));

                        if (receiptVoucherBO.IsDraft)
                        {
                            FormName = "DraftReceiptVoucher";
                        }

                        var j = dEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dEntity.SpCreateReceiptVoucherV3(
                                    SerialNo.Value.ToString(),
                                    (DateTime)receiptVoucherBO.ReceiptDate,
                                    receiptVoucherBO.CustomerID,
                                    receiptVoucherBO.AccountHeadID,
                                    receiptVoucherBO.BankID,
                                    receiptVoucherBO.ReceiptAmount,
                                    receiptVoucherBO.CurrencyID,
                                    receiptVoucherBO.PaymentTypeID,
                                    receiptVoucherBO.BankID,
                                    receiptVoucherBO.Date,
                                    receiptVoucherBO.BankReferanceNumber,
                                    receiptVoucherBO.Remarks,
                                    receiptVoucherBO.IsDraft,
                                    Settlements,
                                   
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    ReceiptID,
                                     receiptVoucherBO.BankInstrumentNumber,
                                      receiptVoucherBO.checqueDate,
                                     receiptVoucherBO.ReceiverBankID
                                   
                                   
                                   
                                    );

                        if (ReceiptID.Value != null)
                        {
                            foreach (var itm in receiptItemBO)
                            {
                                dEntity.SpCreateReceiptVoucherTransV3(
                                          itm.CreditNoteID,
                                          itm.DebitNoteID,
                                          receiptVoucherBO.ReceiptDate,
                                          receiptVoucherBO.CustomerID,
                                          receiptVoucherBO.PaymentTypeID,
                                          receiptVoucherBO.BankID,
                                          receiptVoucherBO.BankReferanceNumber,
                                          itm.AdvanceReceivedAmount,
                                          Convert.ToInt32(ReceiptID.Value),
                                          itm.ReceivableID,
                                          itm.AdvanceID,
                                          itm.DocumentType,
                                          itm.DocumentNo,
                                          itm.ReceivableDate,
                                          itm.Amount,
                                          itm.Balance,
                                          itm.AmountToBeMatched,
                                          itm.Status,
                                          itm.PendingDays,
                                          itm.SalesReturnID,
                                          itm.CustomerReturnVoucherID,
                                          GeneralBO.FinYear,
                                          GeneralBO.LocationID,
                                          GeneralBO.ApplicationID,
                                          RetValue
                                          );
                                if (Convert.ToInt32(RetValue.Value) == -1)
                                {
                                    throw new Exception("Already settled");
                                }
                            }
                        };
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }

        }

        public DatatableResultBO GetReceiptVoucherListV3(string Type, string ReceiptNoHint, string InvoiceDateHint, string AccountHeadHint, string ReceiptAmountHint, string ReconciledDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetReceiptVoucherListV3(Type, ReceiptNoHint, InvoiceDateHint, AccountHeadHint, ReceiptAmountHint, ReconciledDateHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                ReceiptNo = item.VoucherNo,
                                ReceiptDate = ((DateTime)item.VoucherDate).ToString("dd-MMM-yyyy"),
                                ReceiptAmount = (decimal)item.ReceiptAmount,
                                AccountHead = item.AccountHead,
                                BankName = item.BankName,
                                PaymentTypeName = item.Mode,
                                Remarks = item.Remarks,
                                BankReferanceNumber = item.ReferenceNo,
                                IsDraft = (bool)item.IsDraft,
                                Status = item.Status,
                                AccountHeadID = item.AccountHeadID,
                                ReconciledDate=((DateTime)item.ReconciledDate).ToString("dd-MMM-yyyy"),
                                Mode=item.Mode

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public ReceiptVoucherBO GetReceiptDetailsV3(int ID)
        {
            ReceiptVoucherBO ReceiptVoucher = new ReceiptVoucherBO();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                ReceiptVoucher = dEntity.SpGetReceiptVoucherDetailV3(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptVoucherBO
                {
                    ID = a.ID,
                    ReceiptNo = a.VoucherNo,
                    ReceiptDate = (DateTime)a.VoucherDate,
                    ReceiptAmount = (decimal)a.ReceiptAmount,
                    AccountHead = a.AccountHead.Trim(),
                    BankName = a.BankName,
                    BankID = (int)a.BankID,
                    PaymentTypeID = (int)a.PaymentTypeID,
                    PaymentTypeName = a.Mode,
                    Remarks = a.Remarks,
                    BankReferanceNumber = a.ReferenceNo,
                    IsDraft = (bool)a.IsDraft,
                    AccountHeadID = (int)a.AccountHeadID,
                    Date = a.IDate,
                    ReconciledDate=(DateTime)a.ReconciledDate,
                    Currency = a.Currency,
                    CurrencyID = (int)a.CurrencyID,
                    supplierCurrencycode = a.supplierCurrencycode,
                    SuuplierCurrencyconverion = a.SuuplierCurrencyconverion,
                    currencycode = a.currencycode,
                    CalculatedAmount =(decimal) a.CalculatedAmount,
                    AmountInWords = a.AmountInWords,
                    ReceiverBankName= a.ReceiverBankName,
                    BankInstrumentNumber = a.BankInstrumentNumber,
                    checqueDate = a.ChecqueDate
                    
                }).FirstOrDefault();
            }
            return ReceiptVoucher;
        }

        public List<ReceiptItemBO> GetReceiptTransV3(int ID)
        {
            List<ReceiptItemBO> list = new List<ReceiptItemBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceiptVoucherTransV3(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptItemBO
                {
                    DocumentNo = a.DocumentNo,
                    DocumentType = a.DocumentType,
                    ReceivableDate = (DateTime)a.ReceivableDate,
                    Amount = (decimal)a.Amount,
                    Balance = (decimal)a.Balance,
                    AmountToBeMatched = (decimal)a.AmountToBeMatched,
                    ReceivableID = (int)a.ReceivableID,
                    VoucherID = (int)a.ReceiptVoucherID,
                    Status = a.Status,
                    CreditNoteID = (int)a.CreditNoteID,
                    DebitNoteID = (int)a.DebitNoteID,
                    AdvanceID = (int)a.AdvanceReceiptID,
                    PendingDays = (int)a.PendingDays

                }).ToList();
            }
            return list;

        }

        public List<ReceiptItemBO> GetReceiptTransForEditV3(int ID)
        {
            List<ReceiptItemBO> list = new List<ReceiptItemBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceiptVoucherTransForEditV3(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReceiptItemBO
                {
                    DocumentNo = a.DocumentNo,
                    DocumentType = a.DocumentType,
                    ReceivableDate = (DateTime)a.ReceivableDate,
                    Amount = (decimal)a.Amount,
                    Balance = (decimal)a.Balance,
                    AmountToBeMatched = (decimal)a.AmountToBeMatched,
                    ReceivableID = (int)a.ReceivableID,
                    VoucherID = (int)a.ReceiptVoucherID,
                    Status = a.Status,
                    CreditNoteID = (int)a.CreditNoteID,
                    DebitNoteID = (int)a.DebitNoteID,
                    AdvanceID = (int)a.AdvanceReceiptID,
                    ClassType = a.ClassType,
                    PendingDays = (int)a.PendingDays,
                    SalesReturnID = a.SalesReturnID,
                    CustomerReturnVoucherID = (int)a.CustomerReturnVoucherID,
               

                }).ToList();
                return list;
            }

        }

        public bool UpdateV3(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, string Settlements)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        ObjectParameter ReceiptID = new ObjectParameter("ReceiptVoucherID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));

                        var i = dEntity.SpUpdateReceiptVoucherV3(
                             receiptVoucherBO.ID,
                             receiptVoucherBO.ReceiptDate,
                             receiptVoucherBO.CustomerID,
                             receiptVoucherBO.AccountHeadID,
                             receiptVoucherBO.ReceiptAmount,
                             receiptVoucherBO.CurrencyID,
                             receiptVoucherBO.PaymentTypeID,
                             receiptVoucherBO.BankID,
                             receiptVoucherBO.Date,
                             receiptVoucherBO.BankReferanceNumber,
                             receiptVoucherBO.Remarks,
                             receiptVoucherBO.IsDraft,
                             Settlements,
                             GeneralBO.CreatedUserID,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID
                          );

                        foreach (var itm in receiptItemBO)
                        {

                            dEntity.SpCreateReceiptVoucherTransV3(
                                              itm.CreditNoteID,
                                              itm.DebitNoteID,
                                              receiptVoucherBO.ReceiptDate,
                                              receiptVoucherBO.CustomerID,
                                              receiptVoucherBO.PaymentTypeID,
                                              receiptVoucherBO.BankID,
                                              receiptVoucherBO.BankReferanceNumber,
                                              itm.AdvanceReceivedAmount,
                                              receiptVoucherBO.ID,
                                              itm.ReceivableID,
                                              itm.AdvanceID,
                                              itm.DocumentType,
                                              itm.DocumentNo,
                                              itm.ReceivableDate,
                                              itm.Amount,
                                              itm.Balance,
                                              itm.AmountToBeMatched,
                                              itm.Status,
                                              itm.PendingDays,
                                              itm.SalesReturnID,
                                              itm.CustomerReturnVoucherID,
                                              GeneralBO.FinYear,
                                              GeneralBO.LocationID,
                                              GeneralBO.ApplicationID,
                                              RetValue

                                          );
                            if (Convert.ToInt32(RetValue.Value) == -1)
                            {
                                throw new Exception("Already settled");
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int SaveReconciledDate(int ID,DateTime ReconciledDate,string BankReferanceNumber, string Remarks)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {

                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));

                        var i = dEntity.SpUpdateReceiptReconciledDate(
                             ID,
                             ReconciledDate,
                             BankReferanceNumber,
                             Remarks,
                             GeneralBO.CreatedUserID,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID
                          );
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }


    }
}
