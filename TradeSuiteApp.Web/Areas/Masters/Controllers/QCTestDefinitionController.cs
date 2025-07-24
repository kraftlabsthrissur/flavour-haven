using BusinessLayer;
using BusinessObject;
using Microsoft.AspNet.Identity;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class QCTestDefinitionController : Controller
    {
        private IQcTestDefinitionContract QCTestDefinitionBL;
        private IDropdownContract _dropdown;

        public QCTestDefinitionController(IDropdownContract tempDropdown)
        {
            QCTestDefinitionBL = new QCTestDefinitionBL();
            _dropdown = tempDropdown;
        }

        public ActionResult Index()
        {
            List<QCTestDefinitionModel> TestList = new List<QCTestDefinitionModel>();
            TestList = QCTestDefinitionBL.GetQCTestDefinitionList().Select(a => new QCTestDefinitionModel
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                TestName = a.TestName,
                QCTest = a.QCTest,
            }).ToList();
            return View(TestList);
        }

        public ActionResult Create()
        {
            QCTestDefinitionModel QcTest = new QCTestDefinitionModel();
            QcTest.QCTestList = new SelectList(QCTestDefinitionBL.GetQCList(), "QCTestID", "TestName");
            QcTest.Items = new List<QCTestItemModel>();
            return View(QcTest);
        }

        public ActionResult Save(QCTestDefinitionModel model, int[] ID)
        {
            try
            {
                QCTestBO QCTestBO = new QCTestBO()
                {

                    ItemID = model.ItemID
                };
                List<QCTestItemBO> Items = new List<QCTestItemBO>();
                if (model.Items != null)
                {

                    QCTestItemBO QCDefinitionItem;

                    foreach (var item in model.Items)
                    {
                        QCDefinitionItem = new QCTestItemBO()
                        {
                            ID = item.ID,
                            QCTestID = item.QCTestID,
                            RangeFrom = item.RangeFrom,
                            RangeTo = item.RangeTo,
                            Result = item.Result,
                            IsMandatory = item.IsMandatory,
                            StartDate=General.ToDateTime( item.StartDate),
                            EndDate=General.ToDateTime( item.EndDate)

                        };
                        Items.Add(QCDefinitionItem);
                    }
                }
                
                QCTestDefinitionBL.Save(Items, QCTestBO);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            QCTestDefinitionModel model = QCTestDefinitionBL.GetQCDefinitionDetails(ID).Select(m => new QCTestDefinitionModel()
            {

                ItemID = m.ItemID,
                ItemName = m.ItemName,
            }).First();

            model.Items = QCTestDefinitionBL.GetTestForItemList(ID).Select(m => new QCTestItemModel()
            {
                ID = m.ID,
                QCTestID = m.QCTestID,
                TestName = m.TestName,
                RangeFrom = m.RangeFrom,
                RangeTo = m.RangeTo,
                Result = m.Result,
                IsMandatory = m.IsMandatory,
                StartDate=General.FormatDate((DateTime) m.StartDate),
                EndDate=General.FormatDate((DateTime) m.EndDate)
            }).ToList();
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            QCTestDefinitionModel model = QCTestDefinitionBL.GetQCDefinitionDetails(ID).Select(m => new QCTestDefinitionModel()
            {
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                QCTestList = new SelectList(QCTestDefinitionBL.GetQCList(), "QCTestID", "TestName"),
            }).First();

            model.Items = QCTestDefinitionBL.GetTestForItemList(ID).Select(m => new QCTestItemModel()
            {
                ItemID = m.ItemID,
                ID = m.ID,
                QCTestID = m.QCTestID,
                TestName = m.TestName,
                RangeFrom = m.RangeFrom,
                RangeTo = m.RangeTo,
                Result = m.Result,
                IsMandatory = m.IsMandatory,
                StartDate=General.FormatDate((DateTime) m.StartDate),
                EndDate=General.FormatDate((DateTime) m.EndDate)
            }).ToList();
            return View(model);
        }

        public JsonResult GetTestForItemList(int ItemID)
        {
            try
            {
               // List<QCTestItemBO> Items = QCTestDefinitionBL.GetTestForItemList(ItemID);
                List<QCTestItemModel> Items = new List<QCTestItemModel>();
                Items = QCTestDefinitionBL.GetTestForItemList(ItemID).Select(m => new QCTestItemModel()
                {
                    ItemID = m.ItemID,
                    ID = m.ID,
                    QCTestID = m.QCTestID,
                    TestName = m.TestName,
                    RangeFrom = m.RangeFrom,
                    RangeTo = m.RangeTo,
                    Result = m.Result,
                    IsMandatory = m.IsMandatory,
                    StartDate = General.FormatDate((DateTime)m.StartDate),
                    EndDate = General.FormatDate((DateTime)m.EndDate)
                }).ToList();
                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult IsDeletable(int ID, int QCTestID, int ItemID)
        {
            bool IsDeletable = QCTestDefinitionBL.IsDeletable(QCTestID, ID, ItemID);
            if (IsDeletable)
            {
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetQcTestDefinitionList(DatatableModel Datatable)
        {
            try
            {
                string Code = Datatable.Columns[1].Search.Value;
                string ItemName = Datatable.Columns[2].Search.Value;
                string TestName = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = QCTestDefinitionBL.GetQcTestDefinitionList(Code,ItemName, TestName, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}



