//File created by prama on 14-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class PremisesDAL
    {
        public List<PremisesBO> GetPremisesList(int ID)
        {
            List<PremisesBO> Premises = new List<PremisesBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Premises = dEntity.SpGetPremises(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PremisesBO
                {
                    Name = a.name,
                    ID = a.Id,


                }).ToList();
                return Premises;
            }

        }
       
             public List<PremisesBO> GetPremisesWithItemID(int ID)
        {
            List<PremisesBO> Premises = new List<PremisesBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Premises = dEntity.SpGetPremisesWithItemID(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PremisesBO
                {
                    Name = a.PremisesName,
                    ID = a.PremisesID,


                }).ToList();
                return Premises;
            }

        }
    }
}
