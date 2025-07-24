using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class StockAdjustmentScheduleDAL
    {
        public  int GetItemCount()
        {
            try
            {
                ObjectParameter ReturnValue = new ObjectParameter("count", typeof(int));

                using (MasterEntities dbEntity = new MasterEntities())
                {
                  
                    dbEntity.GetItemCount(ReturnValue);
                    return (int)ReturnValue.Value;
                }
               
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public int update(StockAdjustmentScheduleBO stockAdjustmentScheduleBO, List<ExcludedDateBO> Date)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                      

                        var i = dbEntity.UpdateStockAdjustmentSchedle(
                            stockAdjustmentScheduleBO.ID,
                            stockAdjustmentScheduleBO.ItemCount,
                            stockAdjustmentScheduleBO.TimeLimit,
                            stockAdjustmentScheduleBO.FrequencyOfItem,
                            stockAdjustmentScheduleBO.MorningStartTime,
                            stockAdjustmentScheduleBO.MorningEndTime,
                            stockAdjustmentScheduleBO.EveningStartTime,
                            stockAdjustmentScheduleBO.MorningEndTime,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );

                        if (Date != null)
                        {
                            foreach (var itm in Date)
                            {
                                dbEntity.SpCreateStockAdjustmentScheduleExcludedDates(
                                stockAdjustmentScheduleBO.ID,
                                itm.Date
                               );
                            }
                        }


                        transaction.Commit();
                        return Convert.ToInt32(stockAdjustmentScheduleBO.ID);
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public int save(StockAdjustmentScheduleBO stockAdjustmentScheduleBO, List<ExcludedDateBO> Date)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter Id = new ObjectParameter("ID", typeof(int));


                        var i = dbEntity.CreateStockAdjustmentSchedle(
                            stockAdjustmentScheduleBO.ItemCount,
                            stockAdjustmentScheduleBO.TimeLimit,
                            stockAdjustmentScheduleBO.FrequencyOfItem,
                            stockAdjustmentScheduleBO.MorningStartTime,
                            stockAdjustmentScheduleBO.MorningEndTime,
                            stockAdjustmentScheduleBO.EveningStartTime,
                            stockAdjustmentScheduleBO.MorningEndTime,
                            Id);

                        if (Date != null)
                        {
                            foreach (var itm in Date)
                            {
                                dbEntity.SpCreateStockAdjustmentScheduleExcludedDates(
                                Convert.ToInt32(Id.Value),
                                itm.Date
                               );
                            }

                        }
                        transaction.Commit();
                        return Convert.ToInt32(Id.Value);
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
       public DatatableResultBO GetStockAdjustmentList(string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.GetStockAdjustmentSchedleList( SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemCount = item.ItemCount,
                                ID = item.ID,                               
                                NoOfDaysToComplete = item.NoOfDaysToComplete,
                                Frequency = item.FrequencyOfItem,
                               
                                totalRecords=item.totalRecords,
                                recordsFiltered=item.recordsFiltered
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
     public   List<ExcludedDateBO> GetExcludedDate(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGettockAdjustmentScheduleExcludedDates(ID).Select(a => new ExcludedDateBO()
                    {
                        ID = a.ID,
                       Date=(DateTime)a.Date
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StockAdjustmentScheduleBO> GetStockAdjustmentScheduleDetail(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.GetStockAdjustmentSchedle(ID).Select(a => new StockAdjustmentScheduleBO()
                    {
                        ID = a.ID,
                        TimeLimit = (int)a.NoOfDaysToComplete,
                        ItemCount = (int)a.ItemCount,
                        FrequencyOfItem = (int)a.FrequencyOfItem,
                        MorningEndTime = (DateTime)a.MorningTo,
                        MorningStartTime = (DateTime)a.MorningFrom,
                        EveningEndTime = (DateTime)a.EveningTo,
                        EveningStartTime = (DateTime)a.EveningFrom
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

