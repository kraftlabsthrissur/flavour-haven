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
    public class DesignationBL: IDesignationContract
    {
        DesignationDAL designationDAL;
        public DesignationBL()
        {
            designationDAL = new DesignationDAL();
        }
        public List<DesignationBO> GetDesignationList()
        {
            return designationDAL.GetDesignationList();
        }
        public List<DesignationBO> GetDesignationDetails(int DesignationID)
        {
            return designationDAL.GetDesignationDetails(DesignationID);
        }
        public int Save(DesignationBO Designation)
        {
            return designationDAL.Save(Designation);
        }
        public int Update(DesignationBO Designation)
        {
            return designationDAL.Update(Designation);
        }
    }
}
