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
    public class QCTestDefinitionDAL
    {
        public List<QCTestBO> GetQCTestDefinitionList()
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetQCTestDefinition().Select(a => new QCTestBO
                {
                    ItemID = (int)a.ItemID,
                    ItemName = a.ItemName,
                    TestName = a.TestName,
                    QCTest = a.QCTest
                }).ToList();

            }
        }

        public List<QCTestBO> GetQCList()
        {
            List<QCTestBO> QCTestList = new List<QCTestBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                QCTestList = dEntity.SpGetQCTestList().Select(a => new QCTestBO
                {
                    QCTestID = a.ID,
                    TestName = a.Name,

                }).ToList();
                return QCTestList;
            }

        }

        public int Save(List<QCTestItemBO> Items, QCTestBO QCTestBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    foreach (var item in Items)
                    {
                        if (item.ID == 0)
                        {
                            dbEntity.SpCreateQCTestDefinition(
                                    QCTestBO.ItemID,
                                    item.QCTestID,
                                    item.RangeFrom,
                                    item.RangeTo,
                                    item.Result,
                                    item.IsMandatory,
                                    item.StartDate,
                                    item.EndDate,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.ApplicationID

                                );
                        }
                        else
                        {
                            dbEntity.SpUpdateQcTestDefinition(
                            item.ID,
                            QCTestBO.ItemID,
                            item.QCTestID,
                            item.RangeFrom,
                            item.RangeTo,
                            item.Result,
                            item.IsMandatory,
                            item.StartDate,
                            item.EndDate,
                            GeneralBO.CreatedUserID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                        }
                    }
                   
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<QCTestBO> GetQCDefinitionDetails(int ID)
        {
            try
            {
                List<QCTestBO> QCTest = new List<QCTestBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    QCTest = dEntity.SpGetQCTestDefinitionByID(ID).Select(a => new QCTestBO
                    {
                        // ID=a.ID,
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName

                    }).ToList();
                    return QCTest;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QCTestItemBO> GetQCDefinitionTransDetails(int ID)
        {
            try
            {
                List<QCTestItemBO> QCTest = new List<QCTestItemBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    QCTest = dEntity.SpGetQCDefinitionTransDetails(ID).Select(a => new QCTestItemBO
                    {
                       // ID =(int)a.ID,
                        ItemID = (int)a.ItemID,
                        QCTestID = (int)a.QCTestID,
                        TestName = a.TestName,
                        RangeFrom = (decimal)a.FromLimit,
                        RangeTo = (decimal)a.ToLimit,
                        Result = a.Result,
                        IsMandatory = (bool)a.IsMandatory

                    }).ToList();
                    return QCTest;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QCTestItemBO> GetTestForItemList(int ItemID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetTestForItemList(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new QCTestItemBO()
                    {
                        ID = a.ID,
                        TestName = a.TestName,
                        QCTestID = (int)a.QCTestID,
                        RangeFrom = (decimal)a.FromLimit,
                        RangeTo = (decimal)a.ToLimit,
                        Result = a.Result,
                        IsMandatory = (bool)a.IsMandatory,
                        StartDate=a.StartDate,
                        EndDate =a.EndDate
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsDeletable(int QCTestID, int ID, int ItemID)
        {
            ObjectParameter IsDeletable = new ObjectParameter("IsDeletable", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpIsDeletableQCTestDefinition(
                   ID,
                   ItemID,
                   QCTestID,
                   IsDeletable);
                if (Convert.ToInt16(IsDeletable.Value) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public DatatableResultBO GetQcTestDefinitionList(string Code,string ItemName, string TestName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetQcTestDefinitionList(Code,ItemName, TestName, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemID = (int)item.ItemID,
                                ItemName = item.ItemName,
                                TestName = item.TestName,
                                QCTest = item.QCTest,
                                Code = item.Code
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


