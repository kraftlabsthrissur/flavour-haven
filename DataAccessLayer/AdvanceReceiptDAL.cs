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
    public class AdvanceReceiptDAL
    {
        public List<AdvanceReceiptBO> GetCustomerCategoryList()
        {
            List<AdvanceReceiptBO> CategoryList = new List<AdvanceReceiptBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                CategoryList = dEntity.SpGetCustomerCategoryList().Select(a => new AdvanceReceiptBO
                {
                   CategoryID=a.ID,
                   CategoryName=a.Name

                }).ToList();
                return CategoryList;
            }

        }

        public List<SalesOrderBO> GetSalesOrders(int CustomerID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetSalesOrders(CustomerID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new SalesOrderBO()
                    {
                        ID = m.ID,
                        SONo = m.SalesOrderNo,
                        SODate = (DateTime)m.OrderDate,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<SalesOrderBO> GetItemNamesForSalesOrder(int SalesID,string TransNo, string search)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetItemsForSalesOrder(SalesID, TransNo, search,GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new SalesOrderBO()
                    {
                        ItemID = m.ID,
                        ItemName = m.Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Save(AdvanceReceiptBO advanceReceiptBO, List<AdvanceReceiptItemBO> ReceiptItems)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter ReturnValue = new ObjectParameter("AdvanceID", typeof(int));
                    var j = dbEntity.SpUpdateSerialNo("AdvanceReceipt", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    dbEntity.SpCreateAdvanceReceipt(
                            SerialNo.Value.ToString(),
                            advanceReceiptBO.CustomerID,
                            advanceReceiptBO.PaymentTypeID,
                            advanceReceiptBO.BankName,
                            advanceReceiptBO.ReferenceNo,
                            advanceReceiptBO.AdvanceReceiptDate,
                            advanceReceiptBO.AdvanceReceiptNo,
                            advanceReceiptBO.Amount,
                            advanceReceiptBO.NetAmount,
                            advanceReceiptBO.TDSAmount,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ReturnValue
                        );
                    foreach (var item in ReceiptItems)
                    {
                        if (item.ItemAmount > 0)
                        {
                            dbEntity.SpCreateAdvanceReceiptTrans(
                              Convert.ToInt32(ReturnValue.Value),
                                item.SalesOrderDate,
                                item.TransNo,
                                item.ItemID,
                                item.TDSID,
                                item.ItemAmount,
                                advanceReceiptBO.TDSAmount,
                                item.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                            );
                        }
                    }
                };

                return 1;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AdvanceReceiptBO> GetReceiptList()
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                return dEntity.SpGetAdvanceReceipt().Select(a => new AdvanceReceiptBO
                {
                    ID = a.ID,
                    AdvanceReceiptNo = a.AdvanceNo,
                    AdvanceReceiptDate =(DateTime) a.AdvanceDate,
                    CustomerName = a.CustomerName,
                    NetAmount = a.NetAmount
                }).ToList();

            }
        }

        public List<AdvanceReceiptBO> GetAdvanceReceiptDetails(int id)
        {
            try
            {
                List<AdvanceReceiptBO> items = new List<AdvanceReceiptBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    items = dbEntity.SpGetAdvanceReceipyByID(id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceReceiptBO
                    {
                        ID = a.ID,
                        AdvanceReceiptNo = a.AdvanceNo,
                        AdvanceReceiptDate = (DateTime)a.AdvanceDate,
                        CustomerName = a.CustomerName,
                        PaymentTypeName = a.PaymentTypeName,
                        BankName = a.BankName,
                        ReferenceNo = a.ReferenceNo,
                        NetAmount = a.NetAmount,
                        PaymentTypeID=(int)a.PaymentTypeID,
                        

                    }).ToList();
                    return items;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AdvanceReceiptBO> GetAdvanceReceiptTransDetails(int id)
        {
            try
            {
                List<AdvanceReceiptBO> TransItems = new List<AdvanceReceiptBO>();
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    TransItems = dbEntity.SpGetAdvanceReceiptTransDetails(id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceReceiptBO
                    {
                        SalesOrderDate =(DateTime)a.SODate,
                        ItemName =a.ItemName,
                        Amount =(decimal) a.Amount,
                        TDSCode = a.Code,
                        TDSAmount =(decimal) a.TDSAmount,
                        TransNo = a.TransNo,
                        Remarks=a.Remarks,
                        TDSValue= string.Concat(a.TDSID, "#", a.TDSRate)
                    }).ToList();
                    return TransItems;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
