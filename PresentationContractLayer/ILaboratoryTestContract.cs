using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ILaboratoryTestContract
    {
        int Save(LaboratoryTestBO Lab,List<LabItemBO> LabItem);
        LaboratoryTestBO GetLaboratoryTestDetails();
        List<LaboratoryTestBO> GetLaboratoryTestList();
        List<LaboratoryTestBO> GetLaboratoryTestDetailsByID(int ID);
        List<LabItemBO> GetLaboratoryTestItemDetailsByID(int ID);
        DatatableResultBO GetLabTestList(string CodeHint, string TypeHint, string ServiceHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
