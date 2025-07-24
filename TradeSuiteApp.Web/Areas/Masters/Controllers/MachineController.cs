using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class MachineController : Controller
    {
        public IGeneralContract generalBL;
        public IMachineContract machineBL;
        public ILocationContract locationBL;
        public IProcessContract processBL;
        
        public MachineController()
         {
            machineBL = new MachineBL();
            generalBL = new GeneralBL();
            locationBL = new LocationBL();
            processBL = new ProcessBL();
         }
        // GET: Masters/Machine
        public ActionResult Index()
        {
            List<MachineModel> machineList = new List<MachineModel>();
            machineList = machineBL.GetMachineList().Select(m => new MachineModel
            {
                ID = m.ID,
                MachineCode = m.MachineCode,
                InsulationDate = General.FormatDate(m.InsulationDate),
                MachineName = m.MachineName,
                MachineNumber = m.MachineNumber
            }).ToList();
            return View(machineList);
        }

        // GET: Masters/Machine/Details/5
        public ActionResult Details(int id)
        {
            MachineModel machine = machineBL.GetMachineDetails(id).Select(a => new MachineModel
            {
                ID = a.ID,
                MachineCode = a.MachineCode,
                InsulationDate = General.FormatDate(a.InsulationDate),
                Model = a.Model,
                Location = a.Location,
                MachineName = a.MachineName,
                Process = a.Process,
                Motor = a.Motor,
                PowerConsumptionPerHour = a.PowerConsumptionPerHour,
                SoftwareVersion = a.SoftwareVersion,
                MachineNumber = a.MachineNumber,
                Manufacturer = a.Manufacturer,
                OperatorCount = a.OperatorCount,
                HelperCount = a.HelperCount,
                MaintenancePeriod = a.MaintenancePeriod,
                AverageCostPerHour = a.AverageCostPerHour
            }).First();
            
            return View(machine);
        }

        // GET: Masters/Machine/Create
        public ActionResult Create()
        {
            MachineModel machine = new MachineModel();
            machine.InsulationDate = General.FormatDate(DateTime.Now);
            machine.MachineType = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Motor", Value = "1"},
                                                 new SelectListItem { Text = "Fittings", Value = "2"},
                                                 new SelectListItem { Text = "Pipe", Value = "3"},
                                                 }, "Value", "Text");
            machine.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            machine.ProcessTypeList = new SelectList(processBL.GetProcessList(),"ID","Process");
            return View(machine);
        }

        // POST: Masters/Machine/Save
        [HttpPost]
        public ActionResult Save(MachineModel modal)
        {
            try
            {
                MachineBO machine = new MachineBO()
                {
                    ID = modal.ID,
                    MachineCode = modal.MachineCode,
                    InsulationDate = General.ToDateTime(modal.InsulationDate),
                    TypeID = modal.TypeID,
                    Model = modal.Model,
                    LocationID = modal.LocationID,
                    MachineName = modal.MachineName,
                    ProcessID = modal.ProcessID,
                    Motor = modal.Motor,
                    PowerConsumptionPerHour = modal.PowerConsumptionPerHour,
                    SoftwareVersion = modal.SoftwareVersion,
                    MachineNumber = modal.MachineNumber,
                    Manufacturer = modal.Manufacturer,
                    OperatorCount = modal.OperatorCount,
                    HelperCount = modal.HelperCount,
                    MaintenancePeriod = modal.MaintenancePeriod,
                    AverageCostPerHour = modal.AverageCostPerHour
                    
                };
                if(machine.ID == 0)
                {
                    machineBL.SaveMachine(machine);
                }
                else
                {
                    machineBL.UpdateMachineDetails(machine);
                }
                return Json(new { Status = "Success", Message = "Machine Created Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        // GET: Masters/Machine/Edit/5
        public ActionResult Edit(int id)
        {
            MachineModel machine = machineBL.GetMachineDetails(id).Select(a => new MachineModel
            {
                ID = a.ID,
                MachineCode = a.MachineCode,
                InsulationDate = General.FormatDate(a.InsulationDate),
                TypeID = a.TypeID,
                Model = a.Model,
                LocationID = a.LocationID,
                MachineName = a.MachineName,
                ProcessID = a.ProcessID,
                 Motor = a.Motor,
                PowerConsumptionPerHour = a.PowerConsumptionPerHour,
                SoftwareVersion = a.SoftwareVersion,
                MachineNumber = a.MachineNumber,
                Manufacturer = a.Manufacturer,
                OperatorCount = a.OperatorCount,
                HelperCount = a.HelperCount,
                MaintenancePeriod = a.MaintenancePeriod,
                AverageCostPerHour = a.AverageCostPerHour
            }).First();
            machine.LocationList = new  SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            machine.ProcessTypeList = new SelectList(processBL.GetProcessList(), "ID", "Process");

            return View(machine);
        }

        // POST: Masters/Machine/Edit/5
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

        // GET: Masters/Machine/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Masters/Machine/Delete/5
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

        public JsonResult GetAllMachineList(DatatableModel Datatable)
        {
            try
            {
                string MachineCodeHint = Datatable.Columns[1].Search.Value;
                string MachineNameHint = Datatable.Columns[2].Search.Value;
                string LoadedMouldHint = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                //int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                DatatableResultBO resultBO = machineBL.GetAllMachineList(MachineCodeHint, MachineNameHint, LoadedMouldHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetMachineListForAutoComplete(string Hint = "")
        {
            DatatableResultBO resultBO = machineBL.GetAllMachineList("", Hint, "", "MachineName", "ASC", 0, 20);

            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
    }
}
