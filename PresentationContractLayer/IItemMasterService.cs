using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresentationContractLayer
{
    public interface IItemMasterService
    {

        IEnumerable<ItemMasterBO> GetItemMasterDetails();

        int InsertItemMaster(ItemMasterBO itemBO);
    }
}
