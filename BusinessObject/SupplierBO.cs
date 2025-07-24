using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SupplierBO
    {


        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<int> SupplierCategoryID { get; set; }
        public string Location { get; set; }
        public int StateID { get; set; }
        public string GstNo { get; set; }
        public Nullable<bool> IsGSTRegistered { get; set; }
        public string ItemCategory { get; set; }
        public int PaymentDays { get; set; }

        public string SupplierCategoryName { get; set; }
        public string SupplierAccountCategoory { get; set; }
        public int SupplierAccountsCategoryID { get; set; }
        public string SupplierAccountsCategoryName { get; set; }
        public string SupplierTaxCategoryName { get; set; }
        public int SupplierTaxCategoryID { get; set; }
        public string SupplierTaxSubCategoryName { get; set; }
        public int SupplierTaxSubCategoryID { get; set; }
        public string PaymentGroupName { get; set; }
        public int PaymentGroupID { get; set; }
        public string PaymentMethodName { get; set; }
        public int PaymentMethodID { get; set; }
        public string StateName { get; set; }
        public int CreditDaysID { get; set; }
        public int CreditDays { get; set; }
        public string AdhaarCardNo { get; set; }
        public string PanCardNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeActivatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Currency { get; set; }

        public int CurrencyID { get; set; }
        public string CreditDaysName { get; set; }

        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public string SupplierItemCategoryName { get; set; }
        public int SupplierItemCategoryID { get; set; }


        public bool IsBlockForPurcahse { get; set; }
        public bool IsBlockForPayment { get; set; }
        public bool IsBlockForReceipt { get; set; }
        public bool IsDeactivated { get; set; }

        public DateTime DeactivatedDate { get; set; }
        public AddressBO SupplierAddress { get; set; }
        public string UanNo { get; set; }
        public string OldCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AcNo { get; set; }
        public string IfscNo { get; set; }
        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsEmployee { get; set; }
        public string TradeLegalName { get; set; }
        public bool IsActiveSupplier { get; set; }
        public List<RelatedSupplierBO> RelatedSupplierBO { get; set; }
    }

    public class RelatedSupplierBO
    {
        public int RelatedSupplierID { get; set; }
        public string RelatedSupplierLocation { get; set; }
        public string RelatedSupplierName { get; set; }
    }

    public class SupplierDescriptionBO
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
