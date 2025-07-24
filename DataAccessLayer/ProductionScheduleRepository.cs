using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductionScheduleRepository : IProductionSchedule
    {
        private readonly ProductionEntities _entity;
        #region Constructor

        public ProductionScheduleRepository()
        {
            _entity = new ProductionEntities();
        }

        #endregion

        /// <summary>
        /// Get Production Gropus
        /// </summary>
        public List<KeyValuePair<int, string>> GetProductionGroups(string ItemHind)
        {
            List<KeyValuePair<int, string>> item = new List<KeyValuePair<int, string>>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpPGetProductionGroups(ItemHind, GeneralBO.LocationID, GeneralBO.ApplicationID)
                        .Select(x => new KeyValuePair<int, string>(x.ProductionGroupID, x.ProductionGroupName)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return item;
        }

        /// <summary>
        /// Get Production Issue items by Production group
        /// </summary>
        public List<ProductionScheduleItemBO> GetProductionIssueItemsByProductionGroup(int productionGroupID)
        {
            List<ProductionScheduleItemBO> item = new List<ProductionScheduleItemBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpPGetProductionIssueItemsByProductionGroup(productionGroupID, GeneralBO.LocationID, GeneralBO.ApplicationID)
                        .Select(x => new ProductionScheduleItemBO
                        {
                            ID = x.ID,
                            ProductID = x.ProductID ?? 0,
                            ItemID = x.ItemID ?? 0,
                            ItemName = x.ItemName,
                            Unit = x.Unit,
                            YogamQty = x.YogamQty ?? 0,
                            StandardBatchSize = x.StandardBatchSize ?? 0,
                            ProductDefinitionTransID = x.ProductDefinitionTransID,
                            UnitID = (int)x.MaterialUnitID,
                            StandardOutputQty = x.StandardOutputQty ?? 0
                        }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return item;
        }


        /// <summary>
        /// Get production schedule by ID
        /// </summary>
        /// <returns></returns>
        public ProductionScheduleBO GetProductionSchedule(int productionScheduleID)
        {
            ProductionScheduleBO productionScheduleBO = new ProductionScheduleBO();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    productionScheduleBO = dbEntity.SpGetProductionSchedule(productionScheduleID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID)
                        .Select(x => new ProductionScheduleBO
                        {
                            ID = x.ID,
                            TransNo = x.TransNo,
                            TransDate = x.TransDate ?? new DateTime(),
                            ActualBatchSize = x.ActualBatchSize ?? 0,
                            ProductionGroupName = x.ProductionGroupName,
                            ProductionStartDate = x.ProductionStartDate ?? new DateTime(),
                            StandardBatchSize = x.StandardBatchSize ?? 0,
                            ProductionGroupID = x.ProductionGroupID ?? 0,
                            Name = x.Name,
                            RequestedStoreName = x.RequestedStore,
                            RequestedStoreID = x.RequestedStoreID ?? 0,
                            ProductionLocationName = x.LocationName,
                            ProductionLocationID = x.ProductionLocationID ?? 0,
                            IsDraft = x.IsDraft ?? false,
                            BatchNo = x.BatchNO,
                            IsCancelled = x.IsCancelled ?? false,

                            MachineID = (int)x.MachineID,
                            Machine = x.MachineName,
                            ProcessID = (int)x.ProcessID,
                            Process = x.Process,
                            MouldID = (int)x.MouldID,
                            MouldName = x.MouldName,
                            EndDate = x.EndDate,
                            EndTime = x.EndTime,
                            StartTime = x.StartTime,
                            Remarks = x.Remarks,
                            Items = GetProductionScheduleTransDetails(x.ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID)

                        }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionScheduleBO;
        }

        public List<ProductionScheduleItemBO> GetProductionScheduleTransDetails(int productionScheduleID, int finYear, int locationID, int applicationID)
        {
            List<ProductionScheduleItemBO> productionScheduleItemBOList = new List<ProductionScheduleItemBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    productionScheduleItemBOList = dbEntity.SpPGetProductionScheduleTransDetails(productionScheduleID, finYear, locationID, applicationID)
                        .Select(x => new ProductionScheduleItemBO
                        {
                            ProductionScheduleID = x.ProductionScheduleID,
                            ItemID = x.ItemID,
                            ItemName = x.ItemName,
                            Unit = x.Unit,
                            QtyMet = x.QtyMet ?? 0,
                            RequiredQty = x.RequiredQty,
                            RequiredDate = x.RequiredDate,
                            YogamQty = (decimal)x.YogamQty,
                            Remarks = x.Remarks,
                            ProductDefinitionTransID = (int)x.ProductDefinitionTransID,
                            StandardBatchSize = (decimal)x.StandardBatchSize,
                            UnitID = (int)x.UnitID,
                            StandardOutputQty = (decimal)x.StandardOutputQty,
                            MalayalamName = x.MalayalamName,
                            ProcessStage = x.ProcessStage,
                            ProductionSequence = (int)x.ProductionSequence,
                            UsageMode = x.UsageMode
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionScheduleItemBOList;
        }

        /// <summary>
        /// Create or Edit Production Schedule
        /// </summary>
        /// <param name="productionIssueBO"></param>
        /// <returns></returns>
        public int Save(ProductionScheduleBO productionScheduleBO)
        {
            int productionIssueID = 0;
            if (productionScheduleBO.ID <= 0)
            {   //Create
                productionIssueID = Create(productionScheduleBO);
                return productionIssueID;
            }
            else
            {       //Update
                if (Update(productionScheduleBO))
                {
                    return productionScheduleBO.ID;
                }
                return 0;
            }
        }

        /// <summary>
        /// Create new Production Schedule
        /// </summary>
        /// <param name="productionIssueBO"></param>
        /// <returns></returns>
        private int Create(ProductionScheduleBO productionIssueBO)
        {

            int productionScheduleID = 0;
            int stockRequisitionID = 0;
           
            productionIssueBO.CreatedDate = DateTime.Now;

            if (productionIssueBO.ProductionStartTime == new System.DateTime())
            {
                productionIssueBO.ProductionStartTime = System.DateTime.Now;
            }

            //SpPCreateProductionIssue
            using (var transaction = _entity.Database.BeginTransaction())
            {
                try
                {
                    string FormName = "ProductionSchedule";
                    ObjectParameter BatchNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter serialNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter productionScheduleIDOut = new ObjectParameter("ProductionScheduleID", typeof(int));
                    ObjectParameter stockRequisitionIDOut = new ObjectParameter("StockRequisitionID", typeof(int));

                    if (productionIssueBO.IsKalkan)
                    {
                        _entity.SpUpdateSerialNo("BatchMaster", "Kalkan", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, BatchNo);
                    }
                    else
                    {
                        _entity.SpUpdateSerialNo("BatchMaster", "Production", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, BatchNo);
                    }

                    if (productionIssueBO.IsDraft)
                    {
                        FormName = "DraftProductionSchedule";
                    }


                    _entity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, serialNo);


                    var purchaseIssue = _entity.SpPCreateProductionSchedule(serialNo.Value.ToString(), productionIssueBO.TransDate, productionIssueBO.ProductionGroupID,
                        productionIssueBO.ProductID, productionIssueBO.ProductionStartDate, productionIssueBO.ProductionStartTime, BatchNo.Value.ToString(), productionIssueBO.StandardBatchSize,
                        productionIssueBO.ActualBatchSize, productionIssueBO.ProductionLocationID, productionIssueBO.RequestedStoreID, productionIssueBO.IsDraft,
                        productionIssueBO.IsCompleted, productionIssueBO.ProductionStatus, productionIssueBO.IsAborted, productionIssueBO.Remarks, GeneralBO.CreatedUserID,
                        productionIssueBO.CreatedDate, productionIssueBO.MachineID, productionIssueBO.MouldID, productionIssueBO.ProcessID, productionIssueBO.EndDate, productionIssueBO.EndTime, productionIssueBO.StartTime,
                        GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, productionScheduleIDOut, stockRequisitionIDOut);

                    productionScheduleID = (productionScheduleIDOut != null && productionScheduleIDOut.Value != null) ? Convert.ToInt32(productionScheduleIDOut.Value) : 0;
                    stockRequisitionID = (stockRequisitionIDOut != null && stockRequisitionIDOut.Value != null) ? Convert.ToInt32(stockRequisitionIDOut.Value) : 0;
                    if (productionScheduleID > 0)
                    {
                        ////Saving Production schedule
                        if (productionIssueBO.Items != null)
                            foreach (var item in productionIssueBO.Items)
                            {
                                _entity.SpPCreateProductionScheduleTrans(productionScheduleID,
                                    stockRequisitionID,
                                    item.ProductDefinitionTransID,
                                    item.ItemID,
                                    item.RequiredQty,
                                    item.RequiredDate,
                                    item.Remarks,
                                    item.QtyMet,
                                    item.UnitID,
                                    item.YogamQty,
                                    item.StandardOutputQty,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
                            }
                    }

                    _entity.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return productionScheduleID;
        }

        /// <summary>
        /// Update existing Production Schedule
        /// </summary>
        /// <param name="productionIssueBO"></param>
        /// <returns></returns>
        private bool Update(ProductionScheduleBO productionIssueBO)
        {
            bool isUpdated = true;
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter stockRequisitionIDOut = new ObjectParameter("StockRequisitionID", typeof(int));

                        dbEntity.SpPUpdateProductionSchedule(productionIssueBO.ID, productionIssueBO.TransDate, productionIssueBO.ProductionGroupID,
                            productionIssueBO.ProductID, productionIssueBO.ProductionStartDate, productionIssueBO.ProductionStartDate, productionIssueBO.StandardBatchSize,
                            productionIssueBO.ActualBatchSize, productionIssueBO.ProductionLocationID, productionIssueBO.RequestedStoreID, productionIssueBO.IsDraft, productionIssueBO.MachineID,
                            productionIssueBO.MouldID, productionIssueBO.ProcessID, productionIssueBO.EndDate, productionIssueBO.EndTime, productionIssueBO.Remarks,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, stockRequisitionIDOut);

                        int stockRequisitionID = (stockRequisitionIDOut != null && stockRequisitionIDOut.Value != null) ? Convert.ToInt32(stockRequisitionIDOut.Value) : 0;

                        ////Saving Production schedule
                        if (productionIssueBO.Items != null)
                            foreach (var item in productionIssueBO.Items)
                            {
                                dbEntity.SpPCreateProductionScheduleTrans(
                                    productionIssueBO.ID,
                                    stockRequisitionID,
                                    item.ProductDefinitionTransID,
                                    item.ItemID,
                                    item.RequiredQty,
                                    item.RequiredDate,
                                    item.Remarks,
                                    item.QtyMet,
                                    item.UnitID,
                                    item.YogamQty,
                                    item.StandardOutputQty,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
                            }

                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        isUpdated = false;
                    }
                }
            }
            return isUpdated;
        }

        public List<ProductionScheduleBO> GetProductionSchedulesByItem(int ItemID)
        {

            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    return dbEntity.SpGetProductionSchedulesByItem(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID)
                        .Select(x => new ProductionScheduleBO
                        {
                            ID = x.ID,
                            TransNo = x.TransNo,
                            TransDate = x.TransDate ?? new DateTime(),
                            ActualBatchSize = x.ActualBatchSize ?? 0,
                            ProductionGroupName = x.ProductionGroupName,
                            ProductionStartDate = x.ProductionStartDate ?? new DateTime(),
                            BatchNo = x.BatchNo,
                            BatchID = x.BatchID != null ? (int)x.BatchID : 0
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public int Cancel(int ID, string Table)
        {
            ProductionEntities dEntity = new ProductionEntities();
            return dEntity.SpCancelTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }

        public DatatableResultBO GetProductionScheduleList(string Type, string TransNoHint, string TransDateHint, string ProductionGroupHint, string StartDateHint, string BatchsizeHint,string BatchHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetProductionSchedulesList(Type, TransNoHint, TransDateHint, ProductionGroupHint, StartDateHint, BatchsizeHint, BatchHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ActualBatchSize = item.ActualBatchSize ?? 0,
                                ProductionGroupName = item.ProductionGroupName,
                                ProductionStartDate = ((DateTime)item.ProductionStartDate).ToString("dd-MMM-yyyy"),
                                IsDraft = (bool)item.IsDraft,
                                IsCancelled = (bool)item.IsCancelled,
                                BatchNo = item.BatchNo,
                                Status = item.Status
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
