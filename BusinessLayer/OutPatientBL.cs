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
    public class OutPatientBL : IOutPatientContract
    {
        OutPatientDAL outPatientDAL;

        public OutPatientBL()
        {
            outPatientDAL = new OutPatientDAL();
        }

        public int Save(CustomerBO CustomerBO)
        {
            if (CustomerBO.ID == 0)
            {
                return outPatientDAL.Save(CustomerBO);
            }
            else
            {
                return outPatientDAL.Update(CustomerBO);
            }
        }

        public List<CustomerBO> GetOutPatientList()
        {
            return outPatientDAL.GetOutPatientList();
        }

        public List<CustomerBO> GetOutPatientDetails(int ID)
        {
            return outPatientDAL.GetOutPatientDetails(ID);
        }

        public DatatableResultBO GetOutPatientListForPopup(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return outPatientDAL.GetOutPatientListForPopup(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
