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
    public class SupplierDebitNoteDAL
    {
        public List<SupplierDebitNoteBO> SupplierDebitNoteList()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetSupplierDebitNoteList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierDebitNoteBO
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
        public string Save(SupplierDebitNoteBO Master, List<SupplierDebitNoteTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "SupplierDebitNote";
                        ObjectParameter CcnId = new ObjectParameter("DebitNoteID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (Master.IsDraft)
                        {
                            FormName = "DraftSupplierDebitNote";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateSupplierDebitNote(SerialNo.Value.ToString(), Master.Date, Master.SupplierID, Master.TotalAmount, Master.TaxableAmount, Master.SGSTAmt, Master.CGSTAmt, Master.IGSTAmt, Master.IsDraft, false,
                        Master.RoundOff, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, CcnId);

                        dbEntity.SaveChanges();

                        if (CcnId.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreateSupplierDebitNoteTrans(Convert.ToInt32(CcnId.Value),
                                    itm.ReferenceInvoiceNumber,
                                    itm.ReferenceDocumentDate,
                                    itm.ItemID,
                                    itm.Qty,
                                    itm.Rate,
                                    itm.NetAmount,
                                    itm.DepartmentID,
                                    itm.LocationID,
                                    itm.EmployeeID,
                                    itm.InterCompanyID,
                                    itm.ProjectID,
                                    itm.Remarks,
                                    itm.TaxableAmount,
                                    itm.SGSTAmt,
                                    itm.CGSTAmt,
                                    itm.IGSTAmt,
                                    itm.GSTPercentage,
                                    itm.PurchaseReturnID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
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

        public string Update(SupplierDebitNoteBO Master, List<SupplierDebitNoteTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpUpdateSupplierDebitNote(Master.ID, Master.SupplierID, Master.Date, Master.TotalAmount, Master.TaxableAmount, Master.SGSTAmt, Master.CGSTAmt, Master.IGSTAmt, Master.IsDraft, false, Master.RoundOff, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);

                        foreach (var itm in Details)
                        {
                            dbEntity.SpCreateSupplierDebitNoteTrans(Master.ID,
                                itm.ReferenceInvoiceNumber,
                                itm.ReferenceDocumentDate,
                                itm.ItemID,
                                itm.Qty,
                                itm.Rate,
                                itm.NetAmount,
                                itm.DepartmentID,
                                itm.LocationID,
                                itm.EmployeeID,
                                itm.InterCompanyID,
                                itm.ProjectID,
                                itm.Remarks,
                                itm.TaxableAmount,
                                itm.SGSTAmt,
                                itm.CGSTAmt,
                                itm.IGSTAmt,
                                itm.GSTPercentage,
                                itm.PurchaseReturnID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
                        }
                        transaction.Commit();
                        dbEntity.SaveChanges();
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

        public List<SupplierDebitNoteBO> GetDebitNoteDetail(int CreditNoteID)
        {
            List<SupplierDebitNoteBO> DebitNote = new List<SupplierDebitNoteBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                DebitNote = dbEntity.SpGetSupplierDebitNoteDetails(CreditNoteID, GeneralBO.ApplicationID).Select(a => new SupplierDebitNoteBO
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
                    SupplierID = (int)a.SupplierID,
                    IsGSTRegistered = (bool)a.IsGSTRegistered,
                    StateID = (int)a.StateID,                    
                    RoundOff = (decimal)a.RoundOff,
                    LocationStateID=(int)a.LocationStateID
                }).ToList();
                return DebitNote;
            }

        }
        public List<SupplierDebitNoteTransBO> GetDebitNoteTransDetail(int CreditNoteID)
        {
            List<SupplierDebitNoteTransBO> ItemList = new List<SupplierDebitNoteTransBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                ItemList = dbEntity.SpGetSupplierDebitNoteTransDetails(CreditNoteID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierDebitNoteTransBO
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
                    GSTPercentage = (decimal)a.GSTPercentage,
                    Location = a.Location,
                    Department = a.Department,
                    Employee = a.Employee,
                    InterCompany = a.InterCompany,
                    Project = a.Project,
                    Remarks = a.Remarks,
                    PurchaseReturnNo = a.ReturnNo,
                    ItemID = (int)a.ItemID,
                    LocationID = (int)a.LocationID,
                    DepartmentID = (int)a.DepartmentID,
                    EmployeeID = (int)a.EmployeeID,
                    InterCompanyID = (int)a.InterCompanyID,
                    ProjectID = (int)a.ProjectID,
                }).ToList();
                return ItemList;
            }

        }


        public DatatableResultBO GetSupplierDebitNoteList(string Type, string TransNo, string TransDate, string Supplier, string ReferenceInvoiceNumber, string ReferenceDocumentDate, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetSupplierDebitNoteListForDataTable(Type, TransNo, TransDate, Supplier, ReferenceInvoiceNumber, ReferenceDocumentDate, TotalAmount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
