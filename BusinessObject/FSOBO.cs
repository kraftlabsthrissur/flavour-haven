using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class FSOBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int BusinessCategoryID { get; set; }
        public string BusinessCategoryName { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
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
        public bool IsActive { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int RegionalSalesManagerID { get; set; }
        public bool IsRegionalSalesManager { get; set; }
        public bool IsSalesManager { get; set; }
        public string SalesManager { get; set; }
        public string RegionalSalesManager { get; set; }
        public int SalesManagerID { get; set; }
        public int ReportingToID { get; set; }
        public string ReportingToName { get; set; }
    }

    public class FSOIncentiveItemBO
    {
        public int CustomerID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public DateTime StartDate { get; set; }

        public string CustomerName{ get; set; }
        public string FSOName { get; set; }
        public string CustomerCategory { get; set; }
        public string SalesIncentiveCategory { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string CustomerCode { get; set; }
    }


}
