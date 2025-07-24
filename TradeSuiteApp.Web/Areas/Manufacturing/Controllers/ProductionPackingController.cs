using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers 
{
    public class ProductionPackingController : Controller
    {
        private IProductionPackingContract productionPackingBL;
        private IBatchContract batchBL;
        private IGeneralContract generalBL;
        private IWareHouseContract wareHouseBL;
        private IBatchTypeContract batchTypeBL;
        private IProductionIssue _productionIssueService;

        public ProductionPackingController()
        {
            productionPackingBL = new ProductionPackingBL();
            batchBL = new BatchBL();
            generalBL = new GeneralBL();
            wareHouseBL = new WarehouseBL();
            batchTypeBL = new BatchTypeBL();
        }
        // GET: Manufacturing/ProductionPacking
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Manufacturing/ProductMix/Details/5
        public ActionResult Details(int id)
        {
            PackingViewModel productionPacking = productionPackingBL.GetProductionPacking((int)id).Select(m => new PackingViewModel
            {
                ID = m.ID,
                TransNo = m.TransNo,
                ItemName = m.ItemName,
                UOM = m.UOM,
                BatchNo = m.BatchNo,
                BatchType = m.BatchType,
                PackedQty = m.PackedQty,
                BatchName = m.BatchName,
                IsDraft = m.IsDraft,
                BatchID = m.BatchID,
                ProductGroupID = m.ProductGroupID,
                Remarks=m.Remarks
            }).FirstOrDefault();

            productionPacking.DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore"));

            productionPacking.AvailableStock = batchBL.GetBatchWiseStockForPackingSemiFinishedGood(productionPacking.BatchID, productionPacking.DefaultPackingStoreID,productionPacking.ProductGroupID);

            productionPacking.Materials = productionPackingBL.GetProductionPackingMaterials(id).Select(m => new PackingMaterialModel
            {

                UOM = m.UOM,
                BatchNo = m.BatchNo,
                Store = m.WarehouseName,
                AvailableStock = m.AvailableStock,
                StandardQty = m.StandardQty,
                ActualQty = m.ActualQty,
                IssueQty = m.IssueQty,
                ItemName = m.ItemName,
                ItemCode = m.ItemCode,
                BatchType = m.BatchType,
                BatchTypeID = m.BatchTypeID,
                ItemID = m.ItemID,
                StoreID = m.StoreID == null ? Convert.ToInt16(generalBL.GetConfig("DefaultPackingStore")) : (int)m.StoreID,
                Remarks = m.Remarks,
                Variance =m.Variance

            }).ToList();
            ViewBag.StoreList = wareHouseBL.GetWareHouses();

            productionPacking.Output = productionPackingBL.GetProductionPackingOutput((int)id).Select(m => new PackingOutputModel()
            {
                ItemName = m.ItemName,
                BatchNo = m.BatchNo,
                PackedQty = m.PackedQty,
                BatchType = m.BatchType,
                BatchTypeID = m.BatchTypeID,
                ItemID = m.ItemID,
                BatchID = m.BatchID,
                Date = m.Date == null ? "" : General.FormatDate((DateTime)m.Date),
                IsQCCompleted = m.IsQCCompleted,
                IsDraft = m.IsDraft
            }).ToList();
            productionPacking.Process = productionPackingBL.GetPackingIssueProcess((int)id).Select(item => new PackingProcessModel()
            {
                Stage = item.Stage,
                ProcessName = item.ProcessName,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                BatchTypeID = item.BatchTypeID,
                SkilledLaboursStandard = item.SkilledLaboursStandard,
                SkilledLaboursActual = item.SkilledLaboursActual,
                UnSkilledLabourStandard = item.UnSkilledLabourStandard,
                UnSkilledLabourActual = item.UnSkilledLabourActual,
                MachineHoursStandard = item.MachineHoursStandard,
                MachineHoursActual = item.MachineHoursActual,
                DoneBy = item.DoneBy,
                Status = item.Status,
                PackingProcessDefinitionTransID = item.PackingProcessDefinitionTransID
            }
            ).ToList();

            return View(productionPacking);


        }

        // GET: Manufacturing/ProductMix/Create
        public ActionResult Create()
        {

            PackingViewModel ProductionPacking = new PackingViewModel();
            ProductionPacking.Date = General.FormatDate(DateTime.Now);
            ProductionPacking.TransNo = generalBL.GetSerialNo("PackingIssue", "Code");
            ProductionPacking.BatchList = new SelectList(batchBL.GetBatchList(0, ProductionPacking.DefaultPackingStoreID), "ID", "BatchNo");
            ProductionPacking.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            ProductionPacking.Materials = new List<PackingMaterialModel>();
            ProductionPacking.DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore"));
            ProductionPacking.StoreList = new SelectList(wareHouseBL.GetWareHouses(), "ID", "Name");
            return View(ProductionPacking);
        }

        // POST: Manufacturing/ProductMix/Create
        [HttpPost]
        public ActionResult Save(PackingViewModel Packing)
        {
            var result = new List<object>();
            try
            {
                if (Packing.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PackingBO Temp = productionPackingBL.GetProductionPacking(Packing.ID).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled || Temp.IsCompleted || Temp.IsAborted)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                PackingBO packingBO = new PackingBO()
                {
                    TransNo = Packing.TransNo,
                    TransDate = General.ToDateTime(Packing.Date),
                    IsCompleted = Packing.IsCompleted,
                    IsDraft = Packing.IsDraft,
                    ID = Packing.ID,
                    ItemID = Packing.ItemID,
                    ProductID = Packing.ProductID,
                    ProductGroupID = Packing.ProductGroupID,
                    BatchID = Packing.BatchID,
                    BatchSize = Packing.BatchSize,
                    PackedQty = Packing.PackedQty,
                    BatchTypeID = Packing.BatchTypeID,
                    Remarks=Packing.Remarks
                };
                List<PackingMaterialBO> Materials = new List<PackingMaterialBO>();
                PackingMaterialBO Material;
                foreach (var item in Packing.Materials)
                {
                    Material = new PackingMaterialBO()
                    {
                        PackingIssueID = item.PackingIssueID,
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        StoreID = item.StoreID,
                        AvailableStock = item.AvailableStock,
                        StandardQty = item.StandardQty,
                        IssueQty = item.IssueQty,
                        IsCompleted = Packing.IsCompleted,
                        AverageRate = 0,
                        PackingMaterialMasterID = item.PackingMaterialMasterID,
                        Remarks = item.Remarks,
                        IsAdditionalIssue=item.IsAdditionalIssue,
                        IsMaterialReturn=item.IsMaterialReturn,
                        Variance=item.Variance
                    };
                    Materials.Add(Material);
                }
                List<PackingOutputBO> Items = new List<PackingOutputBO>();
                PackingOutputBO PackingItem;
                foreach (var item in Packing.Output)
                {
                    PackingItem = new PackingOutputBO()
                    {
                        PackingIssueID = item.PackingIssueID,
                        ItemID = item.ItemID,
                        PackedQty = item.PackedQty,
                        BatchTypeID = item.BatchTypeID,
                        BatchID = item.BatchID,
                        StoreID = item.StoreID,
                        UnitID = item.UnitID,
                        ProductionSequence = 1,
                        IsCompleted = Packing.IsCompleted,
                        IsQCCompleted = item.IsQCCompleted,
                        Date = General.ToDateTime(item.Date),
                        IsDraft = Packing.IsDraft
                    };
                    Items.Add(PackingItem);
                }
                List<PackingProcessBO> PackingProcesses = new List<PackingProcessBO>();
                PackingProcessBO Process;
                if (Packing.Process != null)
                {
                    foreach (var item in Packing.Process)
                    {
                        Process = new PackingProcessBO
                        {
                            PackingIssueID = item.PackingIssueID,
                            Stage = item.Stage,
                            ProcessName = item.ProcessName,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            SkilledLaboursStandard = item.SkilledLaboursStandard,
                            SkilledLaboursActual = item.SkilledLaboursActual,
                            UnSkilledLabourStandard = item.UnSkilledLabourStandard,
                            UnSkilledLabourActual = item.UnSkilledLabourActual,
                            MachineHoursStandard = item.MachineHoursStandard,
                            MachineHoursActual = item.MachineHoursActual,
                            DoneBy = item.DoneBy,
                            Status = item.Status,
                            BatchTypeID = item.BatchTypeID,
                            PackingProcessDefinitionTransID = item.PackingProcessDefinitionTransID,
                            BatchSize = item.BatchSize
                        };
                        PackingProcesses.Add(Process);
                    }
                }
                if (productionPackingBL.Save(packingBO, Materials, Items, PackingProcesses))
                {
                    return Json(new { Status = "success", Message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (OutofStockException e)
            {
                result.Add(new { ErrorMessage = "Item out of stock" });
                generalBL.LogError("Manufacturing", "Productionpacking", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = "Unknown error" });
                generalBL.LogError("Manufacturing", "Productionpacking", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        //For Role privilege
        public ActionResult Complete()
        {
            return null;
        }

        // GET: Manufacturing/ProductMix/Edit/5
        public ActionResult Edit(int id)
        {
            PackingViewModel productionPacking = new PackingViewModel();
            try
            {
                productionPacking = productionPackingBL.GetProductionPacking((int)id).Select(m => new PackingViewModel
                {
                    ID = m.ID,
                    TransNo = m.TransNo,
                    ItemID = m.ItemID,
                    ProductID = m.ProductID,
                    ProductGroupID = m.ProductGroupID,
                    ItemName = m.ItemName,
                    UOM = m.UOM,
                    BatchID = m.BatchID,
                    BatchNo = m.BatchNo,
                    BatchType = m.BatchType,
                    PackedQty = m.PackedQty,
                    BatchName = m.BatchName,
                    BatchSize = m.BatchSize,
                    Date = General.FormatDate(m.TransDate),
                    IsCompleted = m.IsCompleted,
                    IsDraft = m.IsDraft,
                    IsAborted = m.IsAborted,
                    BatchTypeID = m.BatchTypeID,
                    IsBatchSuspended=m.IsBatchSuspended,
                    IsCancelled=m.IsCancelled,
                    Remarks=m.Remarks

                }).FirstOrDefault();
              
                productionPacking.DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore"));
                productionPacking.AvailableStock = batchBL.GetBatchWiseStockForPackingSemiFinishedGood(productionPacking.BatchID, productionPacking.DefaultPackingStoreID,productionPacking.ProductGroupID);
                if (!productionPacking.IsDraft || productionPacking.IsAborted || productionPacking.IsCompleted || productionPacking.IsCancelled)
                {
                    return RedirectToAction("Index");
                }
                productionPacking.DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore"));
                productionPacking.BatchList = new SelectList(batchBL.GetBatchList(productionPacking.ProductID, productionPacking.DefaultPackingStoreID), "ID", "BatchNo");
                productionPacking.BatchTypeList = new SelectList(
                                     batchTypeBL.GetBatchTypeList(), "ID", "Name");

                productionPacking.StoreList = new SelectList(wareHouseBL.GetWareHouses(), "ID", "Name");

                productionPacking.Materials = productionPackingBL.GetProductionPackingMaterials(id).Select(m => new PackingMaterialModel
                {
                    PackingIssueID = m.PackingIssueID,
                    UOM = m.UOM,
                    BatchNo = m.BatchNo,
                    Store = m.WarehouseName,
                    AvailableStock = m.CurrentStock,
                    StandardQty = m.StandardQty,
                    ActualQty = m.ActualQty,
                    IssueQty = m.IssueQty,
                    ItemName = m.ItemName,
                    ItemCode = m.ItemCode,
                    BatchType = m.BatchType,
                    BatchTypeID = m.BatchTypeID,
                    ItemID = m.ItemID,
                    IsDraft = productionPacking.IsDraft,
                    PackingMaterialMasterID = m.PackingMaterialMasterID,
                    UnitID = m.UnitID,
                    //    BatchList = new SelectList(batchBL.GetBatchList(m.ItemID, 0), "ID", "BatchNo"),
                    StoreID = m.StoreID == null ? Convert.ToInt16(generalBL.GetConfig("DefaultPackingStore")) : (int)m.StoreID,
                    Remarks = m.Remarks,
                    IsMaterialReturn=m.IsMaterialReturn,
                    IsAdditionalIssue=m.IsAdditionalIssue,
                    Variance=m.Variance
                }).ToList();



                productionPacking.Output = productionPackingBL.GetProductionPackingOutput((int)id).Select(m => new PackingOutputModel()
                {
                    PackingIssueID = m.PackingIssueID,
                    ItemName = m.ItemName,
                    BatchNo = m.BatchNo,
                    PackedQty = m.PackedQty,
                    BatchType = m.BatchType,
                    BatchTypeID = m.BatchTypeID,
                    ItemID = m.ItemID,
                    BatchID = m.BatchID,
                    Date = m.Date == null ? "" : General.FormatDate((DateTime)m.Date),
                    IsQCCompleted = m.IsQCCompleted,
                    IsDraft = m.IsDraft,
                }).ToList();
                productionPacking.Process = productionPackingBL.GetPackingIssueProcess((int)id).Select(item => new PackingProcessModel()
                {
                    PackingIssueID = item.PackingIssueID,
                    Stage = item.Stage,
                    ProcessName = item.ProcessName,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    SkilledLaboursStandard = item.SkilledLaboursStandard,
                    SkilledLaboursActual = item.SkilledLaboursActual,
                    UnSkilledLabourStandard = item.UnSkilledLabourStandard,
                    UnSkilledLabourActual = item.UnSkilledLabourActual,
                    MachineHoursStandard = item.MachineHoursStandard,
                    MachineHoursActual = item.MachineHoursActual,
                    BatchTypeID = item.BatchTypeID,
                    DoneBy = item.DoneBy,
                    Status = item.Status,
                    IsDraft = productionPacking.IsDraft,
                    BatchSize = item.BatchSize,
                    PackedQty = productionPacking.PackedQty
                }).ToList();

                ViewBag.StoreList = wareHouseBL.GetWareHouses();

            }
            catch (Exception e)
            {
                RedirectToAction("Index");
            }
            return View(productionPacking);
        }

        public PartialViewResult GetPackingMaterials(int ItemID, int BatchID, int ProductGroupID, int BatchTypeID, int PackedQty, string BatchType, int StoreID)
        {
            List<PackingMaterialModel> Materials = productionPackingBL.GetPackingMaterials(ItemID, BatchID, ProductGroupID, BatchTypeID, StoreID)
                .Select(
                    a => new PackingMaterialModel()
                    {
                        ItemCode = a.Code,
                        ItemName = a.Name,
                        ItemID = a.ItemID,
                        UOM = a.Unit,
                        UnitID = a.UnitID,
                        StandardQty = a.StandardQty,
                        ActualQty = a.StandardQty * PackedQty,
                        AvailableStock = a.AvailableStock,
                        BatchTypeID = BatchTypeID,
                        BatchType = BatchType,
                        StoreID = a.StoreID == null ? Convert.ToInt16(generalBL.GetConfig("DefaultPackingStore")) : (int)a.StoreID,
                        PackingMaterialMasterID = a.PackingMaterialMasterID
                    }
                ).ToList();
            return PartialView(Materials);
        }
        public JsonResult GetPackingOutputByBatchID(int BatchID)
        {
            List<PackingOutputModel> Output = productionPackingBL.GetPackingOutputByBatchID(BatchID)
                .Select(
                    m => new PackingOutputModel()
                    {
                        PackingIssueID = m.PackingIssueID,
                        ItemName = m.ItemName,
                        BatchNo = m.BatchNo,
                        PackedQty = m.PackedQty,
                        BatchType = m.BatchType,
                        BatchTypeID = m.BatchTypeID,
                        ItemID = m.ItemID,
                        BatchID = m.BatchID,
                        Date = m.Date == null ? "" : General.FormatDate((DateTime)m.Date),
                        IsQCCompleted = m.IsQCCompleted,
                    }
                ).ToList();
            return Json(Output, JsonRequestBehavior.AllowGet);

        }


        public PartialViewResult GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID, int PackedQty, string BatchType)
        {
            List<PackingProcessModel> Processes = productionPackingBL.GetPackingProcess(ItemID, ProductGroupID, BatchTypeID)
                .Select(
                    a => new PackingProcessModel()
                    {
                        Stage = a.Stage,
                        ProcessName = a.ProcessName,
                        UnSkilledLabourStandard = a.UnSkilledLabourStandard,
                        SkilledLaboursStandard = a.SkilledLaboursStandard,
                        MachineHoursStandard = a.MachineHoursStandard,
                        BatchTypeID = BatchTypeID,
                        PackingProcessDefinitionTransID = a.PackingProcessDefinitionTransID,
                        BatchSize = a.BatchSize,
                        PackedQty = PackedQty
                    }
                ).ToList();
            //ViewBag.StoreList = wareHouseBL.GetWareHouses();
            return PartialView(Processes);
        }

        public JsonResult GetProductionPackingOutput(int ItemID, int BatchID)
        {
            List<PackingOutputModel> productionOutput = productionPackingBL.GetProductionPackingOutput(ItemID, BatchID).Select(m => new PackingOutputModel()
            {
                ItemName = m.ItemName,
                BatchNo = m.BatchNo,
                PackedQty = m.PackedQty,
                BatchType = m.BatchType,
                BatchTypeID = m.BatchTypeID,
                ItemID = m.ItemID,
                BatchID = m.BatchID,
                Date = m.Date == null ? "" : General.FormatDate((DateTime)m.Date)

            }).ToList();


            return Json(productionOutput, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cancel(int PackingID, string Table)
        {
            try
            {

                productionPackingBL.CancelPacking(PackingID, Table);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetPackingList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string ProductionGroupHint = Datatable.Columns[3].Search.Value;
                string ItemNameHint = Datatable.Columns[4].Search.Value;
                string BatchNoHint = Datatable.Columns[5].Search.Value;
                string PackedQtyHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = productionPackingBL.GetPackingList(Type, TransNoHint, TransDateHint, ProductionGroupHint, ItemNameHint, BatchNoHint, PackedQtyHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(PackingViewModel Packing)
        {
            return Save(Packing);
        }
    }
}
