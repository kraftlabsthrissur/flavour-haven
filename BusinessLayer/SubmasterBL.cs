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
    public class SubmasterBL : ISubmasterContract
    {
        SubmasterDAL submasterDAL;

        public SubmasterBL()
        {
            submasterDAL = new SubmasterDAL();
        }

        public List<SubmasterBO> GetBloodGroupList()
        {
            try
            {
                return submasterDAL.GetBloodGroupList().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SubmasterBO> GetDurationList()
        {
            return submasterDAL.GetDurationList();
        }
        public List<SubmasterBO> GetTimePeriodList(int DurationID)
        {
            return submasterDAL.GetTimePeriodList(DurationID);
        }

        public List<SubmasterBO> GetPatientReferenceBy()
        {
            return submasterDAL.GetPatientReferenceBy();
        }

        public List<SubmasterBO> GetTreatmentGroupList()
        {
            return submasterDAL.GetTreatmentGroupList();
        }

        public List<SubmasterBO> GetMedicineCategoryGroupList()
        {
            return submasterDAL.GetMedicineCategoryGroupList();
        }
        public List<SubmasterBO> GetOccupationList()
        {
            return submasterDAL.GetOccupationList();
        }

        public int GetEmployeeCategoryID(string Name)
        {
            return submasterDAL.GetEmployeeCategoryID(Name);
        }

        public int GetConfigValue(string Name)
        {
            return submasterDAL.GetConfigValue(Name);
        }

        public List<SubmasterBO> GetRoomTypeList()
        {
            return submasterDAL.GetRoomTypeList();
        }

        public List<SubmasterBO> GetModeOfAdministration()
        {
            return submasterDAL.GetModeOfAdministration();
        }
        public List<SubmasterBO> GetSupplierForLabItems()
        {
            return submasterDAL.GetSupplierForLabItems();
        }

        public List<SubmasterBO> GetMonthList()
        {
            return submasterDAL.GetMonthList();
        }
        public List<SubmasterBO> GetGeneralDiscountType()
        {
            return submasterDAL.GetGeneralDiscountType();
        }

        public List<SubmasterBO> GetDiscountType()
        {
            return submasterDAL.GetDiscountType();
        }
        public List<SubmasterBO> GetFormList()
        {
            return submasterDAL.GetFormList();
        }

    }
}
