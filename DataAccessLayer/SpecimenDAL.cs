using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SpecimenDAL
    {
        public List<SpecimenBO> GetLaboratoryTestList()
        {
            try
            {
                List<SpecimenBO> specimenList= new List<SpecimenBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    specimenList = dbEntity.SpGetSpecimenList().Select(a => new SpecimenBO
                    {
                        ID = a.ID,                       
                        Name = a.Name
                    }).ToList();

                    return specimenList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
