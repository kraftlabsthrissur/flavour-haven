//File created by prama on 14-4-18
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
    public class PremisesBL : IPremisesContract
    {
        PremisesDAL premisesDAl;
        public PremisesBL()
        {
            premisesDAl = new PremisesDAL();
        }
        public List<PremisesBO> GetPremisesList(int ID)
        {
            return premisesDAl.GetPremisesList(ID);
        }
        
            public List<PremisesBO> GetPremisesWithItemID(int ID)
        {
            return premisesDAl.GetPremisesWithItemID(ID);
        }
    }
}
