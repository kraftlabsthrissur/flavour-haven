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
    public class JournalDAL
    {
        public List<JournalBO> GetCreditAccountAutoComplete(string AccountNameHint, string AccountCodeHint)
        {
            List<JournalBO> AccountHead = new List<JournalBO>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    AccountHead = dbEntity.SpGetAccountHeadAutoCompleteForJounal(AccountNameHint, AccountCodeHint, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new JournalBO
                    {
                        CreditAccountHeadID = a.ID,
                        CreditAccountCode = a.AccountID,
                        CreditAccountName = a.AccountName,
                    }).ToList();
                    return AccountHead;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<JournalBO> GetDebitAccountAutoComplete(string AccountNameHint, string AccountCodeHint)
        {
            List<JournalBO> AccountHead = new List<JournalBO>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    AccountHead = dbEntity.SpGetAccountHeadAutoCompleteForJounal(AccountNameHint, AccountCodeHint, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new JournalBO
                    {
                        DebitAccountHeadID = a.ID,
                        DebitAccountCode = a.AccountID,
                        DebitAccountName = a.AccountName,
                    }).ToList();
                    return AccountHead;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Save(JournalBO Master, List<JournalTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "Journal";
                        ObjectParameter Id = new ObjectParameter("JournalID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (Master.IsDraft)
                        {
                            FormName = "DraftJournal";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateJournal(SerialNo.Value.ToString(), Master.Date, Master.TotalCreditAmount, Master.TotalDebitAmount,Master.CurrencyID, Master.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Id);

                        dbEntity.SaveChanges();

                        if (Id.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreateJournalTrans(Convert.ToInt32(Id.Value),
                                    itm.CreditAccountHeadID,
                                    itm.CreditAccountCode,
                                    itm.CreditAmount,
                                    itm.DebitAccountHeadID,
                                    itm.DebitAccountCode,
                                    itm.DebitAmount,
                                    itm.DepartmentID,
                                    itm.EmployeeID,
                                    itm.InterCompanyID,
                                    itm.ProjectID,
                                    itm.Remarks,
                                    itm.JournalLocationID,
                                    itm.LocalCurrencyID,
                                    itm.LocalCurrency,
                                    itm.DebitCurrencyID,
                                    itm.DebitCurrency,
                                    itm.CreditCurrencyID,
                                    itm.CreditCurrency,
                                    itm.DebitExchangeRate,
                                    itm.CreditExchangeRate,
                                    itm.LocalDebitAmount,
                                    itm.LocalCreditAmount,
                                    GeneralBO.FinYear, 
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
                            }

                        };
                        transaction.Commit();
                        return Id.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }


        public List<JournalBO> JournalList()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetJournalList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new JournalBO
                    {
                        ID = a.ID,
                        VoucherNo= a.VoucherNo,
                        Date = (DateTime)a.Date,
                        CreditAccountName=a.CreditAccountName,
                        TotalCreditAmount=a.TotalCreditAmount,
                        DebitAccountName=a.DebitAccountName,
                        TotalDebitAmount=a.TotalDebitAmount,
                        IsDraft=(bool)a.IsDraft,
                    }
                    ).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<JournalBO> GetJournalDetails(int JournalID)
        {
            List<JournalBO> Journal = new List<JournalBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                Journal = dEntity.SpGetJournalDetails(JournalID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new JournalBO
                {
                    ID=a.ID,
                    VoucherNo=a.VoucherNo,
                    Date=(DateTime)a.Date,
                    TotalCreditAmount=a.TotalCreditAmount,
                    TotalDebitAmount=a.TotalDebitAmount,
                    Currency = a.Currency,
                    CurrencyID = (int)a.CurrencyID,
                    IsDraft =(bool)a.IsDraft
                }).ToList();
                return Journal;
            }

        }
        public List<JournalTransBO> GetJournalTransDetails(int JournalID)
        {
            List<JournalTransBO> ItemList = new List<JournalTransBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                ItemList = dEntity.SpGetJournalTransDetails(JournalID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new JournalTransBO
                {
                    CreditAccountCode=a.CreditAccountCode,
                    CreditAccountHeadID =(int)a.CreditAccountID,
                    CreditAccountName =a.CreditAccountName,
                    CreditAmount =a.CreditAmount,
                    DebitAccountCode=a.DebitAccountCode,
                    DebitAccountHeadID =(int) a.DebitAccountID,
                    DebitAccountName =a.DebitAccountName,
                    DebitAmount=a.DebitAmount,
                    Location = a.Location,
                    JournalLocationID = a.JournalLocationID,
                    Department = a.Department,
                    DepartmentID =(int) a.DepartmentID,
                    Employee = a.Employee,
                    EmployeeID =(int)a.EmployeeID,
                    InterCompany = a.InterCompany,
                    InterCompanyID =(int) a.InterCompanyID,
                    Project = a.Project,
                    ProjectID =(int)a.ProjectID,
                    Remarks = a.Remarks,
                    LocalCurrencyID = (int)a.LocalCurrencyID,
                    LocalCurrency = a.LocalCurrency,
                    DebitCurrencyID = (int)a.DebitCurrencyID,
                    DebitCurrency = a.DebitCurrency,
                    CreditCurrencyID = (int)a.CreditCurrencyID,
                    CreditCurrency = a.CreditCurrency,
                    DebitExchangeRate = (decimal)a.DebitExchangeRate,
                    CreditExchangeRate = (decimal)a.CreditExchangeRate,
                    LocalDebitAmount = (decimal)a.LocalDebitAmount,
                    LocalCreditAmount = (decimal)a.LocalCreditAmount

                }).ToList();
                return ItemList;
            }

        }
        public bool Update(JournalBO journal, List<JournalTransBO> journalTrans)
        {
            bool IsSuccess = false;
            using (AccountsEntities entity = new AccountsEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    try
                    {
                        var j = entity.SpUpdateJournal(journal.ID, journal.Date, journal.TotalCreditAmount, journal.TotalDebitAmount,journal.CurrencyID,
                            journal.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        foreach (var itm in journalTrans)
                        {
                            var i = entity.SpCreateJournalTrans(journal.ID,itm.CreditAccountHeadID, itm.CreditAccountCode, 
                                itm.CreditAmount, itm.DebitAccountHeadID, itm.DebitAccountCode, itm.DebitAmount,
                                itm.DepartmentID, itm.EmployeeID, itm.InterCompanyID, itm.ProjectID, itm.Remarks,itm.JournalLocationID,
                                itm.LocalCurrencyID,
                                itm.LocalCurrency,
                                itm.DebitCurrencyID,
                                itm.DebitCurrency,
                                itm.CreditCurrencyID,
                                itm.CreditCurrency,
                                itm.DebitExchangeRate,
                                itm.CreditExchangeRate,
                                itm.LocalDebitAmount,
                                itm.LocalCreditAmount, 
                                GeneralBO.FinYear, 
                                GeneralBO.LocationID, 
                                GeneralBO.ApplicationID);
                        }           
                     
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

        public DatatableResultBO GetJournalList(string Type, string VoucherNoHint, string TransDateHint, string DebitAccountNameHint, string TotalDebitAmountHint, string CreditAccountNameHint, string TotalCreditAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetJournalListForDatatable(Type, VoucherNoHint, TransDateHint, DebitAccountNameHint, CreditAccountNameHint, TotalCreditAmountHint, TotalDebitAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                CreditAccountName = item.CreditAccountName,
                                DebitAccountName=item.DebitAccountName,
                                TotalCreditAmount = item.TotalCreditAmount,
                                TotalDebitAmount = item.TotalDebitAmount,
                                Status = item.status,
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

    }
}
