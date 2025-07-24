using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class DamageEntryBL : IDamageEntryContract
    {
        DamageEntryDAL damageEntryDAL;

        public DamageEntryBL()
        {
            damageEntryDAL = new DamageEntryDAL();
        }
        public List<DamageEntryItemBO> GetDamageEntryItems(int WarehouseID, int ItemCategoryID, int ItemID)
        {
            return damageEntryDAL.GetDamageEntryItems(WarehouseID, ItemCategoryID, ItemID);
        }
        public List<DamageEntryItemBO> GetBatchesByItemIDForDamageEntry(int WarehouseID, int ItemID)
        {
            return damageEntryDAL.GetBatchesByItemIDForDamageEntry(WarehouseID, ItemID);
        }
        public int Save(DamageEntryBO damageEntryBO, List<DamageEntryItemBO> items)
        {
            if (damageEntryBO.ID == 0)
            {
                return damageEntryDAL.Save(damageEntryBO, items);
            }
            else
            {
                return damageEntryDAL.Update(damageEntryBO, items);
            }
        }
        public List<DamageEntryBO> GetDamageEntryList()
        {
            return damageEntryDAL.GetDamageEntryList();
        }
        public List<DamageEntryItemBO> GetDamageEntryTrans(int ID)
        {
            return damageEntryDAL.GetDamageEntryTrans(ID);
        }
        public List<DamageEntryBO> GetDamageEntryDetail(int ID)
        {
            return damageEntryDAL.GetDamageEntryDetail(ID);
        }
    }
}
