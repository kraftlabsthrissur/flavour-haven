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
    public class CustomerCreditNoteDAL
    {
        public List<CustomerCreditNoteBO> CreditNoteList()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetCustomerCreditNoteList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CustomerCreditNoteBO
                    {
                        ID = a.ID,
                        TransNo = a.TransNO,
                        Date = (DateTime)a.Date,
                        CustomerName = a.CustomerName,
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

        public string Save(CustomerCreditNoteBO Master, List<CustomerCreditNoteTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "CustomerCreditNote";
                        ObjectParameter CcnId = new ObjectParameter("CreditNoteID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (Master.IsDraft)
                        {
                            FormName = "DraftCustomerCreditNote";
                        }


                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateCustomerCreditNote(SerialNo.Value.ToString(), Master.Date, Master.CustomerID, Master.TotalAmount, Master.TaxableAmount, Master.SGSTAmt, Master.CGSTAmt, Master.IGSTAmt, Master.IsDraft, false, Master.RoundOff, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, CcnId);

                        dbEntity.SaveChanges();

                        if (CcnId.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreateCustomerCreditNoteTrans(Convert.ToInt32(CcnId.Value), itm.ReferenceInvoiceNumber, itm.ReferenceDocumentDate, itm.ItemID, itm.Qty, itm.Rate, itm.NetAmount, itm.DepartmentID, itm.LocationID, itm.EmployeeID, itm.InterCompanyID, itm.ProjectID, itm.Remarks, itm.TaxableAmount, itm.SGSTAmt, itm.CGSTAmt, itm.IGSTAmt, itm.GSTPercentage, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
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

        public string Update(CustomerCreditNoteBO Master, List<CustomerCreditNoteTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpUpdateCustomerCreditNote(Master.ID, Master.CustomerID, Master.Date, Master.TotalAmount, Master.TaxableAmount, Master.SGSTAmt, Master.CGSTAmt, Master.IGSTAmt, Master.IsDraft, false, Master.RoundOff, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);

                        dbEntity.SaveChanges();

                        foreach (var itm in Details)
                        {
                            dbEntity.SpCreateCustomerCreditNoteTrans(Master.ID, itm.ReferenceInvoiceNumber, itm.ReferenceDocumentDate, itm.ItemID, itm.Qty, itm.Rate, itm.NetAmount, itm.DepartmentID, itm.LocationID, itm.EmployeeID, itm.InterCompanyID, itm.ProjectID, itm.Remarks, itm.TaxableAmount, itm.SGSTAmt, itm.CGSTAmt, itm.IGSTAmt, itm.GSTPercentage, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
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


        public List<CustomerCreditNoteBO> GetCreditNoteDetails(int CreditNoteID)
        {
            List<CustomerCreditNoteBO> CreditNote = new List<CustomerCreditNoteBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                CreditNote = dEntity.SpGetCustomerCreditNoteDetails(CreditNoteID, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.ApplicationID).Select(a => new CustomerCreditNoteBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    CustomerName = a.CustomerName,                  
                    DiscountAmount = (decimal)a.DiscountAmount,
                    TaxableAmount = (decimal)a.TaxableAmount,
                    CGSTAmt = (decimal)a.CGSTAmt,
                    SGSTAmt = (decimal)a.SGSTAmt,
                    IGSTAmt = (decimal)a.IGSTAmt,
                    TotalAmount = a.TotalAmount,
                    IsDraft = (bool)a.IsDraft,
                    CustomerID = a.CustomerID,
                    IsGSTRegistered = (bool)a.IsGSTRegistered,
                    StateID = (int)a.StateID,
                    RoundOff = (decimal)a.RoundOff,
                    LocationStateID=(int)a.LocationStateID
                }).ToList();
                return CreditNote;
            }

        }

        public List<CustomerCreditNoteTransBO> GetCreditNoteTransDetails(int CreditNoteID)
        {
            List<CustomerCreditNoteTransBO> ItemList = new List<CustomerCreditNoteTransBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                ItemList = dEntity.SpGetCustomerCreditNoteTransDetails(CreditNoteID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CustomerCreditNoteTransBO
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
                    Location = a.CreditNoteLocation,
                    Department = a.Department,
                    Employee = a.Employee,
                    InterCompany = a.InterCompany,
                    Project = a.Project,
                    Remarks = a.Remarks,
                    ItemID = (int)a.ItemID,
                    LocationID = (int)a.LocationID,
                    DepartmentID = (int)a.DepartmentID,
                    EmployeeID = (int)a.EmployeeID,
                    InterCompanyID = (int)a.InterCompanyID,
                    ProjectID = (int)a.ProjectID,
                    GSTPercentage = (decimal)a.GSTPercentage,


                }).ToList();
                return ItemList;
            }

        }

        public DatatableResultBO GetCustomerCreditNoteList(string Type, string TransNoNoHint, string TransDateHint, string CustomerHint, string InvoiceNoHint, string DocumentDateHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetCustomerCreditNoteListForDatatable(Type, TransNoNoHint, TransDateHint, CustomerHint, InvoiceNoHint, DocumentDateHint, AmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                CustomerName = item.CustomerName,
                                ReferenceInvoiceNumber = item.ReferenceNumber,
                                ReferenceDocumentDate = ((DateTime)item.ReferenceDate).ToString("dd-MMM-yyyy"),
                                TotalAmount = item.TotalAmount,
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
