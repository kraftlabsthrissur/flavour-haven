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
   public class ProductionPackingScheduleDAL
    {
        public bool Save(PackingBO Packing, List<PackingMaterialBO> Materials)
        {
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PackingSchedule";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter PackingScheduleID = new ObjectParameter("PackingScheduleID", typeof(int));
                        ObjectParameter StockRequisitionID = new ObjectParameter("StockRequisitionID", typeof(int));
                        if (Packing.IsDraft)
                        {
                            FormName = "DraftPackingSchedule";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        dbEntity.SpCreatePackingSchedule(
                            SerialNo.Value.ToString(),
                            Packing.TransDate,
                            Packing.ProductGroupID,
                            Packing.ItemID,
                            Packing.PackedQty,
                            Packing.StartDate,
                            Packing.BatchID,
                            Packing.BatchTypeID,
                            Packing.Remarks,
                            Packing.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            StockRequisitionID,
                            PackingScheduleID
                            );
                        foreach (var item in Materials)
                        {
                            dbEntity.SpCreatePackingScheduleTrans(
                            Convert.ToInt32(PackingScheduleID.Value),
                            Convert.ToInt32(StockRequisitionID.Value),
                            item.ItemID,
                            item.IssueQty,
                            Packing.StartDate,
                            item.Remarks,
                            item.PackingMaterialMasterID,
                            item.UnitID,
                            item.StandardQty,
                            item.ActualQty,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                                );
                        }
                        
                        transaction.Commit();
                    return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public bool Update(PackingBO Packing, List<PackingMaterialBO> Materials)
        {
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter StockRequisitionID = new ObjectParameter("StockRequisitionID", typeof(int));
                        dbEntity.SpUpdatePackingSchedule(
                            Packing.ID,
                            Packing.ProductGroupID,
                            Packing.ItemID,
                            Packing.PackedQty,
                            Packing.StartDate,
                            Packing.BatchID,
                            Packing.BatchTypeID,
                            Packing.Remarks,
                            Packing.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            StockRequisitionID
                            );
                        foreach (var item in Materials)
                        {
                            dbEntity.SpCreatePackingScheduleTrans(
                            Packing.ID,
                            Convert.ToInt32(StockRequisitionID.Value),
                            item.ItemID,
                            item.IssueQty,
                            Packing.StartDate,
                            item.Remarks,
                            item.PackingMaterialMasterID,
                            item.UnitID,
                            item.StandardQty,
                            item.ActualQty,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                                );
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<PackingBO> GetPackingScheduleList()
        {
            List<PackingBO> item = new List<PackingBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpGetPackingSchedule(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new PackingBO
                    {
                        TransNo = k.TransNo,
                        BatchNo=k.BatchNo,
                        BatchID=k.BatchID,
                        BatchTypeID=k.BatchTypeID,
                        BatchType=k.BatchType,
                        PackedQty=k.PackingQty,
                        Remarks=k.Remarks,
                        ItemID=k.ItemID,
                        ItemName=k.ItemName,
                        ProductGroupID=k.ProductionGroupID,
                        ProductGroupName=k.ItemName,
                        StartDate=k.StartDate,
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

        public List<PackingMaterialBO> GetPackingScheduleItems(int PackingScheduleID)
        {
            List<PackingMaterialBO> item = new List<PackingMaterialBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpGetPackingScheduleItems(PackingScheduleID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new PackingMaterialBO
                    {
                       PackingMaterialMasterID=(int)k.ProductDefinitionTransID,
                       ItemName=k.ItemName,
                       ItemID=k.ItemID,
                       Unit=k.Unit,
                       UnitID=(int)k.UnitID,
                       StandardQty=(decimal)k.StdQtyForStdBatch,
                       ActualQty = (decimal)k.StdQtyForActualBatch,
                       IssueQty=(decimal)k.RequiredQty,
                       Remarks=k.Remarks,
                       ItemCode=k.ItemCode,
                       AvailableStock =(decimal) k.AvailableStock
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PackingBO GetPackingSchedule(int PackingScheduleID)
        {
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpGetPackingScheduleByID(PackingScheduleID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new PackingBO
                    {
                        TransNo = k.TransNo,
                        BatchNo = k.BatchNo,
                        BatchID = k.BatchID,
                        BatchTypeID = k.BatchTypeID,
                        BatchType = k.BatchType,
                        PackedQty = k.PackingQty,
                        Remarks = k.Remarks,
                        ItemID = k.ItemID,
                        ItemName = k.ItemName,
                        ProductGroupID = k.ProductionGroupID,
                        ProductGroupName = k.ItemName,
                        StartDate = k.StartDate,
                        IsDraft = (bool)k.IsDraft,
                        ID = k.ID,
                        UOM=k.Unit,
                        UnitID=k.UnitID,
                        TransDate=k.TransDate
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetPackingScheduleList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint,string BatchNoHint,string BatchTypeHint,string PackedQtyHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetPackingScheduleList(Type, TransNoHint, TransDateHint, ItemNameHint, BatchNoHint, BatchTypeHint, PackedQtyHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                ItemName=item.ItemName,
                                BatchNo=item.BatchNo,
                                BatchType=item.BatchType,
                                PackedQty=item.PackingQty,
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
