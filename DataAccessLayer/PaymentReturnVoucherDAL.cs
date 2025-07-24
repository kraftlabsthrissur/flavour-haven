using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class PaymentReturnVoucherDAL
    {
        public bool Save(PaymentReturnVoucherBO Master, List<PaymentReturnVoucherItemBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PaymentReturnVoucher";
                        ObjectParameter PaymentReturnID = new ObjectParameter("PaymentReturnID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (Master.IsDraft)
                        {
                            FormName = "DraftPaymentReturnVoucher";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreatePaymentReturnVoucher(
                            SerialNo.Value.ToString(),
                            Master.VoucherDate,
                            Master.SupplierID,
                            Master.PaymentTypeID,
                            Master.BankID,
                            Master.BankReferenceNumber,
                            Master.IsDraft,
                            Master.DebitNoteID,
                            Master.DebitAccountCode,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            PaymentReturnID);

                        dbEntity.SaveChanges();

                        if (PaymentReturnID.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreatePaymentReturnVoucherTrans(
                                    Convert.ToInt32(PaymentReturnID.Value),
                                    itm.Amount,
                                    itm.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    RetValue);
                            }
                        };
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Total exceeds credit limit");
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

        public bool Update(PaymentReturnVoucherBO Master, List<PaymentReturnVoucherItemBO> Details)
        {
            bool IsSuccess = false;
            using (AccountsEntities entity = new AccountsEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                    try
                    {
                        var j = entity.SpUpdatePaymentReturnVoucher(
                            Master.ID,
                            Master.VoucherDate,
                            Master.PaymentTypeID,
                            Master.BankID,
                            Master.BankReferenceNumber,
                            Master.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID);
                        foreach (var itm in Details)
                        {
                            var i = entity.SpCreatePaymentReturnVoucherTrans(
                                Master.ID,
                                itm.Amount,
                                itm.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                RetValue);
                        }
                        entity.SaveChanges();
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Total exceeds credit limit");
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                return IsSuccess;
            }
        }

        public List<PaymentReturnVoucherBO> GetPaymentReturnVoucherDetails(int PaymentReturnID)
        {
            List<PaymentReturnVoucherBO> list = new List<PaymentReturnVoucherBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetPaymentReturnVoucherDetails(PaymentReturnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentReturnVoucherBO
                {
                    ID = (int)a.ID,
                    VoucherNo = a.VoucherNo,
                    VoucherDate = (DateTime)a.VoucherDate,
                    SupplierName = a.SupplierName,
                    SupplierID = a.SupplierID,
                    PaymentTypeName = a.PaymentTypeName,
                    PaymentTypeID = a.PaymentTypeID,
                    BankName = a.BankName,
                    BankID = a.BankID,
                    BankReferenceNumber = a.BankReferenceNumber,
                    IsDraft = (bool)a.IsDraft,
                    SupplierBankName = a.SupplierBankName,
                    SupplierBankACNo = a.SupplierBankACNo,
                    SupplierIFSCNo = a.SupplierIFSCNo
                }).ToList();
                return list;
            }
        }

        public List<PaymentReturnVoucherItemBO> GetPaymentReturnTransDetails(int PaymentReturnID)
        {
            List<PaymentReturnVoucherItemBO> list = new List<PaymentReturnVoucherItemBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {

                list = dEntity.SpGetPaymentReturnVoucherTransDetails(PaymentReturnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PaymentReturnVoucherItemBO
                {
                    Amount = (decimal)a.Amount,
                    Remarks = a.Remarks,
                }).ToList();
                return list;
            }
        }

        public DatatableResultBO GetPaymentReturnVoucherList(string Type, string VoucherNoHint, string VoucherDateHint, string SupplierNameHint, string PaymentHint, string ReturnAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetPaymentReturnVoucherListForDatatable(Type, VoucherNoHint, VoucherDateHint, SupplierNameHint, PaymentHint, ReturnAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                VoucherNo = item.VoucherNo,
                                VoucherDate = ((DateTime)item.VoucherDate).ToString("dd-MMM-yyyy"),
                                SupplierName = item.SupplierName,
                                PaymentTypeName = item.PaymentTypeName,
                                Amount = item.Amount,
                                Status = item.Status,

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

        public List<DebitNoteBO> GetDebitNoteListForPaymentReturn(int SupplierID)
        {
            List<DebitNoteBO> itm = new List<DebitNoteBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetDebitNoteListForPaymentReturn(SupplierID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new DebitNoteBO()
                    {
                        ID = m.ID,
                        TransNo = m.TransNo,
                        TransDate = (DateTime)m.TransDate,
                        DebitAccount = m.DebitAccount,
                        DebitAccountCode = m.DebitAccountCode,
                        Amount = (decimal)m.Amount,

                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
