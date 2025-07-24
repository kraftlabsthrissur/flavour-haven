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
    public class RetirementDAL
    {
        public DatatableResultBO GetAssetForRetirementList(RetirementFilterBO retirement, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AssetEntities dbEntity = new AssetEntities())
                {
                    var result = dbEntity.SpGetAssetListForRetirement(
                        retirement.CapitalisationDateFrom,
                        retirement.CapitalisationDateTo,
                        retirement.AssetNameFrom,
                        retirement.AssetNameTo,
                        retirement.AssetName,
                        retirement.AssetCodeFrom,
                        retirement.AssetCodeTo,
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
                throw;
            }
            return DatatableResult;
        }
        public List<AssetRetirementBO> GetAssetForRetirement(int ID)
        {
            List<AssetRetirementBO> list = new List<AssetRetirementBO>();
            using (AssetEntities dEntity = new AssetEntities())
            {
                list = dEntity.SpGetAssetForRetirement(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AssetRetirementBO
                {
                    ItemName = a.ItemName,
                    ItemAccountCategory = a.ItemAccountCategory,
                    ResidualValue = (decimal)a.ResidualValue,
                    AssetName = a.AssetName,
                    AssetCode = a.AssetCode,
                    CapitalisationDate=a.CapitalisationDate,
                    Location=a.AssetLocationName,
                    AssetUniqueNo=a.AssetUniqueNo,
                    ClosingGrossBlockValue=(decimal)a.ClosingGrossBlockValue,
                    ClosingAccumulatedDepreciation=(decimal)a.AccumulatedDepreciationValue,
                    ClosingWDV=(decimal)a.ClosingWDV,
                    AssetQty=(decimal)a.CurrentQty
                   
                }).ToList();
                return list;
            }

        }
        public bool Save(AssetRetirementBO retirementBO)
        {
            using (AssetEntities dEntity = new AssetEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        
                            var j = dEntity.SpUpdateSerialNo("AssetRetirement", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                           

                        var i = dEntity.SpCreateAssetRetirement(
                         retirementBO.ID,
                         SerialNo.Value.ToString(),
                         retirementBO.Date,
                         retirementBO.ClosingGrossBlockValue,
                         retirementBO.ClosingAccumulatedDepreciation,
                         retirementBO.ClosingWDV,
                         retirementBO.SaleQty,
                         retirementBO.SaleValue,
                         retirementBO.EndDate, 
                         retirementBO.Status,                                              
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
    }
}
