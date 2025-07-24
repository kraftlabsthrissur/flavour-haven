using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class RoleBL : IRoleContract
    {
        RoleDAL roleDAL;
        public RoleBL()
        {
            roleDAL = new RoleDAL();
        }
        public List<ActionBO> GetActions(int RoleId)
        {
            List<ActionBO> Actions = roleDAL.GetActions(RoleId);
            Actions.Select(a =>
            {
                try
                {
                    a.ControllerNameFormatted = this.Split(a.Controller);

                    switch (a.Action)
                    {
                        case "Index":
                        case "index":
                            a.Name = "View list";
                            break;
                        case "Details":
                        case "details":
                            a.Name = "View details";
                            break;
                        default:
                            if (a.Type == "Tab")
                            {
                                a.Name = Split(a.Name);
                            }
                            else
                            {
                                a.Name = Split(a.Action);
                            }
                            break;
                    }
                    a.SortOrder = Actions.Where(b => b.Key == a.Key && b.SortOrder != 0).FirstOrDefault().SortOrder;
                }
                catch (Exception e)
                {
                    a.SortOrder = 0;
                }
                return a;
            }).ToList();
            return Actions;
        }

        private string Split(string s)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(s, " ");
        }

        public int Save(RoleBO roleBO, List<ActionIDBO> Actions, List<ActionIDBO> Tabs)
        {
            if (roleBO.ID == 0)
            {
                return roleDAL.Save(roleBO, Actions, Tabs);
            }
            else
            {
                return roleDAL.Update(roleBO, Actions, Tabs);
            }


        }

        public List<RoleBO> GetRoleList()
        {
            return roleDAL.GetRoleList();
        }

        public List<RoleBO> GetRole(int ID)
        {
            return roleDAL.GetRole(ID);
        }

        public List<ActionBO> GetRoleActions(int ID)
        {
            List<ActionBO> Actions = roleDAL.GetRoleActions(ID);
            Actions.Select(a =>
            {
                try
                {
                    a.ControllerNameFormatted = this.Split(a.Controller);

                    switch (a.Action)
                    {
                        case "Index":
                        case "index":
                            a.Name = "View list";
                            break;
                        case "Details":
                        case "details":
                            a.Name = "View details";
                            break;
                        default:
                            if (a.Area == "Reports")
                            {
                                a.Name = "View " + Split(a.Action) + " Report";
                            }
                            if (a.Type == "Tab")
                            {
                                a.Name = a.Name;
                            }
                            else
                            {
                                a.Name = Split(a.Action);
                            }
                            break;
                    }

                }
                catch (Exception e)
                {
                    throw (e);
                }
                return a;
            }).ToList();
            return Actions;
        }

        public DatatableResultBO GetRoleListForDatatable(string Code, string RoleName, string Remarks, string Actions, string Tabs, string SortField, string SortOrder, int Offset, int Limit)
        {
            return roleDAL.GetRoleListForDatatable(Code, RoleName, Remarks, Actions, Tabs, SortField, SortOrder, Offset, Limit);
        }

        public List<ActionBO> GetAreas()
        {
            return roleDAL.GetAreas();
        }

        public List<ActionBO> GetActionsList(string Area, int RoleID)
        {
            List<ActionBO> Actions = roleDAL.GetActionsList(Area, RoleID);
            Actions.Select(a =>
            {
                try
                {
                    a.ControllerNameFormatted = this.Split(a.Controller);

                    switch (a.Action)
                    {
                        case "Index":
                        case "index":
                            a.Name = "View list";
                            break;
                        case "Details":
                        case "details":
                            a.Name = "View details";
                            break;
                        default:

                            if (a.Area == "Reports" && a.Type != "Tab")
                            {
                                a.Name = "View " + Split(a.Action) + " Report";
                            }
                            
                            break;
                    }
                    a.SortOrder = Actions.Where(b => b.Key == a.Key && b.SortOrder != 0).FirstOrDefault().SortOrder;
                }
                catch (Exception e)
                {
                    a.SortOrder = 0;
                }
                return a;
            }).ToList();
            return Actions;
        }

        public List<ActionBO> GetActionID(int ID)
        {
            return roleDAL.GetActionID(ID);
        }
    }
}
