using System;
using System.Collections.Generic;
using PresentationContractLayer;
using DataAccessLayer;
using BusinessObject;

namespace BusinessLayer
{
    public class GatePassBL : IGatePassContract
    {
        GatePassDAL gateDAL = new GatePassDAL();
        public bool SaveGatePass(GatePassBO gatePass, List<GatePassItemsBO> ListItem)
        {
            if (gatePass.ID == 0)
            {
                return gateDAL.SaveGatePass(gatePass, ListItem);
            }
            else
            {
                return gateDAL.UpdateGatePass(gatePass, ListItem);
            }
        }

        public bool SaveDeliveryDate(List<GatePassItemsBO> GatePassItems)
        {
            return gateDAL.SaveDeliveryDate(GatePassItems);
        }

        public List<GatePassBO> GetGatePassList()
        {
            return gateDAL.GetGatePassList();
        }
        public List<GatePassBO> GetGatePassDetails(int ID)
        {
            return gateDAL.GetGatePassDetails(ID);
        }
        public List<GatePassItemsBO> GetGatePassTransDetails(int ID)
        {
            return gateDAL.GetGatePassTransDetails(ID);
        }
        public List<GatePassItemsBO> getGatePassItems(DateTime FromDate, DateTime ToDate, string Type)
        {
            return gateDAL.getGatePassItems(FromDate, ToDate, Type);
        }
        public DatatableResultBO GetGatePassListForDataTable(string Type, string TransNo, string TransDate, string VehicleNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            return gateDAL.GetGatePassListForDataTable(Type, TransNo, TransDate, VehicleNo, SortField, SortOrder, Offset, Limit);
        }

    }
}
