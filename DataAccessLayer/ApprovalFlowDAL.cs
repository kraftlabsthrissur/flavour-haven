using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
    public class ApprovalFlowDAL
    {
        public List<ApprovalFlowBO> GetApprovalFlows()
        {
            List<ApprovalFlowBO> item = new List<ApprovalFlowBO>();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    item = dbEntity.SpGetApprovalFlow(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ApprovalFlowBO
                    {
                        ID = k.ID,
                        LocationCode = k.Code,
                        ApprovalType = k.FlowName,
                        ForDepartmentID = (int)k.DeptID,
                        ForDepartmentName = k.DepartmentName,
                        AmountAbove = (decimal)k.AmountAbove,
                        AmountBelow = (decimal)k.AmountBelow,
                        ItemCategoryID = (int)k.KeyValue1,
                        ItemCategoryName = k.KeyValue1Name,
                        ItemAccountsCategoryID = (int)k.KeyValue2,
                        ItemAccountsCategoryName = k.KeyValue2Name,
                        SupplierCategoryID = (int)k.KeyValue3,
                        SuppliercategoryName = k.KeyValue3Name,
                        SupplierAccountsCategoryID = (int)k.KeyValue4,
                        SupplierAccountscategoryName = k.KeyValue4Name,
                        AppQueueName = k.QueueName

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApprovalFlowBO GetApprovalFlow(int ID)
        {
            ApprovalFlowBO item = new ApprovalFlowBO();

            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    item = dbEntity.SpGetApprovalFlowDetails(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ApprovalFlowBO()
                    {
                        ID = k.ID,
                        LocationCode = k.Code,
                        ApprovalType = k.FlowName,
                        ForDepartmentID = (int)k.DeptID,
                        ForDepartmentName = k.DepartmentName,
                        AmountAbove = (decimal)k.AmountAbove,
                        AmountBelow = (decimal)k.AmountBelow,
                        ItemCategoryID = (int)k.KeyValue1,
                        ItemCategoryName = k.KeyValue1Name,
                        ItemAccountsCategoryID = (int)k.KeyValue2,
                        ItemAccountsCategoryName = k.KeyValue2Name,
                        SupplierCategoryID = (int)k.KeyValue3,
                        SuppliercategoryName = k.KeyValue3Name,
                        SupplierAccountsCategoryID = (int)k.KeyValue4,
                        SupplierAccountscategoryName = k.KeyValue4Name,
                        AppQueueName = k.QueueName,
                        ApprovalQueueID = k.QueueID,
                        LocationName = k.LocationName,
                        UserLocationID = k.UserLocationID

                    }).FirstOrDefault();

                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(ApprovalFlowBO approvalFlowBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (ApprovalEntities dbEntity = new ApprovalEntities())
            {
                dbEntity.SpCreateApprovalFlow(
                approvalFlowBO.LocationName,
                approvalFlowBO.ForDepartmentID,
                approvalFlowBO.AmountAbove,
                approvalFlowBO.AmountBelow,
                approvalFlowBO.ItemCategoryID,
                approvalFlowBO.ItemAccountsCategoryID,
                approvalFlowBO.ApprovalQueueID,
                approvalFlowBO.SupplierCategoryID,
                approvalFlowBO.SupplierAccountsCategoryID,
                GeneralBO.CreatedUserID,
               approvalFlowBO.UserLocationID,
                GeneralBO.ApplicationID,
                ReturnValue
                 );
                int item = Convert.ToInt16(ReturnValue.Value);
                if (item == -1)
                {
                    throw new Exception("Already exists");
                }

            }
            return 1;
        }

        public int Update(ApprovalFlowBO approvalFlowBO)
        {
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                     dbEntity.SpUpdateApprovalFlow(
                            approvalFlowBO.ID,
                            approvalFlowBO.LocationName,
                            approvalFlowBO.ForDepartmentID,
                            approvalFlowBO.AmountAbove,
                            approvalFlowBO.AmountBelow,
                            approvalFlowBO.ItemCategoryID,
                            approvalFlowBO.ItemAccountsCategoryID,
                            approvalFlowBO.ApprovalQueueID,
                            approvalFlowBO.SupplierCategoryID,
                            approvalFlowBO.SupplierAccountsCategoryID,
                            GeneralBO.CreatedUserID,
                            approvalFlowBO.UserLocationID,
                            GeneralBO.ApplicationID
                            );
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<ApprovalFlowItemBO> GetApprovalFlow(int ForDepartmentID, int ItemCategoryID, int ItemAccountsCategoryID, int UserLocationID)
        {
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    return dbEntity.SpGetItemsForApprovalFlow(ForDepartmentID, ItemCategoryID, ItemAccountsCategoryID, UserLocationID ).Select(a => new ApprovalFlowItemBO()
                    {
                        ID = a.ID,
                        ForDepartmentID = (int)a.ForDepartmentID,
                        AmountAbove = (decimal)a.AmountAbove,
                        AmountBelow = (decimal)a.AmountBelow,
                        ItemCategoryID = (int)a.KeyValue1,
                        ItemAccountsCategoryID = (int)a.KeyValue2,
                        ApprovalQueueID = (int)a.ApprovalQueueID,
                        SupplierAccountsCategoryID= (int)a.KeyValue4,
                        SupplierCategoryID=(int)a.KeyValue3,
                        ForDepartmentName=a.DepartmentName,
                        ItemCategoryName=a.ItemCategory,
                        ItemAccountsCategoryName=a.ItemAccountCategory,
                        AppQueueName=a.QueueName,
                        SupplierAccountscategoryName=a.SupplierAccountCategory,
                        SuppliercategoryName=a.SupplierCategory,
                        LocationName=a.Code 

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetApprovalFlowList(string AppQueueName, string ForDepartmentName, string AmountAbove, string AmountBelow, string ItemCategoryName, string ItemAccountsCategoryName, string SuppliercategoryName, string SupplierAccountscategoryName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {

                    var result = dbEntity.SpGetApprovalFlowList(AppQueueName, ForDepartmentName, AmountAbove, AmountBelow, ItemCategoryName, ItemAccountsCategoryName, SuppliercategoryName, SupplierAccountscategoryName, SortField, SortOrder, Offset, Limit,GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                LocationCode = item.Code,
                                ApprovalType = item.FlowName,
                                ForDepartmentID = (int)item.DeptID,
                                ForDepartmentName = item.DepartmentName,
                                AmountAbove = (decimal)item.AmountAbove,
                                AmountBelow = (decimal)item.AmountBelow,
                                ItemCategoryID = (int)item.KeyValue1,
                                ItemCategoryName = item.KeyValue1Name,
                                ItemAccountsCategoryID = (int)item.KeyValue2,
                                ItemAccountsCategoryName = item.KeyValue2Name,
                                SupplierCategoryID = (int)item.KeyValue3,
                                SuppliercategoryName = item.KeyValue3Name,
                                SupplierAccountsCategoryID = (int)item.KeyValue4,
                                SupplierAccountscategoryName = item.KeyValue4Name,
                                AppQueueName = item.QueueName
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

        public List<ApprovalFlowBO> GetLocationList()
        {
            List<ApprovalFlowBO> item = new List<ApprovalFlowBO>();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    item = dbEntity.SpGetLocationListForApprovalFlow(GeneralBO.ApplicationID).Select(k => new ApprovalFlowBO
                    {
                        UserLocationID = k.ID,
                        LocationName = k.Name,
                        LocationCode = k.Code,

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
