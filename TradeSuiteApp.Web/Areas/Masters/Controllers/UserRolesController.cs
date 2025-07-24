using BusinessLayer;
using BusinessObject;
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
    public class UserRolesController : Controller
    {
        private IUserRolesContract userRolesBL;
        private ILocationContract locationBL;

        public UserRolesController()
        {
            userRolesBL = new UserRolesBL();
            locationBL = new LocationBL();
        }
        // GET: Masters/UserRoles
        public ActionResult Index()
        {
            List<UserRolesModel> UserRoles = new List<UserRolesModel>();
            UserRoles = userRolesBL.GetUserRoles().Select(a => new UserRolesModel
            {
                UserID = a.UserID,
                UserName = a.UserName,
                RoleName = a.RoleName,
                Code = a.Code
            }).ToList();

            return View(UserRoles);

        }

        public ActionResult Create()
        {
            UserRolesModel userRolesModel = new UserRolesModel();
            userRolesModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            return View(userRolesModel);
        }

        public ActionResult Save(UserRolesModel model)
        {
            try
            {
                List<UserRolesBO> Items = new List<UserRolesBO>();
                UserRolesBO Roles;
                if (model.UserRoles != null)
                {
                    foreach (var item in model.UserRoles)
                    {
                        Roles = new UserRolesBO()
                        {
                            RoleID = item.RoleID,
                            UserID = item.UserID,
                            LocationID = item.LocationID,
                        };
                        Items.Add(Roles);
                    }
                }
                userRolesBL.Save(Items);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsUserhaveRoles(int UserID)
        {
            try
            {
                bool IsExist = false;
                IsExist = userRolesBL.IsUserhaveRoles(UserID);
                return Json(new { Status = "success", Data = IsExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            try
            {
                UserRolesModel userRolesModel = new UserRolesModel();
                userRolesModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
                userRolesModel.ID = ID;
                List<UserRolesBO> Userroles = userRolesBL.GetUserRolesDetails(ID);
                userRolesModel.UserID = ID;
                userRolesModel.UserName = Userroles.FirstOrDefault().UserName;
                RolesModel RolesModel;
                userRolesModel.UserRoles = new List<RolesModel>();
                foreach (var items in Userroles)
                {
                    RolesModel = new RolesModel()
                    {
                        UserID = items.UserID,
                        UserName = items.UserName,
                        RoleID = items.RoleID,
                        RoleName = items.RoleName,
                        LocationID = items.LocationID,
                        Location = items.Location
                    };
                    userRolesModel.UserRoles.Add(RolesModel);
                }
                return View(userRolesModel);
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        public ActionResult Edit(int ID)
        {
            try
            {
                UserRolesModel userRolesModel = new UserRolesModel();
                userRolesModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
                userRolesModel.ID = ID;
                List<UserRolesBO> Userroles = userRolesBL.GetUserRolesDetails(ID);
                userRolesModel.UserID = ID;
                userRolesModel.UserName = Userroles.FirstOrDefault().UserName;
                RolesModel RolesModel;
                userRolesModel.UserRoles = new List<RolesModel>();
                foreach (var items in Userroles)
                {
                    RolesModel = new RolesModel()
                    {
                        UserID = items.UserID,
                        UserName = items.UserName,
                        RoleID = items.RoleID,
                        RoleName = items.RoleName,
                        LocationID = items.LocationID,
                        Location = items.Location
                    };
                    userRolesModel.UserRoles.Add(RolesModel);
                }
                return View(userRolesModel);
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        public JsonResult GetRolesList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = userRolesBL.GetRolesList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRolesForAutoComplete(string Hint = "")
        {
            try
            {
                DatatableResultBO resultBO = userRolesBL.GetRolesList("", Hint, "Name", "ASC", 0, 20);
                var result = new { Status = "success", data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}



