using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IPhysiotherapyContract
    {
        PhysiotherapyTestBO GetPhysiotherapyCategory();
        int Save(PhysiotherapyTestBO Physiotherapy);
        List<PhysiotherapyTestBO> GetPhysiotherapyList();
        List<PhysiotherapyTestBO> GetPhysiotherapyDetailsByID(int ID);
        int Update(PhysiotherapyTestBO Physiotherapy);
    }
}
