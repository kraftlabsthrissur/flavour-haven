using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;


namespace DataAccessLayer
{
   public  class DepreciationDAL
    {
        public int CalculateDepreciation()
        {
            int success = 1;
            try
            {
                using (AssetEntities dEntity = new AssetEntities())
                {
                    dEntity.SpCalculateDepreciation(DateTime.Today, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    return success;
                }
            }catch(Exception )
            {
                return 0;
            }
        }

    }
}
