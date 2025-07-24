using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface ISubmasterContract
    {
        List<SubmasterBO> GetBloodGroupList();
        List<SubmasterBO> GetDurationList();
        List<SubmasterBO> GetTimePeriodList(int DurationID);
        List<SubmasterBO> GetPatientReferenceBy();
        List<SubmasterBO> GetMedicineCategoryGroupList();
        List<SubmasterBO> GetTreatmentGroupList();
        List<SubmasterBO> GetOccupationList();
        int GetEmployeeCategoryID(string Name);
        int GetConfigValue(string Name);
        List<SubmasterBO> GetRoomTypeList();
        List<SubmasterBO> GetModeOfAdministration();
        List<SubmasterBO> GetSupplierForLabItems();
        List<SubmasterBO> GetMonthList();
        List<SubmasterBO> GetGeneralDiscountType();
        List<SubmasterBO> GetDiscountType();
        List<SubmasterBO> GetFormList();
    }
}
