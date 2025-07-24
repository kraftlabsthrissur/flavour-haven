using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
    public class ProjectDAL
    {
        public List<ProjectBO> GetProjectList()
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetProject().Select(Project => new ProjectBO
                {
                       ID=Project.ID,
                       Name=Project.Name,
                       Code=Project.Code,
                       Place = Project.Place,
                       Description=Project.Description
                }).ToList();

            }
        }

        public int Save(ProjectBO ProjectBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateProject(
                ProjectBO.Name,
                ProjectBO.Description,
                ProjectBO.Code,
                ProjectBO.StartDate,
                ProjectBO.EndDate,
                GeneralBO.CreatedUserID,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID,
                ReturnValue
                 );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Project already exists");
                }
            }
            return 1;
        }

        public int Update(ProjectBO ProjectBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateProject(
                        ProjectBO.ID,
                        ProjectBO.Name,
                        ProjectBO.Description,
                        ProjectBO.Code,
                        ProjectBO.StartDate,
                        ProjectBO.EndDate,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProjectBO GetProjectDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetProjectByID(ID).Select(a => new ProjectBO()
                    {
                        ID = a.ID,
                        Code=a.Code,
                        Name = a.Name,
                        Description=a.Description,
                        StartDate= (DateTime)a.StartDate,
                        EndDate= (DateTime)a.EndDate

                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
