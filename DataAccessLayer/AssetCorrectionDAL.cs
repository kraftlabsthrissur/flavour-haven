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
    public class AssetCorrectionDAL
    {
        public DatatableResultBO GetCapitalListForCorrection(AssetCorrectionFilterBO correction, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AssetEntities dbEntity = new AssetEntities())
                {
                    var result = dbEntity.SpGetCapitalListForCorrection(
                        correction.AssetCodeFrom,
                        correction.AssetCodeTo,
                        correction.AssetNameFrom,
                        correction.AssetNameTo,
                        correction.AssetName,
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
        public List<AssetCorrectionBO> GetAssetForCorrection(int ID)
        {
            List<AssetCorrectionBO> list = new List<AssetCorrectionBO>();
            using (AssetEntities dEntity = new AssetEntities())
            {
                list = dEntity.SpGetAssetForCorrection(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AssetCorrectionBO
                {
                    ItemName = a.ItemName,
                    AssetUniqueNo = a.AssetUniqueNo,
                    ItemAccountCategory = a.ItemAccountCategory,
                    AssetCode = a.AssetCode,
                    AssetName = a.AssetName,
                    DebitACCode = a.DebitAcCode,
                    DebitACName = a.DebitAcName,
                    CreditACCode = a.CreditAcCode,
                    CreditACName = a.CreditAcName,
                    AssetValue = (decimal)a.GrossBlockAssetValue,
                    DebitACID=(int)a.DebitAcID,
                    CreditACID=(int)a.CreditAcID
                }).ToList();
                return list;
            }

        }
        public bool Save(AssetCorrectionBO assetCorrectionBO)
        {
            using (AssetEntities dEntity = new AssetEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        var j = dEntity.SpUpdateSerialNo("AssetCorrection", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);


                        var i = dEntity.SpCreateAssetCorrection(
                            SerialNo.Value.ToString(),
                            assetCorrectionBO.CorrectionDate,
                            assetCorrectionBO.ID,
                            assetCorrectionBO.AmountValue,
                            assetCorrectionBO.DebitACID,                            
                            assetCorrectionBO.CreditACID,                           
                            assetCorrectionBO.IsAdditionDuringYear,
                            assetCorrectionBO.IsDepreciation,
                            assetCorrectionBO.Remark,
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
