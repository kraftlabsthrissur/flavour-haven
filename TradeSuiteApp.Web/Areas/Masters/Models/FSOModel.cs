using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class FSOModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int BusinessCategoryID { get; set; }
        public string BusinessCategoryName { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public int SalesIncentiveMappingCategoryID { get; set; }
        public string SalesIncentiveCategoryName { get; set; }
        public int SalesCategoryID { get; set; }
        public string SalesCategoryName { get; set; }
        public string FSOName { get; set; }
        public string FSOCode { get; set; }
        public int EmployeeID { get; set; }
        public string RouteName { get; set; }
        public string RouteCode { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string AreaManager { get; set; }
        public int ZonalManagerID { get; set; }
        public int AreaManagerID { get; set; }
        public string ZonalManager { get; set; }
        public bool IsZonalManager { get; set; }
        public bool IsAreaManager { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AreaManagerStatus { get; set; }
        public string ZonalManagerStatus { get; set; }
        public string IsActiveStatus { get; set; }
        public bool IsActive { get; set; }
        public int RegionalSalesManagerID { get; set; }
        public bool IsRegionalSalesManager { get; set; }
        public bool IsSalesManager { get; set; }
        public string SalesManager { get; set; }
        public string RegionalSalesManager { get; set; }
        public int SalesManagerID { get; set; }
        public int StateID { get; set; }
        public int FSOID { get; set; }
        public int CustomerCategoryID { get; set; }
        public string SalesManagerStatus { get; set; }
        public string RegionalSalesManagerStatus { get; set; }
        public int ReportingToID { get; set; }
        public string ReportingToName { get; set; }
        public List<FSOIncentiveItemModel> Items { get; set; }

        public SelectList SalesCategoryList { get; set; }
        public SelectList BusinessCategoryList { get; set; }
        public SelectList SalesIncentiveCategoryList { get; set; }
        public SelectList SalesManagerList { get; set; }
        public SelectList RegionalSalesManagerList { get; set; }
        public SelectList ZonalManagerList { get; set; }
        public SelectList AreaManagerList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
    }
    public class FSOIncentiveItemModel
    {
        public int CustomerID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public string StartDate { get; set; }
    }

}