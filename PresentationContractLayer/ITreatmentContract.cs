using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ITreatmentContract
    {
        int Save(TreatmentListBO treatmentBO);
        List<TreatmentListBO> GetAllTreatment();
        List<TreatmentListBO> GetTreatmentDetails(int ID);
        int UpdateTreatment(TreatmentListBO Treatment);
        DatatableResultBO GetTreatmentAutoComplete(string Hint);

    }
}
