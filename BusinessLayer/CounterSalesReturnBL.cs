using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CounterSalesReturnBL : ICounterSalesReturn
    {
        CounterSalesReturnDAL counterSalesReturnDAL;
        public CounterSalesReturnBL()
        {
            counterSalesReturnDAL = new CounterSalesReturnDAL();
        }

        public List<CounterSalesReturnBO> GetSalesReturnList()
        {
            return counterSalesReturnDAL.GetSalesReturnList();
        }
        public List<CounterSalesReturnBO> GetCounterSalesReturn(int ReturnID)
        {
            return counterSalesReturnDAL.GetCounterSalesReturn(ReturnID);
        }
        public List<CounterSalesReturnItemBO> GetCounterSalesReturnTrans(int ID)
        {
            return counterSalesReturnDAL.GetCounterSalesReturnTrans(ID);

        }
        public DatatableResultBO GetCounterSalesReturnListForDataTable(string Type, string ReturnNo, string ReturnDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return counterSalesReturnDAL.GetCounterSalesReturnListForDataTable(Type, ReturnNo, ReturnDate,SortField, SortOrder, Offset, Limit);
        }

        public bool SaveCounterSalesReturn(CounterSalesReturnBO counterSalesBO, List<CounterSalesReturnItemBO> Items)
        {
            string XMLItems = "<counterSalesTrans>";

            foreach (var items in Items)
            {
                XMLItems += "<Items>";
                XMLItems += "<CounterSalesTransID>" + items.CounterSalesTransID + "</CounterSalesTransID>";
                XMLItems += "<FullOrLoose>" + items.FullOrLoose + "</FullOrLoose>";
                XMLItems += "<ItemID>" + items.ItemID + "</ItemID>";
                XMLItems += "<BatchID>" + items.BatchID + "</BatchID>";
                XMLItems += "<Quantity>" + items.Quantity + "</Quantity>";
                XMLItems += "<ReturnQty>" + items.ReturnQty + "</ReturnQty>";
                XMLItems += "<Rate>" + items.Rate + "</Rate>";
                XMLItems += "<MRP>" + items.MRP + "</MRP>";
                XMLItems += "<GrossAmount>" + items.GrossAmount + "</GrossAmount>";
                XMLItems += "<DiscountPercentage>" + items.DiscountPercentage + "</DiscountPercentage>";
                XMLItems += "<DiscountAmount>" + items.DiscountAmount + "</DiscountAmount>";
                XMLItems += "<TaxableAmount>" + items.TaxableAmount + "</TaxableAmount>";
                XMLItems += "<SGSTPercentage>" + items.SGSTPercentage + "</SGSTPercentage>";
                XMLItems += "<CGSTPercentage>" + items.CGSTPercentage + "</CGSTPercentage>";
                XMLItems += "<IGSTPercentage>" + items.IGSTPercentage + "</IGSTPercentage>";
                XMLItems += "<SGSTAmount>" + items.SGSTAmount + "</SGSTAmount>";
                XMLItems += "<CGSTAmount>" + items.CGSTAmount + "</CGSTAmount>";
                XMLItems += "<IGSTAmount>" + items.IGSTAmount + "</IGSTAmount>";
                XMLItems += "<NetAmount>" + items.NetAmount + "</NetAmount>";
                XMLItems += "<BatchTypeID>" + items.BatchTypeID + "</BatchTypeID>";
                XMLItems += "<WareHouseID>" + items.WareHouseID + "</WareHouseID>";
                XMLItems += "<UnitID>" + items.UnitID + "</UnitID>";
                XMLItems += "<CessPercentage>" + items.CessPercentage + "</CessPercentage>";
                XMLItems += "<CessAmount>" + items.CessAmount + "</CessAmount>";
                XMLItems += "<SecondaryUnit>" + items.SecondaryUnit + "</SecondaryUnit>";
                XMLItems += "<SecondaryReturnQty>" + items.SecondaryReturnQty + "</SecondaryReturnQty>";
                XMLItems += "<SecondaryUnitSize>" + items.SecondaryUnitSize + "</SecondaryUnitSize>";
                XMLItems += "<SecondaryRate>" + items.SecondaryRate + "</SecondaryRate>";
                XMLItems += "<VATPercentage>" + items.VATPercentage + "</VATPercentage>";
                XMLItems += "<VATAmount>" + items.VATAmount + "</VATAmount>";
                XMLItems += "</Items>";
            }
            XMLItems += "</counterSalesTrans>";

            if (counterSalesBO.ID > 0) 
            {
                return counterSalesReturnDAL.UpdateCounterSalesReturn(counterSalesBO, XMLItems);
            }
            else
            {
                return counterSalesReturnDAL.SaveCounterSalesReturn(counterSalesBO, XMLItems);
            }

        }
    }
}
