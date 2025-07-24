using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class JobWorkReceiptController : Controller
    {
        private IGeneralContract generalBL;
        private IJobWorkReceiptContract jobworkreceiptBL;
        private IJobWorkIssueContrtact jobworkissueBL;
        private IWareHouseContract warehouseBL;
        public JobWorkReceiptController()
        {
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            jobworkreceiptBL = new JobWorkReceiptBL();
            jobworkissueBL = new JobWorkIssueBL();
        }
        // GET: Manufacturing/JobWorkReceipt
      
        public ActionResult Index()
        {
            try
            {
                List<JobWorkReceiptViewModel> JobWorkReceipts = jobworkreceiptBL.GetJobWorkReceipts().Select(a => new JobWorkReceiptViewModel()
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    TransDate = General.FormatDate(a.TransDate),
                    IssueNo = a.IssueNo,
                    Supplier = a.Supplier,
                    Status = a.IsDraft ? "draft" : "",
                }).ToList();
                return View(JobWorkReceipts);
            }

            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult Create()
        {
            JobWorkReceiptViewModel jobWorkReceiptViewModel = new JobWorkReceiptViewModel();
            jobWorkReceiptViewModel.TransNo = generalBL.GetSerialNo("JobWorkReceipt", "Code");
            jobWorkReceiptViewModel.TransDate = General.FormatDate(DateTime.Now);
            jobWorkReceiptViewModel.IssuedItems = new List<JobWorkIssuedModel>();
            jobWorkReceiptViewModel.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            jobWorkReceiptViewModel.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultGRNStore"));
            return View(jobWorkReceiptViewModel);
            
        }
        // POST: Manufacturing/JobWorkReceipt/Save
        [HttpPost]
        public ActionResult Save(JobWorkReceiptViewModel jobWorkReceiptViewModel)
        {
            try
            {
                JobWorkReceiptBO jobWorkReceiptBO = new JobWorkReceiptBO()
                {
                    ID= jobWorkReceiptViewModel.ID,
                    IsDraft = jobWorkReceiptViewModel.IsDraft,
                    TransNo = jobWorkReceiptViewModel.TransNo,
                    TransDate = General.ToDateTime(jobWorkReceiptViewModel.TransDate),
                    SupplierID = jobWorkReceiptViewModel.SupplierID,
                    IssueID = jobWorkReceiptViewModel.IssueID

                };
                List<JobWorkIssuedItemBO> jobWorkIssuedItems = new List<JobWorkIssuedItemBO>();
                JobWorkIssuedItemBO jobWorkIssuedItem;
                foreach (var item in jobWorkReceiptViewModel.IssuedItems)
                {
                    jobWorkIssuedItem = new JobWorkIssuedItemBO()
                    {
                        IssueTransID = item.IssueTransID,
                        PendingQuantity = item.PendingQuantity,
                        IsCompleted = item.IsCompleted
                    };

                    jobWorkIssuedItems.Add(jobWorkIssuedItem);
                }
                List<JobWorkReceiptItemBO> JobWorkReceiptItems = new List<JobWorkReceiptItemBO>();
                JobWorkReceiptItemBO JobWorkReceiptItem;
                foreach (var item in jobWorkReceiptViewModel.ReceiptItems)
                {
                    JobWorkReceiptItem = new JobWorkReceiptItemBO()
                    {
                        ReceiptItemID = item.ReceiptItemID,
                        ReceiptUnit = item.ReceiptUnit,
                        ReceiptQty = item.ReceiptQty,
                        ReceiptDate = General.ToDateTime(item.ReceiptDate),
                        WarehouseID=item.WarehouseID
                    };

                    JobWorkReceiptItems.Add(JobWorkReceiptItem);
                }
                 jobworkreceiptBL.Save(jobWorkReceiptBO, jobWorkIssuedItems,JobWorkReceiptItems);

                return
                     Json(new
                     {
                         Status = "success",
                         data = "",
                         message = ""
                     }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return
                      Json(new
                      {
                          Status = "",
                          data = "",
                          message = e.Message
                      }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            try
            {
                JobWorkReceiptViewModel jobWorkReceiptViewModel;
                JobWorkIssuedModel jobWorkIssuedModel;
                JobWorkReceiptModel jobWorkReceiptModel;
                JobWorkReceiptBO JobWorkReceiptBO= jobworkreceiptBL.GetJobWorkReceipt((int)id);
                jobWorkReceiptViewModel = new JobWorkReceiptViewModel()
                {

                    TransNo = JobWorkReceiptBO.TransNo,
                    TransDate = General.FormatDate(JobWorkReceiptBO.TransDate),
                    Supplier = JobWorkReceiptBO.Supplier,
                    IsDraft = JobWorkReceiptBO.IsDraft,
                    Status = JobWorkReceiptBO.IsDraft ? "draft" : "",
                    ID = JobWorkReceiptBO.ID,
                    IssueNo = JobWorkReceiptBO.IssueNo,
                    IssueID = JobWorkReceiptBO.IssueID,
                    SupplierID = JobWorkReceiptBO.SupplierID

                };

                List<JobWorkReceiptItemBO> jobWorkReceiptItemBO = jobworkreceiptBL.GetJobWorkReceiptItems((int)id);

                jobWorkReceiptViewModel.ReceiptItems = new List<JobWorkReceiptModel>();
                foreach (var m in jobWorkReceiptItemBO)
                {
                    jobWorkReceiptModel = new JobWorkReceiptModel()
                    {
                        ReceiptItemID = m.ReceiptItemID,
                        ReceiptItemName = m.ReceiptItemName,
                        ReceiptDate = General.FormatDate (m.ReceiptDate),
                        ReceiptQty = m.ReceiptQty,
                        ReceiptUnit = m.ReceiptUnit,
                        WarehouseID = m.WarehouseID
                    };
                    jobWorkReceiptViewModel.ReceiptItems.Add(jobWorkReceiptModel);
                }

                List<JobWorkIssuedItemBO> JobWorkIssuedItemBO = jobworkreceiptBL.GetJobWorkIssuedItems((int)id);

                jobWorkReceiptViewModel.IssuedItems = new List<JobWorkIssuedModel>();
                foreach (var m in JobWorkIssuedItemBO)
                {
                    jobWorkIssuedModel = new JobWorkIssuedModel()
                    {
                        IssueTransID = m.IssueTransID,
                        IsCompleted = m.IsCompleted,
                        PendingQuantity = m.PendingQuantity,
                        IssuedQty = m.IssuedQty,
                        IssuedItem = m.IssuedItem,
                        IssuedUnit = m.IssuedUnit
                        
                    };
                    jobWorkReceiptViewModel.IssuedItems.Add(jobWorkIssuedModel);
              
                }
                jobWorkReceiptViewModel.IssueDate = General.FormatDate(jobworkissueBL.GetJobWorkIssue((int)jobWorkReceiptViewModel.IssueID).IssueDate);
                return View(jobWorkReceiptViewModel);
            
            }
            catch (Exception e)
           {
                return View(e);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            try
            {
                JobWorkReceiptViewModel jobWorkReceiptViewModel;
                JobWorkIssuedModel jobWorkIssuedModel;
                JobWorkReceiptModel jobWorkReceiptModel;
                JobWorkReceiptBO JobWorkReceiptBO = jobworkreceiptBL.GetJobWorkReceipt((int)id);
                jobWorkReceiptViewModel = new JobWorkReceiptViewModel()
                {

                    TransNo = JobWorkReceiptBO.TransNo,
                    TransDate = General.FormatDate(JobWorkReceiptBO.TransDate),
                    Supplier = JobWorkReceiptBO.Supplier,
                    IsDraft = JobWorkReceiptBO.IsDraft,
                    Status = JobWorkReceiptBO.IsDraft ? "draft" : "",
                    ID = JobWorkReceiptBO.ID,
                    IssueNo = JobWorkReceiptBO.IssueNo,
                    IssueID = JobWorkReceiptBO.IssueID,
                    SupplierID = JobWorkReceiptBO.SupplierID

                };

                List<JobWorkReceiptItemBO> jobWorkReceiptItemBO = jobworkreceiptBL.GetJobWorkReceiptItems((int)id);

                jobWorkReceiptViewModel.ReceiptItems = new List<JobWorkReceiptModel>();
                foreach (var m in jobWorkReceiptItemBO)
                {
                    jobWorkReceiptModel = new JobWorkReceiptModel()
                    {
                        ReceiptItemID = m.ReceiptItemID,
                        ReceiptItemName = m.ReceiptItemName,
                        ReceiptDate = General.FormatDate(m.ReceiptDate),
                        ReceiptQty = m.ReceiptQty,
                        ReceiptUnit = m.ReceiptUnit,
                        WarehouseID=m.WarehouseID
                    };
                    jobWorkReceiptViewModel.ReceiptItems.Add(jobWorkReceiptModel);
                }

                List<JobWorkIssuedItemBO> JobWorkIssuedItemBO = jobworkreceiptBL.GetJobWorkIssuedItems((int)id);

                jobWorkReceiptViewModel.IssuedItems = new List<JobWorkIssuedModel>();
                foreach (var m in JobWorkIssuedItemBO)
                {
                    jobWorkIssuedModel = new JobWorkIssuedModel()
                    {
                        IssueTransID = m.IssueTransID,
                        IsCompleted = m.IsCompleted,
                        PendingQuantity = m.PendingQuantity,
                        IssuedQty = m.IssuedQty,
                        IssuedItem = m.IssuedItem,
                        IssuedUnit = m.IssuedUnit
                    };
                    jobWorkReceiptViewModel.IssuedItems.Add(jobWorkIssuedModel);
                }
                jobWorkReceiptViewModel.IssueDate = General.FormatDate(jobworkissueBL.GetJobWorkIssue((int)jobWorkReceiptViewModel.IssueID).IssueDate);
                jobWorkReceiptViewModel.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                jobWorkReceiptViewModel.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultGRNStore"));
                return View(jobWorkReceiptViewModel);

            }
            catch (Exception e)
            {
                return View(e);
            }
        }
    }
}