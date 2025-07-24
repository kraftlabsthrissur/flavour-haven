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
   public class LogicCodeDAL
    {
        public int Save(LogicCodeBO LogicCodeBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateLogicCode(
                LogicCodeBO.Code,
                LogicCodeBO.Name,
                LogicCodeBO.Remarks,
                GeneralBO.ApplicationID,
                GeneralBO.LocationID,
                ReturnValue
                 );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }

        public DatatableResultBO GetLogicCodeList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetLogicCodeList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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

        public LogicCodeBO GetLogicCodeDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetLogicCodeByID(ID).Select(a => new LogicCodeBO()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Remarks = a.Remarks,
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(LogicCodeBO LogicCodeBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateLogicCode(
                            LogicCodeBO.ID,
                            LogicCodeBO.Code,
                            LogicCodeBO.Name,
                            LogicCodeBO.Remarks,
                            GeneralBO.CreatedUserID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
