using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class DashBoardBL : IDashBoardContract
    {
        DashBoardDAL dashboradDAL;
        public DashBoardBL()
        {
            dashboradDAL = new DashBoardDAL();
        }

        public List<SalesSummaryBO> GetMonthWiseSalesSummary()
        {
            return dashboradDAL.GetMonthWiseSalesSummary();
        }

        public List<SalesSummaryBO> GetCategoryWiseSalesWithLocationHead()
        {
            return dashboradDAL.GetCategoryWiseSalesWithLocationHead();
        }

        public List<DateWiseSalesBO> GetSalesAmtByDayWise()
        {
            return dashboradDAL.GetSalesAmtByDayWise();
        }

        public List<SalesSummaryBO> GetActualAndBudgetSalesDetailsForYear()
        {
            return dashboradDAL.GetActualAndBudgetSalesDetailsForYear();
        }

        public List<CategoryWiseStockBO> GetBusinessCategoryWiseStock()
        {
            return dashboradDAL.GetBusinessCategoryWiseStock();
        }

        public List<ProductionQtyBO> GetTotalProductionInYear()
        {
            return dashboradDAL.GetTotalProductionInYear();
        }

        public List<ProductionQtyBO> GetProductionOutputInMonth()
        {
            return dashboradDAL.GetProductionOutputInMonth();
        }

        public List<SalesSummaryBO> GetMonthWiseSalesByLocationHead(string LocationHead,string Batchtype)
        {
            return dashboradDAL.GetMonthWiseSalesByLocationHead(LocationHead, Batchtype);
        }

        public List<ProductionQtyMonthWiseBO> GetProductionOutputMonthWise()
        {
            return dashboradDAL.GetProductionOutputMonthWise();
        }

        public List<SalesSummaryBO> GetLocationWiseSales(string Type,string Batchtype)
        {
            return dashboradDAL.GetLocationWiseSales(Type, Batchtype);
        }

        public List<SalesSummaryBO> GetCategoryWiseSales(string Type, string Batchtype)
        {
            return dashboradDAL.GetCategoryWiseSales(Type, Batchtype);
        }

        public List<ProductionProgressBO> GetProductionQtyInProgress(string Type, int SalesCategoryID)
        {
            return dashboradDAL.GetProductionQtyInProgress(Type, SalesCategoryID);
        }

        public List<SalesByLocationBO> GetSalesByLocation(DateTime Date, int LocationID = 0)
        {
            return dashboradDAL.GetSalesByLocation(Date, LocationID);
        }

        //public DatatableResultBO GetProductionProgressDetails(string Type, int CategoryID, string BatchHint, string ItemHint, string ExpectedEndDateHint, string SortField, string SortOrder, int Offset, int Limit)
        //{
        //    return dashboradDAL.GetProductionProgressDetails(Type, CategoryID, BatchHint, ItemHint, ExpectedEndDateHint, SortField, SortOrder, Offset, Limit);
        //}
    }
}
