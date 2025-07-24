using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;


namespace BusinessLayer
{
    public class GSTBL : IGSTContract
    {
        GSTDAL gstDAL;
        public GSTBL()
        {
            gstDAL = new GSTDAL();
        }

        public List<GSTBO> GetGstList()
        {
            return gstDAL.GetGstList();
        }

    }
}
