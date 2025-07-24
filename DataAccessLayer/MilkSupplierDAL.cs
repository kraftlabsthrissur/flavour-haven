//File created by prama 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DBContext;
using BusinessObject;
using PresentationContractLayer;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class MilkSupplierDAL
    {
        public int SaveMilkSupplier(MilkSupplierBO milkSupplier)
        {

            using (PurchaseEntities entity = new PurchaseEntities())
            {
                try
                {
                    ObjectParameter Count = new ObjectParameter("IsSuccess", typeof(int));

                    entity.SpCreateMilkSupplier(milkSupplier.SupplierName, milkSupplier.Address, milkSupplier.ContactNo, GeneralBO.LocationID, GeneralBO.ApplicationID, Count);
                    return Convert.ToInt16(Count.Value);

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

  
    }
}
