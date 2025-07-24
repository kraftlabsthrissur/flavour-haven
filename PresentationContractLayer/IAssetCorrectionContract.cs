using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IAssetCorrectionContract
    {
        DatatableResultBO GetCapitalListForCorrection(AssetCorrectionFilterBO correction, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit);
        List<AssetCorrectionBO> GetAssetForCorrection(int ID);
        bool Save(AssetCorrectionBO assetCorrectionBO);
    }
}
