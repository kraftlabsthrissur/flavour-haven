using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System.Collections.Generic;

namespace BusinessLayer
{
   public class JobWorkIssueBL : IJobWorkIssueContrtact
    {
        JobWorkIssueDAL jobWorkIssueDAL;

        public JobWorkIssueBL()
        {
            jobWorkIssueDAL = new JobWorkIssueDAL();
        }
        public int Save(JobWorkIssueBO jobworkissueBO, List<JobWorkIssueItemBO> jobWorkIssueItemBO)
        {
            if (jobworkissueBO.ID == 0)
            {
                return jobWorkIssueDAL.Save(jobworkissueBO, jobWorkIssueItemBO);
            }
            else
            {
                return jobWorkIssueDAL.Update(jobworkissueBO, jobWorkIssueItemBO);
            }
               
        }
        public List<JobWorkIssueBO> GetJobWorkIssue()
        {
            return jobWorkIssueDAL.GetJobWorkIssue();
        }
        public JobWorkIssueBO GetJobWorkIssue(int JobWorkIssueID)
        {
            return jobWorkIssueDAL.GetJobWorkIssue(JobWorkIssueID);
        }

        public List<JobWorkIssueItemBO> GetJobWorkIssueItems(int JobWorkIssueID)
        {
            return jobWorkIssueDAL.GetJobWorkIssueItems(JobWorkIssueID);
        }
      

        public DatatableResultBO GetIssueList(int SupplierID, string IssueNoHint, string SupplierHint,  string IssueDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return jobWorkIssueDAL.GetIssueList(SupplierID, IssueNoHint, SupplierHint, IssueDateHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
