using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;


namespace BusinessLayer
{
    public class PatientBL : IPatientContract
    {

        PatientDAL patientDAL;

        public PatientBL()
        {
            patientDAL = new PatientDAL();
        }
        public DatatableResultBO GetPatientList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return patientDAL.GetPatientList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
        }
        public int Save(PatientBO patientBO)
        {
            if (patientBO.ID == 0)
            {
                return patientDAL.Save(patientBO);
            }
            else
            {
                return patientDAL.Update(patientBO);
            }
           
        }

        public  List<PatientBO> GetPatientDetails(int ID)
        {
            return patientDAL.GetPatientDetails(ID);
        }

    }
}
