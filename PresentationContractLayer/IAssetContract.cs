using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IAssetContract
    {

        DatatableResultBO GetAssetList(string Type, AssetFilterBO asset, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit);
        List<CategoryBO> GetAccountCategoryList();
        List<AssetBO> GetAsset(int ID);
        bool Save(AssetBO assetBO);
        bool ChangeStatus(AssetBO assetBO);
        DatatableResultBO GetCapitalList(string Type, DepreciationFilterBO Depreciation, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit);
        int GetAssetUniqueNoCount(string Hint);
    }
}
