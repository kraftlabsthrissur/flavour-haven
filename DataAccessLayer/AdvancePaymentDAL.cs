using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class AdvancePaymentDAL
    {
        public List<AdvancePaymentBO> GetAdvancePaymentList(int ID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetAdvancePayment(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvancePaymentBO
                    {
                        ID = a.ID,
                        AdvancePaymentDate = (DateTime)a.AdvanceDate,
                        Category = a.Category,
                        SelectedName = a.Name,
                        Amt = (decimal)a.Amount,
                        AdvancePaymentNo = a.AdvanceNo,
                        Draft = (bool)a.IsDraft,
                        NetAmount = a.NetAmount

                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AdvancePaymentBO> GetAdvancePaymentDetails(int ID)
        {
            try
            {
                List<AdvancePaymentBO> itm = new List<AdvancePaymentBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    itm = dbEntity.SpGetAdvancePayment(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvancePaymentBO
                    {
                        ID = a.ID,
                        AdvancePaymentNo = a.AdvanceNo,
                        AdvancePaymentDate = (DateTime)a.AdvanceDate,
                        Category = a.Category,
                        SelectedName = a.Name,
                        Amt = (decimal)a.Amount,
                        ModeOfPaymentName = a.Mode,
                        BankDetail = a.BankName,
                        ReferenceNo = a.ReferenceNo,
                        NetAmount = (decimal)a.NetAmount,
                        Purpose = a.Purpose,
                        Draft = (bool)a.IsDraft,
                        SupplierID = (int)a.SupplierID,
                        EmployeeID = (int)a.EmployeeID,
                        ModeOfPaymentID = (int)a.PaymentTypeID,
                        SupplierOrEmployeeBankName = a.SupplierOrEmployeeBankName,
                        SupplierOrEmployeeBankACNo = a.SupplierOrEmployeeBankACNo,
                        SupplierOrEmployeeIFSCNo = a.SupplierOrEmployeeIFSCNo
                    }).ToList();
                    return itm;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AdvancePaymentPurchaseOrderBO> GetPurchaseOrders(int SupplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseOrdersForAdvance(SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new AdvancePaymentPurchaseOrderBO()
                    {
                        ID = m.ID,
                        TransNo = m.PurchaseOrderNo,
                        PurchaseOrderDate = m.PurchaseOrderDate,
                        PaymentWithin = m.PaymentWithInDays,
                        AdvanceAmount = (decimal)m.MaxAdvancableAmt,
                        POAmount = (decimal)m.NetAmt,
                        Advance = (int)m.AdvanceAmount,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<AdvanceRequestTransBO> GetAdvanceRequest(int EmployeeID, int IsOfficial)
        {
            try
            {
                List<AdvanceRequestTransBO> itm = new List<AdvanceRequestTransBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    itm = dbEntity.SpGetAdvanceRequestByEmployeeID(EmployeeID, IsOfficial, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceRequestTransBO
                    {
                        AdvanceRequestNo = a.RequestNo,
                        AdvanceRequestDate = (DateTime)a.RequestedDate,
                        ItemName = a.ItemName,
                        Amount = (decimal)a.Amount,
                        ItemID = a.ItemID,
                        ID = a.AdvanceRequestID,
                        AdvanceAmount = (decimal)a.AdvanceAmount
                    }).ToList();
                    return itm;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AdvanceRequestTransBO> GetAdvanceRequestForEdit(int ID)
        {
            try
            {
                List<AdvanceRequestTransBO> itm = new List<AdvanceRequestTransBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    itm = dbEntity.SpGetAdvanceRequestTransForEdit(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceRequestTransBO
                    {
                        AdvanceRequestNo = a.RequestNo,
                        AdvanceRequestDate = (DateTime)a.RequestedDate,
                        ItemName = a.ItemName,
                        Amount = (decimal)a.Amount,
                        ItemID =(int) a.ItemID,
                        ID = a.AdvanceRequestID,
                        AdvDetAmount = a.AdvDetAmount,
                        AdvanceAmount = (decimal)a.AdvanceAmount,
                        Remarks=a.Remarks
                    }).ToList();
                    return itm;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<AdvancePaymentPurchaseOrderBO> GetAdvancePaymentTransDetails(int ID)
        {
            try
            {
                List<AdvancePaymentPurchaseOrderBO> adv = new List<AdvancePaymentPurchaseOrderBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    adv = dbEntity.SpGetAdvancePaymentTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvancePaymentPurchaseOrderBO
                    {
                        ID = a.ID,
                        PurchaseOrderDate = (DateTime)a.PODate,
                        PurchaseOrderTerms = a.POTerms,
                        ItemName = a.ItemName,
                        Amount = (decimal)a.Amount,
                        TDSCode = a.Code,
                        TDSAmount = (decimal)a.TDSAmount,
                        Remarks = a.Remarks,
                        TransNo = a.TransNo,
                        Advance = (decimal)a.AdvanceAmount
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

        public List<AdvancePaymentPurchaseOrderBO> GetPurchaseOrdersAdvancePaymentForEdit(int ID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetAdvancePaymentTransForEdit(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new AdvancePaymentPurchaseOrderBO()
                    {
                        ID = (int)m.ID,
                        TransNo = m.PurchaseOrderNo,
                        PurchaseOrderDate = (DateTime)m.PurchaseOrderDate,
                        PaymentWithin = (int)m.PaymentWithInDays,
                        AdvanceAmount = (decimal)m.MaxAdvancableAmt,
                        POAmount = (decimal)m.NetAmt,
                        TDSID = m.TDSID,
                        TDSAmount = m.TDSAmount,
                        Amount = m.Amount,
                        TDSRate = m.TDSRate,
                        ItemID = m.ItemID,
                        ItemName = m.ItemName,
                        Advance = (decimal)m.AdvanceAmount,
                        Remarks = m.Remarks
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<AdvancePaymentBO> GetUnProcessedAdvancePaymentListSupplierWise(int SupplierID, int EmployeeID)
        {
            List<AdvancePaymentBO> Account = new List<AdvancePaymentBO>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    Account = dbEntity.SpGetUnProcessedAdvancePayment(SupplierID, EmployeeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvancePaymentBO
                    {
                        ID = a.ID,
                        AdvancePaymentNo = a.AdvanceNo,
                        AdvancePaymentDate = (DateTime)a.AdvanceDate,
                        SupplierName = a.Name,
                        Category = a.AdvanceCategory,
                        Purpose = a.Purpose,
                        Amt = (decimal)a.Amount

                    }).ToList();
                    return Account;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdvancePaymentTransBO> GetUnProcessedAdvancePaymentTransList(int PaymentID)
        {
            List<AdvancePaymentTransBO> Account = new List<AdvancePaymentTransBO>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    Account = dbEntity.SpGetUnProcessedAdvancePaymentTrans(PaymentID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvancePaymentTransBO
                    {
                        ID = a.AdvancePaymetTransID,
                        AdvancePaymentNo = a.AdvanceNo,
                        PurchaseOrderDate = (DateTime)a.PODate,
                        ItemName = a.ItemName,
                        Amount = (decimal)a.Amount,
                        ItemID = (int)a.ItemID


                    }).ToList();
                    return Account;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetAdvancePaymentListForDataTable(string Type, string AdvancePaymentNo, string AdvancePaymentDate, string Category, string Name, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetAdvancePaymentList(Type, AdvancePaymentNo, AdvancePaymentDate, Category, Name, Amount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                AdvancePaymentNo = item.AdvanceNo,
                                AdvancePaymentDate = ((DateTime)item.AdvanceDate).ToString("dd-MMM-yyyy"),
                                Category = item.Category,
                                Name = item.Name,
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
