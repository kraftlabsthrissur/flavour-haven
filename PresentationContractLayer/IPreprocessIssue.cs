using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IPreprocessIssue
    {
        int Save(PreprocessIssueBO preprocessIssueBO);
        PreprocessIssueBO GetPreProcessIssue(int id);
        List<PreprocessIssueBO> GetPreProcessList();
        List<PreprocessIssueBO> GetProcessList();

        DatatableResultBO GetPreProcessIssueList(string Type,string TransNoHint,string TransDateHint,string CreatedUserHint,string ItemNameHint,string SortField,string SortOrder,int Offset,int Limit);
    }

}
