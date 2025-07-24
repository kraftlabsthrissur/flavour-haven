using BusinessObject;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public  interface IJobWorkIssueContrtact
    {
        int Save(JobWorkIssueBO jobworkissueBO, List<JobWorkIssueItemBO> jobWorkIssueItemBO);

        List<JobWorkIssueBO> GetJobWorkIssue();

        JobWorkIssueBO GetJobWorkIssue(int JobWorkIssueID);

        List<JobWorkIssueItemBO> GetJobWorkIssueItems(int JobWorkIssueID);

        DatatableResultBO GetIssueList(int SupplierID, string IssueNoHint,string SupplierHint, string IssueDateHint, string SortField, string SortOrder, int Offset, int Limit);
      
    }

}
