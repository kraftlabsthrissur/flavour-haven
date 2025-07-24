using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IDesignationContract
    {
        List<DesignationBO> GetDesignationList(); 
        List<DesignationBO> GetDesignationDetails(int DesignationID);
        int Save(DesignationBO Designation);
        int Update(DesignationBO Designation);
    }
}
