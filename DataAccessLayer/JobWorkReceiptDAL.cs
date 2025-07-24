using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
   public class JobWorkReceiptDAL
    {

        public int Save(JobWorkReceiptBO jobWorkReceiptBO, List<JobWorkIssuedItemBO> jobWorkIssuedItems, List<JobWorkReceiptItemBO> JobWorkReceiptItems)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    ObjectParameter JobWorkReceiptID = new ObjectParameter("JobWorkReceiptID", typeof(int));
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                    var j = dbEntity.SpUpdateSerialNo("JobWorkReceipt", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    dbEntity.SpCreateJobWorkReceipt(
                             SerialNo.Value.ToString(),
                             jobWorkReceiptBO.TransDate,
                             jobWorkReceiptBO.SupplierID,
                             jobWorkReceiptBO.IssueID,
                             jobWorkReceiptBO.IsDraft,
                             GeneralBO.CreatedUserID,
                             jobWorkReceiptBO.TransDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID,
                             JobWorkReceiptID
                         );
                   
                    int JobReceiptID = Convert.ToInt32(JobWorkReceiptID.Value);

                    foreach (var item in JobWorkReceiptItems)
                    {
                            dbEntity.SpCreateJobWorkReceiptTrans(JobReceiptID,
                              item.ReceiptItemID,
                              item.ReceiptUnit,
                              item.ReceiptQty,
                              item.ReceiptDate,
                              item.WarehouseID,
                              GeneralBO.FinYear,
                              GeneralBO.LocationID,
                              GeneralBO.ApplicationID
                               );
                    }
                    foreach (var item in jobWorkIssuedItems)
                    {
                        dbEntity.SpCreateJobWorkReceiptIssueDetails(JobReceiptID,
                          item.IssueTransID,
                          item.PendingQuantity,
                          item.IsCompleted,
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

        public int Update(JobWorkReceiptBO jobWorkReceiptBO, List<JobWorkIssuedItemBO> jobWorkIssuedItems, List<JobWorkReceiptItemBO> JobWorkReceiptItems)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {


                    dbEntity.SpUpdateJobWorkReceipt(
                             jobWorkReceiptBO.ID,
                             jobWorkReceiptBO.TransNo,
                             jobWorkReceiptBO.TransDate,
                             jobWorkReceiptBO.SupplierID,
                             jobWorkReceiptBO.IssueID,
                             jobWorkReceiptBO.IsDraft,
                             GeneralBO.CreatedUserID,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID
                        );
                    foreach (var item in JobWorkReceiptItems)
                    {
                        dbEntity.SpCreateJobWorkReceiptTrans(jobWorkReceiptBO.ID,
                          item.ReceiptItemID,
                          item.ReceiptUnit,
                          item.ReceiptQty,
                          item.ReceiptDate,
                          item.WarehouseID,
                          GeneralBO.FinYear,
                          GeneralBO.LocationID,
                          GeneralBO.ApplicationID
                           );
                    }
                    foreach (var item in jobWorkIssuedItems)
                    {
                        dbEntity.SpCreateJobWorkReceiptIssueDetails(jobWorkReceiptBO.ID,
                          item.IssueTransID,
                          item.PendingQuantity,
                          item.IsCompleted,
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
        public List<JobWorkReceiptBO> GetJobWorkReceipts()
        {
            List<JobWorkReceiptBO> item = new List<JobWorkReceiptBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpGetJobWorkReceipts(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new JobWorkReceiptBO
                    {
                        ID=k.ID,
                        TransNo=k.TransNO,
                        TransDate=k.TransDate,
                        IssueNo=k.IssueNo,
                        Supplier=k.Supplier,
                        IsDraft=(bool)k.IsDraft
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobWorkReceiptBO GetJobWorkReceipt(int JobWorkReceiptID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpGetJobWorkReceipt(JobWorkReceiptID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new JobWorkReceiptBO
                    {
                        TransNo=k.TransNO,
                        TransDate=k.TransDate,
                        Supplier=k.Supplier,
                        IsDraft=(bool)k.isDraft,
                        ID=k.ID,
                        IssueNo=k.IssueNo,
                        IssueID=(int)k.IssueID,
                        SupplierID=(int)k.SupplierID
                       
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<JobWorkReceiptItemBO> GetJobWorkReceiptItems(int JobWorkReceiptID)
        {
            List<JobWorkReceiptItemBO> item = new List<JobWorkReceiptItemBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpGetJobWorkReceiptItems(JobWorkReceiptID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new JobWorkReceiptItemBO
                    {
                        ReceiptItemID=(int)k.ReceiptItemID,
                        ReceiptItemName=k.ReceiptItem,
                        ReceiptDate=(DateTime)k.ReceiptDate,
                        ReceiptQty=k.ReceiptQty,
                        ReceiptUnit=k.Unit,
                        WarehouseID=(int)k.WarehouseID
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<JobWorkIssuedItemBO> GetJobWorkIssuedItems(int JobWorkReceiptID)
        {
            List<JobWorkIssuedItemBO> item = new List<JobWorkIssuedItemBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpGetJobWorkIssuedItems(JobWorkReceiptID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new JobWorkIssuedItemBO
                    {
                        IssueTransID=(int)k.JobWorkIssueTransID,
                        IsCompleted=(bool)k.IsCompleted,
                        PendingQuantity=k.PendingQty,
                        IssuedQty=k.IssueQty,
                        IssuedItem=k.IssuedItem,
                        IssuedUnit=k.issuedUnit

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
