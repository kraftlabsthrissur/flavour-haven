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
   public class DebitNoteDAL
    {
        public int Save(DebitNoteBO DebitNote)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "DebitNote";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter DebitNoteID = new ObjectParameter("DebitNoteID", typeof(int));
                        if (DebitNote.IsDraft)
                        {
                            FormName = "DraftDebitNote";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        dbEntity.SpCreateDebitNote(SerialNo.Value.ToString(), DebitNote.TransDate, DebitNote.CreditAccountID, DebitNote.DebitAccountID, DebitNote.Amount, DebitNote.Remarks, DebitNote.IsDraft,
                           DebitNote.IsInclusive, DebitNote.GSTCategoryID, DebitNote.TaxableAmount, DebitNote.GSTAmount, DebitNote.TotalAmount, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, DebitNoteID);
                        transaction.Commit();
                        return (int)DebitNoteID.Value;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int Update(DebitNoteBO DebitNote)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        dbEntity.SpUpdateDebitNote(DebitNote.ID, DebitNote.CreditAccountID, DebitNote.DebitAccountID, DebitNote.Amount, DebitNote.Remarks,
                        DebitNote.IsDraft, DebitNote.IsInclusive, DebitNote.GSTCategoryID, DebitNote.TaxableAmount, DebitNote.GSTAmount, DebitNote.TotalAmount, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        transaction.Commit();
                        return (int)DebitNote.ID;

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }

                }
            }
        }

        public DebitNoteBO GetDebitNote(int DebitNoteID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetDebitNote(DebitNoteID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new DebitNoteBO
                    {
                        TransNo = k.TransNo,
                        TransDate = (DateTime)k.TransDate,
                        IsDraft = (bool)k.IsDraft,
                        ID = k.ID,
                        DebitAccountID = (int)k.DebitAccountID,
                        CreditAccountID = (int)k.CreditAccountID,
                        CreditAccount = k.CreditAccount,
                        DebitAccount = k.DebitAccount,
                        Remarks = k.Remarks,
                        Amount = (decimal)k.Amount,
                        IsInclusive = (bool)k.IsInclusive,
                        GSTCategoryID =(int) k.GSTCategoryID,
                        GSTPercent = (decimal)k.GSTPercent,
                        TaxableAmount = (decimal)k.TaxableAmount,
                        GSTAmount = (decimal)k.GSTAmount,
                        TotalAmount = (decimal)k.TotalAmount,
                        SupplierID=(int)k.SupplierID
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetDebitNoteList(string Type, string TransNo, string TransDate, string DebitAccount, string CreditAccount, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetDebitNoteList(Type, TransNo, TransDate, DebitAccount, CreditAccount, Amount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                DebitAccount = item.DebitAccount,
                                CreditAccount = item.CreditAccount,
                                Amount = item.Amount,
                                Status = item.Status

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
