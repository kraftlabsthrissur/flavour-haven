using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SLAModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ItemAccountsCategory { get; set; }
        public string ItemTaxCategory { get; set; }
        public string CustomerAccountsCategory { get; set; }
        public string CustomerCategory { get; set; }
        public string CustomerTaxCategory { get; set; }
        public string SupplierTaxSubCategory { get; set; }
        public string SupplierAccountsCategory { get; set; }
        public string CustomerID { get; set; }
        public string Supplier { get; set; }
        public string SupplierTaxCategory { get; set; }
        public string Cycle { get; set; }
        public string TransactionType { get; set; }
        public string KeyValue { get; set; }
        public string Guidance { get; set; }
        public string BatchPrefix { get; set; }
        public string Customer { get; set; }
        public string CostComponent { get; set; }
        public string DepartmentCategory { get; set; }
        public string Capitilization { get; set; }
        public string Location { get; set; }
        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
        public string DebitAccount { get; set; }
        public int DebitAccountID { get; set; }
        public string CreditAccount { get; set; }
        public int CreditAccountID { get; set; }
        public string DebitAccountDescription { get; set; }
        public string CreditAccountDescription { get; set; }
        public string EntryInLocation { get; set; }
        public string EntryInDepartment { get; set; }
        public string EntryInEmployee { get; set; }
        public string EntryInInterCompanyField { get; set; }
        public string EntryInProjectField { get; set; }
        public string ItemSubLedger { get; set; }
        public string SupplierSubLedger { get; set; }
        public string CustomerSubLedger { get; set; }
        public string EmployeeSubLedger { get; set; }
        public string AssetsSubLedger { get; set; }
        public string PatientsSubLedger { get; set; }
        public string BankCashSubLedger { get; set; }
        public string Remarks { get; set; }
        public string Item { get; set; }
        public string ItemID { get; set; }
        public string SupplierID { get; set; }



        public string ItemName { get; set; }
        public string ItemAccountsCategoryName { get; set; }
        public string ItemTaxCategoryName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAccountsCategoryName { get; set; }
        public string SupplierTaxSubCategoryName { get; set; }
        public string CustomerCategoryName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAccountsCategoryName { get; set; }

        public SelectList ItemAccountCategoryList { get; set; }
        public SelectList GSTCategoryList { get; set; }
        public SelectList CustomerAccountsCategoryList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public SelectList CustomerTaxCategoryList { get; set; }
        public SelectList SuppliersAccountCategoryList { get; set; }
        public SelectList SelectSupplier { get; set; }
        public SelectList ProcessCycleList { get; set; }
        public SelectList SLAKeyValueList { get; set; }
        public SelectList SuppliersTaxSubCategoryList { get; set; }
        public SelectList TransactionTypeList { get; set; }
        public SelectList SLAKeyValueLists { get; set; }
        public SelectList ItemList { get; set; }
        public SelectList SupplierList { get; set; }
        public SelectList CustomerList { get; set; }

        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime enddate { get; set; }

    }
}