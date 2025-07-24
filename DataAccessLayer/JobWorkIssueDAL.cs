using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace DataAccessLayer
{
    public class JobWorkIssueDAL
    {
        public int Save(JobWorkIssueBO jobworkissueBO, List<JobWorkIssueItemBO> jobWorkIssueItemBO)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    ObjectParameter JobWorkIssueID = new ObjectParameter("JobWorkIssueID", typeof(int));
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                    var j = dbEntity.SpUpdateSerialNo("JobWorkIssue", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    dbEntity.SpCreateJobWorkIssue(
                             SerialNo.Value.ToString(),
                             jobworkissueBO.IssueDate,
                             jobworkissueBO.SupplierID,
                             jobworkissueBO.IsDraft,
                             GeneralBO.CreatedUserID,
                             jobworkissueBO.IssueDate,
                             GeneralBO.FinYear,
                             GeneralBO.ApplicationID,
                             GeneralBO.LocationID,
                             JobWorkIssueID
                         );
                    dbEntity.SaveChanges();
                    int JobIssueID = Convert.ToInt32(JobWorkIssueID.Value);

                    foreach (var item in jobWorkIssueItemBO)
                    {
                        dbEntity.SpCreateJobWorkIssueTrans(JobIssueID,
                          item.IssueItemID,
                          item.IssueUnit,
                          item.IssueQty,
                          item.WarehouseID,
                          GeneralBO.FinYear,
                          GeneralBO.LocationID,
                          GeneralBO.ApplicationID
                           );
                    }
                };

                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<JobWorkIssueBO> GetJobWorkIssue()
        {
            List<JobWorkIssueBO> item = new List<JobWorkIssueBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpGetJobWorkIssueDetails(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new JobWorkIssueBO
                    {
                        IssueNo = k.IssueNo,
                        IssueDate = k.IssueDate,
                        Supplier = k.Supplier,
                        IsDraft = (bool)k.IsDraft,
                        ID = k.ID
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<JobWorkIssueItemBO> GetJobWorkIssueItems(int JobWorkIssueID)
        {
            List<JobWorkIssueItemBO> item = new List<JobWorkIssueItemBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpGetJobWorkIssueItems(JobWorkIssueID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new JobWorkIssueItemBO
                    {
                        IssueItemName = k.IssueItem,
                        IssueUnit = k.Unit,
                        IssueQty = k.IssueQty,
                        IssueItemID = k.IssueItemID,
                        IssueTransID = k.ID,
                        QtyMet = (decimal)k.QtyMet,
                        WarehouseID = (int)k.WarehouseID,
                        Stock = (decimal)k.Stock

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public JobWorkIssueBO GetJobWorkIssue(int JobWorkIssueID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpGetJobWorkIssue(JobWorkIssueID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new JobWorkIssueBO
                    {
                        ID = k.ID,
                        IssueNo = k.IssueNo,
                        IssueDate = k.IssueDate,
                        IsDraft = (bool)k.IsDraft,
                        SupplierID = (int)k.SupplierID,
                        Supplier = k.Supplier,
                        WarehouseID = (int)k.WarehouseID
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(JobWorkIssueBO jobworkissueBO, List<JobWorkIssueItemBO> jobWorkIssueItemBO)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {


                    dbEntity.SpUpdateJobWorkIssue(
                        jobworkissueBO.ID,
                        jobworkissueBO.IssueNo,
                        jobworkissueBO.IssueDate,
                        jobworkissueBO.SupplierID,
                        jobworkissueBO.IsDraft,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.ApplicationID,
                        GeneralBO.LocationID
                        );
                    foreach (var item in jobWorkIssueItemBO)
                    {
                        dbEntity.SpCreateJobWorkIssueTrans(jobworkissueBO.ID,
                          item.IssueItemID,
                          item.IssueUnit,
                          item.IssueQty,
                          item.WarehouseID,
                          GeneralBO.FinYear,
                          GeneralBO.LocationID,
                          GeneralBO.ApplicationID
                           );
                    }

                };

                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetIssueList(int SupplierID, string IssueNoHint, string SupplierHint, string IssueDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetIssueList(SupplierID, IssueNoHint, SupplierHint, IssueDateHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SupplierID = item.SupplierID,
                                IssueNo = item.IssueNo,
                                Supplier = item.Supplier,
                                IssueDate = ((DateTime)item.IssueDate).ToString("dd-MMM-yyyy"),
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
