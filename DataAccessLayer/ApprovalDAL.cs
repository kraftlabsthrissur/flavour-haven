using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class ApprovalDAL
    {
        public int DoApprovalAction(int ApprovalID, int UserID, string ApprovalComment, string Status, int ClarificationUserID)
        {
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    return dbEntity.SpDoApprovalAction(ApprovalID, UserID, ApprovalComment, Status, ClarificationUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().Value;
                }
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public bool HasApprovalProcess(string Area, string Controller, string Action)
        {
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    return dbEntity.SpHasApprovalProcess(Area, Controller, Action, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().Value;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public int InitiateApprovalRequest(string Area, string Controller, string Action, int ID, int ApprovalFlowID)
        {
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    return dbEntity.SpInitiateApprovalRequest(Area, Controller, Action, ID, ApprovalFlowID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<ApprovalProcessBO> GetApprovalProcess(string Area, string Controller, string Action, int ID)
        {
            List<ApprovalProcessBO> ApprovalProcess = new List<ApprovalProcessBO>();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    ApprovalProcess = dbEntity.SpGetApprovalProcess(
                        Area,
                        Controller,
                        Action, ID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new ApprovalProcessBO()
                        {
                            ID = a.ID,
                            ApprovalID = (int)a.ApprovalID,
                            IsActive = (bool)a.IsActive,
                            Date = (DateTime)a.Date,
                            UserID = (int)a.UserID,
                            Status = a.Status,
                            Comment = a.Comment,
                            IsApproved = (bool)a.IsApproved,
                            SortOrder = a.SortOrder,
                            UserName = a.UserName,
                            NextActionUserID = (int)a.NextActionUserID,
                            IsActiveForUser = (bool)a.IsActiveForUser,
                            Requirement = a.Requirement
                        }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ApprovalProcess;
        }

        public List<ApprovalBO> GetApprovalData(string Area, string Controller, string Action, int ID)
        {

            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    return dbEntity.SpGetApprovals(
                        Area,
                        Controller,
                        Action, ID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                    ).Select(
                        a => new ApprovalBO()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            TransID = (int)a.TransID,
                            TransNo = a.TransNo,
                            ApprovalFlowID = (int)a.ApprovalFlowID,
                            IsApproved = (bool)a.IsApproved,
                            Status = a.Status,
                            LastActionUserID = (int)a.LastActionUserID,
                            NextActionUserID = (int)a.NextActionUserID,
                            UserID = (int)a.UserID,
                            CreatedDate = (DateTime)a.CreatedDate,
                            ApprovalTypeID = (int)a.ApprovalTypeID,
                            UserName = a.UserName,
                            Requirement = a.Requirement
                        }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetApprovalList(string Type, string CodeHint, string DateHint, string TypeHint, string SupplierNameHint, string FlowHint, string NetAmountHint, string UserNameHint, string LastActionByHint, string NextActionByHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    var result = dbEntity.SpGetApprovalList(Type, CodeHint, DateHint, TypeHint, SupplierNameHint, FlowHint, NetAmountHint, UserNameHint, LastActionByHint, NextActionByHint, StatusHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                TransID = item.TransID,
                                TransNo = item.TransNo,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                Type = item.Type,
                                ApprovalFlow = item.ApprovalFlow,
                                NetAmount = item.NetAmount,
                                SubmittedBy = item.SubmittedBy,
                                Status = item.Status,
                                Area = item.Area,
                                Controller = item.Controller,
                                Action = item.Action,
                                RowStatus = item.IsNext == 1 ? "next " + item.Status : item.Status,
                                NextAction = item.NextAction,
                                LastAction = item.LastAction,
                                SupplierName = item.SupplierName,
                                Remarks = item.Remark

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
    }
}