using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IQualityCheckContract
    {
        List<QCItemBO> GetQCList(string Status, int Offset, int Limit);

        List<QCItemBO> GetQCItemDetailsByID(int ID);

        List<QCTestBO> GetQCTestDetailsByID(int ID, string Type);

        bool UpdateQC(QCItemBO QCItem, List<QCTestBO> QCTestResults);

        DatatableResultBO GetQualityCheckList(string Type, string TransNoHint, string TransDateHint, string GRNNoHint, string ReceiptDateHint, string ItemNameHint, string UnitNameHint, string SupplierNameHint, string DeliveryChallanNoHint, string SortField, string SortOrder, int Offset, int Limit);

    }
}
