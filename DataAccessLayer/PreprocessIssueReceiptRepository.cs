using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PreprocessIssueReceiptRepository : IPreprocessIssueReceipt
    {

        /// <summary>
        /// GetUnProcessedMaterialPurificationIssueItemList
        /// </summary>
        /// <returns></returns>
        public List<PreprocessReceiptItemBO> GetUnProcessedMaterialPurificationIssueItemList(string search = null)
        {
            List<PreprocessReceiptItemBO> item = new List<PreprocessReceiptItemBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    item = dbEntity.SpPGetUnProcessedMaterialPurificationIssueItemList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, search)
                        .Select(x => new PreprocessReceiptItemBO
                        {

                            ID = x.ID,
                            PurificationIssueID = x.PurificationIssueID ?? 0,
                            ItemID = x.ItemID ?? 0,
                            ItemName = x.ItemName,
                            PurifiedItemID = x.PurifiedItemID ?? 0,
                            PurifiedItemName = x.PurifiedItemName,
                            PurifiedItemUnit = x.PurifiedItemUnit,
                            Unit = x.Unit,
                            PurifiedItemUnitID = x.PurifiedItemUnitID,
                            Quantity = x.Quantity ?? 0,
                            QtyMet = x.QtyMet ?? 0,
                            BalanceQty = x.BalanceQty ?? 0,
                            ProcessID = x.ProcessID ?? 0,
                            TransDate = x.TransDate ?? new DateTime(),
                            TransTime = x.TransTime ?? new DateTime(),
                            ProcessName = x.ProcessName,
                            TransNo = x.TransNo
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;
        }


        public int Save(PreprocessReceiptBO preprocessReceiptBO)
        {
            int preProcessIssueID = 0;
            if (preprocessReceiptBO.ID <= 0)
            {   //Create
                preProcessIssueID = Create(preprocessReceiptBO);
                return preProcessIssueID;
            }
            else
            {       //Update
                preProcessIssueID = Update(preprocessReceiptBO);
                return preProcessIssueID;
            }
        }

        /// <summary>
        /// Create new PreProcessIssue
        /// </summary>
        /// <param name="preprocessIssueBO"></param>
        /// <returns></returns>
        private int Create(PreprocessReceiptBO preprocessReceiptBO)
        {
            int preProcessReceiptID = 0;
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PurificationReceipt";
                        ObjectParameter serialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter preProcessReceiptIDOut = new ObjectParameter("purificationReceiptID", typeof(int));


                        if (preprocessReceiptBO.IsDraft)
                        {
                            FormName = "DraftPurificationReceipt";
                        }

                        dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, serialNo);

                        var preProcessReceipt = dbEntity.SpPCreateMaterialPurificationReceipt(
                            serialNo.Value.ToString(),
                            preprocessReceiptBO.TransDate,
                            preprocessReceiptBO.TransDate,
                            preprocessReceiptBO.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            preProcessReceiptIDOut
                            );

                        preProcessReceiptID = (preProcessReceiptIDOut != null && preProcessReceiptIDOut.Value != null) ? Convert.ToInt32(preProcessReceiptIDOut.Value) : 0;
                        if (preProcessReceiptID > 0 && preprocessReceiptBO.PreProcessReceiptPurificationItemBOList.Count() > 0)
                        {
                            foreach (var itemBO in preprocessReceiptBO.PreProcessReceiptPurificationItemBOList)
                            {
                                dbEntity.SpPCreateMaterialPurificationReceiptTrans(
                                    preProcessReceiptID,
                                    itemBO.MaterialPurificationIssueTransID,
                                    itemBO.IssuedItemID,
                                    itemBO.ReceiptItemID,
                                    itemBO.ReceiptQuantity,
                                    itemBO.ReceiptItemUnitID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    itemBO.IsCompleted,
                                    itemBO.BalanceQty
                                    );
                            }
                        }

                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return preProcessReceiptID;
        }

        /// <summary>
        /// Update preprocess receipt
        /// </summary>
        /// <param name="preprocessIssueBO"></param>
        /// <returns></returns>
        private int Update(PreprocessReceiptBO preprocessReceiptBO)
        {
            int result = 0;
            using (ProductionEntities dbEntity = new ProductionEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        result = dbEntity.SpPUpdateMaterialPurificationReceipt(preprocessReceiptBO.ID, preprocessReceiptBO.TransDate, preprocessReceiptBO.IsDraft, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID,
                            GeneralBO.CreatedUserID);
                        foreach (var itemBO in preprocessReceiptBO.PreProcessReceiptPurificationItemBOList)
                        {
                            dbEntity.SpPCreateMaterialPurificationReceiptTrans(
                                preprocessReceiptBO.ID,
                                itemBO.MaterialPurificationIssueTransID,
                                itemBO.IssuedItemID,
                                itemBO.ReceiptItemID,
                                itemBO.ReceiptQuantity,
                                itemBO.ReceiptItemUnitID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                itemBO.IsCompleted,
                                itemBO.BalanceQty);
                        }

                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return result > 0 ? preprocessReceiptBO.ID : 0;
        }

        /// <summary>
        /// Get Material Purification Receipt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PreprocessReceiptBO GetMaterialPurificationReceipt(int id)
        {
            PreprocessReceiptBO preProcessReceiptBO = new PreprocessReceiptBO();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    preProcessReceiptBO = dbEntity.SpPGetMaterialPurificationReceipt(id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).
                        Select(x => new PreprocessReceiptBO
                        {
                            ID = x.ID,
                            IsDraft = x.IsDraft ?? false,
                            TransDate = x.TransDate ?? new DateTime(),
                            TransNo = x.Code,
                            IsCancelled = x.IsCancelled ?? false,
                            PreProcessReceiptPurificationItemBOList = GetMaterialPurificationReceiptDetails(id)
                        }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return preProcessReceiptBO;
        }

        /// <summary>
        /// Get Material Purification Receipt Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PreProcessReceiptPurificationItemBO> GetMaterialPurificationReceiptDetails(int id)
        {
            List<PreProcessReceiptPurificationItemBO> preProcessReceiptPurificationItemBOList = new List<PreProcessReceiptPurificationItemBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    preProcessReceiptPurificationItemBOList = dbEntity.SpPGetMaterialPurificationReceiptDetails(id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).
                        Select(x => new PreProcessReceiptPurificationItemBO
                        {
                            MaterialPurificationIssueTransID = x.PurificationIssueTransID ?? 0,
                            IssuedItemID = x.IssueItemID ?? 0,
                            IssuedItemName = x.IssuedItem,
                            IssuedItemUnit = x.IssuesUnit,
                            IssuedQuantity = x.IssuedQty ?? 0,
                            IssuedDate = x.IssuedDate ?? new DateTime(),
                            ProcessName = x.Process,
                            ReceiptItemID = x.ReceiptItemID ?? 0,
                            ReceiptItem = x.ReceiptItem,
                            ReceiptItemUnit = x.ReceiptItemUnit,
                            ReceiptQuantity = x.ReceiptQuantity ?? 0,
                            ReceiptDate = x.ReceiptDate ?? new DateTime(),
                            BalanceQty = x.BalanceQty ?? 0,
                            ReceiptItemUnitID = (int)x.ReceiptItemUnitID
                        }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return preProcessReceiptPurificationItemBOList;
        }

        /// <summary>
        /// Get Material Purification Receipts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PreProcessReceiptDisplayBO> GetMaterialPurificationReceipts()
        {
            List<PreProcessReceiptDisplayBO> preProcessReceiptPurificationItemBOList = new List<PreProcessReceiptDisplayBO>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    preProcessReceiptPurificationItemBOList = dbEntity.SpPGetMaterialPurificationReceiptDetails(0, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).
                        Select(x => new PreProcessReceiptDisplayBO
                        {
                            ID = x.ID,
                            IssuedItem = x.IssuedItem,
                            IssuedItemUnit = x.IssuesUnit,
                            IssuedQuantity = x.IssuedQty ?? 0,
                            Activity = x.Process,
                            ReceiptItem = x.ReceiptItem,
                            ReceiptItemUnit = x.ReceiptItemUnit,
                            ReceiptQuantity = x.ReceiptQuantity ?? 0,
                            IsDraft = x.IsDraft ?? false,
                            IsCancelled = (bool)x.IsCancelled,
                            ReceiptCode = x.Code,
                            ReceiptItemUnitID = (int)x.ReceiptItemUnitID
                        }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return preProcessReceiptPurificationItemBOList;
        }
        public int Cancel(int ID, String Table)
        {
            ProductionEntities dEntity = new ProductionEntities();
            return dEntity.SpCancelTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }

        public DatatableResultBO GetPreProcessReceiptList(string Type, string TransNoHint, string IssueItemHint, string UnitHint, string IssueQtyHint, string ReceiptItemHint, string ReceiptUnitHint, string ReceiptQtyHint, string ActivityHint, string QuantityLossHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetPreProcessReceiptList(Type, TransNoHint, IssueItemHint, UnitHint, IssueQtyHint, ReceiptItemHint, ReceiptUnitHint, ReceiptQtyHint, ActivityHint, QuantityLossHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransNo = item.Code,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                IssuedItem = item.IssuedItem,
                                IssuesUnit = item.IssuesUnit,
                                IssuedQty = item.IssuedQty,
                                ReceiptItem = item.ReceiptItem,
                                ReceiptItemUnit = item.ReceiptItemUnit,
                                ReceiptQuantity = item.ReceiptQuantity,
                                QtyLoss = item.QtyLoss,
                                Process = item.Process,
                                Status = item.Status
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