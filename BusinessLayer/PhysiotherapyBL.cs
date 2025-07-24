using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
  public  class PhysiotherapyBL: IPhysiotherapyContract
    {
        PhysiotherapyDAL physiotherapyDAL;

        public PhysiotherapyBL()
        {
            physiotherapyDAL = new PhysiotherapyDAL();
        }
        public PhysiotherapyTestBO GetPhysiotherapyCategory()
        {
            return physiotherapyDAL.GetPhysiotherapyCategory();
        }
        public int Save(PhysiotherapyTestBO Physiotherapy)
        {
            return physiotherapyDAL.Save(Physiotherapy);
        }
        public List<PhysiotherapyTestBO> GetPhysiotherapyList()
        {
            return physiotherapyDAL.GetPhysiotherapyList();
        }
        public List<PhysiotherapyTestBO> GetPhysiotherapyDetailsByID(int ID)
        {
            return physiotherapyDAL.GetPhysiotherapyDetailsByID(ID);
        }
        public int Update(PhysiotherapyTestBO Physiotherapy)
        {
            return physiotherapyDAL.Update(Physiotherapy);
        }
    }
}
