using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IServiceRecieptNote
    {
        bool SaveSRN(ServiceRecieptNoteBO _masterSRN, List<SRNTransBO> _SRNtrans);

        List<PurchaseOrderBO> GetUnProcessedPurchaseOrderServiceForSRN(Nullable<int> supplierID);

        List<SRNTransBO> GetUnProcessedPurchaseOrderServiceTransForSRN(Nullable<int> iD);

        List<ServiceRecieptNoteBO> GetAllSRN(Nullable<int> iD);

        List<SRNTransBO> GetAllSRNTrans(Nullable<int> sRNID);

        int Cancel(int ID);

        int GetInvoiceNumberCount(string Hint, string Table, int SupplierID);

        DatatableResultBO GetSRNList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string DeliveryChallanNoHint, string DeliveryChallanDateHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
