using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class SalesRepresentativeDAL
    {
        public List<SalesRepresentativeBO> GetSalesRepresentatives()
        {
          
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetSalesRepresentatives(GeneralBO.ApplicationID).Select(a => new SalesRepresentativeBO
                {
                    ID = a.ID,
                    ParentID = (int)a.ParentID,
                    FSOName=a.FSOName,
                    Designation=a.Designation,
                    DesignationID=a.DesignationID,
                    IsSubLevel=(bool)a.IsSubLevel,
                    AreaID=(int)a.AreaID,
                    EmployeeID=(int)a.EmployeeID,
                    SalesIncentiveCategoryID=(int)a.SalesIncentiveCategoryID,
                    Area=a.Area,
                    IsChild= Convert.ToBoolean(a.IsChild)
                }).ToList();

            }
        }

        public List<SalesAreaBO> GetAreasByParentArea(int ParentAreaID)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetSalesAreasByParentArea(ParentAreaID).Select(a => new SalesAreaBO
                {
                    ID = a.ID,
                    Name=a.Name
                }).ToList();

            }
        }

        public int Save(SalesRepresentativeBO salesRepresentativeBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateSalesRepresentatives(
                salesRepresentativeBO.ID,
                salesRepresentativeBO.EmployeeID,
                salesRepresentativeBO.FSOName,
                salesRepresentativeBO.ParentID,
                salesRepresentativeBO.DesignationID,
                salesRepresentativeBO.IsSubLevel,
                salesRepresentativeBO.SalesIncentiveCategoryID,
                salesRepresentativeBO.AreaID,
                GeneralBO.CreatedUserID,
                GeneralBO.ApplicationID,
                 ReturnValue
                );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Employee Already Exist");
                }
            }
            return 1;
        }

        public List<SalesAreaBO> GetAreas(int AreaID)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetSalesAreas(AreaID).Select(a => new SalesAreaBO
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();

            }
        }

        public bool RemoveFSO(int ID)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpRemoveFSO(
                   ID,
                   ReturnValue);
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Delete failed");
                }
            }
            return true;
        }

        public DatatableResultBO GetSalesRepresentativeList(string CodeHint, string NameHint, string ParentNameHint, string AreaHint,string SalesIncentiveCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetSalesRepresentativeList(CodeHint, NameHint, ParentNameHint, AreaHint, SalesIncentiveCategoryHint, SortField, SortOrder, Offset, Limit,  GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                Code=item.Code,
                                Name=item.FSOName,
                                ParentName=item.ParentName,
                                Area=item.Area,
                                SalesIncentiveCategory=item.SalesIncentiveCategory
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
