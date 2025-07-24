using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Utils;
using PresentationContractLayer;
using BusinessLayer;
using TradeSuiteApp.Web.Areas.Masters.Models;
using System.Collections.Generic;
using BusinessObject;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class QCProductionController : Controller
    {
        #region Private Declarations

        private IQCProductionContract qcProductionBL;
        private IWareHouseContract warehouseBL;

        #endregion

        public QCProductionController()
        {
            qcProductionBL = new QCProductionBL();
            warehouseBL = new WarehouseBL();
        }

        // GET: Manufacturing/QAForFG
        public ActionResult Index()
        {
            List<QCItemModel> pendingQCList = qcProductionBL.GetQCList("new", 0, 200).Select(a => new QCItemModel()
            {
                ID = a.ID,
                ItemName = a.ItemName,
                ItemID = a.ItemID,
                UnitName = a.UnitName,
                ReferenceNo = a.ReferenceNo,
                SupplierName = a.SupplierName,
                AcceptedQty = a.AcceptedQty,
                ProductionDate = General.FormatDate(a.ProductionIssueDate, "view"),
                BatchNo = a.BatchNo,
                BatchSize = a.BatchSize
            }).ToList();

            List<QCItemModel> onGoingQCList = qcProductionBL.GetQCList("started", 0, 200).Select(a => new QCItemModel()
            {
                ID = a.ID,
                ItemName = a.ItemName,
                ItemID = a.ItemID,
                UnitName = a.UnitName,
                ReferenceNo = a.ReferenceNo,
                SupplierName = a.SupplierName,
                AcceptedQty = a.AcceptedQty,
                ProductionDate = General.FormatDate(a.ProductionIssueDate, "view"),
                QCStatus = "draft",
                BatchNo = a.BatchNo,
                BatchSize = a.BatchSize
            }).ToList();

            List<QCItemModel> completedQCList = qcProductionBL.GetQCList("completed", 0, 200).Select(a => new QCItemModel()
            {
                ID = a.ID,
                ItemName = a.ItemName,
                ItemID = a.ItemID,
                UnitName = a.UnitName,
                ReferenceNo = a.ReferenceNo,
                SupplierName = a.SupplierName,
                AcceptedQty = a.AcceptedQty,
                ApprovedQty = a.ApprovedQty,
                ProductionDate = General.FormatDate(a.ProductionIssueDate, "view"),
                QCStatus = a.QCStatus,
                BatchNo = a.BatchNo
            }).ToList();

            QCItemViewModel qcItemViewModel = new QCItemViewModel();
            qcItemViewModel.pendingQCList = pendingQCList;
            qcItemViewModel.onGoingQCList = onGoingQCList;
            qcItemViewModel.completedQCList = completedQCList;
            ViewBag.Statuses = new List<string>() { "passed", "overruled", "failed", "draft" };
            return View(qcItemViewModel);
        }

        // GET: Manufacturing/QAForFG/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }
            try
            {
                int QCID = (int)id;
                QCItemModel QCItem = qcProductionBL.GetQCItemDetails(QCID).Select(a => new QCItemModel()
                {
                    ID = a.ID,
                    ItemName = a.ItemName,
                    ItemID = a.ItemID,
                    UnitName = a.UnitName,
                    ReferenceNo = a.ReferenceNo,
                    QCNo = a.QCNo,
                    QCDate = General.FormatDate(a.QCDate),
                    SupplierName = a.SupplierName,
                    AcceptedQty = a.AcceptedQty,
                    ApprovedQty = a.ApprovedQty,
                    ProductionDate = General.FormatDate(a.ProductionIssueDate, "view"),
                    ToWareHouseID = a.ToWareHouseID,
                    Remarks = a.Remarks,
                    StandardOutput=a.StandardOutput,
                    BatchNo=a.BatchNo
                }).First();

                List<QCTestModel> physicalTestDetails = qcProductionBL.GetQCTestDetails(QCID, "Physical").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks
                }).ToList();

                List<QCTestModel> chemicalTestDetails = qcProductionBL.GetQCTestDetails(QCID, "chemical").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks
                }).ToList();


                List<QCTestModel> organolepticTestDetails = qcProductionBL.GetQCTestDetails(QCID, "organoleptic").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks
                }).ToList();
                List<QCTestModel> pharmaceuticalTestDetails = qcProductionBL.GetQCTestDetails(QCID, "Pharmaceutical").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks
                }).ToList();
                List<QCTestModel> MicrobiologyTestDetails = qcProductionBL.GetQCTestDetails(QCID, "Microbiology").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks
                }).ToList();
                List<WareHouseModel> wareHouse = warehouseBL.GetWareHouses().Select(a => new WareHouseModel
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    Place = a.Place,
                    ItemTypeID = a.ItemTypeID,
                    Remarks = a.Remarks
                }).ToList();

                QCTestViewModel qcTestViewModel = new QCTestViewModel();
                qcTestViewModel.QCItem = QCItem;
                qcTestViewModel.physicalTestDetails = physicalTestDetails;
                qcTestViewModel.chemicalTestDetails = chemicalTestDetails;
                qcTestViewModel.organolepticTestDetails = organolepticTestDetails;
                qcTestViewModel.pharmaceuticalTestDetails = pharmaceuticalTestDetails;
                qcTestViewModel.MicrobiologyTestDetails = MicrobiologyTestDetails;

                qcTestViewModel.wareHouse = wareHouse;
                return View(qcTestViewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("index");
            }

        }


        // GET: Manufacturing/QAForFG/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }
            try
            {
                int QCID = (int)id;
                QCItemModel QCItem = qcProductionBL.GetQCItemDetails(QCID).Select(a => new QCItemModel()
                {
                    ID = a.ID,
                    ItemName = a.ItemName,
                    ItemID = a.ItemID,
                    UnitName = a.UnitName,
                    ReferenceNo = a.ReferenceNo,
                    QCNo = a.QCNo,
                    QCDate = General.FormatDate(DateTime.Now),
                    SupplierName = a.SupplierName,
                    AcceptedQty = a.AcceptedQty,
                    ApprovedQty = a.ApprovedQty,
                    ProductionDate = General.FormatDate(a.ProductionIssueDate),
                    ToWareHouseID = a.ToWareHouseID,
                    Remarks = a.Remarks,
                    QCStatus = a.QCStatus,
                    BatchNo = a.BatchNo,
                    StandardOutput = a.StandardOutput
                }).First();
                if (QCItem.QCStatus != "Started" && QCItem.QCStatus != "new")
                {
                    return RedirectToAction("index");
                }
                List<QCTestModel> physicalTestDetails = qcProductionBL.GetQCTestDetails(QCID, "Physical").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks,
                    IsMandatory = a.IsMandatory
                }).ToList();

                List<QCTestModel> chemicalTestDetails = qcProductionBL.GetQCTestDetails(QCID, "chemical").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks,
                    IsMandatory = a.IsMandatory
                }).ToList();
                List<QCTestModel> organolepticTestDetails = qcProductionBL.GetQCTestDetails(QCID, "organoleptic").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks,
                    IsMandatory = a.IsMandatory
                }).ToList();
                List<QCTestModel> pharmaceuticalTestDetails = qcProductionBL.GetQCTestDetails(QCID, "Pharmaceutical").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks,
                    IsMandatory = a.IsMandatory
                }).ToList();
                List<QCTestModel> MicrobiologyTestDetails = qcProductionBL.GetQCTestDetails(QCID, "Microbiology").Select(a => new QCTestModel()
                {
                    ID = a.ID,
                    QCID = a.QCID,
                    QCTestID = Convert.ToInt32(a.QCTestID),
                    TestName = a.TestName,
                    RangeFrom = a.RangeFrom,
                    RangeTo = a.RangeTo,
                    DefinedResult = a.DefinedResult,
                    ActualResult = a.ActualResult,
                    ActualValue = a.ActualValue,
                    Remarks = a.Remarks,
                    IsMandatory = a.IsMandatory
                }).ToList();
                List<WareHouseModel> wareHouse = warehouseBL.GetWareHouses().Select(a => new WareHouseModel
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    Place = a.Place,
                    ItemTypeID = a.ItemTypeID,
                    Remarks = a.Remarks
                }).ToList();
                QCTestViewModel qcTestViewModel = new QCTestViewModel();
                qcTestViewModel.QCItem = QCItem;
                qcTestViewModel.physicalTestDetails = physicalTestDetails;
                qcTestViewModel.chemicalTestDetails = chemicalTestDetails;
                qcTestViewModel.organolepticTestDetails = organolepticTestDetails;
                qcTestViewModel.pharmaceuticalTestDetails = pharmaceuticalTestDetails;
                qcTestViewModel.MicrobiologyTestDetails = MicrobiologyTestDetails;
                qcTestViewModel.wareHouse = wareHouse;
                ViewBag.DateRelaxation = (qcProductionBL.GetQCItemDetails(QCID).Select(a => a.ProductionIssueDate).FirstOrDefault() - DateTime.Today).Days;

                return View(qcTestViewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("index");
            }
        }

        private JsonResult Submit(string Status, QCTestViewModel data)
        {
            var result = new List<object>();
            try
            {
                if (data.QCItem.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    QCItemBO Temp = qcProductionBL.GetQCItemDetails(data.QCItem.ID).FirstOrDefault();
                    if (Temp.QCStatus != "Started" && Temp.QCStatus != "new")
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }

                QCItemBO qcItemBO = new QCItemBO()
                {
                    ApprovedQty = data.QCItem.ApprovedQty,
                    ID = data.QCItem.ID,
                    QCDate = General.ToDateTime(data.QCItem.QCDate),
                    ToWareHouseID = data.QCItem.ToWareHouseID,
                    IsCancelled = false,
                    CancelledDate = null,
                    Remarks = (data.QCItem.Remarks == null) ? "" : data.QCItem.Remarks,
                    QCStatus = Status
                };
                List<QCTestBO> testResults = new List<QCTestBO>();
                QCTestBO qcTestBO;
                if (data.testResults != null)
                {
                    foreach (var item in data.testResults)
                    {
                        qcTestBO = new QCTestBO()
                        {
                            ID = item.ID,
                            ActualValue = item.ActualValue,
                            ActualResult = item.ActualResult,
                            Remarks = item.Remarks
                        };
                        testResults.Add(qcTestBO);
                    }
                }

                if (qcProductionBL.UpdateQC(qcItemBO, testResults))
                {
                    return Json(new { Status = "success", data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Production not Completed" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
              
                result.Add(new { ErrorMessage = "Unknown Error" });
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }

        // POST: Purchase/QC/Save/5
        [HttpPost]
        public ActionResult Save(int id, QCTestViewModel data)
        {
            return Submit("Started", data);
        }

        // POST: Purchase/QC/Edit/5
        [HttpPost]
        public ActionResult Approve(int id, QCTestViewModel data)
        {
            if (ModelState.IsValid)
            {
                return Submit("Passed", data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }

        }

        // POST: Purchase/QC/Reject/5
        [HttpPost]
        public ActionResult Reject(int id, QCTestViewModel data)
        {
            if (ModelState.IsValid)
            {
                data.QCItem.ApprovedQty = 0;
                data.QCItem.ToWareHouseID = 3;
                return Submit("Failed", data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        // POST: Purchase/QC/Overrule/5
        [HttpPost]
        public ActionResult Overrule(int id, QCTestViewModel data)
        {
            if (ModelState.IsValid)
            {
                return Submit("Overruled", data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetQCList(DatatableModel Datatable)
        {
            try
            {
                string BatchsizeHint=null;
                string AcceptedQuantityHint=null;
                string ApprovedQuantityHint=null;
                string ProductionNoHint = Datatable.Columns[1].Search.Value;
                string ReceiptDateHint = Datatable.Columns[2].Search.Value;
                string ItemHint = Datatable.Columns[3].Search.Value;
                string BatchNoHint = Datatable.Columns[4].Search.Value;
                string UnitHint = Datatable.Columns[5].Search.Value;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                if(Type== "completed-qc")
                {
                     AcceptedQuantityHint = Datatable.Columns[6].Search.Value;
                     ApprovedQuantityHint = Datatable.Columns[7].Search.Value;
                }
                else
                {
                    BatchsizeHint = Datatable.Columns[6].Search.Value;
                    AcceptedQuantityHint = Datatable.Columns[7].Search.Value;
                }

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = qcProductionBL.GetQCList(Type, ProductionNoHint, ReceiptDateHint, ItemHint, BatchNoHint, UnitHint, AcceptedQuantityHint, ApprovedQuantityHint, BatchsizeHint, SortField, SortOrder, Offset, Limit);
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
