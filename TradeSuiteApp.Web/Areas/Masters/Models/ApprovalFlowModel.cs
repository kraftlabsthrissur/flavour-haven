using System.Collections.Generic;
using System;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models

{
    public class ApprovalFlowModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LocationCode { get; set; }
        public int UserLocationID { get; set; }
        public string LocationName { get; set; }
        public SelectList LocationList { get; set; }
        public string ApprovalType { get; set; }
        public string ApprovalTypeName { get; set; }
        public SelectList ApprovalList { get; set; }
        public int ForDepartmentID { get; set; }
        public string ForDepartmentName { get; set; }
        public SelectList DeptList { get; set; }
        public int ForUserID { get; set; }
        public string ForUserName { get; set; }
        public decimal AmountAbove { get; set; }
        public decimal AmountBelow { get; set; }
        public int ItemCategoryID { get; set; }
        public string ItemCategoryName { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public int ItemAccountsCategoryID { get; set; }
        public string ItemAccountsCategoryName { get; set; }
        public SelectList ItemAccountsCategoryList { get; set; }
        public SelectList SuppliercategoryList { get; set; }
        public SelectList SupplierAccountscategoryList { get; set; }
        public int SupplierCategoryID { get; set; }
        public string SuppliercategoryName { get; set; }
        public int SupplierAccountsCategoryID { get; set; }
        public string SupplierAccountscategoryName { get; set; }
        public int ApprovalQueueID { get; set; }
        public string AppQueueName { get; set; }


    }
}