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
   public class DiagnosisBL: IDiagnosisContract
    {
        DiagnosisDAL diagnosisDAL;

        public DiagnosisBL()
        {
            diagnosisDAL = new DiagnosisDAL();
        }

        public int Save(DiagnosisBO diagnosisBO)
        {
            if(diagnosisBO.ID == 0)
            {
                return diagnosisDAL.Save(diagnosisBO);
            }
            else
            {
                return diagnosisDAL.Update(diagnosisBO);
            }
            
        }

        public DatatableResultBO GetDiagnosisList(string NameHint, string DescriptionHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return diagnosisDAL.GetDiagnosisList(NameHint, DescriptionHint, SortField, SortOrder, Offset, Limit);
        }

        public List<DiagnosisBO> GetDiagnosisDetails(int ID)
        {
            return diagnosisDAL.GetDiagnosisDetails(ID);
        }
    }
}
