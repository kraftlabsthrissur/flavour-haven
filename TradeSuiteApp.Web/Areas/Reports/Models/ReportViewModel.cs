using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BusinessObject;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Reports.Models
{
    public class ReportViewModel
    {
        public string FromDate { get; set; }
        public string TransDateFromFromDate { get; set; }
        public string ToDate { get; set; }
        public string FinStartDate { get; set; }
        public int? ItemCategoryID { get; set; }
        [Required(ErrorMessage = "Please select an item category first")]
        public Nullable<int> ItemID { get; set; }
        public string ItemName { get; set; }
        public string Users { get; set; }
        public int LocationID { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList UsersList { get; set; }
        public string StatusID { get; set; }
        public SelectList StatusList { get; set; }
        public string Summary { get; set; }
        public string StockSummary { get; set; }
        public string Type { get; set; }
        public int? UserID { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string QcType { get; set; }
        public int ItemTypeID { get; set; }
        public string ItemType { get; set; }
        public SelectList ItemTypeList { get; set; }
        public string Status { get; set; }
        public bool IsDraft { get; set; }
        public int PRID { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string QcNoFrom { get; set; }
        public string QcNoTo { get; set; }
        public string GRNNoFrom { get; set; }
        public string GRNNoTo { get; set; }
        public SelectList FromSupplierRangeList { get; set; }
        public SelectList ToSupplierRangeList { get; set; }
        public SelectList FromItemNameRangeList { get; set; }
        public SelectList ToItemNameRangeList { get; set; }
        public SelectList FromLoginNameRangeList { get; set; }
        public SelectList ToLoginNameRangeList { get; set; }
        public SelectList FromItemCategoryRangeList { get; set; }
        public SelectList ToItemCategoryRangeList { get; set; }
        public SelectList ToDepartmentFromRangeList { get; set; }
        public SelectList ToDepartmentToRangeList { get; set; }
        public SelectList UserFromRangeList { get; set; }
        public SelectList UserToRangeList { get; set; }
        // public SelectList ItemNameLanguageRangeList { get; set; }

        public SelectList DDLDepartment { get; set; }
        public string DepartmentTo { get; set; }
        // public string ItemNameLanguageRange { get; set; }

        public string ToDepartmentFromRange { get; set; }
        public string ToDepartmentToRange { get; set; }
        public string FromItemCategoryRange { get; set; }
        public string ToItemCategoryRange { get; set; }
        public string FromSupplierRange { get; set; }
        public string ToSupplierRange { get; set; }
        public string FromItemNameRange { get; set; }
        public string ToItemNameRange { get; set; }
        public string FromUserRange { get; set; }
        public string ToUserRange { get; set; }
        public string PONOFrom { get; set; }
        public int? PONOFromID { get; set; }
        public string PONOTo { get; set; }
        public int? PONOToID { get; set; }
        public string PRNOFrom { get; set; }
        public int? PRNOFromID { get; set; }
        public string PRNOTo { get; set; }
        public int? PRNOToID { get; set; }

        public DateTime? PRDateFrom { get; set; }
        //public string PRDateFrom { get; set; }


        public int PRDateFromID { get; set; }
        //public string PRDateTo { get; set; }
        public DateTime? PRDateTo { get; set; }
        public string PODateFrom { get; set; }
        public string PODateTo { get; set; }
        public string InvoiceDateFrom { get; set; }
        public string InvoiceDateTo { get; set; }
        public int? SupplierID { get; set; }
        public int? QCNOFromID { get; set; }
        public int? QCNOToID { get; set; }
        public int? GRNNOFromID { get; set; }
        public int? GRNNOToID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceNoID { get; set; }
        public string InvoiceNOFrom { get; set; }
        public int? InvoiceNOFromID { get; set; }
        public string InvoiceNOTo { get; set; }
        public int? InvoiceNOToID { get; set; }
        public string ReturnNOFrom { get; set; }
        public int? ReturnNOFromID { get; set; }
        public string ReturnNOTo { get; set; }
        public int? ReturnNOToID { get; set; }
        public int InvoiceStatusID { get; set; }
        public string InvoiceStatus { get; set; }
        public string SupplierInvoiceNO { get; set; }
        public int? SupplierInvoiceNOID { get; set; }
        // public int? GRNFrom{ get; set; }
        public string SRNFrom { get; set; }
        public string SRNTo { get; set; }

        public int? ToDepartmentID { get; set; }

        public string SupplierInvoicesHistory { get; set; }
        public int InvoiceID { get; set; }
        public string DocumentNo { get; set; }

        public string GRNNo { get; set; }
        public string GRNFrom { get; set; }
        public string GRNTo { get; set; }
        public int? SRNNOFromID { get; set; }
        public int? SRNNOToID { get; set; }
        public String GRNFromDate { get; set; }
        public String GRNToDate { get; set; }
        public string FromLoginNameRange { get; set; }
        public String ToLoginNameRange { get; set; }
        public int? purchaseRetunNoFromId { get; set; }
        public int? purchaseRetunNoTOId { get; set; }
        public string Filters { get; set; }
        public bool IsOverruled { get; set; }
    }

    public class AccountReportModel
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RefDocDate { get; set; }
        public string RefInvoiceNo { get; set; }
        public int RefInvoiceNoID { get; set; }
        public int CreditAccountHeadID { get; set; }
        public int? AccountID { get; set; }
        public int AccountDebitID { get; set; }
        public int? AccountCreditID { get; set; }
        public int AccountGroupID { get; set; }
        public int? AccountNameID { get; set; }
        public string AccountName { get; set; }
        public int AccountGroup { get; set; }
        public int? SupplierID { get; set; }
        public int? CustomerCodeFromID { get; set; }
        public int? CustomerCodeToID { get; set; }
        public int? CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int LocationID { get; set; }
        public int? ItemLocationID { get; set; }
        public int UserID { get; set; }
        public int? DefaultLocationID { get; set; }
        public int PremisesID { get; set; }
        public string StatusID { get; set; }
        public string Type { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string Summary { get; set; }
        public int ItemID { get; set; }
        public string ToSupplierRange { get; set; }
        public int? VoucherID { get; set; }
        public int? VoucherFromID { get; set; }
        public int? VoucherToID { get; set; }
        public int? DocumentID { get; set; }
        public int? BankID { get; set; }
        public string DocType { get; set; }
        public string VoucherNo { get; set; }
        public string DocumentNo { get; set; }
        public string BankDetails { get; set; }
        public int VoucherReturnID { get; set; }
        public string VoucherReturnNo { get; set; }
        public int? DebitNoteNoFromID { get; set; }
        public int? DebitNoteNoToID { get; set; }
        public int CreditNoteNoFromID { get; set; }
        public int CreditNoteNoToID { get; set; }
        public string AdvancePaymentNoFrom { get; set; }
        public int? AdvancePaymentNoFromID { get; set; }
        public string AdvancePaymentNoTo { get; set; }
        public int? AdvancePaymentNoToID { get; set; }
        public int? AdvancePaymentNoID { get; set; }
        public int? AdvReturnVchNoID { get; set; }
        public int? SLAStatusID { get; set; }
        public string ToDepartmentRange { get; set; }
        public string FromDepartmentRange { get; set; }
        public string DocTypeFromRange { get; set; }
        public string DocTypeToRange { get; set; }
        public string TransTypeFromRange { get; set; }
        public string TransTypeToRange { get; set; }
        public string TransType { get; set; }
        public int? TransTypeID { get; set; }
        public int? TransCodeID { get; set; }
        public int? AccountCodeID { get; set; }
        public string AccountCode { get; set; }
        public string AccountCodeFrom { get; set; }
        public int? AccountCodeFromID { get; set; }
        public string AccountCodeTo { get; set; }
        public int? AccountCodeToID { get; set; }
        public string AccountNameFromRange { get; set; }
        public string AccountNameToRange { get; set; }
        public int? DepartmentID { get; set; }
        public string EmployeeNoFromRange { get; set; }
        public string EmployeeNoToRange { get; set; }
        public string InterCompanyFromRange { get; set; }
        public string InterCompanyToRange { get; set; }
        public int? InterCompanyID { get; set; }
        public int? ProjectID { get; set; }
        public string FromSupplierCatRange { get; set; }
        public string ToSupplierCatRange { get; set; }
        public int? PaymentTypeID { get; set; }
        public string AdvReturnVchNoFrom { get; set; }
        public int? AdvReturnVchNoFromID { get; set; }
        public string AdvReturnVchNoTo { get; set; }
        public int? AdvReturnVchNoToID { get; set; }
        public string Status { get; set; }
        public string FromLocationRange { get; set; }
        public string ToLoationRange { get; set; }
        public string PartyType { get; set; }
        public string Department { get; set; }
        public string EmployeeFromRange { get; set; }
        public string EmployeeToRange { get; set; }
        public string party { get; set; }
        public int FromBankNameID { get; set; }
        public int ToBankNameID { get; set; }
        public int FromBankAccountNoID { get; set; }
        public string FromBankAccountNo { get; set; }
        public int ToBankAccountNoID { get; set; }
        public string ToBankAccountNo { get; set; }
        public string FromTransNo { get; set; }
        public int FromTransNoID { get; set; }
        public string ToTransNo { get; set; }
        public int ToTransNoID { get; set; }
        public int InstrumentNoID { get; set; }
        public string FromInstrumentNo { get; set; }
        public int ToInstrumentNoID { get; set; }
        public string ToInstrumentNo { get; set; }
        public string InstrumentNo { get; set; }
        public string InstrumentStatus { get; set; }
        public string Filters { get; set; }
        public string UserName { get; set; }
        public int? Location { get; set; }
        public string ReportDataType { get; set; }


        public SelectList FromBankNameList { get; set; }
        public SelectList ToBankNameList { get; set; }
        public string DocumentNoFrom { get; set; }
        public string DocumentNoTo { get; set; }
        public int? DocumentNoFromID { get; set; }
        public int? DocumentNoToID { get; set; }
        public string KeyValue { get; set; }
        public string AccountDescription { get; set; }
        public int? BankNameID { get; set; }
        public int? BankAccountNoID { get; set; }
        public SelectList EmployeeFromRangeList { get; set; }
        public SelectList EmployeeToRangeList { get; set; }

        public SelectList DepartmentList { get; set; }
        public SelectList StatusList { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public SelectList FromSupplierRangeList { get; set; }
        public SelectList ToSupplierRangeList { get; set; }
        public string FromSupplierRange { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList PremisesList { get; set; }
        public SelectList ProjectList { get; set; }
        public SelectList InterCompanyList { get; set; }
        public SelectList EmployeeList { get; set; }
        public SelectList DDLDepartment { get; set; }
        //public string DepartmentTo { get; set; }
        public SelectList FromDocTypeRangeList { get; set; }
        public SelectList ToDocTypeRangeList { get; set; }
        public SelectList FromTransTypeRangeList { get; set; }
        public SelectList ToTransTypeRangeList { get; set; }
        public SelectList FromAccountNameRangeList { get; set; }
        public SelectList ToAccountNameRangeList { get; set; }
        public SelectList FromEmployeeNoRangeList { get; set; }
        public SelectList ToEmployeeNoRangeList { get; set; }
        public SelectList FromInterCompanyRangeList { get; set; }
        public SelectList ToInterCompanyRangeList { get; set; }
        public SelectList FromSupplierCatRangeList { get; set; }
        public SelectList ToSupplierCatRangeList { get; set; }
        public SelectList FromLocationRangeList { get; set; }
        public SelectList ToLocationRangeList { get; set; }
        public SelectList FromDepartmentRangeList { get; set; }
        public SelectList ToDepartmentRangeList { get; set; }
        public SelectList PaymentTypeList { get; set; }
    }

    public class ManufacturingModel
    {
        public int ID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ProductName { get; set; }
        public string ProductionGroupName { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public string Summary { get; set; }
        public string ProductionGroupNameFrom { get; set; }
        public string ProductionGroupNameTo { get; set; }
        public int? ProductionGroupID { get; set; }
        public string FromOutputItemCodeRange { get; set; }
        public string ToOutputItemCodeRange { get; set; }
        public string FromInputItemCodeRange { get; set; }
        public string ToInputItemCodeRange { get; set; }
        public SelectList FromOutputItemCodeRangeList { get; set; }
        public SelectList ToOutputItemCodeRangeList { get; set; }
        public SelectList FromInputItemCodeRangeList { get; set; }
        public SelectList ToInputItemCodeRangeList { get; set; }
        public string OutputItemCode { get; set; }
        public string FromOutputItemNameRange { get; set; }
        public string ToOutputItemNameRange { get; set; }
        public SelectList FromOutputItemNameRangeList { get; set; }
        public SelectList ToOutputItemNameRangeList { get; set; }
        public string FromInputItemNameRange { get; set; }
        public string ToInputItemNameRange { get; set; }
        public SelectList FromInputItemNameRangeList { get; set; }
        public SelectList ToInputItemNameRangeList { get; set; }
        public string OutputItemName { get; set; }
        public string InputItemName { get; set; }
        public int OuputItemCodeID { get; set; }
        public int? OuputItemNameID { get; set; }
        public int InputItemCodeID { get; set; }
        public int InputItemNameID { get; set; }
        public string PSTransNoFrom { get; set; }
        public string PSTransNoTo { get; set; }
        public int? PSTransNoFromID { get; set; }
        public int? PSTransNoToID { get; set; }
        public string PSBatchNoFrom { get; set; }
        public string PSBatchNoTo { get; set; }
        public int? PSBatchNoFromID { get; set; }
        public int? PSBatchNoToID { get; set; }
        public string PSBatchDateFrom { get; set; }
        public string PSBatchDateTo { get; set; }
        public string CategoryFrom { get; set; }
        public string CategoryTo { get; set; }
        public int CategoryFromID { get; set; }
        public int CategoryToID { get; set; }
        public string BatchStatusFrom { get; set; }
        public string BatchStatusTo { get; set; }
        public string RateValue { get; set; }
        public string ProductionType { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string StdCost { get; set; }
        public int? ItemCodeFromID { get; set; }
        public int? ItemCodeToID { get; set; }
        public string ItemCodeID { get; set; }
        public int InputItemCodeFromID { get; set; }
        public int InputItemCodeToID { get; set; }
        public string Status { get; set; }
        public SelectList StatusList { get; set; }
        public int ScheduleNoFromID { get; set; }
        public int ScheduleNoToID { get; set; }
        public int? IssueNoFromID { get; set; }
        public int? IssueNoToID { get; set; }
        public int? ReceiptNoFromID { get; set; }
        public int? ReceiptNoToID { get; set; }
        public int? ProductionIssueNoFromID { get; set; }
        public int? ProductionIssueNoToID { get; set; }
        public int? SalesCategoryID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public int? ProductionCategoryID { get; set; }
        public SelectList ProductionCategoryList { get; set; }
        public string ItemName { get; set; }
        public int? ItemID { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public int LocationID { get; set; }
        public string Filters { get; set; }
    }

    public class SalesModel : ReportModel
    {
        public string ReportAsAt { get; set; }
        public int ID { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string ReportName { get; set; }
        //public string FromDate { get; set; }
        //public string ToDate { get; set; }
        public string ReceiptDateFrom { get; set; }
        public string ReceiptDateTo { get; set; }
        public string SalesType { get; set; }
        public int? InvoiceNOFromID { get; set; }
        public int? InvoiceNOToID { get; set; }
        public int? CustomerCodeFromID { get; set; }
        public int? CustomerCodeToID { get; set; }
        public string FromCustomerRange { get; set; }
        public string ToCustomerRange { get; set; }
        public SelectList FromCustomerRangeList { get; set; }
        public SelectList ToCustomerRangeList { get; set; }
        public int? CustomerID { get; set; }
        public int? LocationFromID { get; set; }
        public int? LocationToID { get; set; }
        public int? LocationID { get; set; }
        public int? ItemLocationID { get; set; }
        public string FromCategoryRange { get; set; }
        public string ToCategoryRange { get; set; }


        public int? ItemCategoryID { get; set; }
        public int? ItemCodeFromID { get; set; }
        public int? ItemCodeToID { get; set; }
        public string ItemFromRange { get; set; }
        public string ItemToRange { get; set; }
        public int? ItemID { get; set; }
        public string InvoiceDateFrom { get; set; }
        public string InvoiceDateTo { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string AgeingBucket { get; set; }
        public int AgeingBucketID { get; set; }
        public SelectList FromItemCategoryRangeList { get; set; }
        public SelectList ToItemCategoryRangeList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList FromCategoryRangeList { get; set; }
        public SelectList ToCategoryRangeList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList FromItemRangeList { get; set; }
        public SelectList ToItemRangeList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList AgeingBucketsList { get; set; }
        public int? SalesOrderNoFromID { get; set; }
        public int? SalesOrderNoToID { get; set; }
        public int? SalesInvoiceID { get; set; }
        public int? SalesyCategoryID { get; set; }

        public int? ReceiptNoFromID { get; set; }
        public string ReceiptNoFrom { get; set; }
        public string ReceiptNoTo { get; set; }
        public int? ReceiptNoToID { get; set; }
        public string ChequeStatus { get; set; }
        public string CustomerCode { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public int CustomerCodeID { get; set; }
        public SelectList ChequeStatusList { get; set; }
        public string SalesMarginType { get; set; }
        public int SalesID { get; set; }
        public int? SalesCategoryID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public int? CustomerCategoryID { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public string Filters { get; set; }
        public int? Locations { get; set; }
        public int? DoctorID { get; set; }
        public SelectList DoctorList { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public int? BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public string Type { get; set; }
        public int? FSOID { get; set; }
        public string FSOName { get; set; }
        public int EmployeeID { get; set; }
        public int? SalesIncentiveCategoryID { get; set; }
        public int? PatientID { get; set; }
        public SelectList SalesIncentiveCategoryList { get; set; }
        public string ItemType { get; set; }
        public string ExportType { get; set; }

        public SelectList PaymentModeList { get; set; }
        public int? PaymentModeID { get; set; }
    }

    public class GSTViewModel
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Type { get; set; }
        public string FromSupplierRange { get; set; }
        public string FromSupplierTaxSubCategoryRange { get; set; }
        public string ToSupplierTaxSubCategoryRange { get; set; }
        public string ToSupplierRange { get; set; }
        public string FromItemCategoryRange { get; set; }
        public string ToItemCategoryRange { get; set; }
        public string FromItemNameRange { get; set; }
        public string ToItemNameRange { get; set; }
        public string InvoiceDateFrom { get; set; }
        public string InvoiceDateTo { get; set; }
        public string QCNOFrom { get; set; }
        public int? QCNOFromID { get; set; }
        public string QCNOTo { get; set; }
        public int? QCNOToID { get; set; }
        public int? GRNNOFromID { get; set; }
        public int? GRNNOToID { get; set; }
        public int? SupplierGSTNoID { get; set; }
        public string QCDateFrom { get; set; }
        public string QCDateTo { get; set; }
        public string GRNNoFrom { get; set; }
        public string GRNNoTo { get; set; }
        public string GRNDateFrom { get; set; }
        public string GRNDateTo { get; set; }
        public int GSTRateFrom { get; set; }
        public int GSTRateTo { get; set; }
        public decimal? FromGSTRateRange { get; set; }
        public decimal? ToGSTRateRange { get; set; }
        public string FromSupplierGSTNNo { get; set; }
        public string ToSupplierGSTNNo { get; set; }
        public int? ItemCategoryID { get; set; }
        public int? SupplierID { get; set; }
        public int? ItemID { get; set; }
        public string SupplierTaxSubCategory { get; set; }
        public int? SupplierTaxSubCategoryID { get; set; }
        public string IGST { get; set; }
        public string Filters { get; set; }
        public int ItemLocationID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public SelectList FromSupplierTaxSubCategoryRangeList { get; set; }
        public SelectList ToSupplierTaxSubCategoryRangeList { get; set; }
        public SelectList FromSupplierRangeList { get; set; }
        public SelectList ToSupplierRangeList { get; set; }
        public SelectList FromItemCategoryRangeList { get; set; }
        public SelectList ToItemCategoryRangeList { get; set; }
        public SelectList FromItemNameRangeList { get; set; }
        public SelectList ToItemNameRangeList { get; set; }
        public SelectList FromGSTRateRangeList { get; set; }
        public SelectList ToGSTRateRangeList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList FromSupplierGSTNNoRangeList { get; set; }
        public SelectList ToSupplierGSTNNoRangeList { get; set; }
        public SelectList LocationList { get; set; }

    }

    public class SalesGSTModel
    {
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string ReportDataType { get; set; }
        public string InvoiceDateFrom { get; set; }
        public string InvoiceDateTo { get; set; }
        public string FromCustomerRange { get; set; }
        public SelectList FromCustomerRangeList { get; set; }
        public string ToCustomerRange { get; set; }
        public SelectList ToCustomerRangeList { get; set; }
        public int CustomerID { get; set; }
        public int? CustomerTaxCategoryID { get; set; }
        public SelectList CustomerTaxCategoryList { get; set; }
        public int? ItemCategoryID { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public int? SalesCategoryID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public string ItemFromRange { get; set; }
        public SelectList FromItemRangeList { get; set; }
        public string ItemToRange { get; set; }
        public SelectList ToItemRangeList { get; set; }
        public int? ItemID { get; set; }
        public int? InvoiceNoFromID { get; set; }
        public int? InvoiceNoToID { get; set; }
        public string CustomerGSTNo { get; set; }
        public int? CustomerGSTNoID { get; set; }
        public int GSTRateFrom { get; set; }
        public int GSTRateTo { get; set; }
        public string IGST { get; set; }
        public int FromGSTRateRange { get; set; }
        public SelectList FromGSTRateRangeList { get; set; }
        public int ToGSTRateRange { get; set; }
        public SelectList ToGSTRateRangeList { get; set; }
        public string TransactionType { get; set; }
        public string Filters { get; set; }
        public int UserID { get; set; }
        public int? LocationID { get; set; }
        public string ItemType { get; set; }
        public int? Locations { get; set; }
        public SelectList LocationList { get; set; }
    }

    public class MISViewModel
    {
        public string DateAsOn { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BalanceType { get; set; }
        public string FromSupplierRange { get; set; }
        public string ToSupplierRange { get; set; }
        public string SupplierName { get; set; }
        public string FromInvoiceDate { get; set; }
        public string ToInvoiceDate { get; set; }
        public string DocumentNoFrom { get; set; }
        public string DocumentNoTo { get; set; }
        public int MyProperty { get; set; }
        public int FromInvoiceTransNo { get; set; }
        public int ToInvoiceTransNo { get; set; }
        public string FromTransDate { get; set; }
        public string ToTransDate { get; set; }
        public string OutstandingRange { get; set; }
        public int? OutstandingDays { get; set; }
        public bool BalancePayableYes { get; set; }
        public int? OutstandingDaysNo { get; set; }
        public string InoiceStatusFrom { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string ReportDataType { get; set; }
        public int? SupplierID { get; set; }
        public int InvoiceID { get; set; }
        public string DocumentNo { get; set; }
        public string FromTransType { get; set; }
        public string ToTransType { get; set; }
        public string KeyValueFrom { get; set; }
        public string KeyValueTo { get; set; }
        public string KeyValue { get; set; }
        public int KeyValueID { get; set; }
        public string FromItemAccountsCategory { get; set; }
        public string ToItemAccountsCategory { get; set; }
        public SelectList FromItemAccountsCategoryList { get; set; }
        public string FromItemNameRange { get; set; }
        public string ToItemNameRange { get; set; }
        public string FromDocumentRange { get; set; }
        public string ToDocumentRange { get; set; }
        public string FromItemCategoryRange { get; set; }
        public string ToItemCategoryRange { get; set; }
        public string FromTransTypeRange { get; set; }
        public string ToTransTypeRange { get; set; }
        public SelectList FromTransTypeRangeList { get; set; }
        public int? TransTypeID { get; set; }
        public int? ItemCategoryID { get; set; }
        public int? InvoiceNOFromID { get; set; }
        public int? InvoiceNOToID { get; set; }
        public string InvoiceNOFrom { get; set; }
        public string InvoiceNOTo { get; set; }
        public string InvoiceStatus { get; set; }
        public int? SupplierInvoiceNOFromID { get; set; }
        public int? SupplierInvoiceNOID { get; set; }
        public string SupplierInvoiceNO { get; set; }
        public string SupplierInvoiceNOTO { get; set; }
        public int? SupplierInvoiceNOTOID { get; set; }
        public int? ItemID { get; set; }
        public string Status { get; set; }
        public int? DocumentID { get; set; }
        public string InoiceTransNoFrom { get; set; }
        public string InoiceTransNoTo { get; set; }
        public string BalancePayableOnly { get; set; }
        public SelectList OutstandingRangeList { get; set; }
        public SelectList AgeingBucketList { get; set; }
        public string TransactionNoFrom { get; set; }
        public string TransactionNoTo { get; set; }
        public string TransactionTypeFrom { get; set; }
        public string TransactionTypeTo { get; set; }
        public string TransactionType { get; set; }
        public string TransType { get; set; }
        public string ItemAccountCategoryFrom { get; set; }
        public string ItemAccountCategoryTo { get; set; }
        public string ItemAccountCategory { get; set; }
        public string StatusFrom { get; set; }
        public string StatusTo { get; set; }
        public SelectList FromDocumentRangeList { get; set; }
        public SelectList ToDocumentRangeList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList FromItemCategoryRangeList { get; set; }
        public SelectList ToItemCategoryRangeList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public int AccountGroupID { get; set; }
        public int AccountID { get; set; }
        public string DocumentType { get; set; }
        public string DocTypeFromRange { get; set; }
        public string DocTypeToRange { get; set; }
        public string TransTypeFromRange { get; set; }
        public string TransTypeToRange { get; set; }
        public int? AccountCodeFromID { get; set; }
        public int? AccountCodeToID { get; set; }
        public string AccountNameFromRange { get; set; }
        public string AccountNameToRange { get; set; }
        public SelectList AccountNameFromList { get; set; }
        public int? AccountNameID { get; set; }
        public int LocationID { get; set; }
        public int? DepartmentID { get; set; }
        public string EmployeeNoToRange { get; set; }
        public int? EmployeeID { get; set; }
        public string InterCompanyFromRange { get; set; }
        public string InterCompanyToRange { get; set; }
        public string EmployeeNoFromRange { get; set; }
        public int? InterCompanyID { get; set; }
        public int? ProjectID { get; set; }
        public string AgeingBucket { get; set; }
        public int AgeingBucketID { get; set; }
        public SelectList FromItemNameRangeList { get; set; }
        public SelectList ToItemNameRangeList { get; set; }
        public SelectList SupplierRangeList { get; set; }
        public string ItemType { get; set; }
        public DateTime TransDateFrom
        {
            get
            {
                return FromTransDate != null ? General.ToDateTime(FromTransDate) : DateTime.Now;
            }
            set
            {
                TransDateFrom = value;
            }
        }
        public DateTime TransDateTo
        {
            get
            {
                return ToTransDate != null ? General.ToDateTime(ToTransDate) : DateTime.Now;
            }
            set
            {
                TransDateTo = value;
            }
        }
        public DateTime StartDate
        {
            get
            {
                return FromDate != null ? General.ToDateTime(FromDate) : DateTime.Now;
            }
            set
            {
                StartDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return ToDate != null ? General.ToDateTime(ToDate) : DateTime.Now;
            }
            set
            {
                EndDate = value;
            }
        }
        public string Filters { get; set; }
    }

    public class TransactionNumberModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
    }

    public class CashOrBankModel
    {
        public int ID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Summary { get; set; }
        public string Filters { get; set; }
        public int? TreasuryID { get; set; }
        public string DocumentNo { get; set; }
        public int? BankID { get; set; }
        public string BankDetails { get; set; }
        public string AccountCodeFrom { get; set; }
        public int? AccountCodeFromID { get; set; }
        public int? AccountNameID { get; set; }
        public string AccountName { get; set; }
        public int BankAccountNo { get; set; }
        public int BankAccountNoID { get; set; }
        public int? Location { get; set; }
        public int? LocationID { get; set; }
        public SelectList LocationList { get; set; }
        public int? PaymentModeID { get; set; }
        public SelectList PaymentModeList { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }

    }

    public class ItemMovementModel
    {
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public int? RequestNoFromID { get; set; }
        public string RequestNoFrom { get; set; }
        public int? RequestNoToID { get; set; }
        public string RequestNoTo { get; set; }
        public string RequestDateFrom { get; set; }
        public string RequestDateTo { get; set; }
        public string DeliveredDateFrom { get; set; }
        public string DeliveredDateTo { get; set; }
        public int LocationID { get; set; }
        public string LocationFrom { get; set; }
        public int? LocationFromID { get; set; }
        public int LocationToID { get; set; }
        public string LocationTo { get; set; }
        public string PremisesFrom { get; set; }
        public int? PremisesFromID { get; set; }
        public string PremisesTo { get; set; }
        public int? PremisesToID { get; set; }
        public string ItemCategory { get; set; }
        public int? ItemCategoryID { get; set; }
        public int? ItemCodeFromID { get; set; }
        public int? ItemCodeToID { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeFrom { get; set; }
        public string ItemCodeTo { get; set; }
        public int? ItemNameID { get; set; }
        public string ItemName { get; set; }
        public string BatchType { get; set; }
        public int? BatchTypeID { get; set; }
        public string TransactionType { get; set; }
        public string FromItemCategoryRange { get; set; }
        public string ToItemCategoryRange { get; set; }
        public string FromItemNameRange { get; set; }
        public string ToItemNameRange { get; set; }
        public int? ItemID { get; set; }
        public string StatusFrom { get; set; }
        public string StatusTo { get; set; }
        public string ItemMovementTransactionType { get; set; }
        public string purchaseOrderNoFrom { get; set; }
        public int? purchaseOrderNOFromID { get; set; }
        public string PurchaseOrderNoTo { get; set; }
        public int? PurchaseOrderNoToID { get; set; }
        public string GRNNoFrom { get; set; }
        public int? GRNNoFromID { get; set; }
        public string GRNNoTo { get; set; }
        public int? GRNNoToID { get; set; }
        public string GRNDateFrom { get; set; }
        public string GRNDateTo { get; set; }
        public string ValueType { get; set; }
        public string RateValue { get; set; }
        public int? BatchID { get; set; }
        public string BatchNo { get; set; }
        public string Filters { get; set; }
        public int UserID { get; set; }
        public List<LocationHead> LocationHeadList { get; set; }
        public SelectList FromItemCategoryRangeList { get; set; }
        public SelectList ToItemCategoryRangeList { get; set; }
        public SelectList FromItemNameRangeList { get; set; }
        public SelectList ToItemNameRangeList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList PremiseList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList TransactionTypeList { get; set; }
        public SelectList PremisesToList { get; set; }
        public SelectList PremisesFromList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList ItemMovementTransactionTypeList { get; set; }
        public SelectList ValueList { get; set; }
    }

    public class StockModel
    {
        public string Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DeliveredDateFrom { get; set; }
        public string DeliveredDateTo { get; set; }
        public string TransactionType { get; set; }
        public int? TransactionTypeID { get; set; }
        public SelectList TransactionTypeList { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public string RequestNoFrom { get; set; }
        public int RequestNoFromID { get; set; }
        public string RequestNoTo { get; set; }
        public int RequestNoToID { get; set; }
        //public string IssueDateFrom { get; set; }
        //public string IssueDateTo { get; set; }
        public string IssueNoFrom { get; set; }
        public int IssueNoFromID { get; set; }
        public string IssueNoTo { get; set; }
        public int IssueNoToID { get; set; }
        public int LocationID { get; set; }
        public string LocationFrom { get; set; }
        public int LocationFromID { get; set; }
        public string LocationTo { get; set; }
        public int LocationToID { get; set; }
        public string PremisesFrom { get; set; }
        public int PremisesFromID { get; set; }
        public string PremisesTo { get; set; }
        public int PremisesToID { get; set; }
        public string ItemCategoryFromRange { get; set; }
        public string ItemCategoryToRange { get; set; }
        public int ItemCategoryID { get; set; }
        public int? ItemCodeFromID { get; set; }
        public int? ItemCodeToID { get; set; }
        public string ItemCodeID { get; set; }
        public string ItemNameFromRange { get; set; }
        public string ItemNameToRange { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Summary { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string StockType { get; set; }
        public string ValueType { get; set; }
        public SelectList FromItemCategoryRangeList { get; set; }
        public SelectList ToItemCategoryRangeList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList FromItemNameRangeList { get; set; }
        public SelectList ToItemNameRangeList { get; set; }
        public int ToLocationID { get; set; }
        public int FromLocationID { get; set; }
        public SelectList LocationList { get; set; }
        public List<LocationHead> LocationHeadList { get; set; }
        public int ToPremiseID { get; set; }
        public int FromPremiseID { get; set; }
        public int PremiseID { get; set; }
        public SelectList PremiseList { get; set; }
        public SelectList ValueList { get; set; }
        public string Filters { get; set; }
    }

    public class LocationHead
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LocationType { get; set; }
        public string LocationHeadName { get; set; }
        public int LocationHeadID { get; set; }
    }

    public class StockAgeing
    {
        public string StockAsAt { get; set; }
        public string Date { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public string PremisesFrom { get; set; }
        public string PremisesTo { get; set; }
        public int? PremiseId { get; set; }
        public string Premises { get; set; }
        public string categoryFrom { get; set; }
        public string categoryTo { get; set; }
        public int? ItemCategoryID { get; set; }
        public int? SalesCategoryID { get; set; }
        public string ItemCodeFrom { get; set; }
        public string ItemCodeTo { get; set; }
        public int? ItemCodeFromID { get; set; }
        public int? ItemCodeToID { get; set; }
        public string ItemNameFrom { get; set; }
        public string ItemNameTo { get; set; }
        public string ItemName { get; set; }
        public int? ItemNameID { get; set; }
        public int? ItemID { get; set; }
        public string BatchTypeFrom { get; set; }
        public string BatchTypeTo { get; set; }
        public int? BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public string AgeingBucket { get; set; }
        public int AgeingBucketID { get; set; }
        public string ValueType { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }
        public string StockAgeingType { get; set; }
        public string Batch { get; set; }
        public string BatchNo { get; set; }
        public int BatchID { get; set; }
        public string Filters { get; set; }

        public SelectList LocationFromList { get; set; }
        public SelectList LocationToList { get; set; }
        public SelectList PremisesFromList { get; set; }
        public SelectList PremisesToList { get; set; }
        public SelectList categoryFromList { get; set; }
        public SelectList categoryToList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList ItemCodeFromList { get; set; }
        public SelectList ItemCodeToList { get; set; }
        public SelectList ItemNameFromList { get; set; }
        public SelectList ItemNameToList { get; set; }
        public SelectList AgeingBucketList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList PremisesList { get; set; }
        public SelectList BatchTypeFromList { get; set; }
        public SelectList BatchTypeToList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList ValueList { get; set; }
    }

    public class CustomerAgeing
    {
        public string ReportAsAt { get; set; }
        public string ItemCodeFrom { get; set; }
        public string ItemCodeTo { get; set; }
        public int? ItemCodeFromID { get; set; }
        public int? ItemCodeToID { get; set; }
        public string ItemNameFrom { get; set; }
        public string ItemNameTo { get; set; }
        public string ItemName { get; set; }
        public int? ItemID { get; set; }
        public string FromItemNameRange { get; set; }
        public string ToItemNameRange { get; set; }
        public string LocatioFrom { get; set; }
        public int? LocationFromID { get; set; }
        public string LocationTo { get; set; }
        public int? LocationToID { get; set; }
        public int CustomerCodeFromID { get; set; }
        public int CustomerCodeToID { get; set; }
        public string CustomerCodeFrom { get; set; }
        public string CustomerCodeTo { get; set; }
        public string CustomerNameFrom { get; set; }
        public string CustomerNameTo { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerID { get; set; }
        public string InvoiceNoFrom { get; set; }
        public string InvoiceNoTo { get; set; }
        public string InvoiceDateFrom { get; set; }
        public string InvoiceDateTo { get; set; }
        public string AgeingBuckets { get; set; }
        public int AgeingBucketID { get; set; }
        public int? InvoiceNoFromID { get; set; }
        public int? InvoiceNoToID { get; set; }
        public string type { get; set; }
        public SelectList FromItemNameRangeList { get; set; }
        public SelectList ToItemNameRangeList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList CustomerNameFromList { get; set; }
        public SelectList CustomerNameToList { get; set; }
        public SelectList AgeingBucketsList { get; set; }
    }

    public class ReportModel
    {
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }

        public string Filters { get; set; }
        public string ExportType { get; set; }
        public DateTime? FromDate
        {
            get
            {

                return General.ToDateTimeNull(FromDateString);
            }

            set
            {
                FromDate = value;
            }
        }
        public DateTime? ToDate
        {
            get
            {
                return General.ToDateTimeNull(ToDateString);
            }

            set
            {
                ToDate = value;
            }
        }
        public string FromDateFormatted
        {
            get
            {
                return General.FormatDateNull(FromDate, "view");
            }

            set
            {
                FromDateFormatted = value;
            }
        }
        public string ToDateFormatted
        {
            get
            {
                return General.FormatDateNull(ToDate, "view");
            }

            set
            {
                ToDateFormatted = value;
            }
        }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList A2ZRangeList { get; set; }
        public SelectList StatusList { get; set; }
        public string ReportType { get; set; }
        public string PaymentType { get; set; }

    }

    public class PrintModel : ReportModel
    {
        public int ID { get; set; }
    }

    public class StockReportModel : ReportModel
    {
        public int? PremiseID { get; set; }
        public SelectList PremiseList { get; set; }
        public int? BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public int? ItemID { get; set; }
        public int? ItemCategoryID { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public int ToLocationID { get; set; }
        public int FromLocationID { get; set; }
        public int? SalesCategoryID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        //public int UserID { get; set; }
        public int? ItemLocationID { get; set; }
        public string Itemtype { get; set; }
        public int BatchID { get; set; }

    }

    public class SalesReportModel : ReportModel
    {
        public int? ItemID { get; set; }
        public int? BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public int? ItemCategoryID { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public int? SalesCategoryID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        //public int UserID { get; set; }
        public int? StateID { get; set; }
        public SelectList StateList { get; set; }
        public int? CustomerID { get; set; }
        public string ItemType { get; set; }
        public SelectList ReportTypeList { get; set; }
        public int ItemLocationID { get; set; }
        public int PaymentModeID { get; set; }
        public SelectList PaymentModeList { get; set; }
        public string ReportDataType { get; set; }
        public SelectList DoctorList { get; set; }
        public int? DoctorID { get; set; }
    }


    public class AccountsReportModel : ReportModel
    {
        //public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class SalesOrderReportModel : SalesReportModel
    {
        public int SalesOrderFromID { get; set; }
        public int SalesOrderToID { get; set; }
        public int CustomerFromID { get; set; }
        public int CustomerToID { get; set; }
        public string FromCustomerRange { get; set; }
        public string ToCustomerRange { get; set; }
        public int ItemFromID { get; set; }
        public int ItemToID { get; set; }
        public string Status { get; set; }
    }

    public class PurchaseReportModel : ReportModel
    {
        public int? SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int? PONOFromID { get; set; }
        public string PONOFrom { get; set; }
        public int? PONOToID { get; set; }
        public string PONOTo { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }

    }
    public class GRMTransReportModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
    }
    public class PurchaseOrderReportModel
    {
        public Nullable<decimal> CGSTAmt { get; set; }
        public string MinimumCurrency { get; set; }
        public string AmountInWords { get; set; }
        public string MobileNo { get; set; }
        public string CountryName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Shipment {  get; set; }
        public string BAddressLine1 { get; set; }
        public string BAddressLine2 { get; set; }
        public string BAddressLine3 { get; set; }
        public string SupplierName { get; set; }
        public string PaymentMode { get; set; }
        public int PaymentWithin { get; set; }
        public string TermsOfPrice { get; set; }
        public string BillingLocation { get; set; }
        public string DeliveryWithin { get; set; }
        //public int DeliveryWithin { get; set; }
        public string SupplierReferenceNo { get; set; }
        public string Remarks { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public decimal NetAmt { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string SuppQuotNo { get; set; }
        public int SupplierID { get; set; }
        public string Email { get; set; }
        public string OrderType { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> SuppDocAmount { get; set; }
        public Nullable<decimal> SuppShipAmount { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> SuppOtherCharge { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }

        public decimal DecimalPlaces { get; set; }


    }

    public class ReceiptVoucherReportModel : AccountsReportModel
    {
        public int? CustomerCodeFromID { get; set; }
        public int? CustomerID { get; set; }
        public int? ItemLocationID { get; set; }
    }

    public class StockExpiryReportModel : StockReportModel
    {
        public int ItemCodeFromID { get; set; }
        public int ItemCodeToID { get; set; }
    }

    public class StockValuationReportModel : StockReportModel
    {
        public int? ItemCodeFromID { get; set; }
        //public int? BatchID { get; set; }
        public string ValueType { get; set; }
        public SelectList ValueList { get; set; }
        public bool IsQtyZero { get; set; }
    }

    public class StockTransferShortageReportModel : StockReportModel
    {
        public List<LocationHead> LocationHeadList { get; set; }
    }

    public class StockTransferGSTReportModel : StockReportModel
    {
        public int IssueNoFromID { get; set; }
        public int IssueNoToID { get; set; }
        public int ReceiptNoFromID { get; set; }
        public int ReceiptNoToID { get; set; }
        public List<LocationHead> LocationHeadList { get; set; }
    }

    public class TransportPermitReportModel : SalesReportModel
    {
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public int SalesInvoiceNoFromID { get; set; }
        public int StockTransferNoFromID { get; set; }
        public int StockTransferNoToID { get; set; }
        public int SalesInvoiceNoToID { get; set; }

    }

    public class StockLedgerReportModel : StockReportModel
    {
        //public int? BatchID { get; set; }
        public string Type { get; set; }
        public string NMode { get; set; }
        public string NItemType { get; set; }
        public string ValueType { get; set; }
        public SelectList ValueList { get; set; }
    }

    public class StockAdjustmentReportModel : StockReportModel
    {
        public string StockAdjustmentType { get; set; }
        public string Mode { get; set; }
    }

    public class SalesInvoiceReportModel : SalesReportModel
    {
        public int? InvoiceNoFromID { get; set; }
        public int? InvoiceNoToID { get; set; }
        //public int DoctorID { get; set; }
        //public int PatientID { get; set; }      
        public string ItemAutoType { get; set; }
        //public SelectList DoctorList { get; set; }
        public string PaymentType { get; set; }

    }


    public class DoctorWiseSalesModel : SalesReportModel
    {
        public int? InvoiceNoFromID { get; set; }
        public int? InvoiceNoToID { get; set; }
        //public int DoctorID { get; set; }
        //public int PatientID { get; set; }
        public string ItemAutoType { get; set; }
        public SelectList DoctorList { get; set; }

    }

    public class ManufacturingReportModel : ReportModel
    {
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public int? ItemCodeID { get; set; }
        public string ItemCode { get; set; }
        public int SalesCategoryID { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public double BasicPrice { get; set; }
        public int PremiseID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList PremiseList { get; set; }
    }

    public class MaterialPurificationModel : ManufacturingReportModel
    {
        public string ReceiptDateFrom { get; set; }
        public string ReceiptDateTo { get; set; }
        public string SupplierName { get; set; }
        public int? SupplierID { get; set; }
        public int? IssueItemCodeID { get; set; }
        public int? IssueItemID { get; set; }
        public int? IssueNoID { get; set; }
        public string IssueNo { get; set; }
        public int? ReceiptItemCodeID { get; set; }
        public int? ReceiptItemID { get; set; }
        public int? ReceiptNoID { get; set; }
        public string ReceiptNo { get; set; }
        public int? ProcessID { get; set; }
        public SelectList ProcessList { get; set; }
    }

    public class ProductionDefinitionModel : ManufacturingReportModel
    {
        public string ProductionGroupName { get; set; }
        public string Code { get; set; }
        public int? ProductionGroupID { get; set; }
        public decimal StandardBatchSize { get; set; }
        public decimal ActualBatchSize { get; set; }
        public decimal PackSize { get; set; }
        public decimal Qty { get; set; }
        public int PackingItemID { get; set; }
        public SelectList ItemPackingList { get; set; }

    }

    public class TDSReportModel : AccountsReportModel
    {
        public string TransactionNo { get; set; }
        public int? TransactionID { get; set; }
        public string FromSupplierRange { get; set; }
        public string ToSupplierRange { get; set; }
        public string SupplierName { get; set; }
        public int? SupplierID { get; set; }
        public string TDSCode { get; set; }
        public int? TDSCodeID { get; set; }
        public string PanNo { get; set; }
        public int? PanNoID { get; set; }
        public int TDSID { get; set; }
        public int Location { get; set; }

    }

    public class BatchwiseProductionPackingModel : ManufacturingReportModel
    {
    }

    public class ProductionOutputAnalysisModel : ManufacturingReportModel
    {
        public string PackingFromDateString { get; set; }
        public string PackingToDateString { get; set; }
        public int? ProductionGroupID { get; set; }
        public string ProductionGroupName { get; set; }
        public int? BatchNoID { get; private set; }
        public string BatchNo { get; set; }
        public string ProductionIssueNoFrom { get; set; }
        public string ProductionIssueNoTo { get; set; }
        public string PackingNoFrom { get; set; }
        public string PackingNoTo { get; set; }
        public string Status { get; set; }
        public SelectList StatusList { get; set; }

        public DateTime? PackingFromDate
        {
            get
            {

                return General.ToDateTimeNull(PackingFromDateString);
            }

            set
            {
                PackingFromDate = value;
            }
        }
        public DateTime? PackingToDate
        {
            get
            {
                return General.ToDateTimeNull(PackingToDateString);
            }

            set
            {
                PackingToDate = value;
            }
        }
        public string PackingFromDateFormatted
        {
            get
            {
                return General.FormatDateNull(PackingFromDate, "view");
            }

            set
            {
                PackingFromDateFormatted = value;
            }
        }
        public string PackingToDateFormatted
        {
            get
            {
                return General.FormatDateNull(PackingToDate, "view");
            }

            set
            {
                PackingToDateFormatted = value;
            }
        }

    }

    public class ProductionTargetVSActual : ManufacturingReportModel
    {

    }

    public class ProductionWorkInProgressModel : ManufacturingReportModel
    {
        public int ProductionGroupID { get; set; }
        public int ProductionCategoryID { get; set; }
        public SelectList ProductionCategoryList { get; set; }
        public int Value { get; set; }
    }

    public class SalesReturnReportModel : SalesReportModel
    {
        public int? InvoiceNoFromID { get; set; }
        public int? InvoiceNoToID { get; set; }
        public int? ItemLocationID { get; set; }
    }

    public class IncentiveReportModel : SalesReportModel
    {

        public string PartyType { get; set; }
        public SelectList PartyList { get; set; }
        public int TimePeriodID { get; set; }
        public SelectList TimePeriodList { get; set; }
        public int DurationID { get; set; }
        public SelectList DurationList { get; set; }
    }

    public class PurchaseOrderStatusReportModel : PurchaseReportModel
    {
        public int? ItemLocationID { get; set; }
        public string Status { get; set; }
    }

    public class CostingReportModel : StockReportModel
    {
        public SelectList CostCategoryList { get; set; }
    }
    public class TreatmentReportModel : ReportModel
    {
        public int? TherapistID { get; set; }
        public int? PatientID { get; set; }
        public int? TreatmentID { get; set; }
        public int TreatmentGroupID { get; set; }
        public int? TreatmentRoomID { get; set; }
        public int WareHouseFromID { get; set; }
        public int WareHouseToID { get; set; }
        public int NursingStationID { get; set; }
        public string Therapist { get; set; }
        public string PatientName { get; set; }
        public string Treatment { get; set; }
        public string TreatmentGroup { get; set; }
        public string TreatmentRoom { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string NursingStation { get; set; }
        public string WareHouseFrom { get; set; }
        public string WareHouseTo { get; set; }
        public int? DoctorID { get; set; }
        public SelectList DoctorList { get; set; }
        //public int ReferedBYID { get; set; }
        public SelectList PatientReferedList { get; set; }
        public SelectList TreatmentGroupList { get; set; }

        public string Room { get; set; }
        public int? RoomID { get; set; }

    }

    public class RoomModel : ReportModel
    {
        public int? RoomTypeID { get; set; }
        public SelectList RoomTypeList { get; set; }
    }
    public class OpRegisterModel : TreatmentReportModel
    {
        public string Date { get; set; }
        public string Month { get; set; }
        public int? ReferedByID { get; set; }
        public int? MonthID { get; set; }
        public SelectList MonthList { get; set; }
        public SelectList CategoryList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public List<SelectListItem> Year { get; set; }
        public string YearName { get; set; }
        //public string Year { get; set; }
        public int YearID { get; set; }
        public int CurrentYear { get; set; }
        public int? DepartmentID { get; set; }
        public string Department { get; set; }
    }

    public class CentralOpRegisterModel : TreatmentReportModel
    {
        public string Date { get; set; }
        public int Month { get; set; }
        public int MonthID { get; set; }
        public SelectList MonthList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public List<SelectListItem> Year { get; set; }
        public int YearID { get; set; }
        public int CurrentYear { get; set; }
        public SelectList DepartmentList { get; set; }
        public SelectList FromPlaceRangeList { get; set; }
        public SelectList ToPlaceRangeList { get; set; }
        public int? DepartmentID { get; set; }
        public string DoctorName { get; set; }
        public int? DistrictID { get; set; }
        public SelectList DistrictList { get; set; }
        public string Place { get; set; }
    }

    public class TrialBalanceModel : AccountsReportModel
    {
        public string TBType { get; set; }
    }

    public class DailySummaryModel : AccountsReportModel
    {
        public SelectList UserList { get; set; }
    }

    public class GeneralLedgerModel : AccountsReportModel
    {
        public string AccountName { get; set; }
        public int AccountNameID { get; set; }
        public string AccountGroup { get; set; }
        public int AccountGroupID { get; set; }
    }

    public class GSTRModel : SalesReportModel
    {
        public SelectList GSTPercentageList { get; set; }
        public decimal? GSTPercentage { get; set; }
    }
    public class DebitAndCreditNoteModel : AccountsReportModel
    {
        public string Type { get; set; }
        public int? DebitNoteNoFromID { get; set; }
        public int? DebitNoteNoToID { get; set; }
        public int CreditNoteNoFromID { get; set; }
        public int CreditNoteNoToID { get; set; }
    }
    public class SalesInvoiceAndCounterSalesModel : SalesReportModel
    {
        public int? InvoiceNoFromID { get; set; }
        public int? InvoiceNoToID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public string ItemAutoType { get; set; }
        public SelectList DoctorList { get; set; }
    }
    public class ProfitReportModel : ReportModel
    {
        public string Type { get; set; }
        public int ItemID { get; set; }
        public string Summary { get; set; }
    }

    public class CounterSaleReportModel : ReportModel
    {
        public string Type { get; set; }
        public int FinYear { get; set; }
        public string Mode { get; set; }
        public int InvoiceNoID { get; set; }
        public string SupplierName { get; set; }
        public int SupplierID { get; set; }
        public SelectList UsersList { get; set; }

    }

    public class DailyCollectionModel : SalesReportModel
    {

    }

    public class CollectionSummaryModel : ReportModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AccountName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class DoctorwiseMonthlySummaryModel : TreatmentReportModel
    {
        public string DoctorName { get; set; }
        public string YearName { get; set; }
        public int YearID { get; set; }
        public int CurrentYear { get; set; }
        public SelectList DoctorsList { get; set; }

    }
    public class TreatmentScheduleByTherapistModel : TreatmentReportModel
    {
        public string TherapistName { get; set; }
        public SelectList therapistList { get; set; }

        // public string PatientName { get; set; }
        public SelectList patientsList { get; set; }
        public string Mode { get; set; }
    }

    public class GeneralReportsModel : SalesReportModel
    {

    }


}


