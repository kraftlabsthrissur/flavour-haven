using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class EmployeeFreeMedicineCreditLimitDAL
    {
        public List<EmployeeFreeMedicineCreditLimitBO> GetEmployeeByFilterForFreeMedicineCreditLimit(int LocationID, int EmployeeCategoryID,int EmployeeID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetEmployeeByFilterForFreeMedicineCreditLimit(EmployeeCategoryID, EmployeeID, LocationID).Select(a => new EmployeeFreeMedicineCreditLimitBO()
                    {
                        EmployeeID = (int)a.EmployeeID,
                        EmployeeName = a.Employee,
                        EmployeeCategoryID = (int)a.EmployeeCategoryID,
                        EmployeeCategory = a.EmployeeCategory,
                        LocationID = (int)a.LocationID,
                        EmployeeCode = a.EmployeeCode,
                        BalAmount= (int)a.BalAmount,
                        UsedAmount = (int)a.UsedAmount,
                        CreditLimit = (int)a.CreditLimit,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(string EmployeeFreeMedicineCreditLimits)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                        dbEntity.SpCreateEmployeeFreeMedicineCreditLimit(
                        EmployeeFreeMedicineCreditLimits,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                            );
                };

                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetEmployeeFreeMedicineCreditLimitList(string EmployeeCodeHint, string EmployeeNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetEmployeeFreeMedicineCreditLimitList(EmployeeCodeHint, EmployeeNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                BalAmount = (decimal)item.BalAmount,
                                EmployeeName = item.Employee,
                                EmployeeCode = item.EmployeeCode,
                                CreditLimit = (decimal)item.CreditLimit,
                                UsedAmount = (decimal)item.UsedAmount
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
