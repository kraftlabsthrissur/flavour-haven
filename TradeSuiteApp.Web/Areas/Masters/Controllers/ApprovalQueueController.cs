using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class ApprovalQueueController : Controller
    {
        IApprovalQueueContract approvalQueueBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;

        public string QueueName { get; private set; }

        public ApprovalQueueController()
        {
            approvalQueueBL = new ApprovalQueueBL();
            employeeBL = new EmployeeBL();
            locationBL = new LocationBL();

        }


        // GET: Masters/ApprovalQueue
        public ActionResult Index()
        {
            try
            {
                List<ApprovalQueueModel> ApprovalQueues = approvalQueueBL.GetApprovalQueues().Select(a => new ApprovalQueueModel()
                {
                    ID = a.AppQueueID,
                    QueueName = a.QueueName,
                    Location = a.LocationName

                }).ToList();
                return View(ApprovalQueues);
            }

            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Masters/ApprovalQueue/Details/5
        public ActionResult Details(int id)
        {
            try
            {

                ApprovalQueueBO ApprovalQueue = approvalQueueBL.GetApprovalQueue(id);
                ApprovalQueueModel ApprovalQueueModel = new ApprovalQueueModel
                {
                    ID = ApprovalQueue.AppQueueID,
                    QueueName = ApprovalQueue.QueueName,
                    Location = ApprovalQueue.LocationName,
                    LocationID=ApprovalQueue.LocationID
                };

                ApprovalQueueModel.QueueTrans = approvalQueueBL.GetApprovalQueueTrans(id).Select(a => new ApprovalQueueTransModel()
                {
                    ID = a.ID,
                    ApprovalQueueID = a.ApprovalQueueID,
                    UserID = a.UserID,
                    UserName = a.UserName,
                    SortOrder = a.SortOrder
                }
                ).ToList();

                return View(ApprovalQueueModel);
            }

            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Masters/ApprovalQueue/Create
        public ActionResult Create()
        {
            ApprovalQueueModel ApprovalQueue = new ApprovalQueueModel();
            ApprovalQueue.LocationID = GeneralBO.LocationID;
            ApprovalQueue.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Place");
            ApprovalQueue.EmployeeList = new SelectList(employeeBL.GetAspNetUsersList(), "ID", "Name");
            ApprovalQueue.QueueTrans = new List<ApprovalQueueTransModel>();


            return View(ApprovalQueue);
        }

        // GET: Masters/ApprovalQueue/Save
        public ActionResult Save(ApprovalQueueModel ApprovalQueue)
        {
            try
            {

                ApprovalQueueBO ApprovalQueueBO = new ApprovalQueueBO()
                {
                    QueueName = ApprovalQueue.QueueName,
                    AppQueueID = ApprovalQueue.ID,
                    LocationID = ApprovalQueue.LocationID
                };

                List<ApprovalQueueTransBO> ApprovalQueueItems = new List<ApprovalQueueTransBO>();
                ApprovalQueueTransBO ApprovalQueueItem;
                foreach (var item in ApprovalQueue.QueueTrans)
                {
                    ApprovalQueueItem = new ApprovalQueueTransBO()
                    {
                        UserID = item.UserID,
                        SortOrder = item.SortOrder

                    };

                    ApprovalQueueItems.Add(ApprovalQueueItem);
                }
                approvalQueueBL.Save(ApprovalQueueBO, ApprovalQueueItems);

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


        // POST: Masters/ApprovalQueue/Create
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

        // GET: Masters/ApprovalQueue/Edit/5
        // Code added on 25 Feb 2019
        public ActionResult Edit(int id)
        {
            try
            {

                ApprovalQueueBO ApprovalQueue = approvalQueueBL.GetApprovalQueue(id);
                ApprovalQueueModel ApprovalQueueModel = new ApprovalQueueModel
                {
                    ID = ApprovalQueue.AppQueueID,
                    QueueName = ApprovalQueue.QueueName,
                    Location = ApprovalQueue.LocationName,
                    LocationID = ApprovalQueue.LocationID,
                    LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Place"),
                    EmployeeList = new SelectList(employeeBL.GetAspNetUsersList(), "ID", "Name"),
                    QueueTrans = approvalQueueBL.GetApprovalQueueTrans(id).Select(a => new ApprovalQueueTransModel()
                    {
                        ID = a.ID,
                        ApprovalQueueID = a.ApprovalQueueID,
                        UserID = a.UserID,
                        UserName = a.UserName,
                        SortOrder = a.SortOrder
                    }).ToList()
                };

                return View(ApprovalQueueModel);
            }

            catch (Exception e)
            {
                return View();
            }

        }



    }
}
