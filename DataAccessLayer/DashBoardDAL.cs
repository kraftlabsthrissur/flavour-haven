using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DashBoardDAL
    {
        public List<SalesSummaryBO> GetMonthWiseSalesSummary()
        {
            List<SalesSummaryBO> SalesDataList = new List<SalesSummaryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalesDataList = dbEntity.SpGetActualAndBudgetSalesDetailsForDashBoard(GeneralBO.FinYear,GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new SalesSummaryBO
                    {
                       BudgetValue =(decimal)k.BudgetQtyInNos,
                       ActualValue=(decimal)k.ActualQtyInNos,
                       BudgetGrossRevenue=(decimal)k.BudgetGrossRevenue,
                       ActualSalesRevenue=(decimal)k.ActualGrossRevenue,
                       BudgetInKgValue=(decimal)k.BudgetQtyInKgs,
                       ActualSalesInKgValue=(decimal)k.ActualQtyInKgs,
                       XaxisName=k.LocationHead
                    }).ToList();
                }
                return SalesDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesSummaryBO> GetCategoryWiseSalesWithLocationHead()
        {
            List<SalesSummaryBO> SalesDataList = new List<SalesSummaryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalesDataList = dbEntity.SpGetActualAndBudgetSalesDetailsByCategoryWise(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new SalesSummaryBO
                    {
                        BudgetValue = (decimal)k.BudgetQtyInNos,
                        ActualValue = (decimal)k.ActualQtyInNos,
                        BudgetGrossRevenue = (decimal)k.BudgetGrossRevenue,
                        ActualSalesRevenue = (decimal)k.ActualGrossRevenue,
                        BudgetInKgValue = (decimal)k.BudgetQtyInKgs,
                        ActualSalesInKgValue = (decimal)k.ActualQtyInKgs,
                        LocationHead = k.LocationHead,
                        Category = k.Category
                    }).ToList();
                }
                return SalesDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DateWiseSalesBO> GetSalesAmtByDayWise()
        {
            List<DateWiseSalesBO> DateWiseSales = new List<DateWiseSalesBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    DateWiseSales = dbEntity.SpGetSalesAmtByDayWise(GeneralBO.FinYear, GeneralBO.ApplicationID).Select(k => new DateWiseSalesBO
                    {
                        Value = (decimal)k.GrossAmt,
                        Date = (DateTime)k.InvoiceDate
                    }).ToList();
                }
                return DateWiseSales;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesSummaryBO> GetActualAndBudgetSalesDetailsForYear()
        {
            List<SalesSummaryBO> SalesDataList = new List<SalesSummaryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalesDataList = dbEntity.SpGetActualAndBudgetSalesDetailsForYear(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new SalesSummaryBO
                    {
                        BudgetValue = (decimal)k.BudgetQtyInNos,
                        ActualValue = (decimal)k.ActualQtyInNos,
                        BudgetGrossRevenue = (decimal)k.BudgetGrossRevenue,
                        ActualSalesRevenue = (decimal)k.ActualGrossRevenue,
                        BudgetInKgValue = (decimal)k.BudgetQtyInKgs,
                        ActualSalesInKgValue = (decimal)k.ActualQtyInKgs,
                        XaxisName = k.LocationHead
                    }).ToList();
                }
                return SalesDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CategoryWiseStockBO> GetBusinessCategoryWiseStock()
        {
            List<CategoryWiseStockBO> SalesDataList = new List<CategoryWiseStockBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalesDataList = dbEntity.SpGetStockByBusinessCategoryWise(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new CategoryWiseStockBO
                    {
                        LocationHead = k.LocationHead,
                        Stock = (decimal)k.Stock,
                        BusinessCategory = k.BusinessCategory
                    }).ToList();
                }
                return SalesDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductionQtyBO> GetProductionOutputInMonth()
        {
            List<ProductionQtyBO> ProductionDataList = new List<ProductionQtyBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ProductionDataList = dbEntity.SpGetProductionOutputInMonth(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ProductionQtyBO
                    {
                        TotalProduction = (decimal)k.TotalProduction,
                        LocationHead = k.LocationHead
                    }).ToList();
                }
                return ProductionDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductionQtyBO> GetTotalProductionInYear()
        {
            List<ProductionQtyBO> ProductionDataList = new List<ProductionQtyBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ProductionDataList = dbEntity.SpGetProductionOutputInYear(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ProductionQtyBO
                    {
                        TotalProduction = (decimal)k.TotalProduction,
                        LocationHead = k.LocationHead
                    }).ToList();
                }
                return ProductionDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesSummaryBO> GetMonthWiseSalesByLocationHead(string LocationHead,string Batchtype)
        {
            List<SalesSummaryBO> SalesDataList = new List<SalesSummaryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalesDataList = dbEntity.SpGetMonthWiseSalesDetailsByLocationHead(LocationHead, Batchtype,GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new SalesSummaryBO
                    {
                        BudgetValue = (decimal)k.BudgetQtyInNos,
                        ActualValue = (decimal)k.ActualQtyInNos,
                        BudgetGrossRevenue = (decimal)k.BudgetGrossRevenue,
                        ActualSalesRevenue = (decimal)k.ActualGrossRevenue,
                        BudgetInKgValue = (decimal)k.BudgetQtyInKgs,
                        ActualSalesInKgValue = (decimal)k.ActualQtyInKgs,
                        PrevoiusYearGrossRevenue=(decimal)k.PrevoiusYearGrossRevenue,
                        PrevoiusYearSalesInKgValue=(decimal)k.PrevoiusYearQtyInKgs,
                        PrevoiusYearSalesValue = (decimal)k.PrevoiusYearQtyInNos,
                        XaxisName = k.Month,
                        Year = (int)k.FinYear,
                       


                    }).ToList();
                }
                return SalesDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductionQtyMonthWiseBO> GetProductionOutputMonthWise()
        {
            List<ProductionQtyMonthWiseBO> ProductionDataList = new List<ProductionQtyMonthWiseBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ProductionDataList = dbEntity.SpGetProductionOutputMonthWise(GeneralBO.FinYear, GeneralBO.ApplicationID).Select(k => new ProductionQtyMonthWiseBO
                    {
                        TotalProductionInTKY = (decimal)k.TotalProductionInTKY,
                        TotalProductionInPOL = (decimal)k.TotalProductionInPOL,
                        TotalProductionInCHU = (decimal)k.TotalProductionInCHU,
                        Month = k.Month,
                        Year = (int)k.YearValue,
                        MonthNumber = (int)k.MonthValue
                    }).ToList();
                }
                return ProductionDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesSummaryBO> GetLocationWiseSales(string Type,string Batchtype)
        {
            List<SalesSummaryBO> SalesDataList = new List<SalesSummaryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalesDataList = dbEntity.SpGetLocationWiseSales(Type, Batchtype,GeneralBO.FinYear, GeneralBO.ApplicationID).Select(k => new SalesSummaryBO
                    {
                     Location=k.Location,
                     ActualSalesInKgValue=(decimal)k.ActualQtyInKgs,
                     BudgetInKgValue=(decimal)k.BudgetQtyInKgs,
                     BudgetGrossRevenue=(decimal)k.BudgetGrossRevenue,
                     ActualSalesRevenue=(decimal)k.ActualGrossRevenue
                    }).ToList();
                }
                return SalesDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesSummaryBO> GetCategoryWiseSales(string Type,string Batchtype)
        {
            List<SalesSummaryBO> SalesDataList = new List<SalesSummaryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalesDataList = dbEntity.SpGetCategoryWiseSales(Type, Batchtype, GeneralBO.FinYear, GeneralBO.ApplicationID).Select(k => new SalesSummaryBO
                    {
                        Category = k.Category,
                        ActualSalesInKgValue = (decimal)k.ActualQtyInKgs,
                        BudgetInKgValue = (decimal)k.BudgetQtyInKgs,
                        BudgetGrossRevenue = (decimal)k.BudgetGrossRevenue,
                        ActualSalesRevenue = (decimal)k.ActualGrossRevenue
                    }).ToList();
                }
                return SalesDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductionProgressBO> GetProductionQtyInProgress(string Type,int SalesCategoryID)
        {
            List<ProductionProgressBO> ProductionDataList = new List<ProductionProgressBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ProductionDataList = dbEntity.SpGetProductionProgressDetails(Type, SalesCategoryID, GeneralBO.FinYear, GeneralBO.ApplicationID).Select(k => new ProductionProgressBO
                    {
                        ItemName = k.ProductionGroupName,
                        StandardOutput = (decimal)k.StandardOutput,
                        ExpectedDate = (DateTime)k.ExpectedEndDate,
                        BatchNo = k.BatchNo
                    }).ToList();
                }
                return ProductionDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesByLocationBO> GetSalesByLocation(DateTime Date, int LocationID = 0)
        {
            try
            {
                List<SalesByLocationBO> sales = new List<SalesByLocationBO>();
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return dbEntity.SpGetSalesByLocation(Date, GeneralBO.CreatedUserID, LocationID, GeneralBO.ApplicationID).Select(a => new SalesByLocationBO
                    {
                        LocationCode = a.LocationCode,
                        LocationType = a.LocationType,
                        PreviousMonthAmount = (decimal)a.PreviousMonthAmount,
                        LocationName = a.LocationName,
                        Amount = (decimal)a.Amount,
                        Budget = (decimal)a.Budget
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
