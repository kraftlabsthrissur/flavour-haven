using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IQCProductionContract
    {
        List<QCItemBO> GetQCList(string Status, int Offset, int Limit);
        List<QCItemBO> GetQCItemDetails(int ID);
        List<QCTestBO> GetQCTestDetails(int ID, string Type);
        bool UpdateQC(QCItemBO QCItem,List<QCTestBO> QCTestResults);
        DatatableResultBO GetQCList(string Type, string ProductionNoHint, string ReceiptDateHint, string ItemHint, string BatchNoHint, string UnitHint,string AcceptedQuantityHint,string ApprovedQuantityHint,string BatchsizeHint,string SortField,string SortOrder, int Offset,int Limit);


    }
}
