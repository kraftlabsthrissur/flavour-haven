using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;


namespace PresentationContractLayer
{
    public interface ICounterSalesContract
    {
        int SaveCounterSalesInvoice(CounterSalesBO counterSalesBO, List<CounterSalesItemsBO> Items, List<CounterSalesAmountDetailsBO> AmountDetails);



        List<CounterSalesBO> GetCounterSalesList(Int32 ID);

        CounterSalesBO GetIsCounterSalesAlreadyExists(string PartyName, decimal NetAmount);

        List<CounterSalesBO> GetCounterSalesDetail(int ID);

        List<CounterSalesItemsBO> GetCounterSalesListDetails(Int32 ID);

        List<CounterSalesAmountDetailsBO> GetCounterSalesListAmount(int ID);

        int Cancel(int CounterSalesID);

        DatatableResultBO GetCounterSalesForReturn(int PartyID, string TransHint, string SortField, string SortOrder, int Offset, int Limit);

        List<CounterSalesItemsBO> GetBatchwiseItemForCounterSales(int ItemID, int WarehouseID, int BatchTypeID, int UnitID, decimal Qty, string CustomerType, int TaxTypeID);

        List<CounterSalesItemsBO> GetGoodsReciptItemForCounterSales(int[] counterSalesIDs);

        List<CounterSalesItemsBO> GetCounterSalesTransForCounterSalesReturn(int InvoiceID, int PriceListID);

        List<CounterSalesBO> GetCounterSalesType();

        string GetPrintTextFile(int CounterSalesID);

        DatatableResultBO GetCustomerCounterSalesList(int CustomerID,string TransNoHint, string TransDateHint, string PartyNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetListForCounterSales(string Type, string TransNoHint, string TransDateHint, string SalesTypeHint, string PartyNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetAppointmentProcessList(string TransNoHint, string TransDateHint, string PatientNameHint, string DoctorNameHint, string PhoneHint, string SortField, string SortOrder, int Offset, int Limit);

        List<SalesItemBO> GetBatchwisePrescriptionItems(int AppointmentProcessID, int PatientID);

        bool IsCancelable(int counterSalesID);

        List<CounterSalesBO> GetCounterSalesSignOutPrint(string Type);

        bool IsDotMatrixPrint();

        bool IsThermalPrint();
        CurrencyClassBO GetCurrencyDecimalClassByCurrencyID(int CurrencyID);
    }


}
