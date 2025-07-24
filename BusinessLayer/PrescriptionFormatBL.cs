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
   public class PrescriptionFormatBL:IPrescriptionFormatContract
    {
        PrescriptionFormatDAL prescriptionformatDAL;

        public PrescriptionFormatBL()
        {
            prescriptionformatDAL = new PrescriptionFormatDAL();
        }

        public int Save(List<PrescriptionFormatItemBO> Items, PrescriptionFormatBO PrescriptionFormatBO)
        {
            return prescriptionformatDAL.Save(Items, PrescriptionFormatBO);
        }
        public List<PrescriptionFormatItemBO> GetPrescriptionFormatItemList(int ID)
        {
            return prescriptionformatDAL.GetPrescriptionFormatItemList(ID);
        }
        public List<PrescriptionFormatBO> GetPrescriptionFormatList()
        {
            return prescriptionformatDAL.GetPrescriptionFormatList();
        }
        public List<PrescriptionFormatBO> GetPrescriptionFormatDetails(int ID)
        {
            return prescriptionformatDAL.GetPrescriptionFormatDetails(ID);
        }
        public List<PrescriptionFormatItemBO> GetPrescriptionFormatDetailTrans(int ID)
        {
            return prescriptionformatDAL.GetPrescriptionFormatDetailsTrans(ID);
        }

        public List<PrescriptionFormatBO> GetPrescription(int CategoryID)
        {
            return prescriptionformatDAL.GetPrescription(CategoryID);
        }
    }
}
