using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public  interface IProjectContract
    {
        List<ProjectBO> GetProjectList();
        int Save(ProjectBO ProjectBO);
        ProjectBO GetProjectDetails(int ID);

    }
}
