using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class ProductionDefinitionController : Controller
    {
        private IProductionDefinitionContract productionDefinitionBL;
        private IBatchTypeContract batchTypeBL;
        private ICategoryContract categoryBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;

        public ProductionDefinitionController()
        {
            productionDefinitionBL = new ProductionDefinitionBL();
            batchTypeBL = new BatchTypeBL();
            categoryBL = new CategoryBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
        }

        // GET: Masters/ProductionDefinition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ProductionDefinitionModel productionDefinition = new ProductionDefinitionModel();
            productionDefinition.ItemCategoryID = Convert.ToInt32(categoryBL.GetCategoryID("Semifinished Goods"));
            productionDefinition.LocationList = new SelectList(locationBL.GetProductionLocationList(), "ID", "Name");
            productionDefinition.FinishedGoodsItemCategoryID = Convert.ToInt32(categoryBL.GetCategoryID("Finished Goods"));
            productionDefinition.SemifinishedGoodsItemCategoryID = Convert.ToInt32(categoryBL.GetCategoryID("Semifinished Goods"));
            productionDefinition.Sequences = new List<SequenceModel>();
            return View(productionDefinition);
        }

        public PartialViewResult GetSequenceTemplate()
        {
            SequenceModel Sequence = new SequenceModel();
            Sequence.Items = new List<ProductionDefinitionMaterial>();
            Sequence.Processes = new List<ProductionDefinitionProcess>();
            Sequence.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            Sequence.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            return PartialView("~/Areas/Masters/Views/ProductionDefinition/Sequence.cshtml", Sequence);
        }

        public ActionResult Details(int id)
        {
            List<ProductionDefinitionBO> DefinitionBO = productionDefinitionBL.GetProductionDefinition(id);
            List<ProductionDefinitionMaterialBO> MaterialsBO = productionDefinitionBL.GetProductionDefinitionMaterials(id);
            List<ProductionDefinitionProcessBO> ProcessesBO = productionDefinitionBL.GetProductionDefinitionProcesses(id);
            ProductionDefinitionModel productionDefinition = DefinitionBO.Select(a => new ProductionDefinitionModel()
            {
                ID = (int)a.ID,
                ProductionGroupID = (int)a.ProductionGroupID,
                ProductionGroupItemID = a.ProductionGroupItemID,
                ProductionGroupName = a.ProductionGroupName,
                ItemName = a.ItemName,
                BatchSize = a.BatchSize,
                IsKalkan = a.IsKalkan,
                IsEditable = productionDefinitionBL.IsEditable(a.ProductionGroupID),
                LocationID = a.LocationID,
                Location = a.Location
            }).First();
            productionDefinition.LocationList = new SelectList(locationBL.GetProductionLocationList(), "ID", "Name");
            productionDefinition.Sequences = DefinitionBO.Select(a => new SequenceModel()
            {
                ProductName = a.ProductName,
                Unit = a.Unit,
                ProcessStage = a.ProcessStage,
                StandardOutputQty = a.StandardOutputQty,
                ProductionSequence = a.ProductionSequence,
                SequenceType = a.SequenceType,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name"),
                Items = MaterialsBO.Where(m => m.ProductionSequence == a.ProductionSequence && m.SequenceType == a.SequenceType).Select(m => new ProductionDefinitionMaterial()
                {
                    MaterialName = m.MaterialName,
                    Unit = m.MaterialUnit,
                    Qty = m.Qty,
                    UsageMode = m.UsageMode,
                    PrimaryToPackUnitConFact = m.PrimaryToPackUnitConFact,
                    BatchTypeID = m.BatchTypeID,
                    BatchType = m.BatchType,
                    StartDate = m.StartDate != null ? General.FormatDate((DateTime)m.StartDate, "view") : "",
                    EndDate = m.EndDate != null ? General.FormatDate((DateTime)m.EndDate, "view") : ""
                }).ToList(),

                Processes = ProcessesBO.Where(p => p.ProductionSequence == a.ProductionSequence && p.SequenceType == a.SequenceType).Select(p => new ProductionDefinitionProcess()
                {
                    ProcessName = p.ProcessName,
                    Steps = p.Steps,
                    SkilledLabourMinutes = p.SkilledLabourMinutes,
                    SkilledLabourCost = p.SkilledLabourCost,
                    UnSkilledLabourMinutes = p.UnSkilledLabourMinutes,
                    UnSkilledLabourCost = p.UnSkilledLabourCost,
                    MachineMinutes = p.MachineMinutes,
                    MachineCost = p.MachineCost,
                    Process = p.Process,
                }).ToList()

            }).ToList();
            return View(productionDefinition);
        }

        [HttpPost]
        public ActionResult Save(ProductionDefinitionSaveModel productionDefinitionModel)
        {
            try
            {
                ProductionDefinitionBO productionDefinitionBO = new ProductionDefinitionBO()
                {
                    ProductionID = productionDefinitionModel.ProductionID,
                    ProductionGroupID = productionDefinitionModel.ProductionGroupID,
                    ProductionGroupItemID = productionDefinitionModel.ProductionGroupItemID,
                    ProductionGroupName = productionDefinitionModel.ProductionGroupName,
                    BatchSize = productionDefinitionModel.BatchSize,
                    IsKalkan = productionDefinitionModel.IsKalkan,
                    ProductionLocationID = productionDefinitionModel.ProductionLocationID
                };
                List<ProductionDefinitionSequenceBO> SequenceList = new List<ProductionDefinitionSequenceBO>();
                ProductionDefinitionSequenceBO Sequences;
                foreach (var item in productionDefinitionModel.Sequences)
                {
                    Sequences = new ProductionDefinitionSequenceBO()
                    {
                        ProductionID = item.ProductionID,
                        ProductID = item.ProductID,
                        UnitID = item.UnitID,
                        ProcessStage = item.ProcessStage,
                        StandardOutputQty = item.StandardOutputQty,
                        ProductionSequence = item.ProductionSequence,
                        PackingSequence = item.PackingSequence,
                        BatchType = item.BatchType,
                        BatchTypeID = item.BatchTypeID,
                    };
                    SequenceList.Add(Sequences);
                }
                List<ProductionDefinitionMaterialBO> MaterialList = new List<ProductionDefinitionMaterialBO>();
                ProductionDefinitionMaterialBO Items;
                foreach (var item in productionDefinitionModel.Materials)
                {
                    Items = new ProductionDefinitionMaterialBO()
                    {
                        ID = item.ID,
                        ProductionDefinitionID = item.ProductionDefinitionID,
                        MaterialID = item.MaterialID,
                        MaterialUnitID = item.UnitID,
                        Qty = item.Qty,
                        UsageMode = item.UsageMode,
                        PrimaryToPackUnitConFact = item.PrimaryToPackUnitConFact,
                        ProductionSequence = item.ProductionSequence,
                        PackingSequence = item.PackingSequence,
                        BatchType = item.BatchType,
                        BatchTypeID = item.BatchTypeID,
                        StartDate = General.ToDateTime(item.StartDate),
                        EndDate = General.ToDateTime(item.EndDate)
                    };
                    MaterialList.Add(Items);
                }
                List<ProductionDefinitionProcessBO> ProcessList = new List<ProductionDefinitionProcessBO>();
                ProductionDefinitionProcessBO Process;
                if (productionDefinitionModel.Processes != null)
                {
                    foreach (var item in productionDefinitionModel.Processes)
                    {
                        Process = new ProductionDefinitionProcessBO()
                        {
                            ProcessID = item.ProcessID,
                            ProductionDefinitionID = item.ProductionDefinitionID,
                            ProcessName = item.ProcessName,
                            Steps = item.Steps,
                            Process = item.Process,
                            SkilledLabourMinutes = item.SkilledLabourMinutes,
                            SkilledLabourCost = item.SkilledLabourCost,
                            UnSkilledLabourMinutes = item.UnSkilledLabourMinutes,
                            UnSkilledLabourCost = item.UnSkilledLabourCost,
                            MachineMinutes = item.MachineMinutes,
                            MachineCost = item.MachineCost,
                            ProductionSequence = item.ProductionSequence,
                            PackingSequence = item.PackingSequence,
                            BatchType = item.BatchType,
                            BatchTypeID = item.BatchTypeID,
                        };
                        ProcessList.Add(Process);
                    }
                }

                List<ProductionDefinitionMaterialBO> DeleteMaterialList = new List<ProductionDefinitionMaterialBO>();
                ProductionDefinitionMaterialBO DeleteMaterial;
                if (productionDefinitionModel.DeleteMaterials != null)
                {
                    foreach (var item in productionDefinitionModel.DeleteMaterials)
                    {
                        DeleteMaterial = new ProductionDefinitionMaterialBO()
                        {
                            ID = item.ID,
                            PackingSequence = item.PackingSequence
                        };
                        DeleteMaterialList.Add(DeleteMaterial);
                    }
                }

                List<ProductionDefinitionProcessBO> DeleteProcessList = new List<ProductionDefinitionProcessBO>();
                ProductionDefinitionProcessBO DeleteProcess;
                if (productionDefinitionModel.DeleteProcesses != null)
                {
                    foreach (var item in productionDefinitionModel.DeleteProcesses)
                    {
                        DeleteProcess = new ProductionDefinitionProcessBO()
                        {
                            ProcessID = item.ProcessID,
                            PackingSequence = item.PackingSequence
                        };
                        DeleteProcessList.Add(DeleteProcess);
                    }
                }
                List<ProductionDefinitionBO> LocationList = new List<ProductionDefinitionBO>();
                ProductionDefinitionBO Location;

                foreach (var item in productionDefinitionModel.LocationList)
                {
                    Location = new ProductionDefinitionBO()
                    {
                        LocationID = item.LocationID
                    };
                    LocationList.Add(Location);
                }
                productionDefinitionBL.Save(productionDefinitionBO, SequenceList, MaterialList, ProcessList, DeleteMaterialList, DeleteProcessList, LocationList);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "ProductionDefinition", "Save", productionDefinitionModel.ID, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            List<ProductionDefinitionBO> DefinitionBO = productionDefinitionBL.GetProductionDefinition(id);
            List<ProductionDefinitionMaterialBO> MaterialsBO = productionDefinitionBL.GetProductionDefinitionMaterials(id);
            List<ProductionDefinitionProcessBO> ProcessesBO = productionDefinitionBL.GetProductionDefinitionProcesses(id);
            ProductionDefinitionModel productionDefinition = DefinitionBO.Select(a => new ProductionDefinitionModel()
            {
                ProductionID = a.ID,
                ProductionGroupID = a.ProductionGroupID,
                ProductionGroupItemID = a.ProductionGroupItemID,
                ProductionGroupName = a.ProductionGroupName,
                ItemName = a.ItemName,
                BatchSize = a.BatchSize,
                IsKalkan = a.IsKalkan,
                LocationID = a.ProductionLocationID
            }).First();
            productionDefinition.LocationList = new SelectList(locationBL.GetProductionLocationList(), "ID", "Name");
            productionDefinition.ItemCategoryID = Convert.ToInt32(categoryBL.GetCategoryID("Semifinished Goods"));
            productionDefinition.FinishedGoodsItemCategoryID = Convert.ToInt32(categoryBL.GetCategoryID("Finished Goods"));
            productionDefinition.SemifinishedGoodsItemCategoryID = Convert.ToInt32(categoryBL.GetCategoryID("Semifinished Goods"));
            productionDefinition.Sequences = DefinitionBO.Select(a => new SequenceModel()
            {
                ProductionID = (int)a.ProductionID,
                ProductID = a.ProductID,
                ProductName = a.ProductName,
                Unit = a.Unit,
                UnitID = a.UnitID,
                BatchSize = a.BatchSize,
                ProcessStage = a.ProcessStage,
                StandardOutputQty = a.StandardOutputQty,
                ProductionSequence = a.ProductionSequence,
                SequenceType = a.SequenceType,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text"),
                BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name"),
                Items = MaterialsBO.Where(m => m.ProductionSequence == a.ProductionSequence && m.SequenceType == a.SequenceType).Select(m => new ProductionDefinitionMaterial()
                {
                    ID = m.ID,
                    ProductionDefinitionID = m.ProductionDefinitionID,
                    MaterialID = m.MaterialID,
                    MaterialName = m.MaterialName,
                    Unit = m.MaterialUnit,
                    UnitID = m.MaterialUnitID,
                    Qty = m.Qty,
                    UsageMode = m.UsageMode,
                    PrimaryToPackUnitConFact = m.PrimaryToPackUnitConFact,
                    BatchTypeID = m.BatchTypeID,
                    ProductionSequence = m.ProductionSequence,
                    BatchType = m.BatchType,
                    StartDate = m.StartDate != null ? General.FormatDate((DateTime)m.StartDate) : "",
                    EndDate = m.EndDate != null ? General.FormatDate((DateTime)m.EndDate) : ""
                }).ToList(),

                Processes = ProcessesBO.Where(p => p.ProductionSequence == a.ProductionSequence && p.SequenceType == a.SequenceType).Select(p => new ProductionDefinitionProcess()
                {
                    ProcessID = p.ProcessID,
                    ProductionDefinitionID = p.ProductionDefinitionID,
                    ProcessName = p.ProcessName,
                    Steps = p.Steps,
                    SkilledLabourMinutes = p.SkilledLabourMinutes,
                    SkilledLabourCost = p.SkilledLabourCost,
                    UnSkilledLabourMinutes = p.UnSkilledLabourMinutes,
                    UnSkilledLabourCost = p.UnSkilledLabourCost,
                    MachineMinutes = p.MachineMinutes,
                    MachineCost = p.MachineCost,
                    Process = p.Process,
                    ProductionSequence = p.ProductionSequence
                }).ToList()

            }).ToList();

            return View(productionDefinition);
        }

        public JsonResult GetProductionLocationMapping(int ProductionGroupID)
        {
            List<ProductionDefinitionModel> LocationList = locationBL.GetProductionLocationMapping(ProductionGroupID).Select(a => new ProductionDefinitionModel()
            {
                LocationID = a.ID,
                Location = a.LocationName,
                ProductionLocationID = a.LocationID
            }).ToList();
            return Json(LocationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionDefinitionList(DatatableModel Datatable)
        {
            try
            {
                string ProductionGroup = Datatable.Columns[1].Search.Value;
                string Name = Datatable.Columns[2].Search.Value;
                string BatchSize = Datatable.Columns[3].Search.Value;
                string OutputQty = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = productionDefinitionBL.GetProductionDefinitionList(ProductionGroup, Name, BatchSize, OutputQty, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "ProductionDefinition", "GetProductionDefinitionList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}