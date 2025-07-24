using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AppDAL
    {
        public DatatableResultBO GetActionList(string Type, string NameHint, string AreaHint, string ControllerHint, string ActionHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    var result = dbEntity.SpGetActionList(Type, NameHint, AreaHint, ControllerHint, ActionHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = item.Name,
                                Area = item.Area,
                                Controller = item.Controller,
                                Action = item.Action
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public int EnableItems(List<AppBO> EnableItems)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    foreach (var item in EnableItems)
                    {
                        dbEntity.SpEnableActionList(
                                   item.ID,
                                   item.Controller,
                                   item.Action,
                                   GeneralBO.ApplicationID
                                );
                    }
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
