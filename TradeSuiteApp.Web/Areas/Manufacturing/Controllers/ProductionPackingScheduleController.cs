using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class ProductionPackingScheduleController : Controller
    {

        private IBatchContract batchBL;
        private IWareHouseContract wareHouseBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private IBatchTypeContract batchTypeBL;
        private IProductionPackingContract productionPackingBL;
        private IProductionPackingScheduleContract ProductionPackingScheduleBL;

        public ProductionPackingScheduleController()
        {

            batchBL = new BatchBL();
            wareHouseBL = new WarehouseBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            batchTypeBL = new BatchTypeBL();
            productionPackingBL = new ProductionPackingBL();
            ProductionPackingScheduleBL = new ProductionPackingScheduleBL();

        }
        // GET: Manufacturing/PackingSchedule
        public ActionResult Index()
        {
            ProductionPackingScheduleModel ProductionPacking = new ProductionPackingScheduleModel();
            return View(ProductionPacking);
        }
        public ActionResult Create()
        {
            ProductionPackingScheduleModel ProductionPacking = new ProductionPackingScheduleModel();
            ProductionPacking.Date = General.FormatDate(DateTime.Now);
            ProductionPacking.TransNo = generalBL.GetSerialNo("PackingSchedule", "Code");
            ProductionPacking.BatchList = new SelectList(batchBL.GetBatchList(0, ProductionPacking.DefaultPackingStoreID), "ID", "BatchNo");
            ProductionPacking.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            ProductionPacking.Materials = new List<PackingMaterialModel>();
            ProductionPacking.DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore"));
            ProductionPacking.StoreList = new SelectList(wareHouseBL.GetWareHouses(), "ID", "Name");
            return View(ProductionPacking);
        }
        public ActionResult Edit(int ID)
        {
            if (ID == 0)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    List<PackingMaterialBO> PackingMaterialBO;
                    ProductionPackingScheduleModel productionPackingScheduleModel;
                    PackingMaterialModel packingMaterialModel;
                    PackingBO packingBO = ProductionPackingScheduleBL.GetPackingSchedule(ID);
                    if (!packingBO.IsDraft)
                    {
                        return RedirectToAction("Index");
                    }
                    productionPackingScheduleModel = new ProductionPackingScheduleModel()
                    {
                        TransNo = packingBO.TransNo,
                        Date = General.FormatDate(packingBO.TransDate),
                        PackedQty = packingBO.PackedQty,
                        ItemID = packingBO.ItemID,
                        StartDate = packingBO.StartDate,
                        ProductGroupID = packingBO.ProductGroupID,
                        ProductGroupName = packingBO.ProductGroupName,
                        ItemName = packingBO.ItemName,
                        Remarks = packingBO.Remarks,
                        BatchID = packingBO.BatchID,
                        BatchNo = packingBO.BatchNo,
                        BatchTypeID = packingBO.BatchTypeID,
                        BatchType = packingBO.BatchType,
                        IsDraft=packingBO.IsDraft,
                        UnitID=packingBO.UnitID,
                        UOM=packingBO.UOM,
                        ID = packingBO.ID
                    };

                    PackingMaterialBO = ProductionPackingScheduleBL.GetPackingScheduleItems(ID);

                    productionPackingScheduleModel.Materials = new List<PackingMaterialModel>();
                    foreach (var m in PackingMaterialBO)
                    {
                        packingMaterialModel = new PackingMaterialModel()
                        {
                            BatchID = m.BatchID,
                            ItemID = m.ItemID,
                            ItemName = m.ItemName,
                            Remarks = m.Remarks,
                            ActualQty = m.ActualQty,
                            StandardQty = m.StandardQty,
                            IssueQty=m.IssueQty,
                            UOM = m.Unit,
                            UnitID=m.UnitID,
                            BatchType = productionPackingScheduleModel.BatchType,
                            ItemCode=m.ItemCode,
                            AvailableStock=m.AvailableStock,
                            PackingMaterialMasterID=m.PackingMaterialMasterID
                        };
                        productionPackingScheduleModel.Materials.Add(packingMaterialModel);
                    }
                    productionPackingScheduleModel.BatchList = new SelectList(batchBL.GetBatchList(0, productionPackingScheduleModel.DefaultPackingStoreID), "ID", "BatchNo");
                    productionPackingScheduleModel.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
                    productionPackingScheduleModel.DefaultPackingStoreID = int.Parse(generalBL.GetConfig("DefaultPackingStore"));
                    productionPackingScheduleModel.StoreList = new SelectList(wareHouseBL.GetWareHouses(), "ID", "Name");
                    return View(productionPackingScheduleModel);
                }

                catch (Exception e)
                {
                    return View(e);
                }
            }
        }
        public ActionResult Details(int ID)
        {
            if (ID == 0)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    List<PackingMaterialBO> PackingMaterialBO;
                    ProductionPackingScheduleModel productionPackingScheduleModel;
                    PackingMaterialModel packingMaterialModel;
                    PackingBO packingBO = ProductionPackingScheduleBL.GetPackingSchedule(ID);

                    productionPackingScheduleModel = new ProductionPackingScheduleModel()
                    {
                        TransNo = packingBO.TransNo,
                        Date = General.FormatDate(packingBO.TransDate),
                        PackedQty = packingBO.PackedQty,
                        ItemID = packingBO.ItemID,
                        StartDate = packingBO.StartDate,
                        //StartDateStr= General.FormatDate(packingBO.StartDate),
                        ProductGroupID = packingBO.ProductGroupID,
                        ProductGroupName = packingBO.ProductGroupName,
                        ItemName = packingBO.ItemName,
                        Remarks = packingBO.Remarks,
                        BatchID = packingBO.BatchID,
                        BatchNo = packingBO.BatchNo,
                        BatchTypeID = packingBO.BatchTypeID,
                        BatchType = packingBO.BatchType,
                        IsDraft=packingBO.IsDraft,
                        UnitID=packingBO.UnitID,
                        UOM=packingBO.UOM,
                        ID=packingBO.ID
                        
                    };

                    PackingMaterialBO = ProductionPackingScheduleBL.GetPackingScheduleItems(ID);

                    productionPackingScheduleModel.Materials = new List<PackingMaterialModel>();
                    foreach (var m in PackingMaterialBO)
                    {
                        packingMaterialModel = new PackingMaterialModel()
                        {
                            BatchID = m.BatchID,
                            ItemID = m.ItemID,
                            ItemName = m.ItemName,
                            Remarks = m.Remarks,
                            ActualQty = m.ActualQty,
                            StandardQty = m.StandardQty,
                            IssueQty=m.IssueQty,
                            UOM = m.Unit,
                            UnitID=m.UnitID,
                            BatchType = productionPackingScheduleModel.BatchType,
                            ItemCode=m.ItemCode,
                            AvailableStock = m.AvailableStock,
                            PackingMaterialMasterID=m.PackingMaterialMasterID
                        };
                        productionPackingScheduleModel.Materials.Add(packingMaterialModel);
                    }
                    return View(productionPackingScheduleModel);
                }
                catch (Exception e)
                {
                    return View(e);
                }
            }
        }
        [HttpPost]
        public ActionResult Save(ProductionPackingScheduleModel Packing)
        {
            var result = new List<object>();
            try
            {
                if (Packing.ID != 0)
                {
                    ////Edit
                    ////Check whether editable or not
                    //PackingBO Temp = productionPackingBL.GetProductionPacking(Packing.ID).FirstOrDefault();
                    //if (!Temp.IsDraft || Temp.IsCancelled || Temp.IsCompleted || Temp.IsAborted)
                    //{
                    //    result.Add(new { ErrorMessage = "Not Editable" });
                    //    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    //}
                }
                PackingBO packingBO = new PackingBO()
                {
                    TransNo = Packing.TransNo,
                    TransDate = General.ToDateTime(Packing.Date),
                    IsDraft = Packing.IsDraft,
                    ID = Packing.ID,
                    ItemID = Packing.ItemID,
                    ProductID = Packing.ProductID,
                    ProductGroupID = Packing.ProductGroupID,
                    BatchID = Packing.BatchID,
                    PackedQty = Packing.PackedQty,
                    BatchTypeID = Packing.BatchTypeID,
                    Remarks = Packing.Remarks,
                    StartDate = Packing.StartDate
                    //StartDate= General.ToDateTime(Packing.StartDateStr)
                };
                List<PackingMaterialBO> Materials = new List<PackingMaterialBO>();
                PackingMaterialBO Material;
                foreach (var item in Packing.Materials)
                {
                    Material = new PackingMaterialBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        StoreID = item.StoreID,
                        StandardQty = item.StandardQty,
                        ActualQty = item.ActualQty,
                        PackingMaterialMasterID = item.PackingMaterialMasterID,
                        Remarks = item.Remarks,
                        IssueQty = item.IssueQty
                    };
                    Materials.Add(Material);
                }
                if (ProductionPackingScheduleBL.Save(packingBO, Materials))
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

        public ActionResult SaveAsDraft(ProductionPackingScheduleModel Packing)
        {
            return Save(Packing);
        }

        public JsonResult GetPackingScheduleForDataTable(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string ItemNameHint = Datatable.Columns[3].Search.Value;
                string BatchNoHint = Datatable.Columns[4].Search.Value;
                string BatchTypeHint = Datatable.Columns[5].Search.Value;
                string PackedQtyHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = ProductionPackingScheduleBL.GetPackingScheduleList(Type, TransNoHint, TransDateHint, ItemNameHint, BatchNoHint, BatchTypeHint,PackedQtyHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
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
    }
}