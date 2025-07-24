using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class InterCompanyBL : IInterCompanyContract
    {
        InterCompanyDAL interCompanyDAL;
        public  InterCompanyBL()
        {
            interCompanyDAL = new InterCompanyDAL();
        }
         public List<InterCompanyBO> GetInterCompanyList()
        {
            return interCompanyDAL.GetInterCompanyList();
        }
        public List<InterCompanyBO> GetInterCompanyDetails(int InterCompanyID)
        {
            return interCompanyDAL.GetInterCompanyDetails(InterCompanyID);
        }
        public int Save(InterCompanyBO interCompanyBO)
        {
            return interCompanyDAL.Save(interCompanyBO);

        }
        public int UpdateDepartment(InterCompanyBO interCompanyBO)
        {
            return interCompanyDAL.Update(interCompanyBO);
        }
    }
}
