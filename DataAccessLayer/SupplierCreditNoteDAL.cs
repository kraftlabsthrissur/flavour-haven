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
    public class SupplierCreditNoteDAL
    {
        public List<SupplierCreditNoteBO> SupplierCreditNoteList()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetSupplierCreditNoteList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierCreditNoteBO
                    {
                        ID = a.ID,
                        TransNo = a.TransNo,
                        Date = (DateTime)a.Date,
                        SupplierName = a.SupplierName,
                        ReferenceInvoiceNumber = a.ReferenceNumber,
                        ReferenceDocumentDate = (DateTime)a.ReferenceDate,
                        TotalAmount = a.TotalAmount,
                        IsDraft = (bool)a.IsDraft

                    }
                    ).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string Save(SupplierCreditNoteBO Master, List<SupplierCreditNoteTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "SupplierCreditNote";
                        ObjectParameter CcnId = new ObjectParameter("CreditNoteID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (Master.IsDraft)
                        {
                            FormName = "DraftSupplierCreditNote";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateSupplierCreditNote(SerialNo.Value.ToString(), Master.Date, Master.SupplierID, Master.TotalAmount, Master.TaxableAmount, Master.SGSTAmt, Master.CGSTAmt, Master.IGSTAmt, Master.IsDraft, false,
                           Master.RoundOff, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, CcnId);

                        dbEntity.SaveChanges();

                        if (CcnId.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreateSupplierCreditNoteTrans(Convert.ToInt32(CcnId.Value), itm.ReferenceInvoiceNumber,
                                    itm.ReferenceDocumentDate, itm.ItemID,
                                    itm.Qty, itm.Rate, itm.NetAmount, itm.DepartmentID, itm.LocationID, itm.EmployeeID,
                                    itm.InterCompanyID, itm.ProjectID, itm.Remarks, itm.TaxableAmount, itm.SGSTAmt, itm.CGSTAmt, itm.IGSTAmt, itm.GSTPercentage, GeneralBO.FinYear,
                                    GeneralBO.LocationID, GeneralBO.ApplicationID);
                            }

                        };
                        transaction.Commit();
                        return CcnId.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }
        public List<SupplierCreditNoteBO> GetCreditNoteDetail(int CreditNoteID)
        {
            List<SupplierCreditNoteBO> list = new List<SupplierCreditNoteBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                list = dbEntity.SpGetSupplierCreditNoteDetail(CreditNoteID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierCreditNoteBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    SupplierName = a.SupplierName,
                    GSTNo = a.GSTNo,
                    Addresses = a.Addresses,
                    BillingStateID = a.BillingStateID,
                    BillingState = a.BillingState,
                    BankName = a.BankName,
                    IFSCNo = a.IFSCNo,
                    BankACNo = a.BankACNo,
                    DiscountAmount = (decimal)a.DiscountAmount,
                    TaxableAmount = (decimal)a.TaxableAmount,
                    CGSTAmt = (decimal)a.CGSTAmt,
                    SGSTAmt = (decimal)a.SGSTAmt,
                    IGSTAmt = (decimal)a.IGSTAmt,
                    TotalAmount = a.TotalAmount,
                    IsDraft = (bool)a.IsDraft,
                    StateID = a.StateID,
                    IsGSTRegistred = (bool)a.IsGSTRegistered,
                    SupplierID = a.SupplierID,
                    RoundOff = (decimal)a.RoundOff,
                    LocationStateID=(int)a.LocationStateID
                }).ToList();
                return list;
            }
        }

        public List<SupplierCreditNoteTransBO> GetCreditNoteDetailTrans(int CreditNoteID)
        {
            List<SupplierCreditNoteTransBO> ItemList = new List<SupplierCreditNoteTransBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                ItemList = dbEntity.SpGetSupplierCreditNoteTransDetail(CreditNoteID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierCreditNoteTransBO
                {
                    ReferenceInvoiceNumber = a.ReferenceNumber,
                    ReferenceDocumentDate = (DateTime)a.ReferenceDate,
                    Item = a.Item,
                    HSNCode = a.HSNCode,
                    Unit = a.Unit,
                    Qty = (decimal)a.Quantity,
                    Rate = (decimal)a.Rate,
                    Amount = (decimal)a.Amount,
                    DiscountAmount = (decimal)a.DiscountAmount,
                    TaxableAmount = (decimal)a.TaxableAmount,
                    CGSTAmt = (decimal)a.CGSTAmt,
                    SGSTAmt = (decimal)a.SGSTAmt,
                    IGSTAmt = (decimal)a.IGSTAmt,
                    Location = a.Location,
                    Department = a.Department,
                    Employee = a.Employee,
                    InterCompany = a.InterCompany,
                    Project = a.Project,
                    Remarks = a.Remarks,
                    GSTPercentage = (decimal)a.GSTPercentage,
                    LocationID = a.LocationID,
                    DepartmentID = a.DepartmentID,
                    EmployeeID = a.EmployeeID,
                    InterCompanyID = a.InterCompanyID,
                    ProjectID = a.ProjectID,
                    ItemID = (int)a.ItemID,
                }).ToList();
                return ItemList;
            }

        }

        public string Update(SupplierCreditNoteBO Master, List<SupplierCreditNoteTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {

                        var i = dbEntity.SpUpdateSupplierCreditNote(Master.ID, Master.SupplierID, Master.Date, Master.TotalAmount, Master.TaxableAmount, Master.CGSTAmt, Master.SGSTAmt, Master.IGSTAmt, Master.IsDraft,
                           Master.RoundOff, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);

                        dbEntity.SaveChanges();


                        foreach (var itm in Details)
                        {
                            dbEntity.SpCreateSupplierCreditNoteTrans(Master.ID, itm.ReferenceInvoiceNumber,
                                itm.ReferenceDocumentDate, itm.ItemID,
                                itm.Qty, itm.Rate, itm.NetAmount, itm.DepartmentID, itm.LocationID, itm.EmployeeID,
                                itm.InterCompanyID, itm.ProjectID, itm.Remarks, itm.TaxableAmount, itm.SGSTAmt, itm.CGSTAmt, itm.IGSTAmt, itm.GSTPercentage, GeneralBO.FinYear,
                                GeneralBO.LocationID, GeneralBO.ApplicationID);
                        }

                        transaction.Commit();
                        return 1.ToString();
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }

        public DatatableResultBO GetSupplierCreditNoteList(string Type, string TransNo, string TransDate, string Supplier, string ReferenceInvoiceNumber, string ReferenceDocumentDate, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetSupplierCreditNoteListForDataTable(Type, TransNo, TransDate, Supplier, ReferenceInvoiceNumber, ReferenceDocumentDate, TotalAmount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransDate = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                Supplier = item.SupplierName,
                                ReferenceInvoiceNumber = item.ReferenceNumber,
                                ReferenceDocumentDate = ((DateTime)item.ReferenceDate).ToString("dd-MMM-yyyy"),
                                TotalAmount = item.TotalAmount,
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
