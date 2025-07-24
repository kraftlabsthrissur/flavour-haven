using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class AssetCorrectionBL : IAssetCorrectionContract
    {
        AssetCorrectionDAL assetCorrectionDAL;
        public AssetCorrectionBL()
        {
            assetCorrectionDAL = new AssetCorrectionDAL();
        }
        public DatatableResultBO GetCapitalListForCorrection(AssetCorrectionFilterBO correction, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return assetCorrectionDAL.GetCapitalListForCorrection(correction, TransNoHint, AssetNumberHint, AssetNameHint, ItemNameHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
        }
        public List<AssetCorrectionBO> GetAssetForCorrection(int ID)
        {
            return assetCorrectionDAL.GetAssetForCorrection(ID);
        }
        public bool Save(AssetCorrectionBO assetCorrectionBO)
        {
            return assetCorrectionDAL.Save(assetCorrectionBO);
        }
    }
}
