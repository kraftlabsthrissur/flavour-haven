using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;


namespace PresentationContractLayer
{
    public interface ISerialNo
    {

        int CreateSerialNumber(SerialNumberBO serialNumber);
        DatatableResultBO GetSerialNumberList(string FormHint, string PrefixHint, string LocationPrefixHint, string FinYearPrefixHint, string SortField, string SortOrder, int Offset, int Limit);
        bool UpdateFinYearAndFinPrefix(List<SerialNumberBO> serialBO);
        DatatableResultBO GetSerialNumberByFinYear(int NewFinYear, string FormHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit);
    }


    #region Enums
    public enum SerialEnum
    {
        PurchaseInvoice
    }

    #endregion
}
