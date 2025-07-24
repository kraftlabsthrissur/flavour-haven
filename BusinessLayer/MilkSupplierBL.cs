//File created by prama 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
   public class MilkSupplierBL:IMilkSupplierContract
    {
        MilkSupplierDAL milkSupplierDAL;
        public MilkSupplierBL()
        {
            milkSupplierDAL = new MilkSupplierDAL();
        }

        public int SaveMilkSupplier(MilkSupplierBO milkSupplierBO)

        {
            return milkSupplierDAL.SaveMilkSupplier(milkSupplierBO);
        }
      
    }
}
