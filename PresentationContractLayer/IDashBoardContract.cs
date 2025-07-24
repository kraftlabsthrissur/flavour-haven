using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IDashBoardContract
    {
        List<SalesSummaryBO> GetMonthWiseSalesSummary();

        List<SalesSummaryBO> GetCategoryWiseSalesWithLocationHead();

        List<DateWiseSalesBO> GetSalesAmtByDayWise();

        List<SalesSummaryBO> GetActualAndBudgetSalesDetailsForYear();

        List<CategoryWiseStockBO> GetBusinessCategoryWiseStock();

        List<ProductionQtyBO> GetTotalProductionInYear();

        List<ProductionQtyBO> GetProductionOutputInMonth();

        List<SalesSummaryBO> GetMonthWiseSalesByLocationHead(string LocationHead,string Batchtype);

        List<ProductionQtyMonthWiseBO> GetProductionOutputMonthWise();

        List<SalesSummaryBO> GetLocationWiseSales(string Type, string Batchtype);

        List<SalesSummaryBO> GetCategoryWiseSales(string Type,string Batchtype);

        List<ProductionProgressBO> GetProductionQtyInProgress(string Type, int SalesCategoryID);

        List<SalesByLocationBO> GetSalesByLocation(DateTime Date, int LocationID);
    }
}
