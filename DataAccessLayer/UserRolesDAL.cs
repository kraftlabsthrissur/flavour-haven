using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;


namespace DataAccessLayer
{
    public class UserRolesDAL
    {

        public List<UserRolesBO> GetUserRoles()
        {
            List<UserRolesBO> UserRoles = new List<UserRolesBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                UserRoles = dEntity.SpGetUserRoles(GeneralBO.ApplicationID).Select(a => new UserRolesBO
                {
                    UserID = (int)a.UserID,
                    UserName = a.Name,
                    RoleName = a.RoleName,
                    Code = a.Code
                }).ToList();
                return UserRoles;
            }

        }

        public int Save(List<UserRolesBO> Items)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpDeleteUserRoles(
                            Items.FirstOrDefault().UserID,
                            GeneralBO.CreatedUserID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                                 );
                    foreach (var item in Items)
                    {
                        dbEntity.SpCreateUserRoles(
                                  item.UserID,
                                  item.RoleID,
                                  GeneralBO.ApplicationID,
                                  item.LocationID
                                  );
                    }
                    return 1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UserRolesBO> GetUserRolesDetails(int ID)
        {
            try
            {
                List<UserRolesBO> UserRoles = new List<UserRolesBO>();
                using (MasterEntities dEntity = new MasterEntities())
                {
                    UserRoles = dEntity.SpGetUserRolesByID(ID).Select(a => new UserRolesBO
                    {
                        UserID = a.UserID,
                        UserName = a.UserName,
                        RoleID = a.RoleID,
                        RoleName = a.RoleName,
                        Location=a.Location,
                        LocationID=(int)a.LocationID
                    }).ToList();
                    return UserRoles;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsUserhaveRoles(int UserID)
        {
            try
            {
                ObjectParameter IsExist = new ObjectParameter("IsExist", typeof(bool));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result= dbEntity.SpIsUserhaveRoles(
                                  UserID,
                                  GeneralBO.ApplicationID,
                                  IsExist
                                  );
                    return Convert.ToBoolean(IsExist.Value);
                    
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetRolesList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetRolesList(CodeHint, NameHint,SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Code = item.Code,
                                Name = item.Name,
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



    }
}
