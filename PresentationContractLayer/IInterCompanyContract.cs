using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IInterCompanyContract
    {
        List<InterCompanyBO> GetInterCompanyList();
        int Save(InterCompanyBO InterCompanyBO);
        int UpdateDepartment(InterCompanyBO InterCompanyBO);
        List<InterCompanyBO> GetInterCompanyDetails(int InterCompanyID);
    }
}
