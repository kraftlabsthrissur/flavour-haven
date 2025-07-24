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
    public class RetirementBL:IRetirementContract
    {
        RetirementDAL retirementDAL;
        public RetirementBL()
        {
            retirementDAL = new RetirementDAL();
        }
        public DatatableResultBO GetAssetForRetirementList(RetirementFilterBO retirementBO, string TransNoHint, string AssetNumberHint, string AssetNameHint, string ItemNameHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return retirementDAL.GetAssetForRetirementList(retirementBO, TransNoHint, AssetNumberHint, AssetNameHint, ItemNameHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
        }
        public List<AssetRetirementBO> GetAssetForRetirement(int ID)
        {
            return retirementDAL.GetAssetForRetirement(ID);
        }
        public bool Save(AssetRetirementBO assetRetirementBO)
        {
            return retirementDAL.Save(assetRetirementBO);
        }
    }
}
