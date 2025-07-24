using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;

namespace DataAccessLayer
{
    public class RoleDAL
    {
        public List<ActionBO> GetActions(int RoleId)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    return dbEntity.SpGetActions(GeneralBO.ApplicationID, RoleId).Select(
                    a => new ActionBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Area = a.Area,
                        Controller = a.Controller,
                        Action = a.Action,
                        SortOrder = a.SortOrder,
                        Key = a.Key,
                        Checked = a.Checked,
                        Type = a.Type
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int Save(RoleBO roleBO, List<ActionIDBO> Actions, List<ActionIDBO> Tabs)
        {
            try
            {
                ObjectParameter RoleID = new ObjectParameter("RoleID", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpCreateRole(
                           roleBO.Code,
                           roleBO.RoleName,
                           roleBO.Remarks,
                           GeneralBO.ApplicationID,
                           RoleID
                        );
                    if (RoleID.Value != null)
                    {
                        foreach (var item in Actions)
                        {
                            dbEntity.SpCreateRolePrivileges(
                                Convert.ToInt16(RoleID.Value),
                                item.ID,
                                GeneralBO.ApplicationID
                                );
                        }
                        foreach (var item in Tabs)
                        {
                            dbEntity.SpCreateRoleTabPrivileges(
                               Convert.ToInt16(RoleID.Value),
                                item.ID,
                                GeneralBO.ApplicationID
                                );
                        }
                    };
                }

                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(RoleBO roleBO, List<ActionIDBO> Actions, List<ActionIDBO> Tabs)
        {
            try
            {

                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpUpdateRole(
                           roleBO.ID,
                           roleBO.Code,
                           roleBO.RoleName,
                           roleBO.Remarks,
                           GeneralBO.CreatedUserID,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID
                        );

                    foreach (var item in Actions)
                    {
                        dbEntity.SpCreateRolePrivileges(
                            roleBO.ID,
                            item.ID,
                            GeneralBO.ApplicationID
                            );
                    }
                    foreach (var item in Tabs)
                    {
                        dbEntity.SpCreateRoleTabPrivileges(
                            roleBO.ID,
                            item.ID,
                            GeneralBO.ApplicationID
                            );
                    }
                };
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<RoleBO> GetRoleList()
        {
            List<RoleBO> item = new List<RoleBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetRoleList(GeneralBO.ApplicationID).Select(k => new RoleBO
                    {
                        ID = k.ID,
                        RoleName = k.Name,
                        Code = k.Code,
                        Remarks = k.Remarks,
                        Actions = k.Actions,
                        Controller = k.Controller,
                        Tabs = k.Tabs
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RoleBO> GetRole(int ID)
        {
            try
            {
                List<RoleBO> roleBO = new List<RoleBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    return roleBO = dbEntity.SpGetRole(GeneralBO.ApplicationID, ID).Select(m => new RoleBO()
                    {
                        ID = m.ID,
                        Code = m.Code,
                        RoleName = m.Name,
                        Remarks = m.Remarks
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ActionBO> GetRoleActions(int ID)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    return dbEntity.SpGetRoleActions(ID, GeneralBO.ApplicationID).Select(
                    a => new ActionBO()
                    {
                        Name = a.Name,
                        Area = a.Area,
                        Controller = a.Controller,
                        Action = a.Action,
                        Type = a.Type
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public DatatableResultBO GetRoleListForDatatable(string Code, string RoleName, string Remarks, string Actions, string Tabs, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetRoleListForDatatable(Code, RoleName, Remarks, Actions, Tabs, SortField, SortOrder, Offset, Limit, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                RoleName = item.Name,
                                Code = item.Code,
                                Remarks = item.Remarks,
                                Actions = item.Actions,
                                Controller = item.Controller,
                                Tabs = item.Tabs
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public List<ActionBO> GetAreas()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    return dbEntity.SpGetAreas(GeneralBO.ApplicationID).Select(
                    a => new ActionBO()
                    {
                        Area = a.Area
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<ActionBO> GetActionsList(string Area, int RoleID)
        {
           using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    return dbEntity.SpGetActionsList(Area,RoleID, GeneralBO.ApplicationID).Select(
                    a => new ActionBO()
                    {
                        ID = (int)a.ID,
                        Name = a.Name,
                        Area = a.Area,
                        Controller = a.Controller,
                        Action = a.Action,
                        Key = a.Key,
                        Checked = a.Checked,
                        Type = a.Type
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<ActionBO> GetActionID(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetRoleActions(ID, GeneralBO.ApplicationID).Select(
                   a => new ActionBO()
                   {
                       ActionID =(int) a.ActionID,
                       TabID =(int) a.TabID,
                       Name = a.Name,
                       Area = a.Area,
                       Controller = a.Controller,
                       Action = a.Action,
                       Type = a.Type
                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
