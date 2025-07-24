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
    public class IPCaseSheetBL : IIPCaseSheetContract
    {
        IPCaseSheetDAL ipCaseSheetDAL;

        public IPCaseSheetBL()
        {
            ipCaseSheetDAL = new IPCaseSheetDAL();
        }

        public List<PatientDiagnosisBO> GetPatientDetails(int ID)
        {
            return ipCaseSheetDAL.GetPatientDetails(ID);
        }

        public List<DetailedExaminationBO> GetDetailedExaminationList(string Type)
        {
            return ipCaseSheetDAL.GetDetailedExaminationList(Type);
        }

        public int Save(PatientDiagnosisBO diagnosis, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<VitalChartBO> VitalChartItems, List<ReportBO> ReportItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<LabtestsBO> LabTestItems, List<PhysiotherapyBO> PhysiotherapyItems, List<XrayBO> XrayItem, List<RoundsBO> RoundsList, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems, List<DischargeSummaryBO> DischargeSummary, List<DoctorListBO> DoctorList)
        {
            return ipCaseSheetDAL.Save(diagnosis, Items, BaseLineItems, VitalChartItems, ReportItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, LabTestItems, PhysiotherapyItems, XrayItem, RoundsList, InternalMedicinesList, InternalMedicinesItems, DischargeSummary, DoctorList);
        }

        public List<ExaminationBO> GetExaminationList(int ID, int IPID)
        {
            return ipCaseSheetDAL.GetExaminationList(ID, IPID);
        }

        public List<DetailedExaminationBO> GetOptionList(string Type)
        {
            return ipCaseSheetDAL.GetOptionList(Type);
        }

        public List<DropDownListBO> GetTherapistDetails()
        {
            return ipCaseSheetDAL.GetTherapistDetails();
        }

        public List<PatientDiagnosisBO> GetDateListByID(int ID)
        {
            return ipCaseSheetDAL.GetDateListByID(ID);
        }

        public List<PatientDiagnosisBO> GetTreatmentNumberList()
        {
            return ipCaseSheetDAL.GetTreatmentNumberList();
        }

        public List<DropDownListBO> GetInstructionsList()
        {
            return ipCaseSheetDAL.GetInstructionsList();
        }

        public List<DropDownListBO> GetMedicineTimeList(string Type)
        {
            return ipCaseSheetDAL.GetMedicineTimeList(Type);
        }

        public List<VitalChartBO> GetVitalChart(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetVitalChart(PatientID, IPID);
        }

        public List<RoundsBO> GetRoundsList(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetRoundsList(PatientID, IPID);
        }
        public List<RoundsBO> GetRoundsListV5(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetRoundsListV5(PatientID, IPID);
        }

        public List<TreatmentBO> GetTreatmentListByID(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetTreatmentListByID(PatientID, IPID);
        }

        public List<ReportBO> GetReportListByID(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetReportListByID(PatientID, IPID);
        }
        public List<ReportBO> GetReportListByIDV5(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetReportListByIDV5(PatientID, IPID);
        }
        public List<TreatmentItemBO> GetTreatmentMedicineListByID(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetTreatmentMedicineListByID(PatientID, IPID);
        }

        public List<MedicineBO> GetMedicinesDetails(int DischargeSumaryID, int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetMedicinesDetails(DischargeSumaryID, PatientID, IPID);
        }

        public List<MedicineItemBO> GetMedicinesItemsList(int DischargeSummaryID, int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetMedicinesItemsList(DischargeSummaryID, PatientID, IPID);
        }

        public List<MedicineBO> GetMedicineListByID(int DischargeSummaryID, int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetMedicineListByID(DischargeSummaryID, PatientID, IPID);
        }

        public PatientDiagnosisBO GetCaseSheet(int PatientID, DateTime FromDate)
        {
            return ipCaseSheetDAL.GetCaseSheet(PatientID, FromDate);
        }

        public List<ExaminationBO> GetExaminationByDate(int PatientID, DateTime FromDate)
        {
            return ipCaseSheetDAL.GetExaminationByDate(PatientID, FromDate);
        }

        public DatatableResultBO GetIPPatientList(string Type, string CodeHint, string NameHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return ipCaseSheetDAL.GetIPPatientList(Type, CodeHint, NameHint, DateHint, SortField, SortOrder, Offset, Limit);
        }

        public List<TreatmentBO> GetTreatmentList(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetTreatmentList(PatientID, IPID);
        }

        public List<DropDownListBO> GetLabTestAutoComplete(string Hint)
        {
            return ipCaseSheetDAL.GetLabTestAutoComplete(Hint);
        }

        public List<DropDownListBO> GetPhysiotherapyAutoComplete(string Hint)
        {
            return ipCaseSheetDAL.GetPhysiotherapyAutoComplete(Hint);
        }

        public List<DropDownListBO> GetXrayAutoComplete(string Hint)
        {
            return ipCaseSheetDAL.GetXrayAutoComplete(Hint);
        }

        public List<LabtestsBO> GetLabItems(int OPID)
        {
            return ipCaseSheetDAL.GetLabItems(OPID);
        }
        public List<LabtestsBO> GetLabTestResultList(int OPID)
        {
            return ipCaseSheetDAL.GetLabTestResultList(OPID);
        }
        public List<LabtestsBO> GetCheckedTest(int OPID)
        {
            return ipCaseSheetDAL.GetCheckedTest(OPID);
        }
        public List<XrayBO> GetXrayItemList(int OPID)
        {
            return ipCaseSheetDAL.GetXrayItemList(OPID);
        }
        public List<XrayBO> GetPrescribedXrayTest(int OPID)
        {
            return ipCaseSheetDAL.GetPrescribedXrayTest(OPID);
        }
        public List<XrayBO> GetXrayResultItems(int OPID)
        {
            return ipCaseSheetDAL.GetXrayResultItems(OPID);
        }

        public List<LabTestItemBO> GetLabItems(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetLabItems(PatientID, IPID);
        }
        public List<LabTestItemBO> GetXrayItems(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetXrayItems(PatientID, IPID);
        }
        public DischargeSummaryBO GetDischargeSummary(int PatientID, int IPID)
        {
            return ipCaseSheetDAL.GetDischargeSummary(PatientID, IPID);
        }
        public List<ExaminationBO> GetIPBaseLineInformationDetails(int ID, int IPID)
        {
            return ipCaseSheetDAL.GetIPBaseLineInformationDetails(ID, IPID);
        }
        public List<ExaminationBO> GetIPBaseLineInformationDetailsByID(int ID, int IPID)
        {
            return ipCaseSheetDAL.GetIPBaseLineInformationDetailsByID(ID, IPID);
        }

        public List<DoctorListBO> GetDoctorList(int PatientID, DateTime FromDate, int IPID)
        {
            return ipCaseSheetDAL.GetDoctorList(PatientID, FromDate, IPID);
        }
        //for ilaj by priyanka

        public List<ExaminationBO> GetCaseSheetList(int ID, int OPID)
        {
            return ipCaseSheetDAL.GetCaseSheetList(ID, OPID);
        }
        public List<ExaminationBO> GetBaseLineInformationList(int ID, int IPID)
        {
            return ipCaseSheetDAL.GetBaseLineInformationList(ID, IPID);
        }
        public List<ExaminationBO> GetRogaPareekshaList(int ID, int OPID)
        {
            return ipCaseSheetDAL.GetRogaPareekshaList(ID, OPID);
        }

        public List<ExaminationBO> GetRogaNirnayamList(int ID, int OPID)
        {
            return ipCaseSheetDAL.GetRogaNirnayamList(ID, OPID);
        }

        public int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<RoundsBO> RoundsList, List<DischargeSummaryBO> DischargeSummary, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems)
        {
            return ipCaseSheetDAL.SaveV2(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem, RoundsList, DischargeSummary, InternalMedicinesList, InternalMedicinesItems);
        }
        public int SaveV5(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<ExaminationBO> NewItems, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<RoundsBO> RoundsList, List<DischargeSummaryBO> DischargeSummary, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems)
        {
            return ipCaseSheetDAL.SaveV5(diagnosis, VitalChartItems, Items, NewItems, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem, RoundsList, DischargeSummary, InternalMedicinesList, InternalMedicinesItems);
        }
        public List<ExaminationBO> GetIPExaminationList(int ID, int IPID)
        {
            return ipCaseSheetDAL.GetIPExaminationList(ID, IPID);
        }
        public List<ExaminationBO> GetDashaVidhaPareekhsalist(int PatientID, int AppointmentProcessID)
        {
            return ipCaseSheetDAL.GetDashaVidhaPareekhsalist(PatientID, AppointmentProcessID);
        }
    }
}