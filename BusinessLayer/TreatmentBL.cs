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
   public class TreatmentBL:ITreatmentContract
    {
        TreatmentDAL treatmentDAL;
        public TreatmentBL()
        {
            treatmentDAL = new TreatmentDAL();

        }
        public int Save(TreatmentListBO treatmentBO)
        {
            return treatmentDAL.Save(treatmentBO);
        }
        public List<TreatmentListBO> GetAllTreatment()
        {
            return treatmentDAL.GetAllTreatment();
        }
        public List<TreatmentListBO> GetTreatmentDetails(int ID)
        {
            return treatmentDAL.GetTreatmentDetails(ID);
        }
        public int UpdateTreatment(TreatmentListBO Treatment)
        {
            return treatmentDAL.Update(Treatment);
        }


        public DatatableResultBO GetTreatmentAutoComplete(string Hint)
        {
            return treatmentDAL.GetTreatmentAutoComplete(Hint);
        }
    }
}
