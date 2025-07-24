using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class ItemMasterBusinessService : IItemMasterService
    {

        public IEnumerable<ItemMasterBO> GetItemMasterDetails()
        {
            ItemMasterRepository itemMasterRepository = new ItemMasterRepository();

            return itemMasterRepository.GetItemMasterDetails();
        }

        public int InsertItemMaster(ItemMasterBO itemBO)
        {

            ItemMasterRepository itemMasterRepository = new ItemMasterRepository();
            itemMasterRepository.InsertItemMaster(itemBO);

            return 0;
        }
    }
}
