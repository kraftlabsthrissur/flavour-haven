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
    public class CustomerReturnVoucherDAL
    {
        public List<CustomerReturnVoucherBO> GetCustomerReturnList()
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetCustomerReturnVoucherList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CustomerReturnVoucherBO
                {
                    ID = (int)a.ID,
                    VoucherNo = a.VoucherNo,
                    VoucherDate = (DateTime)a.VoucherDate,
                    CustomerName = a.CustomerName,
                    PaymentTypeName = a.PaymentTypeName,
                    Amount = a.Amount,
                    IsDraft = (bool)a.IsDraft,
                }).ToList();
            }
        }

        public List<CustomerReturnVoucherBO> GetCustomerReturnDetails(int CustomerReturnID)
        {
            List<CustomerReturnVoucherBO> list = new List<CustomerReturnVoucherBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetCustomerReturnVoucherDetails(CustomerReturnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CustomerReturnVoucherBO
                {
                    ID = (int)a.ID,
                    VoucherNo = a.VoucherNo,
                    VoucherDate = a.VoucherDate,
                    CustomerName = a.CustomerName,
                    CustomerID = a.CustomerID,
                    PaymentTypeName = a.PaymentTypeName,
                    PaymentTypeID = a.PaymentTypeID,
                    BankName = a.BankName,
                    BankID = a.BankID,
                    BankReferenceNumber = a.BankReferenceNumber,
                    IsDraft = (bool)a.IsDraft
                }).ToList();
                return list;
            }
        }

        public List<CustomerReturnVoucherItemBO> GetCustomerReturnTransDetails(int FundTransferID)
        {
            List<CustomerReturnVoucherItemBO> list = new List<CustomerReturnVoucherItemBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {

                list = dEntity.SpGetCustomerReturnVoucherTransDetails(FundTransferID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CustomerReturnVoucherItemBO
                {
                    CustomerName = a.CustomerName,
                    Amount = (decimal)a.Amount,
                    Remarks = a.Remarks,
                }).ToList();
                return list;
            }
        }

        public string Save(CustomerReturnVoucherBO Master, List<CustomerReturnVoucherItemBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "CustomerReturnVoucher";
                        ObjectParameter customerReturnID = new ObjectParameter("CustomerReturnID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (Master.IsDraft)
                        {
                            FormName = "DraftCustomerReturnVoucher";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateCustomerReturnVoucher(
                            SerialNo.Value.ToString(),
                            Master.VoucherDate,
                            Master.CustomerID,
                            Master.PaymentTypeID,
                            Master.BankID,
                            Master.BankReferenceNumber,
                            Master.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            customerReturnID);

                        dbEntity.SaveChanges();

                        if (customerReturnID.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreateCustomerReturnVoucherTrans(
                                    Convert.ToInt32(customerReturnID.Value),
                                    itm.Amount,
                                    itm.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    RetValue);
                            }
                        };
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Total exceeds credit limit");
                        }
                        transaction.Commit();
                        return customerReturnID.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }
        public bool Update(CustomerReturnVoucherBO Master, List<CustomerReturnVoucherItemBO> Details)
        {
            bool IsSuccess = false;
            using (AccountsEntities entity = new AccountsEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                    try
                    {
                        var j = entity.SpUpdateCustomerReturnVoucher(
                            Master.ID,
                            Master.VoucherDate,
                            Master.PaymentTypeID,
                            Master.BankID,
                            Master.BankReferenceNumber,
                            Master.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID);
                        foreach (var itm in Details)
                        {
                            var i = entity.SpCreateCustomerReturnVoucherTrans(
                                Master.ID,
                                itm.Amount,
                                itm.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                RetValue);
                        }
                        entity.SaveChanges();
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Total exceeds credit limit");
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                return IsSuccess;
            }
        }

        public DatatableResultBO GetCustomerReturnVoucherList(string Type, string VoucherNoHint, string VoucherDateHint, string CustomerNameHint, string PaymentHint, string ReturnAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetCustomerReturnVoucherListForDatatable(Type, VoucherNoHint, VoucherDateHint, CustomerNameHint, PaymentHint, ReturnAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                VoucherDate = ((DateTime)item.VoucherDate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
                                PaymentTypeName = item.PaymentTypeName,
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
