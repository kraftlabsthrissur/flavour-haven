using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IQcTestDefinitionContract
    {
        List<QCTestBO> GetQCTestDefinitionList();
        List<QCTestBO> GetQCList();
        int Save(List<QCTestItemBO> Items, QCTestBO QCTestBO);
        List<QCTestBO> GetQCDefinitionDetails(int ID);
        List<QCTestItemBO> GetTestForItemList(int ItemID);
        List<QCTestItemBO> GetQCDefinitionTransDetails(int ID);
        bool IsDeletable(int QCTestID, int ID, int ItemID);
        DatatableResultBO GetQcTestDefinitionList(string Code,string ItemName,string TestName,string SortField,string SortOrder,int Offset,int Limit);

    }
}
