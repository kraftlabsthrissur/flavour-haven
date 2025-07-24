
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class QCController : Controller
    {

        #region Private Declarations

        private IQualityCheckContract qualityCheckBL;
        private IWareHouseContract warehouseBL;
        private IGeneralContract generalBL;
        private string failureMessage = App_LocalResources.Common.createfail;
    
    #endregion

    public QCController()
        {
            qualityCheckBL = new QualityCheckBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
           
    }

    // GET: Purchase/QC
    public ActionResult Index()
    {
        ViewBag.Statuses = new List<string>() { "Passed", "Overruled", "Failed" };
        return View();
    }

    // GET: Purchase/QC/Details/5
    public ActionResult Details(int? id)
    {

        if (id == null)
        {
            return View("PageNotFound");
        }
        int QCID = (int)id;
        QCItemModel QCItem = qualityCheckBL.GetQCItemDetailsByID(QCID).Select(a => new QCItemModel()
        {
            ID = a.ID,
            ItemName = a.ItemName,
            ItemID = a.ItemID,
            UnitName = a.UnitName,
            ReferenceNo = a.ReferenceNo,
            QCNo = a.QCNo,
            QCDate = General.FormatDate(a.QCDate, "view"),
            SupplierName = a.SupplierName,
            AcceptedQty = a.AcceptedQty,
            ApprovedQty = a.ApprovedQty,
            GRNDate = General.FormatDate(a.GRNDate, "view"),
            ToWareHouseID = a.ToWareHouseID,
            Remarks = a.Remarks,
            DeliveryChallanNo = a.DeliveryChallanNo,
            DeliveryChallanDate = General.FormatDate(a.DeliveryChallanDate, "view"),

        }).First();

        List<QCTestModel> physicalTestDetails = qualityCheckBL.GetQCTestDetailsByID(QCID, "physical").Select(a => new QCTestModel()
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

        List<QCTestModel> chemicalTestDetails = qualityCheckBL.GetQCTestDetailsByID(QCID, "chemical").Select(a => new QCTestModel()
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
        qcTestViewModel.wareHouse = wareHouse;

        return View(qcTestViewModel);

    }

    // GET: Purchase/QC/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return View("PageNotFound");
        }
        try
        {
            int QCID = (int)id;
            QCItemModel QCItem = qualityCheckBL.GetQCItemDetailsByID(QCID).Select(a => new QCItemModel()
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
                GRNDate = General.FormatDate(a.GRNDate),
                ToWareHouseID = Convert.ToInt16(generalBL.GetConfig("DefaultRMStore")),
                Remarks = a.Remarks,
                QCStatus = a.QCStatus,
                DeliveryChallanNo = a.DeliveryChallanNo,
                DeliveryChallanDate = General.FormatDate(a.DeliveryChallanDate, "view"),
            }).First();
            if ( QCItem.QCStatus != "Started" && QCItem.QCStatus!="New")
            {
                return RedirectToAction("index");
            }
            List<QCTestModel> physicalTestDetails = qualityCheckBL.GetQCTestDetailsByID(QCID, "physical").Select(a => new QCTestModel()
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

            List<QCTestModel> chemicalTestDetails = qualityCheckBL.GetQCTestDetailsByID(QCID, "chemical").Select(a => new QCTestModel()
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
            qcTestViewModel.wareHouse = wareHouse;

            ViewBag.DateRelaxation = (qualityCheckBL.GetQCItemDetailsByID(QCID).Select(a => a.GRNDate).FirstOrDefault() - DateTime.Today).Days;
            return View(qcTestViewModel);
        }
        catch (Exception e)
        {
            return View("Error");
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
                    QCItemBO Temp = qualityCheckBL.GetQCItemDetailsByID(data.QCItem.ID).FirstOrDefault();
                    if (Temp.QCStatus != "Started" && Temp.QCStatus!= "New")
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
                QCStatus = Status,
                IsDraft = data.QCItem.IsDraft
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

            if (qualityCheckBL.UpdateQC(qcItemBO, testResults))
            {
                return Json(new { Status = "success", data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.Add(new { ErrorMessage = failureMessage+" QC" });
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
        //if (ModelState.IsValid)
        //{
        return Submit("Passed", data);
        //}
        //else
        //{
        // var errors = ModelState.Values.SelectMany(v => v.Errors);
        // return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
        // }

    }

    // POST: Purchase/QC/Reject/5
    [HttpPost]
    public ActionResult Reject(int id, QCTestViewModel data)
    {
        //if (ModelState.IsValid)
        //{
        data.QCItem.ApprovedQty = 0;
        data.QCItem.ToWareHouseID = 3;
        return Submit("Failed", data);
        //}
        //else
        //{
        //    var errors = ModelState.Values.SelectMany(v => v.Errors);
        //    return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
        //}
    }

    // POST: Purchase/QC/Overrule/5
    [HttpPost]
    public ActionResult Overrule(int id, QCTestViewModel data)
    {
        //if (ModelState.IsValid)
        //{
        return Submit("Overruled", data);
        //}
        //else
        //{
        //    var errors = ModelState.Values.SelectMany(v => v.Errors);
        //    return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
        //}
    }

    [HttpPost]
    public JsonResult GetQualityCheckList(DatatableModel Datatable)
    {
        try
        {
            string TransNoHint = Datatable.Columns[1].Search.Value;
            string TransDateHint = Datatable.Columns[2].Search.Value;
            string GRNNoHint = Datatable.Columns[3].Search.Value;
            string ReceiptDateHint = Datatable.Columns[4].Search.Value;
            string ItemNameHint = Datatable.Columns[5].Search.Value;
            string UnitNameHint = Datatable.Columns[6].Search.Value;
            string SupplierNameHint = Datatable.Columns[8].Search.Value;
            string DeliveryChallanNoHint = Datatable.Columns[9].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
            string SortOrder = Datatable.Order[0].Dir;
            int Offset = Datatable.Start;
            int Limit = Datatable.Length;

            string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

            DatatableResultBO resultBO = qualityCheckBL.GetQualityCheckList(Type, TransNoHint, TransDateHint, GRNNoHint, ReceiptDateHint, ItemNameHint, UnitNameHint, SupplierNameHint, DeliveryChallanNoHint, SortField, SortOrder, Offset, Limit);
            var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
            return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult SaveAsDraft(int id, QCTestViewModel data)
    {
        return Save(id, data);
    }

}
}
