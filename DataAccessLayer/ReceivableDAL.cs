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
    public class ReceivableDAL
    {
        public List<ReceivablesBO> GetReceivables(int CustomerID)
        {
            List<ReceivablesBO> list = new List<ReceivablesBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceivables(CustomerID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new ReceivablesBO
                {
                    DocumentNo = m.DocumentNo,
                    TransDate = (DateTime)m.TransDate,
                    ReceivableAmount = (decimal)m.ReceivableAmount,
                    Balance = (decimal)m.Balance,
                    DocumentType = m.DocumentType,
                    ID = m.ID,
                    CreditNoteID = m.CreditNoteID,
                    DebitNoteID = m.DebitNoteID,
                    AdvanceID=m.AdvanceID,
                    SalesReturnID=m.SalesReturnID,
                    CustomerReturnVoucherID = (int)m.CustomerReturnVoucherID
                }).ToList();
                return list;
            }
        }

        public int SaveReceivables(ReceivablesBO Receivable)
        {
            int ReceivableID = 0;
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        ObjectParameter ReceivableIDOut = new ObjectParameter("ReceivableID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        var j = dbEntity.SpUpdateSerialNo("Receivables", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SpInsertReceivables(
                            SerialNo.Value.ToString(),
                            Receivable.TransDate,
                            Receivable.ReceivableType,
                            Receivable.ReferenceID,
                            Receivable.DocumentNo,
                            Receivable.PartyID,
                            Receivable.ReceivableAmount,
                            Receivable.Description,
                            Receivable.ReceivedAmount,
                            Receivable.Status,
                            Receivable.Discount,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ReceivableIDOut
                            );
                      
                        ReceivableID = Convert.ToInt32(ReceivableIDOut.Value);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return ReceivableID;
        }
        public List<ReceivablesBO> GetReceivablesV3(int AccountHeadID)
        {
            List<ReceivablesBO> list = new List<ReceivablesBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetReceivablesV3(AccountHeadID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new ReceivablesBO
                {
                    DocumentNo = m.DocumentNo,
                    TransDate = (DateTime)m.TransDate,
                    ReceivableAmount = (decimal)m.ReceivableAmount,
                    Balance = (decimal)m.Balance,
                    DocumentType = m.DocumentType,
                    ID = m.ID,
                    CreditNoteID = m.CreditNoteID,
                    DebitNoteID = m.DebitNoteID,
                    AdvanceID = m.AdvanceID,
                    SalesReturnID = m.SalesReturnID,
                    CustomerReturnVoucherID = (int)m.CustomerReturnVoucherID
                }).ToList();
                return list;
            }
        }
    }
}
