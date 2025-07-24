using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IDiagnosisContract
    {
        int Save(DiagnosisBO diagnosisBO);
        DatatableResultBO GetDiagnosisList(string NameHint, string DescriptionHint, string SortField, string SortOrder, int Offset, int Limit);
        List<DiagnosisBO> GetDiagnosisDetails(int ID);
    }
}
