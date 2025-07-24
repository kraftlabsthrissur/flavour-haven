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
    public class AssetDAL
    {
        public DatatableResultBO GetAssetList(string Type, AssetFilterBO asset, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AssetEntities dbEntity = new AssetEntities())
                {
                    var result = dbEntity.SpGetAssetList(
                        Type,
                        asset.TransNoFromID,
                        asset.TransNoToID,
                        asset.TransDateFrom,
                        asset.TransDateTo,
                        asset.ReceiptNoFromID,
                        asset.ReceiptNoToID,
                        asset.AssetNameFrom,
                        asset.AssetNameTo,
                        asset.AssetName,
                        asset.AccountCategoryFrom,
                        asset.AccountCategoryTo,
                        asset.AccountCategoryID,
                        asset.SupplierNameFrom,
                        asset.SupplierNameTo,
                        asset.SupplierID,
                        TransNoHint,
                        AssetNumberHint,
                        AssetNameHint,
                        ItemNameHint,
                        SupplierNameHint,
                        SortField,
                        SortOrder,
                        Offset,
                        Limit,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();
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
                                SupplierName = item.SupplierName,
                                ItemName = item.ItemName,
                                AssetName = item.AssetName,
                                GrossBlockAssetValue = (decimal)item.GrossBlockAssetValue,
                                TransNo = item.TransNo,
                                AssetCode = item.AssetCode
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
        public DatatableResultBO GetCapitalList(string Type, DepreciationFilterBO depreciation, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AssetEntities dbEntity = new AssetEntities())
                {
                    var result = dbEntity.SpGetCapitalList(
                        depreciation.TransDateFrom,
                        depreciation.TransDateTo,
                        depreciation.FromCompanyDepreciationRate,
                        depreciation.ToCompanyDepreciationRate,
                        depreciation.FromIncomeTaxDepreciationRate,
                        depreciation.ToIncomeTaxDepreciationRate,
                        AssetNameHint,
                        ItemNameHint,
                        SupplierNameHint,
                        SortField,
                        SortOrder,
                        Offset,
                        Limit,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();
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
                                SupplierName = item.SupplierName,
                                ItemName = item.ItemName,
                                AssetName = item.AssetName,
                                GrossBlockAssetValue = (decimal)item.GrossBlockAssetValue,
                                CurrentValue = (decimal)item.CurrentValue,
                                DepreciationRate = item.CompaniesDepreciationRate,
                                OpeningWDV = item.OpeningWDVOnCompanyDepreciationRate,
                                AccumulatedDepreciationValue=(decimal)item.AccumulatedDepreciationValueOnCompanyDepreciationRate,
                                DepreciationValue=(decimal)item.DepreciationValueOnCompanyDepreciationRate
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

        public List<CategoryBO> GetAccountCategoryList()
        {
            List<CategoryBO> item = new List<CategoryBO>();
            using (AssetEntities dEntity = new AssetEntities())
            {
                item = dEntity.GetAccountCategoryForAsset().Select(a => new CategoryBO
                {
                    Name = a.Name,
                    ID = a.ID,
                }).ToList();
                return item;
            }
        }

        public int GetAssetUniqueNoCount(string hint)
        {

            ObjectParameter count = new ObjectParameter("count", typeof(int));
            int value;
            using (AssetEntities dEntity = new AssetEntities())
            {
                dEntity.SpGetAssetUniqueNoCount(hint, count);
                value = (int)count.Value;
                return value;
            }
        }
        public List<AssetBO> GetAsset(int ID)
        {
            List<AssetBO> list = new List<AssetBO>();
            using (AssetEntities dEntity = new AssetEntities())
            {
                list = dEntity.SpGetAsset(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AssetBO
                {
                    TransNo = a.TransNo,
                    TransDate = a.TransDate,
                    ItemName = a.ItemName,
                    SupplierName = a.SupplierName,
                    AssetUniqueNo = a.AssetUniqueNo,
                    ItemAccountCategory = a.ItemAccountCategory,
                    IsRepairable = (bool)a.IsRepairable,
                    CompanyDepreciationRate = (decimal)a.CompaniesDepreciationRate,
                    IncomeTaxDepreciationRate = (decimal)a.IncomeTaxDepreciationRate,
                    AssetValue = (decimal)a.CurrentValue,
                    AdditionToAssetNo = a.AdditionToAssetNumber,
                    ResidualValue = (decimal)a.ResidualValue,
                    DepreciationStartDate = (DateTime)a.DepreciationStartDate,
                    Department = a.DepartmentName,
                    Location = a.AssetLocationName,
                    Employee = a.EmployeeName,
                    Project = a.ProjectName,
                    Remark = a.Remarks,
                    Status = a.Status,
                    AssetName = a.AssetName,
                    DepreciationEndDate = a.DepreciationEndDate,
                    AssetCode = a.AssetCode,                   
                    LifeInYears = (decimal)a.LifeInYears,
                    IsCapital = a.AssetCode == null ? false : true,
                    Qty = (decimal)a.AssetQty,
                  

                }).ToList();
                return list;
            }

        }
        public bool Save(AssetBO assetBO)
        {
            using (AssetEntities dEntity = new AssetEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (assetBO.Status == "Capital")
                        {
                            var j = dEntity.SpUpdateSerialNo("Capital", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                            assetBO.AssetCode = SerialNo.Value.ToString();
                        }
                        else
                        {
                            assetBO.AssetCode = null;
                        }

                        var i = dEntity.SpCreateAsset(
                          assetBO.ID,
                          assetBO.TransDate,
                          assetBO.AssetCode,
                          assetBO.AssetName,
                          assetBO.AssetUniqueNo,
                          assetBO.IsRepairable,
                          assetBO.CompanyDepreciationRate,
                          assetBO.IncomeTaxDepreciationRate,
                          assetBO.LifeInYears,
                          assetBO.AdditionToAssetNo,
                          assetBO.ResidualValue,
                          assetBO.Remark,
                          assetBO.Status,
                          assetBO.StatusChangeDate,
                          assetBO.DepreciationStartDate,
                          assetBO.DepreciationEndDate,
                          assetBO.IsDraft,
                          GeneralBO.CreatedUserID,
                          DateTime.Now,
                          GeneralBO.FinYear,
                          GeneralBO.LocationID,
                          GeneralBO.ApplicationID
                             );
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public bool ChangeStatus(AssetBO assetBO)
        {
            using (AssetEntities dEntity = new AssetEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (assetBO.IsCapital == false && assetBO.Status == "Capital")
                        {
                            var j = dEntity.SpUpdateSerialNo("Capital", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                            assetBO.AssetCode = SerialNo.Value.ToString();
                        }
                        var i = dEntity.SpChangeAssetStatus(
                          assetBO.ID,
                          assetBO.Status,
                          assetBO.TransDate,
                          assetBO.AssetCode,
                          assetBO.DepreciationEndDate,
                          GeneralBO.CreatedUserID,
                          GeneralBO.FinYear,
                          GeneralBO.LocationID,
                          GeneralBO.ApplicationID
                             );
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
    }
}
