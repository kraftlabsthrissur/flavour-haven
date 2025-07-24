using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class ProductionScheduleViewModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDateStr { get; set; }
        public DateTime? TransDate
        {
            get
            {
                return TransDateStr.ToDateTime();
            }
            set { TransDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }


        public string ProductionStartDateStr { get; set; }
        public DateTime? ProductionStartDate
        {
            get
            {
                return ProductionStartDateStr.ToDateTime();
            }
            set { ProductionStartDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }


        public DateTime ProductionStartTime { get; set; }


        public int MouldID { get; set; }
        public string MouldName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal BatchSize { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
        public int ProductionLocationID { get; set; }
        public string ProductionLocationName { get; set; }
        public int UnitID { get; set; }
        public SelectList ProductionLocationList { get; set; }
        public SelectList StoreList { get; set; }
        public SelectList UnitList { get; set; }
        public List<ProductionScheduleItemModel> Items { get; set; }

        public int ProductionGroupID { get; set; }
        public int ProductID { get; set; }
        public bool IsKalkan { get; set; }

        public decimal StandardBatchSize { get; set; }
        public decimal ActualBatchSize { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsAborted { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductionStatus { get; set; }

        public string ProductionGroupName { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal QtyMet { get; set; }
        public decimal RequiredQty { get; set; }
        public DateTime RequiredDate { get; set; }
        public string UOM { get; set; }
        public string BatchNo { get; set; }
        public int BatchID { get; set; }
        public string Status { get; set; }
        public bool IsCancelled { get; set; }
        public int ProcessID { get; set; }
        public string Process { get; set; }
        public int MachineID { get; set; }
        public string Machine { get; set; }
        public string EndDate { get; set; }
        public SelectList ProcessList { get; set; }
        public SelectList MachineList { get; set; }

    }

    public class ProductionScheduleItemModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal YogamQty { get; set; }
        public decimal StandardBatchSize { get; set; }

        public string RequiredDateStr { get; set; }
        public DateTime? RequiredDate
        {
            get
            {
                return RequiredDateStr.ToDateTime();
            }
            set { RequiredDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public decimal RequiredQty { get; set; }
        public string Remarks { get; set; }
        public decimal QtyMet { get; set; }
        public int ProductDefinitionTransID { get; set; }
        public int UnitID { get; set; }
        public decimal StandardOutputQty { get; set; }
    }


    public static partial class Mapper
    {

        public static List<ProductionScheduleViewModel> MapToModelList(this List<ProductionScheduleBO> boList)
        {
            if (boList != null && boList.Count() > 0)
            {
                return (from x in boList
                        select new ProductionScheduleViewModel()
                        {
                            ID = x.ID,
                            TransNo = x.TransNo,
                            TransDate = x.TransDate,
                            ProductionGroupName = x.ProductionGroupName,
                            ProductionStartDate = x.ProductionStartDate,
                            StandardBatchSize = x.StandardBatchSize,
                            ActualBatchSize = x.ActualBatchSize,
                            ProductionGroupID = x.ProductionGroupID,
                            Name = x.Name,
                            Store = x.RequestedStoreName,
                            StoreID = x.RequestedStoreID,
                            ProductionLocationName = x.ProductionLocationName,
                            ProductionLocationID = x.ProductionLocationID,
                            ItemName = x.ItemName,
                            Unit = x.Unit,
                            QtyMet = x.QtyMet,
                            RequiredQty = x.RequiredQty,
                            RequiredDate = x.RequiredDate,
                            Remarks = x.Remarks,
                            IsDraft = x.IsDraft,
                            Items = x.Items.MapToModelList()
                        }).ToList();
            }
            else
                return new List<ProductionScheduleViewModel>();
        }

        public static ProductionScheduleViewModel MapToModel(this ProductionScheduleBO bo)
        {
            if (bo != null)
            {
                return new ProductionScheduleViewModel()
                {
                    ID = bo.ID,
                    TransNo = bo.TransNo,
                    TransDate = bo.TransDate,
                    ProductionGroupName = bo.ProductionGroupName,
                    ProductionStartDate = bo.ProductionStartDate,
                    StandardBatchSize = bo.StandardBatchSize,
                    ActualBatchSize = bo.ActualBatchSize,
                    ProductionGroupID = bo.ProductionGroupID,

                    Name = bo.Name,
                    Store = bo.RequestedStoreName,
                    StoreID = bo.RequestedStoreID,

                    ProductionLocationName = bo.ProductionLocationName,
                    ProductionLocationID = bo.ProductionLocationID,
                    ItemName = bo.ItemName,
                    Unit = bo.Unit,
                    QtyMet = bo.QtyMet,
                    RequiredQty = bo.RequiredQty,
                    RequiredDate = bo.RequiredDate,
                    Remarks = bo.Remarks,
                    IsDraft = bo.IsDraft,
                    BatchNo = bo.BatchNo,
                    IsCancelled = bo.IsCancelled,

                    MachineID=bo.MachineID,
                    Machine=bo.Machine,
                    ProcessID=bo.ProcessID,
                    Process=bo.Process,
                    MouldID=bo.MouldID,
                    MouldName=bo.MouldName,
                    EndDate=bo.EndDate,
                    EndTime=bo.EndTime,
                    StartTime=bo.StartTime,
                    Items = bo.Items.MapToModelList()
                };
            }
            else
                return new ProductionScheduleViewModel();
        }

        public static List<ProductionScheduleItemModel> MapToModelList(this List<ProductionScheduleItemBO> boList)
        {
            if (boList != null && boList.Count() > 0)
            {
                return (from x in boList
                        select new ProductionScheduleItemModel()
                        {
                            ID = x.ID,
                            ProductID = x.ProductID,
                            ItemID = x.ItemID,
                            ItemName = x.ItemName,
                            Unit = x.Unit,
                            YogamQty = x.YogamQty,
                            StandardBatchSize = x.StandardBatchSize,
                            Remarks = x.Remarks,
                            RequiredQty = x.RequiredQty,
                            QtyMet = x.QtyMet,
                            RequiredDate = x.RequiredDate,
                            ProductDefinitionTransID = x.ProductDefinitionTransID,
                            UnitID = x.UnitID,
                            StandardOutputQty = x.StandardOutputQty
                        }).ToList();
            }
            else
                return new List<ProductionScheduleItemModel>();
        }

        public static ProductionScheduleBO MapToBO(this ProductionScheduleViewModel model)
        {
            return new ProductionScheduleBO()
            {
                ActualBatchSize = model.ActualBatchSize,
                CreatedDate = model.CreatedDate,
                ID = model.ID,
                IsAborted = model.IsAborted,
                IsCompleted = model.IsCompleted,
                IsDraft = model.IsDraft,
                ProductID = model.ProductID,
                Items = model.Items.MapToBOList(),
                ProductionGroupID = model.ProductionGroupID,
                ProductionLocationID = model.ProductionLocationID,
                ProductionStartDate = (DateTime)model.ProductionStartDate,
                StartTime =model.StartTime,
                ProductionStatus = model.ProductionStatus,
                Remarks = model.Remarks,
                RequestedStoreID = model.StoreID,
                StandardBatchSize = model.StandardBatchSize,
                TransDate = (DateTime)model.TransDate,
                TransNo = model.TransNo,
                BatchNo = model.BatchNo,
                MachineID = model.MachineID,
                ProcessID = model.ProcessID,
                MouldID = model.MouldID,
                EndDate = model.EndDate,
                EndTime = model.EndTime,
                IsKalkan = model.IsKalkan
            };
        }

        public static List<ProductionScheduleItemBO> MapToBOList(this List<ProductionScheduleItemModel> modelList)
        {
            if (modelList != null && modelList.Count() > 0)
            {
                return (from m in modelList
                        select new ProductionScheduleItemBO

                        {
                            ID = m.ID,
                            ItemID = m.ItemID,
                            ItemName = m.ItemName,
                            ProductID = m.ProductID,
                            QtyMet = m.QtyMet,
                            Remarks = m.Remarks,
                            RequiredDate = (DateTime)m.RequiredDate,
                            RequiredQty = m.RequiredQty,
                            StandardBatchSize = m.StandardBatchSize,
                            Unit = m.Unit,
                            YogamQty = m.YogamQty,
                            UnitID = m.UnitID,
                            ProductDefinitionTransID = m.ProductDefinitionTransID,
                            StandardOutputQty = m.StandardOutputQty,
                        }).ToList();
            }
            else
                return new List<ProductionScheduleItemBO>();
        }
    }
}
