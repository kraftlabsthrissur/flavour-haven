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
     public class ProjectBL : IProjectContract
    {
        ProjectDAL projectDAL;

        public ProjectBL()
        {
            projectDAL = new ProjectDAL();
        }

        public List<ProjectBO> GetProjectList()
        {
            return projectDAL.GetProjectList();
        }
        public int Save(ProjectBO ProjectBO)
        {
            if (ProjectBO.ID == 0)
            {
                return projectDAL.Save(ProjectBO);
            }
            else
            {
                return projectDAL.Update(ProjectBO);
            }
        }

        public ProjectBO GetProjectDetails(int ID)
        {
            return projectDAL.GetProjectDetails(ID);
        }

    }
}
