using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;


namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class ProductionProcessController : Controller
    {
        // GET: Manufacturing/ProductionProcess
        public ActionResult Index()
        {
            List<ProductionProcessViewModel> ProductionProcess = new List<ProductionProcessViewModel>() {
            new ProductionProcessViewModel()
            {
                ID=1,
                ItemName="Item1",
                BatchName="Batch1",
                BatchNo="BT001",
                BatchSize=10
            },
            new ProductionProcessViewModel()
            {
                ID=2,
                 ItemName="Item2",
                BatchName="Batch1",
                BatchNo="BT001",
                BatchSize=10
            },
            new Models.ProductionProcessViewModel()
            {
                ID=3,
                 ItemName="Item1",
                BatchName="Batch2",
                BatchNo="BT001",
                BatchSize=10
            },
            new ProductionProcessViewModel()
            {
                ID=4,
                 ItemName="Item8",
                BatchName="Batch1",
                BatchNo="BT001",
                BatchSize=10
            }

        };
            return View(ProductionProcess);
        }

        // GET: Manufacturing/ProductionProcess/Details/5
        public ActionResult Details(int id)
        {
            ProductionProcessViewModel ProductionProcess = new ProductionProcessViewModel();
            ProductionProcess.SlNo = "PM0000011";
            ProductionProcess.Date = "28-03-2018";
            ProductionProcess.BatchList = new SelectList(
                                            new List<SelectListItem>
                                            {
                                                new SelectListItem { Text = "Batch1", Value = "1"},
                                                new SelectListItem { Text = "Batch2", Value = "2"},
                                            }, "Value", "Text");
            ProductionProcess.Items = new List<ProductionProcessItemModel>() {
                new ProductionProcessItemModel() {
                     stage="1",
                     ProcessName="ABC",
                     StartTime="28-03-2018",
                     InputQuantity=10,
                     EndTime="12-2-18",
                     OutputQty=10,
                     SkilledLabours=4,
                     UnSkilledLabours=5,
                     MachineHours=1,
                     DoneBy="ABC",
                      StatusList = new SelectList(
                                            new List<SelectListItem>
                                            {
                                                new SelectListItem { Text = "Completed", Value = "1"},
                                                new SelectListItem { Text = "Not Completed", Value = "2"},
                                            }, "Value", "Text")
                    } };
           return View(ProductionProcess);
        }

        // GET: Manufacturing/ProductionProcess/Create
        public ActionResult Create()
        {
            ProductionProcessViewModel productmix = new ProductionProcessViewModel();
            productmix.SlNo = "PM0000011";
            productmix.Date = "28-03-2018";
            productmix.BatchList = new SelectList(
                                            new List<SelectListItem>
                                            {
                                                new SelectListItem { Text = "Batch1", Value = "1"},
                                                new SelectListItem { Text = "Batch2", Value = "2"},
                                            }, "Value", "Text");
            return View(productmix);
        }

        // POST: Manufacturing/ProductionProcess/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Manufacturing/ProductionProcess/Edit/5
        public ActionResult Edit()
        {
           ProductionProcessViewModel ProductionProcess = new ProductionProcessViewModel();
            ProductionProcess.SlNo = "PM0000011";
            ProductionProcess.Date = "28-03-2018";
            ProductionProcess.BatchList = new SelectList(
                                            new List<SelectListItem>
                                            {
                                                new SelectListItem { Text = "Batch1", Value = "1"},
                                                new SelectListItem { Text = "Batch2", Value = "2"},
                                            }, "Value", "Text");
            ProductionProcess.Items = new List<ProductionProcessItemModel>() {
                new ProductionProcessItemModel() {
                     stage="1",
                     ProcessName="ABC",
                     StartTime="28-03-2018",
                     InputQuantity=10,
                     EndTime="12-2-18",
                     OutputQty=10,
                     SkilledLabours=4,
                     UnSkilledLabours=5,
                     MachineHours=1,
                     DoneBy="ABC",
                        StatusList = new SelectList(
                                            new List<SelectListItem>
                                            {
                                                new SelectListItem { Text = "Completed", Value = "1"},
                                                new SelectListItem { Text = "Not Completed", Value = "2"},
                                            }, "Value", "Text")

                    } };
            return View(ProductionProcess);
        }

        // POST: Manufacturing/ProductionProcess/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Manufacturing/ProductionProcess/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Manufacturing/ProductionProcess/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
