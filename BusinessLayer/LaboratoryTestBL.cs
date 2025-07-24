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
   public class LaboratoryTestBL: ILaboratoryTestContract
    {
        LaboratoryTestDAL laboratoryTestDAL;
        public LaboratoryTestBL()
        {
            laboratoryTestDAL = new LaboratoryTestDAL();
        }

        public int Save(LaboratoryTestBO Lab, List<LabItemBO> LabItem)
        {
            string XMLItems = XMLHelper.Serialize(LabItem);
            if (Lab.ID == 0)
            {
                return laboratoryTestDAL.Save(Lab, XMLItems);
            }
            else
            {
                return laboratoryTestDAL.Update(Lab, XMLItems);
            }           
        }
        public LaboratoryTestBO GetLaboratoryTestDetails()
        {
            return laboratoryTestDAL.GetLaboratoryTestDetails();
        }
        public List<LaboratoryTestBO> GetLaboratoryTestList()
        {
            return laboratoryTestDAL.GetLaboratoryTestList();
        }
        public List<LaboratoryTestBO> GetLaboratoryTestDetailsByID(int ID)
        {
            return laboratoryTestDAL.GetLaboratoryTestDetailsByID(ID);
        }
        public List<LabItemBO> GetLaboratoryTestItemDetailsByID(int ID)
        {
            return laboratoryTestDAL.GetLaboratoryTestItemDetailsByID(ID);
        }
        public DatatableResultBO GetLabTestList(string CodeHint, string TypeHint, string ServiceHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return laboratoryTestDAL.GetLabTestList(CodeHint, TypeHint, ServiceHint, NameHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
