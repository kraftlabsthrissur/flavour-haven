using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
  public  interface IPatientContract
    {
        DatatableResultBO GetPatientList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);
        int Save(PatientBO patientBO);
        List<PatientBO> GetPatientDetails(int ID);

    }
}
