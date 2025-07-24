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
    public class PreprocessIssueRepository : IPreprocessIssue
    {
        private readonly ProductionEntities _entity;

        #region Constructor

        public PreprocessIssueRepository()
        {
            _entity = new ProductionEntities();
        }

        #endregion


        /// <summary>
        /// Save PreProductionIssue
        /// </summary>
        /// <param name="preprocessIssueBO"></param>
        /// <returns></returns>
        public int Save(PreprocessIssueBO preprocessIssueBO)
        {
            int preProcessIssueID = 0;
            if (preprocessIssueBO.PreProcessIssueID <= 0)
            {   //Create
                preProcessIssueID = Create(preprocessIssueBO);
                return preProcessIssueID;
            }
            else
            {
                Update(preprocessIssueBO);
                return preprocessIssueBO.PreProcessIssueID;
            }

        }

        private void CreateMaterialPurificationIssue(PreprocessIssueItemBO Item, bool IsDraft, int PreProcessIssueID, ProductionEntities dbEntity)
        {
            try
            {
                ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(string));
                dbEntity.SpPCreateMaterialPurificationIssueTransBatchWise(
                    PreProcessIssueID,
                    IsDraft,
                    Item.ItemID,
                    Item.Quantity,
                    Item.Quantity,
                    Item.ProcessID,
                    Item.UnitID,
                    Item.BatchID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    RetValue
                );
                if (Convert.ToInt32(RetValue.Value) == 0)
                {
                    throw new OutofStockException("Item out of stock");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Create new PreProcessIssue
        /// </summary>
        /// <param name="preprocessIssueBO"></param>
        /// <returns></returns>
        private int Create(PreprocessIssueBO preprocessIssueBO)
        {
            int preProcessIssueID = 0;
            using (var transaction = _entity.Database.BeginTransaction())
            {
                try
                {
                    string FormName = "PurificationIssue";
                    ObjectParameter serialNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter preProcessIssueIDOut = new ObjectParameter("PurificationIssueID", typeof(int));

                    if (preprocessIssueBO.IsDraft)
                    {
                        FormName = "DraftPurificationIssue";
                    }
                    _entity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, serialNo);

                    var preProcessIssue = _entity.SpPCreateMaterialPurificationIssue(
                        serialNo.Value.ToString(),
                        preprocessIssueBO.IssueDate,
                        preprocessIssueBO.IsDraft,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        preProcessIssueIDOut
                        );

                    preProcessIssueID = (preProcessIssueIDOut != null && preProcessIssueIDOut.Value != null) ? Convert.ToInt32(preProcessIssueIDOut.Value) : 0;
                    if (preProcessIssueID > 0 && preprocessIssueBO.PreprocessIssueItemBOList != null && preprocessIssueBO.PreprocessIssueItemBOList.Count() > 0)
                    {
                        foreach (var preprocessItemBO in preprocessIssueBO.PreprocessIssueItemBOList)
                        {
                            CreateMaterialPurificationIssue(preprocessItemBO, preprocessIssueBO.IsDraft, preProcessIssueID, _entity);
                        }
                    }
                    _entity.SpPCreateMaterialPurificationIssueTrans(

                        preProcessIssueID,
                         GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                    _entity.SaveChanges();
                    transaction.Commit();
                }
                catch (OutofStockException e)
                {
                    transaction.Rollback();
                    throw e;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return preProcessIssueID;
        }

        private int Update(PreprocessIssueBO preprocessIssueBO)
        {
            int result = 0;
            using (var transaction = _entity.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter serialNo = new ObjectParameter("SerialNo", typeof(string));

                    result = _entity.SpPUpdateMaterialPurificationIssue(preprocessIssueBO.IsDraft, preprocessIssueBO.PreProcessIssueID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID);

                    foreach (var preprocessItemBO in preprocessIssueBO.PreprocessIssueItemBOList)
                    {
                        CreateMaterialPurificationIssue(preprocessItemBO, preprocessIssueBO.IsDraft, preprocessIssueBO.PreProcessIssueID, _entity);
                    }
                    _entity.SpPCreateMaterialPurificationIssueTrans(

                       preprocessIssueBO.PreProcessIssueID,
                       GeneralBO.FinYear,
                      GeneralBO.LocationID,
                      GeneralBO.ApplicationID
                      );
                    _entity.SaveChanges();
                    transaction.Commit();
                }
                catch (OutofStockException e)
                {
                    transaction.Rollback();
                    throw e;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return preprocessIssueBO.PreProcessIssueID;
        }

        /// <summary>
        /// Get PreProcess Issue by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PreprocessIssueBO GetPreProcessIssue(int id)
        {
            PreprocessIssueBO bo = new PreprocessIssueBO();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    bo = dbEntity.SpPGetMaterialPurificationIssue(id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(x => new PreprocessIssueBO
                    {
                        PreProcessIssueID = x.ID,
                        IssueNo = x.Code,
                        IssueDate = x.TransDate ?? new DateTime(),
                        IsDraft = x.IsDraft ?? false,
                        IsCancelled=x.IsCancelled??false,
                        PreprocessIssueItemBOList = dbEntity.SpGetMaterialPurificationIssueTrans(id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(item => new PreprocessIssueItemBO()
                        {
                            ID = item.ID,
                            ItemID = item.ItemID ?? 0,
                            Quantity = item.Quantity ?? 0,
                            //QtyMet
                            ProcessID = item.ProcessID ?? 0,
                            ItemName = item.ItemName,
                            Unit = item.Unit,
                            UnitID=(int)item.UnitID,
                            ProcessName = item.ProcessName,
                            Stock = (decimal)item.Stock,
                            BatchID=item.BatchID,
                            BatchNo=item.BatchNo
                        }).ToList()
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return bo;
        }

        public List<PreprocessIssueBO> GetPreProcessList()
        {
            List<PreprocessIssueBO> list = new List<PreprocessIssueBO>();
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                list = dEntity.SpGetPreProcessIssueList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(x => new PreprocessIssueBO
                {
                    PreProcessIssueID = x.ID,
                    IssueNo = x.Code,
                    IssueDate = x.TransDate ?? new DateTime(),
                    IsDraft = x.IsDraft ?? false,
                    CreatedUserID = x.CreatedUserID ?? 0,
                    CreatedUser = x.CreatedUser,
                    IsProcessed = (bool)x.IsProcessed,
                    IsCancelled = (bool)x.IsCancelled,
                    ItemName=x.ItemName
                }).ToList();
                return list;
            }
        }

        public List<PreprocessIssueBO> GetProcessList()
        {
            List<PreprocessIssueBO> list = new List<PreprocessIssueBO>();
            using (ProductionEntities dEntity = new ProductionEntities())
            {
                list = dEntity.SpGetMaterialPurificationProcess().Select(a => new PreprocessIssueBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return list;
            }
        }

        public DatatableResultBO GetPreProcessIssueList(string Type, string TransNoHint, string TransDateHint, string CreatedUserHint, string ItemNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetPreProcessIssueListForDatatable(Type, TransNoHint, TransDateHint, CreatedUserHint, ItemNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                CreatedUser = item.CreatedUser,
                                ItemName = item.ItemName,
                                Status = item.Status,
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
