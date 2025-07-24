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
    public class AssetBL : IAssetContract
    {
        AssetDAL assetDAL;
        public AssetBL()
        {
            assetDAL = new AssetDAL();
        }
        public DatatableResultBO GetAssetList(string Type, AssetFilterBO asset, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return assetDAL.GetAssetList(Type, asset, TransNoHint, AssetNumberHint, AssetNameHint, ItemNameHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetCapitalList(string Type, DepreciationFilterBO Depreciation, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return assetDAL.GetCapitalList(Type, Depreciation, AssetNameHint, ItemNameHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
        }
        public int GetAssetUniqueNoCount(string Hint)
        {
            return assetDAL.GetAssetUniqueNoCount(Hint);
        }
        public List<CategoryBO> GetAccountCategoryList()
        {
            return assetDAL.GetAccountCategoryList();
        }
        public List<AssetBO> GetAsset(int ID)
        {
            return assetDAL.GetAsset(ID);
        }

        public bool Save(AssetBO assetBO)
        {
            return assetDAL.Save(assetBO);
        }
        public bool ChangeStatus(AssetBO assetBO)
        {
            return assetDAL.ChangeStatus(assetBO);
        }
    }
}
