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
    public class BankExpensesDAL
    {
        public List<BankExpensesBO> BankExpensesList()
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetBankExpensesList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BankExpensesBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    ItemName = a.ItemName,
                    ModeOfPayment = a.ModeOfPayment,
                    TotalAmount = a.TotalAmount,
                    IsDraft = (bool)a.IsDraft
                }).ToList();
            }
        }
        public string Save(BankExpensesBO master, List<BankExpensesTransBO> details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "BankExpenses";
                        ObjectParameter BankExpensesID = new ObjectParameter("BankExpensesID", typeof(int));
                        ObjectParameter Returnvalue = new ObjectParameter("ReturnValue", typeof(int));
                        //if (master.ID!=0)
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (master.IsDraft)
                        {
                            FormName = "DraftBankExpenses";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateBankExpense(SerialNo.Value.ToString(),
                            master.Date,
                            master.BankID,
                            master.TotalAmount,
                            master.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            BankExpensesID);

                        dbEntity.SaveChanges();

                        if (BankExpensesID.Value != null)
                        {
                            foreach (var itm in details)
                            {
                                dbEntity.SpCreateBankExpenseTrans(Convert.ToInt32(BankExpensesID.Value),
                                    itm.TransactionNumber,
                                    itm.TransactionDate,
                                    itm.ModeOfPaymentID,
                                    itm.Amount,
                                    itm.ItemID,
                                    itm.Remarks,
                                    itm.ReferenceNo,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    Returnvalue);
                                if ((int)Returnvalue.Value==-1)
                                {
                                    throw new DatabaseException("Total exceeds credit limit");
                                }
                            }
                            
                        };
                        transaction.Commit();
                        return BankExpensesID.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }

        public string Update(BankExpensesBO Master, List<BankExpensesTransBO> Details)
        {
          
            ObjectParameter Returnvalue = new ObjectParameter("ReturnValue", typeof(int));

            using (AccountsEntities entity = new AccountsEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    try
                    {
                        var j = entity.SpUpdateBankExpenses(Master.ID,
                            Master.Date,
                            Master.BankID,
                            Master.TotalAmount,
                            Master.IsDraft,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            GeneralBO.CreatedUserID);
                        foreach (var itm in Details)
                        {
                            var i = entity.SpCreateBankExpenseTrans(Master.ID,
                                itm.TransactionNumber,
                                itm.TransactionDate,
                                itm.ModeOfPaymentID,
                                itm.Amount,
                                itm.ItemID,
                                itm.Remarks,
                                itm.ReferenceNo,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                Returnvalue);
                            if ((int)Returnvalue.Value == -1)
                            {
                                throw new DatabaseException("Total exceeds credit limit");
                            }
                        }
                       
                        transaction.Commit();
                      
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;

                    }
                }
                return Master.ID.ToString();
            }

        }

        public List<BankExpensesBO> GetAccountAutoComplete(string AccountNameHint, string AccountCodeHint)
        {
            List<BankExpensesBO> AccountHead = new List<BankExpensesBO>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    AccountHead = dbEntity.SpGetAccountHeadAutoComplete(AccountNameHint, AccountCodeHint, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BankExpensesBO
                    {
                        AccountHeadID = a.ID,
                        AccountCode = a.AccountID,
                        AccountName = a.AccountName,
                    }).ToList();
                    return AccountHead;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BankExpensesBO> GetBankExpensesDetails(int BankExpensesID)
        {
            List<BankExpensesBO> BankExpenses = new List<BankExpensesBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                BankExpenses = dEntity.SpGetBankExpensesDetails(BankExpensesID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BankExpensesBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    BankID = (int)a.BankID,
                    Bank = a.BankName,
                    TotalAmount = a.TotalAmount,
                    IsDraft = (bool)a.IsDraft
                }).ToList();
                return BankExpenses;
            }

        }

        public List<BankExpensesTransBO> GetBankExpensesTransDetails(int BankExpensesID)
        {
            List<BankExpensesTransBO> ItemList = new List<BankExpensesTransBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                ItemList = dEntity.SpGetBankExpensesTransDetails(BankExpensesID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new BankExpensesTransBO
                {
                    TransactionNumber = a.TransactionNo,
                    TransactionDate = (DateTime)a.TransactionDate,
                    ItemID = a.ItemID,
                    ModeOfPaymentID = a.ModeOfPaymentID,
                    ModeOfPayment = a.ModeOfPayment,
                    Amount = (decimal)a.Amount,
                    ItemName = a.ItemName,
                    Remarks = a.Remarks,
                    ReferenceNo = a.ReferenceNo
                }).ToList();
                return ItemList;
            }
        }

        public List<BankExpensesBO> GetItemName()
        {
            List<BankExpensesBO> Items = new List<BankExpensesBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                Items = dEntity.SpGetItemsForBankExpenses().Select(a => new BankExpensesBO
                {
                    ID = a.ID,
                    ItemName = a.Name
                }).ToList();
                return Items;
            }
        }

        public DatatableResultBO GetFinancialExpensesList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint, string PaymentHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetFinancialExpensesList(Type, TransNoHint, TransDateHint, ItemNameHint, PaymentHint, AmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                ItemName = item.ItemName,
                                ModeOfPayment = item.ModeOfPayment,
                                TotalAmount = item.TotalAmount,
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
    }
}
