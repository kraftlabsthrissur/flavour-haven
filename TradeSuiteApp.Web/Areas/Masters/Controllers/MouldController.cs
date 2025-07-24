using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;


namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class MouldController : Controller
    {
        private IGeneralContract generalBL;
        private ILocationContract locationBL;
        private IMouldContract mouldBL;
        public MouldController()
        {
            locationBL = new LocationBL();
            mouldBL = new MouldBL();
            generalBL = new GeneralBL();
        }
        // GET: Masters/Mold
        public ActionResult Index()
        {
            try
            {
                List<MouldModel> Mould = mouldBL.GetMould().Select(a => new MouldModel()
                {
                    Code = a.Code,
                    ID = a.ID,
                    MouldName = a.MouldName,
                    InceptionDate = General.FormatDate(a.InceptionDate),
                    ExpairyDate = General.FormatDate(a.ExpairyDate)
                }).ToList();
                return View(Mould);
            }

            catch (Exception e)
            {
                return View(e);
            }

        }
        public ActionResult Create()
        {
            MouldModel moldModel = new MouldModel();
            List<MouldMachinesBO> Mouldmachine = mouldBL.GetMachines(0);
            MouldMachinesModel Mouldmachines;
            moldModel.Machines = new List<MouldMachinesModel>();
            foreach (var m in Mouldmachine)
            {
                Mouldmachines = new MouldMachinesModel()
                {
                    ID = m.ID,
                    Machine = m.Machine,
                    check = m.check
                };
                moldModel.Machines.Add(Mouldmachines);
            }
            moldModel.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            return View(moldModel);
        }

        [HttpPost]
        public ActionResult Save(MouldModel Mould)
        {
            try
            {
                MouldBO MouldBO = new MouldBO()
                {
                    ID = Mould.ID,
                    Code = Mould.Code,
                    MouldName = Mould.MouldName,
                    InceptionDate = General.ToDateTime(Mould.InceptionDate),
                    ExpairyDate = General.ToDateTime(Mould.ExpairyDate),
                    MandatoryMaintenanceTime = Mould.MandatoryMaintenanceTime,
                    ManufacturedBy = Mould.ManufacturedBy,
                    CurrentLocationID = Mould.CurrentLocationID
                };
                List<MouldItemBO> MouldItems = new List<MouldItemBO>();
                MouldItemBO MouldItem;
                foreach (var item in Mould.Items)
                {
                    MouldItem = new MouldItemBO()
                    {
                        ItemID = item.ItemID,
                        NoOfCavity = item.NoOfCavity,
                        StdTime = item.StdTime,
                        StdWeight = item.StdWeight,
                        StdGrindingWaste = item.StdGrindingWaste,
                        StdRunningWaste = item.StdRunningWaste,
                        StdShootingWaste = item.StdShootingWaste
                    };

                    MouldItems.Add(MouldItem);
                }
                List<MouldMachinesBO> Machines = new List<MouldMachinesBO>();
                MouldMachinesBO MouldMachines;
                foreach (var m in Mould.Machines)
                {
                    MouldMachines = new MouldMachinesBO()
                    {
                        ID = m.ID,
                        Machine = m.Machine
                    };
                    Machines.Add(MouldMachines);
                }

                mouldBL.Save(MouldBO, MouldItems, Machines);
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
                MouldModel MouldModel;
                MouldItemModel MouldItem;
                MouldBO MouldBO = mouldBL.GetMouldDetails((int)id);
                MouldModel = new MouldModel()
                {
                    ID = MouldBO.ID,
                    MouldName = MouldBO.MouldName,
                    Code = MouldBO.Code,
                    InceptionDate = General.FormatDate(MouldBO.InceptionDate),
                    ExpairyDate = General.FormatDate(MouldBO.ExpairyDate),
                    MandatoryMaintenanceTime = MouldBO.MandatoryMaintenanceTime,
                    ManufacturedBy = MouldBO.ManufacturedBy,
                    CurrentLocationID = MouldBO.CurrentLocationID,
                    CurrentLocationName = MouldBO.CurrentLocationName
                };

                List<MouldItemBO> MouldItems = mouldBL.GetMouldItems((int)id);

                MouldModel.Items = new List<MouldItemModel>();
                foreach (var m in MouldItems)
                {
                    MouldItem = new MouldItemModel()
                    {
                        ItemName = m.ItemName,
                        ItemID = m.ItemID,
                        NoOfCavity = m.NoOfCavity,
                        StdTime = m.StdTime,
                        StdWeight = m.StdWeight,
                        StdGrindingWaste = m.StdGrindingWaste,
                        StdRunningWaste = m.StdRunningWaste,
                        StdShootingWaste = m.StdShootingWaste,

                    };
                    MouldModel.Items.Add(MouldItem);
                }
                List<MouldMachinesBO> Mouldmachine = mouldBL.GetMachines((int)id);
                MouldMachinesModel Mouldmachines;
                MouldModel.Machines = new List<MouldMachinesModel>();
                foreach (var m in Mouldmachine)
                {
                    Mouldmachines = new MouldMachinesModel()
                    {
                        ID = m.ID,
                        Machine = m.Machine,
                        check = m.check
                    };
                    MouldModel.Machines.Add(Mouldmachines);
                }

                return View(MouldModel);
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

            try
            {
                MouldModel MouldModel;
                MouldItemModel MouldItem;
                MouldBO MouldBO = mouldBL.GetMouldDetails((int)id);
                MouldModel = new MouldModel()
                {
                    ID = MouldBO.ID,
                    MouldName = MouldBO.MouldName,
                    Code = MouldBO.Code,
                    InceptionDate = General.FormatDate(MouldBO.InceptionDate),
                    ExpairyDate = General.FormatDate(MouldBO.ExpairyDate),
                    MandatoryMaintenanceTime = MouldBO.MandatoryMaintenanceTime,
                    ManufacturedBy = MouldBO.ManufacturedBy,
                    CurrentLocationID = MouldBO.CurrentLocationID

                };

                List<MouldItemBO> MouldItems = mouldBL.GetMouldItems((int)id);

                MouldModel.Items = new List<MouldItemModel>();
                foreach (var m in MouldItems)
                {
                    MouldItem = new MouldItemModel()
                    {
                        ItemName = m.ItemName,
                        ItemID = m.ItemID,
                        NoOfCavity = m.NoOfCavity,
                        StdTime = m.StdTime,
                        StdWeight = m.StdWeight,
                        StdGrindingWaste = m.StdGrindingWaste,
                        StdRunningWaste = m.StdRunningWaste,
                        StdShootingWaste = m.StdShootingWaste,

                    };
                    MouldModel.Items.Add(MouldItem);
                }
                List<MouldMachinesBO> Mouldmachine = mouldBL.GetMachines((int)id);
                MouldMachinesModel Mouldmachines;
                MouldModel.Machines = new List<MouldMachinesModel>();
                foreach (var m in Mouldmachine)
                {
                    Mouldmachines = new MouldMachinesModel()
                    {
                        ID = m.ID,
                        Machine = m.Machine,
                        check = m.check
                    };
                    MouldModel.Machines.Add(Mouldmachines);
                }
                MouldModel.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
                return View(MouldModel);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public JsonResult GetMouldList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string MouldNameHint = Datatable.Columns[3].Search.Value;
                string ItemNameHint = Datatable.Columns[4].Search.Value;
                string MachineNameHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = mouldBL.GetMouldList(CodeHint, MouldNameHint, ItemNameHint, MachineNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMouldListForAutoComplete(string term = "")
        {
            try
            {
                DatatableResultBO resultBO = mouldBL.GetMouldList("", term, "", "", "MouldName", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

    }
}