using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace PresentationContractLayer
{
    public interface IGatePassContract
    {
        bool SaveGatePass(GatePassBO gatePass,List<GatePassItemsBO> ItemList);
        //bool SaveDeliveryDate(List<SalesInvoiceBO> ItemList);
        bool SaveDeliveryDate(List<GatePassItemsBO> GatePassItems);
        List<GatePassBO> GetGatePassList();
        List<GatePassBO> GetGatePassDetails(int ID);
        List<GatePassItemsBO> GetGatePassTransDetails(int ID);
        List<GatePassItemsBO> getGatePassItems(DateTime FromDate, DateTime ToDate,String Type);
        DatatableResultBO GetGatePassListForDataTable(string Type, string TransNo, string TransDate, string VehicleNo, string SortField, string SortOrder, int Offset, int Limit);
    }
}
