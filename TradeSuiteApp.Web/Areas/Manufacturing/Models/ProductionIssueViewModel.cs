using BusinessObject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class ProductionIssueListModel
    {
        public List<ProductionIssueViewModel> Completed;
        public List<ProductionIssueViewModel> OnGoing;
    }
    public class ProductionIssueViewModel
    {
        public int ProductionIssueID { get; set; }
        public string SlNo { get; set; }
        public string Date { get; set; }
        public DateTime? StartDate
        {
            get
            {
                return StartDateStr == "" ? null : StartDateStr.ToDateTime();
            }
            set { StartDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public string StartDateStr { get; set; }
        public string BatchNo { get; set; }
        //public decimal BatchSize { get; set; }
        public decimal StandardBatchSize { get; set; }
        public decimal ActualBatchSize { get; set; }
        public int UnitID { get; set; }
        //public string UnitName { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }

        public string UOM { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public int PartyID { get; set; }
        public decimal NetAmount { get; set; }
        //public int ID { get; set; }
        public decimal DefinitionQty { get; set; }
        public string BatchName { get; set; }
        public string TransNo { get; set; }
        public DateTime? StartTime { get { return StartTimeStr.ToTime(); } set { this.StartTimeStr = value == null ? "" : ((DateTime)value).ToTimeStr(); } }
        public string StartTimeStr { get; set; }
        public int Quantity { get; set; }
        public decimal ActualOutput { get; set; }
        public string ProductionType { get; set; }
        public string AdditionalName { get; set; }
        public string ProductionLocation { get; set; }
        public SelectList ReceiptStoreList { get; set; }
        public int ReceiptStoreID { get; set; }
        public string ReceiptStoreName { get; set; }  
        public SelectList BatchList { get; set; }
        public string Batch { get; set; }
        public DateTime? TransDate { get { return TransDateStr.ToDateTime(); } set { this.TransDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); } }
        public string TransDateStr { get; set; }


        //public string AddItemName { get; set; }
        //public string AddUOM { get; set; }
        public string AddIssueQty { get; set; }

        public int ProductionLocationID { get; set; }
        public decimal StandardOutput { get; set; }
        public decimal Variance { get; set; }

        public DateTime? EndDate { get { return EndDateStr.ToDateTime(); } set { this.EndDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); } }
        public string EndDateStr { get; set; }

        public DateTime? EndTime { get { return EndTimeStr.ToTime(); } set { this.EndTimeStr = value == null ? "" : ((DateTime)value).ToTimeStr(); } }
        public string EndTimeStr { get; set; }

        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public string ProductionStatus { get; set; }
        public bool IsAborted { get; set; }
        public decimal AverageCost { get; set; }
        public int ProductionGroupID { get; set; }
        public string ProductionGroupName { get; set; }
        public int ProductionSequence { get; set; }
        public string ProductionStageItem { get; set; }
        //public string Unit { get; set; }
        public string ProcessStage { get; set; }
        public bool IsKalkan { get; set; }
        public int CreatedUserID { get; set; }
        public int ProductionID { get; set; }

        public string ProductionScheduleName { get; set; }
        public int ProductionScheduleID { get; set; }
        public SelectList SequenceItemList { get; set; }
        public int SequenceItemID { get; set; }
        public int WarehouseID { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }

        public List<ProductionIssueMaterialItemModel> Materials { get; set; }

        public List<ProductionIssueProcessItemModel> Processes { get; set; }

        public List<ProductionAdditionalItemModel> Additional { get; set; }


        public SelectList ItemNameList { get; set; }

        public List<ProductionIssueItemModel> ProductionIssueItemModelList { get; set; }

        public List<MaterialTransModel> MaterialTransModelList { get; set; }
        public List<OutputModel> Output { get; set; }        //Grid showing item details
    }
    public class ProductionIssueMaterialItemModel
    {
        public int MaterialProductionIssueID { get; set; }
        //public string ItemName { get; set; }
        //public string IssuedQty { get; set; }
        public string BatchNo { get; set; }
        public string UOM { get; set; }
        public decimal StandardQty { get; set; }
        public decimal ActualQty { get; set; }
        public decimal Variance { get; set; }
        public int WareHouseID { get; set; }
        public string Store { get; set; }
        public string Remarks { get; set; }
        public decimal AverageRate { get; set; }
        //public string ActualQty { get; set; }
        public DateTime? IssueDate { get { return IssueDateStr.ToDateTime(); } set { this.IssueDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); } }
        public string IssueDateStr { get; set; }
        public int ProductionSequence { get; set; }
        public int RawMaterialId { get; set; }
        public string RawMaterialName { get; set; }
        public decimal RawMaterialQty { get; set; }
        public int RawMaterialUnitID { get; set; }
        public int QOM { get; set; }
        public string UnitName { get; set; }
        public decimal Stock { get; set; }
        public decimal IssueQty { get; set; }
        public decimal AdditionalIssueQty { get; set; }
        public int BatchID { get; set; }
        public bool IsQcRequired { get; set; }
        public SelectList BatchSelectList { get; set; }
        public int ProductDefinitionTransID { get; set; }
        public bool? IsAdditionalIssue { get; set; }
        public decimal ActualOutPutForStdBatch { get; set; }
        public string Category { get; set; }
        public bool IsSubProduct { get; set; }
        public int BatchTypeID { get; set; }
    }
    public class ProductionIssueProcessItemModel
    {
        public int ProcessProductionIssueID { get; set; }
        public int ProductionIssueID { get; set; }
        public string Stage { get; set; }
        public string ProcessName { get; set; }
        public DateTime? StartTime { get { return StartTimeStr.ProcessToTime(); } set { this.StartTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }
        public string StartTimeStr { get; set; }
        public DateTime? StartDate { get { return StartDateStr.ProcessToDate(); } set { this.StartDateStr = value == null ? null : ((DateTime)value).ToDateStr(); } }
        public string StartDateStr { get; set; }
        public int InputQuantity { get; set; }
        public DateTime? EndTime { get { return EndTimeStr.ProcessToTime(); } set { this.EndTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }
        public string EndTimeStr { get; set; }
        public DateTime? EndDate { get { return EndDateStr.ProcessToDate(); } set { this.EndDateStr = value == null ? null : ((DateTime)value).ToDateStr(); } }
        public string EndDateStr { get; set; }
        public int OutputQty { get; set; }
        public decimal SkilledLaboursStandard { get; set; }
        public decimal UnSkilledLabourStandard { get; set; }
        public decimal MachineHoursStandard { get; set; }

        public decimal SkilledLaboursStandardForStandardBatchSize { get; set; }
        public decimal UnSkilledLabourStandardForStandardBatchSize { get; set; }
        public decimal MachineHoursStandardForStandardBatchSize { get; set; }

        public decimal MachineHoursActual { get; set; }
        public int ProductionSequence { get; set; }

        public decimal SkilledLaboursActual { get; set; }
        public decimal UnSkilledLabourActual { get; set; }

        public string DoneBy { get; set; }
        public int StatusId { get; set; }
        public SelectList StatusList { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public decimal AverageProcessCost { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProcessDefinitionTransID { get; set; }

        //public decimal UnSkilledLabourHours { get; set; }
        //public decimal SkilledLabourHours { get; set; }
        //public decimal MachineHours { get; set; }


    }

    public class ProductionAdditionalItemModel
    {
        public string Item { get; set; }
        public string StandardQty { get; set; }
        public string ActualOutput { get; set; }
        public string Variance { get; set; }
        public SelectList ReceiptStoreList { get; set; }
        public int ReceiptStoreID { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }


    }
    public class ProductionIssueItemModel
    {
        public int ItemID { get; set; }
        public string ProductName { get; set; }
        public string UOM { get; set; }
        public int QOM { get; set; }
        public string UnitName { get; set; }
        public decimal BatchSize { get; set; }
        public decimal StandardOutput { get; set; }
        public string ProcessStage { get; set; }
        public int ProductionGroupId { get; set; }
        public int ProductionSequence { get; set; }
        public int UnitID { get; set; }

    }
    public class AdditionalIssueItemModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public decimal Stock { get; set; }
        public decimal QtyUnderQC { get; set; }
        public decimal QtyOrdered { get; set; }
    }
    public class MaterialReturnItemModel
    {
        public int RawMaterialId { get; set; }
        public string RawMaterialName { get; set; }
        public decimal RawMaterialQty { get; set; }
        public int RawMaterialUnitID { get; set; }
        public string UOM { get; set; }
        public int QOM { get; set; }
        public string UnitName { get; set; }
        public decimal Stock { get; set; }
    }
    public class MaterialTransModel
    {
        public int ID { get; set; }
        public int ProductionIssueID { get; set; }
        public int ProductionIssueMaterialsID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public string BatchNo { get; set; }
        public decimal IssueQty { get; set; }
        public decimal AverageRate { get; set; }
        public string Remarks { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssueDateStr { get { return IssueDate.ToDateStr(); } set { } }
    }
    public class OutputModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int ProductionSequence { get; set; }
        public decimal StandardBatchSize { get; set; }
        public decimal ActualBatchSize { get; set; }
        public decimal StandardOutput { get; set; }
        public decimal ActualOutput { get; set; }
        public decimal Variance { get; set; }
        public int StoreID { get; set; }
        public string Store { get; set; }
        public DateTime? StartDate { get { return StartDateStr.ToDateTime(); } set { this.StartDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); } }
        public string StartDateStr { get; set; }
        public DateTime? StartTime { get { return StartTimeStr.ToTime(); } set { this.StartTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }
        public string StartTimeStr { get; set; }
        public DateTime? EndDate { get { return EndDateStr.ToDateTime(); } set { this.EndDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); } }
        public string EndDateStr { get; set; }
        public DateTime? EndTime { get { return EndTimeStr.ToTime(); } set { this.EndTimeStr = value == null ? null : ((DateTime)value).ToTimeStr(); } }
        public string EndTimeStr { get; set; }
        public bool IsCompleted { get; set; }
        public int ProductionID { get; set; }
        public int ProductionIssueID { get; set; }
        public bool IsQcRequired { get; set; }
        public string ProductionStatus { get; set; }
        public bool IsSubProduct { get; set; }
        public string ProcessStage { get; set; }
    }
    public static partial class Mapper
    {

        /// <summary>
        /// Map ProductionIssueViewModel to ProductionIssueBO
        /// </summary>
        /// <param name="productionViewModel"></param>
        /// <returns></returns>
        public static ProductionIssueBO MapToBO(this ProductionIssueViewModel productionViewModel)
        {
            ProductionIssueBO productionIssueBO = new ProductionIssueBO()
            {
                ProductionIssueID = productionViewModel.ProductionIssueID,
                TransNo = productionViewModel.TransNo,
                TransDate = (DateTime)productionViewModel.TransDate,
                BatchNo = productionViewModel.BatchNo,
                StandardBatchSize = productionViewModel.StandardBatchSize,
                ActualBatchSize = productionViewModel.ActualBatchSize,
                StartDate = productionViewModel.StartDate,
                StartTime = productionViewModel.StartTime,
                ProductionLocationID = productionViewModel.ProductionLocationID,
                ProductionScheduleID = productionViewModel.ProductionScheduleID,
                IsDraft = productionViewModel.IsDraft,
                IsCompleted = productionViewModel.IsCompleted,
                ProductionStatus = productionViewModel.ProductionStatus,
                IsAborted = productionViewModel.IsAborted,
                AverageCost = productionViewModel.AverageCost,
                ProductionGroupID = productionViewModel.ProductionGroupID,
                ProductionID = productionViewModel.ProductionID,
                ProductionSequence = productionViewModel.ProductionSequence,
                MaterialProductionIssueBOList = productionViewModel.Materials.MapToBOList(),
                ProcessProductionIssueBOList = productionViewModel.Processes.MapToBOList(),
                OutputBOList = productionViewModel.Output.MapToBOList(),
                MaterialTransBOList = productionViewModel.MaterialTransModelList.MapToBOList(),
                IsKalkan = productionViewModel.IsKalkan
            };

            return productionIssueBO;
        }

        public static ProductionIssueViewModel MapToModel(this ProductionIssueBO productionIssueBO)
        {
            if (productionIssueBO != null)
            {
                return new ProductionIssueViewModel()
                {
                    ProductionIssueID = productionIssueBO.ProductionIssueID,
                    TransNo = productionIssueBO.TransNo,
                    TransDate = productionIssueBO.TransDate,
                    BatchNo = productionIssueBO.BatchNo,
                    StandardBatchSize = productionIssueBO.StandardBatchSize,
                    ActualBatchSize = productionIssueBO.ActualBatchSize,
                    StartDate = productionIssueBO.StartDate,
                    StartTime = productionIssueBO.StartTime,
                    ProductionLocationID = productionIssueBO.ProductionLocationID,
                    ProductionLocation = productionIssueBO.ProductionLocation,
                    IsDraft = productionIssueBO.IsDraft,
                    IsCompleted = productionIssueBO.IsCompleted,
                    ProductionStatus = productionIssueBO.ProductionStatus,
                    IsAborted = productionIssueBO.IsAborted,
                    AverageCost = productionIssueBO.AverageCost,
                    CreatedUserID = productionIssueBO.CreatedUserID,
                    UOM = productionIssueBO.Unit,
                    ProductionID = productionIssueBO.ProductionID,
                    ProductionSequence = productionIssueBO.ProductionSequence,
                    ProductionGroupID = productionIssueBO.ProductionGroupID,
                    ProductionGroupName = productionIssueBO.ProductionGroupName,
                    ProductionScheduleID = productionIssueBO.ProductionScheduleID,
                    ProductionScheduleName=productionIssueBO.ProductionScheduleName,
                    Materials = productionIssueBO.MaterialProductionIssueBOList != null ? productionIssueBO.MaterialProductionIssueBOList.MapToModelList() : new List<ProductionIssueMaterialItemModel>(),
                    Processes = productionIssueBO.ProcessProductionIssueBOList != null ? productionIssueBO.ProcessProductionIssueBOList.MapToModelList(productionIssueBO.ActualBatchSize / productionIssueBO.StandardBatchSize) : new List<ProductionIssueProcessItemModel>(),
                    Output = productionIssueBO.OutputBOList != null ? productionIssueBO.OutputBOList.MapToModelList() : new List<OutputModel>()
                };
            }
            return new ProductionIssueViewModel();
        }

        /// <summary>
        /// Map ProductionIssueBO list to ProductionIssueViewModel List
        /// </summary>
        /// <param name="productionIssueBOList"></param>
        /// <returns></returns>
        public static List<ProductionIssueViewModel> MapToModelList(this List<ProductionIssueBO> productionIssueBOList)
        {
            if (productionIssueBOList != null && productionIssueBOList.Count > 0)
            {
                return (from x in productionIssueBOList
                        select new ProductionIssueViewModel
                        {
                            TransNo = x.TransNo,
                            TransDate = x.TransDate,
                            ProductionGroupID = x.ProductionGroupID,
                            ProductionGroupName = x.ProductionGroupName,
                            ProductionIssueID = x.ProductionIssueID,
                            StartDate = x.StartDate,
                            BatchNo = x.BatchNo,
                            ProductionStageItem = x.ProductionStageItem,
                            UOM = x.Unit,
                            UnitID = x.UnitID,
                            ProductionSequence = x.ProductionSequence,
                            ProcessStage = x.ProcessStage,
                            StandardBatchSize = x.StandardBatchSize,
                            ActualBatchSize = x.ActualBatchSize,
                            ProductionID = x.ProductionID,
                            IsCompleted = x.IsCompleted
                        }).ToList();
            }
            else
                return new List<ProductionIssueViewModel>();
        }

        /// <summary>
        /// Map ProductionIssueItemModel to ItemBO
        /// </summary>
        /// <param name="productionIssueItemModelList"></param>
        public static void MapToBOList(this List<ProductionIssueItemModel> productionIssueItemModelList)
        {
            throw new Exception("Not implemented");
        }

        /// <summary>
        /// Map ItemBO to ProductionIssueModel
        /// </summary>
        /// <param name="itemBOList"></param>
        /// <returns></returns>
        public static List<ProductionIssueItemModel> MapToModelList(this List<ItemBO> itemBOList)
        {
            if (itemBOList != null && itemBOList.Count() > 0)
            {
                return (from itm in itemBOList
                        select new ProductionIssueItemModel()
                        {
                            ItemID = itm.ID,
                            ProductName = itm.ProductName,
                            UOM = itm.UOM,
                            QOM = itm.QOM,
                            UnitName = itm.UnitName,
                            BatchSize = itm.BatchSize,
                            StandardOutput = itm.StandardOutput,
                            ProcessStage = itm.ProcessStage,
                            ProductionGroupId = itm.ProductionGroupId,
                            ProductionSequence = itm.ProductionSequence,
                            UnitID = itm.UnitID
                        }).ToList();
            }
            else
                return new List<ProductionIssueItemModel>();
        }

        public static List<ProductionOutputBO> MapToBOList(this List<OutputModel> OutputList)
        {
            return OutputList != null ? (from a in OutputList
                                         select new ProductionOutputBO
                                         {
                                             ItemID = a.ItemID,
                                             ItemName = a.ItemName,
                                             ProductionSequence = a.ProductionSequence,
                                             StandardBatchSize = a.StandardBatchSize,
                                             ActualBatchSize = a.ActualBatchSize,
                                             StandardOutput = a.StandardOutput,
                                             ActualOutput = a.ActualOutput,
                                             Variance = a.Variance,
                                             ReceiptStoreID = a.StoreID,
                                             StartDate = a.StartDate,
                                             StartTime = a.StartTime,
                                             EndDate = a.EndDate,
                                             EndTime = a.EndTime,
                                             IsCompleted = a.IsCompleted,
                                             ProductionID = a.ProductionID,
                                             ProductionStatus = a.ProductionStatus,
                                             ProductionIssueID = a.ProductionIssueID,
                                             IsQcRequired = a.IsQcRequired,
                                             IsSubProduct = a.IsSubProduct,
                                             ProcessStage = a.ProcessStage
                                         }).ToList() : new List<ProductionOutputBO>();
        }
        /// <summary>
        /// Map ProductionMaterialItemModel to BO
        /// </summary>
        /// <param name="productionMaterialItemModelList"></param>
        public static List<MaterialProductionIssueBO> MapToBOList(this List<ProductionIssueMaterialItemModel> productionMaterialItemModelList)
        {
            return productionMaterialItemModelList != null ? (from p in productionMaterialItemModelList
                                                              select new MaterialProductionIssueBO
                                                              {
                                                                  MaterialProductionIssueID = p.MaterialProductionIssueID,
                                                                  RawMaterialId = p.RawMaterialId,
                                                                  RawMaterialQty = p.RawMaterialQty,
                                                                  StandardQty = p.StandardQty,
                                                                  ActualStandardQty = p.ActualQty,
                                                                  IssueQty = p.IssueQty,
                                                                  AdditionalIssueQty = p.AdditionalIssueQty,
                                                                  Variance = p.Variance,
                                                                  BatchNo = p.BatchNo,
                                                                  BatchID = p.BatchID,
                                                                  WareHouseID = p.WareHouseID,
                                                                  IssueDate = p.IssueDateStr.ToDateTime(),
                                                                  Remarks = p.Remarks,
                                                                  AverageRate = p.AverageRate,
                                                                  ProductionSequence = p.ProductionSequence,
                                                                  ProductDefinitionTransID = p.ProductDefinitionTransID,
                                                                  IsAdditionalIssue = p.IsAdditionalIssue,
                                                                  RawMaterialUnitID=p.RawMaterialUnitID,
                                                                  BatchTypeID=p.BatchTypeID

                                                              }).ToList() : new List<MaterialProductionIssueBO>();

        }

        /// <summary>
        /// Map MaterialProductionIssueBO to Model
        /// </summary>
        /// <param name="itemBOList"></param>
        /// <returns></returns>
        public static List<ProductionIssueMaterialItemModel> MapToModelList(this List<MaterialProductionIssueBO> itemBOList)
        {
            if (itemBOList != null && itemBOList.Count() > 0)
            {
                return (from itm in itemBOList
                        select new ProductionIssueMaterialItemModel()
                        {
                            RawMaterialId = itm.RawMaterialId,
                            RawMaterialName = itm.RawMaterialName,
                            RawMaterialQty = itm.RawMaterialQty,
                            RawMaterialUnitID = itm.RawMaterialUnitID,
                            UOM = itm.UOM,
                            QOM = itm.QOM,
                            UnitName = itm.UnitName,
                            Stock = itm.Stock,
                            ProductionSequence = itm.ProductionSequence,
                            MaterialProductionIssueID = itm.MaterialProductionIssueID,
                            StandardQty = itm.StandardQty,
                            ActualQty = itm.ActualStandardQty,
                            IssueQty = itm.IssueQty,
                            Variance = itm.Variance,
                            BatchID = itm.BatchID,
                            BatchNo = itm.BatchNo,
                            WareHouseID = itm.StoreID,
                            Store = itm.Store,
                            IssueDate = itm.IssueDate,
                            Remarks = itm.Remarks,
                            IsQcRequired = itm.IsQcRequired,
                            ProductDefinitionTransID = itm.ProductDefinitionTransID,
                            IsAdditionalIssue = itm.IsAdditionalIssue,
                            ActualOutPutForStdBatch=itm.ActualOutPutForStdBatch,
                            Category=itm.Category,
                            IsSubProduct=itm.IsSubProduct
                            //BatchSelectList=new SelectList(itm.Batches.Select(x=>new SelectListItem() { Text=x.BatchNo, Value=x.ID.ToString(), Selected=x.ID==itm.BatchID}))
                            //  BatchSelectList = new SelectList(itm.Batches, "ID", "BatchNo", itm.BatchID.ToString())

                        }).ToList();
            }
            else
                return new List<ProductionIssueMaterialItemModel>();
        }

        /// <summary>
        /// Map ProductionProcesItemModel to BO
        /// </summary>
        /// <param name="productionMaterialItemModelList"></param>
        public static List<ProcessProductionIssueBO> MapToBOList(this List<ProductionIssueProcessItemModel> productionProcesItemModelList)
        {
            List<ProcessProductionIssueBO> processesProductionIssueBO = new List<ProcessProductionIssueBO>();
            ProcessProductionIssueBO processBO;
            if (productionProcesItemModelList != null)
            {
                foreach (var p in productionProcesItemModelList)
                {

                    processBO = new ProcessProductionIssueBO()
                    {
                        //   Stage = string.Concat(p.StartDateStr, ' ', p.StartTimeStr),
                        //ProcessName = string.Concat(p.EndDateStr, ' ', p.EndTimeStr),
                        ProcessProductionIssueID = p.ProcessProductionIssueID,
                        ProductionSequence = p.ProductionSequence,
                        Stage = p.Stage,
                        ProcessName = p.ProcessName,
                        //  StartTime = General.ToDateTime(string.Concat(p.StartDateStr, ' ', p.StartTimeStr), "datetime"),
                        //  EndTime = (p.EndDateStr == null) ? null General.ToDateTime(string.Concat(p.EndDateStr, ' ', p.EndTimeStr), "datetime"),

                        SkilledLabourHours = p.SkilledLaboursStandard,
                        SkilledLabourActualHours = p.SkilledLaboursActual,
                        UnSkilledLabourHours = p.UnSkilledLabourStandard,
                        UnSkilledLabourActualHours = p.UnSkilledLabourActual,
                        MachineHours = p.MachineHoursStandard,
                        MachineActualHours = p.MachineHoursActual,
                        Status = p.Status,
                        DoneBy = p.DoneBy,
                        Remarks = p.Remarks,
                        AverageProcessCost = p.AverageProcessCost,
                        ProcessDefinitionTransID = p.ProcessDefinitionTransID
                    };
                    if (p.StartDateStr != null && p.StartTimeStr != null)
                    {
                        processBO.StartTime = General.ToDateTime(string.Concat(p.StartDateStr, ' ', p.StartTimeStr), "datetime");
                    }
                    else if (p.StartDateStr != null)
                    {
                        processBO.StartTime = General.ToDateTime(p.StartDateStr);

                    }
                    if (p.EndDateStr != null && p.EndTimeStr != null)
                    {
                        processBO.EndTime = General.ToDateTime(string.Concat(p.EndDateStr, ' ', p.EndTimeStr), "datetime");
                    }
                    else if (p.EndDateStr != null)
                    {
                        processBO.EndTime = General.ToDateTime(p.EndDateStr);

                    }
                    processesProductionIssueBO.Add(processBO);
                }

                return processesProductionIssueBO;
            }
            else
            {
                return processesProductionIssueBO;

            }
        }


        /// <summary>
        /// Map MaterialProductionIssueBO to Model
        /// </summary>
        /// <param name="itemBOList"></param>
        /// <returns></returns>
        public static List<ProductionIssueProcessItemModel> MapToModelList(this List<ProcessProductionIssueBO> itemBOList, decimal NumberOfTimes)
        {

            if (itemBOList != null && itemBOList.Count() > 0)
            {
                return (from itm in itemBOList
                        select new ProductionIssueProcessItemModel()
                        {
                            Stage = itm.Stage,
                            ProcessName = itm.ProcessName,
                            UnSkilledLabourStandardForStandardBatchSize = itm.UnSkilledLabourHours / NumberOfTimes,
                            UnSkilledLabourStandard = itm.UnSkilledLabourHours,
                            UnSkilledLabourActual = itm.UnSkilledLabourActualHours,
                            SkilledLaboursStandardForStandardBatchSize = itm.SkilledLabourHours / NumberOfTimes,
                            SkilledLaboursStandard = itm.SkilledLabourHours,
                            SkilledLaboursActual = itm.SkilledLabourActualHours,
                            MachineHoursStandardForStandardBatchSize = itm.MachineHours / NumberOfTimes,
                            MachineHoursStandard = itm.MachineHours,
                            MachineHoursActual = itm.MachineActualHours,
                            ProductionSequence = itm.ProductionSequence,
                            ProcessProductionIssueID = itm.ProcessProductionIssueID,
                            ProductionIssueID = itm.ProductionIssueID,
                            StartTime = itm.StartTime,
                            EndTime = itm.EndTime,
                            StartDate = itm.StartTime,
                            EndDate = itm.EndTime,
                            Status = itm.Status,
                            Remarks = itm.Remarks,
                            AverageProcessCost = itm.AverageProcessCost,
                            CreateUserID = itm.CreateUserID,
                            CreatedDate = itm.CreatedDate,
                            DoneBy = itm.DoneBy,
                            ProcessDefinitionTransID = itm.ProcessDefinitionTransID
                        }).ToList();
            }
            else
            {
                return new List<ProductionIssueProcessItemModel>();
            }

        }


        public static List<OutputModel> MapToModelList(this List<ProductionOutputBO> OutputBOList)
        {
            if (OutputBOList != null && OutputBOList.Count() > 0)
            {
                return (from a in OutputBOList
                        select new OutputModel()
                        {
                            ProductionID = a.ProductionID,
                            ProductionSequence = a.ProductionSequence,
                            ItemID = a.ItemID,
                            StandardBatchSize = a.StandardBatchSize,
                            ActualBatchSize = a.ActualBatchSize,
                            StandardOutput = a.StandardOutput,
                            ActualOutput = a.ActualOutput,
                            Variance = a.Variance,
                            StartDate = a.StartDate,
                            StartTime = a.StartTime,
                            EndDate = a.EndDate,
                            EndTime = a.EndTime,
                            StoreID = a.ReceiptStoreID,
                            IsCompleted = a.IsCompleted,
                            ProductionStatus = a.ProductionStatus,
                            ItemName = a.ItemName,
                            Store = a.ReceiptStore,
                            ProductionIssueID = a.ProductionIssueID,
                            IsSubProduct = a.IsSubProduct,
                            ProcessStage = a.ProcessStage,
                            Unit=a.Unit
                        }).ToList();
            }
            else
                return new List<OutputModel>();
        }
        /// <summary>
        /// Map AdditionalIssueItemModel to BO
        /// </summary>
        /// <param name="productionMaterialItemModelList"></param>
        public static void MapToBOList(this List<AdditionalIssueItemModel> productionProcesItemModelList)
        {
            throw new Exception("Not implemented");
        }

        /// <summary>
        /// Map AdditionalIssueItemBO to Model
        /// </summary>
        /// <param name="itemBOList"></param>
        /// <returns></returns>
        public static List<AdditionalIssueItemModel> MapToModelList(this List<AdditionalIssueItemBO> itemBOList)
        {
            if (itemBOList != null && itemBOList.Count() > 0)
            {
                return (from a in itemBOList
                        select new AdditionalIssueItemModel()
                        {
                            ID = a.ID,
                            Code = a.Code,
                            Name = a.Name,
                            Unit = a.Unit,
                            UnitID = a.UnitID,
                            Stock = a.Stock,
                            QtyUnderQC = a.QtyUnderQC,
                            QtyOrdered = a.QtyOrdered
                        }).ToList();
            }
            else
                return new List<AdditionalIssueItemModel>();
        }

        /// <summary>
        /// Map AdditionalIssueItemModel to BO
        /// </summary>
        /// <param name="productionMaterialItemModelList"></param>
        public static void MapToBOList(this List<MaterialReturnItemModel> productionProcesItemModelList)
        {
            throw new Exception("Not implemented");
        }

        /// <summary>
        /// Map AdditionalIssueItemBO to Model
        /// </summary>
        /// <param name="itemBOList"></param>
        /// <returns></returns>
        public static List<MaterialReturnItemModel> MapToModelList(this List<MaterialReturnItemBO> itemBOList)
        {
            if (itemBOList != null && itemBOList.Count() > 0)
            {
                return (from a in itemBOList
                        select new MaterialReturnItemModel()
                        {
                            RawMaterialId = a.RawMaterialId,
                            RawMaterialName = a.RawMaterialName,
                            RawMaterialQty = a.RawMaterialQty,
                            RawMaterialUnitID = a.RawMaterialUnitID,
                            UOM = a.UOM,
                            QOM = a.QOM,
                            UnitName = a.UnitName,
                            Stock = a.Stock
                        }).ToList();
            }
            else
                return new List<MaterialReturnItemModel>();
        }

        /// <summary>
        /// Map AdditionalIssueItemModel to BO
        /// </summary>
        /// <param name="materialTransModelList"></param>
        public static List<MaterialTransBO> MapToBOList(this List<MaterialTransModel> materialTransModelList)
        {
            return materialTransModelList != null ? (from m in materialTransModelList
                                                     select new MaterialTransBO
                                                     {
                                                         ProductionIssueMaterialsID = m.ProductionIssueMaterialsID,
                                                         ItemID = m.ItemID,
                                                         IssueQty = m.IssueQty,
                                                         IssueDate = m.IssueDate,
                                                         AverageRate = m.AverageRate,
                                                         Remarks = m.Remarks,
                                                         BatchID = m.BatchID,
                                                         BatchNo = m.BatchNo,

                                                     }).ToList() : new List<MaterialTransBO>();
        }
        /// <summary>
        /// Map AdditionalIssueItemBO to Model
        /// </summary>
        /// <param name="itemBOList"></param>
        /// <returns></returns>
        public static List<MaterialTransModel> MapToModelList(this List<MaterialTransBO> itemBOList)
        {
            if (itemBOList != null && itemBOList.Count() > 0)
            {
                return (from a in itemBOList
                        select new MaterialTransModel()
                        {
                            AverageRate = a.AverageRate,
                            BatchID = a.BatchID,
                            BatchNo = a.BatchNo,
                            ID = a.ID,
                            IssueDate = a.IssueDate,
                            IssueQty = a.IssueQty,
                            ItemID = a.ItemID,
                            ItemName = a.ItemName,
                            ProductionIssueID = a.ProductionIssueID,
                            ProductionIssueMaterialsID = a.ProductionIssueMaterialsID,
                            Remarks = a.Remarks,
                            Unit = a.Unit,
                            UnitID = a.UnitID
                        }).ToList();
            }
            else
                return new List<MaterialTransModel>();
        }
    }

    public static class ExtensionHelper
    {
        public static DateTime? ToDateTime(this string dateStr, string format = "dd-MM-yyyy")
        {
            //if (string.IsNullOrEmpty(format))
            //format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            if (!string.IsNullOrEmpty(dateStr))
            {
                return DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
            }
            return null;
        }
        public static string ToDateStr(this DateTime date, string format = "dd-MM-yyyy")
        {
            if (date != null && date != new DateTime())
            {
                return date.ToString(format);
            }
            //else return new DateTime(1900, 01, 01).ToString(format);
            else return string.Empty;
        }
        public static string ToTimeStr(this DateTime date, string format = "hh:mm tt")
        {
            if (date != null && date != new DateTime())
            {
                return date.ToString(format);
            }
            //else return new DateTime(1900, 01, 01).ToString(format);
            else return string.Empty;
        }

        public static DateTime? ToTime(this string timeStr)
        {
            if (!string.IsNullOrEmpty(timeStr))
            {
                timeStr = "01-01-1900 " + timeStr;
                return DateTime.ParseExact(timeStr, "dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
            return null;
        }
        public static DateTime? ProcessToTime(this string timeStr)
        {
            if (!string.IsNullOrEmpty(timeStr))
            {
                //   timeStr = "01-01-1900 " + timeStr;
                return DateTime.ParseExact(timeStr, "hh:mm tt", CultureInfo.InvariantCulture);
            }
            return null;
        }
        public static DateTime? ProcessToDate(this string timeStr)
        {
            if (!string.IsNullOrEmpty(timeStr))
            {
                //   timeStr = "01-01-1900 " + timeStr;
                return DateTime.ParseExact(timeStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //return (DateTime)timeStr;
            }
            return null;
        }
    }
}