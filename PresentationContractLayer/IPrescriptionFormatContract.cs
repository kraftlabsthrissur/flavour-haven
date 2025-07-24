using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IPrescriptionFormatContract
    {
        int Save(List<PrescriptionFormatItemBO> Items, PrescriptionFormatBO PrescriptionFormatBO);
        List<PrescriptionFormatItemBO> GetPrescriptionFormatItemList(int ID);
        List<PrescriptionFormatBO> GetPrescriptionFormatList();
        List<PrescriptionFormatBO> GetPrescriptionFormatDetails(int ID);
        List<PrescriptionFormatItemBO> GetPrescriptionFormatDetailTrans(int ID);
        List<PrescriptionFormatBO> GetPrescription(int CategoryID);


    }
}
