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
    public class BRSDAL
    {
        public List<BRSTransBO> getStatusAsPerBooks(DateTime FromTransactionDate, DateTime ToTransactionDate)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetStatusAsPerBooksForBRS(FromTransactionDate, ToTransactionDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BRSTransBO
                {
                    DocumentNumber = a.DocumentNo,
                    InstrumentNumber = a.InstrumentNumber,
                    InstrumentDate = Convert.ToDateTime(a.InstrumentDate),
                    Credit = Convert.ToDecimal(a.Credit),
                    Debit = Convert.ToDecimal(a.Debit),
                    BankCharges = Convert.ToDecimal(a.BankCharges)
                }).ToList();
            }
        }
        public string Save(BRSBO master, List<BRSTransBO> details, List<BankStatementBO> statements)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "BRS";
                        ObjectParameter BRSId = new ObjectParameter("BRSID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (master.IsDraft)
                        {
                            FormName = "DraftBRS";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateBRS(SerialNo.Value.ToString(), master.Date, master.BankID, master.FromTransactionDate, master.ToTransactionDate, master.AttachmentID, master.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, BRSId);

                        dbEntity.SaveChanges();

                        if (BRSId.Value != null)
                        {
                            foreach (var itm in details)
                            {
                                dbEntity.SpCreateBRSTrans(Convert.ToInt32(BRSId.Value), itm.DocumentNumber, itm.InstrumentNumber, itm.InstrumentDate, itm.Credit, itm.Debit, itm.BankCharges, 5, itm.EquivalentBankTransactionNumber, itm.Status, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);

                            }

                            foreach (var itm in statements)
                            {
                                dbEntity.SpCreateBRSBankTrans(Convert.ToInt32(BRSId.Value), itm.InstrumentNumber, itm.InstrumentDate, itm.Credit, itm.Debit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);

                            }

                        };

                        transaction.Commit();
                        return BRSId.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }
        public List<BRSBO> getBRSList()
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetBRSList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BRSBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    BankName = a.Bankname,
                    FromTransactionDate = (DateTime)a.FromTransactionDate,
                    ToTransactionDate = (DateTime)a.ToTransactionDate,
                    FileName = a.FileName,
                    IsDraft = (bool)a.IsDraft
                }).ToList();
            }
        }
        public List<BRSBO> getBRSDetails(int BRSID)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetBRSDetails(BRSID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BRSBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    BankID = (int)a.BankID,
                    BankName = a.BankName,
                    AttachmentID = (int)a.AttachmentID,
                    FromTransactionDate = (DateTime)a.FromTransactionDate,
                    ToTransactionDate = (DateTime)a.ToTransactionDate,
                    FileName = a.FileName,
                    IsDraft = (bool)a.IsDraft
                }).ToList();
            }
        }
        public List<BRSTransBO> getBRSTransDetails(int BRSID)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetBRSTransDetails(BRSID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BRSTransBO
                {
                    DocumentNumber = a.DocumentNo,
                    InstrumentNumber = a.InstrumentNo,
                    InstrumentDate = Convert.ToDateTime(a.InstrumentDate),
                    Credit = Convert.ToDecimal(a.Credit),
                    Debit = Convert.ToDecimal(a.Debit),
                    BankCharges = Convert.ToDecimal(a.BankCharge),
                    ItemName = a.ItemName,
                    EquivalentBankTransactionNumber = (int)a.EquivalentTransactionNo,
                    Status = a.Status
                }).ToList();
            }
        }
        public List<BankStatementBO> getBRSBankTransDetails(int BRSID)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetBRSBankTransDetails(BRSID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BankStatementBO
                {
                    InstrumentNumber = a.InstrumentNo,
                    InstrumentDate = Convert.ToDateTime(a.InstrumentDate),
                    Credit = Convert.ToDecimal(a.Credit),
                    Debit = Convert.ToDecimal(a.Debit),
                }).ToList();
            }
        }
        public bool UpdateBRS(BRSBO brs, List<BRSTransBO> brsTrans, List<BankStatementBO> statements)
        {
            bool IsSuccess = false;
            using (AccountsEntities entity = new AccountsEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    try
                    {
                        var j = entity.SpUpdateBRS(brs.ID, brs.Date, brs.BankID, brs.FromTransactionDate, brs.ToTransactionDate, brs.AttachmentID,
                            brs.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        foreach (var itm in brsTrans)
                        {
                            var i = entity.SpCreateBRSTrans(brs.ID, itm.DocumentNumber, itm.InstrumentNumber, itm.InstrumentDate, itm.Credit,
                                itm.Debit, itm.BankCharges, 5, itm.EquivalentBankTransactionNumber, itm.Status, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        }
                        foreach (var itm in statements)
                        {
                            var k = entity.SpCreateBRSBankTrans(brs.ID, itm.InstrumentNumber, itm.InstrumentDate, itm.Credit, itm.Debit
                               , GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        }
                        entity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                    }
                }
                return IsSuccess;
            }
        }
        public List<BRSTransBO> GetDataForBankReconciliation(int BankID, DateTime FromDate, DateTime ToDate)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetDataForBankReconciliation(BankID, FromDate, ToDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BRSTransBO
                {
                    DocumentNumber = a.DocumentNo,
                    InstrumentNumber = a.DocumentNo,
                    InstrumentDate = Convert.ToDateTime(a.DocumentDate),
                    Credit = Convert.ToDecimal(a.CreditAmount),
                    Debit = Convert.ToDecimal(a.DebitAmount),
                    BankCharges = 0,
                    DocumentType = a.DocumentType,
                    AccountName = a.AccountName,
                    ReferenceNo = a.ReferenceNo,
                    Remarks = a.Remarks,
                    ReconciledDate = Convert.ToDateTime(a.ReconciledDate),
                    DocumentID=a.DocumentID
                }).ToList();
            }
        }

        public string SaveBankReconciledDateV3(List<BRSTransBO> ItemList)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {

                        foreach (var itm in ItemList)
                        {
                            if (itm.DocumentType == "Payment Voucher")
                            {
                                dbEntity.SpUpdatePaymentReconciledDate(
                                                        itm.DocumentID,
                                                        itm.ReconciledDate,
                                                        itm.ReferenceNo,
                                                        itm.Remarks,
                                                        GeneralBO.CreatedUserID,
                                                        GeneralBO.FinYear,
                                                        GeneralBO.LocationID,
                                                        GeneralBO.ApplicationID
                                                        );
                            }
                            else
                            {
                                dbEntity.SpUpdateReceiptReconciledDate(
                                                             itm.DocumentID,
                                                             itm.ReconciledDate,
                                                             itm.Remarks,
                                                             itm.ReferenceNo,
                                                             GeneralBO.CreatedUserID,
                                                             GeneralBO.FinYear,
                                                             GeneralBO.LocationID,
                                                             GeneralBO.ApplicationID);
                            }
                        }

                        transaction.Commit();
                        return "Sucess";
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }

        public DatatableResultBO GetBRSListV3(string Type,string DocumentType, string DocumentNumber, string TransactionDate,string AccountName, string BankName, string DebitAmount, string CreditAmount, string ReconciledDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetBRSListV3(Type, DocumentType, DocumentNumber, TransactionDate, AccountName, BankName, DebitAmount, CreditAmount, ReconciledDate, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                DocumentType = item.DocumentType,
                                AccountName=item.AccountName,
                                DocumentDate = ((DateTime)item.DocumentDate).ToString("dd-MMM-yyyy"),
                                DocumentNo = item.DocumentNo,
                                DebitAmount = item.DebitAmount,
                                CreditAmount = item.CreditAmount,
                                Status = item.Type,
                                BankName = item.BankName,
                                BankReferanceNumber = item.ReferenceNo,
                                ReconciledDate = ((DateTime)item.ReconciledDate).ToString("dd-MMM-yyyy"),

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


        public List<BRSBO> GetTotalBalanceAmountDetailsForBankReconciliation(int BankID, DateTime FromDate, DateTime ToDate)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetTotalBalanceAmountDetailsForBankReconciliation(BankID, FromDate, ToDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BRSBO
                {
                    CreditAmountNotReflectedInBank=(decimal)a.CreditAmountNotReflectedInBank,
                    DebitAmountNotReflectedInBank=(decimal)a.DebitAmountNotReflectedInBank,
                    BalAsPerCompanyBooks=((decimal)a.DebitAmountReflectedInBank-(decimal)a.CreditAmountReflectedInBank),
                    BalAsPerBank = ((decimal)a.DebitAmountReflectedInBank - (decimal)a.CreditAmountReflectedInBank)+
                                   ((decimal)a.DebitAmountNotReflectedInBank - (decimal)a.CreditAmountNotReflectedInBank),
                }).ToList();
            }
        }

    }
}
