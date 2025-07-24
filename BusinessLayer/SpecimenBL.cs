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
    public class SpecimenBL : ISpecimenContract
    {
        private SpecimenDAL specimenDAL;
        public SpecimenBL()
        {
            specimenDAL = new SpecimenDAL();
        }
        public List<SpecimenBO> GetSpecimenList()
        {
            return specimenDAL.GetLaboratoryTestList();
        }
    }
}
