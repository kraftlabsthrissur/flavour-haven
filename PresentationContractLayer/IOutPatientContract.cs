using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IOutPatientContract
    {
        int Save(CustomerBO CustomerBO);
        List<CustomerBO> GetOutPatientList();
        List<CustomerBO> GetOutPatientDetails(int ID);

        DatatableResultBO GetOutPatientListForPopup(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
