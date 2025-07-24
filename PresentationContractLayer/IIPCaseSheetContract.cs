using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IIPCaseSheetContract
    {
        List<PatientDiagnosisBO> GetPatientDetails(int ID);
        List<DetailedExaminationBO> GetDetailedExaminationList(string Type);
        int Save(PatientDiagnosisBO diagnosis, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<VitalChartBO> VitalChartItems, List<ReportBO> ReportItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<LabtestsBO> LabTestItems, List<PhysiotherapyBO> PhysiotherapyItems, List<XrayBO> XrayItem, List<RoundsBO> RoundsList, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems, List<DischargeSummaryBO> DischargeSummary, List<DoctorListBO> DoctorList);
        List<ExaminationBO> GetExaminationList(int ID,int IPID);
        List<DetailedExaminationBO> GetOptionList(string Type);
        List<DropDownListBO> GetTherapistDetails();
        List<PatientDiagnosisBO> GetDateListByID(int ID);
        List<PatientDiagnosisBO> GetTreatmentNumberList();
        List<DropDownListBO> GetInstructionsList();
        List<DropDownListBO> GetMedicineTimeList(string Type);
        List<VitalChartBO> GetVitalChart(int PatientID, int IPID);
        List<TreatmentBO> GetTreatmentListByID(int PatientID, int IPID);
        List<ReportBO> GetReportListByID(int PatientID, int IPID);
        List<ReportBO> GetReportListByIDV5(int PatientID, int IPID);
        List<TreatmentItemBO> GetTreatmentMedicineListByID(int PatientID, int IPID);
        List<MedicineBO> GetMedicineListByID(int DischargeSummaryID,int PatientID, int IPID);
        List<MedicineItemBO> GetMedicinesItemsList(int DischargeSummaryID,int PatientID, int IPID);
        PatientDiagnosisBO GetCaseSheet(int PatientID, DateTime FromDate);
        List<ExaminationBO> GetExaminationByDate(int PatientID, DateTime FromDate);
        DatatableResultBO GetIPPatientList(string Type,string CodeHint, string NameHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit);
        List<MedicineBO> GetMedicinesDetails(int DischargeSumaryID,int PatientID, int IPID);
        List<TreatmentBO> GetTreatmentList(int PatientID, int IPID);
        List<DropDownListBO> GetLabTestAutoComplete(string Hint);
        List<DropDownListBO> GetPhysiotherapyAutoComplete(string Hint);
        List<DropDownListBO> GetXrayAutoComplete(string Hint);
        List<RoundsBO> GetRoundsList(int PatientID, int IPID);
        List<RoundsBO> GetRoundsListV5(int PatientID, int IPID);
        List<LabtestsBO> GetLabItems(int OPID);
        List<LabtestsBO> GetLabTestResultList(int OPID);
        List<LabtestsBO> GetCheckedTest(int OPID);
        List<XrayBO> GetXrayItemList(int OPID);
        List<XrayBO> GetPrescribedXrayTest(int OPID);
        List<XrayBO> GetXrayResultItems(int OPID);
        List<LabTestItemBO> GetLabItems(int PatientID, int IPID);
        List<LabTestItemBO> GetXrayItems(int PatientID, int IPID);
        DischargeSummaryBO GetDischargeSummary(int PatientID, int IPID);
        List<ExaminationBO> GetIPBaseLineInformationDetails(int ID, int OPID);
        List<ExaminationBO> GetIPBaseLineInformationDetailsByID(int ID, int OPID);
        List<DoctorListBO> GetDoctorList(int PatientID, DateTime FromDate, int IPID);
        //for Ilaj 
        List<ExaminationBO> GetBaseLineInformationList(int ID, int IPID);
        List<ExaminationBO> GetCaseSheetList(int ID, int OPID);
        List<ExaminationBO> GetRogaPareekshaList(int ID, int OPID);
        List<ExaminationBO> GetRogaNirnayamList(int ID, int OPID);
        List<ExaminationBO> GetIPExaminationList(int ID, int IPID);
        int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<RoundsBO> RoundsList, List<DischargeSummaryBO> DischargeSummary, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems);
        int SaveV5(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<ExaminationBO> NewItems, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<RoundsBO> RoundsList, List<DischargeSummaryBO> DischargeSummary, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems);
        List<ExaminationBO> GetDashaVidhaPareekhsalist(int PatientID, int IPID);

    }
}
