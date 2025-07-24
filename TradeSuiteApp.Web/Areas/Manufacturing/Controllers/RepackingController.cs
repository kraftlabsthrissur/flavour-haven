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
    public class RepackingController : Controller
    {
        private IRepakingContract repackingBL;
        private IBatchContract batchBL;
        private IGeneralContract generalBL;
        private IProductionIssue _productionIssueService;
        private IBatchTypeContract batchTypeBL;
        public RepackingController(IProductionIssue productionIssueService)
        {
            batchBL = new BatchBL();
            generalBL = new GeneralBL();
            repackingBL = new RepackingBL();
            batchTypeBL = new BatchTypeBL();
            this._productionIssueService = productionIssueService;
        }
        // GET: Manufacturing/Repacking
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Manufacturing/Repacking/Details/5
        public ActionResult Details(int id)
        {
            RepackingViewModel Repacking = repackingBL.GetRepackingIssue((int)id).Select(m => new RepackingViewModel()
            {
                RepackingNo = m.RepackingNo,
                RepackingDate = General.FormatDate(m.RepackingDate, "view"),
                ItemName = m.ItemName,
                BatchNo = m.BatchNo,
                BatchType = m.BatchType,
                QuantityIn = m.QuantityIn,
                IsDraft = m.IsDraft,
                IsCancelled = m.IsCancelled,
                ID = m.ID,
                ReceiptItemBatchTypeID = m.ReceiptItemBatchTypeID,
                ReceiptItemBatchType = m.ReceiptItemBatchType,
                QuantityOut = m.QuantityOut,
                ReceiptItemID = m.ReceiptItemID,
                ItemReceipt = m.ItemReceipt,
                BatchName = m.BatchNo,


            }).FirstOrDefault();

            Repacking.Materials = repackingBL.GetRepackingMaterials(id).Select(m => new ProductionRePackingMaterialItemModel()
            {
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                UnitID = m.UnitID,
                UOM = m.UOM,
                BatchTypeID = m.BatchTypeID,
                BatchType = m.BatchType,
                ActualQty = m.ActualQty,
                StandardQty = m.StandardQty,
                IssueQty = m.IssueQty,
                StandardQtyForStdBatch = m.StandardQtyForStdBatch,
                ItemCode = m.ItemCode,
                Variance = m.Variance
            }).ToList();

            Repacking.Process = repackingBL.GetRepackingProcess(id).Select(m => new ProductionREPackingProcesItemModel()
            {
                Stage = m.Stage,
                ProcessName = m.ProcessName,
                StartTime = General.FormatDateTime(m.StartTime),
                EndTime = General.FormatDateTime(m.EndTime),
                SkilledLaboursStandard = m.SkilledLaboursStandard,
                SkilledLaboursActual = m.SkilledLaboursActual,
                UnSkilledLabourActual = m.UnSkilledLaboursActual,
                UnSkilledLabourStandard = m.UnSkilledLabourStandard,
                MachineHoursActual = m.MachineHoursActual,
                MachineHoursStandard = m.MachineHoursStandard,
                Status = m.Status,
                DoneBy = m.DoneBy,
            }).ToList();

            Repacking.Output = repackingBL.GetRepackingOutput(id).Select(a => new RepakingPackingOutputModel()
            {
                ItemName = a.ItemName,
                PackedQty = a.PackedQty,
                BatchType = a.BatchType,
                BatchNo = a.BatchNo,
                QCCompleted = a.QCCompleted,
                Date = General.FormatDate(a.Date, "view"),
                IsQCCompleted = a.IsQCCompleted

            }).ToList();

            return View(Repacking);
        }

        // GET: Manufacturing/Repacking/Create
        public ActionResult Create()
        {
            try
            {
                RepackingViewModel repacking = new RepackingViewModel();
                repacking.RepackingNo = generalBL.GetSerialNo("Repacking", "Code");
                repacking.RepackingDate = General.FormatDate(DateTime.Now);
                repacking.DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore"));
                repacking.Materials = new List<ProductionRePackingMaterialItemModel>();
                repacking.Output = new List<RepakingPackingOutputModel>();
                repacking.BatchNameList = new SelectList(batchBL.GetBatchList(0, 0), "ID", "BatchNo");
                repacking.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
                return View(repacking);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        // GET: Manufacturing/Repacking/Edit/5
        public ActionResult Edit(int id)
        {
            RepackingViewModel Repacking = repackingBL.GetRepackingIssue((int)id).Select(m => new RepackingViewModel()
            {
                RepackingNo = m.RepackingNo,
                RepackingDate = General.FormatDate(m.RepackingDate),
                ItemName = m.ItemName,
                BatchNo = m.BatchNo,
                BatchType = m.BatchType,
                BatchTypeID = m.IsuueItemBatchTypeID,
                QuantityIn = m.QuantityIn,
                IsDraft = m.IsDraft,
                IsCancelled = m.IsCancelled,
                ID = m.ID,
                ReceiptItemBatchTypeID = m.ReceiptItemBatchTypeID,
                ReceiptItemBatchType = m.ReceiptItemBatchType,
                QuantityOut = m.QuantityOut,
                ReceiptItemID = m.ReceiptItemID,
                ItemReceipt = m.ItemReceipt,
                BatchName = m.BatchNo,
                ItemID = m.IssueItemID,
                DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore")),
                StandardQty = m.StandardQty,
                Remark = m.Remark,
                IssueItemID = m.IssueItemID,
                ReceiptConversionFactorP2S = m.ReceiptConversionFactorP2S,
                IssueConversionFactorP2S = m.IssueConversionFactorP2S
            }).FirstOrDefault();
            if (!Repacking.IsDraft || Repacking.IsCancelled)
            {
                return RedirectToAction("Index");
            }
            Repacking.Materials = repackingBL.GetRepackingMaterials(id).Select(m => new ProductionRePackingMaterialItemModel()
            {
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                UnitID = m.UnitID,
                UOM = m.UOM,
                BatchTypeID = m.BatchTypeID,
                BatchType = m.BatchType,
                ActualQty = m.ActualQty,
                StandardQty = m.StandardQty,
                IssueQty = m.IssueQty,
                StandardQtyForStdBatch = m.StandardQtyForStdBatch,
                ItemCode = m.ItemCode,
                StoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore")),
                AvailableStock = m.AvailableStock,
                IsAdditionalIssue = m.IsAdditionalIssue,
                IsMaterialReturn = m.IsMaterialReturn,
                Variance = m.Variance
            }).ToList();

            Repacking.Process = repackingBL.GetRepackingProcess(id).Select(m => new ProductionREPackingProcesItemModel()
            {
                Stage = m.Stage,
                ProcessName = m.ProcessName,
                StartTime = General.FormatDate(m.StartTime),
                EndTime = General.FormatDate(m.EndTime),
                SkilledLaboursStandard = m.SkilledLaboursStandard,
                SkilledLaboursActual = m.SkilledLaboursActual,
                UnSkilledLabourActual = m.UnSkilledLaboursActual,
                UnSkilledLabourStandard = m.UnSkilledLabourStandard,
                MachineHoursActual = m.MachineHoursActual,
                MachineHoursStandard = m.MachineHoursStandard,
                Status = m.Status,
                DoneBy = m.DoneBy,
                BatchTypeID = m.BatchTypeID,
                PackingIssueID = (int)m.PackingIssueID
            }).ToList();

            Repacking.Output = repackingBL.GetRepackingOutput(id).Select(a => new RepakingPackingOutputModel()
            {
                ItemName = a.ItemName,
                PackedQty = a.PackedQty,
                BatchType = a.BatchType,
                BatchNo = a.BatchNo,

                Date = General.FormatDate(a.Date),
                BatchTypeID = a.BatchTypeID,
                BatchID = a.BatchID,
                RepackingIssueID = a.RepackingIssueID,
                ItemID = a.ItemID,
                IsQCCompleted = a.IsQCCompleted

            }).ToList();
            Repacking.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            return View(Repacking);
        }

        public PartialViewResult GetPackingMaterials(int ItemID, int ProductGroupID, int BatchTypeID, int PackedQty, string BatchType, int StoreID, String ItemName, String Unit, decimal QuantityIn, int IssueItemID, int IssueBatchTypeID, int BatchID)
        {
            List<ProductionRePackingMaterialItemModel> Materials = repackingBL.GetPackingMaterials(ItemID, ProductGroupID, BatchTypeID, StoreID, IssueItemID, QuantityIn, PackedQty, IssueBatchTypeID, BatchID)
                .Select(a => new ProductionRePackingMaterialItemModel()
                {
                    ItemCode = a.Code,
                    ItemName = a.Name,
                    ItemID = a.ItemID,
                    UOM = a.Unit,
                    UnitID = a.UnitID,
                    BatchType = BatchType,
                    AvailableStock = a.AvailableStock,
                    StandardQty = a.StandardQty,
                    ActualQty = a.ActualQty,//(decimal)a.StandardQty * PackedQty,
                    IssueQty = a.ActualQty,//(decimal)a.StandardQty * PackedQty,
                    Variance = 0,
                    ReceiptItem = ItemName,
                    ReceiptBatchType = a.BatchType,
                    ReceiptUnit = Unit,
                    ReceiptQty = QuantityIn,
                    BatchTypeID = a.ReceiptItemBatchTypeID,
                    StoreID = a.StoreID == null ? Convert.ToInt16(generalBL.GetConfig("DefaultPackingStore")) : (int)a.StoreID,
                }
                ).ToList();
            return PartialView(Materials);
        }
        public PartialViewResult GetPackingProcess(int ItemID, int ProductGroupID, int BatchTypeID, int PackedQty, string BatchType)
        {
            List<ProductionREPackingProcesItemModel> Process = repackingBL.GetPackingProcess(ItemID, ProductGroupID, BatchTypeID)
                .Select(
                    a => new ProductionREPackingProcesItemModel()
                    {
                        Stage = a.Stage,
                        ProcessName = a.ProcessName,
                        UnSkilledLabourStandard = a.UnSkilledLabourStandard,
                        SkilledLaboursStandard = a.SkilledLaboursStandard,
                        MachineHoursStandard = a.MachineHoursStandard,
                        BatchTypeID = BatchTypeID,
                        StartTimeStr = General.FormatDate(DateTime.Now),
                        EndTimeStr = General.FormatDate(DateTime.Now)
                    }
                ).ToList();
            //ViewBag.StoreList = wareHouseBL.GetWareHouses();
            return PartialView(Process);
        }

        // /Manufacturing/Repacking/Save
        [HttpPost]
        public ActionResult Save(RepackingViewModel repacking)

        {
            var result = new List<object>();
            try
            {
                if (repacking.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    RepackingBO Temp = repackingBL.GetRepackingIssue(repacking.ID).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                RepackingBO repackingBO = new RepackingBO()
                {
                    RepackingNo = repacking.RepackingNo,
                    RepackingDate = General.ToDateTime(repacking.RepackingDate),
                    ItemIssue = repacking.ItemIssue,
                    IssueItemID = repacking.IssueItemID,
                    IsuueItemBatchID = repacking.IssueItemBatchID,
                    IsuueItemBatchTypeID = repacking.IssueItemBatchTypeID,
                    QuantityIn = repacking.QuantityIn,
                    ItemReceipt = repacking.ItemReceipt,
                    ReceiptItemID = repacking.ReceiptItemID,
                    ReceiptItemBatchID = repacking.ReceiptItemBatchID,
                    ReceiptItemBatchTypeID = repacking.ReceiptItemBatchTypeID,
                    QuantityOut = repacking.QuantityOut,
                    Isprocessed = repacking.Isprocessed,
                    BatchNo = repacking.BatchNo,
                    BatchType = repacking.BatchType,
                    IsDraft = repacking.IsDraft,
                    ID = repacking.ID,
                    StandardQty = repacking.StandardQty,
                    Remark = repacking.Remark
                };
                List<ProductionRePackingMaterialItemBO> Materials = new List<ProductionRePackingMaterialItemBO>();
                ProductionRePackingMaterialItemBO Material;
                foreach (var item in repacking.Materials)
                {
                    Material = new ProductionRePackingMaterialItemBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        BatchTypeID = item.BatchTypeID,
                        StoreID = item.StoreID,
                        AvailableStock = item.AvailableStock,
                        StandardQty = item.StandardQty,
                        IssueQty = item.IssueQty,
                        Variance = item.Variance,
                        StandardQtyForStdBatch = item.StandardQtyForStdBatch,
                        IsMaterialReturn = item.IsMaterialReturn,
                        IsAdditionalIssue = item.IsAdditionalIssue
                    };
                    Materials.Add(Material);
                }
                List<ProductionREPackingProcesItemBO> ReProcesses = new List<ProductionREPackingProcesItemBO>();
                ProductionREPackingProcesItemBO Process;
                if (repacking.Process != null)
                {
                    foreach (var item in repacking.Process)
                    {
                        Process = new ProductionREPackingProcesItemBO
                        {
                            PackingIssueID = item.PackingIssueID,
                            Stage = item.Stage,
                            ProcessName = item.ProcessName,
                            StartTime = General.ToDateTime(item.StartTime),
                            EndTime = General.ToDateTime(item.EndTime),
                            SkilledLaboursStandard = item.SkilledLaboursStandard,
                            SkilledLaboursActual = item.SkilledLaboursActual,
                            UnSkilledLabourStandard = item.UnSkilledLabourStandard,
                            UnSkilledLabourActual = item.UnSkilledLabourActual,
                            MachineHoursStandard = item.MachineHoursStandard,
                            MachineHoursActual = item.MachineHoursActual,
                            DoneBy = item.DoneBy,
                            Status = item.Status,
                            BatchTypeID = item.BatchTypeID
                        };
                        ReProcesses.Add(Process);
                    }
                }
                List<RepakingPackingOutputBO> Output = new List<RepakingPackingOutputBO>();
                RepakingPackingOutputBO Out;
                foreach (var item in repacking.Output)
                {
                    Out = new RepakingPackingOutputBO
                    {
                        RepackingIssueID = item.RepackingIssueID,
                        ItemID = item.ItemID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        QuantityOut = item.QuantityOut,
                        IsQCCompleted = item.IsQCCompleted,
                        Date = General.ToDateTime(item.Date)

                    };
                    Output.Add(Out);
                }

                if (repackingBL.Save(repackingBO, Materials, ReProcesses, Output))
                {
                    return Json(new { Status = "success", Message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Failed To Save" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Manufacturing", "Repacking", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Cancel(int ID, string Table)
        {
            repackingBL.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAsDraft(RepackingViewModel repacking)
        {
            return Save(repacking);
        }

        public JsonResult GetRepackingList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string ItemNameHint = Datatable.Columns[3].Search.Value;
                string BatchNoHint = Datatable.Columns[4].Search.Value;
                string BatchTypeHint = Datatable.Columns[5].Search.Value;
                string QuantityInHint = Datatable.Columns[6].Search.Value; ;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = repackingBL.GetRepackingList(Type, TransNoHint, TransDateHint, ItemNameHint, BatchNoHint, BatchTypeHint, QuantityInHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
