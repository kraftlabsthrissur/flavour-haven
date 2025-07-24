using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Utils;
using AutoMapper;
using Newtonsoft.Json;
using System.Web;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;


namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class JobWorkIssueController : Controller
    {
        private IGeneralContract generalBL;
        private IJobWorkIssueContrtact jobworkissueBL;
        private IWareHouseContract warehouseBL;
        public JobWorkIssueController()
        {
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            jobworkissueBL = new JobWorkIssueBL();
        }

        // GET: Manufacturing/JobWorkIssue
        public ActionResult Index()
        {
            try
            {
                List<JobWorkIssueViewModel> jobworkissue = jobworkissueBL.GetJobWorkIssue().Select(a => new JobWorkIssueViewModel()
                {
                    IssueNo = a.IssueNo,
                    IssueDate = General.FormatDate(a.IssueDate),
                    Supplier = a.Supplier,
                    Status = a.IsDraft ? "draft" : "",
                    ID=a.ID

                }).ToList();
                return View(jobworkissue);
            }

            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult Create()
        {
            JobWorkIssueViewModel jobworkissueviewmodel = new JobWorkIssueViewModel();
            jobworkissueviewmodel.IssueNo = generalBL.GetSerialNo("JobWorkIssue", "Code");
            jobworkissueviewmodel.IssueDate = General.FormatDate(DateTime.Now);
            jobworkissueviewmodel.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            jobworkissueviewmodel.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultRMStore"));
            return View(jobworkissueviewmodel);
        }
        // POST: Manufacturing/JobWorkIssue/Save
        [HttpPost]
        public ActionResult Save(JobWorkIssueViewModel jobWorkIssueViewModel)
        {
            try
            {
                JobWorkIssueBO jobworkissueBO = new JobWorkIssueBO()
                {
                    IsDraft = jobWorkIssueViewModel.IsDraft,
                    IssueNo = jobWorkIssueViewModel.IssueNo,
                    IssueDate = General.ToDateTime(jobWorkIssueViewModel.IssueDate),
                    SupplierID= jobWorkIssueViewModel.SupplierID,
                    ID=jobWorkIssueViewModel.ID,
                    

                };
                List<JobWorkIssueItemBO> jobWorkIssueItemS = new List<JobWorkIssueItemBO>();
                JobWorkIssueItemBO jobWorkIssueItem;
                foreach (var item in jobWorkIssueViewModel.Items)
                {
                    jobWorkIssueItem = new JobWorkIssueItemBO()
                    {
                        IssueItemID = item.IssueItemID,
                        IssueQty = item.IssueQty,
                        IssueUnit = item.IssueUnit,
                        WarehouseID=item.WarehouseID
                    };

                    jobWorkIssueItemS.Add(jobWorkIssueItem);
                }
                jobworkissueBL.Save(jobworkissueBO, jobWorkIssueItemS);

                return
                     Json(new
                     {
                         Status = "success",
                         data = "",
                         message = ""
                     }, JsonRequestBehavior.AllowGet);
            
            }
            catch(Exception e)
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
                JobWorkIssueViewModel JobWorkIssueViewModel;
                JobWorkIssueItemModel JobWorkIssueItem;
                JobWorkIssueBO JobWorkIssueBO = jobworkissueBL.GetJobWorkIssue((int)id);
                JobWorkIssueViewModel = new JobWorkIssueViewModel()
                {
                    ID = JobWorkIssueBO.ID,
                    IssueNo = JobWorkIssueBO.IssueNo,
                    IssueDate = General.FormatDate(JobWorkIssueBO.IssueDate),
                    IsDraft = JobWorkIssueBO.IsDraft,
                    Status = JobWorkIssueBO.IsDraft ? "draft" : "",
                    SupplierID = JobWorkIssueBO.SupplierID,
                    Supplier = JobWorkIssueBO.Supplier,
                    WarehouseID= JobWorkIssueBO.WarehouseID

                };

                List<JobWorkIssueItemBO> jobWorkIssueItemBO = jobworkissueBL.GetJobWorkIssueItems((int)id);

                JobWorkIssueViewModel.Items = new List<JobWorkIssueItemModel>();
                foreach (var m in jobWorkIssueItemBO)
                {
                    JobWorkIssueItem = new JobWorkIssueItemModel()
                    {
                        IssueItemName = m.IssueItemName,
                        IssueUnit = m.IssueUnit,
                        IssueQty = m.IssueQty,
                        IssueItemID = m.IssueItemID,
                        WarehouseID=m.WarehouseID
                    };
                    JobWorkIssueViewModel.Items.Add(JobWorkIssueItem);
                    JobWorkIssueViewModel.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                }

                return View(JobWorkIssueViewModel);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    
                    JobWorkIssueViewModel JobWorkIssueViewModel;
                    JobWorkIssueItemModel JobWorkIssueItem;
                    JobWorkIssueBO JobWorkIssueBO = jobworkissueBL.GetJobWorkIssue((int)id);
                    JobWorkIssueViewModel = new JobWorkIssueViewModel()
                    {
                        ID = JobWorkIssueBO.ID,
                        IssueNo = JobWorkIssueBO.IssueNo,
                        IssueDate = General.FormatDate(JobWorkIssueBO.IssueDate),
                        IsDraft = JobWorkIssueBO.IsDraft,
                        Status = JobWorkIssueBO.IsDraft ? "draft" : "",
                        SupplierID = JobWorkIssueBO.SupplierID,
                        Supplier = JobWorkIssueBO.Supplier,
                        WarehouseID = JobWorkIssueBO.WarehouseID

                    };

                    List<JobWorkIssueItemBO> jobWorkIssueItemBO = jobworkissueBL.GetJobWorkIssueItems((int)id);

                    JobWorkIssueViewModel.Items = new List<JobWorkIssueItemModel>();
                    foreach (var m in jobWorkIssueItemBO)
                    {
                        JobWorkIssueItem = new JobWorkIssueItemModel()
                        {
                            IssueItemName = m.IssueItemName,
                            IssueUnit = m.IssueUnit,
                            IssueQty = m.IssueQty,
                            IssueItemID = m.IssueItemID,
                            WarehouseID = m.WarehouseID,
                            Stock=m.Stock
                        };
                        JobWorkIssueViewModel.Items.Add(JobWorkIssueItem);
                    }
                    JobWorkIssueViewModel.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                    return View(JobWorkIssueViewModel);
                }
                catch (Exception e)
                {
                    return View();
                }
            }    

        }

        public JsonResult GetIssueList(DatatableModel Datatable)
        {
            try
            {
                string IssueNoHint = Datatable.Columns[2].Search.Value;
                string SupplierHint = Datatable.Columns[3].Search.Value;
                string IssueDateHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int SupplierID = Convert.ToInt32(Datatable.GetValueFromKey("SupplierID", Datatable.Params));
                DatatableResultBO resultBO = jobworkissueBL.GetIssueList(SupplierID, IssueNoHint, SupplierHint ,IssueDateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetIssueListForAutoComplete(string term = "",int SupplierID=0)
        {
            try
            {
                DatatableResultBO resultBO = jobworkissueBL.GetIssueList(SupplierID, term, "", "", "IssueNo", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetIssueItems(int IssueID)
        {
            try
            {
                List<JobWorkIssueItemBO> jobWorkIssueItemBO = jobworkissueBL.GetJobWorkIssueItems((int)IssueID);
                return Json(new { Status = "success", Data = jobWorkIssueItemBO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}