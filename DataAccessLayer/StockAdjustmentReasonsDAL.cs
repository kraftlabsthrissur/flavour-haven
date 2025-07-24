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
   public class StockAdjustmentReasonsDAL
    {

        public int Save(StockAdjustmentReasonsBO stockAdjustmentReasonsBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateStockAdjustmentReasons(
                stockAdjustmentReasonsBO.Name,
                stockAdjustmentReasonsBO.Code,
                stockAdjustmentReasonsBO.Remarks,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                GeneralBO.FinYear,
                ReturnValue
                 );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }

        public DatatableResultBO GetStockAdjustmentReasonsList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetStockAdjustmentReasonsList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = item.DamageTypes,
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

        public StockAdjustmentReasonsBO GetstockAdjustmentReasonsDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetStockAdjustmentReasonsByID(ID).Select(a => new StockAdjustmentReasonsBO()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.DamageTypes,
                        Remarks=a.Remarks
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(StockAdjustmentReasonsBO stockAdjustmentReasonsBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateStockAdjustmentReasons(
                            stockAdjustmentReasonsBO.ID,
                            stockAdjustmentReasonsBO.Code,
                            stockAdjustmentReasonsBO.Name,
                            stockAdjustmentReasonsBO.Remarks,
                            GeneralBO.CreatedUserID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
