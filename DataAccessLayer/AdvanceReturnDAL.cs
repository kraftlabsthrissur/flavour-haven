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
    public class AdvanceReturnDAL
    {


        public bool Save(AdvanceReturnBO advanceReturn, List<AdvanceReturnTransBO> advancereturntrans)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "AdvanceReturn";
                        ObjectParameter SiId = new ObjectParameter("AdvanceReturnID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter Retvalue = new ObjectParameter("RetVal", typeof(int));

                        if (advanceReturn.IsDraft)
                        {
                            FormName = "DraftAdvanceReturn";
                        }

                        var j = dEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var CreatedUserID = GeneralBO.CreatedUserID;
                        dEntity.SaveChanges();

                        var i = dEntity.SpCreateAdvanceReturn(
                             SerialNo.Value.ToString(),
                            advanceReturn.Date,
                            advanceReturn.Category,
                            advanceReturn.SupplierID,
                            advanceReturn.EmployeeID,
                            advanceReturn.NetAmount,
                            advanceReturn.IsDraft,
                            advanceReturn.PaymentTypeID,
                            advanceReturn.BankID,
                            advanceReturn.ReferenceNumber,
                            advanceReturn.Remarks,
                             GeneralBO.CreatedUserID,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID,

                             SiId);

                        dEntity.SaveChanges();
                        if (SiId.Value != null)
                        {
                            foreach (var itm in advancereturntrans)
                            {

                                dEntity.SpCreateAdvanceReturnTrans(
                                              Convert.ToInt32(SiId.Value),
                                              itm.AdvanceID,
                                              itm.ReturnAmount,
                                              itm.ItemID,
                                              itm.AdvancePaidAmount,
                                              GeneralBO.FinYear,
                                              GeneralBO.LocationID,
                                              GeneralBO.ApplicationID,
                                              Retvalue
                                              );
                                if ((int)Retvalue.Value == 1)
                                {
                                    throw new DatabaseException("Settled amount exceeds advance amount");
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
        public List<AdvanceReturnBO> GetAdvanceReturnList()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetAdvanceReturn(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceReturnBO
                    {
                        ID = a.ID,
                        NetAmount = (decimal)a.Amount,
                        Category = a.AdvanceReturnCategory,
                        EmployeeName = a.Name,
                        ReturnNo = a.AdvanceReturnNo,
                        IsDraft = (bool)a.IsDRaft,
                        Date = (DateTime)a.AdvanceReturnDate


                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AdvanceReturnBO> GetAdvanceReturnDetails(int ID)
        {
            try
            {
                List<AdvanceReturnBO> itm = new List<AdvanceReturnBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    itm = dbEntity.SpGetAdvanceReturnDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceReturnBO
                    {
                        ID = a.ID,
                        NetAmount = (decimal)a.Amount,
                        Category = a.AdvanceReturnCategory,
                        ReturnNo = a.AdvanceReturnNo,
                        IsDraft = (bool)a.IsDRaft,
                        Date = (DateTime)a.AdvanceReturnDate,
                        PaymentTypeID = (int)a.PaymentTypeID,
                        BankID = (int)a.BankID,
                        PaymentTypeName = a.PaymentType,
                        BankName = a.BankName,
                        ReferenceNumber = a.ReferenceNo,
                        Remarks = a.Remarks,
                        SupplierID = (int)a.SupplierID,
                        EmployeeID = (int)a.EmployeeID,
                        SupplierName = a.SupplierName,
                        EmployeeName = a.EmployeeName,
                    }).ToList();
                    return itm;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AdvanceReturnTransBO> GetAdvanceReturnTransDetails(int ID)
        {
            try
            {
                List<AdvanceReturnTransBO> adv = new List<AdvanceReturnTransBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    adv = dbEntity.SpGetAdvanceReturnTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceReturnTransBO
                    {
                        ItemName = a.ItemName,
                        ReturnAmount = (decimal)a.ReturnAmount,
                        Amount = (decimal)a.PaidAmount,
                        PODate = (DateTime)a.PODate,
                        TransNo = a.AdvanceNo,
                        AdvanceID = a.AdvanceID,
                        ItemID = (int)a.ItemID
                    }
                    ).ToList();
                    return adv;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(AdvanceReturnBO advanceReturn, List<AdvanceReturnTransBO> advancereturntrans)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                ObjectParameter Retvalue = new ObjectParameter("RetVal", typeof(int));
                ObjectParameter SiId = new ObjectParameter("AdvanceReturnID", typeof(int));
                try
                {
                    dbEntity.SpUpdateAdvanceReturn(
                        advanceReturn.ID,
                        advanceReturn.Date,
                        advanceReturn.Category,
                        advanceReturn.SupplierID,
                        advanceReturn.EmployeeID,
                        advanceReturn.NetAmount,
                        advanceReturn.IsDraft,
                        advanceReturn.PaymentTypeID,
                        advanceReturn.BankID,
                        advanceReturn.ReferenceNumber,
                        advanceReturn.Remarks,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                    foreach (var itm in advancereturntrans)
                    {

                        dbEntity.SpCreateAdvanceReturnTrans(
                                      advanceReturn.ID,
                                      itm.AdvanceID,
                                      itm.ReturnAmount,
                                      itm.ItemID,
                                      itm.AdvancePaidAmount,
                                      GeneralBO.FinYear,
                                      GeneralBO.LocationID,
                                      GeneralBO.ApplicationID,
                                      Retvalue
                                      );
                        if ((int)Retvalue.Value == 1)
                        {
                            throw new DatabaseException("Settled amount exceeds advance amount");
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        public DatatableResultBO GetAdvanceReturnList(string Type, string ARNoHint, string ARDateHint, string NameHint, string CategoryHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetAdvanceReturnList(Type, ARNoHint, ARDateHint, NameHint, CategoryHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                AdvanceReturnNo = item.AdvanceReturnNo,
                                AdvanceReturnDate = ((DateTime)item.AdvanceReturnDate).ToString("dd-MMM-yyyy"),
                                Name = item.Name,
                                AdvanceReturnCategory = item.AdvanceReturnCategory,
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

    }

}
