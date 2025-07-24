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
    public class FundTransferReceiptDAL
    {
        public DatatableResultBO GetFundTransferIssueList(int IssueLocationID, string IssueTransNoHint, string IssueLocationHint, string IssueBankDetailsHint, string ReceiptLocationHint, string ReceiptBankDetailsHint, string ModeOfPaymentHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {

                    var result = dbEntity.SpGetFundTransferIssueList(IssueLocationID, IssueTransNoHint, IssueLocationHint, IssueBankDetailsHint, ReceiptLocationHint, ReceiptBankDetailsHint, ModeOfPaymentHint, AmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransNo = item.TransNo,
                                FromLocationID = item.FromLocationID,
                                ToLocation = item.ToLocationID,
                                FromBankID = item.FromBankID,
                                ToBankID = item.ToBankID,
                                FromLocationName = item.FromLocationName,
                                ToLocationName = item.ToLocationName,
                                FromBankName = item.FromBankName,
                                ToBankName = item.ToBankName,
                                Amount = item.Amount,
                                ModeOfPayment = item.ModeOfPayment,
                                Payment = item.PaymentMode
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

        public List<FundTransferReceiptBO> GetTransferIssuedItems(int IssueID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetFundTransferIssueItems(IssueID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new FundTransferReceiptBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNo,
                        FromLocationID = (int)a.FromLocationID,
                        ToLocationID = (int)a.ToLocationID,
                        FromBankID = (int)a.FromBankID,
                        ToBankID = (int)a.ToBankID,
                        FromLocationName = a.FromLocationName,
                        ToLocationName = a.ToLocationName,
                        FromBankName = a.FromBankName,
                        ToBankName = a.ToBankName,
                        Amount = (decimal)a.Amount,
                        ModeOfPayment = (int)a.ModeOfPayment,
                        Payment = a.PaymentMode,
                        InstrumentDate = (DateTime)a.InstrumentDate,
                        InstrumentNumber = a.InstrumentNumber,
                        Remarks = a.Remarks

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Save(FundTransferReceiptBO ReceiptBO, List<FundTransferItemBO> Items)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    ObjectParameter ReturnValue = new ObjectParameter("ReceiptID", typeof(int));
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    var j = dbEntity.SpUpdateSerialNo("FundTransferReceipt", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    var i = dbEntity.SpCreateFundTransferReceipt(
                               ReceiptBO.ID,
                               SerialNo.Value.ToString(),
                               ReceiptBO.TransDate,
                               GeneralBO.FinYear,
                               GeneralBO.LocationID,
                               GeneralBO.ApplicationID,
                               ReturnValue
                            );
                    foreach (var item in Items)
                    {
                        dbEntity.SpCreateFundTransferReceiptTransDetails(
                              Convert.ToInt32(ReturnValue.Value),
                              item.IssueTransID,
                              item.FromLocationID,
                              item.FromBankID,
                              item.ToLocationID,
                              item.ToBankID,
                              item.ModeOfPayment,
                              item.Amount,
                              GeneralBO.FinYear,
                              GeneralBO.LocationID,
                              GeneralBO.ApplicationID
                                );
                    }

                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<FundTransferReceiptBO> GetFundTransferReceiptByID(int ID)
        {
            try
            {
                List<FundTransferReceiptBO> Receipt = new List<FundTransferReceiptBO>();

                using (AccountsEntities dEntity = new AccountsEntities())
                {
                    Receipt = dEntity.SpGetFundTransferReceiptByID(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new FundTransferReceiptBO
                    {
                        TransDate = (DateTime)a.TransDate,
                        TransNo = a.TransNo
                    }).ToList();
                    return Receipt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FundTransferItemBO> GetFundTransferReceiptTransByID(int ID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetFundTransferReceiptTransByID(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new FundTransferItemBO()
                    {
                        FromLocationName = a.FromLocation,
                        ToLocationName = a.ToLocation,
                        Payment = a.PaymentMode,
                        Amount = (decimal)a.Amount,
                        FromBankName = a.FromBankName,
                        ToBankName = a.ToBankName,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FundTransferReceiptBO> GetFundTransferReceiptList()
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                return dEntity.SpGetFundTransferReceiptList(GeneralBO.LocationID).Select(a => new FundTransferReceiptBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    TransDate = (DateTime)a.TransDate,
                    FromLocationName = a.FromLocation,
                    ToLocationName = a.ToLocation,
                    Payment = a.PaymentMode,
                    Amount = (decimal)a.Amount
                }).ToList();

            }
        }

        public DatatableResultBO GetFundTransferReceipt(string FundTransferNo, string FundTransferDate, string FromLocation, string ToLocation, string ModeOfPayment, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetFundTransferReceiptListForDataTable(FundTransferNo, FundTransferDate, FromLocation, ToLocation, ModeOfPayment, TotalAmount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                FundTransferNo = item.TransNo,
                                FundTransferDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                FromLocation = item.FromLocation,
                                ToLocation = item.ToLocation,
                                ModeOfPayment = item.PaymentMode,
                                TotalAmount = item.Amount
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }
    }
}

