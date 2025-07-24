using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class UserLocationsController : Controller
    {
        private IUserLocationsContract userlocationBL;

        public UserLocationsController()
        {
            userlocationBL = new UserLocationsBL();
        }
        // GET: Masters/UserLocations
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUserLocationsList(DatatableModel Datatable)
        {
            try
            {
                string Code = Datatable.Columns[1].Search.Value;
                string Name = Datatable.Columns[2].Search.Value;
                string UserName = Datatable.Columns[3].Search.Value;
                string DefaultLocation = Datatable.Columns[4].Search.Value;
                string CurrentLocation = Datatable.Columns[5].Search.Value;
                string OtherLocation = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = userlocationBL.GetUserLocationsList(Code, Name, UserName, DefaultLocation, CurrentLocation, OtherLocation, SortField, SortOrder, Offset, Limit);
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