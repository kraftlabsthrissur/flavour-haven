using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IScreeningContract
    {
        DatatableResultBO GetOpPatientList(string Type, string CodeHint, string NameHint, string TimeHint, string TokenHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit);
        int Save(PatientDiagnosisBO diagnosis,List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems);
        PatientDiagnosisBO GetVitalChart(int PatientID, DateTime FromDate, int AppointmentProcessID);
        List<PatientDiagnosisBO> GetDateListByID(int ID, int OPID);

        //for Ilaj by priyanka
        int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems,List<QuestionnaireBO> QuestionnaireItems);
        List<QuestionnaireBO> GetQuestionnaireAndAnswers(int PatientID, int AppointmentProcessID);
        List<QuestionnaireBO> GetQuestionnaireList(int PatientID, int AppointmentProcessID);
        QuestionnaireBO IsPatientExists(int OPID);
        //List<ExaminationBO> GetBaseLineInformationList(int ID, int OPID);

    }
}
