using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IDamageEntryContract
    {
        List<DamageEntryItemBO> GetDamageEntryItems(int WarehouseID, int ItemCategoryID, int ItemID);
        List<DamageEntryItemBO> GetBatchesByItemIDForDamageEntry(int WarehouseID, int ItemID);
        int Save(DamageEntryBO damageEntryBO, List<DamageEntryItemBO> items);
        List<DamageEntryBO> GetDamageEntryList();
        List<DamageEntryBO> GetDamageEntryDetail(int ID);
        List<DamageEntryItemBO> GetDamageEntryTrans(int ID);
    }
}
