using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class ServiceRecieptNoteBL : IServiceRecieptNote
    {
        public readonly ServiceRecieptNoteRepository _ISRN;

        public ServiceRecieptNoteBL()
        {
            _ISRN = new ServiceRecieptNoteRepository();
        }

        public List<ServiceRecieptNoteBO> GetAllSRN(int? iD)
        {
            return _ISRN.GetAllSRN(iD);
        }

        public List<SRNTransBO> GetAllSRNTrans(int? sRNID)
        {
            return _ISRN.GetAllSRNTrans(sRNID);
        }

        public List<PurchaseOrderBO> GetUnProcessedPurchaseOrderServiceForSRN(int? supplierID)
        {
            return _ISRN.GetUnProcessedPurchaseOrderServiceForSRN(supplierID);
        }

        public List<SRNTransBO> GetUnProcessedPurchaseOrderServiceTransForSRN(int? iD)
        {
            return _ISRN.GetUnProcessedPurchaseOrderServiceTransForSRN(iD);
        }

        public bool SaveSRN(ServiceRecieptNoteBO _masterSRN, List<SRNTransBO> _SRNtrans)
        {
            if (_masterSRN.ID > 0)
            {
                return _ISRN.UpdateSRN(_masterSRN, _SRNtrans);
            }
            else
            {
                return _ISRN.SaveSRN(_masterSRN, _SRNtrans);
            }
        }

        public int Cancel(int ID)
        {
            return _ISRN.Cancel(ID);
        }

        public int GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            return _ISRN.GetInvoiceNumberCount(Hint,Table,SupplierID);
        }

        public DatatableResultBO GetSRNList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string DeliveryChallanNoHint, string DeliveryChallanDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return _ISRN.GetSRNList(Type, TransNoHint, TransDateHint, SupplierNameHint, DeliveryChallanNoHint, DeliveryChallanDateHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
