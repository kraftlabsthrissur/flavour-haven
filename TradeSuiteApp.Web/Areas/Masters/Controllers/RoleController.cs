using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class RoleController : Controller
    {
        private IRoleContract roleBL;
        public RoleController()
        {
            roleBL = new RoleBL();
        }

        // GET: Masters/Role
        public ActionResult Index()
        {
            try
            {
                List<RoleModel> Roles = roleBL.GetRoleList().Select(a => new RoleModel()
                {
                    ID = a.ID,
                    Code = a.Code,
                    RoleName = a.RoleName,
                    Remarks = a.Remarks,
                    ActionNames = General.Split(a.Controller) + " - " + a.Actions,
                    Tabs = a.Tabs
                }).ToList();
                return View(Roles);
            }

            catch (Exception e)
            {
                return View(e);
            }
        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return View("Page Not Found");
            }
            try
            {
                RoleModel roleModel = roleBL.GetRole((int)id).Select(m => new RoleModel()
                {
                    ID = m.ID,
                    Code = m.Code,
                    Remarks = m.Remarks,
                    RoleName = m.RoleName,
                }).FirstOrDefault();
                roleModel.Actions = roleBL.GetRoleActions((int)id);
                roleModel.Areas = roleModel.Actions.GroupBy(a => a.Area).Select(a => a.First()).ToList();
                return View(roleModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return View("Page Not Found");
            }
            try
            {
                RoleModel roleModel = roleBL.GetRole((int)id).Select(m => new RoleModel()
                {
                    ID = m.ID,
                    Code = m.Code,
                    Remarks = m.Remarks,
                    RoleName = m.RoleName
                }).FirstOrDefault();
                roleModel.Areas = roleBL.GetAreas();

                //roleModel.Actions = roleBL.GetActions((int)id);
                //roleModel.Areas = roleModel.Actions.GroupBy(a => a.Area).Select(a => a.First()).ToList();
                // roleModel.ActionID = roleBL.GetRoleActions((int)id).Select(m => new ActionIDModel
                //{
                //    ID = m.ID
                //}).ToList();

              

                return View(roleModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult Create()
        {
            RoleModel roleModel = new RoleModel();
            roleModel.Actions = new List<ActionBO>();
            //roleModel.Actions = roleBL.GetActions(0);
            //roleModel.Areas = roleModel.Actions.GroupBy(a => a.Area).Select(a => a.First()).ToList();
            roleModel.Areas = roleBL.GetAreas();
            return View(roleModel);
        }

        public ActionResult Save(RoleModel model,int [] ActionID,int [] TabID)
        {
            try
            {
                RoleBO roleBO = new RoleBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    RoleName = model.RoleName,
                    Remarks = model.Remarks
                };
                List<ActionIDBO> Actions = new List<ActionIDBO>();
                if (ActionID != null)
                {

                    ActionIDBO Action;

                    foreach (var item in ActionID)
                    {
                        Action = new ActionIDBO()
                        {
                            ID = item,

                        };
                        Actions.Add(Action);
                    }
                }
                List<ActionIDBO> Tabs = new List<ActionIDBO>();
                if (TabID != null)
                {

                    ActionIDBO Tab;

                    foreach (var item in TabID)
                    {
                        Tab = new ActionIDBO()
                        {
                            ID = item,
                        };
                        Tabs.Add(Tab);
                    }
                }
                roleBL.Save(roleBO, Actions, Tabs);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRoleList(DatatableModel Datatable)
        {
            try
            {
                string Code = Datatable.Columns[1].Search.Value;
                string RoleName = Datatable.Columns[2].Search.Value;
                string Remarks = Datatable.Columns[3].Search.Value;
                string Actions = Datatable.Columns[4].Search.Value;
                string Tabs = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = roleBL.GetRoleListForDatatable(Code, RoleName, Remarks, Actions, Tabs,SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult Actions(string Area,int RoleID)
        {            
            List<ActionBO> Actions = roleBL.GetActionsList(Area,RoleID);
            return PartialView(Actions);
        }

        public JsonResult GetRoleActions(int RoleID)
        {
            List<ActionBO> ActionID = roleBL.GetActionID(RoleID);
            return Json(new {ActionID = ActionID }, JsonRequestBehavior.AllowGet);
        }

    }
}