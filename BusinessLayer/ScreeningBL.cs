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
    public class ScreeningBL : IScreeningContract
    {
        ScreeningDAL screeningDAL;

        public ScreeningBL()
        {
            screeningDAL = new ScreeningDAL();
        }

        public DatatableResultBO GetOpPatientList(string Type, string CodeHint, string NameHint, string TimeHint, string TokenHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return screeningDAL.GetOpPatientList(Type, CodeHint, NameHint, TimeHint, TokenHint, DateHint, SortField, SortOrder, Offset, Limit);
        }

        public int Save(PatientDiagnosisBO diagnosis,List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems)
        {
            return screeningDAL.Save(diagnosis,VitalChartItems, Items, BaseLineItems, ReportItems);
        }

        public PatientDiagnosisBO GetVitalChart(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            return screeningDAL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
        }

        public List<PatientDiagnosisBO> GetDateListByID(int ID, int OPID)
        {
            return screeningDAL.GetDateListByID(ID, OPID);
        }

        //for Ilaj by priyanka
        public int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<QuestionnaireBO> QuestionnaireItems)
        {
            return screeningDAL.SaveV2(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, QuestionnaireItems);
        }
        public List<QuestionnaireBO> GetQuestionnaireAndAnswers(int PatientID, int AppointmentProcessID)
        {
            return screeningDAL.GetQuestionnaireAndAnswers(PatientID, AppointmentProcessID);
        }
        public List<QuestionnaireBO> GetQuestionnaireList(int PatientID, int AppointmentProcessID)
        {
            return screeningDAL.GetQuestionnaireList(PatientID, AppointmentProcessID);
        }
        public QuestionnaireBO IsPatientExists(int OPID)
        {
            return screeningDAL.IsPatientExists(OPID);
        }
    }
}
