using BusinessObject;
using System.Configuration;
using System.Web;
using System.Web.Optimization;

namespace TradeSuiteApp.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string AppName;

            AppName = ConfigurationManager.AppSettings["AppName"];

            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/Js/SignalR").Include(
                   "~/Assets/scripts/SignalR/jquery.signalR-2.4.1.js",
                    "~/Assets/scripts/SignalR/SignalRClient.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Login").Include(
                    "~/Assets/scripts/common.min.js",
                    "~/Assets/scripts/uikit_custom.js",
                    "~/Assets/scripts/altair_admin_common.js",
                    "~/Assets/scripts/pages/Login.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Common").Include(
                     "~/Assets/scripts/common.min.js",
                     "~/Assets/scripts/uikit_custom.js",
                     "~/Assets/scripts/altair_admin_common.js",
                     "~/Assets/scripts/plugins/jquery.inputmask.bundle.min.js",
                     "~/Assets/scripts/app.js",
                     "~/Assets/scripts/Config.js",
                     "~/Assets/scripts/pages/hotkeys.js",
                     "~/Assets/scripts/pages/qrcode.min.js",
                     "~/Assets/scripts/pages/JsBarcode.all.min.js"
                     //"~/Assets/scripts/pages/JsBarcode.ean-upc.min.js"

                     ));

            bundles.Add(new ScriptBundle("~/Js/DataTable").Include(
                    "~/Assets/scripts/plugins/datatables/jquery.dataTables.js",
                    "~/Assets/scripts/plugins/datatables/dataTables.uikit.min.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Country").Include(
                    "~/Assets/scripts/pages/Masters/Country.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Contact").Include(
                    "~/Assets/scripts/pages/Masters/Contact.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/AgeingBucket").Include(
                    "~/Assets/scripts/pages/Masters/AgeingBucket.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/District").Include(
                    "~/Assets/scripts/pages/Masters/District.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/DiscountCategory").Include(
                   "~/Assets/scripts/pages/Masters/DiscountCategory.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/Places").Include(
                     "~/Assets/scripts/pages/Masters/Places.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Category").Include(
                     "~/Assets/scripts/pages/Masters/Category.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/EmployeeFreeMedicineCreditLimit").Include(
                     "~/Assets/scripts/pages/Masters/EmployeeFreeMedicineCreditLimit.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SchemeAllocation").Include(
                    "~/Assets/scripts/pages/Masters/SchemeAllocation.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/Treasury").Include(
                     "~/Assets/scripts/pages/Masters/Treasury.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/FSO").Include(
                     "~/Assets/scripts/pages/Masters/FSO.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/AccountGroup").Include(
                     "~/Assets/scripts/pages/Masters/AccountGroup.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/IncentiveCalculation").Include(
                    "~/Assets/scripts/pages/Masters/IncentiveCalculation.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/Supplier").Include(
                     "~/Assets/scripts/pages/Masters/Supplier.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/IncentiveTarget").Include(
                     "~/Assets/scripts/pages/Masters/IncentiveTarget.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SupplierAccountsCategory").Include(
                   "~/Assets/scripts/pages/Masters/SupplierAccountsCategory.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/SupplierCategory").Include(
                  "~/Assets/scripts/pages/Masters/SupplierCategory.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/Employee").Include(
                     "~/Assets/scripts/plugins/dropify.min.js",
                     "~/Assets/scripts/pages/Masters/Employee.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/Item").Include(
                    "~/Assets/scripts/pages/Masters/Item.js"
                    ));
            //New Item.js for Doctor's Clinic
            bundles.Add(new ScriptBundle("~/Js/Page/ItemV3").Include(
                   "~/Assets/scripts/pages/Masters/ItemV3.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/ItemV4").Include(
                   "~/Assets/scripts/plugins/dropify.min.js",
                   "~/Assets/scripts/pages/Masters/ItemV4.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/Driver").Include(
                 "~/Assets/scripts/pages/Masters/Driver.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/Fleet").Include(
                "~/Assets/scripts/pages/Masters/Fleet.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/GSTSubCategory").Include(
                     "~/Assets/scripts/pages/Masters/GSTSubCategory.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Project").Include(
                  "~/Assets/scripts/pages/Masters/Project.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/SLAMaster").Include(
                    "~/Assets/scripts/pages/Masters/SLAMaster.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/PriceList").Include(
                    "~/Assets/scripts/pages/Masters/PriceList.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/Customer").Include(
                     "~/Assets/scripts/pages/Masters/Customer.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Unit").Include(
                     "~/Assets/scripts/pages/Masters/Unit.js"
            ));
            bundles.Add(new ScriptBundle("~/Js/Page/SecondaryUnit").Include(
                    "~/Assets/scripts/pages/Masters/SecondaryUnit.js"
           ));
            bundles.Add(new ScriptBundle("~/Js/Page/LocalPurchaseInvoice").Include(
                    "~/Assets/scripts/pages/Purchase/LocalPurchaseInvoice.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/WareHouse").Include(
                     "~/Assets/scripts/pages/Masters/WareHouse.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/State").Include(
                     "~/Assets/scripts/pages/Masters/State.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Discount").Include(
                     "~/Assets/scripts/pages/Masters/Discount.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SalesForecast").Include(
                     "~/Assets/scripts/pages/SNOP/SalesForecast.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Department").Include(
                     "~/Assets/scripts/pages/Masters/Department.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/PaymentMode").Include(
                     "~/Assets/scripts/pages/Masters/PaymentMode.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/PaymentType").Include(
                     "~/Assets/scripts/pages/Masters/PaymentType.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/TDS").Include(
                     "~/Assets/scripts/pages/Masters/TDS.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Location").Include(
                    "~/Assets/scripts/pages/Masters/Location.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/InterCompany").Include(
                    "~/Assets/scripts/pages/Masters/InterCompany.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/GSTCategory").Include(
                  "~/Assets/scripts/pages/Masters/GSTCategory.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Designation").Include(
                "~/Assets/scripts/pages/Masters/Designation.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/LeaveType").Include(
                "~/Assets/scripts/pages/Masters/LeaveType.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Holidays").Include(
                "~/Assets/scripts/pages/Masters/Holidays.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/DACalculation").Include(
                "~/Assets/scripts/pages/Masters/DACalculation.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/QC").Include(
                     "~/Assets/scripts/pages/Purchase/QC.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/InterCompanyPurchaseInvoice").Include(
                   "~/Assets/scripts/pages/Purchase/InterCompanyPurchaseInvoice.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/QCProduction").Include(
                     "~/Assets/scripts/pages/Manufacturing/QC.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/JobWorkIssue").Include(
                     "~/Assets/scripts/pages/Manufacturing/JobWorkIssue.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/JobWorkReceipt").Include(
                     "~/Assets/scripts/pages/Manufacturing/JobWorkReceipt.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseOrder").Include(
                     "~/Assets/scripts/pages/Purchase/PurchaseOrder.js",
                     "~/Assets/scripts/pages/Approvals/Approvals.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseOrderV4").Include(
                     "~/Assets/scripts/pages/Purchase/PurchaseOrderV4.js",
                     "~/Assets/scripts/pages/Approvals/Approvals.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/ServicePurchaseOrder").Include(
                     "~/Assets/scripts/pages/Purchase/ServicePurchaseOrder.js",
                     "~/Assets/scripts/pages/Approvals/Approvals.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Approvals").Include(
                     "~/Assets/scripts/pages/Approvals/Approvals.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/MilkPurchase").Include(
                     "~/Assets/scripts/pages/Purchase/MilkPurchase.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseRequisition").Include(
                     "~/Assets/scripts/pages/Purchase/PurchaseRequisition.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/ServicePurchaseRequisition").Include(
                     "~/Assets/scripts/pages/Purchase/ServicePurchaseRequisition.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/GRN").Include(
                     "~/Assets/scripts/pages/Purchase/GRN.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/GRNV3").Include(
                      "~/Assets/scripts/pages/Purchase/GRN.js",
                     "~/Assets/scripts/pages/Purchase/GRNV3.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/GRNV4").Include(
                      "~/Assets/scripts/pages/Purchase/GRN.js",
                     "~/Assets/scripts/pages/Purchase/GRNV4.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/ServiceSRN").Include(
                     "~/Assets/scripts/pages/Purchase/ServiceSRN.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseInvoice").Include(
                     "~/Assets/scripts/pages/Purchase/PurchaseInvoice.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseInvoiceV3").Include(
                     "~/Assets/scripts/pages/Purchase/PurchaseInvoiceV3.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseInvoiceV4").Include(
                     "~/Assets/scripts/pages/Purchase/PurchaseInvoiceV4.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/IRG").Include(
                     "~/Assets/scripts/pages/Purchase/IRG.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/ServicePurchaseInvoice").Include(
                     "~/Assets/scripts/pages/Purchase/ServicePurchaseInvoice.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/AdvanceReturn").Include(
                     "~/Assets/scripts/pages/Accounts/AdvanceReturn.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/AdvancePayment").Include(
                     "~/Assets/scripts/pages/Accounts/AdvancePayment.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/PaymentVoucher").Include(
                     "~/Assets/scripts/pages/Accounts/PaymentVoucher.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/SLA").Include(
                     "~/Assets/scripts/pages/Accounts/SLA.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/BRS").Include(
                     "~/Assets/scripts/pages/Accounts/BRS.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/BRSV3").Include(
                     "~/Assets/scripts/pages/Accounts/BRSV3.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/BankExpenses").Include(
                     "~/Assets/scripts/pages/Accounts/BankExpenses.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/Accounts/ChequeStatus").Include(
                     "~/Assets/scripts/pages/Accounts/ChequeStatus.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/StockValue").Include(
                     "~/Assets/scripts/pages/Stock/StockValue.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockAdjustment").Include(
                    "~/Assets/scripts/pages/Stock/StockAdjustment.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockAdjustmentV3").Include(
                   "~/Assets/scripts/pages/Stock/StockAdjustmentV3.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/DamageEntry").Include(
                    "~/Assets/scripts/pages/Stock/DamageEntry.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/ServiceItemReceipt").Include(
                  "~/Assets/scripts/pages/Stock/ServiceItemReceipt.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/RateAdjustment").Include(
                 "~/Assets/scripts/pages/Stock/RateAdjustment.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/ServiceItemIssue").Include(
                 "~/Assets/scripts/pages/Stock/ServiceItemIssue.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/Journal").Include(
                     "~/Assets/scripts/pages/Accounts/Journal.js",
                     "~/Assets/scripts/pages/Accounts/Journal_" + AppName + ".js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/CustomerCreditNote").Include(
                     "~/Assets/scripts/pages/Accounts/CustomerCreditNote.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/CustomerDebitNote").Include(
                     "~/Assets/scripts/pages/Accounts/CustomerDebitNote.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SupplierCreditNote").Include(
                     "~/Assets/scripts/pages/Accounts/SupplierCreditNote.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SupplierDebitNote").Include(
                     "~/Assets/scripts/pages/Accounts/SupplierDebitNote.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/CreditNote").Include(
                     "~/Assets/scripts/pages/Accounts/CreditNote.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/DebitNote").Include(
                     "~/Assets/scripts/pages/Accounts/DebitNote.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/FundTransfer").Include(
                     "~/Assets/scripts/pages/Accounts/FundTransfer.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/CustomerReturnVoucher").Include(
                    "~/Assets/scripts/pages/Accounts/CustomerReturnVoucher.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/PaymentReturnVoucher").Include(
                   "~/Assets/scripts/pages/Accounts/PaymentReturnVoucher.js"
                   ));

            //Reports - start ----------------------------------------------------------//

            bundles.Add(new ScriptBundle("~/Js/Page/Purchase").Include(
                     "~/Assets/scripts/pages/Reports/Purchase.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/AccountsReport").Include(
                     "~/Assets/scripts/pages/Reports/AccountsReport.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/ManufacturingReport").Include(
                      "~/Assets/scripts/pages/Reports/ManufacturingReport.js",
                      "~/Assets/scripts/pages/Reports/ReportHelper.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/Sales").Include(
                      "~/Assets/scripts/pages/Reports/Sales.js",
                      "~/Assets/scripts/pages/Reports/ReportHelper.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/ChequeStatus").Include(
                      "~/Assets/scripts/pages/Reports/Sales/ChequeStatus.js",
                      "~/Assets/scripts/pages/Reports/ReportHelper.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockExpiryReport").Include(
                      "~/Assets/scripts/pages/Reports/Stock/StockExpiry.js",
                      "~/Assets/scripts/pages/Reports/ReportHelper.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/ShortTransferReport").Include(
                      "~/Assets/scripts/pages/Reports/Stock/ShortTransfer.js",
                      "~/Assets/scripts/pages/Reports/ReportHelper.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockValuation").Include(
                     "~/Assets/scripts/pages/Reports/Stock/StockValuation.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockLedger").Include(
                     "~/Assets/scripts/pages/Reports/Stock/StockLedger.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockTransferGSTReport").Include(
                      "~/Assets/scripts/pages/Reports/Stock/StockTransferGST.js",
                      "~/Assets/scripts/pages/Reports/ReportHelper.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/GST").Include(
                     "~/Assets/scripts/pages/Reports/GST.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SalesGST").Include(
                     "~/Assets/scripts/pages/Reports/Sales/GST.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Reports/PurchaseReturn").Include(
                     "~/Assets/scripts/pages/Reports/PurchaseReturn.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Stock").Include(
                  "~/Assets/scripts/pages/Reports/Stock.js",
                  "~/Assets/scripts/pages/Reports/ReportHelper.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/_Version2/Stock").Include(
                   "~/Assets/scripts/pages/Reports/_Version2/Stock.js",
                   "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/TransportPermitReport").Include(
                    "~/Assets/scripts/pages/Sales/TransportPermit.js",
                    "~/Assets/scripts/pages/Reports/ReportHelper.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/SalesInvoiceReport").Include(
                     "~/Assets/scripts/pages/Reports/Sales/SalesInvoice.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SalesOrderReport").Include(
                    "~/Assets/scripts/pages/Reports/Sales/SalesOrder.js",
                    "~/Assets/scripts/pages/Reports/ReportHelper.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockAdjustmentReport").Include(
                   "~/Assets/scripts/pages/Reports/Stock/StockAdjustment.js",
                   "~/Assets/scripts/pages/Reports/ReportHelper.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockAdjustmentPendingReport").Include(
                  "~/Assets/scripts/pages/Reports/Stock/StockAdjustmentPending.js",
                  "~/Assets/scripts/pages/Reports/ReportHelper.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockAdjustmentToBeScheduled").Include(
                  "~/Assets/scripts/pages/Reports/Stock/StockAdjustmentToBeScheduled.js",
                  "~/Assets/scripts/pages/Reports/ReportHelper.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/TDSReport").Include(
                   "~/Assets/scripts/pages/Reports/Accounts/TDS.js",
                   "~/Assets/scripts/pages/Reports/ReportHelper.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/MaterialPurificationReport").Include(
                   "~/Assets/scripts/pages/Reports/Manufacturing/MaterialPurification.js",
                   "~/Assets/scripts/pages/Reports/ReportHelper.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProductionOutputAnalysisReport").Include(
                   "~/Assets/scripts/pages/Reports/Manufacturing/ProductionOutputAnalysis.js",
                   "~/Assets/scripts/pages/Reports/ReportHelper.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/BatchwiseProductionPacking").Include(
                  "~/Assets/scripts/pages/Reports/Manufacturing/BatchwiseProductionPacking.js",
                  "~/Assets/scripts/pages/Reports/ReportHelper.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProductionTargetVSActual").Include(
                   "~/Assets/scripts/pages/Reports/Manufacturing/ProductionTargetVSActual.js",
                   "~/Assets/scripts/pages/Reports/ReportHelper.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProductionWorkInProgress").Include(
                  "~/Assets/scripts/pages/Reports/Manufacturing/ProductionWorkInProgress.js",
                  "~/Assets/scripts/pages/Reports/ReportHelper.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProductionDefinitionReport").Include(
                   "~/Assets/scripts/pages/Reports/Manufacturing/ProductionDefinition.js",
                   "~/Assets/scripts/pages/Reports/ReportHelper.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/IncentiveReport").Include(
                  "~/Assets/scripts/pages/Reports/Sales/IncentiveReport.js",
                  "~/Assets/scripts/pages/Reports/ReportHelper.js"
                  ));

            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseOrderStatusReport").Include(
                 "~/Assets/scripts/pages/Reports/Purchase/PurchaseOrderStatus.js",
                 "~/Assets/scripts/pages/Reports/ReportHelper.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Page/Costing").Include(
                     "~/Assets/scripts/pages/Reports/Stock/Costing.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockValueWithGST").Include(
                     "~/Assets/scripts/pages/Reports/Stock/StockValueWithGST.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/GeneralLedgerV3").Include(
                     "~/Assets/scripts/pages/Reports/Accounts/GeneralLedgerV3.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/TrialBalanceV3").Include(
                     "~/Assets/scripts/pages/Reports/Accounts/TrialBalanceV3.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/SalesGSTRReport").Include(
                     "~/Assets/scripts/pages/Reports/Sales/SalesGSTRReport.js",
                     "~/Assets/scripts/pages/Reports/ReportHelper.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/DailyConsultationBill").Include(
                    "~/Assets/scripts/pages/Reports/AHCMS/DailyConsultationBill.js",
                    "~/Assets/scripts/pages/Reports/ReportHelper.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseInvoiceReport").Include(
                 "~/Assets/scripts/pages/Reports/Purchase/PurchaseInvoice.js",
                 "~/Assets/scripts/pages/Reports/ReportHelper.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/GeneralReport").Include(
                 "~/Assets/scripts/pages/Reports/Sales/GeneralReport.js",
                 "~/Assets/scripts/pages/Reports/ReportHelper.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/DailyTotalCollection").Include(
           "~/Assets/scripts/pages/Reports/AHCMS/DailyTotalCollection.js",
           "~/Assets/scripts/pages/Reports/ReportHelper.js"
           ));
            //Reports - end --------------------------------------------------//


            bundles.Add(new ScriptBundle("~/Js/Page/SalesOrder").Include(
                      "~/Assets/scripts/pages/Sales/SalesOrder.js",
                      "~/Assets/scripts/pages/Sales/SalesOrderV2.js",
                      //"~/Assets/scripts/pages/Sales/SalesOrder_" + AppName + ".js",
                      "~/Assets/scripts/pages/Sales/Sales.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/SalesInquiry").Include(
                     "~/Assets/scripts/pages/Sales/SalesInquiry.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/ServiceSalesOrder").Include(
                      "~/Assets/scripts/pages/Sales/ServiceSalesOrder.js",
                      "~/Assets/scripts/pages/Sales/Sales.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/CounterSalesReturn").Include(
                   "~/Assets/scripts/pages/Sales/CounterSalesReturn.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/CounterSales").Include(
                     "~/Assets/scripts/pages/Sales/CounterSales.js",
                     "~/Assets/scripts/pages/Sales/CounterSales_" + AppName + ".js",
                     "~/Assets/scripts/pages/Sales/Sales.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/CounterSalesV4").Include(
                     "~/Assets/scripts/pages/Sales/CounterSales.js",
                     "~/Assets/scripts/pages/Sales/CounterSales_" + AppName + ".js",
                     "~/Assets/scripts/pages/Sales/CounterSalesV4.js",
                     "~/Assets/scripts/pages/Sales/Sales.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/CounterSalesV5").Include(
                    "~/Assets/scripts/pages/Sales/CounterSales.js",
                    "~/Assets/scripts/pages/Sales/CounterSales_" + AppName + ".js",
                    "~/Assets/scripts/pages/Sales/CounterSalesV5.js",
                    "~/Assets/scripts/pages/Sales/Sales.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/GatePass").Include(
                    "~/Assets/scripts/pages/Sales/GatePass.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/SalesInvoice").Include(
                      "~/Assets/scripts/pages/Sales/SalesInvoice.js",
                      "~/Assets/scripts/pages/Sales/SalesInvoiceV2.js",
                      "~/Assets/scripts/pages/Sales/SalesInvoice_" + AppName + ".js",
                       "~/Assets/scripts/pages/Sales/Sales.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProformaInvoice").Include(
                      "~/Assets/scripts/pages/Sales/ProformaInvoice.js",
                      "~/Assets/scripts/pages/Sales/ProformaInvoice_" + AppName + ".js",
                      "~/Assets/scripts/pages/Approvals/Approvals.js",
                       "~/Assets/scripts/pages/Sales/Sales.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/GoodsReceipt").Include(
                    "~/Assets/scripts/pages/Sales/GoodsReceipt.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/SalesReturn").Include(
                      "~/Assets/scripts/pages/Sales/SalesReturn.js"
                      ));


            bundles.Add(new ScriptBundle("~/Js/Page/SalesReturn").Include(
                       "~/Assets/scripts/pages/Sales/SalesReturn.js"
                       ));
            bundles.Add(new ScriptBundle("~/Js/Page/Doctor").Include(
                    "~/Assets/scripts/plugins/dropify.min.js",
                   "~/Assets/scripts/pages/Masters/Employee.js",
                   "~/Assets/scripts/pages/Masters/Doctor.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/Patient").Include(
                  "~/Assets/scripts/pages/Masters/Patient.js"
                  ));


            bundles.Add(new ScriptBundle("~/Js/Page/AdvanceRequest").Include(
                      "~/Assets/scripts/pages/Accounts/AdvanceRequest.js",
                      "~/Assets/scripts/pages/Approvals/Approvals.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/AdvanceReturn").Include(
                      "~/Assets/scripts/pages/Accounts/AdvanceReturn.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/Repacking").Include(
                      "~/Assets/scripts/pages/Manufacturing/Repacking.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/ProductionPackingSchedule").Include(
                      "~/Assets/scripts/pages/Manufacturing/ProductionPackingSchedule.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/ProductionSchedule").Include(
                       "~/Assets/scripts/pages/Manufacturing/ProductionSchedule.js",
                      "~/Assets/scripts/pages/Manufacturing/ProductionSchedule_" + AppName + ".js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/StockRequest").Include(
                      "~/Assets/scripts/pages/Stock/StockRequest.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockIssue").Include(
                      "~/Assets/scripts/pages/Stock/StockIssue.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockReceipt").Include(
                      "~/Assets/scripts/pages/Stock/StockReceipt.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/ProductionIssue").Include(
                      "~/Assets/scripts/pages/Manufacturing/ProductionIssue.js",
                      "~/Assets/scripts/pages/Manufacturing/ProductionIssue_" + AppName + ".js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/ReceiptVoucherV3").Include(
                      "~/Assets/scripts/pages/Accounts/ReceiptVoucher_" + AppName + ".js",
                      "~/Assets/scripts/pages/Accounts/AccountHeadAdd.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/ReceiptVoucher").Include(
                      "~/Assets/scripts/pages/Accounts/ReceiptVoucher.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/PreProcessIssue").Include(
                      "~/Assets/scripts/pages/Manufacturing/PreProcessIssue.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProductionPacking").Include(
                      "~/Assets/scripts/pages/Manufacturing/ProductionPacking.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/PreProcessReceipt").Include(
                      "~/Assets/scripts/pages/Manufacturing/PreProcessReceipt.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/Asset").Include(
                      "~/Assets/scripts/pages/Asset/Asset.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Correction").Include(
                    "~/Assets/scripts/pages/Asset/Correction.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Batch").Include(
                   "~/Assets/scripts/pages/Asset/Batch.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/Depreciation").Include(
                      "~/Assets/scripts/pages/Asset/Depreciation.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/Retirement").Include(
                    "~/Assets/scripts/pages/Asset/Retirement.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/Attendance").Include(
                    "~/Assets/scripts/plugins/fullcalendar.js",
                    "~/Assets/scripts/pages/Payroll/Attendance.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/LeaveApplication").Include(
                   "~/Assets/scripts/pages/Payroll/LeaveApplication.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/Payroll").Include(
                  "~/Assets/scripts/pages/Payroll/Payroll.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Dashboard").Include(
                "~/Assets/scripts/plugins/d3.min.js",
                "~/Assets/scripts/plugins/metricsgraphics.js",
                "~/Assets/scripts/plugins/chartist.js",
                "~/Assets/scripts/plugins/chartist-plugin-tooltip.js",
                "~/Assets/scripts/plugins/jquery.peity.min.js",
                "~/Assets/scripts/plugins/jquery.easypiechart.min.js",
                "~/Assets/scripts/plugins/countUp.min.js",
                "~/Assets/scripts/plugins/handlebars.min.js",
                "~/Assets/scripts/plugins/handlebars_helpers.min.js",
                "~/Assets/scripts/plugins/clndr.js",
                "~/Assets/scripts/pages/dashboard/dashboard.js"
                      ));

            bundles.Add(new StyleBundle("~/Css/Common").Include(
                      "~/Assets/css/uikit/uikit.almost-flat.css",
                      "~/Assets/css/main.css",
                      "~/Assets/css/theme_" + AppName + ".css",
                      "~/Assets/css/additional_" + AppName + ".css",
                      "~/Assets/css/custom.css",
                       "~/Assets/css/style_switcher.css",
                       "~/Assets/css/style_switcher_horizontal.css",
                       "~/Assets/css/style_switcher_batch.css"
                      ));

            bundles.Add(new StyleBundle("~/Css/Dashboard").Include(
                      "~/Assets/css/weather-icons.min.css",
                      "~/Assets/css/metricsgraphics.css",
                      "~/Assets/css/chartist.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Css/Employee").Include(
                     "~/Assets/css/dropify.css",
                      "~/Assets/css/fullcalendar.css"
                     ));
            bundles.Add(new StyleBundle("~/Css/Item").Include(
                   "~/Assets/css/dropify.css"
                   ));

            ////Voucher
            bundles.Add(new ScriptBundle("~/Js/Page/Voucher").Include(
                     "~/Assets/scripts/pages/Accounts/PaymentVoucher.js",
                     "~/Assets/scripts/pages/Accounts/PaymentVoucher_" + AppName + ".js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/VoucherV3").Include(
                     "~/Assets/scripts/pages/Accounts/PaymentVoucher.js",
                     "~/Assets/scripts/pages/Accounts/PaymentVoucher_" + AppName + ".js",
                     "~/Assets/scripts/pages/Accounts/PaymentVoucherV3.js",
                     "~/Assets/scripts/pages/Accounts/AccountHeadAdd.js"
                      ));


            bundles.Add(new ScriptBundle("~/Js/Plugins/JQueryUI").Include(
                      "~/Assets/scripts/plugins/Accounts/jquery-ui.min.js"
                      ));

            /////Purchase Return
            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseReturn").Include(
                      "~/Assets/scripts/pages/Purchase/PurchaseReturn.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseReturnProcessing").Include(
                      "~/Assets/scripts/pages/Purchase/PurchaseReturnProcessing.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/PurchaseReturnOrder").Include(
                      "~/Assets/scripts/pages/Purchase/PurchaseReturnOrder.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/ManageAccessPermission").Include(
                      "~/Assets/scripts/pages/dashboard/MangeAccessPermission.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/OpeningStock").Include(
                      "~/Assets/scripts/pages/Stock/OpeningStock.js"
                      ));

            bundles.Add(new ScriptBundle("~/Js/Page/QCTest").Include(
                    "~/Assets/scripts/pages/Masters/QCTest.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/QCTestDefinition").Include(
                  "~/Assets/scripts/pages/Masters/QCTestDefinition.js"
                  ));


            bundles.Add(new ScriptBundle("~/Js/Page/AdvanceReceipt").Include(
                   "~/Assets/scripts/pages/Accounts/AdvanceReceipt.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/Discount").Include(
                  "~/Assets/scripts/pages/Masters/Discount.js"
                  ));


            bundles.Add(new ScriptBundle("~/Js/Page/PriceList").Include(
                "~/Assets/scripts/pages/Masters/PriceList.js"
                ));

            bundles.Add(new ScriptBundle("~/Js/Page/Role").Include(
                "~/Assets/scripts/pages/Masters/Role.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/UserRole").Include(
               "~/Assets/scripts/pages/Masters/UserRole.js"
                    ));
            bundles.Add(new ScriptBundle("~/Js/Page/Mould").Include(
                 "~/Assets/scripts/pages/Masters/Mould.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProductionProcess").Include(
                     "~/Assets/scripts/pages/Manufacturing/ProductionProcess.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/MouldSettings").Include(
                     "~/Assets/scripts/pages/Masters/MouldSettings.js"
                     ));


            bundles.Add(new ScriptBundle("~/Js/Page/PowerConsumption").Include(
              "~/Assets/scripts/pages/Masters/PowerConsumption.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/Process").Include(
            "~/Assets/scripts/pages/Masters/Process.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Page/MaterialPurification").Include(
                 "~/Assets/scripts/pages/Masters/MaterialPurification.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Page/ProductionDefinition").Include(
                 "~/Assets/scripts/pages/Masters/ProductionDefinition.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Page/App").Include(
                "~/Assets/scripts/pages/Admin/App.js"
                ));

            bundles.Add(new ScriptBundle("~/Js/Page/Machine").Include(
               "~/Assets/scripts/pages/Masters/Machine.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/Categories").Include(
               "~/Assets/scripts/pages/Masters/Categories.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/ServiceSalesInvoice").Include(
              "~/Assets/scripts/pages/Sales/ServiceSalesInvoice.js",
              "~/Assets/scripts/pages/Sales/Sales.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/SerialNumber").Include(
               "~/Assets/scripts/pages/Masters/SerialNumber.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/CreditDays").Include(
                 "~/Assets/scripts/pages/Masters/CreditDays.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Page/PaymentDays").Include(
                 "~/Assets/scripts/pages/Masters/PaymentDays.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Page/TurnOverDiscount").Include(
                "~/Assets/scripts/pages/Masters/TurnOverDiscount.js"
                ));

            bundles.Add(new ScriptBundle("~/Js/Page/ApprovalFlow").Include(
                "~/Assets/scripts/pages/Masters/ApprovalFlow.js"
                ));

            bundles.Add(new ScriptBundle("~/Js/Page/ApprovalQueue").Include(
                "~/Assets/scripts/pages/Masters/ApprovalQueue.js"
                ));

            bundles.Add(new ScriptBundle("~/Js/Page/LogicCode").Include(
               "~/Assets/scripts/pages/Masters/LogicCode.js"
               ));

            bundles.Add(new ScriptBundle("~/Js/Page/StockAdjustmentReasons").Include(
               "~/Assets/scripts/pages/Masters/StockAdjustmentReasons.js"
               ));

            bundles.Add(new ScriptBundle("~/Js/Page/Batch").Include(
               "~/Assets/scripts/pages/Masters/Batch.js"
               ));

            bundles.Add(new ScriptBundle("~/Js/Page/PaymentGroup").Include(
              "~/Assets/scripts/pages/Masters/PaymentGroup.js"
              ));

            bundles.Add(new ScriptBundle("~/Js/Page/PeriodClosing").Include(
             "~/Assets/scripts/pages/Masters/PeriodClosing.js"
             ));

            bundles.Add(new ScriptBundle("~/Js/Page/UserLocations").Include(
           "~/Assets/scripts/pages/Masters/UserLocations.js"
           ));

            bundles.Add(new ScriptBundle("~/Js/Page/ChartOfAccounts").Include(
         "~/Assets/scripts/pages/Masters/ChartOfAccounts.js"
         ));
            bundles.Add(new ScriptBundle("~/Js/Page/SalesRepresentative").Include(
        "~/Assets/scripts/pages/Masters/SalesRepresentative.js"
        ));

            bundles.Add(new ScriptBundle("~/Js/Page/FundTransferReceipt").Include(
                     "~/Assets/scripts/pages/Accounts/FundTransferReceipt.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/SalesBudget").Include(
                 "~/Assets/scripts/pages/Masters/SalesBudget.js"
                 ));



            bundles.Add(new ScriptBundle("~/Js/Page/OutPatient").Include(
                   "~/Assets/scripts/pages/Masters/OutPatient.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/MaterialRequirementPlan").Include(
                   "~/Assets/scripts/pages/Stock/MaterialRequirementPlan.js"
                   ));




            bundles.Add(new ScriptBundle("~/Js/Page/PrintList").Include(
                   "~/Assets/scripts/pages/PrintList.js"
                   ));



            bundles.Add(new ScriptBundle("~/Js/Page/AppointmentSchedule").Include(
                      "~/Assets/scripts/pages/AHCMS/AppointmentSchedule.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/AppointmentScheduleV2").Include(
                     "~/Assets/scripts/pages/AHCMS/AppointmentSchedule.js",
                     "~/Assets/scripts/pages/AHCMS/AppointmentScheduleV2.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/AppointmentScheduleV3").Include(
                     "~/Assets/scripts/pages/AHCMS/AppointmentSchedule.js",
                     "~/Assets/scripts/pages/AHCMS/AppointmentScheduleV3.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/MedicineConsumption").Include(
                      "~/Assets/scripts/pages/AHCMS/MedicineConsumption.js"
                      ));
            bundles.Add(new ScriptBundle("~/Js/Page/InternationalPatient").Include(
                     "~/Assets/scripts/pages/Masters/InternationalPatient.js"
                     ));

            bundles.Add(new ScriptBundle("~/Js/Page/Diagnosis").Include(
                    "~/Assets/scripts/pages/Masters/Diagnosis.js"
                    ));

            bundles.Add(new ScriptBundle("~/Js/Page/TreatmentRoom").Include(
                   "~/Assets/scripts/pages/Masters/TreatmentRoom.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/TreatmentRoom").Include(
                   "~/Assets/scripts/pages/Masters/TreatmentRoom.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/PatientDiagnosis").Include(
                 "~/Assets/scripts/pages/AHCMS/PatientDiagnosis.js"
                 ));

            //for Ilaj
            bundles.Add(new ScriptBundle("~/Js/Page/PatientDiagnosisV2").Include(
                "~/Assets/scripts/pages/AHCMS/PatientDiagnosis.js",
                "~/Assets/scripts/pages/AHCMS/PatientDiagnosisV2.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/PatientDiagnosisV5").Include(
                "~/Assets/scripts/pages/AHCMS/PatientDiagnosis.js",
                "~/Assets/scripts/pages/AHCMS/PatientDiagnosisV2.js",
                "~/Assets/scripts/pages/AHCMS/PatientDiagnosisV5.js"
                ));


            bundles.Add(new ScriptBundle("~/Js/Page/MilkRate").Include(
                   "~/Assets/scripts/pages/Masters/MilkRate.js"
                   ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockConsumption").Include(
                  "~/Assets/scripts/pages/Stock/StockConsumption.js"
                  ));


            bundles.Add(new ScriptBundle("~/Js/Page/Treatment").Include(
                           "~/Assets/scripts/pages/Masters/Treatment.js"
                           ));
            bundles.Add(new ScriptBundle("~/Js/Page/PrescriptionFormat").Include(
               "~/Assets/scripts/pages/Masters/PrescriptionFormat.js"
               ));
            bundles.Add(new ScriptBundle("~/Js/Page/TreatmentSchedule").Include(
               "~/Assets/scripts/pages/AHCMS/TreatmentSchedule.js"
               ));
            bundles.Add(new ScriptBundle("~/Js/Page/TreatmentProcess").Include(
              "~/Assets/scripts/pages/AHCMS/TreatmentProcess.js"
              ));
            bundles.Add(new ScriptBundle("~/Js/Page/DirectPurchaseInvoice").Include(
                      "~/Assets/scripts/pages/Purchase/DirectPurchaseInvoice.js",
                      "~/Assets/scripts/pages/Purchase/DirectPurchaseInvoice V1.js"
                          ));

            bundles.Add(new ScriptBundle("~/Js/Page/ReOrder").Include(
           "~/Assets/scripts/pages/Purchase/ReOrder.js"
               ));

            bundles.Add(new ScriptBundle("~/Js/Page/Room").Include(
                     "~/Assets/scripts/pages/Masters/Room.js"
                     ));
            bundles.Add(new ScriptBundle("~/Js/Page/IP").Include(
                  "~/Assets/scripts/pages/AHCMS/IP.js"
                  ));

            bundles.Add(new ScriptBundle("~/Js/Page/IPCaseSheet").Include(
                 "~/Assets/scripts/pages/AHCMS/IPCaseSheet.js"
                 ));

            bundles.Add(new ScriptBundle("~/Js/Page/RoomReservation").Include(
                  "~/Assets/scripts/pages/AHCMS/RoomReservation.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/RoomAllocation").Include(
                 "~/Assets/scripts/pages/AHCMS/RoomAllocation.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/OPRegister").Include(
                  "~/Assets/scripts/pages/Reports/AHCMS/OPRegister.js",
                  "~/Assets/scripts/pages/Reports/ReportHelper.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/TreatmentReport").Include(
                 "~/Assets/scripts/pages/Reports/AHCMS/TreatmentReport.js",
                 "~/Assets/scripts/pages/Reports/ReportHelper.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/MedicineSchedule").Include(
                "~/Assets/scripts/pages/Reports/AHCMS/MedicineSchedule.js",
                "~/Assets/scripts/pages/Reports/ReportHelper.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/TransferQuantity").Include(
               "~/Assets/scripts/pages/Reports/AHCMS/TransferQuantity.js",
               "~/Assets/scripts/pages/Reports/ReportHelper.js"
               ));
            bundles.Add(new ScriptBundle("~/Js/Page/RoomChange").Include(
                "~/Assets/scripts/pages/AHCMS/RoomChange.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/IPRegister").Include(
                 "~/Assets/scripts/pages/Reports/AHCMS/IPRegister.js",
                 "~/Assets/scripts/pages/Reports/ReportHelper.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/LabTest").Include(
             "~/Assets/scripts/pages/AHCMS/LabTest.js"
             ));
            bundles.Add(new ScriptBundle("~/Js/Page/BillType").Include(
            "~/Assets/scripts/pages/AHCMS/BillType.js"
            ));
            bundles.Add(new ScriptBundle("~/Js/Page/Discharge").Include(
             "~/Assets/scripts/pages/AHCMS/Discharge.js"
             ));
            bundles.Add(new ScriptBundle("~/Js/Page/Xray").Include(
           "~/Assets/scripts/pages/AHCMS/Xray.js"
           ));
            bundles.Add(new ScriptBundle("~/Js/Page/LaboratoryTest").Include(
                   "~/Assets/scripts/pages/Masters/LaboratoryTest.js"
                   ));

            bundles.Add(new ScriptBundle("~/Js/Page/XrayTest").Include(
                  "~/Assets/scripts/pages/Masters/XrayTest.js"
                  ));
            bundles.Add(new ScriptBundle("~/Js/Page/ServiceItem").Include(
                 "~/Assets/scripts/pages/Masters/ServiceItem.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/Physiotherapy").Include(
                "~/Assets/scripts/pages/Masters/Physiotherapy.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/Manufacturer").Include(
                "~/Assets/scripts/pages/Masters/Manufacturer.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/ConsultationSchedule").Include(
               "~/Assets/scripts/pages/Masters/ConsultationSchedule.js"
               ));
            bundles.Add(new ScriptBundle("~/Js/Page/GenericName").Include(
               "~/Assets/scripts/pages/Masters/GenericName.js"
               ));
            bundles.Add(new ScriptBundle("~/Js/Page/StockAdjustmentSchedule").Include(
              "~/Assets/scripts/pages/Masters/StockAdjustmentSchedule.js"
              ));
            bundles.Add(new ScriptBundle("~/Js/Page/LaboratoryInvoice").Include(
                 "~/Assets/scripts/pages/AHCMS/LaboratoryInvoice.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/LaboratoryTestResult").Include(
                "~/Assets/scripts/pages/AHCMS/LaboratoryTestResult.js"
                 ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProfitabilityReport").Include(
                "~/Assets/scripts/pages/Reports/Sales/ProfitabilityReport.js",
                "~/Assets/scripts/pages/Reports/ReportHelper.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/FastMovingItems").Include(
               "~/Assets/scripts/pages/Reports/Sales/FastMovingItems.js",
               "~/Assets/scripts/pages/Reports/ReportHelper.js"
               ));
            bundles.Add(new ScriptBundle("~/Js/Page/EmployeeDailyReport").Include(
               "~/Assets/scripts/pages/Reports/Sales/EmployeeDailyReport.js",
               "~/Assets/scripts/pages/Reports/ReportHelper.js"
              ));
            bundles.Add(new ScriptBundle("~/Js/Page/ProfitabilityCalculation").Include(
              "~/Assets/scripts/pages/Reports/Stock/ProfitabilityCalculation.js",
              "~/Assets/scripts/pages/Reports/ReportHelper.js"
              ));
            bundles.Add(new ScriptBundle("~/Js/Page/CostingAndProfitability").Include(
              "~/Assets/scripts/pages/Reports/Purchase/CostingAndProfitability.js",
              "~/Assets/scripts/pages/Reports/ReportHelper.js"
              ));
            bundles.Add(new ScriptBundle("~/Js/Page/AccountHead").Include(
                "~/Assets/scripts/pages/Masters/AccountHead.js"
                ));
            bundles.Add(new ScriptBundle("~/Js/Page/ExpiringAndExpiredItems").Include(
              "~/Assets/scripts/pages/Reports/Stock/ExpiringAndExpiredItems.js",
              "~/Assets/scripts/pages/Reports/ReportHelper.js"
              ));
            bundles.Add(new ScriptBundle("~/Js/Page/Currency").Include(
              "~/Assets/scripts/pages/Masters/Currency.js"
               ));
            bundles.Add(new ScriptBundle("~/Js/Page/Brand").Include(
            "~/Assets/scripts/pages/Masters/Brand.js"
             ));
            bundles.Add(new ScriptBundle("~/Js/Page/Bin").Include(
           "~/Assets/scripts/pages/Masters/Bin.js"
            ));
            bundles.Add(new ScriptBundle("~/Js/Page/CurrencyConversion").Include(
            "~/Assets/scripts/pages/Masters/CurrencyConversion.js"
              ));
            bundles.Add(new ScriptBundle("~/Js/Page/taxtype").Include(
          "~/Assets/scripts/pages/Masters/taxtype.js"
            ));

        }
    }
}
