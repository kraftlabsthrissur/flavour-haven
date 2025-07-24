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
    public class AdvanceRequestDAL
    {
        public List<AdvanceRequestBO> GetAdvanceRequestList(int ID)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                return dEntity.spGetAdvanceRequest(ID, GeneralBO.CreatedUserID,GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceRequestBO
                {
                    ID = a.ID,
                    AdvanceRequestNo = a.RequestNo,
                    AdvanceRequestDate = (DateTime)a.CreatedDate,
                    Amount = (Decimal)a.Amount,
                    EmployeeName = a.EmployeeName,
                    IsProcessed = (bool)a.IsProcessed,
                    IsSuspend=(bool)a.IsSuspend,
                    SelectedQuotationID=(int)a.QuotationID
                }).ToList();
            }
        }

        public List<AdvanceRequestTransBO> GetAdvanceRequesTrans(int ID)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                return dEntity.spGetAdvanceRequestTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AdvanceRequestTransBO
                {
                    ItemName = a.Name,
                    EmployeeName = a.EmployeeName,
                    Amount = (Decimal)a.Amount,
                    Remarks = a.Remarks,
                    ExpectedDate = (DateTime)a.ExpectedDate,
                    Category = a.IsOfficial

                }).ToList();
            }
        }

        public string SaveAdvanceRequest(AdvanceRequestBO advanceRequestBO, List<AdvanceRequestTransBO> advanceRequestTransBO)
        {
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        var j = dEntity.SpUpdateSerialNo("AdvanceRequest", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dEntity.SaveChanges();
                        var i = dEntity.SpCreateAdvanceRequest(
                            SerialNo.Value.ToString(),
                            advanceRequestBO.AdvanceRequestDate,
                            advanceRequestBO.Amount,
                            advanceRequestBO.SelectedQuotationID,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ID
                            );
                        dEntity.SaveChanges();
                        if (ID.Value != null)
                        {
                            foreach (var itm in advanceRequestTransBO)
                            {
                                dEntity.SpCreateAdvanceRequestTrans(
                                    Convert.ToInt32(ID.Value),
                                    itm.ItemID,
                                    itm.EmployeeID,
                                    itm.IsOfficial,
                                    itm.Amount,
                                    itm.Remarks,
                                    itm.ExpectedDate,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
                            }

                        };
                        transaction.Commit();
                        return ID.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int Suspend(int ID)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                try { 
                dbEntity.SpSuspendAdvanceRequest(ID, GeneralBO.FinYear,GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                return 1;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


        public DatatableResultBO GetAdvanceRequestListForDataTable(string Type,string AdvanceRequestNo, string AdvanceRequestDate, string EmployeeName, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.spGetAdvanceRequestList(Type,AdvanceRequestNo, AdvanceRequestDate, EmployeeName, Amount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).ToList();
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
                                AdvanceRequestNo = item.RequestNo,
                                AdvanceRequestDate = ((DateTime)item.RequestedDate).ToString("dd-MMM-yyyy"),
                                EmployeeName = item.EmployeeName,
                                Amount = item.Amount,
                                Status=item.Status
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