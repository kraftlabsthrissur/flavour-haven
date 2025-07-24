using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
  public  interface IRetirementContract
    {
        DatatableResultBO GetAssetForRetirementList(RetirementFilterBO Retirement, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit);
        List<AssetRetirementBO> GetAssetForRetirement(int ID);
        bool Save(AssetRetirementBO assetRetirementBO);

    }

}
