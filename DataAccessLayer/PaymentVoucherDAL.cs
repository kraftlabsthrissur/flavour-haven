//File Created by prama on 28-3-2018
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class PaymentVoucherDAL
    {
        //Code below by prama on 28-3-2018 for payment voucher List view
        public List<PaymentVoucherBO> GetPaymentVoucher(int ID)
        {
            List<PaymentVoucherBO> itm = new List<PaymentVoucherBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                itm = dbEntity.SpGetPaymentVoucherDetail(0, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentVoucherBO
                {
                    ID = a.ID,
                    VoucherNo = a.VoucherNo,
                    VoucherDate = (DateTime)a.VoucherDate,
                    SupplierName = a.SupplierName,
                    VoucherAmount = (decimal)a.VoucherAmount,
                    IsDraft=a.IsDraft
                }).ToList();
                return itm;
            }
        }

       
        public List<PaymentVoucherBO> GetPaymentVoucherDetail(int ID)
        {
            List<PaymentVoucherBO> itm = new List<PaymentVoucherBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                itm = dbEntity.SpGetPaymentVoucherDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentVoucherBO
                {
                    ID = a.ID,
                    VoucherNo = a.VoucherNo,
                    VoucherDate = (DateTime)a.VoucherDate,
                    SupplierName = a.SupplierName,
                    BankName = a.BankName,
                    ReferenceNumber = a.ReferenceNo,
                    Remark = a.Remark,
                    PaymentTypeName = a.PaymentTypeName,
                    IsDraft = a.IsDraft,
                    PaymentTypeID = (int)a.PaymentTypeID,
                    BankID =(int) a.BankID,
                    SupplierBankName = a.SupplierBankName,
                    SupplierBankACNo = a.SupplierBankACNo,
                    SupplierIFSCNo = a.SupplierIFSCNo,
                    VoucherAmount=(decimal)a.VoucherAmount
                }).ToList();
                return itm;
            }
        }


        public List<PaymentVoucherItemBO> GetPaymentVoucherTrans(int ID)
        {
            List<PaymentVoucherItemBO> itm = new List<PaymentVoucherItemBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                itm = dbEntity.SpGetPaymentVoucherTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentVoucherItemBO
                {
                    ID = a.PaymentDetID,
                    PaidAmount = (decimal)a.PaidAmount,
                    OrginalAmount = (decimal)a.DocumentAmount,
                    InvoiceNo = a.DocumentNo,
                    Date = (DateTime)a.SettledDate,
                    Balance = (decimal)a.AmountToBePaid,//(decimal)a.Balance,
                    DocumentType = a.DocumentType,
                    Narration=a.Narration
                }).ToList();
                return itm;
            }
        }

        public List<PaymentVoucherItemBO> GetPaymentVoucherTransForEdit(int ID)
        {
            List<PaymentVoucherItemBO> itm = new List<PaymentVoucherItemBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                itm = dbEntity.SpGetPaymentVoucherTransForEdit(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentVoucherItemBO
                {
                    ID = (int)a.PaymentDetID,
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
                    PaymentReturnVoucherTransID=a.PaymentReturnVoucherTransID,
                    SupplierName = a.SupplierName,
                    PaidAmount = (decimal)a.Amount,
                    Narration=a.Narration
                }).ToList();
                return itm;
            }
        }

        public List<PayableDetailsBO> GetDocumentAutoComplete(string term)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetDocumentNoAutocomplete(term, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.LocationID).Select(a => new PayableDetailsBO
                    {
                        ID = a.ID,
                        DocumentNo = a.DocumentNo

                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DatatableResultBO GetPaymentVoucherList(string Type, string VoucherNumber, string VoucherDate, string SupplierName, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetPaymentVoucherList(Type, VoucherNumber, VoucherDate, SupplierName, Amount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                VoucherNumber = item.VoucherNo,
                                VoucherDate=((DateTime)item.VoucherDate).ToString("dd-MMM-yyyy"),
                                SupplierName=item.SupplierName,
                                Amount=item.VoucherAmount,
                                Status=item.Status
                               
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

        public List<PaymentVoucherBO> GetPaymentVoucherDetailV3(int ID)
        {
            List<PaymentVoucherBO> itm = new List<PaymentVoucherBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                itm = dbEntity.SpGetPaymentVoucherDetailV3(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentVoucherBO
                {
                    ID = a.ID,
                    VoucherNo = a.VoucherNo,
                    VoucherDate = (DateTime)a.VoucherDate,
                    AccountHead = a.AccountName,
                    AccountHeadID = a.AccountHeadID,
                    BankName = a.BankName,
                    ReferenceNumber = a.ReferenceNo,
                    Remark = a.Remark,
                    PaymentTypeName = a.PaymentTypeName,
                    IsDraft = a.IsDraft,
                    PaymentTypeID = (int)a.PaymentTypeID,
                    BankID = (int)a.BankID,
                    VoucherAmount = (decimal)a.VoucherAmount,
                    ReconciledDate = (DateTime)a.ReconciledDate,
                    Currency=a.Currency,
                    CurrencyID=(int)a.CurrencyID,
                    supplierCurrencycode=a.supplierCurrencycode,
                    currencycode=a.currencycode,
                    SuuplierCurrencyconverion=(decimal)a.SuuplierCurrencyconverion,
                    AmountInWords=a.AmountInWords,
                    MinimumCurrency = a.MinimumCurrency,
                    MinimumCurrencyCode=a.MinimumCurrencyCode,
                    ReceiverBankName=a.ReceiverBankName,
                    BankInstrumentNumber=a.BankInstrumentNumber,
                    checqueDate=a.ChecqueDate,
                    Bankcharges= (decimal)a.BankCharges,
                    LocalCurrencyID = (int)a.LocalCurrencyID,
                    LocalCurrencyCode = a.LocalCurrencyCode,
                    CurrencyExchangeRate = (decimal)a.CurrencyExchangeRate,
                    LocalVoucherAmt = (decimal)a.LocalNetAmt,




                }).ToList();
                return itm;
            }
        }

        public List<PaymentVoucherItemBO> GetPaymentVoucherTransForEditV3(int ID)
        {
            List<PaymentVoucherItemBO> itm = new List<PaymentVoucherItemBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                itm = dbEntity.SpGetPaymentVoucherTransForEditV3(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentVoucherItemBO
                {
                    ID = (int)a.PaymentDetID,
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
                    PaymentReturnVoucherTransID = a.PaymentReturnVoucherTransID,
                    PaidAmount = (decimal)a.Amount,
                    Narration = a.Narration
                }).ToList();
                return itm;
            }
        }

        public DatatableResultBO GetPaymentVoucherListV3(string Type, string VoucherNumber, string VoucherDate, string AccountHead, string Amount,string ReconciledDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetPaymentVoucherListV3(Type, VoucherNumber, VoucherDate, AccountHead, Amount, ReconciledDate, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                VoucherNumber = item.VoucherNo,
                                VoucherDate = ((DateTime)item.VoucherDate).ToString("dd-MMM-yyyy"),
                                AccountHead = item.AccountHead,
                                Amount = item.VoucherAmount,
                                Status = item.Status,
                                BankName = item.BankName,
                                PaymentTypeName = item.PaymentTypeName,
                                Remarks = item.Remark,
                                BankReferanceNumber = item.ReferenceNo,
                                ReconciledDate= ((DateTime)item.ReconciledDate).ToString("dd-MMM-yyyy"),

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

        public List<PaymentVoucherItemBO> GetPaymentVoucherTransV3(int ID)
        {
            List<PaymentVoucherItemBO> itm = new List<PaymentVoucherItemBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                itm = dbEntity.SpGetPaymentVoucherTransV3(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentVoucherItemBO
                {
                    ID = a.PaymentDetID,
                    PaidAmount = (decimal)a.PaidAmount,
                    //  DocumentAmount = (decimal)a.DocumentAmount,
                    OrginalAmount = (decimal)a.DocumentAmount,
                    InvoiceNo = a.DocumentNo,
                    Date = (DateTime)a.SettledDate,
                    //AmountToBePayed = (decimal)a.AmountToBePaid,//(decimal)a.Balance,
                    Balance = (decimal)a.AmountToBePaid,
                    DocumentType = a.DocumentType,
                    Narration = a.Narration
                }).ToList();
                return itm;
            }
        }

        public int SaveReconciledDate(int ID, DateTime ReconciledDate, string BankReferanceNumber, string Remarks)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        var i = dEntity.SpUpdatePaymentReconciledDate(
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
